using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;

namespace Duality.Editor
{
	public static class ExtMethodsControl
	{
		/// <summary>
		/// Retrieves the currently active item from all (immediate, not deep) child toolstrips of this <see cref="Control"/>.
		/// An item is considered active when it is hovered by the mouse cursor while no other dropped down 
		/// is open, which would capture the mouse. All items are considered, even if nested within dropdowns.
		/// </summary>
		/// <param name="control"></param>
		/// <param name="globalPos"></param>
		/// <param name="isOnActiveDropDown"></param>
		public static ToolStripItem GetHoveredToolStripItem(this Control control, Point globalPos, out bool isOnActiveDropDown)
		{
			isOnActiveDropDown = false;

			// Check for active dropdowns and their child items first - they'll capture cursor focus
			foreach (ToolStrip toolstrip in control.Controls.OfType<ToolStrip>())
			{
				ToolStripDropDownItem dropItem = toolstrip.GetActiveDropDown();
				if (dropItem != null)
				{
					isOnActiveDropDown = true;
					return dropItem.DropDown.GetItemAtDeep(globalPos);
				}
			}

			// Check whether any root-level toolstrip item is hovered
			foreach (ToolStrip toolstrip in control.Controls.OfType<ToolStrip>())
			{
				ToolStripItem hoveredItem = null;
				Point toolStripLocalPos = toolstrip.PointToClient(globalPos);
				hoveredItem = toolstrip.GetItemAt(toolStripLocalPos) ?? hoveredItem;
				if (hoveredItem != null)
					return hoveredItem;
			}

			// Nothing found
			return null;
		}
		public static Control GetChildAtPointDeep(this Control control, Point pt, GetChildAtPointSkip skip)
		{
			Point globalPt = control.PointToScreen(pt);
			Control child = control.GetChildAtPoint(pt, skip);
			Control deeperChild = child;
			while (deeperChild != null)
			{
				child = deeperChild;
				deeperChild = deeperChild.GetChildAtPoint(deeperChild.PointToClient(globalPt), skip);
			}
			return deeperChild ?? child;
		}
		public static T GetControlAncestor<T>(this Control control) where T : class
		{
			while (control != null)
			{
				if (control is T) return control as T;
				control = control.Parent;
			}
			return null;
		}
		public static IEnumerable<T> GetControlAncestors<T>(this Control control) where T : class
		{
			while (control != null)
			{
				if (control is T) yield return control as T;
				control = control.Parent;
			}
			yield break;
		}
		
		public static U InvokeEx<T,U>(this T control, Func<T, U> func) where T : Control
		{
			return control.InvokeRequired ? (U)control.Invoke(func, control) : func(control);
		}
		public static void InvokeEx<T>(this T control, Action<T> func, bool waitForResult = true) where T : Control
		{
			if (waitForResult)
			{
				control.InvokeEx(c => { func(c); return c; });
			}
			else
			{
				// Perform an asynchronous invoke, if necessary
				if (control.InvokeRequired)
					control.BeginInvoke(func, control);
				else
					func(control);
			}
		}
		public static void InvokeEx<T>(this T control, Action action, bool waitForResult = true) where T : Control
		{
			control.InvokeEx(c => action(), waitForResult);
		}
	}
	public static class ExtMethodsDockState
	{
		public static bool IsAutoHide(this WeifenLuo.WinFormsUI.Docking.DockState state)
		{
			return state == WeifenLuo.WinFormsUI.Docking.DockState.DockBottomAutoHide ||
				state == WeifenLuo.WinFormsUI.Docking.DockState.DockLeftAutoHide ||
				state == WeifenLuo.WinFormsUI.Docking.DockState.DockRightAutoHide ||
				state == WeifenLuo.WinFormsUI.Docking.DockState.DockTopAutoHide;
		}
	}
}
