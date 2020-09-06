using Duality.Editor.Plugins.Base.Forms;

namespace Duality.Editor.Plugins.Base
{
	/// <summary>
	/// Contains all user settings related to the editor base plugin.
	/// </summary>
	public class EditorBasePluginSettings
	{
		private PixmapSlicerSettings pixmapSlicer = new PixmapSlicerSettings();

		/// <summary>
		/// Settings related to the <see cref="PixmapSlicerForm"/> tool window.
		/// </summary>
		public PixmapSlicerSettings PixmapSlicer
		{
			get { return this.pixmapSlicer; }
			set { this.pixmapSlicer = value; }
		}
	}
}
