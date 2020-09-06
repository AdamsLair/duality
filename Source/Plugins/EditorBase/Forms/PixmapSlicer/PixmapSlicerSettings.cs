using Duality.Editor.Plugins.Base.Forms;
using Duality.Editor.Plugins.Base.Forms.PixmapSlicer.States;

namespace Duality.Editor.Plugins.Base
{
	/// <summary>
	/// Contains all user settings for the <see cref="PixmapSlicerForm"/>.
	/// </summary>
	public class PixmapSlicerSettings
	{
		private bool darkBackground = false;
		private PixmapNumberingStyle displayIndices = PixmapNumberingStyle.Hovered;

		/// <summary>
		/// Whether the slicer window should use a dark background instead of a bright one.
		/// </summary>
		public bool DarkBackground
		{
			get { return this.darkBackground; }
			set { this.darkBackground = value; }
		}
		/// <summary>
		/// Specifies if and how atlas slice indices are displayed to the user.
		/// </summary>
		public PixmapNumberingStyle DisplayIndices
		{
			get { return this.displayIndices; }
			set { this.displayIndices = value; }
		}
	}
}
