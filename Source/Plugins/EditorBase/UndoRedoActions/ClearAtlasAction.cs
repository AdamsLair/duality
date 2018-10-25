using System.Collections.Generic;
using System.Linq;
using Duality.Editor.Plugins.Base.Properties;
using Duality.Resources;

namespace Duality.Editor.Plugins.Base.UndoRedoActions
{
	public class ClearAtlasAction : UndoRedoAction
	{
		private Pixmap[]		pixmaps			= null;
		private List<RectAtlas> originalAtlases	= null;

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
			this.originalAtlases = new List<RectAtlas>();

			for (int i = 0; i < this.pixmaps.Length; i++)
			{
				// Copy the existing atlas
				this.originalAtlases.Add(this.pixmaps[i].Atlas == null
					? null
					: new RectAtlas(this.pixmaps[i].Atlas));

				this.pixmaps[i].Atlas = null;
			}

			DualityEditorApp.NotifyObjPropChanged(this, new ObjectSelection(this.pixmaps.Distinct()));
		}

		public override void Undo()
		{
			for (int i = 0; i < this.pixmaps.Length; i++)
			{
				this.pixmaps[i].Atlas = this.originalAtlases[i] == null
					? null
					: new RectAtlas(this.originalAtlases[i]);
			}

			DualityEditorApp.NotifyObjPropChanged(this, new ObjectSelection(this.pixmaps.Distinct()));
		}
	}
}
