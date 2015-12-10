using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

using Duality;
using Duality.Plugins.Tilemaps;
using Duality.Editor.Plugins.Tilemaps.Properties;


namespace Duality.Editor.Plugins.Tilemaps.CamViewStates
{
	/// <summary>
	/// A dummy tool that represents not performing any operation on the edited <see cref="Tilemap"/>.
	/// </summary>
	public class NoTilemapTool : TilemapTool
	{
		public override string Name
		{
			get { return null; }
		}
		public override Image Icon
		{
			get { return null;; }
		}
		public override Cursor ActionCursor
		{
			get { return CursorHelper.Arrow; }
		}

		public override void UpdatePreview()
		{
			ITilemapToolEnvironment env = this.Environment;

			env.ActiveAreaOutlines.Clear();
			env.ActiveOrigin = env.HoveredTile;
			env.ActiveArea.ResizeClear(0, 0);
			env.SubmitActiveAreaChanges(true);
		}
	}
}
