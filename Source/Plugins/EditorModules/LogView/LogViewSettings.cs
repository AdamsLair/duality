namespace Duality.Editor.Plugins.LogView
{
	public class LogViewSettings
	{
		private bool showMessages = true;
		private bool showWarnings = true;
		private bool showErrors = true;
		private bool showCore = true;
		private bool showEditor = true;
		private bool showGame = true;
		private bool autoClear = true;
		private bool pauseOnError = true;

		public bool ShowMessages
		{
			get { return this.showMessages; }
			set { this.showMessages = value; }
		}
		public bool ShowWarnings
		{
			get { return this.showWarnings; }
			set { this.showWarnings = value; }
		}
		public bool ShowErrors
		{
			get { return this.showErrors; }
			set { this.showErrors = value; }
		}
		public bool ShowCore
		{
			get { return this.showCore; }
			set { this.showCore = value; }
		}
		public bool ShowEditor
		{
			get { return this.showEditor; }
			set { this.showEditor = value; }
		}
		public bool ShowGame
		{
			get { return this.showGame; }
			set { this.showGame = value; }
		}
		public bool AutoClear
		{
			get { return this.autoClear; }
			set { this.autoClear = value; }
		}
		public bool PauseOnError
		{
			get { return this.pauseOnError; }
			set { this.pauseOnError = value; }
		}
	}
}
