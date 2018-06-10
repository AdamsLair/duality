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
		private bool            mouseDown    = false;
		private Point           rectAddStart = Point.Empty;
		private Rect?           newAtlasRect = null;


		public NewRectPixmapSlicerState()
		{
			this.Cursor = Cursors.Default;

			this.cancelButton = new ToolStripButton(null, EditorBaseResCache.IconCancel,
				(s, e) => this.CancelState());

			this.cancelButton.ToolTipText = EditorBaseRes.ToolTip_PixmapSlicerCancel;

			this.StateControls.Add(this.cancelButton);
		}

		private void CommitRect()
		{
			int index = this.TargetPixmap.Atlas != null
				? this.TargetPixmap.Atlas.Count
				: 0;

			UndoRedoManager.Do(new SetAtlasRectAction(this.newAtlasRect.Value, index, new[] { this.TargetPixmap }));
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

			// Check if rect creation hasn't started yet
			if (!this.newAtlasRect.HasValue)
			{
				this.rectAddStart = new Point(e.X, e.Y);
				this.newAtlasRect = this.View.GetAtlasRect(new Rectangle(e.X, e.Y, 0, 0));
			}

			int rx = MathF.Min(e.X, this.rectAddStart.X);
			int ry = MathF.Min(e.Y, this.rectAddStart.Y);
			int rw = MathF.Abs(e.X - this.rectAddStart.X);
			int rh = MathF.Abs(e.Y - this.rectAddStart.Y);

			// Clip the rect to the image
			Rectangle displayRect = new Rectangle(rx, ry, rw, rh);

			// Keep the displayRect within bounds of the image
			displayRect.Intersect(this.View.DisplayedImageRect);

			Rect atlasRect = this.View.GetAtlasRect(displayRect);
			atlasRect.X = MathF.RoundToInt(atlasRect.X);
			atlasRect.Y = MathF.RoundToInt(atlasRect.Y);
			atlasRect.W = MathF.RoundToInt(atlasRect.W);
			atlasRect.H = MathF.RoundToInt(atlasRect.H);
			if (atlasRect.W != 0 && atlasRect.H != 0)
			{
				Rect updatedArea = this.newAtlasRect.Value.ExpandedToContain(atlasRect);
				this.newAtlasRect = atlasRect;
				this.UpdateDisplay(updatedArea);
			}
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

			Rectangle rect = this.View.GetDisplayRect(this.newAtlasRect.Value);
			e.Graphics.DrawRectangle(this.SelectedRectPen, rect);
		}

		public override HelpInfo ProvideHoverHelp(Point localPos, ref bool captured)
		{
			return HelpInfo.FromText(EditorBaseRes.Help_PixmapSlicerNewRect_Topic,
				EditorBaseRes.Help_PixmapSlicerNewRect_Desc);
		}
	}
}
