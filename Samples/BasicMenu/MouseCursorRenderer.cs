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
		void ICmpRenderer.GetCullingInfo(out CullingInfo info)
		{
			info.Position = Vector3.Zero;
			info.Radius = float.MaxValue;
			info.Visibility = VisibilityFlag.AllGroups | VisibilityFlag.ScreenOverlay;
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
