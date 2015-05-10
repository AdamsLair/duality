using System;
using System.Collections.Generic;
using System.Linq;

using OpenTK.Graphics.OpenGL;
using OpenTK;

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
		VertexFormatDefinition VertexFormat { get; }

		void UploadVertices(IVertexUploader target, List<IDrawBatch> uploadBatches);

		bool SameVertexType(IDrawBatch other);
		bool CanAppendJIT<T>(float invZSortAccuracy, float zSortIndex, BatchInfo material, VertexMode vertexMode) where T : struct, IVertexData;
		void AppendJIT(object vertexData, int length);
		bool CanAppend(IDrawBatch other);
		void Append(IDrawBatch other);
	}

	public interface IVertexUploader
	{
		void UploadBatchVertices<T>(VertexFormatDefinition format, T[] vertices, int vertexCount) where T : struct, IVertexData;
	}
}
