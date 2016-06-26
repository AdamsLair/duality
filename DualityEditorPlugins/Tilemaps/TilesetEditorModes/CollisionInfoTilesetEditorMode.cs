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
		/// <summary>
		/// Represents a certain area within a tile in the <see cref="TilesetView"/>,
		/// which can be hovered or clicked by the user.
		/// </summary>
		private enum TileHotSpot
		{
			None,

			Center,
			Top,
			Bottom,
			Left,
			Right
		}
		/// <summary>
		/// Describes the way in which the current user drawing operation will interact
		/// with existing collision bits.
		/// </summary>
		private enum CollisionDrawMode
		{
			Set,
			Add,
			Remove
		}
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
		private TileHotSpot              hoveredArea    = TileHotSpot.None;
		private bool                     isUserDrawing  = false;
		private bool                     drawSimple     = false;
		private CollisionDrawMode        drawMode       = CollisionDrawMode.Set;
		private TileCollisionShape       drawShape      = TileCollisionShape.Free;


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
			Color colorFree = Color.White;
			Color colorCollision = Color.FromArgb(128, 192, 255);

			TileInput[] tileInput = e.Tileset.TileInput.Data;
			for (int i = 0; i < e.PaintedTiles.Count; i++)
			{
				TilesetViewPaintTileData paintData = e.PaintedTiles[i];

				// Prepare some data we'll need for drawing the tile collision info overlay
				TileCollisionShape collision = TileCollisionShape.Free;
				if (tileInput.Length > paintData.TileIndex)
				{
					collision = tileInput[paintData.TileIndex].Collision[this.editLayerIndex];
				}
				bool tileHovered = this.TilesetView.HoveredTileIndex == paintData.TileIndex;
				bool simpleCollision = 
					collision == TileCollisionShape.Solid ||
					collision == TileCollisionShape.Free;

				// Draw the center icon indicating the tiles simple solid / free state, as well as diagonal slopes
				{
					bool centerIsCollision = 
						collision == TileCollisionShape.Solid ||
						collision.HasFlag(TileCollisionShape.DiagonalUp) ||
						collision.HasFlag(TileCollisionShape.DiagonalDown);

					Bitmap centerImage;
					if      (collision == TileCollisionShape.Solid)              centerImage = TilemapsResCache.TilesetCollisionBit;
					else if (collision.HasFlag(TileCollisionShape.DiagonalUp))   centerImage = TilemapsResCache.TilesetCollisionDiagUp;
					else if (collision.HasFlag(TileCollisionShape.DiagonalDown)) centerImage = TilemapsResCache.TilesetCollisionDiagDown;
					else                                                         centerImage = TilemapsResCache.TilesetCollisionBit;
					
					Color centerColor;
					if (centerIsCollision) centerColor = colorCollision;
					else                   centerColor = colorFree;

					e.Graphics.DrawImageTint(
						centerImage, 
						Color.FromArgb(GetCollisionIconAlpha(
							tileHovered, 
							this.hoveredArea == TileHotSpot.Center, 
							centerIsCollision), centerColor), 
						paintData.ViewRect.X + (paintData.ViewRect.Width - centerImage.Width) / 2,
						paintData.ViewRect.Y + (paintData.ViewRect.Height - centerImage.Height) / 2);
				}

				// Draw collision icons for specific directional passability.
				if (!simpleCollision || (tileHovered && (!this.isUserDrawing || !this.drawSimple)))
				{
					e.Graphics.DrawImageTint(
						TilemapsResCache.TilesetCollisionVertical, 
						Color.FromArgb(GetCollisionIconAlpha(
							tileHovered, 
							this.hoveredArea == TileHotSpot.Right, 
							!simpleCollision && collision.HasFlag(TileCollisionShape.Right)), 
							collision.HasFlag(TileCollisionShape.Right) ? colorCollision : colorFree), 
						paintData.ViewRect.X + paintData.ViewRect.Width - TilemapsResCache.TilesetCollisionVertical.Width - 1,
						paintData.ViewRect.Y + (paintData.ViewRect.Height - TilemapsResCache.TilesetCollisionVertical.Height) / 2);
					e.Graphics.DrawImageTint(
						TilemapsResCache.TilesetCollisionVertical, 
						Color.FromArgb(GetCollisionIconAlpha(
							tileHovered, 
							this.hoveredArea == TileHotSpot.Left, 
							!simpleCollision && collision.HasFlag(TileCollisionShape.Left)), 
							collision.HasFlag(TileCollisionShape.Left) ? colorCollision : colorFree), 
						paintData.ViewRect.X + 1,
						paintData.ViewRect.Y + (paintData.ViewRect.Height - TilemapsResCache.TilesetCollisionVertical.Height) / 2);
					e.Graphics.DrawImageTint(
						TilemapsResCache.TilesetCollisionHorizontal, 
						Color.FromArgb(GetCollisionIconAlpha(
							tileHovered, 
							this.hoveredArea == TileHotSpot.Top, 
							!simpleCollision && collision.HasFlag(TileCollisionShape.Top)), 
							collision.HasFlag(TileCollisionShape.Top) ? colorCollision : colorFree), 
						paintData.ViewRect.X + (paintData.ViewRect.Width - TilemapsResCache.TilesetCollisionHorizontal.Width) / 2,
						paintData.ViewRect.Y + 1);
					e.Graphics.DrawImageTint(
						TilemapsResCache.TilesetCollisionHorizontal, 
						Color.FromArgb(GetCollisionIconAlpha(
							tileHovered, 
							this.hoveredArea == TileHotSpot.Bottom, 
							!simpleCollision && collision.HasFlag(TileCollisionShape.Bottom)), 
							collision.HasFlag(TileCollisionShape.Bottom) ? colorCollision : colorFree), 
						paintData.ViewRect.X + (paintData.ViewRect.Width - TilemapsResCache.TilesetCollisionHorizontal.Width) / 2,
						paintData.ViewRect.Y + paintData.ViewRect.Height - TilemapsResCache.TilesetCollisionHorizontal.Height - 1);
				}
			}
		}
		private void TilesetView_MouseMove(object sender, MouseEventArgs e)
		{
			Size tileSize = this.TilesetView.DisplayedTileSize;
			Point tilePos = this.TilesetView.GetTileIndexLocation(this.TilesetView.HoveredTileIndex);
			Point posOnTile = new Point(e.X - tilePos.X, e.Y - tilePos.Y);
			Size centerSize = new Size(
				tileSize.Width - TilemapsResCache.TilesetCollisionVertical.Width * 2 - 2,
				tileSize.Height - TilemapsResCache.TilesetCollisionHorizontal.Height * 2 - 2);

			// Determine the hovered tile hotspot for user interaction
			TileHotSpot lastHoveredArea = this.hoveredArea;
			if (posOnTile.X > (tileSize.Width - centerSize.Width) / 2 &&
				posOnTile.Y > (tileSize.Height - centerSize.Height) / 2 &&
				posOnTile.X < (tileSize.Width + centerSize.Width) / 2 &&
				posOnTile.Y < (tileSize.Height + centerSize.Height) / 2)
			{
				this.hoveredArea = TileHotSpot.Center;
			}
			else
			{
				float angle = MathF.Angle(tileSize.Width / 2, tileSize.Height / 2, posOnTile.X, posOnTile.Y);
				if      (MathF.CircularDist(angle, 0.0f             ) < MathF.RadAngle45) this.hoveredArea = TileHotSpot.Top;
				else if (MathF.CircularDist(angle, MathF.RadAngle90 ) < MathF.RadAngle45) this.hoveredArea = TileHotSpot.Right;
				else if (MathF.CircularDist(angle, MathF.RadAngle180) < MathF.RadAngle45) this.hoveredArea = TileHotSpot.Bottom;
				else                                                                      this.hoveredArea = TileHotSpot.Left;
			}
			
			// If the user is in the process of setting or clearing bits, perform the drawing operation
			if (this.isUserDrawing)
				this.PerformUserDrawAction();

			if (lastHoveredArea != this.hoveredArea)
				this.TilesetView.InvalidateTile(this.TilesetView.HoveredTileIndex, 0);
		}
		private void TilesetView_MouseUp(object sender, MouseEventArgs e)
		{
			this.isUserDrawing = false;
			this.TilesetView.InvalidateTile(this.TilesetView.HoveredTileIndex, 0);
		}
		private void TilesetView_MouseDown(object sender, MouseEventArgs e)
		{
			Tileset tileset = this.SelectedTileset.Res;
			if (tileset == null) return;

			int tileIndex = this.TilesetView.HoveredTileIndex;
			if (tileIndex < 0 || tileIndex > tileset.TileCount) return;

			// Conditional toggle operation on left click
			if (e.Button == MouseButtons.Left)
			{
				TileInput input = tileset.TileInput.Count > tileIndex ? tileset.TileInput[tileIndex] : default(TileInput);
				TileCollisionShape collision = input.Collision[this.editLayerIndex];
				this.drawSimple = false;
				switch (this.hoveredArea)
				{
					case TileHotSpot.Left:
						this.drawShape = TileCollisionShape.Left;
						this.drawMode = !collision.HasFlag(TileCollisionShape.Left) ? CollisionDrawMode.Add : CollisionDrawMode.Remove;
						break;
					case TileHotSpot.Right:
						this.drawShape = TileCollisionShape.Right;
						this.drawMode = !collision.HasFlag(TileCollisionShape.Right) ? CollisionDrawMode.Add : CollisionDrawMode.Remove;
						break;
					case TileHotSpot.Top:
						this.drawShape = TileCollisionShape.Top;
						this.drawMode = !collision.HasFlag(TileCollisionShape.Top) ? CollisionDrawMode.Add : CollisionDrawMode.Remove;
						break;
					case TileHotSpot.Bottom:
						this.drawShape = TileCollisionShape.Bottom;
						this.drawMode = !collision.HasFlag(TileCollisionShape.Bottom) ? CollisionDrawMode.Add : CollisionDrawMode.Remove;
						break;
					default:
						if (collision == TileCollisionShape.Free)
						{
							this.drawSimple = true;
							this.drawMode = CollisionDrawMode.Set;
							this.drawShape = TileCollisionShape.Solid;
						}
						else if (collision == TileCollisionShape.Solid)
						{
							this.drawMode = CollisionDrawMode.Set;
							this.drawShape = TileCollisionShape.DiagonalUp;
						}
						else if (collision.HasFlag(TileCollisionShape.DiagonalUp))
						{
							this.drawMode = CollisionDrawMode.Set;
							this.drawShape = TileCollisionShape.DiagonalDown;
						}
						else if (collision.HasFlag(TileCollisionShape.DiagonalDown))
						{
							this.drawMode = CollisionDrawMode.Set;
							this.drawShape = TileCollisionShape.Free;
						}
						else
						{
							this.drawMode = CollisionDrawMode.Set;
							this.drawShape = TileCollisionShape.Solid;
						}
						break;
				}
				this.isUserDrawing = true;
			}
			// Clear operation on right click
			else if (e.Button == MouseButtons.Right)
			{
				this.drawSimple = true;
				this.drawShape = TileCollisionShape.Free;
				this.drawMode = CollisionDrawMode.Set;
				this.isUserDrawing = true;
			}

			// Perform the drawing operation
			this.PerformUserDrawAction();
			this.TilesetView.InvalidateTile(tileIndex, 0);
		}

		private void PerformUserDrawAction()
		{
			Tileset tileset = this.SelectedTileset.Res;
			if (tileset == null) return;

			int tileIndex = this.TilesetView.HoveredTileIndex;
			if (tileIndex < 0 || tileIndex > tileset.TileCount) return;

			TileInput input = tileset.TileInput.Count > tileIndex ? tileset.TileInput[tileIndex] : default(TileInput);
			TileCollisionShape lastCollision = input.Collision[this.editLayerIndex];

			if (this.drawMode == CollisionDrawMode.Add)
				input.Collision[this.editLayerIndex] |= this.drawShape;
			else if (this.drawMode == CollisionDrawMode.Remove)
				input.Collision[this.editLayerIndex] &= ~this.drawShape;
			else
				input.Collision[this.editLayerIndex] = this.drawShape;

			TileCollisionShape newCollision = input.Collision[this.editLayerIndex];
			if (lastCollision != newCollision)
			{
				UndoRedoManager.Do(new EditTilesetTileInputAction(tileset, tileIndex, input));
			}
		}

		private static int GetCollisionIconAlpha(bool tileHovered, bool areaHovered, bool isActive)
		{
			if (tileHovered && areaHovered) return 255;
			else if (isActive)              return 192;
			else if (tileHovered)           return 128;
			else                            return 64;
		}
	}
}
