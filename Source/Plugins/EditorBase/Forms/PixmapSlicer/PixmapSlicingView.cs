using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

using Duality;
using Duality.Drawing;
using Duality.Resources;
using Duality.Editor.Plugins.Base.Forms.PixmapSlicer.States;

namespace Duality.Editor.Plugins.Base.Forms.PixmapSlicer
{
	public class PixmapSlicingView : ScrollableControl
	{
		private static readonly Brush IndexTextBackBrush = new SolidBrush(Color.FromArgb(128, Color.Black));
		private static readonly Brush IndexTextForeBrush = new SolidBrush(Color.White);
		private static readonly Pen RectPenLight = new Pen(Color.Black, 1);
		private static readonly Pen RectPenDark = new Pen(Color.White, 1);
		private static readonly Pen SelectedRectPenLight = new Pen(Color.Blue, 1);
		private static readonly Pen SelectedRectPenDark = new Pen(Color.DeepSkyBlue, 1);

		private Pixmap targetPixmap = null;
		private bool darkMode = false;
		private PixmapNumberingStyle rectNumbering = PixmapNumberingStyle.Hovered;
		private float scaleFactor = 1.0f;
		private bool allowUserSelection = true;
		private bool allowNumberingStyleChange = true;

		private int hoveredRectIndex = -1;
		private int selectedRectIndex = -1;
		private PixmapSlicingRectSide hoveredRectSide = PixmapSlicingRectSide.None;

		private Bitmap image = null;
		private Rectangle contentRect = Rectangle.Empty;
		private Rectangle imageRect = Rectangle.Empty;

		private Bitmap backBitmap = null;
		private TextureBrush backBrush = null;


		public event EventHandler TargetPixmapChanged = null;
		public event EventHandler HoveredAtlasChanged = null;
		public event EventHandler SelectedAtlasChanged = null;
		public event EventHandler NumberingStyleChanged = null;
		public event EventHandler<PaintEventArgs> PaintContentOverlay = null;


		/// <summary>
		/// The <see cref="Pixmap"/> currently being sliced
		/// </summary>
		public Pixmap TargetPixmap
		{
			get { return this.targetPixmap; }
			set
			{
				if (this.targetPixmap != value)
				{
					this.targetPixmap = value;
					this.image = this.GenerateDisplayImage();

					this.UpdateContentLayout();
					this.Invalidate();

					if (this.TargetPixmapChanged != null)
						this.TargetPixmapChanged(this, EventArgs.Empty);
				}
			}
		}
		public bool DarkMode
		{
			get { return this.darkMode; }
			set
			{
				if (this.darkMode != value)
				{
					this.darkMode = value;
					this.GenerateBackgroundPattern();
					this.Invalidate();
				}
			}
		}
		public PixmapNumberingStyle NumberingStyle
		{
			get { return this.rectNumbering; }
			set
			{
				if (!this.allowNumberingStyleChange)
					return;

				if (this.rectNumbering != value)
				{
					this.rectNumbering = value;
					this.Invalidate();

					if (this.NumberingStyleChanged != null)
						this.NumberingStyleChanged(this, EventArgs.Empty);
				}
			}
		}
		public float ScaleFactor
		{
			get { return this.scaleFactor; }
			set
			{
				if (this.scaleFactor != value)
				{
					this.scaleFactor = MathF.Max(0.01f, value);
					this.UpdateContentLayout();
					this.Invalidate();
				}
			}
		}
		public bool AllowUserSelection
		{
			get { return this.allowUserSelection; }
			set { this.allowUserSelection = value; }
		}
		public bool AllowNumberingStyleChange
		{
			get { return this.allowNumberingStyleChange; }
			set { this.allowNumberingStyleChange = value; }
		}
		public Rectangle DisplayedImageRect
		{
			get { return this.imageRect; }
		}
		public int AtlasCount
		{
			get
			{
				if (this.TargetPixmap == null) return 0;
				if (this.TargetPixmap.Atlas == null) return 0;
				return this.TargetPixmap.Atlas.Count;
			}
		}
		public int HoveredAtlasIndex
		{
			get { return this.hoveredRectIndex; }
		}
		public PixmapSlicingRectSide HoveredAtlasRectSide
		{
			get { return this.hoveredRectSide; }
		}
		public int SelectedAtlasIndex
		{
			get { return this.selectedRectIndex; }
			set
			{
				if (this.selectedRectIndex != value)
				{
					this.selectedRectIndex = value;
					this.Invalidate();

					if (this.SelectedAtlasChanged != null)
						this.SelectedAtlasChanged(this, EventArgs.Empty);
				}
			}
		}
		protected Pen RectPen
		{
			get { return this.darkMode ? RectPenDark : RectPenLight; }
		}
		protected Pen SelectedRectPen
		{
			get { return this.darkMode ? SelectedRectPenDark : SelectedRectPenLight; }
		}


