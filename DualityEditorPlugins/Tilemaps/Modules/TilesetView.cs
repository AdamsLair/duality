using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
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

		protected enum MultiColumnMode
		{
			None,
			Horizontal,
			Vertical
		}


		private ContentRef<Tileset> tileset                = null;
		private int                 displayedConfigIndex   = 0;
		private HorizontalAlignment rowAlignment           = HorizontalAlignment.Left;
		private bool                allowMultiColumnMode   = true;
		private MultiColumnMode     multiColumnMode        = MultiColumnMode.None;
		private int                 multiColumnCount       = 1;
		private int                 multiColumnLength      = 0;
		private int                 totalTileCount         = 0;
		private Size                displayedTileSize      = Size.Empty;
		private Point               tileCount              = Point.Empty;
		private Size                tilesetContentSize     = Size.Empty;
		private Bitmap              tileBitmap             = null;
		private Bitmap              backBitmap             = null;
		private TextureBrush        backBrush              = null;
		private int                 additionalSpace        = 0;
		private Size                contentSize            = Size.Empty;
		private Size                spacing                = new Size(2, 2);
		private int                 hoverIndex             = -1;
		private bool                globalEventsSubscribed = false;

		private RawList<TilesetViewPaintTileData> paintTileBuffer = new RawList<TilesetViewPaintTileData>();

		public event EventHandler<TilesetViewPaintTilesEventArgs> PaintTiles = null;
		public event EventHandler<TilesetViewTileIndexChangeEventArgs> HoveredTileChanged = null;


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
		[DefaultValue(true)]
		public bool AllowMultiColumn
		{
			get { return this.allowMultiColumnMode; }
			set
			{
				if (this.allowMultiColumnMode == value) return;
				this.allowMultiColumnMode = value;
				this.UpdateContentStats();
			}
		}
		public int HoveredTileIndex
		{
			get { return this.hoverIndex; }
			set
			{
				if (this.hoverIndex != value)
				{
					this.hoverIndex = value;
					this.Invalidate();
				}
			}
		}
		/// <summary>
		/// [GET] The pixel size of a single tile, as displayed in this <see cref="TilesetView"/>.
		/// Note that this does not necessarily need to reflect the original <see cref="Tileset"/>
		/// tile size.
		/// </summary>
		public Size DisplayedTileSize
		{
			get { return this.displayedTileSize; }
		}
		/// <summary>
		/// [GET] The original tileset layout's number of tiles in X and Y direction.
		/// </summary>
		protected Point TileCount
		{
			get { return this.tileCount; }
		}
		/// <summary>
		/// [GET] The number of displayed tiles in X and Y direction.
		/// </summary>
		protected Point DisplayedTileCount
		{
			get
			{
				Point displayedTileCount = this.tileCount;
				if (this.multiColumnMode == MultiColumnMode.Horizontal)
				{
					displayedTileCount = new Point(
						this.tileCount.X * this.multiColumnCount,
						this.multiColumnLength);
				}
				else if (this.multiColumnMode == MultiColumnMode.Vertical)
				{
					displayedTileCount = new Point(
						this.multiColumnLength,
						this.tileCount.Y * this.multiColumnCount);
				}
				return displayedTileCount;
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
		/// Invalidates the assigned <see cref="Tileset"/> and forces re-generating its
		/// visual representation inside this <see cref="TilesetView"/>. This is only
		/// necessary if the <see cref="Tileset"/> has changed outside the <see cref="TilesetView"/>.
		/// </summary>
		public void InvalidateTileset()
		{
			this.OnTilesetChanged();
		}
		public void InvalidateTile(int tileIndex, int pixelBorder)
		{
			this.InvalidateTiles(tileIndex, 1, 1, pixelBorder);
		}
		public void InvalidateTiles(int tileIndex, int tileCountX, int tileCountY, int pixelBorder)
		{
			if (tileIndex == -1) return;

			Rectangle rect;
			Point loc = this.GetTileIndexLocation(tileIndex);
			rect = new Rectangle(
				loc.X - this.spacing.Width - pixelBorder, 
				loc.Y - this.spacing.Height - pixelBorder, 
				1 + (this.displayedTileSize.Width + this.spacing.Width) * tileCountX + this.spacing.Width + pixelBorder * 2, 
				1 + (this.displayedTileSize.Height + this.spacing.Height) * tileCountY + this.spacing.Height + pixelBorder * 2);

			this.Invalidate(rect);
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

			// Determine the model index from the normalized local pixel position
			int modelIndex;
			{
				Point singleColumnTilePos = this.GetTilesetTilePos(
					x / (this.displayedTileSize.Width + this.spacing.Width),
					y / (this.displayedTileSize.Height + this.spacing.Height));

				// Clamp selected 2D tile coordinates
				bool isValidCoordinate = 
					singleColumnTilePos.X < this.tileCount.X && 
					singleColumnTilePos.Y < this.tileCount.Y;
				if (!isValidCoordinate && allowNearest)
				{
					if (singleColumnTilePos.X >= this.tileCount.X) singleColumnTilePos.X = this.tileCount.X - 1;
					if (singleColumnTilePos.Y >= this.tileCount.Y) singleColumnTilePos.Y = this.tileCount.Y - 1;
					isValidCoordinate = true;
				}

				// Transform 2D tile coordinates to the model index of that tile
				if (isValidCoordinate)
					modelIndex = singleColumnTilePos.Y * this.tileCount.X + singleColumnTilePos.X;
				else
					modelIndex = -1;
			}

			// Clamp selected model index
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
				Point multiColumnTilePos = this.GetDisplayedTilePos(
					tileIndex % this.tileCount.X,
					tileIndex / this.tileCount.X);

				result.X += multiColumnTilePos.X * (this.displayedTileSize.Width + this.spacing.Width);
				result.Y += multiColumnTilePos.Y * (this.displayedTileSize.Height + this.spacing.Height);
			}

			if (scrolled)
			{
				result.X += this.AutoScrollPosition.X;
				result.Y += this.AutoScrollPosition.Y;
			}
			return result;
		}

		/// <summary>
		/// Converts a 2D tile (not pixel) coordinate from displayed to original tileset space.
		/// </summary>
		/// <param name="displayedTileX"></param>
		/// <param name="displayedTileY"></param>
		/// <returns></returns>
		public Point GetTilesetTilePos(int displayedTileX, int displayedTileY)
		{
			int rowIndex = displayedTileY;
			int colIndex = displayedTileX;

			// Adjust column and row indices depending on multi-column display modes
			if (this.multiColumnMode == MultiColumnMode.Horizontal)
			{
				int multiColumnIndex = colIndex / this.tileCount.X;
				colIndex %= this.tileCount.X;
				rowIndex += multiColumnIndex * this.multiColumnLength;
			}
			else if (this.multiColumnMode == MultiColumnMode.Vertical)
			{
				int multiColumnIndex = rowIndex / this.tileCount.Y;
				rowIndex %= this.tileCount.Y;
				colIndex += multiColumnIndex * this.multiColumnLength;
			}

			return new Point(colIndex, rowIndex);
		}
		/// <summary>
		/// Converts a 2D tile (not pixel) coordinate from original tileset to displayed space.
		/// </summary>
		/// <param name="tilesetTileX"></param>
		/// <param name="tilesetTileY"></param>
		/// <returns></returns>
		public Point GetDisplayedTilePos(int tilesetTileX, int tilesetTileY)
		{
			int rowIndex = tilesetTileY;
			int colIndex = tilesetTileX;

			// Adjust column and row indices depending on multi-column display modes
			if (this.multiColumnMode == MultiColumnMode.Horizontal)
			{
				int multiColumnIndex = rowIndex / this.multiColumnLength;
				rowIndex %= this.multiColumnLength;
				colIndex += multiColumnIndex * this.tileCount.X;
			}
			else if (this.multiColumnMode == MultiColumnMode.Vertical)
			{
				int multiColumnIndex = colIndex / this.multiColumnLength;
				colIndex %= this.multiColumnLength;
				rowIndex += multiColumnIndex * this.tileCount.Y;
			}

			return new Point(colIndex, rowIndex);
		}

		/// <summary>
		/// Converts a 2D tile (not pixel) coordinate (in tileset space) into the associated tile index.
		/// </summary>
		/// <param name="tileX"></param>
		/// <param name="tileY"></param>
		/// <returns></returns>
		public int GetTileIndex(int tileX, int tileY)
		{
			return tileX + tileY * this.tileCount.X;
		}
		/// <summary>
		/// Converts a tile index into a 2D tile (not pixel) coordinate, in tileset space.
		/// </summary>
		/// <param name="tileIndex"></param>
		/// <returns></returns>
		public Point GetTilePos(int tileIndex)
		{
			return new Point(
				tileIndex % this.tileCount.X,
				tileIndex / this.tileCount.X);
		}
		
		private void UpdateMultiColumnMode()
		{
			MultiColumnMode lastMode = this.multiColumnMode;
			int lastCount = this.multiColumnCount;
			int lastLength = this.multiColumnLength;

			// Calculate how many columns we could fit vertically and horizontally
			Point maxColumnCount;
			if (this.totalTileCount == 0 || !this.allowMultiColumnMode)
			{
				maxColumnCount = Point.Empty;
			}
			else
			{
				int scrollbarBuffer = 1; // Stop one-pixel scrollbar appearances
				maxColumnCount = new Point(
					(this.ClientSize.Width - this.Padding.Horizontal - scrollbarBuffer) / this.tilesetContentSize.Width,
					(this.ClientSize.Height - this.Padding.Vertical - scrollbarBuffer) / this.tilesetContentSize.Height);
			}

			// Use multiple columns if we can't fit the entire tileset,
			// but we *could fit* it when split up.
			if (maxColumnCount.X > 1 && maxColumnCount.Y == 0)
			{
				this.multiColumnMode = MultiColumnMode.Horizontal;
				this.multiColumnCount = maxColumnCount.X;
				this.multiColumnLength = (int)Math.Ceiling((float)this.tileCount.Y / (float)this.multiColumnCount);
			}
			else if (maxColumnCount.Y > 1 && maxColumnCount.X == 0)
			{
				this.multiColumnMode = MultiColumnMode.Vertical;
				this.multiColumnCount = maxColumnCount.Y;
				this.multiColumnLength = (int)Math.Ceiling((float)this.tileCount.X / (float)this.multiColumnCount);
			}
			else
			{
				this.multiColumnMode = MultiColumnMode.None;
				this.multiColumnCount = 1;
				this.multiColumnLength = this.tileCount.Y;
			}

			// If we changed our multi-column layout, notify everyone who needs to know
			if (this.multiColumnMode != lastMode ||
				this.multiColumnCount != lastCount ||
				this.multiColumnLength != lastLength)
			{
				this.OnTileDisplayModeChanged();
			}
		}
		private void UpdateContentStats()
		{
			this.UpdateMultiColumnMode();

			Rectangle contentArea = new Rectangle(
				this.ClientRectangle.X + this.Padding.Left,
				this.ClientRectangle.Y + this.Padding.Top,
				this.ClientRectangle.Width - this.Padding.Horizontal,
				this.ClientRectangle.Height - this.Padding.Vertical);

			{
				Point displayedTileCount = this.tileCount;
				if (this.multiColumnMode == MultiColumnMode.Horizontal)
				{
					displayedTileCount.X *= this.multiColumnCount;
					displayedTileCount.Y = this.multiColumnLength;
				}
				else if (this.multiColumnMode == MultiColumnMode.Vertical)
				{
					displayedTileCount.X = this.multiColumnLength;
					displayedTileCount.Y *= this.multiColumnCount;
				}
				displayedTileCount.X = Math.Min(displayedTileCount.X, this.totalTileCount);

				this.contentSize = new Size(
					displayedTileCount.X * this.displayedTileSize.Width  + (displayedTileCount.X - 1) * this.spacing.Width, 
					displayedTileCount.Y * this.displayedTileSize.Height + (displayedTileCount.Y - 1) * this.spacing.Height);
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

				Size cellBaseSize = (this.displayedTileSize == Size.Empty) ? new Size(Tileset.DefaultTileSize.X, Tileset.DefaultTileSize.Y) : this.displayedTileSize;
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
				Vector2 originalTileSize = tileset.TileSize;
				float minDisplayedSize = 30.0f;
				float maxDisplayedSize = 50.0f;

				// Find a suitable display size for the tileset
				Vector2 displayedTileSize = originalTileSize;
				while (displayedTileSize.X > maxDisplayedSize)
					displayedTileSize /= 2.0f;
				while (displayedTileSize.X < minDisplayedSize)
					displayedTileSize *= 2.0f;

				displayedTileSize = displayedTileSize * 
					(MathF.Clamp(
						displayedTileSize.X, 
						minDisplayedSize, 
						maxDisplayedSize) / 
					displayedTileSize.X);

				this.tileBitmap = sourceData.MainLayer.ToBitmap();
				this.displayedTileSize = new Size(
					(int)displayedTileSize.X, 
					(int)displayedTileSize.Y);
				this.tileCount = new Point(
					this.tileBitmap.Width / (mainInput.SourceTileSize.X + mainInput.SourceTileSpacing * 2),
					this.tileBitmap.Height / (mainInput.SourceTileSize.Y + mainInput.SourceTileSpacing * 2));
				this.totalTileCount = this.tileCount.X * this.tileCount.Y;
				this.tilesetContentSize = new Size(
					this.displayedTileSize.Width * this.tileCount.X + this.spacing.Width * (this.tileCount.X - 1), 
					this.displayedTileSize.Height * this.tileCount.Y + this.spacing.Height * (this.tileCount.Y - 1));
			}
			else
			{
				this.tileBitmap = null;
				this.displayedTileSize = Size.Empty;
				this.tileCount = Point.Empty;
				this.totalTileCount = 0;
				this.tilesetContentSize = Size.Empty;
			}

			this.GenerateBackgroundPattern();
			this.UpdateContentStats();
			this.Invalidate();
		}
		protected virtual void OnTileDisplayModeChanged() { }
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

		/// <summary>
		/// Draws the tile layer of the <see cref="TilesetView"/>.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnPaintTiles(PaintEventArgs e)
		{
			Tileset tileset = this.tileset.Res;

			// Early-out if there are no tiles to draw
			if (tileset == null) return;
			if (this.totalTileCount == 0) return;

			// Determine which tiles are visible in the current viewport, so not all of them are drawn needlessly
			// Note: Not using clip rectangle here, because the multicolumn rendering algorithm assumes that
			// we always start at the beginning.
			int firstIndex = this.PickTileIndexAt(0, 0, true, true);
			int lastIndex = this.PickTileIndexAt(this.ClientSize.Width - 1, this.ClientSize.Height - 1, true, true);
			Point firstItemPos = this.GetTileIndexLocation(firstIndex);
			if (lastIndex < firstIndex) return;

			Size texSize = new Size(
				MathF.NextPowerOfTwo(this.tileBitmap.Width),
				MathF.NextPowerOfTwo(this.tileBitmap.Height));

			// ToDo: Cleanup the below algorithm, it's hard to understand and has far too many
			// special cases. If performance doesn't suffer too much, maybe even do a 2D grid draw
			// and query tile indices using PickTileIndexAt?

			// Determine rendering data for all visible tile items
			paintTileBuffer.Count = 0;
			paintTileBuffer.Reserve(lastIndex - firstIndex);
			{
				Point basePos = firstItemPos;
				Point curPos = basePos;
				int itemsPerRow = (this.multiColumnMode == MultiColumnMode.Vertical) ? this.multiColumnLength : this.tileCount.X;
				int skipItemsPerRow = (firstIndex % itemsPerRow);
				int itemsPerRenderedRow = itemsPerRow - skipItemsPerRow;
				int itemsInCurrentRow = 0;
				for (int i = firstIndex; i <= lastIndex; i++)
				{
					Rectangle tileRect = new Rectangle(curPos.X, curPos.Y, this.displayedTileSize.Width, this.displayedTileSize.Height);

					// If the tile is actually visible, add the required data to the paint buffer
					if (e.ClipRectangle.IntersectsWith(tileRect))
					{
						Point2 atlasTilePos;
						Point2 atlasTileSize;
						tileset.LookupTileSourceRect(this.displayedConfigIndex, i, out atlasTilePos, out atlasTileSize);
				
						paintTileBuffer.Count++;
						paintTileBuffer.Data[paintTileBuffer.Count - 1] = new TilesetViewPaintTileData
						{
							TileIndex = i,
							SourceRect = new Rectangle(atlasTilePos.X, atlasTilePos.Y, atlasTileSize.X, atlasTileSize.Y),
							ViewRect = tileRect
						};
					}

					itemsInCurrentRow++;
					bool isLastIndexInRow = (itemsInCurrentRow == itemsPerRenderedRow);
					bool isLastIndexInHorizontalMultiColumn = 
						(this.multiColumnMode == MultiColumnMode.Horizontal) &&
						(i % (this.tileCount.X * this.multiColumnLength)) == (this.tileCount.X * this.multiColumnLength - 1);

					if (isLastIndexInHorizontalMultiColumn)
					{
						// Determine how many tiles we need to skip to the next horizontal multicolumn
						int baseIndexInRow = firstIndex % this.tileCount.X;
						int tilesToNextMultiColumn = this.tileCount.X - baseIndexInRow;

						// Switch to the next horizontal multicolumn
						itemsInCurrentRow = 0;
						basePos.X += tilesToNextMultiColumn * (this.displayedTileSize.Width + this.spacing.Width);
						curPos = basePos;
						i = this.PickTileIndexAt(curPos.X, curPos.Y, true, false);
						if (i == -1) break;

						// Recalculate regular rowskip values because we switched to the next horizontal multicolumn
						// If we previously rendered the last part of a row, this no longer applies. Since we are
						// now back at the beginning of a row, we don't skip any items prior and render full rows instead.
						// (Yes, this could be optimized, but there's no need right now)
						skipItemsPerRow = 0;
						itemsPerRenderedRow = itemsPerRow - skipItemsPerRow;

						i--;
					}
					else if (isLastIndexInRow)
					{
						curPos.X = basePos.X;
						curPos.Y += this.displayedTileSize.Height + this.spacing.Height;
						itemsInCurrentRow = 0;
						i += skipItemsPerRow;

						if (this.multiColumnMode == MultiColumnMode.Vertical)
						{
							i = this.PickTileIndexAt(curPos.X, curPos.Y, true, false);
							if (i == -1) break;

							// Recalculate regular rowskip values because we might have switched to the next
							// vertical multicolumn and might need to skip some tiles at the end because they
							// don't map to a valid model index
							int maxValidItemsInThisRow = this.tileCount.X - this.GetTilePos(i).X;
							skipItemsPerRow = itemsPerRow - Math.Min(this.multiColumnLength, maxValidItemsInThisRow);
							itemsPerRenderedRow = itemsPerRow - skipItemsPerRow;

							i--;
						}
					}
					else
					{
						curPos.X += this.displayedTileSize.Width + this.spacing.Width;
					}
				}
			}

			// Set the interpolation mode based on whether we're scaling up or down
			Vector2 scaleFactor = new Vector2(this.displayedTileSize.Width, this.displayedTileSize.Height) / tileset.TileSize;
			bool scalingUpClean = 
				scaleFactor.X > 1.0f &&
				scaleFactor.X == scaleFactor.Y &&
				scaleFactor.X == (int)scaleFactor.X &&
				scaleFactor.Y == (int)scaleFactor.Y;
			if (scalingUpClean)
				e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
			else
				e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

			// Draw the previously determined visible tiles accordingly
			TilesetViewPaintTileData[] rawPaintData = paintTileBuffer.Data;
			int paintedTileCount = paintTileBuffer.Count;
			for (int i = 0; i < rawPaintData.Length; i++)
			{
				if (i >= paintedTileCount) break;

				// Adjust the image rect by half the scale factor in pixels, 
				// because for some reason the nearest-neighbor-filtered image 
				// might end up too small otherwise.
				Rectangle imageRect = rawPaintData[i].ViewRect;
				if (scalingUpClean)
				{
					imageRect.Width += MathF.RoundToInt(scaleFactor.X / 2.0f);
					imageRect.Height += MathF.RoundToInt(scaleFactor.Y / 2.0f);
				}

				e.Graphics.DrawImage(
					this.tileBitmap, 
					imageRect, 
					rawPaintData[i].SourceRect, 
					GraphicsUnit.Pixel);
			}
			e.Graphics.InterpolationMode = InterpolationMode.Default;

			// Invoke the event handler
			if (this.PaintTiles != null)
				this.PaintTiles(this, new TilesetViewPaintTilesEventArgs(
					e.Graphics,
					e.ClipRectangle,
					tileset,
					this.tileBitmap,
					paintTileBuffer));
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

			// Paint the tile layer, including the user interaction state
			this.OnPaintTiles(e);

			// Overlay with disabled plate
			if (!this.Enabled)
			{
				e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(128, this.BackColor)), this.ClientRectangle);
			}
		}
		protected override void OnMouseLeave(EventArgs e)
		{
			int lastHoverIndex = this.hoverIndex;
			this.hoverIndex = -1;

			// Invoke base event handlers after updating the hover index, so that's already up-to-date
			base.OnMouseLeave(e);

			if (lastHoverIndex != -1)
			{
				if (this.HoveredTileChanged != null)
					this.HoveredTileChanged(this, new TilesetViewTileIndexChangeEventArgs(this.hoverIndex, lastHoverIndex));
			}
			this.InvalidateTile(lastHoverIndex, 0);
		}
		protected override void OnMouseMove(MouseEventArgs e)
		{
			int oldHoverIndex = this.hoverIndex;
			this.hoverIndex = this.PickTileIndexAt(e.X, e.Y);

			// Invoke base event handlers after updating the hover index, so that's already up-to-date
			base.OnMouseMove(e);

			if (oldHoverIndex != this.hoverIndex)
			{
				if (this.HoveredTileChanged != null)
					this.HoveredTileChanged(this, new TilesetViewTileIndexChangeEventArgs(this.hoverIndex, oldHoverIndex));
				this.InvalidateTile(oldHoverIndex, 0);
				this.InvalidateTile(this.hoverIndex, 0);
			}
		}
		protected override void OnMouseClick(MouseEventArgs e)
		{
			base.OnMouseClick(e);
			this.Focus();
		}

		private void DualityEditorApp_ObjectPropertyChanged(object sender, ObjectPropertyChangedEventArgs e)
		{
			if (this.tileset.Res != null)
			{
				bool affectsTileset = 
					e.HasObject(this.tileset.Res);
				bool affectsRenderConfig = 
					e.HasAnyObject(this.tileset.Res.RenderConfig) || 
					e.HasProperty(TilemapsReflectionInfo.Property_Tileset_RenderConfig);

				if (!affectsTileset && !affectsRenderConfig)
					return;

				if (affectsRenderConfig)
					this.OnTilesetChanged();
				else if (affectsTileset)
					this.UpdateContentStats();

				this.Invalidate();
			}
		}
	}
}
