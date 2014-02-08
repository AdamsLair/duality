using System;
using System.Text;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;

using Duality;
using Duality.Resources;
using Duality.Animation;
using Duality.ColorFormat;

using OpenTK;

using NUnit.Framework;

namespace Duality.Tests.Animation
{
	[TestFixture]
	public class CanvasTest
	{
		[Test] public void RenderFail()
		{
			// Perform the RedSquare test, but with a one pixel offset. This should fail.
			Assert.IsFalse(this.AreImagesEqual(TestingResources.CanvasTestRedSquare, c => 
			{
				c.CurrentState.ColorTint = ColorRgba.Red;
				c.FillRect(1, 0, c.Width, c.Height);
			}));
		}
		[Test] public void RenderNothing()
		{
			Assert.IsTrue(this.AreImagesEqual(TestingResources.CanvasTestNothing, c => {}));
		}
		[Test] public void RenderRedSquare()
		{
			Assert.IsTrue(this.AreImagesEqual(TestingResources.CanvasTestRedSquare, c => 
			{
				c.CurrentState.ColorTint = ColorRgba.Red;
				c.FillRect(0, 0, c.Width, c.Height);
			}));
		}

		private bool AreImagesEqual(Bitmap referenceImage, Action<Canvas> renderMethod)
		{
			Pixmap.Layer reference = new Pixmap.Layer(referenceImage);
			Pixmap.Layer image = this.RenderToTexture(referenceImage.Width, referenceImage.Height, renderMethod);
			return this.AreImagesEqual(reference, image);
		}
		private bool AreImagesEqual(Pixmap.Layer first, Pixmap.Layer second)
		{
			if (first == second) return true;
			if (first.Width != second.Width) return false;
			if (first.Height != second.Height) return false;

			ColorRgba[] firstData = first.Data;
			ColorRgba[] secondData = second.Data;
			int error = 0;
			int maxError = firstData.Length / (16 * 16); // 1/255 off per 16x16 pixels is okay.
			for (int i = 0; i < firstData.Length; i++)
			{
				error += Math.Abs(firstData[i].R - secondData[i].R);
				error += Math.Abs(firstData[i].G - secondData[i].G);
				error += Math.Abs(firstData[i].B - secondData[i].B);
				error += Math.Abs(firstData[i].A - secondData[i].A);
				if (error >= maxError) return false;
			}

			return true;
		}
		private Pixmap.Layer RenderToTexture(int width, int height, Action<Canvas> renderMethod)
		{
			Pixmap.Layer pixelData;

			using (Texture texture = new Texture(width, height, Texture.SizeMode.NonPowerOfTwo))
			using (RenderTarget renderTarget = new RenderTarget(AAQuality.High, texture))
			using (DrawDevice device = new DrawDevice())
			{
				device.Perspective = PerspectiveMode.Flat;
				device.VisibilityMask = VisibilityFlag.AllGroups | VisibilityFlag.ScreenOverlay;
				device.RenderMode = RenderMatrix.OrthoScreen;
				device.Target = renderTarget;
				device.ViewportRect = new Rect(renderTarget.Width, renderTarget.Height);

				device.BeginRendering(ClearFlag.All, ColorRgba.TransparentBlack, 1.0f);
				{
					Canvas canvas = new Canvas(device);
					renderMethod(canvas);
				}
				device.EndRendering();
				
				RenderTarget.Bind(RenderTarget.None);

				pixelData = texture.RetrievePixelData();
			}

			return pixelData;
		}
	}
}
