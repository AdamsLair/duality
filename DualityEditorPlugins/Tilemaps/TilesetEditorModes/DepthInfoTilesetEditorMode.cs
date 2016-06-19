using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

using Aga.Controls.Tree;

using Duality.Plugins.Tilemaps;
using Duality.Editor.Plugins.Tilemaps;
using Duality.Editor.Plugins.Tilemaps.UndoRedoActions;
using Duality.Editor.Plugins.Tilemaps.Properties;

namespace Duality.Editor.Plugins.Tilemaps.TilesetEditorModes
{
	/// <summary>
	/// Allows to edit the depth information of each tile, which can be used for Z-Offset generation and Z sorting during rendering.
	/// </summary>
	public class DepthInfoTilesetEditorMode : TilesetEditorMode
	{
		private enum EditMode
		{
			Offset,
			Vertical
		}
		private class DepthInfoLayerNode : TilesetEditorLayerNode
		{
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

			public DepthInfoLayerNode(string name, string description, Image image) : base()
			{
				this.name = name;
				this.desc = description;
				this.Image = image;
			}
		}


		private TreeModel          treeModel             = new TreeModel();
		private DepthInfoLayerNode treeNodeOffsetLayer   = null;
		private DepthInfoLayerNode treeNodeVerticalLayer = null;
		private EditMode           editMode              = EditMode.Offset;
		private int                drawDepthOffset       = 0;
		private bool               drawVertical          = false;
		private bool               isUserDrawing         = false;

		private Dictionary<string,Bitmap> textCache = new Dictionary<string,Bitmap>();


		public override string Id
		{
			get { return "DepthInfoTilesetEditorMode"; }
		}
		public override string Name
		{
			get { return TilemapsRes.TilesetEditorMode_DepthInfo_Name; }
		}
		public override Image Icon
		{
			get { return TilemapsResCache.IconTilesetDepthInfo; }
		}
		public override int SortOrder
		{
			get { return -80; }
		}
		public override ITreeModel LayerModel
		{
			get { return this.treeModel; }
		}


		public DepthInfoTilesetEditorMode()
		{
			this.treeNodeOffsetLayer = new DepthInfoLayerNode(
				TilemapsRes.TilesetEditorDepthOffset_Name, 
				TilemapsRes.TilesetEditorDepthOffset_Desc,
				TilemapsResCache.IconTilesetDepthLayer);
			this.treeNodeVerticalLayer = new DepthInfoLayerNode(
				TilemapsRes.TilesetEditorDepthVertical_Name, 
				TilemapsRes.TilesetEditorDepthVertical_Desc,
				TilemapsResCache.IconTilesetDepthVertical);
			this.treeModel.Nodes.Add(this.treeNodeOffsetLayer);
			this.treeModel.Nodes.Add(this.treeNodeVerticalLayer);
		}

		private Bitmap GetDepthOverlayBitmap(string text)
		{
			Font font = this.TilesetView.Font;
			float outlineWidth = 3;
			
			StringFormat textFormat = StringFormat.GenericTypographic;
			textFormat.Alignment = StringAlignment.Center;
			textFormat.LineAlignment = StringAlignment.Center;

			Bitmap bitmap;
			if (!this.textCache.TryGetValue(text, out bitmap))
			{
				bitmap = new Bitmap(Tileset.DefaultTileSize.X, Tileset.DefaultTileSize.Y);

				using (Graphics g = Graphics.FromImage(bitmap))
				using (GraphicsPath path = new GraphicsPath())
				using (Brush brush = new SolidBrush(Color.White))
				using (Brush penBrush = new SolidBrush(Color.Black))
				using (Pen pen = new Pen(penBrush, outlineWidth))
				{
					path.AddString(
						text, 
						font.FontFamily, 
						(int)font.Style, 
						font.Size * 2, 
						new Rectangle(0, 0, bitmap.Width, bitmap.Height), 
						textFormat);

					g.SmoothingMode = SmoothingMode.HighQuality;
					g.DrawPath(pen, path);                
					g.FillPath(brush, path);                            
					g.SmoothingMode = SmoothingMode.Default;
				}

				this.textCache.Add(text, bitmap);
			}
			return bitmap;
		}

