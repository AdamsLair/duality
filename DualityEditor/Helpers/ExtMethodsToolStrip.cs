using System.Linq;
using System.Windows.Forms;
using System.Drawing;

namespace Duality.Editor
{
	public static class ExtMethodsToolStrip
	{
		public static ToolStripItem GetItemAtDeep(this ToolStrip t, Point screenPos)
		{
			if (!t.Visible) return null;

			Point toolStripLocalPos = t.PointToClient(screenPos);
			ToolStripItem item = t.GetItemAt(toolStripLocalPos);
			if (item != null) return item;

			ToolStripDropDownItem dropItem = t.Items.OfType<ToolStripDropDownItem>().FirstOrDefault(i => i.DropDown.Visible);
			if (dropItem != null) return dropItem.DropDown.GetItemAtDeep(screenPos);

			return null;
		}
	}
}
