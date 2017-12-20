using System;
using System.Collections.Generic;
using System.Linq;

using Duality;
using Duality.Drawing;
using Duality.Tests.Properties;

using NUnit.Framework;

namespace Duality.Tests.Drawing
{
	public class DrawDeviceTest
	{
		[Test] public void IsSphereInViewScreenSpace()
		{
			Vector2 viewportSize = new Vector2(800, 600);
			using (DrawDevice device = new DrawDevice())
			{
				device.TargetSize = viewportSize;
				device.ViewportRect = new Rect(viewportSize);
				device.RenderMode = RenderMode.Screen;

				// Screen space mode is supposed to ignore projection settings
				device.ViewerAngle = MathF.DegToRad(90.0f);
				device.ViewerPos = new Vector3(7000, 8000, -500);
				device.FocusDist = 500;
				device.NearZ = 100;
				device.FarZ = 10000;
				device.Projection = ProjectionMode.Perspective;

				// Viewport center
				Assert.IsTrue(device.IsSphereInView(new Vector3(viewportSize.X * 0.5f, viewportSize.Y * 0.5f, 0), 150));

				// Just inside each of the viewports sides
				Assert.IsTrue(device.IsSphereInView(new Vector3(-100, 0, 0), 150));
				Assert.IsTrue(device.IsSphereInView(new Vector3(0, -100, 0), 150));
				Assert.IsTrue(device.IsSphereInView(new Vector3(viewportSize.X + 100, 0, 0), 150));
				Assert.IsTrue(device.IsSphereInView(new Vector3(0, viewportSize.Y + 100, 0), 150));
				Assert.IsTrue(device.IsSphereInView(new Vector3(0, 0, 10000), 150));
				Assert.IsTrue(device.IsSphereInView(new Vector3(0, 0, 50), 150));

				// Just outside each of the viewports sides
				Assert.IsFalse(device.IsSphereInView(new Vector3(-200, 0, 0), 150));
				Assert.IsFalse(device.IsSphereInView(new Vector3(0, -200, 0), 150));
				Assert.IsFalse(device.IsSphereInView(new Vector3(viewportSize.X + 200, 0, 0), 150));
				Assert.IsFalse(device.IsSphereInView(new Vector3(0, viewportSize.Y + 200, 0), 150));
				Assert.IsFalse(device.IsSphereInView(new Vector3(0, 0, 1000000000), 150));
				Assert.IsFalse(device.IsSphereInView(new Vector3(0, 0, -50), 150));
			}
		}
		[Test] public void IsSphereInViewOrthographic()
		{
			Vector2 viewportSize = new Vector2(800, 600);
			using (DrawDevice device = new DrawDevice())
			{
				device.TargetSize = viewportSize;
				device.ViewportRect = new Rect(viewportSize);
				device.ViewerPos = new Vector3(0, 0, 0);
				device.FocusDist = 500;
				device.NearZ = 100;
				device.FarZ = 10000;
				device.Projection = ProjectionMode.Orthographic;
				device.RenderMode = RenderMode.World;

				// Viewport center
				Assert.IsTrue(device.IsSphereInView(new Vector3(0, 0, device.FocusDist), 150));

				// Just inside each of the viewports sides
				Assert.IsTrue(device.IsSphereInView(new Vector3(-viewportSize.X * 0.5f - 100, 0, device.FocusDist), 150));
				Assert.IsTrue(device.IsSphereInView(new Vector3(0, -viewportSize.Y * 0.5f - 100, device.FocusDist), 150));
				Assert.IsTrue(device.IsSphereInView(new Vector3(viewportSize.X * 0.5f + 100, 0, device.FocusDist), 150));
				Assert.IsTrue(device.IsSphereInView(new Vector3(0, viewportSize.Y * 0.5f + 100, device.FocusDist), 150));
				Assert.IsTrue(device.IsSphereInView(new Vector3(0, 0, device.FarZ - 50), 150));
				Assert.IsTrue(device.IsSphereInView(new Vector3(0, 0, device.NearZ + 50), 150));
				
				// Just outside each of the viewports sides
				Assert.IsFalse(device.IsSphereInView(new Vector3(-viewportSize.X * 0.5f - 200, 0, device.FocusDist), 150));
				Assert.IsFalse(device.IsSphereInView(new Vector3(0, -viewportSize.Y * 0.5f - 200, device.FocusDist), 150));
				Assert.IsFalse(device.IsSphereInView(new Vector3(viewportSize.X * 0.5f + 200, 0, device.FocusDist), 150));
				Assert.IsFalse(device.IsSphereInView(new Vector3(0, viewportSize.Y * 0.5f + 200, device.FocusDist), 150));
				Assert.IsFalse(device.IsSphereInView(new Vector3(0, 0, device.FarZ + 50), 150));
				Assert.IsFalse(device.IsSphereInView(new Vector3(0, 0, device.NearZ - 50), 150));
			}
		}
		[Test] public void IsSphereInViewPerspective()
		{
			Vector2 viewportSize = new Vector2(800, 600);
			using (DrawDevice device = new DrawDevice())
			{
				device.TargetSize = viewportSize;
				device.ViewportRect = new Rect(viewportSize);
				device.ViewerPos = new Vector3(0, 0, 0);
				device.FocusDist = 500;
				device.NearZ = 100;
				device.FarZ = 10000;
				device.Projection = ProjectionMode.Perspective;
				device.RenderMode = RenderMode.World;

				// Viewport center
				Assert.IsTrue(device.IsSphereInView(new Vector3(0, 0, device.FocusDist), 150));

				// Just inside each of the viewports sides
				Assert.IsTrue(device.IsSphereInView(new Vector3(-viewportSize.X * 0.5f - 100, 0, device.FocusDist), 150));
				Assert.IsTrue(device.IsSphereInView(new Vector3(0, -viewportSize.Y * 0.5f - 100, device.FocusDist), 150));
				Assert.IsTrue(device.IsSphereInView(new Vector3(viewportSize.X * 0.5f + 100, 0, device.FocusDist), 150));
				Assert.IsTrue(device.IsSphereInView(new Vector3(0, viewportSize.Y * 0.5f + 100, device.FocusDist), 150));
				Assert.IsTrue(device.IsSphereInView(new Vector3(0, 0, device.FarZ - 50), 150));
				Assert.IsTrue(device.IsSphereInView(new Vector3(0, 0, device.NearZ + 50), 150));
				
				// Just outside each of the viewports sides
				Assert.IsFalse(device.IsSphereInView(new Vector3(-viewportSize.X * 0.5f - 200, 0, device.FocusDist), 150));
				Assert.IsFalse(device.IsSphereInView(new Vector3(0, -viewportSize.Y * 0.5f - 200, device.FocusDist), 150));
				Assert.IsFalse(device.IsSphereInView(new Vector3(viewportSize.X * 0.5f + 200, 0, device.FocusDist), 150));
				Assert.IsFalse(device.IsSphereInView(new Vector3(0, viewportSize.Y * 0.5f + 200, device.FocusDist), 150));
				Assert.IsFalse(device.IsSphereInView(new Vector3(0, 0, device.FarZ + 50), 150));
				Assert.IsFalse(device.IsSphereInView(new Vector3(0, 0, device.NearZ - 50), 150));

				// Things that are in/visible because of perspective projection
				Assert.IsFalse(device.IsSphereInView(new Vector3(-viewportSize.X * 0.5f - 100, 0, device.FocusDist * 0.5f), 150));
				Assert.IsTrue(device.IsSphereInView(new Vector3(-viewportSize.X * 0.5f - 200, 0, device.FocusDist * 2.0f), 150));
			}
		}
	}
}
