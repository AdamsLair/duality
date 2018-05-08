using System.Collections.Generic;
using System.Linq;
using Duality.Editor.Plugins.Base.Properties;
using Duality.Resources;

namespace Duality.Editor.Plugins.Base.UndoRedoActions
{
	public class SetAtlasRectAction : UndoRedoAction
	{
		private Rect		rectAdded		= Rect.Empty;
		private int			indexSet		= -1;
		private Pixmap[]	pixmaps			= null;
		private Rect?[]		originalRects	= null;

		public override string Name
		{
			get { return EditorBaseRes.UndoRedo_SetAtlasRect; }
		}

		public SetAtlasRectAction(Rect atlasRect, int index, IEnumerable<Pixmap> pixmapsEnum)
		{
			this.rectAdded = atlasRect;
			this.indexSet = index;
			this.pixmaps = pixmapsEnum.ToArray();
		}

		public override void Do()
		{
			this.originalRects = new Rect?[this.pixmaps.Length];

			for (int i = 0; i < this.pixmaps.Length; i++)
			{
				Pixmap pixmap = this.pixmaps[i];

				if (pixmap.Atlas == null)
					pixmap.Atlas = new List<Rect>();

				if (this.indexSet < pixmap.Atlas.Count)
				{
					this.originalRects[i] = pixmap.Atlas[this.indexSet];
					pixmap.Atlas[this.indexSet] = this.rectAdded;
				}
				else
				{
					this.originalRects[i] = null;
					pixmap.Atlas.Add(this.rectAdded);
				}
			}

			DualityEditorApp.NotifyObjPropChanged(this, new ObjectSelection(this.pixmaps.Distinct()));
		}

		public override void Undo()
		{
			for (int i = 0; i < this.pixmaps.Length; i++)
			{
				Pixmap pixmap = this.pixmaps[i];
				Rect? originalRect = this.originalRects[i];

				if (originalRect.HasValue)
				{
					pixmap.Atlas[this.indexSet] = originalRect.Value;
				}
				else
				{
					pixmap.Atlas.RemoveAt(this.indexSet);
				}
			}

			DualityEditorApp.NotifyObjPropChanged(this, new ObjectSelection(this.pixmaps.Distinct()));
		}
	}
}
