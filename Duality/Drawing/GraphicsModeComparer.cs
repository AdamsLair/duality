using System.Collections.Generic;
using OpenTK.Graphics;

namespace Duality.Drawing
{
	/// <summary>
	/// Compares two <see cref="GraphicsMode">GraphicsModes</see>.
	/// </summary>
	public class GraphicsModeComparer : IEqualityComparer<GraphicsMode>
	{
		/// <summary>
		/// Returns whether two GraphicsModes are equal.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public bool Equals(GraphicsMode x, GraphicsMode y)
		{
			return 
				x.AccumulatorFormat == y.AccumulatorFormat &&
				x.Buffers == y.Buffers &&
				x.ColorFormat == y.ColorFormat &&
				x.Depth == y.Depth &&
				x.Samples == y.Samples &&
				x.Stencil == y.Stencil &&
				x.Stereo == y.Stereo;
		}
		/// <summary>
		/// Returns the hash code of a GraphicsMode.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public int GetHashCode(GraphicsMode obj)
		{
			return 
				obj.AccumulatorFormat.GetHashCode() ^
				obj.Buffers.GetHashCode() ^
				obj.ColorFormat.GetHashCode() ^
				obj.Depth.GetHashCode() ^
				obj.Samples.GetHashCode() ^
				obj.Stencil.GetHashCode() ^
				obj.Stereo.GetHashCode();
		}
	}
}
