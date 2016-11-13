using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using AdamsLair.WinForms;
using AdamsLair.WinForms.PropertyEditing;

using Duality.Plugins.Tilemaps;
using Duality.Editor.Plugins.Tilemaps.Properties;

namespace Duality.Editor.Plugins.Tilemaps
{
	/// <summary>
	/// A user dialog for resizing a set of <see cref="Tilemap"/> instances
	/// relative to a specified origin.
	/// </summary>
	public partial class TilemapResizeDialog : Form
	{
		private Tilemap[] tilemaps = null;
		private Point2 currentSize = Point2.Zero;

		/// <summary>
		/// [GET / SET] The tilemaps that are to be adjusted by the dialog.
		/// </summary>
		public IEnumerable<Tilemap> Tilemaps
		{
			get { return this.tilemaps; }
			set
			{
				this.tilemaps = value.NotNull().ToArray();
				this.UpdateContent();
			}
		}


		public TilemapResizeDialog()
		{
			this.InitializeComponent();
			this.UpdateContent();
		}


		private void UpdateContent()
		{
			if (this.tilemaps == null) return;
			if (this.tilemaps.Length == 0) return;

			this.currentSize = new Point2(
				this.tilemaps.Min(t => t.Size.X),
				this.tilemaps.Min(t => t.Size.Y));

			Color multiSizeColor = Color.FromArgb(242, 212, 170);
			bool multiSize = this.tilemaps.Any(t => t.Size != this.currentSize);

			this.editorWidth.Value = this.currentSize.X;
			this.editorHeight.Value = this.currentSize.Y;
			this.labelMultiselect.Visible = multiSize;
			this.editorWidth.BackColor = multiSize ? multiSizeColor : SystemColors.Window;
			this.editorHeight.BackColor = multiSize ? multiSizeColor : SystemColors.Window;
			this.Text = (this.tilemaps.Length == 1) ?
				string.Format(TilemapsRes.TilemapSetupHeader_ResizeSingleTilemap, this.tilemaps[0].GameObj != null ? this.tilemaps[0].GameObj.Name : typeof(Tilemap).Name) :
				string.Format(TilemapsRes.TilemapSetupHeader_ResizeMultipleTilemaps, this.tilemaps.Length);
		}
		
		private void editorWidth_ValueChanged(object sender, EventArgs e)
		{
			this.originSelector.InvertArrowsHorizontal = this.editorWidth.Value < this.currentSize.X;
		}
		private void editorHeight_ValueChanged(object sender, EventArgs e)
		{
			this.originSelector.InvertArrowsVertical = this.editorHeight.Value < this.currentSize.Y;
		}
		private void buttonOk_Click(object sender, EventArgs e)
		{
			if (this.tilemaps != null && this.tilemaps.Length > 0)
			{
				Point2 newSize = new Point2(
					(int)this.editorWidth.Value,
					(int)this.editorHeight.Value);
				Alignment origin = OriginSelectorToAlignment(this.originSelector.SelectedOrigin);
				UndoRedoManager.Do(new UndoRedoActions.ResizeTilemapAction(
					this.tilemaps, 
					newSize, 
					origin));
			}
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private static Alignment OriginSelectorToAlignment(OriginSelector.Origin origin)
		{
			switch (origin)
			{
				default:
				case OriginSelector.Origin.Center:      return Alignment.Center;
				case OriginSelector.Origin.Left:        return Alignment.Left;
				case OriginSelector.Origin.Right:       return Alignment.Right;
				case OriginSelector.Origin.TopLeft:     return Alignment.TopLeft;
				case OriginSelector.Origin.Top:         return Alignment.Top;
				case OriginSelector.Origin.TopRight:    return Alignment.TopRight;
				case OriginSelector.Origin.BottomLeft:  return Alignment.BottomLeft;
				case OriginSelector.Origin.Bottom:      return Alignment.Bottom;
				case OriginSelector.Origin.BottomRight: return Alignment.BottomRight;
			}
		}
	}
}
