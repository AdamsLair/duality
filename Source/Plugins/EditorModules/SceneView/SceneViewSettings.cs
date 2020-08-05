namespace Duality.Editor.Plugins.SceneView
{
	public class SceneViewSettings
	{
		private bool showComponents = true;

		/// <summary>
		/// Controls whether components are shown as individual nodes in the scene tree
		/// </summary>
		public bool ShowComponents
		{
			get { return this.showComponents; }
			set { this.showComponents = value; }
		}
	}
}