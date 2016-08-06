using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;

using Aga.Controls.Tree;

using Duality.Plugins.Tilemaps;
using Duality.Editor.UndoRedoActions;
using Duality.Editor.Plugins.Tilemaps;
using Duality.Editor.Plugins.Tilemaps.Properties;
using Duality.Editor.Plugins.Tilemaps.UndoRedoActions;

namespace Duality.Editor.Plugins.Tilemaps.TilesetEditorModes
{
	/// <summary>
	/// Allows to edit a Tileset's AutoTile layers, which define how similar adjacent tiles join together.
	/// </summary>
	public class AutoTileTilesetEditorMode : TilesetEditorMode
	{
		/// <summary>
		/// Describes how the current user drawing operation will affect existing AutoTile data.
		/// </summary>
		private enum AutoTileDrawMode
		{
			AddConnection,
			RemoveConnection
		}
		private class AutoTileInputNode : TilesetEditorLayerNode
		{
			private TilesetAutoTileInput autoTile = null;

			public override string Title
			{
				get { return this.autoTile.Name; }
			}
			public override string Description
			{
				get { return this.autoTile.Id; }
			}
			public TilesetAutoTileInput AutoTileInput
			{
				get { return this.autoTile; }
			}

			public AutoTileInputNode(TilesetAutoTileInput autoTile) : base()
			{
				this.autoTile = autoTile;
				this.Image = Properties.TilemapsResCache.IconTilesetAutoTileLayer;
			}

			public void Invalidate()
			{
				this.NotifyModel();
			}
		}


		private static TileConnection[] Neighbourhood = new TileConnection[]
		{
			TileConnection.TopLeft,
			TileConnection.Top,
			TileConnection.TopRight,
			TileConnection.Left,
			TileConnection.Right,
			TileConnection.BottomLeft,
			TileConnection.Bottom,
			TileConnection.BottomRight
		};

		private	TreeModel        treeModel      = new TreeModel();
		private TileConnection   hoveredArea    = TileConnection.None;
		private bool             isUserDrawing  = false;
		private bool             isBaseTileDraw = false;
		private AutoTileDrawMode userDrawMode   = AutoTileDrawMode.AddConnection;


		public override string Id
		{
			get { return "AutoTileTilesetEditorMode"; }
		}
		public override string Name
		{
			get { return TilemapsRes.TilesetEditorMode_AutoTile_Name; }
		}
		public override Image Icon
		{
			get { return TilemapsResCache.IconTilesetAutoTile; }
		}
		public override int SortOrder
		{
			get { return -30; }
		}
		public override ITreeModel LayerModel
		{
			get { return this.treeModel; }
		}
		public override LayerEditingCaps AllowLayerEditing
		{
			get { return LayerEditingCaps.All; }
		}
		protected TilesetAutoTileInput SelectedAutoTile
		{
			get { return DualityEditorApp.Selection.Objects.OfType<TilesetAutoTileInput>().FirstOrDefault(); }
		}


		private void UpdateTreeModel()
		{
			Tileset tileset = this.SelectedTileset.Res;

			// If the tileset is unavailable, or none is selected, there are no nodes
			if (tileset == null)
			{
				this.treeModel.Nodes.Clear();
				return;
			}

			// Remove nodes that no longer have an equivalent in the Tileset
			foreach (AutoTileInputNode node in this.treeModel.Nodes.ToArray())
			{
				if (!tileset.AutoTileConfig.Contains(node.AutoTileInput))
				{ 
					this.treeModel.Nodes.Remove(node);
				}
			}

			// Notify the model of changes for the existing nodes to account for name and id changes.
			// This is only okay because we have so few notes in total. Don't do this for actual data.
			foreach (AutoTileInputNode node in this.treeModel.Nodes)
			{
				node.Invalidate();
			}

			// Add nodes that don't have a corresponding tree model node yet
			foreach (TilesetAutoTileInput autoTile in tileset.AutoTileConfig)
			{
				if (!this.treeModel.Nodes.Any(node => (node as AutoTileInputNode).AutoTileInput == autoTile))
				{
					this.treeModel.Nodes.Add(new AutoTileInputNode(autoTile));
				}
			}
		}

