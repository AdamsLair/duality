using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Duality.Editor.Forms
{
	public partial class SplashScreen : Form
	{
		private	bool	recover	= false;

		public SplashScreen(bool recover)
		{
			this.InitializeComponent();
			this.recover = recover;
		}
		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);
			this.mainFormLoader.RunWorkerAsync(this);
		}
		protected MainForm InitEditor()
		{
			try
			{
				MainForm main = new MainForm();
				DualityEditorApp.Init(main, this.recover);
				return main;
			}
			catch (Exception e)
			{
				Log.Editor.Write("An error occurred while initializing the editor: {0}", LogFormat.Exception(e));
				return null;
			}
		}

		private void mainFormLoader_DoWork(object sender, DoWorkEventArgs e)
		{
			SplashScreen screen = e.Argument as SplashScreen;

			System.Threading.Thread.Sleep(50); // Assures the screen had time to display correctly
			object result = screen.Invoke(new Func<MainForm>(screen.InitEditor));

			e.Result = result;
		}
		private void mainFormLoader_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			this.Hide();
			try
			{
				MainForm mainForm = e.Result as MainForm;
				if (mainForm != null)
				{
					mainForm.Show();
				}
				else
				{
					// If we don't get a main form back, something went wrong.
					// Exit, so we don't end up as a hidden process without UI.
					Application.Exit();
				}
			}
			finally
			{
				this.Close();
			}
		}
	}
}
