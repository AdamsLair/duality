using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using Duality.Resources;
using Duality.Plugins.Tilemaps;


namespace Duality.Editor.Plugins.Tilemaps
{
	public class TilesetView : Panel
	{
		private Tileset tileset                = null;
		private Point2  displayedTileSize      = Point2.Zero;
		private Point2  displayedTileCount     = Point2.Zero;
		private Bitmap  displayedTiles         = null;
		private bool    globalEventsSubscribed = false;

		public Tileset Tileset
		{
			get { return this.tileset; }
			set
			{
				if (this.tileset != value)
				{
					this.tileset = value;
					this.OnTilesetChanged();

					if (this.tileset != null)
						this.SubscribeGlobalEvents();
				}
			}
		}

		public TilesetView()
		{
			this.SetStyle(ControlStyles.Selectable, true);
			this.TabStop = true;

			this.AutoScroll = true;

			this.SetStyle(ControlStyles.ResizeRedraw, true);
			this.SetStyle(ControlStyles.UserPaint, true);
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.Opaque, true);
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
		}
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.UnsubscribeGlobalEvents();
			}
			base.Dispose(disposing);
		}
		
		private void SubscribeGlobalEvents()
		{
			if (this.globalEventsSubscribed) return;
			DualityEditorApp.ObjectPropertyChanged += this.DualityEditorApp_ObjectPropertyChanged;
			this.globalEventsSubscribed = true;
		}
		private void UnsubscribeGlobalEvents()
		{
			DualityEditorApp.ObjectPropertyChanged -= this.DualityEditorApp_ObjectPropertyChanged;
			this.globalEventsSubscribed = false;
		}

		protected virtual void OnTilesetChanged()
		{
			TilesetRenderInput mainInput = (this.tileset != null) ? this.tileset.RenderConfig.FirstOrDefault() : null;
			Pixmap sourceData = (mainInput != null) ? mainInput.SourceData.Res : null;

			if (this.displayedTiles != null)
			{
				this.displayedTiles.Dispose();
				this.displayedTiles = null;
			}

			if (mainInput != null && sourceData != null)
			{
				this.displayedTiles = sourceData.MainLayer.ToBitmap();
				this.displayedTileSize = new Point2((int)this.tileset.TileSize.X, (int)this.tileset.TileSize.Y);
				this.displayedTileCount = new Point2(
					this.displayedTiles.Width / (mainInput.SourceTileSize.X + mainInput.SourceTileSpacing),
					this.displayedTiles.Height / (mainInput.SourceTileSize.Y + mainInput.SourceTileSpacing));

				this.AutoScrollMinSize = new Size(
					this.displayedTileSize.X * this.displayedTileCount.X, 
					this.displayedTileSize.Y * this.displayedTileCount.Y);
			}
			else
			{
				this.displayedTiles = null;
				this.displayedTileSize = Point2.Zero;
				this.displayedTileCount = Point2.Zero;
				this.AutoScrollMinSize = Size.Empty;
				this.AutoScrollPosition = Point.Empty;
			}
			this.Invalidate();
		}
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			e.Graphics.FillRectangle(new SolidBrush(this.BackColor), this.ClientRectangle);
		}

		private void DualityEditorApp_ObjectPropertyChanged(object sender, ObjectPropertyChangedEventArgs e)
		{
			if (this.tileset != null && e.HasObject(this.tileset))
			{
				this.Invalidate();
			}
		}
	}
}
