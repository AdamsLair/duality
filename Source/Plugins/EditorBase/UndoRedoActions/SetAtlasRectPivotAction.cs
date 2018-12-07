using Duality.Editor.Plugins.Base.Properties;
using Duality.Resources;

namespace Duality.Editor.Plugins.Base.UndoRedoActions
{
	public class SetAtlasRectPivotAction : UndoRedoAction
	{
		private Vector2  pivot         = Vector2.Zero;
		private Vector2? originalPivot = null;
		private int      atlasIndex    = -1;
		private Pixmap   pixmap        = null;

		public override string Name
		{
			get { return EditorBaseRes.UndoRedo_SetAtlasRectPivot; }
		}

		public SetAtlasRectPivotAction(Pixmap pixmap, int atlasIndex, Vector2 pivot)
		{
			this.pixmap = pixmap;
			this.atlasIndex = atlasIndex;
			this.pivot = pivot;
		}
		public override bool CanAppend(UndoRedoAction action)
		{
			SetAtlasRectPivotAction atlasAction = action as SetAtlasRectPivotAction;
			if (atlasAction == null) return false;
			if (atlasAction.pixmap != this.pixmap) return false;
			if (atlasAction.atlasIndex != this.atlasIndex) return false;
			return true;
		}
		public override void Append(UndoRedoAction action, bool performAction)
		{
			base.Append(action, performAction);
			SetAtlasRectPivotAction atlasAction = action as SetAtlasRectPivotAction;

			if (performAction)
			{
				atlasAction.originalPivot = this.originalPivot;
				atlasAction.Do();
			}
			this.pivot = atlasAction.pivot;
		}
		public override void Do()
		{
			if (this.pixmap.Atlas == null)
				this.pixmap.Atlas = new RectAtlas();

			if (this.atlasIndex < this.pixmap.Atlas.Count)
			{
				this.originalPivot = this.pixmap.Atlas.Pivots[this.atlasIndex];
				this.pixmap.Atlas.Pivots[this.atlasIndex] = this.pivot;
			}
			else
			{
				this.originalPivot = null;
				this.pixmap.Atlas.Add(Rect.Empty);
				this.pixmap.Atlas.Pivots[this.pixmap.Atlas.Count - 1] = this.pivot;
			}

			DualityEditorApp.NotifyObjPropChanged(this, new ObjectSelection(this.pixmap));
		}

		public override void Undo()
		{
			if (this.originalPivot.HasValue)
			{
				this.pixmap.Atlas.Pivots[this.atlasIndex] = this.originalPivot.Value;
			}
			else
			{
				this.pixmap.Atlas.RemoveAt(this.atlasIndex);
			}

			DualityEditorApp.NotifyObjPropChanged(this, new ObjectSelection(this.pixmap));
		}
	}
}
