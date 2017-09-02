using System;
using System.Collections.Generic;
using System.Linq;

using Duality.Editor;
using Duality.Resources;

namespace Duality.Drawing
{
	public interface IDrawBatch
	{
		int VertexOffset { get; }
		int VertexCount { get; }
		VertexMode VertexMode { get; }
		BatchInfo Material { get; }
		VertexDeclaration VertexDeclaration { get; }

		bool SameVertexType(IDrawBatch other);
	}
}
