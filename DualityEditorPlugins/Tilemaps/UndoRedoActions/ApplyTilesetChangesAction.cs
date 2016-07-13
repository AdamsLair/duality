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
	public class ApplyTilesetChangesAction : UndoRedoAction
	{
		private Tileset targetTileset;
		private Tileset backupBeforeChanges;
		private Tileset backupBeforeApply;

		public override string Name
		{
			get { return TilemapsRes.UndoRedo_ApplyTilesetChanges; }
		}
		public override bool IsVoid
		{
			get { return this.targetTileset == null || this.backupBeforeChanges == null; }
		}

		public ApplyTilesetChangesAction(Tileset targetTileset, Tileset backupBeforeChanges)
		{
			this.targetTileset = targetTileset;
			this.backupBeforeChanges = backupBeforeChanges;
		}

		public override void Do()
		{
			// Make a backup of the tileset in its current state
			if (this.backupBeforeApply == null)
				this.backupBeforeApply = this.targetTileset.DeepClone(BackupCloneContext);

			// Compile the tileset so all the changes become visible
			this.targetTileset.Compile();

			// Notify the editor we changes our target tileset
			this.OnNotifyPropertyChanged();
		}
		public override void Undo()
		{
			// First, revert the tileset back to its unchanged state and compile it
			this.backupBeforeChanges.DeepCopyTo(this.targetTileset);
			this.targetTileset.Compile();

			// Next, apply the (uncompiled) changes back to the tileset to
			// achieve the previous state of "not yet compiled changes"
			this.backupBeforeApply.DeepCopyTo(this.targetTileset);

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
