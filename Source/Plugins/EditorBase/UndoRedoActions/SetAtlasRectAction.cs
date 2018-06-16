using System.Collections.Generic;
using System.Linq;
using Duality.Editor.Plugins.Base.Properties;
using Duality.Resources;

namespace Duality.Editor.Plugins.Base.UndoRedoActions
{
	public class SetAtlasRectAction : UndoRedoAction
	{
		private Rect     rect         = Rect.Empty;
		private int      atlasIndex   = -1;
		private Pixmap   pixmap       = null;
		private Rect?    originalRect = null;

		public override string Name
		{
			get { return EditorBaseRes.UndoRedo_SetAtlasRect; }
		}

		public SetAtlasRectAction(Pixmap pixmap, int atlasIndex, Rect atlasRect)
		{
			this.pixmap = pixmap;
			this.atlasIndex = atlasIndex;
			this.rect = atlasRect;
		}

		public override bool CanAppend(UndoRedoAction action)
		{
			SetAtlasRectAction atlasAction = action as SetAtlasRectAction;
			if (atlasAction == null) return false;
			if (atlasAction.pixmap != this.pixmap) return false;
			if (atlasAction.atlasIndex != this.atlasIndex) return false;
			return true;
		}
		public override void Append(UndoRedoAction action, bool performAction)
		{
			base.Append(action, performAction);
			SetAtlasRectAction atlasAction = action as SetAtlasRectAction;

			if (performAction)
			{
				atlasAction.originalRect = this.originalRect;
				atlasAction.Do();
			}
			this.rect = atlasAction.rect;
		}
		public override void Do()
		{
			if (this.pixmap.Atlas == null)
				this.pixmap.Atlas = new List<Rect>();

			if (this.atlasIndex < this.pixmap.Atlas.Count)
			{
				this.originalRect = this.pixmap.Atlas[this.atlasIndex];
				this.pixmap.Atlas[this.atlasIndex] = this.rect;
			}
			else
			{
				this.originalRect = null;
				this.pixmap.Atlas.Add(this.rect);
			}

			DualityEditorApp.NotifyObjPropChanged(this, new ObjectSelection(this.pixmap));
		}
		public override void Undo()
		{
			if (this.originalRect.HasValue)
			{
				this.pixmap.Atlas[this.atlasIndex] = this.originalRect.Value;
			}
			else
			{
				this.pixmap.Atlas.RemoveAt(this.atlasIndex);
			}

			DualityEditorApp.NotifyObjPropChanged(this, new ObjectSelection(this.pixmap));
		}
	}
}
