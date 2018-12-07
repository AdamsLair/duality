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

namespace Duality.Tests.Resources
{
	[TestFixture(Category = TestCategory.Rendering)]
	public class RenderTargetTest
	{
		[Test] public void RenderFail()
		{
			PixelData pixelData = this.RenderToTexture(8, 8, AAQuality.Off, device =>
			{
				device.AddVertices(Material.SolidWhite, VertexMode.Quads,
					new VertexC1P3 { Pos = new Vector3(1, 1, 0), Color = ColorRgba.Red },
					new VertexC1P3 { Pos = new Vector3(1, device.TargetSize.Y, 0), Color = ColorRgba.Red },
					new VertexC1P3 { Pos = new Vector3(device.TargetSize.X, device.TargetSize.Y, 0), Color = ColorRgba.Red },
					new VertexC1P3 { Pos = new Vector3(device.TargetSize.X, 1, 0), Color = ColorRgba.Red });
			});
			Assert.IsFalse(this.IsFilledWithColor(pixelData, ColorRgba.Red));
		}
		[Test] public void RenderBasic()
		{
			PixelData pixelData = this.RenderToTexture(8, 8, AAQuality.Off, device =>
			{
				device.AddVertices(Material.SolidWhite, VertexMode.Quads,
					new VertexC1P3 { Pos = new Vector3(0, 0, 0), Color = ColorRgba.Red },
					new VertexC1P3 { Pos = new Vector3(0, device.TargetSize.Y, 0), Color = ColorRgba.Red },
					new VertexC1P3 { Pos = new Vector3(device.TargetSize.X, device.TargetSize.Y, 0), Color = ColorRgba.Red },
					new VertexC1P3 { Pos = new Vector3(device.TargetSize.X, 0, 0), Color = ColorRgba.Red });
			});
			Assert.IsTrue(this.IsFilledWithColor(pixelData, ColorRgba.Red));
		}
		[Test] public void RenderAntialiased()
		{
			PixelData pixelData = this.RenderToTexture(8, 8, AAQuality.High, device =>
			{
				device.AddVertices(Material.SolidWhite, VertexMode.Quads,
					new VertexC1P3 { Pos = new Vector3(0, 0, 0), Color = ColorRgba.Red },
					new VertexC1P3 { Pos = new Vector3(0, device.TargetSize.Y, 0), Color = ColorRgba.Red },
					new VertexC1P3 { Pos = new Vector3(device.TargetSize.X, device.TargetSize.Y, 0), Color = ColorRgba.Red },
					new VertexC1P3 { Pos = new Vector3(device.TargetSize.X, 0, 0), Color = ColorRgba.Red });
			});
			Assert.IsTrue(this.IsFilledWithColor(pixelData, ColorRgba.Red));
		}
		[Test] public void RenderAndDispose()
		{
			PixelData pixelData;

			// In this test, we'll dispose the render target immediately after using it
			// and only retrieve the pixel data from its bound texture later on.
			// Since the texture that was rendered to is still alive, this should work.
			using (Texture texture = new Texture(8, 8, TextureSizeMode.NonPowerOfTwo, TextureMagFilter.Nearest, TextureMinFilter.Nearest))
			{
				using (RenderTarget renderTarget = new RenderTarget(AAQuality.High, false, texture))
				using (DrawDevice device = new DrawDevice())
				{
					device.Projection = ProjectionMode.Screen;
					device.VisibilityMask = VisibilityFlag.AllGroups | VisibilityFlag.ScreenOverlay;
					device.Target = renderTarget;
					device.TargetSize = renderTarget.Size;
					device.ViewportRect = new Rect(renderTarget.Size);

					device.PrepareForDrawcalls();
					device.AddVertices(Material.SolidWhite, VertexMode.Quads,
						new VertexC1P3 { Pos = new Vector3(0, 0, 0), Color = ColorRgba.Red },
						new VertexC1P3 { Pos = new Vector3(0, device.TargetSize.Y, 0), Color = ColorRgba.Red },
						new VertexC1P3 { Pos = new Vector3(device.TargetSize.X, device.TargetSize.Y, 0), Color = ColorRgba.Red },
						new VertexC1P3 { Pos = new Vector3(device.TargetSize.X, 0, 0), Color = ColorRgba.Red });
					device.Render();
				}

				pixelData = texture.GetPixelData();
			}

			Assert.IsTrue(this.IsFilledWithColor(pixelData, ColorRgba.Red));
		}

		private bool IsFilledWithColor(PixelData pixelData, ColorRgba color)
		{
			for (int i = 0; i < pixelData.Data.Length; i++)
			{
				if (pixelData.Data[i] != color)
					return false;
			}
			return true;
		}
		private PixelData RenderToTexture(int width, int height, AAQuality antialiasing, Action<IDrawDevice> renderMethod)
		{
			PixelData pixelData;

			using (Texture texture = new Texture(width, height, TextureSizeMode.NonPowerOfTwo, TextureMagFilter.Nearest, TextureMinFilter.Nearest))
			using (RenderTarget renderTarget = new RenderTarget(antialiasing, false, texture))
			using (DrawDevice device = new DrawDevice())
			{
				device.Projection = ProjectionMode.Screen;
				device.VisibilityMask = VisibilityFlag.AllGroups | VisibilityFlag.ScreenOverlay;
				device.Target = renderTarget;
				device.TargetSize = renderTarget.Size;
				device.ViewportRect = new Rect(renderTarget.Size);

				device.PrepareForDrawcalls();
				renderMethod(device);
				device.Render();

				pixelData = renderTarget.GetPixelData();
			}

			return pixelData;
		}
	}
}
