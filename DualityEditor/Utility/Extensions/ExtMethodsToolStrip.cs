using System.Linq;
using System.Windows.Forms;
using System.Drawing;

namespace Duality.Editor
{
	public static class ExtMethodsToolStrip
	{
		public static ToolStripItem GetItemAtDeep(this ToolStrip toolstrip, Point screenPos)
		{
			if (!toolstrip.Visible) return null;

			Point toolStripLocalPos = toolstrip.PointToClient(screenPos);
			ToolStripItem item = toolstrip.GetItemAt(toolStripLocalPos);
			if (item != null) return item;

			ToolStripDropDownItem dropItem = toolstrip.GetActiveDropDown();
			if (dropItem != null) return dropItem.DropDown.GetItemAtDeep(screenPos);

			return null;
		}
		/// <summary>
		/// Returns the currently dropped down item of this <see cref="ToolStrip"/>.
		/// Returns null, if no dropdown is active.
		/// </summary>
		/// <param name="toolstrip"></param>
		/// <returns></returns>
		public static ToolStripDropDownItem GetActiveDropDown(this ToolStrip toolstrip)
		{
			if (!toolstrip.Visible) return null;
			return toolstrip.Items.OfType<ToolStripDropDownItem>().FirstOrDefault(item => item.DropDown.Visible);
		}
	}
}
