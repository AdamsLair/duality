using System.Collections.Generic;
using System.Linq;
using Duality.Resources;

namespace Duality.Editor.Plugins.Base.UndoRedoActions
{
	public class SetAtlasAction : UndoRedoAction
	{
		private Rect[]			rects			= null;
		private Pixmap[]		pixmaps			= null;
		private List<Rect[]>	originalRects	= null;

		// TODO: move string to resources
		public override string Name
		{
			get { return "Set Atlas"; }
		}

		public SetAtlasAction(IEnumerable<Rect> atlasRects, IEnumerable<Pixmap> pixmapsEnum)
		{
			this.rects = atlasRects.ToArray();
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

				this.pixmaps[i].Atlas = new List<Rect>(this.rects);
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
