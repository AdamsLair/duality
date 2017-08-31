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
		int VertexCount { get; }
		VertexMode VertexMode { get; }
		BatchInfo Material { get; }
		VertexDeclaration VertexDeclaration { get; }

		void UploadVertices(IVertexUploader target, List<IDrawBatch> uploadBatches);

		bool SameVertexType(IDrawBatch other);
		bool CanAppendJIT<T>(float invZSortAccuracy, float zSortIndex, BatchInfo material, VertexMode vertexMode) where T : struct, IVertexData;
		void AppendJIT(object vertexData, int length);
		bool CanAppend(IDrawBatch other);
		void Append(IDrawBatch other);
	}

	public interface IVertexUploader
	{
		void UploadBatchVertices(VertexDeclaration declaration, IntPtr vertices, int vertexCount);
	}
}
