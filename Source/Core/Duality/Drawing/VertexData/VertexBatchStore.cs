using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Duality.Drawing
{
	/// <summary>
	/// Manages and provides CPU-side storage space for dynamically gathered vertex data.
	/// </summary>
	[DontSerialize]
	public class VertexBatchStore
	{
		private IVertexBatch[] batches = new IVertexBatch[4];
		private object[] vertices = new object[4];

		/// <summary>
		/// [GET] An list of stored vertex batches where each one is located at
		/// the type index of its matching <see cref="VertexDeclaration"/>. May contain
		/// null at indices where no vertex data of that type has been stored.
		/// </summary>
		public IReadOnlyList<IVertexBatch> Batches
		{
			get { return this.batches; }
		}

		/// <summary>
		/// Retrieves the stored vertex batch that matches the vertex type as specified
		/// by the generic type parameter.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public VertexBatch<T> GetBatch<T>() where T : struct, IVertexData
		{
			int typeIndex = VertexDeclaration.Get<T>().TypeIndex;
			if (typeIndex >= this.batches.Length) return null;
			return this.batches[typeIndex] as VertexBatch<T>;
		}
		/// <summary>
		/// Rents a slice of the specified length in an appropriately typed vertex array,
		/// allowing to write vertex data into it.
		/// 
		/// Ownership remains in <see cref="VertexBatchStore"/> and there is no tracking of
		/// rented slices. Invoke <see cref="Clear"/> to reset all data and start over with
		/// no slices in use.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="length"></param>
		/// <returns></returns>
		public VertexSlice<T> Rent<T>(int length) where T : struct, IVertexData
		{
			RawList<T> vertexList = this.GetVertexList<T>();
			vertexList.Count += length;
			return new VertexSlice<T>(
				vertexList.Data, 
				vertexList.Count - length, 
				length);
		}
		/// <summary>
		/// Clears all previously rented slices and resets all data that was written to
		/// any of them before.
		/// </summary>
		public void Clear()
		{
			for (int i = 0; i < this.batches.Length; i++)
			{
				if (this.batches[i] == null) continue;
				this.batches[i].Clear();
			}
		}

		private RawList<T> GetVertexList<T>() where T : struct, IVertexData
		{
			// Fast path: Retrieve and return
			int typeIndex = VertexDeclaration.Get<T>().TypeIndex;
			RawList<T> vertexList = null;
			if (typeIndex < this.vertices.Length && (vertexList = this.vertices[typeIndex] as RawList<T>) != null)
			{
				return vertexList;
			}
			// Slow / init path
			else
			{
				if (typeIndex >= this.vertices.Length)
				{
					Array.Resize(ref this.batches, typeIndex + 1);
					Array.Resize(ref this.vertices, typeIndex + 1);
				}
				VertexBatch<T> batch = new VertexBatch<T>();
				this.batches[typeIndex] = batch;
				this.vertices[typeIndex] = batch.Vertices;
				return batch.Vertices;
			}
		}
	}
}
