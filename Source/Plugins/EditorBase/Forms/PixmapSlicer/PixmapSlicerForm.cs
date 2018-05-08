using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Duality.Drawing;
using Duality.Editor.Plugins.Base.Forms.PixmapSlicer.States;
using Duality.Editor.Plugins.Base.Properties;
using Duality.Resources;
using WeifenLuo.WinFormsUI.Docking;

namespace Duality.Editor.Plugins.Base.Forms.PixmapSlicer
{
	// TODO: tooltips
	public partial class PixmapSlicerForm : DockContent, IHelpProvider
	{
		/// <summary>
		/// <see cref="EventArgs"/> for general pixmap slicer
		/// state events/changes
		/// </summary>
		public class PixmapSlicerStateEventArgs : EventArgs
		{
			public Type StateType { get; private set; }

			public PixmapSlicerStateEventArgs(Type stateType)
			{
				this.StateType = stateType;
			}
		}

		private IPixmapSlicerState	state				= null;
		private Pixmap				targetPixmap		= null;
		private Bitmap				displayedImage		= null;
		private Rectangle			imageRect			= Rectangle.Empty;
		private Rectangle			paintingRect		= Rectangle.Empty;
		private Rectangle			displayedImageRect	= Rectangle.Empty;
		private float				prevImageLum		= 0;
		private float				scaleFactor			= 1f;
		private int					horizontalScroll	= 0;
		private int					verticalScroll		= 0;

		/// <summary>
		/// The <see cref="Pixmap"/> currently being sliced
		/// </summary>
		public Pixmap TargetPixmap
		{
			get { return this.targetPixmap; }
			set
			{
				this.targetPixmap = value;
				this.state.TargetPixmap = value;
				this.ResetImage();
				this.Invalidate();
			}
		}

		public PixmapSlicerForm()
		{
			InitializeComponent();

			this.SetState(new DefaultPixmapSlicerState());

			// TODO: only have this visible when scaleFactor > 1f
			this.horizontalScrollBar.Scroll += this.ScrollBarOnScroll;
			this.verticalScrollBar.Scroll += this.ScrollBarOnScroll;

			// Set styles that reduce flickering and optimize drawing
			this.SetStyle(ControlStyles.ResizeRedraw, true);
			this.SetStyle(ControlStyles.UserPaint, true);
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.Opaque, true);
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

