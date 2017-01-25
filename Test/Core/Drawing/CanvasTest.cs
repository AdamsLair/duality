using System;
using System.IO;
using System.Text;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Diagnostics;

using Duality;
using Duality.Resources;
using Duality.Drawing;
using Duality.Tests.Properties;

using NUnit.Framework;

namespace Duality.Tests.Drawing
{
	[TestFixture(Category = TestCategory.Rendering)]
	public class CanvasTest
	{
		private Texture texCoordUV;

		[TestFixtureSetUp] public void FixtureSetup()
		{
			this.texCoordUV = new Texture(new Pixmap(BitmapToPixelData(TestRes.TexCoordUV)));
		}
		[TestFixtureTearDown] public void FixtureTearDown()
		{
			this.texCoordUV.Dispose();
			this.texCoordUV = null;
		}

		[Test] public void RenderFail()
		{
			// Perform the RedSquare test, but with a one pixel offset. This should fail.
			Assert.IsFalse(this.AreImagesEqual(TestRes.CanvasTestRedSquare, c => 
			{
				c.State.ColorTint = ColorRgba.Red;
				c.FillRect(1, 0, c.Width, c.Height);
			}));
		}
		[Test] public void RenderNothing()
		{
			this.TestImagesEqual(TestRes.CanvasTestNothing, c => {});
		}
		[Test] public void RenderRedSquare()
		{
			this.TestImagesEqual(TestRes.CanvasTestRedSquare, c => 
			{
				c.State.ColorTint = ColorRgba.Red;
				c.FillRect(0, 0, c.Width, c.Height);
			});
		}
		[Test] public void RenderDiagonalLine()
		{
			this.TestImagesEqual(TestRes.CanvasTestDiagonalLine, c => 
			{
				c.DrawLine(0, 0, c.Width, c.Height);
			});
		}
		[Test] public void RenderAllShapes()
		{
			this.TestImagesEqual(TestRes.CanvasTestAllShapes, c => 
			{
				// Background
				c.State.ColorTint = new ColorRgba(128, 192, 255);
				c.FillRect(0, 0, c.Width, c.Height);
				c.State.ColorTint = ColorRgba.White;

				// White shapes
				this.DrawTestImageRow(c, 100, 100);

				// Textured shapes
				c.State.SetMaterial(new BatchInfo(DrawTechnique.Mask, ColorRgba.White, this.texCoordUV));
				this.DrawTestImageRow(c, 100, 300);
			});
		}
		[Test] public void RenderAllShapesTransformed()
		{
			this.TestImagesEqual(TestRes.CanvasTestAllShapesTransformed, c => 
			{
				// Background
				c.State.ColorTint = new ColorRgba(128, 192, 255);
				c.FillRect(0, 0, c.Width, c.Height);
				c.State.ColorTint = ColorRgba.White;
				c.State.TransformHandle = new Vector2(5, 5);
				c.State.TransformScale = new Vector2(0.75f, 0.75f);
				c.State.TransformAngle = MathF.RadAngle30;

				// White shapes
				this.DrawTestImageRow(c, 100, 100);

				// Textured shapes
				c.PushState();
				c.State.SetMaterial(new BatchInfo(DrawTechnique.Mask, ColorRgba.White, this.texCoordUV));
				this.DrawTestImageRow(c, 100, 300);
				c.PopState();
			});
		}
		
		private void DrawTestImageRow(Canvas c, int baseX, int baseY)
		{
			Vector2[] polygon = new Vector2[]
			{ 
				new Vector2(0.0f, 0.0f), 
				new Vector2(50.0f, 0.0f), 
				new Vector2(50.0f, 45.0f), 
				new Vector2(5.0f, 50.0f), 
			};

			int x = baseX;
			int y = baseY;

			// Outline Shapes
			c.PushState();
			{
				c.DrawCircle(x, y, 25);
				x += 100;

				c.DrawCircleSegment(x, y, 25, 0.0f, MathF.RadAngle30 * 4, true);
				x += 100;

				c.DrawLine(x, y , x + 50, y + 25);
				c.DrawDashLine(x, y + 25, x + 50, y + 50);
				c.DrawThickLine(x, y + 50, x + 50, y + 75, 3);
				x += 100;

				c.DrawOval(x, y, 50, 50);
				x += 100;

				c.DrawOvalSegment(x, y, 50, 50, 0.0f, MathF.RadAngle30 * 4, true);
				x += 100;

				c.DrawPolygon(polygon, x, y);
				x += 100;

				c.DrawRect(x, y, 50, 50);
				x += 100;

				c.DrawText("Hello World", x, y, drawBackground: true);
				x = baseX;
				y += 100;
			}
			c.PopState();

			// Filled Shapes
			c.PushState();
			{
				c.FillCircle(x, y, 25);
				x += 100;

				c.FillCircleSegment(x, y, 0, 25, MathF.RadAngle30 * 0, MathF.RadAngle30 * 4);
				c.FillCircleSegment(x, y, 0, 25, MathF.RadAngle30 * 5, MathF.RadAngle30 * 9, 10);
				x += 100;

				c.FillThickLine(x, y + 25, x + 50, y + 50, 3);
				x += 100;

				c.FillOval(x, y, 50, 50);
				x += 100;

				c.FillOvalSegment(x, y, 0, 50, 50, MathF.RadAngle30 * 0, MathF.RadAngle30 * 4);
				c.FillOvalSegment(x, y, 0, 50, 50, MathF.RadAngle30 * 5, MathF.RadAngle30 * 9, 10);
				x += 100;

				c.FillPolygon(polygon, x, y);
				x += 100;

				c.FillRect(x, y, 50, 50);
				x = baseX;
				y += 100;
			}
			c.PopState();
		}