		public PixmapSlicingView()
		{
			this.AutoScroll = true;

			this.SetStyle(ControlStyles.ResizeRedraw, true);
			this.SetStyle(ControlStyles.UserPaint, true);
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.Opaque, true);
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

			this.GenerateBackgroundPattern();
		}

		public void AdjustZoom(float targetScaleFactor, bool preferPixelPerfectZoom = true)
		{
			float preferredScale = targetScaleFactor;
			if (preferPixelPerfectZoom)
			{
				// Lock to 1.0f when reducing zoom
				if ((this.scaleFactor > 1.0f && targetScaleFactor < 1.0f) || 
					(this.scaleFactor < 1.0f && targetScaleFactor > 1.0f))
				{
					preferredScale = 1.0f;
				}
				// Lock to N when target zooming in at higher levels
				else if (preferredScale >= 1.9f)
				{
					preferredScale = MathF.Round(preferredScale);

					// Adjust by at least one step
					if (preferredScale == this.scaleFactor)
					{
						if (targetScaleFactor > this.scaleFactor)
							preferredScale += 1.0f;
						else if (targetScaleFactor < this.scaleFactor)
							preferredScale -= 1.0f;
					}
				}
			}
			this.ScaleFactor = preferredScale;
		}
		public void ZoomToFit()
		{
			if (this.image != null)
			{
				Vector2 imageSize = new Vector2(this.image.Width, this.image.Height);
				Vector2 targetSize = new Vector2(this.contentRect.Width, this.contentRect.Height);
				Vector2 fitSize;
				if (imageSize.X > targetSize.X || imageSize.Y > targetSize.Y)
					fitSize = TargetResize.Fit.Apply(imageSize, targetSize);
				else
					fitSize = imageSize;

				this.ScaleFactor = fitSize.X / imageSize.X;
			}
			else
			{
				this.ScaleFactor = 1.0f;
			}
		}
		public void ClearSelection()
		{
			this.SelectedAtlasIndex = -1;
		}

		public Rect GetAtlasRect(int index)
		{
			if (this.targetPixmap == null) return Rect.Empty;
			if (this.targetPixmap.Atlas == null) return Rect.Empty;
			if (index < 0) return Rect.Empty;
			if (index >= this.targetPixmap.Atlas.Count) return Rect.Empty;

			return this.targetPixmap.Atlas[index];
		}

