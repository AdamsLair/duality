using System;
using System.Collections.Generic;
using System.Linq;

using Duality;
using Duality.Components;
using Duality.Drawing;

namespace CustomRenderingSetup
{
	public class ScreenSpaceRenderer : Component, ICmpRenderer
	{
		void ICmpRenderer.GetCullingInfo(out CullingInfo info)
		{
			info.Position = Vector3.Zero;
			info.Radius = float.MaxValue;
			info.Visibility = VisibilityFlag.AllGroups | VisibilityFlag.ScreenOverlay;
		}
		void ICmpRenderer.Draw(IDrawDevice device)
		{
			Canvas canvas = new Canvas();
			canvas.Begin(device);

			// Draw screen space edge indicators
			canvas.DrawLine(0, 10, device.TargetSize.X, 10);
			canvas.DrawLine(0, device.TargetSize.Y - 10, device.TargetSize.X, device.TargetSize.Y - 10);
			canvas.DrawLine(10, 0, 10, device.TargetSize.Y);
			canvas.DrawLine(device.TargetSize.X - 10, 0, device.TargetSize.X - 10, device.TargetSize.Y);

			// Draw some sample text
			canvas.DrawText("Top Left", 10, 10, blockAlign: Alignment.TopLeft);
			canvas.DrawText("Top Right", device.TargetSize.X - 10, 10, blockAlign: Alignment.TopRight);
			canvas.DrawText("Bottom Left", 10, device.TargetSize.Y - 10, blockAlign: Alignment.BottomLeft);
			canvas.DrawText("Bottom Right", device.TargetSize.X - 10, device.TargetSize.Y - 10, blockAlign: Alignment.BottomRight);

			canvas.End();
		}
	}
}