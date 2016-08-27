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
	public class EditTilesetAutoTileItemAction : UndoRedoAction
	{
		private Tileset                      tileset;
		private TilesetAutoTileInput         autoTile;
		private RawList<TilesetAutoTileItem> tileInput;
		private RawList<bool>                tileInputMask;
		private RawList<TilesetAutoTileItem> backupTileInput;

		public override string Name
		{
			get { return TilemapsRes.UndoRedo_TilesetEditTileInput; }
		}
		public override bool IsVoid
		{
			get { return this.tileset == null || this.autoTile == null; }
		}

		public EditTilesetAutoTileItemAction(Tileset tileset, TilesetAutoTileInput autoTile, int tileIndex, TilesetAutoTileItem tileInput)
		{
			if (tileset == null) throw new ArgumentNullException("tileset");
			if (autoTile == null) throw new ArgumentNullException("autoTile");

			this.tileset = tileset;
			this.autoTile = autoTile;

			this.tileInput = new RawList<TilesetAutoTileItem>(tileIndex + 1);
			this.tileInput.Count = tileIndex + 1;
			this.tileInput.Data[tileIndex] = tileInput;
			
			this.tileInputMask = new RawList<bool>(tileIndex + 1);
			this.tileInputMask.Count = tileIndex + 1;
			this.tileInputMask.Data[tileIndex] = true;
		}
		public EditTilesetAutoTileItemAction(Tileset tileset, TilesetAutoTileInput autoTile, RawList<TilesetAutoTileItem> tileInput, RawList<bool> tileInputMask)
		{
			if (tileset == null) throw new ArgumentNullException("tileset");
			if (autoTile == null) throw new ArgumentNullException("autoTile");
			if (tileInput == null) throw new ArgumentNullException("tileInput");
			if (tileInputMask == null) throw new ArgumentNullException("tileInputMask");
			if (tileInputMask.Count != tileInput.Count) throw new ArgumentException("Input Mask needs to be the same size as input.", "tileInputMask");

			this.autoTile = autoTile;
			this.tileset = tileset;
			this.tileInput = tileInput;
			this.tileInputMask = tileInputMask;
		}

		public override void Do()
		{
			// Make a backup copy of the original tile input data block
			if (this.backupTileInput == null)
			{
				this.backupTileInput = new RawList<TilesetAutoTileItem>(this.autoTile.TileInput);
			}

			// Expand the tile input when required
			this.autoTile.TileInput.Count = Math.Max(
				this.autoTile.TileInput.Count,
				this.tileInput.Count);

			// Adjust the backup length to fit the new tile input length
			this.backupTileInput.Count = this.autoTile.TileInput.Count;

			// Copy all edited tile input indices
			for (int i = 0; i < this.tileInput.Count; i++)
			{
				if (!this.tileInputMask[i]) continue;
				this.autoTile.TileInput[i] = this.tileInput[i];
			}

			// Notify the editor that we changed the tileset
			this.OnNotifyPropertyChanged();
		}
		public override void Undo()
		{
			// Expand the tile input when required
			this.autoTile.TileInput.Count = Math.Max(
				this.autoTile.TileInput.Count,
				this.backupTileInput.Count);

			// Copy the original tile input data block back to the tileset where changes occurred
			for (int i = 0; i < this.tileInput.Count; i++)
			{
				if (!this.tileInputMask[i]) continue;
				this.autoTile.TileInput[i] = this.backupTileInput[i];
			}

			// Notify the editor that we changed the tileset
			this.OnNotifyPropertyChanged();
		}

		public override bool CanAppend(UndoRedoAction action)
		{
			EditTilesetAutoTileItemAction castAction = action as EditTilesetAutoTileItemAction;
			if (castAction == null) return false;
			if (castAction.autoTile != this.autoTile) return false;
			return true;
		}
		public override void Append(UndoRedoAction action, bool performAction)
		{
			base.Append(action, performAction);
			EditTilesetAutoTileItemAction castAction = action as EditTilesetAutoTileItemAction;

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
			this.backupTileInput.Count = this.autoTile.TileInput.Count;
		}

		private void OnNotifyPropertyChanged()
		{
			DualityEditorApp.NotifyObjPropChanged(
				this,
				new ObjectSelection(this.tileset),
				TilemapsReflectionInfo.Property_Tileset_AutoTileConfig);
		}
	}
}