		/// <summary>
		/// Transforms the given atlas rect to display coordinates
		/// </summary>
		/// <param name="atlasRect">
		/// A <see cref="Rect"/> relative to 
		/// the <see cref="Pixmap"/> being edited
		/// </param>
		public Rectangle GetDisplayRect(Rect atlasRect)
		{
			Point topLeft = this.GetDisplayPos(atlasRect.TopLeft);
			Point bottomRight = this.GetDisplayPos(atlasRect.BottomRight);
			return new Rectangle(
				topLeft.X, 
				topLeft.Y, 
				bottomRight.X - topLeft.X,
				bottomRight.Y - topLeft.Y);
		}
		/// <summary>
		/// Transforms the given rectangle in display coordinates
		/// to atlas coordinates
		/// </summary>
		public Rect GetAtlasRect(Rectangle displayRect)
		{
			Vector2 topLeft = this.GetAtlasPos(new Point(displayRect.Left, displayRect.Top));
			Vector2 bottomRight = this.GetAtlasPos(new Point(displayRect.Right, displayRect.Bottom));
			return new Rect(
				topLeft.X,
				topLeft.Y,
				bottomRight.X - topLeft.X,
				bottomRight.Y - topLeft.Y);
		}
		/// <summary>
		/// Converts the given local / client coordinates into <see cref="Pixmap"/> atlas space.
		/// </summary>
		/// <param name="point">The point to transform</param>
		public Vector2 GetAtlasPos(Point point, bool scrolled = true)
		{
			if (scrolled)
			{
				point.X -= this.AutoScrollPosition.X;
				point.Y -= this.AutoScrollPosition.Y;
			}

			Vector2 pixmapPos;
			pixmapPos.X = this.targetPixmap.Width * (point.X - this.imageRect.X) / (float)this.imageRect.Width;
			pixmapPos.Y = this.targetPixmap.Height * (point.Y - this.imageRect.Y) / (float)this.imageRect.Height;

			return pixmapPos;
		}
		public Point GetDisplayPos(Vector2 atlasPos, bool scrolled = true)
		{
			Point displayPos = Point.Empty;
			if (this.targetPixmap != null && this.targetPixmap.Width > 0 && this.targetPixmap.Height > 0)
			{
				displayPos.X = (int)(this.imageRect.X + (atlasPos.X / this.targetPixmap.Width) * (float)this.imageRect.Width);
				displayPos.Y = (int)(this.imageRect.Y + (atlasPos.Y / this.targetPixmap.Height) * (float)this.imageRect.Height);
			}

			if (scrolled)
			{
				displayPos.X += this.AutoScrollPosition.X;
				displayPos.Y += this.AutoScrollPosition.Y;
			}

			return displayPos;
		}

		public void InvalidatePixmap()
		{
			if (this.targetPixmap == null) return;

			this.InvalidatePixmap(new Rect(
				0.0f, 
				0.0f, 
				this.targetPixmap.Width, 
				this.targetPixmap.Height));
		}
		public void InvalidatePixmap(Rect atlasRect)
		{
			if (atlasRect.W == 0.0f || atlasRect.H == 0.0f) return;
			if (this.targetPixmap == null) return;

			Rectangle viewRect = new Rectangle(
				(int)(atlasRect.X * this.scaleFactor),
				(int)(atlasRect.Y * this.scaleFactor),
				(int)(atlasRect.W * this.scaleFactor),
				(int)(atlasRect.H * this.scaleFactor));

			viewRect.X += this.imageRect.X;
			viewRect.Y += this.imageRect.Y;

			viewRect.X += this.AutoScrollPosition.X;
			viewRect.Y += this.AutoScrollPosition.Y;

			// Grow the rect slightly to make sure we'll invalidate around potential borders
			viewRect.Inflate(10, 10);

			this.Invalidate(viewRect);
		}

		/// <summary>
		/// Returns a <see cref="Bitmap"/> of <see cref="TargetPixmap"/>
		/// of the appropriate size for display
		/// </summary>
		private Bitmap GenerateDisplayImage()
		{
			if (this.TargetPixmap == null)
				return null;

			PixelData layer = this.TargetPixmap.MainLayer;
			if (layer == null)
				return null;

			return layer.ToBitmap();
		}

