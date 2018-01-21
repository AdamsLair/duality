using System;
using System.Collections.Generic;

using Duality.Backend;

namespace Duality.Drawing
{
	/// <summary>
	/// This is a container for vertex and index data that is stored on the GPU.
	/// </summary>
	[DontSerialize]
	public class VertexBuffer : IDisposable
	{
		private INativeGraphicsBuffer nativeVertex = null;
		private INativeGraphicsBuffer nativeIndex = null;
		private VertexDeclaration vertexType = null;
		private IndexDataElementType indexType = IndexDataElementType.UnsignedShort;
		private int vertexCount = 0;
		private int indexCount = 0;


		public INativeGraphicsBuffer NativeVertex
		{
			get { return this.nativeVertex; }
		}
		public VertexDeclaration VertexType
		{
			get { return this.vertexType; }
		}
		public int VertexCount
		{
			get { return this.vertexCount; }
		}

		public INativeGraphicsBuffer NativeIndex
		{
			get { return this.nativeIndex; }
		}
		public IndexDataElementType IndexType
		{
			get { return this.indexType; }
		}
		public int IndexCount
		{
			get { return this.indexCount; }
		}


		public void Dispose()
		{
			if (this.nativeVertex != null)
			{
				this.nativeVertex.Dispose();
				this.nativeVertex = null;
			}
			if (this.nativeIndex != null)
			{
				this.nativeIndex.Dispose();
				this.nativeIndex = null;
			}
		}

		public void ClearVertexData()
		{
			this.vertexCount = 0;
		}
		public void LoadVertexData<T>(T[] vertexData, int vertexCount) where T : struct, IVertexData
		{
			this.LoadVertexData(
				VertexDeclaration.Get<T>(),
				vertexData,
				vertexCount);
		}
		public void LoadVertexData<T>(VertexDeclaration vertexType, T[] vertexData, int vertexCount) where T : struct
		{
			this.EnsureNativeVertex();

			this.vertexCount = vertexCount;
			this.vertexType = vertexType;

			this.nativeVertex.LoadData(vertexData, vertexCount);
		}
		public void LoadVertexData(VertexDeclaration vertexType, IntPtr vertexData, int vertexCount)
		{
			this.EnsureNativeVertex();

			this.vertexCount = vertexCount;
			this.vertexType = vertexType;

			this.nativeVertex.LoadData(vertexData, (IntPtr)(vertexCount * vertexType.Size));
		}

		public void ClearIndexData()
		{
			this.indexCount = 0;
		}
		public void LoadIndexData<T>(T[] indexData, int indexCount) where T : struct
		{
			IndexDataElementType elementType;
			if      (typeof(T) == typeof(byte))   elementType = IndexDataElementType.UnsignedByte;
			else if (typeof(T) == typeof(ushort)) elementType = IndexDataElementType.UnsignedShort;
			else
			{
				throw new ArgumentException(string.Format(
					"Index type {0} is not supported. Either specify the type explicitly, or use any of the types listed in {1}",
					typeof(T).Name,
					typeof(IndexDataElementType).Name));
			}

			this.LoadIndexData<T>(elementType, indexData, indexCount);
		}
		public void LoadIndexData<T>(IndexDataElementType indexType, T[] indexData, int indexCount) where T : struct
		{
			this.EnsureNativeIndex();

			this.indexCount = indexCount;
			this.indexType = indexType;

			this.nativeIndex.LoadData(indexData, indexCount);
		}
		public void LoadIndexData(IndexDataElementType indexType, IntPtr indexData, int indexCount)
		{
			this.EnsureNativeIndex();

			this.indexCount = indexCount;
			this.indexType = indexType;

			int indexSize = 0;
			if      (indexType == IndexDataElementType.UnsignedByte)  indexSize = sizeof(byte);
			else if (indexType == IndexDataElementType.UnsignedShort) indexSize = sizeof(ushort);
			this.nativeIndex.LoadData(indexData, (IntPtr)(indexCount * indexSize));
		}

		private void EnsureNativeVertex()
		{
			if (this.nativeVertex == null)
				this.nativeVertex = DualityApp.GraphicsBackend.CreateBuffer(GraphicsBufferType.Vertex);
		}
		private void EnsureNativeIndex()
		{
			if (this.nativeIndex == null)
				this.nativeIndex = DualityApp.GraphicsBackend.CreateBuffer(GraphicsBufferType.Index);
		}
	}
}
