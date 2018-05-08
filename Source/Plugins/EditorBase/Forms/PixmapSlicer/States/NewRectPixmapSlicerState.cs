using System.Drawing;
using System.Windows.Forms;
using Duality.Editor.Plugins.Base.Properties;
using Duality.Editor.Plugins.Base.UndoRedoActions;

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
		private Rect?	newAtlasRect		= null;

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
				this.CommitRect();
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
			if (!this.newAtlasRect.HasValue)
			{
				this.rectAddStart = new PointF(x, y);
				this.newAtlasRect = this.GetAtlasRect(new Rect(x, y, 0, 0));
			}

			float rx = MathF.Min(x, this.rectAddStart.X);
			float ry = MathF.Min(y, this.rectAddStart.Y);
			float rw = MathF.Abs(x - this.rectAddStart.X);
			float rh = MathF.Abs(y - this.rectAddStart.Y);

			// Clip the rect to the image
			Rect displayRect = new Rect(rx, ry, rw, rh);
			Rect atlastRect = this.GetAtlasRect(displayRect);
			atlastRect = atlastRect.Intersection(new Rect(0, 0, this.TargetPixmap.Width, this.TargetPixmap.Height));

			this.newAtlasRect = atlastRect;

			this.UpdateDisplay();
		}

		public override void OnKeyUp(KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape)
				this.CancelState();
		}

		public override void OnPaint(PaintEventArgs e)
		{
			if (!this.newAtlasRect.HasValue)
				return;

			Rect rect = this.GetDisplayRect(this.newAtlasRect.Value);

			rect.X = MathF.RoundToInt(rect.X);
			rect.Y = MathF.RoundToInt(rect.Y);
			rect.W = MathF.RoundToInt(rect.W);
			rect.H = MathF.RoundToInt(rect.H);

			using (Pen rectPen = new Pen(Color.Blue, 1))
			{
				e.Graphics.DrawRectangle(rectPen,
					rect.X, rect.Y, rect.W, rect.H);
			}
		}

		private void CommitRect()
		{
			int index = this.TargetPixmap.Atlas != null 
				? this.TargetPixmap.Atlas.Count 
				: 0;

			UndoRedoManager.Do(new SetAtlasRectAction(this.newAtlasRect.Value, index, new []{ this.TargetPixmap }));
		}

		public override HelpInfo ProvideHoverHelp(Point localPos, ref bool captured)
		{
			return HelpInfo.FromText(EditorBaseRes.Help_PixmapSlicerNewRect_Topic,
				EditorBaseRes.Help_PixmapSlicerNewRect_Desc);
		}
	}
}
