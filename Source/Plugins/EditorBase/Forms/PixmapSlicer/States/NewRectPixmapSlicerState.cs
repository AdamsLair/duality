using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Duality.Editor.Plugins.Base.Forms.PixmapSlicer.States
{
	/// <summary>
	/// A <see cref="IPixmapSlicerState"/> that allows the user
	/// to define new atlas rectangles for the <see cref="TargetPixmap"/>
	/// </summary>
	public class NewRectPixmapSlicerState : PixmapSlicerState
	{
		private ToolStripButton cancelButton = null;

		private bool	mouseDown			= false;
		private PointF	rectAddStart		= Point.Empty;

		public NewRectPixmapSlicerState()
		{
			this.Cursor = Cursors.Default;

			this.cancelButton = new ToolStripButton("Cancel", null,
				(s, e) => this.CancelState());

			this.StateControls.Add(this.cancelButton);
		}

		public override void OnMouseDown(MouseEventArgs e)
		{
			this.mouseDown = true;
		}

		public override void OnMouseUp(MouseEventArgs e)
		{
			if (this.mouseDown)
			{
				this.mouseDown = false;
				this.CancelState();
			}
		}

		public override void OnMouseMove(MouseEventArgs e)
		{
			if (this.TargetPixmap == null || !this.mouseDown)
				return;

			float x, y;
			this.TransformMouseCoordinates(e.Location, out x, out y);

			// Check if rect creation hasn't started yet
			if (this.SelectedRectIndex == -1)
			{
				if (this.TargetPixmap.Atlas == null)
					this.TargetPixmap.Atlas = new List<Rect>();
				this.rectAddStart = new PointF(x, y);
				Rect newDisplayRect = new Rect(x, y, 0, 0);
				this.TargetPixmap.Atlas.Add(this.GetAtlasRect(newDisplayRect));
				this.SelectedRectIndex = this.TargetPixmap.Atlas.Count - 1;
			}

			float rx = MathF.Min(x, this.rectAddStart.X);
			float ry = MathF.Min(y, this.rectAddStart.Y);
			float rw = MathF.Abs(x - this.rectAddStart.X);
			float rh = MathF.Abs(y - this.rectAddStart.Y);

			// Clip the rect to the image
			Rect displayRect = new Rect(rx, ry, rw, rh);
			Rect atlastRect = this.GetAtlasRect(displayRect);
			atlastRect = atlastRect.Intersection(new Rect(0, 0, this.TargetPixmap.Width, this.TargetPixmap.Height));

			this.SetPixmapAtlasRect(atlastRect, this.SelectedRectIndex);
		}

		public override void OnKeyUp(KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape)
				this.CancelState();
		}

		private void SetPixmapAtlasRect(Rect rect, int index)
		{
			this.TargetPixmap.Atlas[index] = rect;
			this.UpdatePixmap();
		}
	}
}
