using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;

using Aga.Controls.Tree;

using Duality.Plugins.Tilemaps;
using Duality.Editor.Plugins.Tilemaps;
using Duality.Editor.Plugins.Tilemaps.UndoRedoActions;
using Duality.Editor.Plugins.Tilemaps.Properties;

namespace Duality.Editor.Plugins.Tilemaps.TilesetEditorModes
{
	/// <summary>
	/// Allows to edit the physical shape of each tile which can be used for collision detection and physics.
	/// </summary>
	public class CollisionInfoTilesetEditorMode : TilesetEditorMode
	{
		private class CollisionInfoLayerNode : TilesetEditorLayerNode
		{
			private int layerIndex = 0;
			private string name = null;
			private string desc = null;

			public override string Title
			{
				get { return this.name; }
			}
			public override string Description
			{
				get { return this.desc; }
			}
			public int LayerIndex
			{
				get { return this.layerIndex; }
			}

			public CollisionInfoLayerNode(int layerIndex, string name, string description, Image image) : base()
			{
				this.layerIndex = layerIndex;
				this.name = name;
				this.desc = description;
				this.Image = image;
			}
		}


		private TreeModel                treeModel      = new TreeModel();
		private CollisionInfoLayerNode[] layerNodes     = null;
		private int                      editLayerIndex = 0;
		private TileCollisionShape       hoveredBit     = TileCollisionShape.Free;
		private bool                     isUserDrawing  = false;
		private bool                     drawSetBit     = false;


		public override string Id
		{
			get { return "CollisionInfoTilesetEditorMode"; }
		}
		public override string Name
		{
			get { return TilemapsRes.TilesetEditorMode_CollisionInfo_Name; }
		}
		public override Image Icon
		{
			get { return TilemapsResCache.IconTilesetCollisionInfo; }
		}
		public override int SortOrder
		{
			get { return -90; }
		}
		public override ITreeModel LayerModel
		{
			get { return this.treeModel; }
		}
		private Bitmap SimpleCollisionBitImage
		{
			get { return TilemapsResCache.TilesetCollisionBit; }
		}
		private Bitmap ComplexCollisionBitImage
		{
			get { return TilemapsResCache.TilesetCollisionBitSmall; }
		}


		public CollisionInfoTilesetEditorMode()
		{
			this.layerNodes = new CollisionInfoLayerNode[TileCollisionShapes.LayerCount];
			for (int layerIndex = 0; layerIndex < this.layerNodes.Length; layerIndex++)
			{
				string nameBase = (layerIndex == 0) ? 
					TilemapsRes.TilesetEditorCollisionMainLayer_Name : 
					TilemapsRes.TilesetEditorCollisionAuxLayer_Name;
				this.layerNodes[layerIndex] = new CollisionInfoLayerNode(
					layerIndex,
					string.Format(nameBase, layerIndex),
					string.Format(TilemapsRes.TilesetEditorCollisionLayer_Desc, layerIndex),
					TilemapsResCache.IconTilesetCollisionLayer);
				this.treeModel.Nodes.Add(this.layerNodes[layerIndex]);
			}
		}
		
		protected override void OnEnter()
		{
			base.OnEnter();
			this.SelectLayer(this.layerNodes[0]);
			this.TilesetView.PaintTiles += this.TilesetView_PaintTiles;
			this.TilesetView.MouseMove += this.TilesetView_MouseMove;
			this.TilesetView.MouseDown += TilesetView_MouseDown;
			this.TilesetView.MouseUp += TilesetView_MouseUp;
		}
		protected override void OnLeave()
		{
			base.OnLeave();
			this.TilesetView.PaintTiles -= this.TilesetView_PaintTiles;
			this.TilesetView.MouseMove -= this.TilesetView_MouseMove;
			this.TilesetView.MouseDown -= TilesetView_MouseDown;
			this.TilesetView.MouseUp -= TilesetView_MouseUp;
		}
		protected override void OnLayerSelectionChanged(LayerSelectionChangedEventArgs args)
		{
			base.OnLayerSelectionChanged(args);

			// There always needs to be a selected collision layer.
			if (args.SelectedNodeTag == null)
				this.SelectLayer(this.layerNodes[0]);
			// Set the internal selected layer index to the one matching the selected node
			else if (args.SelectedNodeTag is CollisionInfoLayerNode)
				this.editLayerIndex = (args.SelectedNodeTag as CollisionInfoLayerNode).LayerIndex;

			this.TilesetView.Invalidate();
		}

