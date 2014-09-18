using System;
using System.Collections;
using System.Windows.Forms;
using System.Threading;

using Duality.Editor.Properties;

namespace Duality.Editor.Forms
{
	public partial class ProcessingBigTaskDialog : Form
	{
		public delegate IEnumerable TaskAction(WorkerInterface worker);
		public class WorkerInterface
		{
			internal	ProcessingBigTaskDialog	parent	= null;
			private	Exception	error		= null;
			private	Form		targetForm	= null;
			private	TaskAction	task		= null;
			private float		progress	= 0.0f;
			private string		stateDesc	= null;
			private	object		data		= null;
			private	string		errorDesc	= null;

			public float Progress
			{
				get { return this.progress; }
				set { this.progress = value; }
			}
			public string StateDesc
			{
				get { return this.stateDesc; }
				set { this.stateDesc = value; }
			}
			public Exception Error
			{
				get { return this.error; }
				set
				{
					if (this.error == null && value != null)
					{
						this.errorDesc = string.Format(GeneralRes.Msg_ErrorPerformBigTask_Desc, this.stateDesc, "\n", Log.Exception(value));
					}
					this.error = value;
				}
			}
			public string ErrorDesc
			{
				get { return this.errorDesc; }
				set { this.errorDesc = value; }
			}
			public Form TargetForm
			{
				get { return this.targetForm; }
				set { this.targetForm = value; }
			}
			public TaskAction Task
			{
				get { return this.task; }
				set { this.task = value; }
			}
			public object Data
			{
				get { return this.data; }
				set { this.data = value; }
			}
		}


		private	Thread			worker				= null;
		private	WorkerInterface	workerInterface		= null;
		private	Form			targetForm			= null;
		private	TaskAction		task				= null;
		private	object			data				= null;
		private	string			taskCaption			= "Performing Task...";
		private	string			taskDesc			= "A task is being performed. Please wait.";
		private	bool			mainThreadRequired	= true;

		public bool MainThreadRequired
		{
			get { return this.mainThreadRequired; }
			set
			{
				if (this.worker != null) throw new InvalidOperationException("The MainThreadRequired property may only change during initialization of the TaskDialog.");
				this.mainThreadRequired = value;
			}
		}


		public ProcessingBigTaskDialog(string caption, string desc, TaskAction task, object data) : this(null, caption, desc, task, data) {}
		public ProcessingBigTaskDialog(MainForm owner, string caption, string desc, TaskAction task, object data)
		{
			this.InitializeComponent();

			if (owner == null) this.StartPosition = FormStartPosition.CenterScreen;

			this.targetForm = (Form)owner ?? this;
			this.taskCaption = caption;
			this.taskDesc = desc;
			this.task = task;
			this.data = data;

			this.Text = this.taskCaption;
			this.descLabel.Text = this.taskDesc;
			this.stateDescLabel.Text = "";
		}

		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);

			this.targetForm.SetTaskbarOverlayIcon(GeneralResCache.IconCog, this.taskCaption);

			this.workerInterface = new WorkerInterface();
			this.workerInterface.parent = this;
			this.workerInterface.TargetForm = this.targetForm;
			this.workerInterface.Task = this.task;
			this.workerInterface.Data = this.data;

			this.worker = new Thread(WorkerThreadProc);
			this.worker.Start(this.workerInterface);

			this.progressTimer.Start();
		}
		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);

			this.targetForm.SetTaskbarProgress(0.0f);
			this.targetForm.SetTaskbarProgressState(ThumbnailProgressState.NoProgress);
			this.targetForm.SetTaskbarOverlayIcon(null, null);
		}

		private void progressTimer_Tick(object sender, EventArgs e)
		{
			this.stateDescLabel.Text = this.workerInterface.StateDesc;
			this.progressBar.Style = this.workerInterface.Progress < 0.0f ? ProgressBarStyle.Marquee : ProgressBarStyle.Continuous;
			this.progressBar.Value = (int)Math.Round(Math.Max(Math.Min(this.workerInterface.Progress, 1.0f), 0.0f) * 100.0f);
			this.targetForm.SetTaskbarProgressState(ThumbnailProgressState.Normal);
			this.targetForm.SetTaskbarProgress(this.progressBar.Value);

			if (this.workerInterface.Error != null)
			{
				this.progressTimer.Stop();

				this.targetForm.SetTaskbarProgressState(ThumbnailProgressState.Error);
				if (!string.IsNullOrEmpty(this.workerInterface.ErrorDesc))
				{
					MessageBox.Show(this, 
						this.workerInterface.ErrorDesc, 
						GeneralRes.Msg_ErrorPerformBigTask_Caption, 
						MessageBoxButtons.OK, MessageBoxIcon.Error);
				}

				this.DialogResult = DialogResult.Abort;
				this.Close();
			}
			else if (!this.worker.IsAlive)
			{
				this.progressTimer.Stop();
				this.DialogResult = DialogResult.OK;
				this.Close();
			}
		}
		
		private static void WorkerThreadProc(object args)
		{
			WorkerInterface workInterface = args as WorkerInterface;
			
			workInterface.Error = null;
			workInterface.Progress = 0.0f;
			try 
			{
				// All work is performed here.
				if (!workInterface.parent.MainThreadRequired)
				{
					var taskEnumerator = workInterface.Task(workInterface).GetEnumerator();
					while (taskEnumerator.MoveNext()) {}
				}
				// All work is actually performed in the main thread.
				else
				{
					// Wait a little so the main thread has time for drawing the GUI.
					Thread.Sleep(20);

					// Perform task on the main thread - which is necessary because OpenGL and friends don't like multithreading.
					// In order to keep the GUI updated, the task is split into chunks. After each chunk, GUI events can be processed.
					var taskEnumerator = workInterface.Task(workInterface).GetEnumerator();
					DateTime lastCheck = DateTime.Now;
					while ((bool)workInterface.TargetForm.Invoke((Func<bool>)taskEnumerator.MoveNext))
					{
						TimeSpan lastTime = DateTime.Now - lastCheck;
						// Wait a little so the main thread has time for drawing the GUI.
						if (lastTime.TotalMilliseconds > 1000.0f)
						{
							Thread.Sleep(20);
							lastCheck = DateTime.Now;
						}
					}
				}

				// Assume the task finished completely
				workInterface.Progress = 1.0f;
			}
			catch (Exception e)
			{
				Log.Editor.WriteError("Failed to perform task: {0}", Log.Exception(e));
				workInterface.Error = e;
			}
		}
	}
}
