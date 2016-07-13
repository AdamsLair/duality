using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Duality;
using Duality.Cloning;
using Duality.Resources;
using Duality.Plugins.Tilemaps;

using Duality.Editor;
using Duality.Editor.Plugins.Tilemaps.Properties;
using Duality.Editor.Plugins.Tilemaps.CamViewStates;

namespace Duality.Editor.Plugins.Tilemaps.UndoRedoActions
{
	public class RevertTilesetChangesAction : UndoRedoAction
	{
		private Tileset targetTileset;
		private Tileset backupBeforeChanges;
		private Tileset backupBeforeRevert;

		public override string Name
		{
			get { return TilemapsRes.UndoRedo_RevertTilesetChanges; }
		}
		public override bool IsVoid
		{
			get { return this.targetTileset == null || this.backupBeforeChanges == null; }
		}

		public RevertTilesetChangesAction(Tileset targetTileset, Tileset backupBeforeChanges)
		{
			this.targetTileset = targetTileset;
			this.backupBeforeChanges = backupBeforeChanges;
		}

		public override void Do()
		{
			// Make a backup of the tileset in its current state
			if (this.backupBeforeRevert == null)
				this.backupBeforeRevert = this.targetTileset.DeepClone(BackupCloneContext);
			
			// Revert the tileset back to its unchanged state. No need to compile it,
			// this is meant to un-do changed that were done without compiling yet.
			this.backupBeforeChanges.DeepCopyTo(this.targetTileset);

			// Notify the editor we changes our target tileset
			this.OnNotifyPropertyChanged();
		}
		public override void Undo()
		{
			// Re-apply the changes we made (and then un-did) to the tileset before.
			// No need to compile, because undoing a revert does not mean applying the
			// changes we re-introduced.
			this.backupBeforeRevert.DeepCopyTo(this.targetTileset);

			// Notify the editor we changes our target tileset
			this.OnNotifyPropertyChanged();
		}

		private void OnNotifyPropertyChanged()
		{
			DualityEditorApp.NotifyObjPropChanged(
				this,
				new ObjectSelection(this.targetTileset));
		}
	}
}
