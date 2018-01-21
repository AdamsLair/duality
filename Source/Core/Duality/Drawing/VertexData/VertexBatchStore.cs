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
		private struct StoredVertexData
		{
			public object Vertices;
			public List<IVertexBatch> Batches;
			public int ActiveBatchCount;
		}

		private int usedSlotCount = 0;
		private StoredVertexData[] vertexDataSlots = new StoredVertexData[4];


		/// <summary>
		/// Returns the number of used type index slots in current storage.
		/// Counting from zero to <see cref="TypeIndexCount"/> - 1 will cover all
		/// vertex type indices that may be present in this <see cref="VertexBatchStore"/>.
		/// </summary>
		public int TypeIndexCount
		{
			get { return this.usedSlotCount; }
		}


		/// <summary>
		/// Retrieves all stored batches of a vertex type and adds them
		/// to the specified list.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="batches"></param>
		public void GetBatches<T>(List<VertexBatch<T>> batches) where T : struct, IVertexData
		{
			int typeIndex = VertexDeclaration.Get<T>().TypeIndex;
			if (typeIndex >= this.usedSlotCount) return;
			if (this.vertexDataSlots[typeIndex].Batches == null) return;

			foreach (IVertexBatch batch in this.vertexDataSlots[typeIndex].Batches)
			{
				batches.Add((VertexBatch<T>)batch);
			}
		}
		/// <summary>
		/// Returns a read-only list reference containing all stored vertex batches
		/// that match the specified vertex type. Returns null, if that type is
		/// not represented in this <see cref="VertexBatchStore"/>.
		/// </summary>
		/// <param name="typeIndex"></param>
		/// <returns></returns>
		public IReadOnlyList<IVertexBatch> GetBatches<T>() where T : struct, IVertexData
		{
			int typeIndex = VertexDeclaration.Get<T>().TypeIndex;
			if (typeIndex >= this.usedSlotCount) return null;
			return this.vertexDataSlots[typeIndex].Batches;
		}
		/// <summary>
		/// Returns a read-only list reference containing all stored vertex batches
		/// that match the specified type index. Returns null, if that type index is
		/// not represented in this <see cref="VertexBatchStore"/>.
		/// </summary>
		/// <param name="typeIndex"></param>
		/// <returns></returns>
		public IReadOnlyList<IVertexBatch> GetBatches(int typeIndex)
		{
			if (typeIndex < 0) return null;
			if (typeIndex >= this.usedSlotCount) return null;
			return this.vertexDataSlots[typeIndex].Batches;
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
			// Clear all vertex memory and reset counters, but keep it all around to avoid allocations
			for (int i = 0; i < this.vertexDataSlots.Length; i++)
			{
				if (this.vertexDataSlots[i].Batches != null)
				{
					foreach (IVertexBatch batch in this.vertexDataSlots[i].Batches)
					{
						batch.Clear();
					}
				}
				this.vertexDataSlots[i].ActiveBatchCount = 0;
				this.vertexDataSlots[i].Vertices = null;
			}
			this.usedSlotCount = 0;
		}

		private RawList<T> GetVertexList<T>() where T : struct, IVertexData
		{
			// Fast path: Retrieve and return
			int typeIndex = VertexDeclaration.Get<T>().TypeIndex;
			RawList<T> vertexList = null;
			if (typeIndex < this.vertexDataSlots.Length && (vertexList = this.vertexDataSlots[typeIndex].Vertices as RawList<T>) != null)
			{
				return vertexList;
			}
			// Slow / init path
			else
			{
				// Allocate slot for vertex type
				if (typeIndex >= this.vertexDataSlots.Length)
				{
					Array.Resize(ref this.vertexDataSlots, typeIndex + 1);
				}
				this.usedSlotCount = Math.Max(this.usedSlotCount, typeIndex + 1);

				// Init batch storage for vertex type
				if (this.vertexDataSlots[typeIndex].Batches == null)
				{
					this.vertexDataSlots[typeIndex].Batches = new List<IVertexBatch>();
					this.vertexDataSlots[typeIndex].Batches.Add(new VertexBatch<T>());
				}

				// Assign current vertex batch for fast access
				VertexBatch<T> batch = (VertexBatch<T>)this.vertexDataSlots[typeIndex].Batches[0];
				this.vertexDataSlots[typeIndex].Vertices = batch.Vertices;
				this.vertexDataSlots[typeIndex].ActiveBatchCount = 1;
				return batch.Vertices;
			}
		}
	}
}