		protected override void OnEnter()
		{
			base.OnEnter();
			this.SelectLayer(this.treeNodeOffsetLayer);
			this.TilesetView.PaintTiles += this.TilesetView_PaintTiles;
			this.TilesetView.MouseDown += this.TilesetView_MouseDown;
			this.TilesetView.MouseUp += this.TilesetView_MouseUp;
			this.TilesetView.KeyDown += this.TilesetView_KeyDown;
			this.TilesetView.HoveredTileChanged += this.TilesetView_HoveredTileChanged;
		}
		protected override void OnLeave()
		{
			base.OnLeave();
			this.TilesetView.PaintTiles -= this.TilesetView_PaintTiles;
			this.TilesetView.MouseDown -= this.TilesetView_MouseDown;
			this.TilesetView.MouseUp -= this.TilesetView_MouseUp;
			this.TilesetView.KeyDown -= this.TilesetView_KeyDown;
			this.TilesetView.HoveredTileChanged -= this.TilesetView_HoveredTileChanged;
			this.textCache.Clear();
		}
		protected override void OnLayerSelectionChanged(LayerSelectionChangedEventArgs args)
		{
			base.OnLayerSelectionChanged(args);

			if (args.SelectedNodeTag == this.treeNodeOffsetLayer)
				this.editMode = EditMode.Offset;
			else if (args.SelectedNodeTag == this.treeNodeVerticalLayer)
				this.editMode = EditMode.Vertical;
			else
				this.SelectLayer(this.treeNodeOffsetLayer);

			this.TilesetView.Invalidate();
		}

		private void TilesetView_PaintTiles(object sender, TilesetViewPaintTilesEventArgs e)
		{
			TileInput[] tileInput = e.Tileset.TileInput.Data;
			for (int i = 0; i < e.PaintedTiles.Count; i++)
			{
				TilesetViewPaintTileData paintData = e.PaintedTiles[i];
				int depthOffset = 0;
				bool isVertical = false;
				if (tileInput.Length > paintData.TileIndex)
				{
					depthOffset = tileInput[paintData.TileIndex].DepthOffset;
					isVertical = tileInput[paintData.TileIndex].IsVertical;
				}

				Bitmap overlayBitmap;
				int overlayAlpha;
				Color overlayColor;

				// Retrieve an overlay text bitmap that represents depth offset
				if (this.editMode == EditMode.Offset)
				{
					string text = depthOffset.ToString();
					overlayBitmap = this.GetDepthOverlayBitmap(text);

					// Determine color and alpha for the text overlay
					overlayAlpha = 
						(this.TilesetView.HoveredTileIndex == paintData.TileIndex ? 255 : 
						(depthOffset != 0 ? 192 : 
						64));
					overlayColor = 
						(depthOffset > 0 ? Color.FromArgb(192, 255, 128) : 
						(depthOffset < 0 ? Color.FromArgb(255, 128, 128) : 
						Color.FromArgb(255, 255, 255)));
				}
				// Use the appropriate icon for vertical / flat tiles
				else
				{
					overlayBitmap = isVertical ? 
						TilemapsResCache.IconTilesetDepthVerticalTile : 
						TilemapsResCache.IconTilesetDepthFlatTile;
					overlayAlpha = 
						(this.TilesetView.HoveredTileIndex == paintData.TileIndex ? 255 : 
						(isVertical ? 192 : 
						64));
					overlayColor = 
						(isVertical ? Color.FromArgb(128, 192, 255) : 
						Color.FromArgb(255, 255, 255));
				}

				// Draw the overlay image in the center of the current tile
				e.Graphics.DrawImageTint(
					overlayBitmap, 
					Color.FromArgb(overlayAlpha, overlayColor), 
					paintData.ViewRect.X + (paintData.ViewRect.Width - overlayBitmap.Width) / 2,
					paintData.ViewRect.Y + (paintData.ViewRect.Height - overlayBitmap.Height) / 2);

				// Draw a hover indicator
				if (paintData.TileIndex == this.TilesetView.HoveredTileIndex)
				{
					Rectangle rect = paintData.ViewRect;
					rect.Width -= 1;
					rect.Height -= 1;
					e.Graphics.DrawRectangle(Pens.Black, rect);
					rect.Inflate(-1, -1);
					e.Graphics.DrawRectangle(new Pen(overlayColor), rect);
					rect.Inflate(-1, -1);
					e.Graphics.DrawRectangle(Pens.Black, rect);
				}
			}
		}
		private void TilesetView_MouseDown(object sender, MouseEventArgs e)
		{
			int tileIndex = this.TilesetView.HoveredTileIndex;
			if (tileIndex == -1) return;

			Tileset tileset = this.SelectedTileset.Res;
			if (tileset == null) return;

			if (this.editMode == EditMode.Offset)
			{
				if (e.Button == MouseButtons.Left)
				{
					this.drawDepthOffset = AddDepthOffset(tileset, tileIndex, 1);
					this.isUserDrawing = true;
				}
				else if (e.Button == MouseButtons.Right)
				{
					this.drawDepthOffset = AddDepthOffset(tileset, tileIndex, -1);
					this.isUserDrawing = true;
				}
				else if (e.Button == MouseButtons.Middle)
				{
					SetDepthOffset(tileset, tileIndex, this.drawDepthOffset);
					this.isUserDrawing = true;
				}
			}
			else
			{
				if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right)
				{
					this.drawVertical = ToggleDepthVertical(tileset, tileIndex);
					this.isUserDrawing = true;
				}
				else if (e.Button == MouseButtons.Middle)
				{
					SetDepthVertical(tileset, tileIndex, this.drawVertical);
					this.isUserDrawing = true;
				}
			}
		}
		private void TilesetView_MouseUp(object sender, MouseEventArgs e)
		{
			this.isUserDrawing = false;
		}
		private void TilesetView_KeyDown(object sender, KeyEventArgs e)
		{
			int tileIndex = this.TilesetView.HoveredTileIndex;
			if (tileIndex == -1) return;

			Tileset tileset = this.SelectedTileset.Res;
			if (tileset == null) return;

			if (this.editMode == EditMode.Offset)
			{
				if (e.KeyCode == Keys.Add)
				{
					this.drawDepthOffset = AddDepthOffset(tileset, tileIndex, 1);
				}
				else if (e.KeyCode == Keys.Subtract)
				{
					this.drawDepthOffset = AddDepthOffset(tileset, tileIndex, -1);
				}
			}
		}
		private void TilesetView_HoveredTileChanged(object sender, TilesetViewTileIndexChangeEventArgs e)
		{
			Tileset tileset = this.SelectedTileset.Res;
			if (tileset == null) return;

			if (this.isUserDrawing && e.TileIndex != -1)
			{
				if (this.editMode == EditMode.Offset)
					SetDepthOffset(tileset, e.TileIndex, this.drawDepthOffset);
				else
					SetDepthVertical(tileset, e.TileIndex, this.drawVertical);
			}
		}

