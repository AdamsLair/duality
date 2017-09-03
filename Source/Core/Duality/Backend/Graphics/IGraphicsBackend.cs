using System;
using System.Collections.Generic;
using System.Linq;

using Duality.Drawing;

namespace Duality.Backend
{
	public interface IGraphicsBackend : IDualityBackend
	{
		IEnumerable<ScreenResolution> AvailableScreenResolutions { get; }
		Point2 ExternalBackbufferSize { get; set; }

		void BeginRendering(IDrawDevice device, VertexBatchStore vertexData, RenderOptions options, RenderStats stats = null);
		void Render(IReadOnlyList<VertexDrawBatch> batches);
		void EndRendering();

		INativeTexture CreateTexture();
		INativeRenderTarget CreateRenderTarget();
		INativeShaderPart CreateShaderPart();
		INativeShaderProgram CreateShaderProgram();
		INativeWindow CreateWindow(WindowOptions options);

		/// <summary>
		/// Retrieves the main rendering buffer's pixel data from video memory in the Rgba8 format.
		/// As a storage array type, either byte or <see cref="ColorRgba"/> is recommended.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="target">The target buffer to store transferred pixel data in.</param>
		/// <param name="dataLayout">The desired color layout of the specified buffer.</param>
		/// <param name="dataElementType">The desired color element type of the specified buffer.</param>
		/// <param name="x">The x position of the rectangular area to read.</param>
		/// <param name="y">The y position of the rectangular area to read.</param>
		/// <param name="width">The width of the rectangular area to read. Defaults to the rendering targets width.</param>
		/// <param name="height">The height of the rectangular area to read. Defaults to the rendering targets height.</param>
		void GetOutputPixelData<T>(
			T[] buffer,
			ColorDataLayout dataLayout,
			ColorDataElementType dataElementType, 
			int x, int y, 
			int width, int height) where T : struct;
	}
}