		/// <inheritdoc />
		public override void AddLayer()
		{
			base.AddLayer();
			Tileset tileset = this.SelectedTileset.Res;
			if (tileset == null) return;
			
			// Decide upon an id for our new layer
			string autoTileId = TilesetAutoTileInput.DefaultId + (tileset.AutoTileConfig.Count).ToString();
			string autoTileName = TilesetAutoTileInput.DefaultName;

			// Create a new AutoTile using an UndoRedo action
			TilesetAutoTileInput newAutoTile = new TilesetAutoTileInput
			{
				Id = autoTileId,
				Name = autoTileName
			};
			UndoRedoManager.Do(new AddTilesetConfigLayerAction<TilesetAutoTileInput>(
				tileset, 
				TilemapsReflectionInfo.Property_Tileset_AutoTileConfig,
				newAutoTile));

			// Select the newly created AutoTile
			AutoTileInputNode modelNode = this.treeModel
				.Nodes
				.OfType<AutoTileInputNode>()
				.FirstOrDefault(n => n.AutoTileInput == newAutoTile);
			this.SelectLayer(modelNode);
		}
		/// <inheritdoc />
		public override void RemoveLayer()
		{
			base.RemoveLayer();

			Tileset tileset = this.SelectedTileset.Res;
			TilesetAutoTileInput autoTile = this.SelectedAutoTile;
			if (tileset == null) return;
			if (autoTile == null) return;

			UndoRedoManager.Do(new RemoveTilesetConfigLayerAction<TilesetAutoTileInput>(
				tileset, 
				TilemapsReflectionInfo.Property_Tileset_AutoTileConfig,
				autoTile));
		}
		
		protected override void OnEnter()
		{
			this.UpdateTreeModel();
			this.TilesetView.PaintTiles += this.TilesetView_PaintTiles;
			this.TilesetView.MouseMove += this.TilesetView_MouseMove;
			this.TilesetView.MouseDown += this.TilesetView_MouseDown;
			this.TilesetView.MouseUp += this.TilesetView_MouseUp;
		}
		protected override void OnLeave()
		{
			base.OnLeave();
			this.TilesetView.PaintTiles -= this.TilesetView_PaintTiles;
			this.TilesetView.MouseMove -= this.TilesetView_MouseMove;
			this.TilesetView.MouseDown -= this.TilesetView_MouseDown;
			this.TilesetView.MouseUp -= this.TilesetView_MouseUp;
		}
		protected override void OnTilesetSelectionChanged(TilesetSelectionChangedEventArgs args)
		{
			this.UpdateTreeModel();
		}
		protected override void OnTilesetModified(ObjectPropertyChangedEventArgs args)
		{
			Tileset tileset = this.SelectedTileset.Res;

			// If an AutoTile was modified, emit an editor-wide change event for
			// the Tileset as well, so the editor knows it will need to save this Resource.
			if (tileset != null && args.HasAnyObject(tileset.AutoTileConfig))
			{
				DualityEditorApp.NotifyObjPropChanged(
					this, 
					new ObjectSelection(tileset),
					TilemapsReflectionInfo.Property_Tileset_AutoTileConfig);
			}

			this.UpdateTreeModel();
		}
		protected override void OnLayerSelectionChanged(LayerSelectionChangedEventArgs args)
		{
			base.OnLayerSelectionChanged(args);
			Tileset tileset = this.SelectedTileset.Res;
			AutoTileInputNode selectedNode = args.SelectedNodeTag as AutoTileInputNode;

			// Update global editor selection, so an Object Inspector can pick up the AutoTile for editing
			if (selectedNode != null)
				DualityEditorApp.Select(this, new ObjectSelection(new object[] { selectedNode.AutoTileInput }));
			else
				DualityEditorApp.Deselect(this, obj => obj is TilesetAutoTileInput);

			this.TilesetView.Invalidate();
		}
		
