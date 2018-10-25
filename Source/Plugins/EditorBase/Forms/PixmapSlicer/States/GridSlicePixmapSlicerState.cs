using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using Duality.Resources;

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
		private ToolStripButton        doneButton   = null;
		private ToolStripButton        cancelButton = null;
		private ToolStripNumericUpDown rowsInput    = null;
		private ToolStripNumericUpDown colsInput    = null;
		private ToolStripNumericUpDown borderInput  = null;

		private RectAtlas originalAtlas = null;


		public GridSlicePixmapSlicerState()
		{
			this.doneButton = new ToolStripButton(null, EditorBaseResCache.IconAcceptCheck,
				(s, e) => this.FinishSlicing());
			this.cancelButton = new ToolStripButton(null, EditorBaseResCache.IconCancel,
				(s, e) => this.CancelSlicing());

			this.doneButton.ToolTipText = EditorBaseRes.ToolTip_PixmapSlicerDone;
			this.cancelButton.ToolTipText = EditorBaseRes.ToolTip_PixmapSlicerCancel;

			this.rowsInput = CreateNumericUpDown("Rows:", 1, 100);
			this.colsInput = CreateNumericUpDown("Cols:", 1, 100);
			this.borderInput = CreateNumericUpDown("Border:", 0, 50);

			this.rowsInput.ValueChanged += this.OnRowsChanged;
			this.colsInput.ValueChanged += this.OnColsChanged;
			this.borderInput.ValueChanged += this.OnBorderChanged;

			this.StateControls.Add(this.doneButton);
			this.StateControls.Add(this.cancelButton);
			this.StateControls.Add(this.rowsInput);
			this.StateControls.Add(this.colsInput);
			this.StateControls.Add(this.borderInput);
		}

		private void GridSlicePixmap()
		{
			if (this.TargetPixmap == null) return;

			// We don't use the outdated AnimCols / AnimRos properties here, as they'll be 
			// gone in v3.0 anyway and are the legacy way to define an animated Pixmap.
			RectAtlas generatedAtlas = PixmapSlicingUtility.SliceGrid(
				this.TargetPixmap, 
				(int)this.colsInput.Value, 
				(int)this.rowsInput.Value, 
				(int)this.borderInput.Value);
			this.SetAtlas(generatedAtlas);
		}

		private void FinishSlicing()
		{
			UndoRedoManager.Finish();
			this.EndState();
		}
		private void CancelSlicing()
		{
			this.SetAtlas(this.originalAtlas);
			this.EndState();
		}

		protected override void OnTargetPixmapChanged()
		{
			base.OnTargetPixmapChanged();

			if (this.TargetPixmap.Atlas != null)
			{
				this.originalAtlas = new RectAtlas(this.TargetPixmap.Atlas);
				this.rowsInput.Value = 3;
				this.colsInput.Value = 3;
				this.borderInput.Value = 0;
				this.GridSlicePixmap();
			}

			this.InvalidatePixmap();
		}
		private void OnRowsChanged(object sender, EventArgs e)
		{
			this.GridSlicePixmap();
			this.InvalidatePixmap();
		}
		private void OnColsChanged(object sender, EventArgs e)
		{
			this.GridSlicePixmap();
			this.InvalidatePixmap();
		}
		private void OnBorderChanged(object sender, EventArgs e)
		{
			this.GridSlicePixmap();
			this.InvalidatePixmap();
		}

		public override HelpInfo ProvideHoverHelp(Point localPos, ref bool captured)
		{
			return HelpInfo.FromText(EditorBaseRes.Help_PixmapSlicerGridSlice_Topic,
				EditorBaseRes.Help_PixmapSlicerGridSlice_Desc);
		}
	}
}
