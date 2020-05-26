namespace Duality.Editor
{
	/// <summary>
	/// Provides general information about this Duality application / game.
	/// </summary>
	public class DualityEditorAppData
	{
		private string sourceDirectory = "Source";

		/// <summary>
		/// [GET / SET] The path to the source code of your game.
		/// </summary>
		public string SourceDirectory
		{
			get { return this.sourceDirectory; }
			set { this.sourceDirectory = value; }
		}
	}
}
