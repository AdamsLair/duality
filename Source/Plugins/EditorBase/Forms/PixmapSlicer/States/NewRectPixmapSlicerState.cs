using System;
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
		private ToolStripButton      cancelButton       = null;
		private Point                rectAddStart       = Point.Empty;
		private Rect                 newAtlasRect       = Rect.Empty;
		private int                  newAtlasRectIndex  = -1;
		private PixmapNumberingStyle prevNumberingStyle = PixmapNumberingStyle.None;


		public NewRectPixmapSlicerState()
		{
			this.cancelButton = new ToolStripButton(null, EditorBaseResCache.IconCancel,
				(s, e) => this.EndState());

			this.cancelButton.ToolTipText = EditorBaseRes.ToolTip_PixmapSlicerCancel;

			this.StateControls.Add(this.cancelButton);
		}

		public override void OnStateEntered(EventArgs e)
		{
			base.OnStateEntered(e);
			this.prevNumberingStyle = this.View.NumberingStyle;
			this.View.NumberingStyle = PixmapNumberingStyle.None;
			this.View.AllowNumberingStyleChange = false;
			this.View.AllowUserSelection = false;
			this.View.ClearSelection();
		}
		public override void OnStateLeaving(EventArgs e)
		{
			base.OnStateLeaving(e);
			this.View.AllowNumberingStyleChange = true;
			this.View.NumberingStyle = this.prevNumberingStyle;
			this.View.AllowUserSelection = true;
		}
		public override void OnMouseDown(MouseEventArgs e)
		{
			// Start a new rect create operation
			if (this.newAtlasRectIndex == -1)
			{
				this.newAtlasRectIndex = this.View.AtlasCount;
				this.rectAddStart = new Point(e.X, e.Y);
				this.newAtlasRect = this.View.GetAtlasRect(new Rectangle(e.X, e.Y, 0, 0));
			}
		}
		public override void OnMouseUp(MouseEventArgs e)
		{
			if (this.newAtlasRectIndex != -1)
			{
				this.newAtlasRectIndex = -1;
				UndoRedoManager.Finish();
				this.EndState();
			}
		}
		public override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);

			if (this.newAtlasRectIndex == -1)
				return;

			int rx = MathF.Min(e.X, this.rectAddStart.X);
			int ry = MathF.Min(e.Y, this.rectAddStart.Y);
			int rw = MathF.Abs(e.X - this.rectAddStart.X);
			int rh = MathF.Abs(e.Y - this.rectAddStart.Y);

			// Clip the rect to the image
			Rectangle displayRect = new Rectangle(rx, ry, rw, rh);

			// Keep the displayRect within bounds of the image
			displayRect.Intersect(this.View.DisplayedImageRect);

			Rect atlasRect = this.View.GetAtlasRect(displayRect);
			atlasRect = new Rect(
				MathF.RoundToInt(atlasRect.LeftX),
				MathF.RoundToInt(atlasRect.TopY),
				MathF.RoundToInt(atlasRect.RightX) - MathF.RoundToInt(atlasRect.LeftX),
				MathF.RoundToInt(atlasRect.BottomY) - MathF.RoundToInt(atlasRect.TopY));
			if (atlasRect.W != 0 && atlasRect.H != 0)
			{
				Rect updatedArea = this.newAtlasRect.ExpandedToContain(atlasRect);
				this.newAtlasRect = atlasRect;
				this.InvalidatePixmap(updatedArea);
			}

			// Apply the new atlas rect
			this.SetAtlasRect(this.newAtlasRectIndex, this.newAtlasRect);

			// Update selection of the view to match our new rect
			this.View.SelectedAtlasIndex = this.newAtlasRectIndex;
		}
		public override void OnKeyUp(KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape)
				this.EndState();
		}

		public override HelpInfo ProvideHoverHelp(Point localPos, ref bool captured)
		{
			return HelpInfo.FromText(EditorBaseRes.Help_PixmapSlicerNewRect_Topic,
				EditorBaseRes.Help_PixmapSlicerNewRect_Desc);
		}
	}
}
