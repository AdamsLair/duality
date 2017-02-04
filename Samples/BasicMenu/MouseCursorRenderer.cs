using System;
using System.Collections.Generic;
using System.Linq;

using Duality;
using Duality.Resources;
using Duality.Drawing;
using Duality.Input;

namespace BasicMenu
{
	public class MouseCursorRenderer : Component, ICmpRenderer
	{
		float ICmpRenderer.BoundRadius
		{
			get { return float.MaxValue; }
		}

		bool ICmpRenderer.IsVisible(IDrawDevice device)
		{
			return 
				(device.VisibilityMask & VisibilityFlag.ScreenOverlay) != VisibilityFlag.None &&
				(device.VisibilityMask & VisibilityFlag.AllGroups) != VisibilityFlag.None;
		}
		void ICmpRenderer.Draw(IDrawDevice device)
		{
			Canvas canvas = new Canvas(device);

			// Draw the mouse cursor when available
			if (DualityApp.Mouse.IsAvailable)
			{
				canvas.State.ColorTint = ColorRgba.White;
				canvas.FillCircle(
					DualityApp.Mouse.Pos.X, 
					DualityApp.Mouse.Pos.Y, 
					2);
			}
		}
	}
}