		private void TestImagesEqual(Bitmap referenceImage, Action<Canvas> renderMethod)
		{
			PixelData image = this.RenderToTexture(referenceImage.Width, referenceImage.Height, renderMethod);
			PixelData reference = BitmapToPixelData(referenceImage);
			bool equal = this.AreImagesEqual(reference, image);
			if (!equal && Debugger.IsAttached)
			{
				// If the debugger is attached, create a nice diff image for the programmer to view.
				PixelData diff = this.CreateDiffImage(image, reference);
			}
			Assert.IsTrue(equal);
		}
		private bool AreImagesEqual(Bitmap referenceImage, Action<Canvas> renderMethod)
		{
			PixelData image = this.RenderToTexture(referenceImage.Width, referenceImage.Height, renderMethod);
			PixelData reference = BitmapToPixelData(referenceImage);
			return this.AreImagesEqual(reference, image);
		}
		private bool AreImagesEqual(PixelData first, PixelData second)
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
		
		private void UpdateReferenceImage(Expression<Func<Bitmap>> oldReferenceImageGetter, Action<Canvas> renderMethod)
		{
			var resourceMember = oldReferenceImageGetter.Body as MemberExpression;
			if (resourceMember == null) throw new ArgumentException("Only lambda methods of the form '() => Property' are accepted");

			string name = resourceMember.Member.Name;
			Bitmap oldRef = oldReferenceImageGetter.Compile()();

			this.CreateReferenceImage(name, oldRef.Width, oldRef.Height, renderMethod);
		}
		private void CreateReferenceImage(string name, int width, int height, Action<Canvas> renderMethod)
		{
			PixelData image = this.RenderToTexture(width, height, renderMethod);

			string formatId = ImageCodec.FormatPng;
			IImageCodec codec = ImageCodec.GetWrite(formatId);
			using (Stream stream = File.Open(TestHelper.GetEmbeddedResourcePath(name, ".png"), FileMode.Create))
			{
				codec.Write(stream, image, formatId);
			}
		}

		private PixelData CreateDiffImage(PixelData first, PixelData second)
		{
			if (first == second) return new PixelData(first.Width, first.Height);
			if (first.Width != second.Width) return new PixelData(1, 1);
			if (first.Height != second.Height) return new PixelData(1, 1);

			PixelData diff = new PixelData(first.Width, first.Height);
			ColorRgba[] firstData = first.Data;
			ColorRgba[] secondData = second.Data;
			ColorRgba[] diffData = diff.Data;
			for (int i = 0; i < firstData.Length; i++)
			{
				diffData[i].R = (byte)MathF.Abs(firstData[i].R - secondData[i].R);
				diffData[i].G = (byte)MathF.Abs(firstData[i].G - secondData[i].G);
				diffData[i].B = (byte)MathF.Abs(firstData[i].B - secondData[i].B);
				diffData[i].A = (byte)MathF.Abs(firstData[i].A - secondData[i].A);
			}

			return diff;
		}
		private PixelData RenderToTexture(int width, int height, Action<Canvas> renderMethod)
		{
			PixelData pixelData;

			using (Texture texture = new Texture(width, height, TextureSizeMode.NonPowerOfTwo))
			using (RenderTarget renderTarget = new RenderTarget(AAQuality.Off, true, texture))
			using (DrawDevice device = new DrawDevice())
			{
				device.Perspective = PerspectiveMode.Flat;
				device.VisibilityMask = VisibilityFlag.AllGroups | VisibilityFlag.ScreenOverlay;
				device.RenderMode = RenderMatrix.ScreenSpace;
				device.Target = renderTarget;
				device.TargetSize = renderTarget.Size;
				device.ViewportRect = new Rect(renderTarget.Size);

				device.PrepareForDrawcalls();
				{
					Canvas canvas = new Canvas(device);
					renderMethod(canvas);
				}
				device.Render();

				pixelData = texture.GetPixelData();
			}

			return pixelData;
		}
		
		private static PixelData BitmapToPixelData(Bitmap bitmap)
		{
			PixelData pixelData = new PixelData();
			pixelData.FromBitmap(bitmap);
			return pixelData;
		}
	}
}
