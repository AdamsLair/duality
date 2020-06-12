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
		/// Clears this buffer from all previous vertex data and re-initializes it
		/// with the specified number of vertices.
		/// </summary>
		public void SetupVertexData<T>(int count) where T : struct, IVertexData
		{
			this.EnsureNativeVertex();

			this.vertexCount = count;
			this.vertexType = VertexDeclaration.Get<T>();

			this.nativeVertex.SetupEmpty<T>(count);
		}
		/// <summary>
		/// Clears this buffer from all previous vertex data and re-initializes it
		/// with the specified number of vertices.
		/// </summary>
		public void SetupVertexData(VertexDeclaration vertexType, int count)
		{
			this.EnsureNativeVertex();

			this.vertexCount = count;
			this.vertexType = vertexType;

			this.nativeVertex.SetupEmpty(count * vertexType.Size);
		}

		/// <summary>
		/// Uploads the specified vertices to this buffer, replacing all previous contents.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="data"></param>
		/// <param name="index"></param>
		/// <param name="count"></param>
		public void LoadVertexData<T>(T[] data, int index, int count) where T : struct, IVertexData
		{
			this.EnsureNativeVertex();

			this.vertexCount = count;
			this.vertexType = VertexDeclaration.Get<T>();

			this.nativeVertex.LoadData(data, index, count);
		}
		/// <summary>
		/// Uploads the specified vertices to this buffer, replacing all previous contents and 
		/// using a specific <see cref="VertexDeclaration"/> to describe the data layout that is used.
		/// This method is unsafe. Where possible, prefer the overload that accepts a managed array.
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
		/// Uploads the specified vertices into a subsection of this buffer, keeping all other content.
		/// This method changes neither size nor vertex type associated with this buffer.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="bufferIndex">Offset in this buffer to which the new data will be copied, as number of elements.</param>
		/// <param name="data"></param>
		/// <param name="index"></param>
		/// <param name="count"></param>
		public void LoadVertexSubData<T>(int bufferIndex, T[] data, int index, int count) where T : struct, IVertexData
		{
			this.EnsureNativeVertex();
			this.nativeVertex.LoadSubData(bufferIndex, data, index, count);
		}
		/// <summary>
		/// Uploads the specified vertices into a subsection of this buffer, keeping all other content.
		/// This method changes neither size nor vertex type associated with this buffer.
		/// This method is unsafe. Where possible, prefer the overload that accepts a managed array.
		/// </summary>
		/// <param name="vertexType"></param>
		/// <param name="bufferIndex">Offset in this buffer to which the new data will be copied, as number of elements.</param>
		/// <param name="vertexData"></param>
		/// <param name="vertexCount"></param>
		public void LoadVertexSubData(VertexDeclaration vertexType, int bufferIndex, IntPtr vertexData, int vertexCount)
		{
			this.EnsureNativeVertex();
			this.nativeVertex.LoadSubData(
				IntPtr.Add(IntPtr.Zero, bufferIndex * vertexType.Size), 
				vertexData, 
				vertexCount);
		}

		/// <summary>
		/// Clears this buffer from all previous index data and re-initializes it
		/// with the specified number of index elements.
		/// 
		/// Index data is optional - if not specified, vertices will be rendered in order of definition.
		/// </summary>
		public void SetupIndexData<T>(int count) where T : struct
		{
			IndexDataElementType elementType = GetIndexElementType<T>();

			this.EnsureNativeVertex();

			this.indexCount = count;
			this.indexType = elementType;

			this.nativeVertex.SetupEmpty<T>(count);
		}
		/// <summary>
		/// Clears this buffer from all previous index data and re-initializes it
		/// with the specified number of index elements.
		/// 
		/// Index data is optional - if not specified, vertices will be rendered in order of definition.
		/// </summary>
		public void SetupIndexData(IndexDataElementType indexType, int count)
		{
			this.EnsureNativeVertex();

			this.indexCount = count;
			this.indexType = indexType;

			int indexSize = GetIndexElementSize(indexType);
			this.nativeVertex.SetupEmpty(count * indexSize);
		}

		/// <summary>
		/// Uploads the specified indices to the GPU.
		/// 
		/// Index data is optional - if not specified, vertices will be rendered in order of definition.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="data"></param>
		/// <param name="index"></param>
		/// <param name="count"></param>
		public void LoadIndexData<T>(T[] data, int index, int count) where T : struct
		{
			IndexDataElementType elementType = GetIndexElementType<T>();

			this.EnsureNativeIndex();

			this.indexCount = count;
			this.indexType = elementType;

			this.nativeIndex.LoadData(data, index, count);
		}
		/// <summary>
		/// Uploads the specified indices to the GPU using a specific <see cref="IndexDataElementType"/>
		/// to describe the data type that is used.
		/// This method is unsafe. Where possible, prefer the overload that accepts a managed array.
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

			int indexSize = GetIndexElementSize(indexType);
			this.nativeIndex.LoadData(indexData, indexCount * indexSize);
		}

		/// <summary>
		/// Uploads the specified indices into a subsection of this buffer, keeping all other content.
		/// This method changes neither size nor index element type associated with this buffer.
		/// 
		/// Index data is optional - if not specified, vertices will be rendered in order of definition.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="bufferIndex">Offset in this buffer to which the new data will be copied, as number of elements.</param>
		/// <param name="data"></param>
		/// <param name="index"></param>
		/// <param name="count"></param>
		public void LoadIndexSubData<T>(int bufferIndex, T[] data, int index, int count) where T : struct
		{
			this.EnsureNativeIndex();
			this.nativeIndex.LoadSubData(bufferIndex, data, index, count);
		}
		/// <summary>
		/// Uploads the specified indices into a subsection of this buffer, keeping all other content.
		/// This method changes neither size nor index element type associated with this buffer.
		/// This method is unsafe. Where possible, prefer the overload that accepts a managed array.
		/// 
		/// Index data is optional - if not specified, vertices will be rendered in order of definition.
		/// </summary>
		/// <param name="bufferIndex">Offset in this buffer to which the new data will be copied, as number of elements.</param>
		/// <param name="indexType"></param>
		/// <param name="indexData"></param>
		/// <param name="indexCount"></param>
		public void LoadIndexSubData(IndexDataElementType indexType, int bufferIndex, IntPtr indexData, int indexCount)
		{
			this.EnsureNativeIndex();

			int indexSize = GetIndexElementSize(indexType);
			this.nativeIndex.LoadSubData(
				IntPtr.Add(IntPtr.Zero, bufferIndex * indexSize),
				indexData,
				indexCount);
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

		private static IndexDataElementType GetIndexElementType<T>() where T : struct
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
			return elementType;
		}
		private static int GetIndexElementSize(IndexDataElementType indexType)
		{
			int indexSize = 0;
			if      (indexType == IndexDataElementType.UnsignedByte)  indexSize = sizeof(byte);
			else if (indexType == IndexDataElementType.UnsignedShort) indexSize = sizeof(ushort);
			return indexSize;
		}
	}
}