		private void TilesetView_PaintTiles(object sender, TilesetViewPaintTilesEventArgs e)
		{
			Bitmap simpleBitImage = this.SimpleCollisionBitImage;
			Bitmap complexBitImage = this.ComplexCollisionBitImage;

			TileInput[] tileInput = e.Tileset.TileInput.Data;
			for (int i = 0; i < e.PaintedTiles.Count; i++)
			{
				TilesetViewPaintTileData paintData = e.PaintedTiles[i];
				TileCollisionShape collision = TileCollisionShape.Free;
				if (tileInput.Length > paintData.TileIndex)
				{
					collision = tileInput[paintData.TileIndex].Collision[this.editLayerIndex];
				}
				bool tileHovered = this.TilesetView.HoveredTileIndex == paintData.TileIndex;
				bool simpleCollision = 
					collision == TileCollisionShape.Free || 
					collision == TileCollisionShape.Solid;

				// Draw a dot for each collision bit of this tile
				Bitmap centerBitImage = simpleCollision ? simpleBitImage : complexBitImage;
				e.Graphics.DrawImageTint(
					centerBitImage, 
					GetCollisionBitColor(TileCollisionShape.Fill, this.hoveredBit, tileHovered, collision), 
					paintData.ViewRect.X + (paintData.ViewRect.Width - centerBitImage.Width) / 2,
					paintData.ViewRect.Y + (paintData.ViewRect.Height - centerBitImage.Height) / 2);
				// Only draw the complex ones when necessary
				if (!simpleCollision || (tileHovered && this.hoveredBit != TileCollisionShape.Fill))
				{
					e.Graphics.DrawImageTint(
						complexBitImage, 
						GetCollisionBitColor(TileCollisionShape.TopLeft, this.hoveredBit, tileHovered, collision), 
						paintData.ViewRect.X + 1,
						paintData.ViewRect.Y + 1);
					e.Graphics.DrawImageTint(
						complexBitImage, 
						GetCollisionBitColor(TileCollisionShape.TopRight, this.hoveredBit, tileHovered, collision), 
						paintData.ViewRect.X + paintData.ViewRect.Width - 1 - complexBitImage.Width,
						paintData.ViewRect.Y + 1);
					e.Graphics.DrawImageTint(
						complexBitImage, 
						GetCollisionBitColor(TileCollisionShape.BottomLeft, this.hoveredBit, tileHovered, collision), 
						paintData.ViewRect.X + 1,
						paintData.ViewRect.Y + paintData.ViewRect.Height - 1 - complexBitImage.Height);
					e.Graphics.DrawImageTint(
						complexBitImage, 
						GetCollisionBitColor(TileCollisionShape.BottomRight, this.hoveredBit, tileHovered, collision), 
						paintData.ViewRect.X + paintData.ViewRect.Width - 1 - complexBitImage.Width,
						paintData.ViewRect.Y + paintData.ViewRect.Height - 1 - complexBitImage.Height);
				}
			}
		}
		private void TilesetView_MouseMove(object sender, MouseEventArgs e)
		{
			Size tileSize = this.TilesetView.TileSize;
			Point tilePos = this.TilesetView.GetTileIndexLocation(this.TilesetView.HoveredTileIndex);
			Point posOnTile = new Point(e.X - tilePos.X, e.Y - tilePos.Y);
			Bitmap cornerBitImage = this.ComplexCollisionBitImage;
			Size cornerBitSize = new Size(
				cornerBitImage.Width + 1,
				cornerBitImage.Height + 1);
			Size centerSize = new Size(
				tileSize.Width - cornerBitSize.Width * 2,
				tileSize.Height - cornerBitSize.Width * 2);

			// Determine the hovered collision bit based on column / row hover states
			TileCollisionShape lastHoveredBit = this.hoveredBit;
			bool defaultToCenter = (lastHoveredBit == TileCollisionShape.Fill);
			if (defaultToCenter)
			{
				// Check all the corners first
				if (posOnTile.X < cornerBitSize.Width && posOnTile.Y < cornerBitSize.Height)
					this.hoveredBit = TileCollisionShape.TopLeft;
				else if (posOnTile.X > tileSize.Width - cornerBitSize.Width && posOnTile.Y < cornerBitSize.Height)
					this.hoveredBit = TileCollisionShape.TopRight;
				else if (posOnTile.X < cornerBitSize.Width && posOnTile.Y > tileSize.Height - cornerBitSize.Height)
					this.hoveredBit = TileCollisionShape.BottomLeft;
				else if (posOnTile.X > tileSize.Width - cornerBitSize.Width && posOnTile.Y > tileSize.Height - cornerBitSize.Height)
					this.hoveredBit = TileCollisionShape.BottomRight;
				// Default to the fill bit if no corner is hit
				else
					this.hoveredBit = TileCollisionShape.Fill;
			}
			else
			{
				// Is the center bit hovered? Check first and default to one of the corners
				if (posOnTile.X > (tileSize.Width - centerSize.Width) / 2 &&
					posOnTile.X < (tileSize.Width + centerSize.Width) / 2 &&
					posOnTile.Y > (tileSize.Height - centerSize.Height) / 2 &&
					posOnTile.Y < (tileSize.Height + centerSize.Height) / 2)
				{
					this.hoveredBit = TileCollisionShape.Fill;
				}
				// Left part of the tile
				else if (posOnTile.X < tileSize.Width / 2)
				{
					if (posOnTile.Y < tileSize.Height / 2)
						this.hoveredBit = TileCollisionShape.TopLeft;
					else
						this.hoveredBit = TileCollisionShape.BottomLeft;
				}
				// Right part of the tile
				else
				{
					if (posOnTile.Y < tileSize.Height / 2)
						this.hoveredBit = TileCollisionShape.TopRight;
					else
						this.hoveredBit = TileCollisionShape.BottomRight;
				}
			}
			
			// If the user is in the process of setting or clearing bits, perform the drawing operation
			if (this.isUserDrawing)
				this.DrawCollisionBit();

			if (lastHoveredBit != this.hoveredBit)
				this.TilesetView.InvalidateTile(this.TilesetView.HoveredTileIndex, 0);
		}
		private void TilesetView_MouseUp(object sender, MouseEventArgs e)
		{
			this.isUserDrawing = false;
		}
		private void TilesetView_MouseDown(object sender, MouseEventArgs e)
		{
			Tileset tileset = this.SelectedTileset.Res;
			if (tileset == null) return;

			int tileIndex = this.TilesetView.HoveredTileIndex;
			if (tileIndex < 0 || tileIndex > tileset.TileCount) return;

			// Determine whether the user will be setting or clearing collision bits
			TileCollisionShape collision = tileset.TileInput[tileIndex].Collision[this.editLayerIndex];
			this.drawSetBit = !collision.HasFlag(this.hoveredBit);
			this.isUserDrawing = true;

			// Perform the drawing operation
			this.DrawCollisionBit();
		}

