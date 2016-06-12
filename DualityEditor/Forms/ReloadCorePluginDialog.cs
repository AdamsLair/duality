using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using System.Reflection;

using Duality;
using Duality.Editor.Properties;
using Duality.Resources;

namespace Duality.Editor.Forms
{
	public sealed partial class ReloadCorePluginDialog : Form
	{
		private class WorkerInterface
		{
			private	bool			finished		= false;
			private	float			progress		= 0.0f;
			private	Exception		error			= null;
			private	List<string>	reloadSchedule	= null;
			private	List<string>	reloadDone		= null;
			private	bool			recoverMode		= false;
			private	bool			shutdown		= false;
			private	Scene			tempScene		= null;
			private	MainForm		mainForm		= null;

			public bool Finished
			{
				get { return this.finished; }
				set { this.finished = value; }
			}
			public float Progress
			{
				get { return this.progress; }
				set { this.progress = value; }
			}
			public Exception Error
			{
				get { return this.error; }
				set { this.error = value; }
			}
			public List<string> ReloadSchedule
			{
				get { return this.reloadSchedule; }
				set { this.reloadSchedule = value; }
			}
			public List<string> ReloadDone
			{
				get { return this.reloadDone; }
				set { this.reloadDone = value; }
			}
			public bool RecoverMode
			{
				get { return this.recoverMode; }
				set { this.recoverMode = value; }
			}
			public bool Shutdown
			{
				get { return this.shutdown; }
				set { this.shutdown = value; }
			}
			public Scene TempScene
			{
				get { return this.tempScene; }
				set { this.tempScene = value; }
			}
			public MainForm MainForm
			{
				get { return this.mainForm; }
				set { this.mainForm = value; }
			}
		}

		public enum ReloaderState
		{
			Idle,
			WaitForPlugins,
			ReloadPlugins,

			RecoverFromRestart
		}


		Thread			worker			= null;
		WorkerInterface	workerInterface	= null;
		MainForm		owner			= null;
		List<string>	reloadSchedule	= new List<string>();
		ReloaderState	state			= ReloaderState.Idle;
		int				waitTime		= 0;


		public	event	EventHandler	BeforeBeginReload	= null;
		public	event	EventHandler	AfterEndReload		= null;


		public List<string> ReloadSchedule
		{
			get { return this.reloadSchedule; }
		}
		public ReloaderState State
		{
			get { return this.state; }
			set
			{
				// Ignore other commands while reloading.
				if (this.state == ReloaderState.ReloadPlugins)
					return;
				if (value == ReloaderState.ReloadPlugins && this.Visible)
					return;
				if (value == ReloaderState.ReloadPlugins && !this.VerifyReloadSchedule())
					value = ReloaderState.Idle;

				this.state = value;
				if (this.state == ReloaderState.Idle)
				{
					this.progressTimer.Stop();
				}
				else if (this.state == ReloaderState.WaitForPlugins)
				{
					this.waitTime = 0;
					this.progressTimer.Start();
				}
				else if (this.state == ReloaderState.ReloadPlugins)
				{
					this.ShowDialog(this.owner);
				}
				else if (this.state == ReloaderState.RecoverFromRestart)
				{
					this.ShowDialog(this.owner);
				}
			}
		}
		public bool IsReloadingPlugins
		{
			get { return this.state != ReloaderState.Idle; }
		}


		public ReloadCorePluginDialog(MainForm owner)
		{
			this.InitializeComponent();
			this.owner = owner;
		}

		private void OnBeforeBeginReload()
		{
			if (this.BeforeBeginReload != null)
				this.BeforeBeginReload(this, null);
		}
		private void OnAfterEndReload()
		{
			if (this.AfterEndReload != null)
				this.AfterEndReload(this, null);
		}
		private bool VerifyReloadSchedule()
		{
			for (int i = this.reloadSchedule.Count - 1; i >= 0; i--)
			{
				string fullPath = Path.GetFullPath(this.reloadSchedule[i]);
				CorePlugin plugin = DualityApp.PluginManager.LoadedPlugins.FirstOrDefault(p => Path.GetFullPath(p.FilePath) == fullPath);
				if (plugin == null) continue;

				int hash = DualityApp.PluginManager.PluginLoader.GetAssemblyHash(this.reloadSchedule[i]);
				if (plugin.FileHash == hash)
				{
					this.reloadSchedule.RemoveAt(i);
				}
			}
			return this.reloadSchedule.Count > 0;
		}

		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);