			DualityEditorApp.ObjectPropertyChanged += this.DualityEditorApp_ObjectPropertyChanged;
		}

		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);

			DualityEditorApp.ObjectPropertyChanged -= this.DualityEditorApp_ObjectPropertyChanged;
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);
			this.Invalidate();
		}

		private void UpdateGeometry()
		{
			int toolStripsHeight = this.stateControlToolStrip.Visible
					? this.stateControlToolStrip.Height
					: 0;

			this.paintingRect = new Rectangle(
				this.ClientRectangle.X, this.ClientRectangle.Y + toolStripsHeight,
				this.ClientRectangle.Width - this.verticalScrollBar.Width, this.ClientRectangle.Height - toolStripsHeight - this.horizontalScrollBar.Height);

			this.imageRect = new Rectangle(
				this.paintingRect.X + 5, this.paintingRect.Y + 5,
				this.paintingRect.Width - 10, this.paintingRect.Height - 10);

			this.horizontalScrollBar.Minimum = 0;
			this.horizontalScrollBar.Maximum = (int)((this.paintingRect.Width * this.scaleFactor) - this.imageRect.Width);
			this.verticalScrollBar.Minimum = 0;
			this.verticalScrollBar.Maximum = (int)((this.paintingRect.Height * this.scaleFactor) - this.imageRect.Height);

			// Determine the geometry of the displayed image
			// while maintaing a constant aspect ratio and using as much space as possible
			if (this.displayedImage != null)
			{
				Rectangle rectImage = new Rectangle(this.imageRect.X + 1, this.imageRect.Y + 1, this.imageRect.Width - 2, this.imageRect.Height - 2);
				Size imgSize = this.displayedImage.Size;
				float widthForHeight = (float)imgSize.Width / imgSize.Height;
				if (widthForHeight * (imgSize.Height - rectImage.Height) > imgSize.Width - rectImage.Width)
				{
					imgSize.Height = Math.Min(rectImage.Height, imgSize.Height);
					imgSize.Width = MathF.RoundToInt(widthForHeight * imgSize.Height);
				}
				else
				{
					imgSize.Width = Math.Min(rectImage.Width, imgSize.Width);
					imgSize.Height = MathF.RoundToInt(imgSize.Width / widthForHeight);
				}
				int imageX = rectImage.X + rectImage.Width / 2 - imgSize.Width / 2;
				int imageY = rectImage.Y + rectImage.Height / 2 - imgSize.Height / 2;

				this.displayedImageRect = new Rectangle(
					imageX, imageY,
					imgSize.Width, imgSize.Height);
				this.state.DisplayBounds = this.displayedImageRect;
			}
		}

		protected override void OnInvalidated(InvalidateEventArgs e)
		{
			base.OnInvalidated(e);

			this.displayedImage = this.GenerateDisplayImage();

			this.UpdateGeometry();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			// Paint checkered background
			Color brightChecker = this.prevImageLum > 0.5f ? Color.FromArgb(48, 48, 48) : Color.FromArgb(224, 224, 224);
			Color darkChecker = this.prevImageLum > 0.5f ? Color.FromArgb(32, 32, 32) : Color.FromArgb(192, 192, 192);
			using (Brush hatchBrush = new HatchBrush(HatchStyle.LargeCheckerBoard, brightChecker, darkChecker))
				e.Graphics.FillRectangle(hatchBrush, this.paintingRect);

			e.Graphics.TranslateTransform(-this.horizontalScroll * this.scaleFactor, -this.verticalScroll * this.scaleFactor);
			e.Graphics.ScaleTransform(this.scaleFactor, this.scaleFactor);

			if (this.displayedImage != null)
			{
				e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
				e.Graphics.DrawImage(this.displayedImage,
					this.displayedImageRect.X, this.displayedImageRect.Y,
					this.displayedImageRect.Width, this.displayedImageRect.Height);
				e.Graphics.InterpolationMode = InterpolationMode.Default;

				// Draw atlas rects
				if (this.targetPixmap.Atlas != null)
				{
					using (Pen rectPen = new Pen(Color.Black, 1))
					using (Pen selectedRectPen = new Pen(Color.Blue, 1))
					{
						for (int i = 0; i < this.targetPixmap.Atlas.Count; i++)
						{
							Rect rect = this.GetDisplayRect(this.targetPixmap.Atlas[i]);
							rect.X = MathF.RoundToInt(rect.X);
							rect.Y = MathF.RoundToInt(rect.Y);
							rect.W = MathF.RoundToInt(rect.W);
							rect.H = MathF.RoundToInt(rect.H);
							e.Graphics.DrawRectangle(i == this.state.SelectedRectIndex
								? selectedRectPen
								: rectPen,
								rect.X, rect.Y, rect.W, rect.H);
						}
					}
				}
			}

			this.state.OnPaint(e);
		}

		protected override void OnKeyUp(KeyEventArgs e)
		{
			base.OnKeyUp(e);

			this.state.OnKeyUp(e);
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);

			this.state.OnMouseDown(e);
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);

			this.state.OnMouseUp(e);
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);

			this.state.OnMouseMove(e);
		}

		protected override void OnMouseWheel(MouseEventArgs e)
		{
			base.OnMouseWheel(e);
			this.scaleFactor += e.Delta > 0 ? .1f : -.1f;
			this.scaleFactor = MathF.Max(1f, this.scaleFactor);
			this.Invalidate();
		}

		private void ScrollBarOnScroll(object sender, ScrollEventArgs se)
		{
			if (se.ScrollOrientation == ScrollOrientation.HorizontalScroll)
			{
				this.horizontalScroll = se.NewValue;
				this.Invalidate();
			}
			if (se.ScrollOrientation == ScrollOrientation.VerticalScroll)
			{
				this.verticalScroll = se.NewValue;
				this.Invalidate();
			}
		}

		private void OnPixmapModified()
		{
			DualityEditorApp.NotifyObjPropChanged(this, new ObjectSelection(this.targetPixmap));
		}

		private void DualityEditorApp_ObjectPropertyChanged(object sender, ObjectPropertyChangedEventArgs e)
		{
			if (e.HasObject(this.targetPixmap))
			{
				this.Invalidate();
			}
		}

		private void SetState(IPixmapSlicerState action)
		{
			this.state = action;

			action.TargetPixmap = this.targetPixmap;
			action.DisplayBounds = this.displayedImageRect;
			action.GetAtlasRect = this.GetAtlasRect;
			action.GetDisplayRect = this.GetDisplayRect;
			action.TransformMouseCoordinates = this.TransformMouseCoordinates;

			action.DisplayUpdated += (s, e) => this.Invalidate();
			action.CursorChanged += (s, e) => this.Cursor = action.Cursor;
			action.SelectionChanged += (s, e) => this.Invalidate(this.paintingRect);
			action.StateCancelled += (s, e) =>
			{
				this.SetState(new DefaultPixmapSlicerState());
			};
			action.StateChangeRequested += this.OnStateChangeRequested;

			// Add controls for the given state
			this.stateControlToolStrip.Items.Clear();
			if (action.StateControls != null && action.StateControls.Count > 0)
			{
				foreach (ToolStripItem item in action.StateControls)
				{
					this.stateControlToolStrip.Items.Add(item);
				}
			}
			this.stateControlToolStrip.Visible = this.stateControlToolStrip.Items.Count > 0;

			this.Invalidate();
		}

		private void OnStateChangeRequested(object sender, PixmapSlicerStateEventArgs e)
		{
			IPixmapSlicerState newState = (IPixmapSlicerState)Activator.CreateInstance(e.StateType);
			this.SetState(newState);
		}

		/// <summary>
		/// Converts the given mouse coordinates to be 
		/// relative to the pixmap rendering area
		/// </summary>
		/// <param name="point">The point to transform</param>
		/// <param name="x">The transformed x coordinate</param>
		/// <param name="y">The transformed y coordinate</param>
		private void TransformMouseCoordinates(Point point, out float x, out float y)
		{
			x = point.X;
			y = point.Y;

			x += this.horizontalScroll * this.scaleFactor;
			y += this.verticalScroll * this.scaleFactor;

			x /= this.scaleFactor;
			y /= this.scaleFactor;
		}

		/// <summary>
		/// Transforms the given atlas rect to display coordinates
		/// </summary>
		/// <param name="atlasRect">
		/// A <see cref="Rect"/> relative to 
		/// the <see cref="Pixmap"/> being edited
		/// </param>
		private Rect GetDisplayRect(Rect atlasRect)
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
		private Rect GetAtlasRect(Rect displayRect)
		{
			float scale = (float)this.targetPixmap.Width / this.displayedImageRect.Width;

			displayRect.Pos -= new Vector2(this.displayedImageRect.X, this.displayedImageRect.Y);
			Rect scaledRect = displayRect.Scaled(scale, scale);
			scaledRect.Pos *= scale;
			return scaledRect;
		}

		private void ResetImage()
		{
			Bitmap baseImage = this.GenerateDisplayImage();
			if (baseImage != null)
			{
				ColorRgba avgColor = baseImage.GetAverageColor();
				this.prevImageLum = avgColor.GetLuminance();
			}
		}

		/// <summary>
		/// Returns a <see cref="Bitmap"/> of <see cref="TargetPixmap"/>
		/// of the appropriate size for display
		/// </summary>
		private Bitmap GenerateDisplayImage()
		{
			return PreviewProvider.GetPreviewImage(
				this.targetPixmap,
				this.imageRect.Width,
				this.imageRect.Height,
				PreviewSizeMode.FixedHeight);
		}

		public HelpInfo ProvideHoverHelp(Point localPos, ref bool captured)
		{
			if (this.paintingRect.Contains(localPos))
			{
				return this.state.ProvideHoverHelp(localPos, ref captured);
			}

			return HelpInfo.FromText(EditorBaseRes.Help_PixmapSlicer_Topic,
				EditorBaseRes.Help_PixmapSlicer_Desc);
		}
	}
}
