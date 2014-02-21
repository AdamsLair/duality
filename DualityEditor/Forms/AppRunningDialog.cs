using System;
using System.Windows.Forms;
using System.Diagnostics;

namespace Duality.Editor.Forms
{
	public partial class AppRunningDialog : Form
	{
		Process		app		= null;

		public AppRunningDialog(Process app)
		{
			this.InitializeComponent();
			this.app = app;
			this.timerProcessState.Enabled = true;
		}

		private void timerProcessState_Tick(object sender, EventArgs e)
		{
			if (this.app.HasExited) this.Close();
		}
	}
}
