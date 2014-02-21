using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;

namespace Duality.Editor
{
	public static class ExtMethodsControl
	{
		public static Control GetChildAtPointDeep(this Control c, Point pt, GetChildAtPointSkip skip)
		{
			Point globalPt = c.PointToScreen(pt);
			Control child = c.GetChildAtPoint(pt, skip);
			Control deeperChild = child;
			while (deeperChild != null)
			{
				child = deeperChild;
				deeperChild = deeperChild.GetChildAtPoint(deeperChild.PointToClient(globalPt), skip);
			}
			return deeperChild ?? child;
		}
		public static T GetControlAncestor<T>(this Control c) where T : class
		{
			while (c != null)
			{
				if (c is T) return c as T;
				c = c.Parent;
			}
			return null;
		}
		public static IEnumerable<T> GetControlAncestors<T>(this Control c) where T : class
		{
			while (c != null)
			{
				if (c is T) yield return c as T;
				c = c.Parent;
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
