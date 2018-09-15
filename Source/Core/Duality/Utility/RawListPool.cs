using System;
using System.Collections.Generic;
using System.Linq;

using Duality.Resources;
using Duality.Cloning;

namespace Duality
{
	/// <summary>
	/// A simple pool of <see cref="RawList{T}"/> instances of a particular type.
	/// Intended to be used locally, by a single owner and from a single thread.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class RawListPool<T>
	{
		private List<RawList<T>> freeLists = new List<RawList<T>>();
		private List<RawList<T>> usedLists = new List<RawList<T>>();


		/// <summary>
		/// Rents a list instance with the specified min capacity.
		/// </summary>
		/// <param name="minCapacity"></param>
		/// <returns></returns>
		public RawList<T> Rent(int minCapacity)
		{
			// No pooled instances available? Create perfectly fitting one on-the-fly.
			if (this.freeLists.Count == 0)
			{
				RawList<T> list = new RawList<T>(minCapacity);
				this.usedLists.Add(list);
				return list;
			}

			// Determine the smallest fitting list regarding our desired capacity
			RawList<T> bestFit = null;
			int bestSizeDiff = int.MaxValue;
			for (int i = 0; i < this.freeLists.Count; i++)
			{
				int sizeDiff = this.freeLists[i].Capacity - minCapacity;
				if (sizeDiff >= 0 && sizeDiff < bestSizeDiff)
				{
					bestFit = this.freeLists[i];
					bestSizeDiff = sizeDiff;
					if (sizeDiff == 0) break;
				}
			}

			// No match found? Use the smallest available list.
			if (bestFit == null)
			{
				bestFit = this.freeLists[0];
				for (int i = 1; i < this.freeLists.Count; i++)
				{
					if (this.freeLists[i].Capacity < bestFit.Capacity)
					{
						bestFit = this.freeLists[i];
						if (bestFit.Capacity == 0) break;
					}
				}
			}

			// Make sure the list has the desired capacity
			bestFit.Reserve(minCapacity);

			// Flag list as used and return it
			this.freeLists.Remove(bestFit);
			this.usedLists.Add(bestFit);
			return bestFit;
		}
		/// <summary>
		/// Returns the specified list to the pool.
		/// </summary>
		/// <param name="list"></param>
		public void Return(RawList<T> list)
		{
			list.Clear();
			this.usedLists.Remove(list);
			this.freeLists.Add(list);
		}

		/// <summary>
		/// Flags all used list instances as unused again and clears their contents.
		/// </summary>
		public void Reset()
		{
			for (int i = 0; i < this.usedLists.Count; i++)
			{
				this.usedLists[i].Clear();
			}
			this.freeLists.AddRange(this.usedLists);
			this.usedLists.Clear();
		}
	}
}