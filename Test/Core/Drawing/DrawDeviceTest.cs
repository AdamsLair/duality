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
				device.Projection = ProjectionMode.Screen;

				// Screen space mode is supposed to ignore view dependent settings
				device.ViewerOrientation = new Vector3(0, 0, MathF.DegToRad(90.0f));
				device.ViewerPos = new Vector3(7000, 8000, -500);
				device.FocusDist = 500;
				device.NearZ = 100;
				device.FarZ = 10000;

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

		[Test] public void GetScaleAtZ()
		{
			using (DrawDevice device = new DrawDevice())
			{
				// We'll check twice the focus distance to make sure orthographic
				// scaling is working as expected.
				device.FocusDist = DrawDevice.DefaultFocusDist * 2.0f;
				device.NearZ = 100;
				device.FarZ = 10000;
				device.ViewerPos = new Vector3(0, 0, -device.FocusDist);

				// Screen space rendering
				device.Projection = ProjectionMode.Screen;

				Assert.AreEqual(1.0f, device.GetScaleAtZ(0.0f));
				Assert.AreEqual(1.0f, device.GetScaleAtZ(1000.0f));
				Assert.AreEqual(1.0f, device.GetScaleAtZ(-1000.0f));
				Assert.AreEqual(1.0f, device.GetScaleAtZ(10000.0f));
				Assert.AreEqual(1.0f, device.GetScaleAtZ(-10000.0f));

				// World space rendering with orthographic projection
				device.Projection = ProjectionMode.Orthographic;

				Assert.AreEqual(2.0f, device.GetScaleAtZ(0.0f));
				Assert.AreEqual(2.0f, device.GetScaleAtZ(1000.0f));
				Assert.AreEqual(2.0f, device.GetScaleAtZ(-1000.0f));
				Assert.AreEqual(2.0f, device.GetScaleAtZ(10000.0f));
				Assert.AreEqual(2.0f, device.GetScaleAtZ(-10000.0f));

				// World space rendering with perspective projection
				device.Projection = ProjectionMode.Perspective;

				Assert.AreEqual(1.0f, device.GetScaleAtZ(0.0f));
				Assert.AreEqual(0.5f, device.GetScaleAtZ(1000.0f));
				Assert.AreEqual(0.25f, device.GetScaleAtZ(3000.0f));
				Assert.AreEqual(2.0f, device.GetScaleAtZ(-500.0f));
				Assert.AreEqual(4.0f, device.GetScaleAtZ(-750.0f));
				Assert.AreEqual(10.0f, device.GetScaleAtZ(-900.0f));
				Assert.AreEqual(10.0f, device.GetScaleAtZ(-1000.0f));
				Assert.AreEqual(10.0f, device.GetScaleAtZ(-10000.0f));
			}
		}
		[Test] public void GetScreenPos()
		{
			using (DrawDevice device = new DrawDevice())
			{
				Vector2 targetSize = new Vector2(800, 600);
				Vector2 viewportCenter = targetSize * 0.5f;

				// We'll check twice the focus distance to make sure orthographic
				// scaling is working as expected.
				device.FocusDist = DrawDevice.DefaultFocusDist * 2.0f;
				device.NearZ = 100;
				device.FarZ = 10000;
				device.TargetSize = targetSize;
				device.ViewportRect = new Rect(targetSize);
				device.ViewerPos = new Vector3(0, 0, -device.FocusDist);

				// Screen space rendering
				device.Projection = ProjectionMode.Screen;

				// 1:1 screen coordinate output in all cases
				Assert.AreEqual(new Vector2(0.0f, 0.0f), device.GetScreenPos(new Vector3(0.0f, 0.0f, 0.0f)));
				Assert.AreEqual(new Vector2(400.0f, 300.0f), device.GetScreenPos(new Vector3(400.0f, 300.0f, 0.0f)));
				Assert.AreEqual(new Vector2(800.0f, 600.0f), device.GetScreenPos(new Vector3(800.0f, 600.0f, 0.0f)));
				Assert.AreEqual(new Vector2(0.0f, 0.0f), device.GetScreenPos(new Vector3(0.0f, 0.0f, 1000.0f)));
				Assert.AreEqual(new Vector2(400.0f, 0.0f), device.GetScreenPos(new Vector3(400.0f, 0.0f, 1000.0f)));
				Assert.AreEqual(new Vector2(800.0f, 0.0f), device.GetScreenPos(new Vector3(800.0f, 0.0f, 1000.0f)));
				Assert.AreEqual(new Vector2(0.0f, 300.0f), device.GetScreenPos(new Vector3(0.0f, 300.0f, 1000.0f)));
				Assert.AreEqual(new Vector2(0.0f, 600.0f), device.GetScreenPos(new Vector3(0.0f, 600.0f, 1000.0f)));

				// World space rendering with orthographic projection
				device.Projection = ProjectionMode.Orthographic;

				// Scaled up 2:1 due to focus distance scaling factor
				Assert.AreEqual(viewportCenter, device.GetScreenPos(new Vector3(0.0f, 0.0f, 0.0f)));
				Assert.AreEqual(viewportCenter + new Vector2(-400.0f, -300.0f), device.GetScreenPos(new Vector3(-200.0f, -150.0f, 0.0f)));
				Assert.AreEqual(viewportCenter + new Vector2(400.0f, 300.0f), device.GetScreenPos(new Vector3(200.0f, 150.0f, 0.0f)));

				// No scale changes at other distances
				Assert.AreEqual(viewportCenter, device.GetScreenPos(new Vector3(0.0f, 0.0f, 1000.0f)));
				Assert.AreEqual(viewportCenter + new Vector2(-400.0f, 0.0f), device.GetScreenPos(new Vector3(-200.0f, 0.0f, 1000.0f)));
				Assert.AreEqual(viewportCenter + new Vector2(400.0f, 0.0f), device.GetScreenPos(new Vector3(200.0f, 0.0f, 1000.0f)));
				Assert.AreEqual(viewportCenter + new Vector2(0.0f, -300.0f), device.GetScreenPos(new Vector3(0.0f, -150.0f, 1000.0f)));
				Assert.AreEqual(viewportCenter + new Vector2(0.0f, 300.0f), device.GetScreenPos(new Vector3(0.0f, 150.0f, 1000.0f)));

				// World space rendering with perspective projection
				device.Projection = ProjectionMode.Perspective;
				
				// 1:1 scaling at focus distance
				Assert.AreEqual(viewportCenter, device.GetScreenPos(new Vector3(0.0f, 0.0f, 0.0f)));
				Assert.AreEqual(viewportCenter + new Vector2(-400.0f, -300.0f), device.GetScreenPos(new Vector3(-400.0f, -300.0f, 0.0f)));
				Assert.AreEqual(viewportCenter + new Vector2(400.0f, 300.0f), device.GetScreenPos(new Vector3(400.0f, 300.0f, 0.0f)));

				// Scaled down 1:2 at double the focus distance
				Assert.AreEqual(viewportCenter, device.GetScreenPos(new Vector3(0.0f, 0.0f, 1000.0f)));
				Assert.AreEqual(viewportCenter + new Vector2(-200.0f, 0.0f), device.GetScreenPos(new Vector3(-400.0f, 0.0f, 1000.0f)));
				Assert.AreEqual(viewportCenter + new Vector2(200.0f, 0.0f), device.GetScreenPos(new Vector3(400.0f, 0.0f, 1000.0f)));
				Assert.AreEqual(viewportCenter + new Vector2(0.0f, -150.0f), device.GetScreenPos(new Vector3(0.0f, -300.0f, 1000.0f)));
				Assert.AreEqual(viewportCenter + new Vector2(0.0f, 150.0f), device.GetScreenPos(new Vector3(0.0f, 300.0f, 1000.0f)));
			}
		}
		[Test] public void GetWorldPos()
		{
			using (DrawDevice device = new DrawDevice())
			{
				Vector2 targetSize = new Vector2(800, 600);
				Vector2 viewportCenter = targetSize * 0.5f;

				// We'll check twice the focus distance to make sure orthographic
				// scaling is working as expected.
				device.FocusDist = DrawDevice.DefaultFocusDist * 2.0f;
				device.NearZ = 100;
				device.FarZ = 10000;
				device.TargetSize = targetSize;
				device.ViewportRect = new Rect(targetSize);
				device.ViewerPos = new Vector3(0, 0, -device.FocusDist);

				// Screen space rendering
				device.Projection = ProjectionMode.Screen;

				// 1:1 world coordinate output in all cases
				AssertRoughlyEqual(new Vector3(0.0f, 0.0f, 0.0f), device.GetWorldPos(new Vector3(0.0f, 0.0f, 0.0f)));
				AssertRoughlyEqual(new Vector3(400.0f, 300.0f, 0.0f), device.GetWorldPos(new Vector3(400.0f, 300.0f, 0.0f)));
				AssertRoughlyEqual(new Vector3(800.0f, 600.0f, 0.0f), device.GetWorldPos(new Vector3(800.0f, 600.0f, 0.0f)));
				AssertRoughlyEqual(new Vector3(0.0f, 0.0f, 1000.0f), device.GetWorldPos(new Vector3(0.0f, 0.0f, 1000.0f)));
				AssertRoughlyEqual(new Vector3(400.0f, 0.0f, 1000.0f), device.GetWorldPos(new Vector3(400.0f, 0.0f, 1000.0f)));
				AssertRoughlyEqual(new Vector3(800.0f, 0.0f, 1000.0f), device.GetWorldPos(new Vector3(800.0f, 0.0f, 1000.0f)));
				AssertRoughlyEqual(new Vector3(0.0f, 300.0f, 1000.0f), device.GetWorldPos(new Vector3(0.0f, 300.0f, 1000.0f)));
				AssertRoughlyEqual(new Vector3(0.0f, 600.0f, 1000.0f), device.GetWorldPos(new Vector3(0.0f, 600.0f, 1000.0f)));

				// World space rendering with orthographic projection
				device.Projection = ProjectionMode.Orthographic;

				// Scaled up 2:1 due to focus distance scaling factor
				AssertRoughlyEqual(new Vector3(0.0f, 0.0f, 0.0f), device.GetWorldPos(new Vector3(viewportCenter, 0.0f)));
				AssertRoughlyEqual(new Vector3(-200.0f, -150.0f, 0.0f), device.GetWorldPos(new Vector3(viewportCenter + new Vector2(-400.0f, -300.0f), 0.0f)));
				AssertRoughlyEqual(new Vector3(200.0f, 150.0f, 0.0f), device.GetWorldPos(new Vector3(viewportCenter + new Vector2(400.0f, 300.0f), 0.0f)));

				// No scale changes at other distances
				AssertRoughlyEqual(new Vector3(0.0f, 0.0f, 1000.0f), device.GetWorldPos(new Vector3(viewportCenter, 1000.0f)));
				AssertRoughlyEqual(new Vector3(-200.0f, 0.0f, 1000.0f), device.GetWorldPos(new Vector3(viewportCenter + new Vector2(-400.0f, 0.0f), 1000.0f)));
				AssertRoughlyEqual(new Vector3(200.0f, 0.0f, 1000.0f), device.GetWorldPos(new Vector3(viewportCenter + new Vector2(400.0f, 0.0f), 1000.0f)));
				AssertRoughlyEqual(new Vector3(0.0f, -150.0f, 1000.0f), device.GetWorldPos(new Vector3(viewportCenter + new Vector2(0.0f, -300.0f), 1000.0f)));
				AssertRoughlyEqual(new Vector3(0.0f, 150.0f, 1000.0f), device.GetWorldPos(new Vector3(viewportCenter + new Vector2(0.0f, 300.0f), 1000.0f)));

				// World space rendering with perspective projection
				device.Projection = ProjectionMode.Perspective;
				
				// 1:1 scaling at focus distance
				AssertRoughlyEqual(new Vector3(0.0f, 0.0f, 0.0f), device.GetWorldPos(new Vector3(viewportCenter, 0.0f)));
				AssertRoughlyEqual(new Vector3(-400.0f, -300.0f, 0.0f), device.GetWorldPos(new Vector3(viewportCenter + new Vector2(-400.0f, -300.0f), 0.0f)));
				AssertRoughlyEqual(new Vector3(400.0f, 300.0f, 0.0f), device.GetWorldPos(new Vector3(viewportCenter + new Vector2(400.0f, 300.0f), 0.0f)));

				// Scaled down 1:2 at double the focus distance
				AssertRoughlyEqual(new Vector3(0.0f, 0.0f, 1000.0f), device.GetWorldPos(new Vector3(viewportCenter, 1000.0f)));
				AssertRoughlyEqual(new Vector3(-400.0f, 0.0f, 1000.0f), device.GetWorldPos(new Vector3(viewportCenter + new Vector2(-200.0f, 0.0f), 1000.0f)));
				AssertRoughlyEqual(new Vector3(400.0f, 0.0f, 1000.0f), device.GetWorldPos(new Vector3(viewportCenter + new Vector2(200.0f, 0.0f), 1000.0f)));
				AssertRoughlyEqual(new Vector3(0.0f, -600.0f, 1000.0f), device.GetWorldPos(new Vector3(viewportCenter + new Vector2(0.0f, -300.0f), 1000.0f)));
				AssertRoughlyEqual(new Vector3(0.0f, 600.0f, 1000.0f), device.GetWorldPos(new Vector3(viewportCenter + new Vector2(0.0f, 300.0f), 1000.0f)));
			}
		}

		private static void AssertRoughlyEqual(Vector3 expected, Vector3 actual)
		{
			float threshold = 0.001f;
			Assert.IsTrue(
				(expected - actual).Length < threshold,
				string.Format(
					"{0} is equal to {1} within a threshold of {2}.",
					actual,
					expected,
					threshold));
		}
	}
}
