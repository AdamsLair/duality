using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;

using Aga.Controls.Tree;

using Duality.Plugins.Tilemaps;
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
			Brush brushNonConnected = new SolidBrush(Color.FromArgb(128, 0, 0, 0));
			TilesetAutoTileInput autoTile = this.SelectedAutoTile;
			
			// Draw a "disabled" overlay if there is nothing we can edit right now
			if (autoTile == null)
			{
				e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(128, this.TilesetView.BackColor)), e.ClipRectangle);
				return;
			}

			TilesetAutoTileItem[] tileInput = autoTile.TileInput.Data;
			for (int i = 0; i < e.PaintedTiles.Count; i++)
			{
				TilesetViewPaintTileData paintData = e.PaintedTiles[i];

				// Prepare some data we'll need for drawing the per-tile info overlay
				bool tileHovered = this.TilesetView.HoveredTileIndex == paintData.TileIndex;
				TilesetAutoTileItem item = (autoTile.TileInput.Count > paintData.TileIndex) ? 
					tileInput[paintData.TileIndex] : 
					default(TilesetAutoTileItem);

				// If the tile is not part of this AutoTile definition, overlay it and continue
				if (!item.IsAutoTile)
				{
					e.Graphics.FillRectangle(
						brushNonConnected, 
						paintData.ViewRect);
					continue;
				}

				// Display the tile's connectivity state via overlay
				foreach (TileConnection neighbour in Neighbourhood)
				{
					if (!item.Neighbours.HasFlag(neighbour))
						e.Graphics.FillRectangle(
							brushNonConnected, 
							GetConnectivityDrawRect(neighbour, paintData.ViewRect));
				}
			}
		}
		private void TilesetView_MouseMove(object sender, MouseEventArgs e)
		{
			Size tileSize = this.TilesetView.DisplayedTileSize;
			Point tilePos = this.TilesetView.GetTileIndexLocation(this.TilesetView.HoveredTileIndex);
			Point posOnTile = new Point(e.X - tilePos.X, e.Y - tilePos.Y);

			// Determine the hovered tile hotspot for user interaction
			TileConnection lastHoveredArea = this.hoveredArea;
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

			TilesetAutoTileItem lastInput = (autoTile.TileInput.Count > tileIndex) ? 
				autoTile.TileInput[tileIndex] : 
				default(TilesetAutoTileItem);
			TilesetAutoTileItem newInput = lastInput;
			if (this.userDrawMode == AutoTileDrawMode.AddConnection)
			{
				newInput.Neighbours |= this.hoveredArea;
				newInput.IsAutoTile = true;
			}
			else if (this.userDrawMode == AutoTileDrawMode.RemoveConnection)
			{ 
				newInput.Neighbours &= ~this.hoveredArea;
				newInput.IsAutoTile = (newInput.Neighbours != TileConnection.None);
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
