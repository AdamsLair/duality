using System;
using System.Collections.Generic;
using System.Linq;

using Duality.Drawing;

namespace Duality.Backend
{
	public interface IGraphicsBackend : IDualityBackend
	{
		/// <summary>
		/// Returns information about the backends graphics / rendering capabilities.
		/// </summary>
		GraphicsBackendCapabilities Capabilities { get; }
		IEnumerable<ScreenResolution> AvailableScreenResolutions { get; }
		Point2 ExternalBackbufferSize { get; set; }
		INativeWindow ActiveWindow { get; }

		void BeginRendering(IDrawDevice device, RenderOptions options, RenderStats stats = null);
		void Render(IReadOnlyList<DrawBatch> batches);
		void EndRendering();

		INativeGraphicsBuffer CreateBuffer(GraphicsBufferType type);
		INativeTexture CreateTexture();
		INativeRenderTarget CreateRenderTarget();
		INativeShaderPart CreateShaderPart();
		INativeShaderProgram CreateShaderProgram();
		INativeWindow CreateWindow(WindowOptions options);

		/// <summary>
		/// Retrieves the main rendering buffer's pixel data from video memory in the Rgba8 format.
		/// 
		/// Note that generic, array-based variants of this method are available via extension method
		/// when using the Duality.Backend namespace.
		/// </summary>
		/// <param name="target">The target buffer to store transferred pixel data in.</param>
		/// <param name="dataLayout">The desired color layout of the specified buffer.</param>
		/// <param name="dataElementType">The desired color element type of the specified buffer.</param>
		/// <param name="x">The x position of the rectangular area to read.</param>
		/// <param name="y">The y position of the rectangular area to read.</param>
		/// <param name="width">The width of the rectangular area to read. Defaults to the rendering targets width.</param>
		/// <param name="height">The height of the rectangular area to read. Defaults to the rendering targets height.</param>
		void GetOutputPixelData(
			IntPtr target,
			ColorDataLayout dataLayout,
			ColorDataElementType dataElementType, 
			int x, int y, 
			int width, int height);
	}

	public static class ExtMethodsIGraphicsBackend
	{
		/// <summary>
		/// Retrieves the main rendering buffer's pixel data from video memory in the Rgba8 format.
		/// As a storage array type, either byte or <see cref="ColorRgba"/> is recommended.
		/// </summary>
		/// <param name="target">The target buffer to store transferred pixel data in.</param>
		/// <param name="dataLayout">The desired color layout of the specified buffer.</param>
		/// <param name="dataElementType">The desired color element type of the specified buffer.</param>
		/// <param name="x">The x position of the rectangular area to read.</param>
		/// <param name="y">The y position of the rectangular area to read.</param>
		/// <param name="width">The width of the rectangular area to read. Defaults to the rendering targets width.</param>
		/// <param name="height">The height of the rectangular area to read. Defaults to the rendering targets height.</param>
		public static void GetOutputPixelData<T>(
			this IGraphicsBackend backend,
			T[] target,
			ColorDataLayout dataLayout,
			ColorDataElementType dataElementType, 
			int x, int y, 
			int width, int height) where T : struct
		{
			using (PinnedArrayHandle pinned = new PinnedArrayHandle(target))
			{
				backend.GetOutputPixelData(
					pinned.Address,
					dataLayout, 
					dataElementType, 
					x, y, 
					width, 
					height);
			}
		}
	}
}
