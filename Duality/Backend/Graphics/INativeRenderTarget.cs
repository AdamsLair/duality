using System;
using System.Collections.Generic;
using System.Linq;

using Duality.Drawing;

namespace Duality.Backend
{
	public interface INativeRenderTarget : IDisposable
	{
		/// <summary>
		/// Initializes the rendering target and configures it. This method needs to be called before any
		/// oprations are performed using the target.
		/// </summary>
		/// <param name="targets"></param>
		/// <param name="multisample"></param>
		void Setup(IReadOnlyList<INativeTexture> targets, AAQuality multisample);

		/// <summary>
		/// Retrieves the rendering targets pixel data from video memory in the Rgba8 format.
		/// As a storage array type, either byte or <see cref="ColorRgba"/> is recommended.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="target">The target buffer to store transferred pixel data in.</param>
		/// <param name="dataLayout">The desired color layout of the specified buffer.</param>
		/// <param name="dataElementType">The desired color element type of the specified buffer.</param>
		/// <param name="targetIndex">The target texture lists index to read from.</param>
		/// <param name="x">The x position of the rectangular area to read.</param>
		/// <param name="y">The y position of the rectangular area to read.</param>
		/// <param name="width">The width of the rectangular area to read. Defaults to the rendering targets width.</param>
		/// <param name="height">The height of the rectangular area to read. Defaults to the rendering targets height.</param>
		void GetData<T>(
			T[] buffer,
			ColorDataLayout dataLayout,
			ColorDataElementType dataElementType, 
			int targetIndex, 
			int x, int y, 
			int width, int height) where T : struct;
	}
}
