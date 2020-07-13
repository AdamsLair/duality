namespace Duality.Editor.Plugins.LogView
{
	public class LogViewSettings
	{
		private bool showMessages = true;
		public bool ShowMessages
		{
			get { return this.showMessages; }
			set { this.showMessages = value; }
		}

		private bool showWarnings = true;
		public bool ShowWarnings
		{
			get { return this.showWarnings; }
			set { this.showWarnings = value; }
		}

		private bool showErrors = true;
		public bool ShowErrors
		{
			get { return this.showErrors; }
			set { this.showErrors = value; }
		}

		private bool showCore = true;
		public bool ShowCore
		{
			get { return this.showCore; }
			set { this.showCore = value; }
		}

		private bool showEditor = true;
		public bool ShowEditor
		{
			get { return this.showEditor; }
			set { this.showEditor = value; }
		}

		private bool showGame = true;
		public bool ShowGame
		{
			get { return this.showGame; }
			set { this.showGame = value; }
		}

		private bool autoClear = true;
		public bool AutoClear
		{
			get { return this.autoClear; }
			set { this.autoClear = value; }
		}

		private bool pauseOnError = true;
		public bool PauseOnError
		{
			get { return this.pauseOnError; }
			set { this.pauseOnError = value; }
		}
	}
}
