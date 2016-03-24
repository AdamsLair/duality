using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Drawing.Drawing2D;

using Aga.Controls.Tree;

using Duality.Plugins.Tilemaps;
using Duality.Editor.Plugins.Tilemaps;
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

		protected override void OnEnter()
		{
			base.OnEnter();
			this.SelectLayer(this.treeNodeOffsetLayer);
			this.TilesetView.PaintTiles += this.TilesetView_PaintTiles;
		}
		protected override void OnLeave()
		{
			base.OnLeave();
			this.TilesetView.PaintTiles -= this.TilesetView_PaintTiles;
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
		}

		private void TilesetView_PaintTiles(object sender, TilesetViewPaintTilesEventArgs e)
		{
			Font font = this.TilesetView.Font;
			float outlineWidth = 2;
			StringFormat textFormat = StringFormat.GenericTypographic;
			textFormat.Alignment = StringAlignment.Center;
			textFormat.LineAlignment = StringAlignment.Center;

			for (int i = 0; i < e.PaintedTiles.Count; i++)
			{
				TilesetViewPaintTileData paintData = e.PaintedTiles[i];
				int depthOffset = e.Tileset.TileData[paintData.TileIndex].DepthOffset;

				string text = depthOffset.ToString();
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

				int alpha = 
					(this.TilesetView.HoveredTileIndex == paintData.TileIndex ? 255 : 
					(depthOffset != 0 ? 192 : 
					64));
				Color color = 
					(depthOffset > 0 ? Color.FromArgb(alpha, 192, 255, 128) : 
					(depthOffset < 0 ? Color.FromArgb(alpha, 255, 128, 128) : 
					Color.FromArgb(alpha, 255, 255, 255)));

				e.Graphics.DrawImageTint(
					bitmap, 
					color, 
					paintData.ViewRect.X + (paintData.ViewRect.Width - bitmap.Width) / 2,
					paintData.ViewRect.Y + (paintData.ViewRect.Height - bitmap.Height) / 2);
			}
		}
	}
}