		private void TilesetView_PaintTiles(object sender, TilesetViewPaintTilesEventArgs e)
		{
			Color hoverHighlightColor = Color.FromArgb(255, 255, 255);
			Color nonConnectedColor = Color.FromArgb(128, 0, 0, 0);
			Brush brushNonConnected = new SolidBrush(nonConnectedColor);
			TilesetAutoTileInput autoTile = this.SelectedAutoTile;
			
			// Early-out if there is nothing we can edit right now
			if (autoTile == null)
				return;

			TilesetViewPaintTileData hoveredData = default(TilesetViewPaintTileData);
			TilesetAutoTileItem hoveredItem = default(TilesetAutoTileItem);
			Region connectedRegion = new Region();
			connectedRegion.MakeEmpty();
			TilesetAutoTileItem[] tileInput = autoTile.TileInput.Data;
			for (int i = 0; i < e.PaintedTiles.Count; i++)
			{
				TilesetViewPaintTileData paintData = e.PaintedTiles[i];

				// Prepare some data we'll need for drawing the per-tile info overlay
				bool tileHovered = this.TilesetView.HoveredTileIndex == paintData.TileIndex;
				bool isBaseTile = autoTile.BaseTileIndex == paintData.TileIndex;
				TilesetAutoTileItem item = (autoTile.TileInput.Count > paintData.TileIndex) ? 
					tileInput[paintData.TileIndex] : 
					default(TilesetAutoTileItem);

				// Remember hovered item data for later (post-overlay)
				if (tileHovered)
				{
					hoveredData = paintData;
					hoveredItem = item;
				}

				// Display the tile's connectivity state by saving connected edges from the overlay
				if (item.IsAutoTile)
				{
					Rectangle centerRect = GetConnectivityDrawRect(TileConnection.None, paintData.ViewRect);
					connectedRegion.Union(centerRect);
					foreach (TileConnection neighbour in Neighbourhood)
					{
						if (item.Neighbours.HasFlag(neighbour))
						{
							Rectangle borderRect = GetConnectivityDrawRect(neighbour, paintData.ViewRect);
							connectedRegion.Union(borderRect);
						}
					}
				}

				// Highlight the base tile
				if (isBaseTile)
				{
					Rectangle rect = paintData.ViewRect;
					rect.Width -= 1;
					rect.Height -= 1;
					e.Graphics.DrawRectangle(new Pen(Color.FromArgb(255, 255, 192, 128)), rect);
					rect.Inflate(-1, -1);
					e.Graphics.DrawRectangle(new Pen(Color.FromArgb(128, 255, 192, 128)), rect);
					rect.Inflate(-1, -1);
					e.Graphics.DrawRectangle(new Pen(Color.FromArgb(64, 255, 192, 128)), rect);
				}
			}

			// Fill all non-connected regions with the overlay brush
			Region surroundingRegion = new Region();
			surroundingRegion.MakeInfinite();
			surroundingRegion.Exclude(connectedRegion);
			e.Graphics.IntersectClip(surroundingRegion);
			e.Graphics.FillRectangle(brushNonConnected, this.TilesetView.ClientRectangle);
			e.Graphics.ResetClip();

			// Draw a hover indicator
			if (!hoveredData.ViewRect.IsEmpty)
			{
				Rectangle rect;
				if (autoTile.BaseTileIndex != -1 && autoTile.BaseTileIndex != hoveredData.TileIndex)
					rect = GetConnectivityDrawRect(this.hoveredArea, hoveredData.ViewRect);
				else
					rect = hoveredData.ViewRect;

				rect.Width -= 1;
				rect.Height -= 1;
				e.Graphics.DrawRectangle(Pens.Black, rect);
				rect.Inflate(-1, -1);
				e.Graphics.DrawRectangle(new Pen(hoverHighlightColor), rect);
				rect.Inflate(-1, -1);
				e.Graphics.DrawRectangle(Pens.Black, rect);
			}
		}
		private void TilesetView_MouseMove(object sender, MouseEventArgs e)
		{
			Size tileSize = this.TilesetView.DisplayedTileSize;
			Point tilePos = this.TilesetView.GetTileIndexLocation(this.TilesetView.HoveredTileIndex);
			Point posOnTile = new Point(e.X - tilePos.X, e.Y - tilePos.Y);
			Size centerSize = new Size(tileSize.Width / 2, tileSize.Height / 2);

			// Determine the hovered tile hotspot for user interaction
			TileConnection lastHoveredArea = this.hoveredArea;
			if (posOnTile.X > (tileSize.Width - centerSize.Width) / 2 &&
				posOnTile.Y > (tileSize.Height - centerSize.Height) / 2 &&
				posOnTile.X < (tileSize.Width + centerSize.Width) / 2 &&
				posOnTile.Y < (tileSize.Height + centerSize.Height) / 2)
			{
				this.hoveredArea = TileConnection.None;
			}
			else
			{
				float angle = MathF.Angle(tileSize.Width / 2, tileSize.Height / 2, posOnTile.X, posOnTile.Y);
				float threshold = MathF.DegToRad(22.5f);
				if      (MathF.CircularDist(angle, MathF.DegToRad(315.0f)) <= threshold) this.hoveredArea = TileConnection.TopLeft;
				else if (MathF.CircularDist(angle, MathF.DegToRad(  0.0f)) <= threshold) this.hoveredArea = TileConnection.Top;
				else if (MathF.CircularDist(angle, MathF.DegToRad( 45.0f)) <= threshold) this.hoveredArea = TileConnection.TopRight;
				else if (MathF.CircularDist(angle, MathF.DegToRad(270.0f)) <= threshold) this.hoveredArea = TileConnection.Left;
				else if (MathF.CircularDist(angle, MathF.DegToRad( 90.0f)) <= threshold) this.hoveredArea = TileConnection.Right;
				else if (MathF.CircularDist(angle, MathF.DegToRad(225.0f)) <= threshold) this.hoveredArea = TileConnection.BottomLeft;
				else if (MathF.CircularDist(angle, MathF.DegToRad(180.0f)) <= threshold) this.hoveredArea = TileConnection.Bottom;
				else                                                                     this.hoveredArea = TileConnection.BottomRight;
			}

			// Update action state
			TilesetAutoTileInput autoTile = this.SelectedAutoTile;
			if (autoTile != null)
			{
				this.isBaseTileDraw = 
					autoTile.BaseTileIndex == -1 || 
					autoTile.BaseTileIndex == this.TilesetView.HoveredTileIndex;
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
			
			TilesetAutoTileInput autoTile = this.SelectedAutoTile;
			if (autoTile == null) return;

			int tileIndex = this.TilesetView.HoveredTileIndex;
			if (tileIndex < 0 || tileIndex > tileset.TileCount) return;

			// Draw operation on left click
			if (e.Button == MouseButtons.Left)
			{
				this.isUserDrawing = true;
				this.userDrawMode = AutoTileDrawMode.AddConnection;
			}
			// Clear operation on right click
			else if (e.Button == MouseButtons.Right)
			{
				this.isUserDrawing = true;
				this.userDrawMode = AutoTileDrawMode.RemoveConnection;
			}

			// Perform the drawing operation
			this.PerformUserDrawAction();
			this.TilesetView.InvalidateTile(tileIndex, 0);
		}

		private void PerformUserDrawAction()
		{
			Tileset tileset = this.SelectedTileset.Res;
			if (tileset == null) return;

			TilesetAutoTileInput autoTile = this.SelectedAutoTile;
			if (autoTile == null) return;

			int tileIndex = this.TilesetView.HoveredTileIndex;
			if (tileIndex < 0 || tileIndex > tileset.TileCount) return;
			
			// Determine data before the operation, and set up data for afterwards
			bool lastIsBaseTile = autoTile.BaseTileIndex == tileIndex;
			bool newIsBaseTile = lastIsBaseTile;
			TilesetAutoTileItem lastInput = (autoTile.TileInput.Count > tileIndex) ? 
				autoTile.TileInput[tileIndex] : 
				default(TilesetAutoTileItem);
			TilesetAutoTileItem newInput = lastInput;

			// Determine how data is modified due to our user operation
			if (this.userDrawMode == AutoTileDrawMode.AddConnection)
			{
				if (this.isBaseTileDraw)
				{
					newInput.Neighbours = TileConnection.All;
					newInput.IsAutoTile = true;
					newIsBaseTile = true;
				}
				else
				{
					newInput.Neighbours |= this.hoveredArea;
					newInput.IsAutoTile = true;
				}
			}
			else if (this.userDrawMode == AutoTileDrawMode.RemoveConnection)
			{
				if (this.isBaseTileDraw)
				{
					newInput.Neighbours = TileConnection.None;
					newInput.IsAutoTile = false;
					newIsBaseTile = false;
				}
				else if (this.hoveredArea == TileConnection.None)
				{
					newInput.Neighbours = TileConnection.None;
					newInput.IsAutoTile = false;
				}
				else
				{
					newInput.Neighbours &= ~this.hoveredArea;
					newInput.IsAutoTile = (newInput.Neighbours != TileConnection.None);
				}
			}
			
			// Apply the new, modified data to the actual data using an UndoRedo operation
			if (newIsBaseTile != lastIsBaseTile)
			{
				UndoRedoManager.Do(new EditPropertyAction(
					null,
					TilemapsReflectionInfo.Property_TilesetAutoTileInput_BaseTile,
					new object[] { autoTile }, 
					new object[] { newIsBaseTile ? tileIndex : -1 }));
			}
			if (!object.Equals(lastInput, newInput))
			{
				UndoRedoManager.Do(new EditTilesetAutoTileItemAction(
					tileset, 
					autoTile, 
					tileIndex, 
					newInput));
			}
		}

		private static Rectangle GetConnectivityDrawRect(TileConnection connectivity, Rectangle baseRect)
		{
			Size borderSize = new Size(
				baseRect.Width / 4,
				baseRect.Height / 4);
			switch (connectivity)
			{
				default:
				case TileConnection.All: return 
					baseRect;
				case TileConnection.TopLeft: return new Rectangle(
					baseRect.X,
					baseRect.Y,
					borderSize.Width,
					borderSize.Height);
				case TileConnection.Top: return new Rectangle(
					baseRect.X + borderSize.Width,
					baseRect.Y,
					baseRect.Width - borderSize.Width * 2,
					borderSize.Height);
				case TileConnection.TopRight: return new Rectangle(
					baseRect.X + baseRect.Width - borderSize.Width,
					baseRect.Y,
					borderSize.Width,
					borderSize.Height);
				case TileConnection.Left: return new Rectangle(
					baseRect.X,
					baseRect.Y + borderSize.Height,
					borderSize.Width,
					baseRect.Height - borderSize.Height * 2);
				case TileConnection.None: return new Rectangle(
					baseRect.X + borderSize.Width,
					baseRect.Y + borderSize.Height,
					baseRect.Width - borderSize.Width * 2,
					baseRect.Height - borderSize.Height * 2);
				case TileConnection.Right: return new Rectangle(
					baseRect.X + baseRect.Width - borderSize.Width,
					baseRect.Y + borderSize.Height,
					borderSize.Width,
					baseRect.Height - borderSize.Height * 2);
				case TileConnection.BottomLeft: return new Rectangle(
					baseRect.X,
					baseRect.Y + baseRect.Height - borderSize.Height,
					borderSize.Width,
					borderSize.Height);
				case TileConnection.Bottom: return new Rectangle(
					baseRect.X + borderSize.Width,
					baseRect.Y + baseRect.Height - borderSize.Height,
					baseRect.Width - borderSize.Width * 2,
					borderSize.Height);
				case TileConnection.BottomRight: return new Rectangle(
					baseRect.X + baseRect.Width - borderSize.Width,
					baseRect.Y + baseRect.Height - borderSize.Height,
					borderSize.Width,
					borderSize.Height);
			}
		}
	}
}
