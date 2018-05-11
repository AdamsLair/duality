using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Duality.Editor.Controls.ToolStrip;
using Duality.Editor.Plugins.Base.Properties;
using Duality.Editor.Plugins.Base.UndoRedoActions;

namespace Duality.Editor.Plugins.Base.Forms.PixmapSlicer.States
{
	/// <summary>
	/// A <see cref="IPixmapSlicerState"/> that provides controls for
	/// defining atlas rects with a variable size grid place over 
	/// the <see cref="Duality.Resources.Pixmap"/>
	/// </summary>
	public class GridSlicePixmapSlicerState : PixmapSlicerState
	{
		private ToolStripButton			doneButton		= null;
		private ToolStripButton			cancelButton	= null;
		private ToolStripNumericUpDown	rowsInput		= null;
		private ToolStripNumericUpDown	colsInput		= null;
		private ToolStripNumericUpDown	borderInput		= null;

		private List<Rect> originalAtlas = null;

		public GridSlicePixmapSlicerState()
		{
			this.doneButton = new ToolStripButton(null, EditorBaseRes.IconAcceptCheck,
				(s, e) => this.FinishSlicing());
			this.cancelButton = new ToolStripButton(null, EditorBaseRes.IconCancel,
				(s, e) => this.UndoAndCancelState());

			this.doneButton.ToolTipText = EditorBaseRes.ToolTip_PixmapSlicerDone;
			this.cancelButton.ToolTipText = EditorBaseRes.ToolTip_PixmapSlicerCancel;

			this.rowsInput = CreateNumericUpDown("Rows:");
			this.colsInput = CreateNumericUpDown("Cols:");
			this.borderInput = CreateNumericUpDown("Border:", 200);

			this.rowsInput.ValueChanged += this.OnRowsChanged;
			this.colsInput.ValueChanged += this.OnColsChanged;
			this.borderInput.ValueChanged += this.OnBorderChanged;

			this.StateControls.Add(this.doneButton);
			this.StateControls.Add(this.cancelButton);
			this.StateControls.Add(this.rowsInput);
			this.StateControls.Add(this.colsInput);
			this.StateControls.Add(this.borderInput);
		}

		public override void OnKeyUp(KeyEventArgs e)
		{
			switch (e.KeyCode)
			{
				case Keys.Subtract:
					if (e.Control) this.TargetPixmap.AnimCols = Math.Max(0, this.TargetPixmap.AnimCols - 1);
					else this.TargetPixmap.AnimRows = Math.Max(0, this.TargetPixmap.AnimRows - 1);
					this.UpdateControls();
					this.UpdateDisplay();
					break;
				case Keys.Add:
					if (e.Control) this.TargetPixmap.AnimCols++;
					else this.TargetPixmap.AnimRows++;
					this.UpdateControls();
					this.UpdateDisplay();
					break;
			}
		}

		protected override void OnPixmapChanged()
		{
			base.OnPixmapChanged();

			if (this.TargetPixmap.Atlas != null)
			{
				this.rowsInput.Value = this.TargetPixmap.AnimRows;
				this.colsInput.Value = this.TargetPixmap.AnimCols;
				this.borderInput.Value = this.TargetPixmap.AnimFrameBorder;

				this.originalAtlas = this.TargetPixmap.Atlas.ToList();
				this.TargetPixmap.Atlas = null;
			}
		}

		private void FinishSlicing()
		{
			List<Rect> newAtlas = this.TargetPixmap.Atlas == null 
				? null 
				: this.TargetPixmap.Atlas.ToList();

			// Set the atlas back to the original so that undoing the following
			// undo redo action will revert to the original rects
			this.TargetPixmap.Atlas = this.originalAtlas;

			UndoRedoManager.Do(new SetAtlasAction(newAtlas, new []{ this.TargetPixmap }));
			this.CancelState();
		}

		private void OnRowsChanged(object sender, EventArgs e)
		{
			this.TargetPixmap.AnimRows = (int) this.rowsInput.Value;
			this.UpdateDisplay();
		}

		private void OnColsChanged(object sender, EventArgs e)
		{
			this.TargetPixmap.AnimCols = (int)this.colsInput.Value;
			this.UpdateDisplay();
		}

		private void OnBorderChanged(object sender, EventArgs e)
		{
			this.TargetPixmap.AnimFrameBorder = (int)this.borderInput.Value;
			this.UpdateDisplay();
		}

		private void UpdateControls()
		{
			this.rowsInput.Value = this.TargetPixmap.AnimRows;
			this.colsInput.Value = this.TargetPixmap.AnimCols;
			this.borderInput.Value = this.TargetPixmap.AnimFrameBorder;
		}

		private void UndoAndCancelState()
		{
			this.TargetPixmap.Atlas = this.originalAtlas;

			this.CancelState();
		}

		public override HelpInfo ProvideHoverHelp(Point localPos, ref bool captured)
		{
			return HelpInfo.FromText(EditorBaseRes.Help_PixmapSlicerGridSlice_Topic,
				EditorBaseRes.Help_PixmapSlicerGridSlice_Desc);
		}
	}
}
