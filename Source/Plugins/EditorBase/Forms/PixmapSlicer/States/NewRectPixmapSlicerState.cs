using System.Drawing;
using System.Windows.Forms;
using Duality.Editor.Plugins.Base.Properties;
using Duality.Editor.Plugins.Base.UndoRedoActions;

namespace Duality.Editor.Plugins.Base.Forms.PixmapSlicer.States
{
	/// <summary>
	/// A <see cref="IPixmapSlicerState"/> that allows the user
	/// to define new atlas rectangles for the <see cref="IPixmapSlicerState.TargetPixmap"/>
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

			this.cancelButton = new ToolStripButton(null, EditorBaseResCache.IconCancel,
				(s, e) => this.CancelState());

			this.cancelButton.ToolTipText = EditorBaseRes.ToolTip_PixmapSlicerCancel;

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
			base.OnMouseMove(e);

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

			// Keep the displayRect within bounds of the image
			displayRect = displayRect.Intersection(new Rect(
				this.DisplayBounds.X, this.DisplayBounds.Y,
				this.DisplayBounds.Width, this.DisplayBounds.Height));

			Rect atlasRect = this.GetAtlasRect(displayRect);
			atlasRect.X = MathF.RoundToInt(atlasRect.X);
			atlasRect.Y = MathF.RoundToInt(atlasRect.Y);
			atlasRect.W = MathF.RoundToInt(atlasRect.W);
			atlasRect.H = MathF.RoundToInt(atlasRect.H);
			if (atlasRect.W != 0 && atlasRect.H != 0)
				this.newAtlasRect = atlasRect;

			this.UpdateDisplay();
		}

		public override void OnKeyUp(KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape)
				this.CancelState();
		}

		public override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			if (!this.newAtlasRect.HasValue)
				return;

			Rect rect = this.GetDisplayRect(this.newAtlasRect.Value);
			float originalWidth = this.SelectedRectPen.Width;
			this.SelectedRectPen.Width /= this.Context.ScaleFactor;
			e.Graphics.DrawRectangle(this.SelectedRectPen, rect.X, rect.Y, rect.W, rect.H);
			this.SelectedRectPen.Width = originalWidth;
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
