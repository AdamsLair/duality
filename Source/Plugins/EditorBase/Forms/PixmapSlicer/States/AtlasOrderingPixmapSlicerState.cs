using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Font = System.Drawing.Font;

namespace Duality.Editor.Plugins.Base.Forms.PixmapSlicer.States
{
	/// <summary>
	/// A <see cref="IPixmapSlicerState"/> that allows the user to specify the ordering
	/// of atlas rectangles by clicking on each rectangle in the desired order
	/// </summary>
	public class AtlasOrderingPixmapSlicerState : PixmapSlicerState
	{
		private ToolStripButton doneButton		= null;
		private ToolStripButton cancelButton	= null;
		private List<int> orderedIndices		= new List<int>();

		public AtlasOrderingPixmapSlicerState()
		{
			this.SelectedRectIndex = -1;

			this.doneButton = new ToolStripButton("Done", null,
				(s, e) => this.FinishOrdering());
			this.cancelButton = new ToolStripButton("Cancel", null,
				(s, e) => this.CancelState());

			this.StateControls.Add(this.doneButton);
			this.StateControls.Add(this.cancelButton);
		}

		public override void OnMouseUp(MouseEventArgs e)
		{
			if (this.TargetPixmap == null || this.TargetPixmap.Atlas == null)
				return;

			float x, y;
			this.TransformMouseCoordinates(e.Location, out x, out y);
			int indexClicked = this.TargetPixmap.Atlas
				.IndexOfFirst(r => this.GetDisplayRect(r).Contains(x, y));

			if (indexClicked == -1 || this.orderedIndices.Contains(indexClicked))
				return;

			this.orderedIndices.Add(indexClicked);

			// The PixmapUpdated event will cause the display to invalidate
			// TODO: remove this for a generic invalidate event
			this.UpdatePixmap();
		}

		public override void OnKeyUp(KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape)
				this.CancelState();
		}

		public override void OnPaint(PaintEventArgs e)
		{
			// Render the index of any clicked rects
			for (int index = 0; index < this.orderedIndices.Count; index++)
			{
				Rect atlasRect = this.TargetPixmap.Atlas[this.orderedIndices[index]];
				Rect displayRect = this.GetDisplayRect(atlasRect);
				RectangleF displayRectangle = new RectangleF(displayRect.X, displayRect.Y, displayRect.W, displayRect.H);
				string renderedString = index.ToString();

				using (Font startingFont = new Font(FontFamily.GenericSerif, 20))
				using (Font font = GetAdjustedFont(e.Graphics, renderedString,
					startingFont, (int)displayRect.W, (int)displayRect.H, 20, 2, false))
				{
					StringFormat format = new StringFormat();
					format.Alignment = StringAlignment.Center;
					e.Graphics.DrawString(renderedString, font, Brushes.White, displayRectangle, format);
				}
			}
		}

		private void FinishOrdering()
		{
			List<Rect> newAtlas = new List<Rect>();

			// Add indices that were order to the atlas first
			foreach (int index in this.orderedIndices)
			{
				newAtlas.Add(this.TargetPixmap.Atlas[index]);
			}

			// Now add all non-ordered indices
			// Use hash-set here for efficiency
			HashSet<int> finishedIndices = new HashSet<int>(this.orderedIndices);
			for (int i = 0; i < this.TargetPixmap.Atlas.Count; i++)
			{
				if (!finishedIndices.Contains(i))
				{
					newAtlas.Add(this.TargetPixmap.Atlas[i]);
				}
			}

			this.TargetPixmap.Atlas = newAtlas;
			this.UpdatePixmap();
			this.CancelState();
		}

		/// <summary>
		/// Returns a font that best fits the given parameters
		/// </summary>
		private static Font GetAdjustedFont(Graphics graphicRef, string graphicString,
			Font originalFont, int containerWidth, int containerHeight,
			int maxFontSize, int minFontSize,
			bool smallestOnFail)
		{
			Font testFont = null;
			// We utilize MeasureString which we get via a control instance           
			for (int adjustedSize = maxFontSize; adjustedSize >= minFontSize; adjustedSize--)
			{
				testFont = new Font(originalFont.Name, adjustedSize, originalFont.Style);

				// Test the string with the new size
				SizeF adjustedSizeNew = graphicRef.MeasureString(graphicString, testFont);

				if (containerWidth > Convert.ToInt32(adjustedSizeNew.Width)
					&& containerHeight > Convert.ToInt32(adjustedSizeNew.Height))
				{
					// Good font, return it
					return testFont;
				}
			}

			// If you get here there was no fontsize that worked
			// return minimumSize or original?
			if (smallestOnFail)
			{
				return testFont;
			}
			else
			{
				return originalFont;
			}
		}
	}
}
