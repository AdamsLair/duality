using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Duality.Editor.Plugins.Base.Properties;
using Duality.Editor.Plugins.Base.UndoRedoActions;

namespace Duality.Editor.Plugins.Base.Forms.PixmapSlicer.States
{
	/// <summary>
	/// A <see cref="IPixmapSlicerState"/> that allows the user to specify the ordering
	/// of atlas rectangles by clicking on each rectangle in the desired order
	/// </summary>
	public class AtlasOrderingPixmapSlicerState : PixmapSlicerState
	{
		private ToolStripButton      doneButton         = null;
		private ToolStripButton      cancelButton       = null;
		private List<int>            orderedIndices     = new List<int>();
		private PixmapNumberingStyle prevNumberingStyle = PixmapNumberingStyle.None;


		public AtlasOrderingPixmapSlicerState()
		{
			this.doneButton = new ToolStripButton(null, EditorBaseResCache.IconAcceptCheck,
				(s, e) => this.FinishOrdering());
			this.cancelButton = new ToolStripButton(null, EditorBaseResCache.IconCancel,
				(s, e) => this.EndState());

			this.doneButton.ToolTipText = EditorBaseRes.ToolTip_PixmapSlicerDone;
			this.cancelButton.ToolTipText = EditorBaseRes.ToolTip_PixmapSlicerCancel;

			this.StateControls.Add(this.doneButton);
			this.StateControls.Add(this.cancelButton);
		}

		private void FinishOrdering()
		{
			List<Rect> newAtlas = new List<Rect>();

			// Add indices that were order to the atlas first
			foreach (int index in this.orderedIndices)
			{
				newAtlas.Add(this.View.GetAtlasRect(index));
			}

			// Now add all non-ordered indices
			// Use hash-set here for efficiency
			HashSet<int> finishedIndices = new HashSet<int>(this.orderedIndices);
			for (int i = 0; i < this.View.AtlasCount; i++)
			{
				if (!finishedIndices.Contains(i))
				{
					newAtlas.Add(this.View.GetAtlasRect(i));
				}
			}

			this.SetAtlas(newAtlas);
			UndoRedoManager.Finish();

			this.EndState();
		}

		public override void OnStateEntered(EventArgs e)
		{
			base.OnStateEntered(e);
			this.prevNumberingStyle = this.View.NumberingStyle;
			this.View.NumberingStyle = PixmapNumberingStyle.Hovered;
		}
		public override void OnStateLeaving(EventArgs e)
		{
			base.OnStateLeaving(e);
			this.View.NumberingStyle = this.prevNumberingStyle;
		}
		public override void OnMouseUp(MouseEventArgs e)
		{
			int indexClicked = this.View.HoveredAtlasIndex;
			if (indexClicked == -1 || this.orderedIndices.Contains(indexClicked))
				return;

			this.orderedIndices.Add(indexClicked);

			this.InvalidatePixmap(this.View.GetAtlasRect(indexClicked));
		}
		public override void OnKeyUp(KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape)
				this.EndState();
		}
		public override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			// Render the index of any clicked rects
			for (int index = 0; index < this.orderedIndices.Count; index++)
			{
				Rect atlasRect = this.View.GetAtlasRect(this.orderedIndices[index]);
				Rectangle displayRect = this.View.GetDisplayRect(atlasRect);
				this.View.DrawRectIndex(e.Graphics, displayRect, index);
			}
		}

		public override HelpInfo ProvideHoverHelp(Point localPos, ref bool captured)
		{
			return HelpInfo.FromText(EditorBaseRes.Help_PixmapSlicerRectOrdering_Topic,
				EditorBaseRes.Help_PixmapSlicerRectOrdering_Desc);
		}
	}
}
