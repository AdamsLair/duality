using System;
using System.IO;
using System.Text;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;

using Duality;
using Duality.Resources;
using Duality.Animation;
using Duality.ColorFormat;
using Duality.Tests.TestingResources;

using OpenTK;

using NUnit.Framework;

namespace Duality.Tests.Graphics
{
	[TestFixture]
	public class CanvasTest
	{
		[Test] public void RenderFail()
		{
			// Perform the RedSquare test, but with a one pixel offset. This should fail.
			Assert.IsFalse(this.AreImagesEqual(TestRes.CanvasTestRedSquare, c => 
			{
				c.CurrentState.ColorTint = ColorRgba.Red;
				c.FillRect(1, 0, c.Width, c.Height);
			}));
		}
		[Test] public void RenderNothing()
		{
			Assert.IsTrue(this.AreImagesEqual(TestRes.CanvasTestNothing, c => {}));
		}
		[Test] public void RenderRedSquare()
		{
			Assert.IsTrue(this.AreImagesEqual(TestRes.CanvasTestRedSquare, c => 
			{
				c.CurrentState.ColorTint = ColorRgba.Red;
				c.FillRect(0, 0, c.Width, c.Height);
			}));
		}
		[Test] public void RenderDiagonalLine()
		{
			Assert.IsTrue(this.AreImagesEqual(TestRes.CanvasTestDiagonalLine, c => 
			{
				c.DrawLine(0, 0, c.Width, c.Height);
			}));
		}
		
		private void CreateReferenceImage(int width, int height, Action<Canvas> renderMethod)
		{
			Pixmap.Layer image = this.RenderToTexture(width, height, renderMethod);
			image.SavePixelData(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "CanvasTestOutput.png"));
		}
		private bool AreImagesEqual(Bitmap referenceImage, Action<Canvas> renderMethod)
		{
			Pixmap.Layer image = this.RenderToTexture(referenceImage.Width, referenceImage.Height, renderMethod);
			Pixmap.Layer reference = new Pixmap.Layer(referenceImage);
			return this.AreImagesEqual(reference, image);
		}
		private bool AreImagesEqual(Pixmap.Layer first, Pixmap.Layer second)
		{
			if (first == second) return true;
			if (first.Width != second.Width) return false;
			if (first.Height != second.Height) return false;

			ColorRgba[] firstData = first.Data;
			ColorRgba[] secondData = second.Data;
			float error = 0;
			float maxError = firstData.Length; // (1/255) off per pixel is probably okay.
			for (int i = 0; i < firstData.Length; i++)
			{
				error += MathF.Abs(firstData[i].R * (firstData[i].A / 255.0f) - secondData[i].R * (secondData[i].A / 255.0f));
				error += MathF.Abs(firstData[i].G * (firstData[i].A / 255.0f) - secondData[i].G * (secondData[i].A / 255.0f));
				error += MathF.Abs(firstData[i].B * (firstData[i].A / 255.0f) - secondData[i].B * (secondData[i].A / 255.0f));
				error += MathF.Abs(firstData[i].A - secondData[i].A);
				if (error >= maxError) return false;
			}

			return true;
		}
		private Pixmap.Layer RenderToTexture(int width, int height, Action<Canvas> renderMethod)
		{
			Pixmap.Layer pixelData;

			using (Texture texture = new Texture(width, height, Texture.SizeMode.NonPowerOfTwo))
			using (RenderTarget renderTarget = new RenderTarget(AAQuality.Off, texture))
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
