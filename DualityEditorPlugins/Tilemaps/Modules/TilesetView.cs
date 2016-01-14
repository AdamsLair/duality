using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using Duality.Resources;
using Duality.Plugins.Tilemaps;


namespace Duality.Editor.Plugins.Tilemaps
{
	public class TilesetView : Panel
	{
		public enum HorizontalAlignment
		{
			Left,
			Right,
			Center
		}


		private ContentRef<Tileset> tileset                = null;
		private int                 displayedConfigIndex   = 0;
		private HorizontalAlignment rowAlignment           = HorizontalAlignment.Left;
		private int                 totalTileCount         = 0;
		private Size                tileSize               = Size.Empty;
		private Point               tileCount              = Point.Empty;
		private Bitmap              tileBitmap             = null;
		private Bitmap              backBitmap             = null;
		private TextureBrush        backBrush              = null;
		private int                 additionalSpace        = 0;
		private Size                contentSize            = Size.Empty;
		private Size                spacing                = new Size(2, 2);
		private int                 hoverIndex             = -1;
		private bool                globalEventsSubscribed = false;
		private Rectangle           selectedArea           = Rectangle.Empty;
		private Point               actionBeginTilePos     = Point.Empty;
		private bool                isUserSelecting        = false;


		public ContentRef<Tileset> TargetTileset
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
		[DefaultValue(0)]
		public int DisplayedConfigIndex
		{
			get { return this.displayedConfigIndex; }
			set
			{
				if (this.displayedConfigIndex != value)
				{
					this.displayedConfigIndex = value;
					this.OnTilesetChanged();
				}
			}
		}
		[DefaultValue(typeof(Size), "2, 2")]
		public Size Spacing
		{
			get { return this.spacing; }
			set
			{
				this.spacing = value;
				this.GenerateBackgroundPattern();
				this.UpdateContentStats();
				this.Invalidate();
			}
		}
		[DefaultValue(HorizontalAlignment.Left)]
		public HorizontalAlignment RowAlignment
		{
			get { return this.rowAlignment; }
			set
			{
				if (this.rowAlignment != value)
				{
					this.rowAlignment = value;
					this.UpdateContentStats();
					this.Invalidate();
				}
			}
		}
		public Rectangle SelectedArea
		{
			get { return this.selectedArea; }
			set
			{
				if (this.selectedArea != value)
				{
					this.selectedArea = value;
					this.Invalidate();
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

			this.OnTilesetChanged();
		}
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.UnsubscribeGlobalEvents();
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// Determines the tile index at the specified pixel / view coordinate.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="scrolled"></param>
		/// <param name="allowNearest"></param>
		/// <returns></returns>
		public int PickTileIndexAt(int x, int y, bool scrolled = true, bool allowNearest = false)
		{
			if (scrolled)
			{
				x -= this.AutoScrollPosition.X;
				y -= this.AutoScrollPosition.Y;
			}

			x -= this.ClientRectangle.X + this.Padding.Left - this.spacing.Width / 2;
			y -= this.ClientRectangle.Y + this.Padding.Top - this.spacing.Height / 2;

			switch (this.rowAlignment)
			{
				default:
				case HorizontalAlignment.Left:
					break;
				case HorizontalAlignment.Right:
					x -= this.additionalSpace;
					break;
				case HorizontalAlignment.Center:
					x -= this.additionalSpace / 2;
					break;
			}

			if (allowNearest)
			{
				if (x < 0) x = 0;
				if (y < 0) y = 0;
				if (x >= this.contentSize.Width) x = this.contentSize.Width - 1;
				if (y >= this.contentSize.Height) y = this.contentSize.Height - 1;
			}
			else
			{
				if (x < 0) return -1;
				if (y < 0) return -1;
				if (x >= this.contentSize.Width) return -1;
				if (y >= this.contentSize.Height) return -1;
			}

			int rowIndex = y / (this.tileSize.Height + this.spacing.Height);
			int colIndex = x / (this.tileSize.Width + this.spacing.Width);
			int modelIndex = rowIndex * this.tileCount.X + colIndex;

			if (allowNearest)
			{
				if (modelIndex < 0) modelIndex = 0;
				if (modelIndex >= this.totalTileCount) modelIndex = this.totalTileCount - 1;
			}
			else
			{
				if (modelIndex < 0) modelIndex = -1;
				if (modelIndex >= this.totalTileCount) modelIndex = -1;
			}

			return modelIndex;
		}
		/// <summary>
		/// Determines the pixel / view coordinate of the specified tile index.
		/// </summary>
		/// <param name="tileIndex"></param>
		/// <param name="scrolled"></param>
		/// <returns></returns>
		public Point GetTileIndexLocation(int tileIndex, bool scrolled = true)
		{
			Point result = this.ClientRectangle.Location;
			result.X += this.Padding.Left;
			result.Y += this.Padding.Top;

			switch (this.rowAlignment)
			{
				default:
				case HorizontalAlignment.Left:
					break;
				case HorizontalAlignment.Right:
					result.X += this.additionalSpace;
					break;
				case HorizontalAlignment.Center:
					result.X += this.additionalSpace / 2;
					break;
			}

			if (this.tileCount.X > 0)
			{
				int rowIndex = tileIndex / this.tileCount.X;
				int colIndex = tileIndex % this.tileCount.X;
				result.X += colIndex * (this.tileSize.Width + this.spacing.Width);
				result.Y += rowIndex * (this.tileSize.Height + this.spacing.Height);
			}

			if (scrolled)
			{
				result.X += this.AutoScrollPosition.X;
				result.Y += this.AutoScrollPosition.Y;
			}
			return result;
		}

		/// <summary>
		/// Converts a 2D tile (not pixel) coordinate into the associated tile index.
		/// </summary>
		/// <param name="tileX"></param>
		/// <param name="tileY"></param>
		/// <returns></returns>
		public int GetTileIndex(int tileX, int tileY)
		{
			return tileX + tileY * this.tileCount.X;
		}
		/// <summary>
		/// Converts a tile index into a 2D tile (not pixel) coordinate.
		/// </summary>
		/// <param name="tileIndex"></param>
		/// <returns></returns>
		public Point GetTilePos(int tileIndex)
		{
			return new Point(
				tileIndex % this.tileCount.X,
				tileIndex / this.tileCount.X);
		}
		
		private void UpdateContentStats()
		{
			Rectangle contentArea = new Rectangle(
				this.ClientRectangle.X + this.Padding.Left,
				this.ClientRectangle.Y + this.Padding.Top,
				this.ClientRectangle.Width - this.Padding.Horizontal,
				this.ClientRectangle.Height - this.Padding.Vertical);

			{
				int hTiles = Math.Min(this.totalTileCount, this.tileCount.X);
				this.contentSize = new Size(
					hTiles * this.tileSize.Width + (hTiles - 1) * this.spacing.Width, 
					this.tileCount.Y * this.tileSize.Height + (this.tileCount.Y - 1) * this.spacing.Height);
			}
			{
				int lastAdditionalSpace = this.additionalSpace;
				this.additionalSpace = Math.Max(contentArea.Width - this.contentSize.Width, 0);
				if (this.additionalSpace != lastAdditionalSpace) this.Invalidate();
			}

			Size autoScrollSize;
			autoScrollSize = this.contentSize;
			autoScrollSize.Width += this.Padding.Horizontal;
			autoScrollSize.Height += this.Padding.Vertical;

			if (this.AutoScrollMinSize != autoScrollSize)
				this.AutoScrollMinSize = autoScrollSize;
		}
		private void GenerateBackgroundPattern()
		{
			// Dispose old local bitmaps
			if (this.backBitmap != null)
			{
				this.backBitmap.Dispose();
				this.backBitmap = null;
			}
			if (this.backBrush != null)
			{
				this.backBrush.Dispose();
				this.backBrush = null;
			}

			// Generate local background image based on tile size
			{
				float backLum = this.BackColor.GetLuminance();
				Brush darkBrush = new SolidBrush(backLum <= 0.5f ? Color.FromArgb(56, 56, 56) : Color.FromArgb(176, 176, 176));
				Brush brightBrush = new SolidBrush(backLum <= 0.5f ? Color.FromArgb(72, 72, 72) : Color.FromArgb(208, 208, 208));

				Size cellBaseSize = (this.tileSize == Size.Empty) ? new Size(Tileset.DefaultTileSize.X, Tileset.DefaultTileSize.Y) : this.tileSize;
				Point cellCount = new Point(4, 4);
				Size cellSize = new Size(
					cellBaseSize.Width / cellCount.X, 
					cellBaseSize.Height / cellCount.Y);

				this.backBitmap = new Bitmap(
					cellBaseSize.Width + this.spacing.Width, 
					cellBaseSize.Height + this.spacing.Height);
				using (Graphics g = Graphics.FromImage(this.backBitmap))
				{
					g.Clear(Color.Transparent);

					int totalCellCount = cellCount.X * cellCount.Y;
					for (int i = 0; i < totalCellCount; i++)
					{
						int lineIndex = i / cellCount.X;
						bool evenLine = (lineIndex % 2) == 0;
						bool darkCell = (i % 2) == (evenLine ? 0 : 1);
						Point cellPos = new Point(
							(i % cellCount.X) * cellSize.Width, 
							lineIndex * cellSize.Height);

						g.FillRectangle(
							darkCell ? darkBrush : brightBrush, 
							cellPos.X, 
							cellPos.Y, 
							cellSize.Width, 
							cellSize.Height);
					}
				}
			}

			// Create a background brush for OnPaint to use
			this.backBrush = new TextureBrush(this.backBitmap);
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
			Tileset tileset = this.tileset.Res;
			TilesetRenderInput mainInput = (tileset != null) ? tileset.RenderConfig.ElementAtOrDefault(this.displayedConfigIndex) : null;
			Pixmap sourceData = (mainInput != null) ? mainInput.SourceData.Res : null;

			// Dispose old local bitmaps
			if (this.tileBitmap != null)
			{
				this.tileBitmap.Dispose();
				this.tileBitmap = null;
			}

			// Retrieve tileset data and create local tileset bitmap
			if (mainInput != null && sourceData != null)
			{
				this.tileBitmap = sourceData.MainLayer.ToBitmap();
				this.tileSize = new Size((int)tileset.TileSize.X, (int)tileset.TileSize.Y);
				this.tileCount = new Point(
					this.tileBitmap.Width / (mainInput.SourceTileSize.X + mainInput.SourceTileSpacing),
					this.tileBitmap.Height / (mainInput.SourceTileSize.Y + mainInput.SourceTileSpacing));
				this.totalTileCount = this.tileCount.X * this.tileCount.Y;
			}
			else
			{
				this.tileBitmap = null;
				this.tileSize = Size.Empty;
				this.tileCount = Point.Empty;
				this.totalTileCount = 0;
			}

			this.GenerateBackgroundPattern();
			this.UpdateContentStats();
			this.Invalidate();
		}
		protected override void OnBackColorChanged(EventArgs e)
		{
			base.OnBackColorChanged(e);
			this.GenerateBackgroundPattern();
			this.Invalidate();
		}
		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);
			this.UpdateContentStats();
			this.Invalidate();
		}
		protected override void OnPaddingChanged(EventArgs e)
		{
			base.OnPaddingChanged(e);
			this.UpdateContentStats();
			this.Invalidate();
		}
		protected override void OnEnabledChanged(EventArgs e)
		{
			base.OnEnabledChanged(e);
			this.Invalidate();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			Tileset tileset = this.tileset.Res;

			// Clear the background entirely
			e.Graphics.Clear(this.BackColor);
			
			// Fill with background pattern
			if (this.backBrush != null)
			{
				Point offset = this.GetTileIndexLocation(0);
				this.backBrush.ResetTransform();
				this.backBrush.TranslateTransform(offset.X, offset.Y);
				e.Graphics.FillRectangle(this.backBrush, this.ClientRectangle);
			}

			// Draw hovered tile background
			if (this.Enabled && this.hoverIndex != -1)
			{
				Point hoverPos = this.GetTileIndexLocation(this.hoverIndex);
				e.Graphics.FillRectangle(
					new SolidBrush(Color.FromArgb(32, this.ForeColor)), 
					hoverPos.X - 1, 
					hoverPos.Y - 1, 
					this.tileSize.Width + 1, 
					this.tileSize.Height + 1);
			}

			// Draw tile items
			if (tileset != null && this.totalTileCount > 0)
			{
				int firstIndex = this.PickTileIndexAt(e.ClipRectangle.Left, e.ClipRectangle.Top, true, true);
				int lastIndex = this.PickTileIndexAt(e.ClipRectangle.Right - 1, e.ClipRectangle.Bottom - 1, true, true);
				Point firstItemPos = this.GetTileIndexLocation(firstIndex);

				Size texSize = new Size(
					MathF.NextPowerOfTwo(this.tileBitmap.Width),
					MathF.NextPowerOfTwo(this.tileBitmap.Height));

				Point basePos = firstItemPos;
				Point curPos = basePos;
				int skipItemsPerRow = (firstIndex % this.tileCount.X);
				int itemsPerRenderedRow = this.tileCount.X - skipItemsPerRow;
				int itemsInCurrentRow = 0;
				for (int i = firstIndex; i <= lastIndex; i++)
				{
					Rectangle tileRect = new Rectangle(curPos.X, curPos.Y, this.tileSize.Width, this.tileSize.Height);

					Point2 atlasTilePos;
					Point2 atlasTileSize;
					tileset.LookupTileSourceRect(this.displayedConfigIndex, i, out atlasTilePos, out atlasTileSize);

					e.Graphics.DrawImage(this.tileBitmap, tileRect, atlasTilePos.X, atlasTilePos.Y, atlasTileSize.X, atlasTileSize.Y, GraphicsUnit.Pixel);

					itemsInCurrentRow++;
					if (itemsInCurrentRow == itemsPerRenderedRow)
					{
						curPos.X = basePos.X;
						curPos.Y += this.tileSize.Height + this.spacing.Height;
						itemsInCurrentRow = 0;
						i += skipItemsPerRow;
					}
					else
					{
						curPos.X += this.tileSize.Width + this.spacing.Width;
					}
				}
			}

			// Draw hovered tile foreground
			if (this.Enabled && this.hoverIndex != -1)
			{
				Point hoverPos = this.GetTileIndexLocation(this.hoverIndex);
				e.Graphics.FillRectangle(
					new SolidBrush(Color.FromArgb(32, this.ForeColor)), 
					hoverPos.X - 1, 
					hoverPos.Y - 1, 
					this.tileSize.Width + 1, 
					this.tileSize.Height + 1);
				e.Graphics.DrawRectangle(
					new Pen(this.ForeColor), 
					hoverPos.X - 1, 
					hoverPos.Y - 1, 
					this.tileSize.Width + 1, 
					this.tileSize.Height + 1);
			}

			// Draw selection indicators
			if (this.Enabled && !this.selectedArea.IsEmpty)
			{
				int startIndex = this.GetTileIndex(this.selectedArea.X, this.selectedArea.Y);
				Point startPos = this.GetTileIndexLocation(startIndex);

				// "Grey out" everything except the current selection a little bit
				Region oldClip = e.Graphics.Clip;
				e.Graphics.SetClip(new Rectangle(
					startPos.X - 1, 
					startPos.Y - 1, 
					this.selectedArea.Width * (this.tileSize.Width + 1), 
					this.selectedArea.Height * (this.tileSize.Height + 1)), 
					System.Drawing.Drawing2D.CombineMode.Exclude);
				e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(64, this.BackColor)), this.ClientRectangle);
				e.Graphics.Clip = oldClip;

