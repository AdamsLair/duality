using System;
using System.Collections.Generic;
using System.Linq;

namespace Duality.Drawing
{
	/// <summary>
	/// Specifies the way in which incoming vertex data is interpreted in order to generate geometry.
	/// </summary>
	public enum VertexMode : byte
	{
		Points,
		Lines,
		LineStrip,
		LineLoop,
		Triangles,
		TriangleStrip,
		TriangleFan,
		Quads,
	}

	public static class ExtMethodsVertexMode
	{
		public static bool IsBatchableMode(this VertexMode mode)
		{
			return 
				mode == VertexMode.Lines || 
				mode == VertexMode.Points || 
				mode == VertexMode.Quads || 
				mode == VertexMode.Triangles;
		}
	}
}
