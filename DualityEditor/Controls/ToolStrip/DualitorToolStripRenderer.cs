using System.Windows.Forms;
using System.Drawing;

namespace Duality.Editor.Controls.ToolStrip
{
	public class DualitorToolStripProfessionalRenderer : ToolStripProfessionalRenderer
	{
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
	}
	public class DualitorToolStripSystemRenderer : ToolStripSystemRenderer
	{
		protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
		{
			if (e.ToolStrip.IsDropDown)
				base.OnRenderToolStripBorder(e);
		}
		protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
		{
			//base.OnRenderSeparator(e);
			if (e.Vertical)
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
	}
}