				// Draw the selected tile area
				e.Graphics.DrawRectangle(
					new Pen(this.BackColor), 
					startPos.X - 1, 
					startPos.Y - 1, 
					this.selectedArea.Width * (this.tileSize.Width + 1), 
					this.selectedArea.Height * (this.tileSize.Height + 1));
				e.Graphics.DrawRectangle(
					new Pen(this.ForeColor), 
					startPos.X - 2, 
					startPos.Y - 2, 
					this.selectedArea.Width * (this.tileSize.Width + 1) + 2, 
					this.selectedArea.Height * (this.tileSize.Height + 1) + 2);
				e.Graphics.DrawRectangle(
					new Pen(this.BackColor), 
					startPos.X - 3, 
					startPos.Y - 3, 
					this.selectedArea.Width * (this.tileSize.Width + 1) + 4, 
					this.selectedArea.Height * (this.tileSize.Height + 1) + 4);
				e.Graphics.DrawRectangle(
					new Pen(Color.FromArgb(128, this.BackColor)), 
					startPos.X - 4, 
					startPos.Y - 4, 
					this.selectedArea.Width * (this.tileSize.Width + 1) + 6, 
					this.selectedArea.Height * (this.tileSize.Height + 1) + 6);
			}

			// Overlay with disabled plate
			if (!this.Enabled)
			{
				e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(128, this.BackColor)), this.ClientRectangle);
			}
		}
		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			this.hoverIndex = -1;
			this.Invalidate();
		}
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			if (e.Button == MouseButtons.Left)
			{
				int tileIndex = this.PickTileIndexAt(e.X, e.Y);
				if (tileIndex != -1)
				{
					this.hoverIndex = -1;
					this.actionBeginTilePos = this.GetTilePos(tileIndex);
					this.isUserSelecting = true;
					this.SelectedArea = new Rectangle(this.actionBeginTilePos.X, this.actionBeginTilePos.Y, 1, 1);
				}
			}
		}
		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			if (!this.isUserSelecting)
			{
				this.SelectedArea = Rectangle.Empty;
			}
			this.actionBeginTilePos = Point.Empty;
			this.isUserSelecting = false;
		}
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			int tileIndex = this.PickTileIndexAt(e.X, e.Y);
			if (this.isUserSelecting)
			{
				if (tileIndex != -1)
				{
					Point tilePos = this.GetTilePos(tileIndex);
					Point selectionTopLeft = new Point(
						Math.Min(this.actionBeginTilePos.X, tilePos.X), 
						Math.Min(this.actionBeginTilePos.Y, tilePos.Y));
					Point selectionBottomRight = new Point(
						Math.Max(this.actionBeginTilePos.X, tilePos.X), 
						Math.Max(this.actionBeginTilePos.Y, tilePos.Y));
					this.SelectedArea = new Rectangle(
						selectionTopLeft.X,
						selectionTopLeft.Y,
						selectionBottomRight.X - selectionTopLeft.X + 1,
						selectionBottomRight.Y - selectionTopLeft.Y + 1);
				}
			}
			else
			{
				int oldHoverIndex = this.hoverIndex;
				this.hoverIndex = tileIndex;
				if (oldHoverIndex != this.hoverIndex)
					this.Invalidate();
			}
		}
		protected override void OnMouseClick(MouseEventArgs e)
		{
			base.OnMouseClick(e);
			this.Focus();
		}

		private void DualityEditorApp_ObjectPropertyChanged(object sender, ObjectPropertyChangedEventArgs e)
		{
			if (this.tileset != null && e.HasObject(this.tileset))
			{
				this.UpdateContentStats();
				this.Invalidate();
			}
		}
	}
}
