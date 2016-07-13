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
	public class EditTilesetTileInputAction : UndoRedoAction
	{
		private Tileset            tileset;
		private RawList<TileInput> tileInput;
		private RawList<bool>      tileInputMask;
		private RawList<TileInput> backupTileInput;

		public override string Name
		{
			get { return TilemapsRes.UndoRedo_TilesetEditTileInput; }
		}
		public override bool IsVoid
		{
			get { return this.tileset == null; }
		}

		public EditTilesetTileInputAction(Tileset tileset, int tileIndex, TileInput tileInput)
		{
			this.tileset = tileset;

			this.tileInput = new RawList<TileInput>(tileIndex + 1);
			this.tileInput.Count = tileIndex + 1;
			this.tileInput.Data[tileIndex] = tileInput;
			
			this.tileInputMask = new RawList<bool>(tileIndex + 1);
			this.tileInputMask.Count = tileIndex + 1;
			this.tileInputMask.Data[tileIndex] = true;
		}
		public EditTilesetTileInputAction(Tileset tileset, RawList<TileInput> tileInput, RawList<bool> tileInputMask)
		{
			if (tileInput == null) throw new ArgumentNullException("tileInput");
			if (tileInputMask == null) throw new ArgumentNullException("tileInputMask");
			if (tileInputMask.Count != tileInput.Count) throw new ArgumentException("Input Mask needs to be the same size as input.", "tileInputMask");

			this.tileset = tileset;
			this.tileInput = tileInput;
			this.tileInputMask = tileInputMask;
		}

		public override void Do()
		{
			// Make a backup copy of the original tile input data block
			if (this.backupTileInput == null)
			{
				this.backupTileInput = new RawList<TileInput>(this.tileset.TileInput);
			}

			// Expand the tile input when required
			this.tileset.TileInput.Count = Math.Max(
				this.tileset.TileInput.Count,
				this.tileInput.Count);

			// Adjust the backup length to fit the new tile input length
			this.backupTileInput.Count = this.tileset.TileInput.Count;

			// Copy all edited tile input indices
			for (int i = 0; i < this.tileInput.Count; i++)
			{
				if (!this.tileInputMask[i]) continue;
				this.tileset.TileInput[i] = this.tileInput[i];
			}

			// Notify the editor that we changed the tileset
			this.OnNotifyPropertyChanged();
		}
		public override void Undo()
		{
			// Expand the tile input when required
			this.tileset.TileInput.Count = Math.Max(
				this.tileset.TileInput.Count,
				this.backupTileInput.Count);

			// Copy the original tile input data block back to the tileset where changes occurred
			for (int i = 0; i < this.tileInput.Count; i++)
			{
				if (!this.tileInputMask[i]) continue;
				this.tileset.TileInput[i] = this.backupTileInput[i];
			}

			// Notify the editor that we changed the tileset
			this.OnNotifyPropertyChanged();
		}

		public override bool CanAppend(UndoRedoAction action)
		{
			EditTilesetTileInputAction castAction = action as EditTilesetTileInputAction;
			if (castAction == null) return false;
			if (castAction.tileset != this.tileset) return false;
			return true;
		}
		public override void Append(UndoRedoAction action, bool performAction)
		{
			base.Append(action, performAction);
			EditTilesetTileInputAction castAction = action as EditTilesetTileInputAction;

			if (performAction)
			{
				castAction.backupTileInput = this.backupTileInput;
				castAction.Do();
			}
			
			// Copy the new actions tile input over to this one and adjust the masking
			this.tileInput.Count = Math.Max(this.tileInput.Count, castAction.tileInput.Count);
			this.tileInputMask.Count = this.tileInput.Count;
			for (int i = 0; i < castAction.tileInput.Count; i++)
			{
				if (!castAction.tileInputMask[i]) continue;
				this.tileInput[i] = castAction.tileInput[i];
				this.tileInputMask[i] = true;
			}

			// Adjust the backup length to fit the new tile input length
			this.backupTileInput.Count = this.tileset.TileInput.Count;
		}

		private void OnNotifyPropertyChanged()
		{
			DualityEditorApp.NotifyObjPropChanged(
				this,
				new ObjectSelection(this.tileset),
				TilemapsReflectionInfo.Property_Tileset_TileInput);
		}
	}
}