			if (this.state != ReloaderState.RecoverFromRestart)
			{
				this.OnBeforeBeginReload();
				this.state = ReloaderState.ReloadPlugins;
			}
			//this.owner.MainContextControl.Context.MakeCurrent(null);

			this.progressTimer.Start();
			this.owner.SetTaskbarOverlayIcon(GeneralResCache.IconCog, GeneralRes.TaskBarOverlay_ReloadCorePlugin_Desc);

			this.workerInterface = new WorkerInterface();
			this.workerInterface.MainForm = this.owner;
			this.workerInterface.ReloadDone = new List<string>();
			if (this.state != ReloaderState.RecoverFromRestart)
				this.workerInterface.ReloadSchedule = new List<string>(this.reloadSchedule);
			else
				this.workerInterface.RecoverMode = true;

			this.worker = new Thread(WorkerThreadProc);
			this.worker.Start(this.workerInterface);

			this.state = ReloaderState.ReloadPlugins;
		}
		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);
			if (this.workerInterface.Shutdown) return;

			this.owner.SetTaskbarProgress(0.0f);
			this.owner.SetTaskbarProgressState(ThumbnailProgressState.NoProgress);
			this.owner.SetTaskbarOverlayIcon(null, null);
			if (this.workerInterface.ReloadDone != null)
			{
				foreach (string done in this.workerInterface.ReloadDone)
					this.reloadSchedule.Remove(done);
			}
			else
			{
				this.reloadSchedule.Clear();
			}

			//this.owner.MainContextControl.MakeCurrent();
			this.state = ReloaderState.Idle;
			this.OnAfterEndReload();

			// If some change messages were late, begin secondary reload.
			if (this.reloadSchedule.Count > 0)
				this.State = ReloaderState.WaitForPlugins;
		}

		private void progressTimer_Tick(object sender, EventArgs e)
		{
            if (this.state == ReloaderState.WaitForPlugins)
			{
				this.waitTime += this.progressTimer.Interval;
				if (this.waitTime > 1000)
                    this.State = ReloaderState.ReloadPlugins;
			}
			else if (this.state == ReloaderState.ReloadPlugins)
			{
				this.progressBar.Value = (int)Math.Round(this.workerInterface.Progress * 100.0f);
				this.owner.SetTaskbarProgressState(ThumbnailProgressState.Normal);
				this.owner.SetTaskbarProgress(this.progressBar.Value);

				if (this.workerInterface.Error != null)
				{
					this.progressTimer.Stop();
					try
					{
						this.owner.SetTaskbarProgressState(ThumbnailProgressState.Error);
						MessageBox.Show(this, 
							String.Format(GeneralRes.Msg_ErrorReloadCorePlugin_Desc, "\n", Log.Exception(this.workerInterface.Error)), 
							GeneralRes.Msg_ErrorReloadCorePlugin_Caption, 
							MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
					catch (Exception exception)
					{
						Log.Editor.WriteError("An error occurred after finishing a Core plugin reload operation: {0}", Log.Exception(exception));
					}
					this.Close();
				}
				else if (this.workerInterface.Finished)
				{
					this.progressTimer.Stop();
					try
					{
						// Re-Apply temporarily saved Scene
						Scene.SwitchTo(this.workerInterface.TempScene, true);
					}
					catch (Exception exception)
					{
						Log.Editor.WriteError("An error occurred after finishing a Core plugin reload operation: {0}", Log.Exception(exception));
					}
					this.Close();
				}
			}
		}
		
		private static void WorkerThreadProc(object args)
		{
			WorkerInterface workInterface = args as WorkerInterface;
			bool fullRestart = false;

			try
			{
				// Reload plugins the regular way.
				PerformPluginReload(workInterface, ref fullRestart);
			}
			catch (Exception e)
			{
				// If we failed for an unknown reason, let's try a full restart instead.
				if (!fullRestart)
				{
					Log.Editor.WriteError("Failed reloading plugins during runtime: {0}", Log.Exception(e));
					Log.Editor.Write("Trying full restart...");

					try
					{
						// Try again, with a forced full restart
						fullRestart = true;
						PerformPluginReload(workInterface, ref fullRestart);
					}
					catch (Exception e2)
					{
						// Failed anyway? Log the error and stop.
						Log.Editor.WriteError(Log.Exception(e2));
						workInterface.Error = e2;
					}
				}
				// If even a full restart has failed, log the error and stop right there.
				else
				{
					Log.Editor.WriteError(Log.Exception(e));
					workInterface.Error = e;
				}
			}
		}
		private static void PerformPluginReload(WorkerInterface workInterface, ref bool fullRestart)
		{
			Stream strScene;
			Stream strData;

			string tempDir = Path.Combine(Path.GetTempPath(), "Duality");
			string tempScenePath = Path.Combine(tempDir, "ReloadPluginBackup" + Resource.GetFileExtByType<Scene>());
			string tempDataPath = Path.Combine(tempDir, "ReloadPluginBackup.dat");
			if (!Directory.Exists(tempDir)) Directory.CreateDirectory(tempDir);

			if (!workInterface.RecoverMode)
			{
				// No full restart scheduled? Well, check if it should be!
				fullRestart = fullRestart || RequiresFullRestart(workInterface.ReloadSchedule);

				// If a full restart is required, store temp data in files.
				if (fullRestart)
				{
					strScene = File.Create(tempScenePath);
					strData = File.Create(tempDataPath);
				}
				// If not, just use memory.
				else
				{
					strScene = new MemoryStream(1024 * 1024 * 10);
					strData = new MemoryStream(512);
				}

				// Save temporary data to restore it later
				Log.Editor.Write("Saving data...");
				SaveTemporaryData(workInterface, strScene, strData);
				Thread.Sleep(20);
			
				// If required, perform a full editor system restart
				if (fullRestart)
				{
					strScene.Close();
					strData.Close();
					PerformFullRestart(workInterface);
					return;
				}
				// Reload things at runtime, if possible
				else
				{
					PerformRuntimeReload(workInterface);
					strScene.Seek(0, SeekOrigin.Begin);
					strData.Seek(0, SeekOrigin.Begin);
				}
			}
			else
			{
				strScene = File.OpenRead(tempScenePath);
				strData = File.OpenRead(tempDataPath);
				workInterface.Progress = 0.6f;
			}

			// Reload data
			Log.Editor.Write("Restoring data...");
			RestoreTemporaryData(workInterface, strScene, strData);
			strScene.Close();
			strData.Close();

			workInterface.Progress = 1.0f;
			workInterface.Finished = true;
		}
		private static bool RequiresFullRestart(IEnumerable<string> reloadPluginPaths)
		{
			Assembly[] allPluginAssemblies = 
				DualityApp.PluginManager.LoadedPlugins.Select(p => p.PluginAssembly).Concat(
				DualityEditorApp.Plugins.Select(p => p.PluginAssembly)).ToArray();

			// If there is any editor plugin to be reloaded, we need a full restart.
			if (reloadPluginPaths.Any(asmFile => asmFile.EndsWith(".editor.dll", StringComparison.InvariantCultureIgnoreCase)))
			{
				return true;
			}
			// If any plugin dependency needs to be reloaded, do a full restart. Due to the way, bindings between
			// different assemblies are resolved, it is impossible to prevent any plugin from still binding the old
			// version of the dependency in question. Any source code referring to the disposed dependency may
			// result in undefined behavior. A full restart is necessary.
			else if (reloadPluginPaths.Any(asmFile => IsDependencyPlugin(asmFile, allPluginAssemblies)))
			{
				return true;
			}

			return false;
		}
		private static void PerformFullRestart(WorkerInterface workInterface)
		{
			bool debug = System.Diagnostics.Debugger.IsAttached;

			// Close old form and wait for it to be closed
			workInterface.Shutdown = true;
			workInterface.MainForm.Invoke(new CloseMainFormDelegate(CloseMainForm), workInterface.MainForm);
			while (workInterface.MainForm.Visible)
			{
				Thread.Sleep(20);
			}

			Process newEditor = Process.Start(Application.ExecutablePath, "recover" + (debug ? " debug" : ""));
		}
		private static void PerformRuntimeReload(WorkerInterface workInterface)
		{
			List<CorePlugin> initSchedule = new List<CorePlugin>();
			
			Log.Editor.Write("Reloading core plugins...");
			Log.Editor.PushIndent();
			int count = workInterface.ReloadSchedule.Count;
			while (workInterface.ReloadSchedule.Count > 0)
			{
				string curPath = workInterface.ReloadSchedule[0];
				
				Log.Editor.Write("{0}...", curPath);
				Log.Editor.PushIndent();
				{
					CorePlugin plugin = workInterface.MainForm.Invoke((Func<string,CorePlugin>)DualityApp.PluginManager.ReloadPlugin, curPath) as CorePlugin;
					if (plugin != null) initSchedule.Add(plugin);
					workInterface.Progress += 0.10f / (float)count;
				}
				Log.Editor.PopIndent();
				Thread.Sleep(20);

				string xmlDocFile = curPath.Replace(".dll", ".xml");
				if (File.Exists(xmlDocFile))
				{
					workInterface.MainForm.Invoke((Action<string>)HelpSystem.LoadXmlCodeDoc, xmlDocFile);
				}
				workInterface.ReloadSchedule.RemoveAt(0);
				workInterface.ReloadDone.Add(curPath);
				workInterface.Progress += 0.05f / (float)count;
			}
			Log.Editor.PopIndent();

			Log.Editor.Write("Initializing reloaded core plugins...");
			Log.Editor.PushIndent();
			foreach (CorePlugin plugin in initSchedule)
			{
				Log.Editor.Write("{0}...", plugin.AssemblyName);
				Log.Editor.PushIndent();
				workInterface.MainForm.Invoke((Action<CorePlugin>)DualityApp.PluginManager.InitPlugin, plugin);
				workInterface.Progress += 0.05f / (float)count;
				Log.Editor.PopIndent();
			}
			Log.Editor.PopIndent();
		}
		private static void SaveTemporaryData(WorkerInterface workInterface, Stream strScene, Stream strData)
		{
			StreamWriter strDataWriter = new StreamWriter(strData);
			strDataWriter.WriteLine(Scene.CurrentPath);
			strDataWriter.Flush();
			workInterface.MainForm.Invoke((Action)delegate()
			{
				// Save all persistent data
				workInterface.MainForm.Invoke((Action)DualityEditorApp.SaveAllProjectData);
				workInterface.MainForm.Invoke((Action)(() => Scene.Current.Save(strScene)));
			});
			workInterface.Progress += 0.4f;
		}
		private static void RestoreTemporaryData(WorkerInterface workInterface, Stream strScene, Stream strData)
		{
			StreamReader strDataReader = new StreamReader(strData);
			string scenePath = strDataReader.ReadLine();
			workInterface.MainForm.Invoke((Action)(() => workInterface.TempScene = Resource.Load<Scene>(strScene, scenePath)));
			if (!workInterface.TempScene.IsRuntimeResource)
			{
				// Register the reloaded Scene in the ContentProvider, if it wasn't just a temporary one.
				workInterface.MainForm.Invoke((Action)(() => ContentProvider.AddContent(scenePath, workInterface.TempScene)));
			}
		}
		
		/// <summary>
		/// Determines whether the plugin that is represented by the specified Assembly file is referenced by any of the
		/// specified Assemblies.
		/// </summary>
		/// <param name="pluginFilePath"></param>
		/// <param name="assemblies"></param>
		/// <returns></returns>
		private static bool IsDependencyPlugin(string pluginFilePath, IEnumerable<Assembly> assemblies)
		{
			string asmName = Path.GetFileNameWithoutExtension(pluginFilePath);
			foreach (Assembly assembly in assemblies)
			{
				AssemblyName[] refNames = assembly.GetReferencedAssemblies();
				if (refNames.Any(rn => rn.Name == asmName)) return true;
			}
			return false;
		}

		private delegate void CloseMainFormDelegate(MainForm form);
		private static void CloseMainForm(MainForm form)
		{
			Application.Exit();
		}
	}
}
