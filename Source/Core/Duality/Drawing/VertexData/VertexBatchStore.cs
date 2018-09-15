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

		private int maxBatchSize = int.MaxValue;
		private int usedSlotCount = 0;
		private StoredVertexData[] vertexDataSlots = new StoredVertexData[4];


		/// <summary>
		/// [GET] The maximum number of vertices that will be stored inside a single
		/// vertex batch. A new batch will be started when adding more vertices would
		/// otherwise exceed this number.
		/// </summary>
		public int MaxBatchSize
		{
			get { return this.maxBatchSize; }
		}
		/// <summary>
		/// [GET] Returns the number of used type index slots in current storage.
		/// Counting from zero to <see cref="TypeIndexCount"/> - 1 will cover all
		/// vertex type indices that may be present in this <see cref="VertexBatchStore"/>.
		/// </summary>
		public int TypeIndexCount
		{
			get { return this.usedSlotCount; }
		}


		public VertexBatchStore() { }
		public VertexBatchStore(int maxBatchSize) : this()
		{
			this.maxBatchSize = maxBatchSize;
		}

		
		/// <summary>
		/// Returns the number of active vertex batches in storage for the specified
		/// vertex type.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public int GetBatchCount<T>() where T : struct, IVertexData
		{
			int typeIndex = VertexDeclaration.Get<T>().TypeIndex;
			if (typeIndex >= this.vertexDataSlots.Length) return 0;
			return this.vertexDataSlots[typeIndex].ActiveBatchCount;
		}
		/// <summary>
		/// Returns the stored vertex batch for the specified batch index and vertex type.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="batchIndex"></param>
		/// <returns></returns>
		public VertexBatch<T> GetBatch<T>(int batchIndex) where T : struct, IVertexData
		{
			int typeIndex = VertexDeclaration.Get<T>().TypeIndex;
			if (typeIndex >= this.vertexDataSlots.Length) return null;
			return (VertexBatch<T>)this.vertexDataSlots[typeIndex].Batches[batchIndex];
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
			if (typeIndex >= this.vertexDataSlots.Length) return null;
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

			if (vertexList.Count + length > this.maxBatchSize)
			{
				if (length > this.maxBatchSize) this.ThrowMaxSizeExceeded(length);
				vertexList = this.AdvanceToNextBatch<T>();
			}

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
		private RawList<T> AdvanceToNextBatch<T>() where T: struct, IVertexData
		{
			int typeIndex = VertexDeclaration.Get<T>().TypeIndex;
			int batchIndex = this.vertexDataSlots[typeIndex].ActiveBatchCount;
			List<IVertexBatch> batchList = this.vertexDataSlots[typeIndex].Batches;

			// Create a new batch, if we didn't already have one ready
			if (batchList.Count <= batchIndex)
				batchList.Add(new VertexBatch<T>());

			// Assign current vertex batch for fast access
			VertexBatch<T> batch = (VertexBatch<T>)batchList[batchIndex];
			this.vertexDataSlots[typeIndex].Vertices = batch.Vertices;
			this.vertexDataSlots[typeIndex].ActiveBatchCount++;
			return batch.Vertices;
		}

		private void ThrowMaxSizeExceeded(int requestedLength)
		{
			throw new ArgumentException(string.Format(
				"This vertex batch storage limits batch size to a maximum of {0} vertices. " +
				"Cannot provide a vertex slice that is {1} vertices long.",
				this.maxBatchSize,
				requestedLength));
		}
	}
}
