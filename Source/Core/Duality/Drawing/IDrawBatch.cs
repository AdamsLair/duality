using System;
using System.Collections.Generic;
using System.Linq;

using Duality.Editor;
using Duality.Resources;

namespace Duality.Drawing
{
	public interface IDrawBatch
	{
		int SortIndex { get; }
		float ZSortIndex { get; }
		int VertexOffset { get; }
		int VertexCount { get; }
		VertexMode VertexMode { get; }
		BatchInfo Material { get; }
		VertexDeclaration VertexDeclaration { get; }

		bool SameVertexType(IDrawBatch other);
		bool CanAppendJIT(VertexDeclaration vertexType, float invZSortAccuracy, float zSortIndex, BatchInfo material, VertexMode vertexMode);
		void AppendJIT(float zSortIndex, int count);
		bool CanAppend(IDrawBatch other);
		void Append(IDrawBatch other);
	}
}