		private static int AddDepthOffset(Tileset tileset, int tileIndex, int delta)
		{
			TileInput input = tileset.TileInput.Count > tileIndex ? tileset.TileInput[tileIndex] : default(TileInput);
			input.DepthOffset += delta;
			UndoRedoManager.Do(new EditTilesetTileInputAction(tileset, tileIndex, input));
			return input.DepthOffset;
		}
		private static void SetDepthOffset(Tileset tileset, int tileIndex, int offset)
		{
			TileInput input = tileset.TileInput.Count > tileIndex ? tileset.TileInput[tileIndex] : default(TileInput);
			if (input.DepthOffset != offset)
			{
				input.DepthOffset = offset;
				UndoRedoManager.Do(new EditTilesetTileInputAction(tileset, tileIndex, input));
			}
		}
		private static bool ToggleDepthVertical(Tileset tileset, int tileIndex)
		{
			TileInput input = tileset.TileInput.Count > tileIndex ? tileset.TileInput[tileIndex] : default(TileInput);
			input.IsVertical = !input.IsVertical;
			UndoRedoManager.Do(new EditTilesetTileInputAction(tileset, tileIndex, input));
			return input.IsVertical;
		}
		private static void SetDepthVertical(Tileset tileset, int tileIndex, bool vertical)
		{
			TileInput input = tileset.TileInput.Count > tileIndex ? tileset.TileInput[tileIndex] : default(TileInput);
			if (input.IsVertical != vertical)
			{
				input.IsVertical = vertical;
				UndoRedoManager.Do(new EditTilesetTileInputAction(tileset, tileIndex, input));
			}
		}
	}
}
