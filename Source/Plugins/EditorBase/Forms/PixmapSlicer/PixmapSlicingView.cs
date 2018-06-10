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

		private Pixmap targetPixmap = null;
		private Bitmap displayedImage = null;
		private Rectangle imageRect = Rectangle.Empty;
		private Rectangle contentRect = Rectangle.Empty;
		private Rectangle displayedImageRect = Rectangle.Empty;
		private float prevImageLum = 0f;
		private bool darkMode = false;
		private PixmapNumberingStyle rectNumbering = PixmapNumberingStyle.Hovered;
		private float scaleFactor = 1.0f;
		private int hoveredRectIndex = -1;


		public event EventHandler<PaintEventArgs> PaintContentOverlay = null;


		/// <summary>
		/// The <see cref="Pixmap"/> currently being sliced
		/// </summary>
		public Pixmap TargetPixmap
		{
			get { return this.targetPixmap; }
			set
			{
				this.targetPixmap = value;
				this.displayedImage = this.GenerateDisplayImage();

				if (this.displayedImage != null)
				{
					ColorRgba avgColor = this.displayedImage.GetAverageColor();
					this.prevImageLum = avgColor.GetLuminance();
				}

				this.UpdateContentLayout();
				this.Invalidate();
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
					this.Invalidate();
				}
			}
		}
		public PixmapNumberingStyle NumberingStyle
		{
			get { return this.rectNumbering; }
			set
			{
				if (this.rectNumbering != value)
				{
					this.rectNumbering = value;
					this.Invalidate();
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
					this.scaleFactor = MathF.Max(1.0f, value);
					this.UpdateContentLayout();
					this.Invalidate();
				}
			}
		}
		public Rectangle DisplayedImageRect
		{
			get { return this.displayedImageRect; }
		}
		public int HoveredAtlasIndex
		{
			get { return this.hoveredRectIndex; }
		}
		protected Pen RectPen
		{
			get { return this.darkMode ? RectPenDark : RectPenLight; }
		}


		public PixmapSlicingView()
		{
			this.AutoScroll = true;

			this.SetStyle(ControlStyles.ResizeRedraw, true);
			this.SetStyle(ControlStyles.UserPaint, true);
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.Opaque, true);
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
		}

		public void ZoomToFit()
		{
			if (this.displayedImage != null)
			{
				Vector2 imageSize = new Vector2(this.displayedImage.Width, this.displayedImage.Height);
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

		/// <summary>
		/// Transforms the given atlas rect to display coordinates
		/// </summary>
		/// <param name="atlasRect">
		/// A <see cref="Rect"/> relative to 
		/// the <see cref="Pixmap"/> being edited
		/// </param>
		public Rect GetDisplayRect(Rect atlasRect)
		{
			float scale = (float)this.displayedImageRect.Width / this.targetPixmap.Width;

			Rect scaledRect = atlasRect.Scaled(scale, scale);
			Vector2 scaledPos = atlasRect.Pos * scale;
			scaledRect.Pos = scaledPos + new Vector2(this.displayedImageRect.X, this.displayedImageRect.Y);
			return scaledRect;
		}
		/// <summary>
		/// Transforms the given rectangle in display coordinates
		/// to atlas coordinates
		/// </summary>
		public Rect GetAtlasRect(Rect displayRect)
		{
			float scale = (float)this.targetPixmap.Width / this.displayedImageRect.Width;

			displayRect.Pos -= new Vector2(this.displayedImageRect.X, this.displayedImageRect.Y);
			Rect scaledRect = displayRect.Scaled(scale, scale);
			scaledRect.Pos *= scale;

			return scaledRect;
		}
		/// <summary>
		/// Converts the given local / client coordinates into <see cref="Pixmap"/> atlas space.
		/// </summary>
		/// <param name="point">The point to transform</param>
		public Vector2 GetAtlasPos(Point point)
		{
			point.X -= this.AutoScrollPosition.X;
			point.Y -= this.AutoScrollPosition.Y;

			Vector2 pixmapPos;
			pixmapPos.X = this.targetPixmap.Width * (point.X - this.displayedImageRect.X) / (float)this.displayedImageRect.Width;
			pixmapPos.Y = this.targetPixmap.Height * (point.Y - this.displayedImageRect.Y) / (float)this.displayedImageRect.Height;

			return pixmapPos;
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
		public void InvalidatePixmap(Rect pixmapRect)
		{
			if (this.targetPixmap == null) return;

			Rectangle viewRect = new Rectangle(
				(int)(pixmapRect.X * this.scaleFactor),
				(int)(pixmapRect.Y * this.scaleFactor),
				(int)(pixmapRect.W * this.scaleFactor),
				(int)(pixmapRect.H * this.scaleFactor));

			viewRect.X += this.displayedImageRect.X;
			viewRect.Y += this.displayedImageRect.Y;

			viewRect.X -= this.AutoScrollPosition.X;
			viewRect.Y -= this.AutoScrollPosition.Y;

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

		private void UpdateAutoScroll()
		{
			Size autoScrollSize = new Size(
				MathF.RoundToInt(this.displayedImageRect.Width),
				MathF.RoundToInt(this.displayedImageRect.Height));
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
			if (this.displayedImage != null)
			{
				Vector2 imageSize = new Vector2(this.displayedImage.Width, this.displayedImage.Height);
				Vector2 displayedSize = imageSize * this.scaleFactor;

				this.displayedImageRect = new Rectangle(
					MathF.RoundToInt(this.contentRect.X), 
					MathF.RoundToInt(this.contentRect.Y),
					MathF.RoundToInt(displayedSize.X),
					MathF.RoundToInt(displayedSize.Y));
			}

			this.UpdateAutoScroll();
		}

		/// <summary>
		/// Draws an index value in the middle of the given Rect
		/// </summary>
		public void DrawRectIndex(Graphics g, Rect displayRect, int index)
		{
			string indexText = index.ToString();
			SizeF textSize = g.MeasureString(indexText, this.Font);
			RectangleF textRect = new RectangleF(
				displayRect.X,
				displayRect.Y,
				3 + textSize.Width,
				3 + textSize.Height);
			textRect.X += (displayRect.W - textRect.Width) / 2;
			textRect.Y += (displayRect.H - textRect.Height) / 2;
			PointF textPos = new PointF(
				textRect.X + textRect.Width * 0.5f - textSize.Width * 0.5f,
				textRect.Y + textRect.Height * 0.5f - textSize.Height * 0.5f);

			g.FillRectangle(IndexTextBackBrush, textRect);
			g.DrawString(indexText, this.Font, IndexTextForeBrush, textPos.X, textPos.Y);
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
			float lum = this.darkMode ? 1 - this.prevImageLum : this.prevImageLum;
			Color brightChecker = lum > 0.5f ? Color.FromArgb(72, 72, 72) : Color.FromArgb(208, 208, 208);
			Color darkChecker = lum > 0.5f ? Color.FromArgb(56, 56, 56) : Color.FromArgb(176, 176, 176);
			using (Brush hatchBrush = new HatchBrush(HatchStyle.LargeCheckerBoard, brightChecker, darkChecker))
			{
				e.Graphics.FillRectangle(hatchBrush, this.ClientRectangle);
			}

			// Draw image outline
			if (this.displayedImage != null)
			{
				e.Graphics.DrawRectangle(Pens.Red, this.displayedImageRect);
			}

			// Draw target image
			if (this.displayedImage != null)
			{
				bool isIntScaling = MathF.Abs(MathF.RoundToInt(this.scaleFactor) - this.scaleFactor) < 0.0001f;

				// Choose filtering mode depending on whether we're scaling by a full Nx factor
				e.Graphics.InterpolationMode =
					isIntScaling ?
					InterpolationMode.NearestNeighbor :
					InterpolationMode.Bilinear;
				e.Graphics.DrawImage(this.displayedImage, this.displayedImageRect);
				e.Graphics.InterpolationMode = InterpolationMode.Default;
			}

			// Draw rect outlines and indices
			if (this.targetPixmap != null && this.targetPixmap.Atlas != null)
			{
				for (int i = 0; i < this.targetPixmap.Atlas.Count; i++)
				{
					Rect atlasRect = this.targetPixmap.Atlas[i];
					Rect displayRect = this.GetDisplayRect(atlasRect);
					e.Graphics.DrawRectangle(
						this.RectPen, 
						displayRect.X,
						displayRect.Y,
						displayRect.W, 
						displayRect.H);

					if (this.rectNumbering == PixmapNumberingStyle.All || 
						(this.rectNumbering == PixmapNumberingStyle.Hovered && this.hoveredRectIndex == i))
					{
						this.DrawRectIndex(e.Graphics, displayRect, i);
					}
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

			// Update hover states for slice rects
			int originalHoveredIndex = this.hoveredRectIndex;
			this.hoveredRectIndex = -1;
			for (int i = 0; i < this.targetPixmap.Atlas.Count; i++)
			{
				Rect displayRect = this.GetDisplayRect(this.targetPixmap.Atlas[i]);
				if (displayRect.Contains(e.X, e.Y))
				{
					this.hoveredRectIndex = i;
					break;
				}
			}

			// Invalidate regions when changing hover states
			if (this.rectNumbering == PixmapNumberingStyle.Hovered && this.hoveredRectIndex != originalHoveredIndex)
			{
				if (this.hoveredRectIndex >= 0 && this.hoveredRectIndex < this.targetPixmap.Atlas.Count)
					{
					Rect hoveredSliceArea = this.targetPixmap.Atlas[this.hoveredRectIndex];
					this.InvalidatePixmap(hoveredSliceArea);
				}
				if (originalHoveredIndex >= 0 && originalHoveredIndex < this.targetPixmap.Atlas.Count)
				{
					Rect prevHoveredSliceArea = this.targetPixmap.Atlas[originalHoveredIndex];
					this.InvalidatePixmap(prevHoveredSliceArea);
				}
			}
		}
		protected override void OnMouseWheel(MouseEventArgs e)
		{
			base.OnMouseWheel(e);
			this.ScaleFactor = this.scaleFactor + this.scaleFactor * (e.Delta > 0 ? 0.1f : -0.1f);
		}
	}
}
