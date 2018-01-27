using System;
using System.Collections.Generic;

using Duality.Backend;

namespace Duality.Drawing
{
	/// <summary>
	/// <see cref="VertexBuffer"/> is a container for vertex and index data that is stored on the GPU.
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


		/// <summary>
		/// [GET] A reference to the native backend storage for this buffers vertex data.
		/// </summary>
		public INativeGraphicsBuffer NativeVertex
		{
			get { return this.nativeVertex; }
		}
		/// <summary>
		/// [GET] A <see cref="VertexDeclaration"/> that describes the kind of vertex data 
		/// that is stored in this buffer.
		/// </summary>
		public VertexDeclaration VertexType
		{
			get { return this.vertexType; }
		}
		/// <summary>
		/// [GET] The number of vertices that are currently stored in this buffer.
		/// </summary>
		public int VertexCount
		{
			get { return this.vertexCount; }
		}

		/// <summary>
		/// [GET] A reference to the native backend storage for this buffers index data.
		/// </summary>
		public INativeGraphicsBuffer NativeIndex
		{
			get { return this.nativeIndex; }
		}
		/// <summary>
		/// [GET] The kind of index data that is stored in this buffer.
		/// </summary>
		public IndexDataElementType IndexType
		{
			get { return this.indexType; }
		}
		/// <summary>
		/// [GET] The number of indices that are currently stored in this buffer.
		/// Returns zero, if this buffer does not use any index data.
		/// </summary>
		public int IndexCount
		{
			get { return this.indexCount; }
		}


		/// <summary>
		/// Disposes this buffer and frees all of its native / unmanaged resources.
		/// </summary>
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

		/// <summary>
		/// Clears this buffer from all vertex data.
		/// </summary>
		public void ClearVertexData()
		{
			this.vertexCount = 0;
		}
		/// <summary>
		/// Uploads the specified vertices to the GPU.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="vertexData"></param>
		/// <param name="vertexCount"></param>
		public void LoadVertexData<T>(T[] vertexData, int vertexCount) where T : struct, IVertexData
		{
			this.LoadVertexData(
				VertexDeclaration.Get<T>(),
				vertexData,
				vertexCount);
		}
		/// <summary>
		/// Uploads the specified vertices to the GPU using a specific <see cref="VertexDeclaration"/>
		/// to describe the data layout that is used. Note that this overload allows to use any kind
		/// of struct array.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="vertexType"></param>
		/// <param name="vertexData"></param>
		/// <param name="vertexCount"></param>
		public void LoadVertexData<T>(VertexDeclaration vertexType, T[] vertexData, int vertexCount) where T : struct
		{
			this.EnsureNativeVertex();

			this.vertexCount = vertexCount;
			this.vertexType = vertexType;

			this.nativeVertex.LoadData(vertexData, vertexCount);
		}
		/// <summary>
		/// Uploads the specified vertices to the GPU using a specific <see cref="VertexDeclaration"/>
		/// to describe the data layout that is used.
		/// </summary>
		/// <param name="vertexType"></param>
		/// <param name="vertexData"></param>
		/// <param name="vertexCount"></param>
		public void LoadVertexData(VertexDeclaration vertexType, IntPtr vertexData, int vertexCount)
		{
			this.EnsureNativeVertex();

			this.vertexCount = vertexCount;
			this.vertexType = vertexType;

			this.nativeVertex.LoadData(vertexData, vertexCount * vertexType.Size);
		}

		/// <summary>
		/// Clears this buffer from all index data.
		/// 
		/// Index data is optional - if not specified, vertices will be rendered in order of definition.
		/// </summary>
		public void ClearIndexData()
		{
			this.indexCount = 0;
		}
		/// <summary>
		/// Uploads the specified indices to the GPU.
		/// 
		/// Index data is optional - if not specified, vertices will be rendered in order of definition.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="indexData"></param>
		/// <param name="indexCount"></param>
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
		/// <summary>
		/// Uploads the specified indices to the GPU using a specific <see cref="IndexDataElementType"/>
		/// to describe the data type that is used.
		/// 
		/// Index data is optional - if not specified, vertices will be rendered in order of definition.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="indexType"></param>
		/// <param name="indexData"></param>
		/// <param name="indexCount"></param>
		public void LoadIndexData<T>(IndexDataElementType indexType, T[] indexData, int indexCount) where T : struct
		{
			this.EnsureNativeIndex();

			this.indexCount = indexCount;
			this.indexType = indexType;

			this.nativeIndex.LoadData(indexData, indexCount);
		}
		/// <summary>
		/// Uploads the specified indices to the GPU using a specific <see cref="IndexDataElementType"/>
		/// to describe the data type that is used.
		/// 
		/// Index data is optional - if not specified, vertices will be rendered in order of definition.
		/// </summary>
		/// <param name="indexType"></param>
		/// <param name="indexData"></param>
		/// <param name="indexCount"></param>
		public void LoadIndexData(IndexDataElementType indexType, IntPtr indexData, int indexCount)
		{
			this.EnsureNativeIndex();

			this.indexCount = indexCount;
			this.indexType = indexType;

			int indexSize = 0;
			if      (indexType == IndexDataElementType.UnsignedByte)  indexSize = sizeof(byte);
			else if (indexType == IndexDataElementType.UnsignedShort) indexSize = sizeof(ushort);
			this.nativeIndex.LoadData(indexData, indexCount * indexSize);
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
