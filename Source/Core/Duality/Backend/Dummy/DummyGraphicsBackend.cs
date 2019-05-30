using System;
using System.Collections.Generic;
using System.Linq;

using Duality.Drawing;

namespace Duality.Backend.Dummy
{
	public class DummyGraphicsBackend : IGraphicsBackend
	{
		private GraphicsBackendCapabilities capabilities = new GraphicsBackendCapabilities();

		string IDualityBackend.Id
		{
			get { return "DummyGraphicsBackend"; }
		}
		string IDualityBackend.Name
		{
			get { return "No Graphics"; }
		}
		int IDualityBackend.Priority
		{
			get { return int.MinValue; }
		}

		GraphicsBackendCapabilities IGraphicsBackend.Capabilities
		{
			get { return this.capabilities; }
		}
		IEnumerable<ScreenResolution> IGraphicsBackend.AvailableScreenResolutions
		{
			get { return new ScreenResolution[] { new ScreenResolution(640, 480, 60) }; }
		}
		Point2 IGraphicsBackend.ExternalBackbufferSize
		{
			get { return Point2.Zero; }
			set { }
		}

		bool IDualityBackend.CheckAvailable()
		{
			return true;
		}
		void IDualityBackend.Init()
		{
			Logs.Core.WriteWarning("DummyGraphicsBackend initialized. This is unusual and may cause problems when interacting with graphic devices or rendering.");
		}
		void IDualityBackend.Shutdown() { }

		void IGraphicsBackend.BeginRendering(IDrawDevice device, RenderOptions options, RenderStats stats) { }
		void IGraphicsBackend.Render(IReadOnlyList<DrawBatch> batches) { }
		void IGraphicsBackend.EndRendering() { }

		INativeGraphicsBuffer IGraphicsBackend.CreateBuffer(GraphicsBufferType type)
		{
			return new DummyNativeGraphicsBuffer(type);
		}
		INativeTexture IGraphicsBackend.CreateTexture()
		{
			return new DummyNativeTexture();
		}
		INativeRenderTarget IGraphicsBackend.CreateRenderTarget()
		{
			return new DummyNativeRenderTarget();
		}
		INativeShaderPart IGraphicsBackend.CreateShaderPart()
		{
			return new DummyNativeShaderPart();
		}
		INativeShaderProgram IGraphicsBackend.CreateShaderProgram()
		{
			return new DummyNativeShaderProgram();
		}
		INativeWindow IGraphicsBackend.CreateWindow(WindowOptions options)
		{
			return new DummyNativeWindow();
		}

		void IGraphicsBackend.GetOutputPixelData(IntPtr target, ColorDataLayout dataLayout, ColorDataElementType dataElementType, int x, int y, int width, int height) { }
	}
}
