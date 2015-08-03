using System;
using System.Collections.Generic;
using System.Linq;

using Duality.Drawing;

namespace Duality.Backend.Dummy
{
	public class DummyGraphicsBackend : IGraphicsBackend
	{
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

		IEnumerable<ScreenResolution> IGraphicsBackend.AvailableScreenResolutions
		{
			get { return new ScreenResolution[] { new ScreenResolution(640, 480, 60) }; }
		}

		bool IDualityBackend.CheckAvailable()
		{
			return true;
		}
		void IDualityBackend.Init() { }
		void IDualityBackend.Shutdown() { }

		void IGraphicsBackend.BeginRendering(IDrawDevice device, RenderOptions options, RenderStats stats) { }
		void IGraphicsBackend.Render(IReadOnlyList<IDrawBatch> batches) { }
		void IGraphicsBackend.EndRendering() { }

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
	}
}
