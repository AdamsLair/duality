using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Duality.Editor.Controls.ToolStrip
{
	public class DualitorToolStripProfessionalRenderer : ToolStripProfessionalRenderer
	{
		public DualitorToolStripProfessionalRenderer()
		{
			this.RoundedEdges = false;
		}

		protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
		{
			if (e.ToolStrip.IsDropDown)
				base.OnRenderToolStripBorder(e);
		}
		protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
		{
			if (e.ToolStrip.IsDropDown)
				base.OnRenderSeparator(e);
			else if (e.Vertical)
			{
				int size = e.ToolStrip.Height * 3 / 4;
				int midX = e.Item.Width / 2;
				int midY = e.ToolStrip.Height / 2;
				e.Graphics.DrawLine(new Pen(Color.FromArgb(64, Color.Black)), midX, midY - size / 2, midX, midY + size / 2);
				e.Graphics.DrawLine(new Pen(Color.FromArgb(64, Color.White)), midX + 1, midY - size / 2, midX + 1, midY + size / 2);
			}
			else
			{
				int size = e.ToolStrip.Width * 3 / 4;
				int midX = e.Item.Width / 2;
				int midY = e.ToolStrip.Width / 2;
				e.Graphics.DrawLine(new Pen(Color.FromArgb(64, Color.Black)), midX - size / 2, midY, midX + size / 2, midY);
				e.Graphics.DrawLine(new Pen(Color.FromArgb(64, Color.White)), midX - size / 2, midY + 1, midX + size / 2, midY + 1);
			}
		}
		protected override void OnRenderOverflowButtonBackground(ToolStripItemRenderEventArgs e)
		{
			Brush triangleBrush = new SolidBrush(Color.Black);
			Point2 itemCenter = new Point2(e.Item.Width / 2, e.Item.Height / 2);
			int triangleSize = 2;

			if (e.Item.Pressed)
			{
				e.Graphics.FillRectangle(
					new SolidBrush(this.ColorTable.ButtonPressedHighlight), 
					1, 1, e.Item.Width - 2, e.Item.Height - 2);
				e.Graphics.DrawRectangle(
					new Pen(this.ColorTable.ButtonPressedHighlightBorder), 
					1, 1, e.Item.Width - 3, e.Item.Height - 3);
			}
			else if (e.Item.Selected)
			{
				e.Graphics.FillRectangle(
					new SolidBrush(this.ColorTable.ButtonSelectedHighlight), 
					1, 1, e.Item.Width - 2, e.Item.Height - 2);
				e.Graphics.DrawRectangle(
					new Pen(this.ColorTable.ButtonSelectedHighlightBorder), 
					1, 1, e.Item.Width - 3, e.Item.Height - 3);
			}

			e.Graphics.FillPolygon(triangleBrush, new Point[] {
				new Point(itemCenter.X - triangleSize, (e.Item.Height * 3 / 4) - triangleSize),
				new Point(itemCenter.X + triangleSize + 1, (e.Item.Height * 3 / 4) - triangleSize),
				new Point(itemCenter.X, (e.Item.Height * 3 / 4) + (triangleSize / 2))});
		}
		protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
		{
			base.OnRenderToolStripBackground(e);
			if (e.ToolStrip.IsDropDown && e.ToolStrip is ToolStripOverflow)
			{
				e.Graphics.FillRectangle(new SolidBrush(e.BackColor), e.AffectedBounds);
			}
		}
	}
}