		private void DrawCollisionBit()
		{
			Tileset tileset = this.SelectedTileset.Res;
			if (tileset == null) return;

			int tileIndex = this.TilesetView.HoveredTileIndex;
			if (tileIndex < 0 || tileIndex > tileset.TileCount) return;

			TileInput input = tileset.TileInput[tileIndex];
			bool simpleCollision = 
				input.Collision[this.editLayerIndex] == TileCollisionShape.Free ||
				input.Collision[this.editLayerIndex] == TileCollisionShape.Solid;

			if (simpleCollision && this.hoveredBit == TileCollisionShape.Fill)
			{
				if (this.drawSetBit)
					input.Collision[this.editLayerIndex] = TileCollisionShape.Solid;
				else
					input.Collision[this.editLayerIndex] = TileCollisionShape.Free;
			}
			else
			{
				if (this.drawSetBit)
					input.Collision[this.editLayerIndex] |= this.hoveredBit;
				else
					input.Collision[this.editLayerIndex] &= ~this.hoveredBit;
			}

			UndoRedoManager.Do(new EditTilesetTileInputAction(tileset, tileIndex, input));
		}

		private static Color GetCollisionBitColor(TileCollisionShape bit, TileCollisionShape hoveredBit, bool tileHovered, TileCollisionShape tileCollision)
		{
			bool simpleCollision = 
				tileCollision == TileCollisionShape.Free ||
				tileCollision == TileCollisionShape.Solid;
			bool importantBit = 
				tileCollision.HasFlag(bit) && 
				(!simpleCollision || bit == TileCollisionShape.Fill);
			return Color.FromArgb(
				(tileHovered && hoveredBit == bit) ? 255 : 
				(tileHovered && importantBit ? 192 : 
				(importantBit ? 160 : 
				(tileHovered ? 128 : 
				64))), 
				(tileCollision.HasFlag(bit)) ? Color.FromArgb(128, 192, 255) : 
				Color.FromArgb(255, 255, 255));
		}
	}
}
