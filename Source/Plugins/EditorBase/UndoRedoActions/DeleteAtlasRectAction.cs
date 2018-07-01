using System.Collections.Generic;
using System.Linq;
using Duality.Editor.Plugins.Base.Properties;
using Duality.Resources;

namespace Duality.Editor.Plugins.Base.UndoRedoActions
{
	public class DeleteAtlasRectAction : UndoRedoAction
	{
		private int			deletedIndex	= -1;
		private Pixmap[]	pixmaps			= null;
		private Rect[]		deletedRects	= null;

		public override string Name
		{
			get { return EditorBaseRes.UndoRedo_DeleteAtlasRect; }
		}

		public DeleteAtlasRectAction(int indexDeleted, IEnumerable<Pixmap> pixmapsEnum)
		{
			this.deletedIndex = indexDeleted;
			this.pixmaps = pixmapsEnum.ToArray();
		}

		public override void Do()
		{
			this.deletedRects = new Rect[this.pixmaps.Length];

			for (int i = 0; i < this.pixmaps.Length; i++)
			{
				this.deletedRects[i] = this.pixmaps[i].Atlas[this.deletedIndex];
				this.pixmaps[i].Atlas.RemoveAt(this.deletedIndex);
			}

			DualityEditorApp.NotifyObjPropChanged(this, new ObjectSelection(this.pixmaps.Distinct()));
		}

		public override void Undo()
		{
			for (int i = 0; i < this.pixmaps.Length; i++)
			{
				this.pixmaps[i].Atlas.Insert(this.deletedIndex, this.deletedRects[i]);
			}

			DualityEditorApp.NotifyObjPropChanged(this, new ObjectSelection(this.pixmaps.Distinct()));
		}
	}
}