		private void GenerateBackgroundPattern()
		{
			// Dispose old background textures
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

			// Generate background texture
			{
				Brush darkBrush = new SolidBrush(this.darkMode ? Color.FromArgb(56, 56, 56) : Color.FromArgb(176, 176, 176));
				Brush brightBrush = new SolidBrush(this.darkMode ? Color.FromArgb(72, 72, 72) : Color.FromArgb(208, 208, 208));
				Size cellSize = new Size(8, 8);

				this.backBitmap = new Bitmap(
					cellSize.Width * 2,
					cellSize.Height * 2);
				using (Graphics g = Graphics.FromImage(this.backBitmap))
				{
					g.Clear(Color.Transparent);

					for (int i = 0; i < 4; i++)
					{
						int lineIndex = i / 2;
						bool evenLine = (lineIndex % 2) == 0;
						bool darkCell = (i % 2) == (evenLine ? 0 : 1);
						Point cellPos = new Point(
							(i % 2) * cellSize.Width,
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
		private void UpdateAutoScroll()
		{
			Size autoScrollSize = new Size(
				MathF.RoundToInt(this.imageRect.Width),
				MathF.RoundToInt(this.imageRect.Height));
			autoScrollSize.Width += this.Padding.Horizontal;
			autoScrollSize.Height += this.Padding.Vertical;

			if (this.AutoScrollMinSize != autoScrollSize)
				this.AutoScrollMinSize = autoScrollSize;
		}
		private void UpdateContentLayout()
		{
			this.contentRect = new Rectangle(
				this.ClientRectangle.X + this.Padding.Left,
				this.ClientRectangle.Y + this.Padding.Top,
				this.ClientRectangle.Width - this.Padding.Horizontal,
				this.ClientRectangle.Height - this.Padding.Vertical);

			// Determine the geometry of the displayed image
			// while maintaing a constant aspect ratio and using as much space as possible
			if (this.image != null)
			{
				Vector2 imageSize = new Vector2(this.image.Width, this.image.Height);
				Vector2 displayedSize = imageSize * this.scaleFactor;

				this.imageRect = new Rectangle(
					MathF.RoundToInt(this.contentRect.X), 
					MathF.RoundToInt(this.contentRect.Y),
					MathF.RoundToInt(displayedSize.X),
					MathF.RoundToInt(displayedSize.Y));

				// Center image on either axis when smaller than the available space
				if (displayedSize.X < this.contentRect.Width)
					this.imageRect.X = (int)(this.contentRect.X + this.contentRect.Width * 0.5f - displayedSize.X * 0.5f);
				if (displayedSize.Y < this.contentRect.Height)
					this.imageRect.Y = (int)(this.contentRect.Y + this.contentRect.Height * 0.5f - displayedSize.Y * 0.5f);
			}

			this.UpdateAutoScroll();
		}

		/// <summary>
		/// Draws an index value in the middle of the given Rect
		/// </summary>
		public void DrawRectIndex(Graphics g, Rectangle displayRect, int index)
		{
			string indexText = index.ToString();
			SizeF textSize = g.MeasureString(indexText, this.Font);
			RectangleF textRect = new RectangleF(
				displayRect.X,
				displayRect.Y,
				3 + textSize.Width,
				3 + textSize.Height);
			textRect.X += (displayRect.Width - textRect.Width) / 2;
			textRect.Y += (displayRect.Height - textRect.Height) / 2;
			PointF textPos = new PointF(
				textRect.X + textRect.Width * 0.5f - textSize.Width * 0.5f,
				textRect.Y + textRect.Height * 0.5f - textSize.Height * 0.5f);

			g.FillRectangle(IndexTextBackBrush, textRect);
			g.DrawString(indexText, this.Font, IndexTextForeBrush, textPos.X, textPos.Y);
		}

		private PixmapSlicingRectSide GetHoveredRectSide(Rectangle rect, int x, int y, int maxDistanceToEdge)
		{
			bool withinRange =
				x + maxDistanceToEdge >= rect.Left &&
				x - maxDistanceToEdge <= rect.Right &&
				y + maxDistanceToEdge >= rect.Top &&
				y - maxDistanceToEdge <= rect.Bottom;
			if (!withinRange) return PixmapSlicingRectSide.None;

			bool onTopEdge = Math.Abs(rect.Top - y) <= maxDistanceToEdge;
			bool onBottomEdge = Math.Abs(rect.Bottom - y) <= maxDistanceToEdge;
			bool onLeftEdge = Math.Abs(rect.Left - x) <= maxDistanceToEdge;
			bool onRightEdge = Math.Abs(rect.Right - x) <= maxDistanceToEdge;
			if (onTopEdge) return PixmapSlicingRectSide.Top;
			if (onBottomEdge) return PixmapSlicingRectSide.Bottom;
			if (onLeftEdge) return PixmapSlicingRectSide.Left;
			if (onRightEdge) return PixmapSlicingRectSide.Right;

			return PixmapSlicingRectSide.None;
		}
		/// <summary>
		/// Returns whether or not the specified rect is hovered. Outputs
		/// the specific side of the rect that was hovered, if any.
		/// </summary>
		private bool IsRectHovered(Rectangle displayRect, int x, int y, int maxDistanceToEdge, out PixmapSlicingRectSide hoveredSide)
		{
			PixmapSlicingRectSide side = this.GetHoveredRectSide(displayRect, x, y, maxDistanceToEdge);
			if (side != PixmapSlicingRectSide.None || displayRect.Contains(x, y))
			{
				hoveredSide = side;
				return true;
			}
			hoveredSide = PixmapSlicingRectSide.None;
			return false;
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);
			this.UpdateContentLayout();
			this.Invalidate();
		}
		protected override void OnPaddingChanged(EventArgs e)
		{
			base.OnPaddingChanged(e);
			this.UpdateContentLayout();
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

			// Paint checkered background
			if (this.backBrush != null)
			{
				Point offset = this.GetDisplayPos(Vector2.Zero);
				this.backBrush.ResetTransform();
				this.backBrush.TranslateTransform(offset.X, offset.Y);
				e.Graphics.FillRectangle(this.backBrush, this.ClientRectangle);
			}

			// Draw image outline
			if (this.image != null)
			{
				Rectangle outlineRect = this.GetDisplayRect(new Rect(this.targetPixmap.Width, this.targetPixmap.Height));

				// Offset the rect, so that it won't overlap the actual image area
				outlineRect.Inflate(1,1);

				e.Graphics.DrawRectangle(
					Pens.Red,
					outlineRect);
			}

			// Draw target image
			if (this.image != null)
			{
				bool isIntScaling = MathF.Abs(MathF.RoundToInt(this.scaleFactor) - this.scaleFactor) < 0.0001f;

				// Choose filtering mode depending on whether we're scaling by a full Nx factor
				e.Graphics.InterpolationMode =
					isIntScaling ?
					InterpolationMode.NearestNeighbor :
					InterpolationMode.HighQualityBicubic;
				e.Graphics.PixelOffsetMode =
					isIntScaling ?
					PixelOffsetMode.Half :
					PixelOffsetMode.None;

				Rectangle scrolledImageRect = this.imageRect;
				scrolledImageRect.X += this.AutoScrollPosition.X;
				scrolledImageRect.Y += this.AutoScrollPosition.Y;
				e.Graphics.DrawImage(this.image, scrolledImageRect);

				e.Graphics.InterpolationMode = InterpolationMode.Default;
				e.Graphics.PixelOffsetMode = PixelOffsetMode.Default;
			}

			// Draw rect outlines and indices
			if (this.targetPixmap != null && this.targetPixmap.Atlas != null)
			{
				for (int i = 0; i < this.targetPixmap.Atlas.Count; i++)
				{
					Rect atlasRect = this.targetPixmap.Atlas[i];
					Rectangle displayRect = this.GetDisplayRect(atlasRect);
					e.Graphics.DrawRectangle(
						this.RectPen,
						displayRect);

					bool isHoveredOrSelected = (this.hoveredRectIndex == i || this.selectedRectIndex == i);
					if (this.rectNumbering == PixmapNumberingStyle.All || 
						(this.rectNumbering == PixmapNumberingStyle.Hovered && isHoveredOrSelected))
					{
						this.DrawRectIndex(e.Graphics, displayRect, i);
					}
				}

				// Draw selected outline on top of all others
				if (this.selectedRectIndex != -1)
				{
					Rect atlasRect = this.GetAtlasRect(this.selectedRectIndex);
					Rectangle displayRect = this.GetDisplayRect(atlasRect);
					e.Graphics.DrawRectangle(
						this.SelectedRectPen,
						displayRect);
				}
			}

			if (this.PaintContentOverlay != null)
				this.PaintContentOverlay(this, e);
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);

			if (this.targetPixmap == null || this.targetPixmap.Atlas == null)
				return;

			PixmapSlicingRectSide prevHoveredSide = this.hoveredRectSide;
			int prevHoveredIndex = this.hoveredRectIndex;

			// Update hover states for slice rects.
			this.hoveredRectIndex = -1;
			this.hoveredRectSide = PixmapSlicingRectSide.None;
			int hoveredRectSideIndex = -1;
			int hoveredRectCenterIndex = -1;
			// Check the selected rect first
			if (this.selectedRectIndex != -1)
			{
				Rectangle displayRect = this.GetDisplayRect(this.targetPixmap.Atlas[this.selectedRectIndex]);
				PixmapSlicingRectSide side;
				if (this.IsRectHovered(displayRect, e.X, e.Y, 3, out side))
				{
					if (side == PixmapSlicingRectSide.None)
						hoveredRectCenterIndex = this.selectedRectIndex;
					else
					{
						this.hoveredRectSide = side;
						hoveredRectSideIndex = this.selectedRectIndex;
					}
				}
			}
			// If none of the selected rects sides were hovered
			if (this.hoveredRectSide == PixmapSlicingRectSide.None)
			{
				// We're using reverse order so atlas rects with higher indices get
				// precedence on overlapping hover checks.
				for (int i = this.targetPixmap.Atlas.Count - 1; i >= 0; i--)
				{
					Rectangle displayRect = this.GetDisplayRect(this.targetPixmap.Atlas[i]);

					PixmapSlicingRectSide side;
					if (this.IsRectHovered(displayRect, e.X, e.Y, 3, out side))
					{
						if (side == PixmapSlicingRectSide.None && hoveredRectCenterIndex == -1)
							hoveredRectCenterIndex = i;
						else if (side != PixmapSlicingRectSide.None)
						{
							this.hoveredRectSide = side;
							hoveredRectSideIndex = i;
							break;
						}
					}
				}
			}
			this.hoveredRectIndex = hoveredRectSideIndex != -1
				? hoveredRectSideIndex
				: hoveredRectCenterIndex;

			// Invalidate regions when changing hover states
			if (this.rectNumbering == PixmapNumberingStyle.Hovered && this.hoveredRectIndex != prevHoveredIndex)
			{
				this.InvalidatePixmap(this.GetAtlasRect(this.hoveredRectIndex));
				this.InvalidatePixmap(this.GetAtlasRect(prevHoveredIndex));
			}

			// Fire events for hover state changes
			if (this.hoveredRectIndex != prevHoveredIndex || this.hoveredRectSide != prevHoveredSide)
			{
				if (this.HoveredAtlasChanged != null)
					this.HoveredAtlasChanged(this, EventArgs.Empty);
			}
		}
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			if (this.allowUserSelection)
				this.SelectedAtlasIndex = this.HoveredAtlasIndex;
		}
	}
}
