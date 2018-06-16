using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using Duality.Resources;

using Duality.Editor.Controls.ToolStrip;
using Duality.Editor.Plugins.Base.UndoRedoActions;


namespace Duality.Editor.Plugins.Base.Forms.PixmapSlicer.States
{
	public abstract class PixmapSlicerState : IPixmapSlicerState
	{
		public event EventHandler StateEndRequested;
		public event EventHandler<PixmapSlicerStateEventArgs> StateChangeRequested;


		public List<ToolStripItem> StateControls { get; private set; }
		public Pixmap TargetPixmap
		{
			get { return this.View.TargetPixmap; }
		}
		public PixmapSlicingView View { get; set; }


		protected PixmapSlicerState()
		{
			this.StateControls = new List<ToolStripItem>();
		}

		protected void InvalidatePixmap()
		{
			this.View.InvalidatePixmap();
		}
		protected void InvalidatePixmap(Rect atlasRect)
		{
			this.View.InvalidatePixmap(atlasRect);
		}

		protected void SetAtlasRect(Rect atlasRect, int index)
		{
			if (this.TargetPixmap == null) return;

			Rect oldRect = this.View.GetAtlasRect(index);
			bool hasOldRect = index >= 0 && index < this.View.AtlasCount;

			UndoRedoManager.Do(new SetAtlasRectAction(this.TargetPixmap, index, atlasRect));

			Rect updatedArea = atlasRect;
			if (hasOldRect)
			{
				updatedArea = updatedArea.ExpandedToContain(oldRect);
			}
			this.InvalidatePixmap(updatedArea);
		}

		protected void EndState()
		{
			if (this.StateEndRequested != null)
				this.StateEndRequested(this, EventArgs.Empty);
		}
		protected void SwitchToState(Type newStateType)
		{
			if (this.StateChangeRequested != null)
				this.StateChangeRequested(this, new PixmapSlicerStateEventArgs(newStateType));
		}

		public virtual void OnStateEntered(EventArgs e)
		{
			this.View.TargetPixmapChanged += this.View_TargetPixmapChanged;
			this.OnTargetPixmapChanged();
		}
		public virtual void OnStateLeaving(EventArgs e)
		{
			this.View.TargetPixmapChanged -= this.View_TargetPixmapChanged;
		}
		public virtual void OnMouseDown(MouseEventArgs e) { }
		public virtual void OnMouseUp(MouseEventArgs e) { }
		public virtual void OnMouseMove(MouseEventArgs e) { }
		public virtual void OnKeyUp(KeyEventArgs e) { }
		/// <summary>
		/// Called during the paint event of the parent control.
		/// Displays all atlas rects and highlights the selected atlas rect.
		/// </summary>
		public virtual void OnPaint(PaintEventArgs e) { }
		protected virtual void OnTargetPixmapChanged() { }

		public abstract HelpInfo ProvideHoverHelp(Point localPos, ref bool captured);

		private void View_TargetPixmapChanged(object sender, EventArgs e)
		{
			this.OnTargetPixmapChanged();
		}

		/// <summary>
		/// Creates a <see cref="ToolStripNumericUpDown"/> with default styling.
		/// The range of the control is set to [0,maximum].
		/// </summary>
		protected static ToolStripNumericUpDown CreateNumericUpDown(string text, int minimum, int maximum)
		{
			return new ToolStripNumericUpDown
			{
				BackColor = Color.Transparent,
				DecimalPlaces = 0,
				Minimum = minimum,
				Maximum = maximum,
				NumBackColor = Color.FromArgb(196, 196, 196),
				Text = text
			};
		}
	}
}
