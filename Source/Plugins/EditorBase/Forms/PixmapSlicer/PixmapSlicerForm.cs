using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Xml.Linq;
using Duality.Drawing;
using Duality.Editor.Controls.ToolStrip;
using Duality.Editor.Plugins.Base.Forms.PixmapSlicer.States;
using Duality.Editor.Plugins.Base.Properties;
using Duality.Resources;
using WeifenLuo.WinFormsUI.Docking;

namespace Duality.Editor.Plugins.Base.Forms
{
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
		private bool				darkMode			= false;
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
				this.stateControlToolStrip.Enabled = this.targetPixmap != null;
				this.displayedImage = this.GenerateDisplayImage();

				if (this.displayedImage != null)
				{
					ColorRgba avgColor = this.displayedImage.GetAverageColor();
					this.prevImageLum = avgColor.GetLuminance();
				}

				this.Invalidate();
			}
		}

		public PixmapSlicerForm()
		{
			InitializeComponent();

			this.stateControlToolStrip.Renderer = new DualitorToolStripProfessionalRenderer();
			this.stateControlToolStrip.Enabled = this.targetPixmap != null;

			Bitmap bmp = EditorBaseResCache.IconPixmapSlicer;
			this.Icon = Icon.FromHandle(bmp.GetHicon());

			this.SetState(new DefaultPixmapSlicerState());

			this.horizontalScrollBar.Scroll += this.ScrollBarOnScroll;
			this.verticalScrollBar.Scroll += this.ScrollBarOnScroll;

			this.horizontalScrollBar.Visible = false;
			this.verticalScrollBar.Visible = false;

			// Set styles that reduce flickering and optimize drawing
			this.SetStyle(ControlStyles.ResizeRedraw, true);
			this.SetStyle(ControlStyles.UserPaint, true);
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.Opaque, true);
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

			DualityEditorApp.ObjectPropertyChanged += this.DualityEditorApp_ObjectPropertyChanged;
			Resource.ResourceDisposing += this.Resource_ResourceDisposing;
		}

		internal void SaveUserData(XElement node)
		{
			node.SetElementValue("DarkBackground", this.buttonBrightness.Checked);
			node.SetElementValue("DisplayIndices", this.state == null
				? PixmapNumberingStyle.None
				: this.state.NumberingStyle);
		}
		internal void LoadUserData(XElement node)
		{
			bool tryParseBool;
			PixmapNumberingStyle tryParseNumeringStyle;

			if (node.GetElementValue("DarkBackground", out tryParseBool))
				this.buttonBrightness.Checked = tryParseBool;
			if (node.GetElementValue("DisplayIndices", out tryParseNumeringStyle))
				this.state.SetNumberingStyle(tryParseNumeringStyle);

			this.UpdateIndicesButton();
		}

		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);

			DualityEditorApp.ObjectPropertyChanged -= this.DualityEditorApp_ObjectPropertyChanged;
			Resource.ResourceDisposing -= this.Resource_ResourceDisposing;
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
			int scrollBarWidth = this.verticalScrollBar.Visible
				? this.verticalScrollBar.Width
				: 0;
			int scrollBarHeight = this.horizontalScrollBar.Visible
				? this.horizontalScrollBar.Height
				: 0;

			this.paintingRect = new Rectangle(
				this.ClientRectangle.X, this.ClientRectangle.Y + toolStripsHeight,
				this.ClientRectangle.Width - scrollBarWidth, this.ClientRectangle.Height - toolStripsHeight - scrollBarHeight);

			Rectangle previousImageRect = this.imageRect;
			this.imageRect = new Rectangle(
				this.paintingRect.X + 5, this.paintingRect.Y + 5,
				this.paintingRect.Width - 10, this.paintingRect.Height - 10);

			// The displayed image depends on these dimensions.
			// If they have changed, generate a new image.
			if (this.imageRect != previousImageRect)
				this.displayedImage = this.GenerateDisplayImage();

			this.horizontalScrollBar.Minimum = 0;
			this.horizontalScrollBar.Maximum = (int)((this.paintingRect.Width * this.scaleFactor) - this.imageRect.Width);
			this.horizontalScroll = this.horizontalScrollBar.Value;
			this.verticalScrollBar.Minimum = 0;
			this.verticalScrollBar.Maximum = (int)((this.paintingRect.Height * this.scaleFactor) - this.imageRect.Height);
			this.verticalScroll = this.verticalScrollBar.Value;

			// Determine the geometry of the displayed image
			// while maintaing a constant aspect ratio and using as much space as possible
			if (this.displayedImage != null)
			{
				Rectangle rectImage = new Rectangle(this.imageRect.X + 1, this.imageRect.Y + 1, this.imageRect.Width - 2, this.imageRect.Height - 2);
				Size imgSize = new Size(this.targetPixmap.Width, this.targetPixmap.Height);
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

			this.UpdateGeometry();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			// Paint checkered background
			float lum = this.darkMode ? 1 - this.prevImageLum : this.prevImageLum;
			Color brightChecker = lum > 0.5f ? Color.FromArgb(72, 72, 72) : Color.FromArgb(208, 208, 208);
			Color darkChecker = lum > 0.5f ? Color.FromArgb(56, 56, 56) : Color.FromArgb(176, 176, 176);
			using (Brush hatchBrush = new HatchBrush(HatchStyle.LargeCheckerBoard, brightChecker, darkChecker))
				e.Graphics.FillRectangle(hatchBrush, this.paintingRect);

			e.Graphics.TranslateTransform(-this.horizontalScroll * this.scaleFactor, -this.verticalScroll * this.scaleFactor);
			e.Graphics.ScaleTransform(this.scaleFactor, this.scaleFactor);

			if (this.displayedImage != null)
			{
				e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
				e.Graphics.DrawImage(this.displayedImage, this.displayedImageRect);
				e.Graphics.InterpolationMode = InterpolationMode.Default;
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
			this.Activate();
			this.state.OnMouseDown(e);
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			this.Activate();
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
			this.SetScaleFactor(this.scaleFactor + (e.Delta > 0 ? .1f : -.1f));
		}

		private void ScrollBarOnScroll(object sender, EventArgs e)
		{
			this.horizontalScroll = this.horizontalScrollBar.Value;
			this.verticalScroll = this.verticalScrollBar.Value;
			this.Invalidate(this.paintingRect);

			// Makes image panning noticably smoother by 
			// updating the display immediately
			this.Update();
		}

		private void DualityEditorApp_ObjectPropertyChanged(object sender, ObjectPropertyChangedEventArgs e)
		{
			if (e.HasObject(this.targetPixmap))
				this.Invalidate();
		}

		private void Resource_ResourceDisposing(object sender, ResourceEventArgs e)
		{
			if (e.ContentType == typeof(Pixmap) && e.Content.Res == this.targetPixmap)
				this.Close();
		}

		private void buttonBrightness_CheckedChanged(object sender, EventArgs e)
		{
			this.darkMode = this.buttonBrightness.Checked;
			this.Invalidate();
		}

		private void buttonZoomIn_Click(object sender, EventArgs e)
		{
			this.SetScaleFactor(this.scaleFactor + .2f);
		}

		private void buttonZoomOut_Click(object sender, EventArgs e)
		{
			this.SetScaleFactor(this.scaleFactor - .2f);
		}

		private void buttonDefaultZoom_Click(object sender, EventArgs e)
		{
			this.SetScaleFactor(1f);
		}

		private void buttonIndices_Click(object sender, EventArgs e)
		{
			PixmapNumberingStyle currentStyle = this.state.NumberingStyle;

			// Cycle through the available styles until we find one that works
			// or we have cycled back to the current style
			PixmapNumberingStyle newStyle = (PixmapNumberingStyle)((int)currentStyle << 1);
			if ((int)newStyle > (int) PixmapNumberingStyle.All) newStyle = PixmapNumberingStyle.None;

			PixmapNumberingStyle supportedStyles = this.state.GetSupportedNumberingStyles();
			while ((supportedStyles & newStyle) == 0 && newStyle != currentStyle)
			{
				newStyle = (PixmapNumberingStyle)((int)newStyle << 1);
				if ((int)newStyle > (int)PixmapNumberingStyle.All) newStyle = PixmapNumberingStyle.None;
			}

			this.state.SetNumberingStyle(newStyle);
			this.UpdateIndicesButton();
		}

		private void UpdateIndicesButton()
		{
			switch (this.state.NumberingStyle)
			{
				case PixmapNumberingStyle.None:
					this.buttonIndices.Image = EditorBaseResCache.IconHideIndices;
					break;
				case PixmapNumberingStyle.Hovered:
					this.buttonIndices.Image = EditorBaseResCache.IconRevealIndices;
					break;
				case PixmapNumberingStyle.All:
					this.buttonIndices.Image = EditorBaseResCache.IconShowIndices;
					break;
			}
		}

		private void SetScaleFactor(float scale)
		{
			this.scaleFactor = MathF.Max(1f, scale);
			this.horizontalScrollBar.Visible = this.scaleFactor > 1f;
			this.verticalScrollBar.Visible = this.scaleFactor > 1f;
			this.Invalidate();
		}

		private void SetState(IPixmapSlicerState action)
		{
			this.stateControlToolStrip.SuspendLayout();

			// Tear down old state
			if (this.state != null)
			{
				// Remove old states controls
				foreach (ToolStripItem item in this.state.StateControls)
				{
					this.stateControlToolStrip.Items.Remove(item);
				}
			}

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
			if (action.StateControls != null && action.StateControls.Count > 0)
			{
				foreach (ToolStripItem item in action.StateControls)
				{
					this.stateControlToolStrip.Items.Add(item);
				}
			}

			this.stateControlToolStrip.ResumeLayout();

			this.UpdateIndicesButton();
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
