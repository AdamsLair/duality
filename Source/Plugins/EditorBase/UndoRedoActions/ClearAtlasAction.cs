using System.Collections.Generic;
using System.Linq;
using Duality.Editor.Plugins.Base.Properties;
using Duality.Resources;

namespace Duality.Editor.Plugins.Base.UndoRedoActions
{
	public class ClearAtlasAction : UndoRedoAction
	{
		private Pixmap[]		pixmaps			= null;
		private List<Rect[]>	originalRects	= null;

		public override string Name
		{
			get { return EditorBaseRes.UndoRedo_ClearAtlas; }
		}

		public ClearAtlasAction(IEnumerable<Pixmap> pixmapsEnum)
		{
			this.pixmaps = pixmapsEnum.ToArray();
		}

		public override void Do()
		{
			this.originalRects = new List<Rect[]>();

			for (int i = 0; i < this.pixmaps.Length; i++)
			{
				// Copy the existing atlas
				this.originalRects.Add(this.pixmaps[i].Atlas == null
					? null
					: this.pixmaps[i].Atlas.ToArray());

				this.pixmaps[i].Atlas = null;
				this.pixmaps[i].AnimCols = 0;
				this.pixmaps[i].AnimRows = 0;
				this.pixmaps[i].AnimFrameBorder = 0;
			}

			DualityEditorApp.NotifyObjPropChanged(this, new ObjectSelection(this.pixmaps.Distinct()));
		}

		public override void Undo()
		{
			for (int i = 0; i < this.pixmaps.Length; i++)
			{
				this.pixmaps[i].Atlas = this.originalRects[i] == null
					? null
					: new List<Rect>(this.originalRects[i]);
			}

			DualityEditorApp.NotifyObjPropChanged(this, new ObjectSelection(this.pixmaps.Distinct()));
		}
	}
}
