using System.Collections.Generic;
using System.Linq;
using Duality.Editor.Plugins.Base.Properties;
using Duality.Resources;

namespace Duality.Editor.Plugins.Base.UndoRedoActions
{
	public class SetAtlasAction : UndoRedoAction
	{
		private Rect[]       rects         = null;
		private Pixmap[]     pixmaps       = null;
		private List<Rect[]> originalRects = null;

		public override string Name
		{
			get { return EditorBaseRes.UndoRedo_SetAtlas; }
		}

		public SetAtlasAction(IEnumerable<Pixmap> pixmapsEnum, IEnumerable<Rect> atlasRects)
		{
			this.rects = atlasRects.ToArray();
			this.pixmaps = pixmapsEnum.Distinct().ToArray();
		}

		public override bool CanAppend(UndoRedoAction action)
		{
			SetAtlasAction atlasAction = action as SetAtlasAction;
			if (atlasAction == null) return false;
			if (!atlasAction.pixmaps.SetEqual(this.pixmaps)) return false;
			return true;
		}
		public override void Append(UndoRedoAction action, bool performAction)
		{
			base.Append(action, performAction);
			SetAtlasAction atlasAction = action as SetAtlasAction;

			if (performAction)
			{
				atlasAction.originalRects = this.originalRects;
				atlasAction.Do();
			}
			this.rects = atlasAction.rects;
		}
		public override void Do()
		{
			this.originalRects = new List<Rect[]>();

			for (int i = 0; i < this.pixmaps.Length; i++)
			{
				// Copy the existing atlas
				this.originalRects.Add(
					this.pixmaps[i].Atlas == null ? 
					null : 
					this.pixmaps[i].Atlas.ToArray());

				this.pixmaps[i].Atlas = new List<Rect>(this.rects);
			}

			DualityEditorApp.NotifyObjPropChanged(this, new ObjectSelection(this.pixmaps));
		}
		public override void Undo()
		{
			for (int i = 0; i < this.pixmaps.Length; i++)
			{
				this.pixmaps[i].Atlas = 
					this.originalRects[i] == null ? 
					null : 
					new List<Rect>(this.originalRects[i]);
			}

			DualityEditorApp.NotifyObjPropChanged(this, new ObjectSelection(this.pixmaps));
		}
	}
}
