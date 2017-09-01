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
		private IVertexBatch[] verticesPerType = new IVertexBatch[4];

		/// <summary>
		/// [GET] An list of stored vertex arrays where each one is located at
		/// the type index of its matching <see cref="VertexDeclaration"/>. May contain
		/// null at indices where no vertex data of that type has been stored.
		/// </summary>
		public IReadOnlyList<IVertexBatch> VerticesByType
		{
			get { return this.verticesPerType; }
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
			IVertexBatch array = this.GetArray<T>();
			RawList<T> typedArray = array.GetTypedData<T>();
			typedArray.Count += length;
			return new VertexSlice<T>(
				typedArray.Data, 
				typedArray.Count - length, 
				length);
		}
		/// <summary>
		/// Clears all previously rented slices and resets all data that was written to
		/// any of them before.
		/// </summary>
		public void Clear()
		{
			for (int i = 0; i < this.verticesPerType.Length; i++)
			{
				if (this.verticesPerType[i] == null) continue;
				this.verticesPerType[i].Clear();
			}
		}

		private IVertexBatch GetArray<T>() where T : struct, IVertexData
		{
			int typeIndex = VertexDeclaration.Get<T>().TypeIndex;
			IVertexBatch array = null;
			if (typeIndex >= this.verticesPerType.Length || (array = this.verticesPerType[typeIndex]) == null)
			{
				array = new VertexBatch<T>();
				this.AssignArray(typeIndex, array);
			}
			return array;
		}
		private void AssignArray(int typeIndex, IVertexBatch array)
		{
			if (this.verticesPerType.Length <= typeIndex)
			{
				Array.Resize(ref this.verticesPerType, typeIndex + 1);
			}
			this.verticesPerType[typeIndex] = array;
		}
	}
}
