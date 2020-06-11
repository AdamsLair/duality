﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Reflection;

namespace Duality
{
	/// <summary>
	/// Encapsulates an Array and provides methods for dynamically modifying it similar to a List{T}, but allows
	/// accessing its raw internal data at the same time. This can be useful for situations wherer raw data access
	/// may significantly improve performance, but dynamic sizes are still required.
	/// </summary>
	/// <remarks>
	/// Use this class with caution and consideration. In almost all cases, either a List{T} or a regular Array should be preferred.
	/// You should only use this class when you know how to use it.
	/// </remarks>
	/// <typeparam name="T"></typeparam>
	[DebuggerTypeProxy(typeof(RawList<>.DebuggerTypeProxy))]
	[DebuggerDisplay("Count = {Count}, Capacity = {Capacity}")]
	public class RawList<T> : IList<T>, IList, IReadOnlyList<T>
	{
		private const int BaseCapacity = 8;


		private T[] data;
		private int count;


		/// <summary>
		/// [GET / SET] The lists internal array for data storage. Assigning an array that is shorter than <see cref="Count"/> will
		/// result in <see cref="Count"/> being automatically adjusted. Assigning a larger array will not affect <see cref="Count"/>.
		/// Assigning null will internally create a zero-length array.
		/// </summary>
		public T[] Data
		{
			get { return this.data; }
			set
			{
				this.data = value ?? new T[0];
				this.count = MathF.Clamp(this.count, 0, this.data.Length);
			}
		}
		/// <summary>
		/// [GET / SET] The number of used indices within the internal <see cref="Data"/> array. Setting this to a higher value
		/// may cause the list to grow its internal array. Decreasing this value does not reset "removed" elements of the internal
		/// array to their default value.
		/// </summary>
		public int Count
		{
			get { return this.count; }
			set
			{
				this.count = Math.Max(value, 0);
				this.Reserve(this.count);
			}
		}
		/// <summary>
		/// [GET] Length of the internal <see cref="Data"/> array.
		/// </summary>
		public int Capacity
		{
			get { return this.data.Length; }
		}

		/// <summary>
		/// [GET / SET] A safety-checked by-ref index accessor to the lists internal array. Will throw an <see cref="System.IndexOutOfRangeException"/>
		/// when attempting to access indices exceeding <see cref="Count"/>.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public ref T this[int index]
		{
			get
			{
				if (index >= this.count) ThrowIndexOutOfRangeException();
				return ref this.data[index];
			}
		}


		/// <summary>
		/// Creates a new, empty list.
		/// </summary>
		public RawList() : this(new T[BaseCapacity], 0) { }
		/// <summary>
		/// Creates a new list with the specified capacity.
		/// </summary>
		/// <param name="capacity"></param>
		public RawList(int capacity) : this(new T[capacity], 0) { }
		/// <summary>
		/// Creates a new list that is a copy of the specified source list.
		/// </summary>
		/// <param name="source"></param>
		public RawList(RawList<T> source) : this((T[])source.Data.Clone(), source.Count) { }
		/// <summary>
		/// Creates a new list that wraps the specified array. Does not copy the array.
		/// </summary>
		/// <param name="wrapAround"></param>
		public RawList(T[] wrapAround) : this(wrapAround, wrapAround.Length) { }
		/// <summary>
		/// Creates a new list that wraps the specified array. Does not copy the array.
		/// </summary>
		/// <param name="wrapAround"></param>
		/// <param name="count"></param>
		public RawList(T[] wrapAround, int count)
		{
			this.data = wrapAround;
			this.count = Math.Max(count, 0);
			this.Reserve(this.count);
		}


		/// <summary>
		/// Returns the first index of the specified item within the used range of the internal array. Returns -1, if not found.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public int IndexOf(T item)
		{
			return Array.IndexOf(this.data, item, 0, this.count);
		}
		/// <summary>
		/// Returns whether the specified item is contained within the used range of the internal array.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public bool Contains(T item)
		{
			return Array.IndexOf(this.data, item, 0, this.count) >= 0;
		}

		/// <summary>
		/// Adds a new item to the list.
		/// </summary>
		/// <param name="item"></param>
		public void Add(T item)
		{
			this.Reserve(this.count + 1);
			this.data[this.count] = item;
			this.count++;
		}
		/// <summary>
		/// Adds a range of new items to the list.
		/// </summary>
		/// <param name="items"></param>
		public void AddRange(T[] items)
		{
			this.InsertRange(this.count, items);
		}

		/// <summary>
		/// Inserts a new item at a specified index.
		/// </summary>
		/// <param name="targetIndex">The index at which to insert the new items.</param>
		/// <param name="item">The source item to insert.</param>
		public void Insert(int targetIndex, T item)
		{
			if (targetIndex < 0 || targetIndex > this.count) throw new IndexOutOfRangeException("Parameter 'targetIndex' is out of range.");
			this.Reserve(this.count + 1);
			if (targetIndex < this.count)
			{
				this.MoveInternal(targetIndex, this.count - targetIndex, 1, false);
			}
			this.data[targetIndex] = item;
			this.count++;
		}
		/// <summary>
		/// Inserts a range of new items at a specified index.
		/// </summary>
		/// <param name="targetIndex">The index at which to insert the new items.</param>
		/// <param name="items">The source array to copy items from.</param>
		public void InsertRange(int targetIndex, T[] items)
		{
			this.InsertRange(targetIndex, items, 0, items.Length);
		}
		/// <summary>
		/// Inserts a range of new items at a specified index.
		/// </summary>
		/// <param name="targetIndex">The index at which to insert the new items.</param>
		/// <param name="items">The source array to copy items from.</param>
		/// <param name="sourceIndex">Index in the source array from which to copy items.</param>
		/// <param name="count">The number of items to insert.</param>
		public void InsertRange(int targetIndex, T[] items, int sourceIndex, int count)
		{
			if (targetIndex < 0 || targetIndex > this.count) throw new IndexOutOfRangeException("Parameter 'targetIndex' is out of range.");
			this.Reserve(this.count + count);
			if (targetIndex < this.count)
			{
				this.MoveInternal(targetIndex, this.count - targetIndex, count, false);
			}
			Array.Copy(items, sourceIndex, this.data, targetIndex, count);
			this.count += count;
		}

		/// <summary>
		/// Removes the first matching item from the list.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public bool Remove(T item)
		{
			int index = this.IndexOf(item);
			if (index >= 0)
			{
				this.RemoveAt(index);
				return true;
			}
			else
			{
				return false;
			}
		}
		/// <summary>
		/// Removes the last item from the list. For primitives and unmanaged value types, this is a trivial
		/// <see cref="Count"/> value change.
		/// </summary>
		public void RemoveLast()
		{
			this.count -= 1;
			if (ReflectionHelper.IsReferenceOrContainsReferences<T>())
			{
				this.data[this.count] = default(T);
			}
		}
		/// <summary>
		/// Removes the last <paramref name="count"/> items from the list. For primitives and unmanaged value 
		/// types, this is a trivial <see cref="Count"/> value change.
		/// </summary>
		/// <param name="count"></param>
		public void RemoveLast(int count)
		{
			this.count -= count;
			if (ReflectionHelper.IsReferenceOrContainsReferences<T>())
			{
				Array.Clear(this.data, this.count, count);
			}
		}
		/// <summary>
		/// Removes the element at the specified index.
		/// </summary>
		/// <param name="index"></param>
		public void RemoveAt(int index)
		{
			this.RemoveRange(index, 1);
		}
		/// <summary>
		/// Removes a range of elements at the specified index.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="count"></param>
		public void RemoveRange(int index, int count)
		{
			// Fast path when items are removed all the way up to the end
			if (index + count >= this.count)
			{
				this.RemoveLast(count);
			}
			// General case where items will have to be moved in order to keep ordering as-is
			else
			{
				this.MoveInternal(index + count, this.count - (index + count), -count, false);
				this.count -= count;
				if (ReflectionHelper.IsReferenceOrContainsReferences<T>())
				{
					Array.Clear(this.data, this.count, count);
				}
			}
		}
		/// <summary>
		/// Removes the element at the specified index by overwriting it with the last element.
		/// 
		/// For large lists, removing items this way can be considerably faster than using the
		/// <see cref="RemoveAt(int)"/> method, since no elements need to be moved around to
		/// preserve list ordering.
		/// </summary>
		/// <param name="index"></param>
		public void RemoveAtFast(int index)
		{
			this.count--;
			this.data[index] = this.data[this.count];
			if (ReflectionHelper.IsReferenceOrContainsReferences<T>())
			{
				this.data[this.count] = default(T);
			}
		}
		/// <summary>
		/// Removes a range of elements at the specified index by overwriting them with elements
		/// taken from the back of the list. The exact order in which the elements of the removed 
		/// range are overwritten is not defined.
		/// 
		/// For large lists, removing items this way can be considerably faster than using the
		/// <see cref="RemoveRange(int, int)"/> method, since no elements need to be moved around
		/// to preserve list ordering.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="count"></param>
		public void RemoveRangeFast(int index, int count)
		{
			// Fast path when items are removed all the way up to the end
			if (index + count >= this.count)
			{
				this.RemoveLast(count);
			}
			// General case where items will be overwritten with items from the end
			else
			{
				for (int i = 0; i < count; i++)
				{
					this.data[index + i] = this.data[this.count - i - 1];
				}
				this.count -= count;
				if (ReflectionHelper.IsReferenceOrContainsReferences<T>())
				{
					Array.Clear(this.data, this.count, count);
				}
			}
		}
		/// <summary>
		/// Removes all matching items from the list.
		/// </summary>
		/// <param name="predicate"></param>
		/// <returns></returns>
		public int RemoveAll(Predicate<T> predicate)
		{
			// Iterate over the internal array with two indices:
			// A source index that skips all removed items and a
			// target index that assigns new positions to all remaining
			// items.
			int source = -1;
			int newCount = -1;
			for (int target = 0; target < this.count; target++)
			{
				// Determine the next source index to copy from
				do
				{
					source++;
					if (source >= this.count)
					{
						newCount = target;
						break;
					}
				}
				while (predicate(this.data[source]));

				// If we reached the end, stop
				if (newCount != -1) break;

				// Copy objects to their new indices
				if (target != source)
				{
					this.data[target] = this.data[source];
				}
			}

			// Shrink the list to match the new number of elements
			if (newCount != this.count && newCount != -1)
			{
				int removedCount = this.count - newCount;
				this.count -= removedCount;
				if (ReflectionHelper.IsReferenceOrContainsReferences<T>())
				{
					Array.Clear(this.data, this.count, removedCount);
				}
				return removedCount;
			}

			return 0;
		}

		/// <summary>
		/// Clears the entire list of its contents and resets its size to zero.
		/// </summary>
		public void Clear()
		{
			if (ReflectionHelper.IsReferenceOrContainsReferences<T>())
			{
				Array.Clear(this.data, 0, this.count);
			}
			this.count = 0;
		}

		/// <summary>
		/// Sorts the entire list.
		/// </summary>
		public void Sort()
		{
			this.Sort(0, this.count, Comparer<T>.Default);
		}
		/// <summary>
		/// Sorts the entire list using a specific comparer.
		/// </summary>
		/// <param name="comparer"></param>
		public void Sort(Comparer<T> comparer)
		{
			this.Sort(0, this.count, comparer);
		}
		/// <summary>
		/// Sorts the entire list using a specific comparison.
		/// </summary>
		/// <param name="comparison"></param>
		public void Sort(Comparison<T> comparison)
		{
			this.Sort(0, this.count, comparison);
		}
		/// <summary>
		/// Sorts a certain range of the list.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="count"></param>
		public void Sort(int index, int count)
		{
			this.Sort(index, count, Comparer<T>.Default);
		}
		/// <summary>
		/// Sorts a certain range of the list using a specific comparer.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="count"></param>
		/// <param name="comparer"></param>
		public void Sort(int index, int count, IComparer<T> comparer)
		{
			if (count == 0) return;
			
			if (count < 0)                        throw new ArgumentException("Parameter 'count' may not be negative.", "count");
			if (index < 0 || index >= this.count) throw new IndexOutOfRangeException("Parameter 'index' is out of range.");
			if (index + count > this.count)       throw new IndexOutOfRangeException("'index + count' is out of range.");

			Array.Sort(this.data, index, count, comparer);
		}
		/// <summary>
		/// Sorts a certain range of the list using a specific comparison.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="count"></param>
		/// <param name="comparison"></param>
		public void Sort(int index, int count, Comparison<T> comparison)
		{
			if (count == 0) return;

			if (count < 0)                        throw new ArgumentException("Parameter 'count' may not be negative.", "count");
			if (index < 0 || index >= this.count) throw new IndexOutOfRangeException("Parameter 'index' is out of range.");
			if (index + count > this.count)       throw new IndexOutOfRangeException("'index + count' is out of range.");

			Array.Sort(this.data, index, count, Comparer<T>.Create(comparison));
		}

		/// <summary>
		/// Shrinks the lists internal array to the current minimum size.
		/// </summary>
		public void ShrinkToFit()
		{
			if (this.data.Length == this.count) return;
			Array.Resize(ref this.data, this.count);
		}
		/// <summary>
		/// Makes sure that the lists internal array has storage space for at least the specified amount of elements.
		/// </summary>
		/// <param name="capacity"></param>
		public void Reserve(int capacity)
		{
			if (this.data.Length >= capacity) return;
			Array.Resize(ref this.data, Math.Max(Math.Max(this.data.Length * 2, capacity), BaseCapacity));
		}
		/// <summary>
		/// Moves a range of elements by a certain value and resets the indices that now remain empty.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="count"></param>
		/// <param name="moveBy"></param>
		public void Move(int index, int count, int moveBy)
		{
			this.MoveInternal(index, count, moveBy, true);
		}

		/// <summary>
		/// Copies the contents of this collection to the specified array.
		/// </summary>
		/// <param name="array"></param>
		/// <param name="arrayIndex"></param>
		public void CopyTo(T[] array, int arrayIndex)
		{
			Array.Copy(this.data, 0, array, arrayIndex, this.count);
		}
		/// <summary>
		/// Copies the contents of this list to the specified target list. The target list size will be
		/// increased when necessary to fit all copied contents of the source list inside it at the specified index.
		/// </summary>
		/// <param name="target"></param>
		/// <param name="targetIndex"></param>
		/// <param name="count"></param>
		public void CopyTo(RawList<T> target, int targetIndex, int count)
		{
			if (target == null) throw new ArgumentNullException("target");
			if (targetIndex < 0) throw new ArgumentException("The target index may not be negative.", "targetIndex");
			if (count > this.count) throw new ArgumentException(string.Format("Attempting to copy {0} elements, even though the source list only contains {1}.", count, this.count), "count");

			// Make sure the target list is able to hold all data
			target.Count = Math.Max(target.Count, targetIndex + count);

			// Copy data from source to target
			Array.Copy(this.data, 0, target.data, targetIndex, count);
		}
		public IEnumerator<T> GetEnumerator()
		{
			for (int i = 0; i < this.count; i++)
			{
				yield return this.data[i];
			}
		}


		private void MoveInternal(int index, int count, int moveBy, bool resetToDefault)
		{
			if (count < 0)                                 throw new ArgumentException("Parameter 'count' may not be negative.", "count");
			if (index < 0 || index >= this.data.Length)    throw new IndexOutOfRangeException("Parameter 'index' is out of range.");
			if (index + count > this.data.Length)          throw new IndexOutOfRangeException("'index + count' is out of range.");
			if (index + moveBy < 0)                        throw new IndexOutOfRangeException("'index + moveBy' is out of range.");
			if (index + moveBy + count > this.data.Length) throw new IndexOutOfRangeException("'index + moveBy + count' is out of range.");

			int baseIndex = index + moveBy;
			if (moveBy > 0)
			{
				// Non-overlapping move
				if (moveBy >= count)
				{
					Array.Copy(this.data, index, this.data, index + moveBy, count);
				}
				// Source and target range within the array are overlapping
				else
				{
					for (int i = baseIndex + count - 1; i >= baseIndex; i--)
					{
						this.data[i] = this.data[i - moveBy];
					}
				}

				if (resetToDefault)
				{
					int clearCount = Math.Min(moveBy, count);
					Array.Clear(this.data, index, clearCount);
				}
			}
			else
			{
				// Non-overlapping move
				if (-moveBy >= count)
				{
					Array.Copy(this.data, index, this.data, index + moveBy, count);
				}
				// Source and target range within the array are overlapping
				else
				{
					for (int i = baseIndex; i < baseIndex + count; i++)
					{
						this.data[i] = this.data[i - moveBy];
					}
				}

				if (resetToDefault)
				{
					int clearCount = Math.Min(-moveBy, count);
					Array.Clear(this.data, index + count - clearCount, clearCount);
				}
			}
		}

		private static void ThrowIndexOutOfRangeException()
		{
			throw new IndexOutOfRangeException();
		}


		#region Explicit Interfaces
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] bool ICollection<T>.IsReadOnly
		{
			get { return false; }
		}
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] bool IList.IsFixedSize
		{
			get { return false; }
		}
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] bool IList.IsReadOnly
		{
			get { return false; }
		}
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] bool ICollection.IsSynchronized
		{
			get { return false; }
		}
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] object ICollection.SyncRoot
		{
			get { return this.data; }
		}

		object IList.this[int index]
		{
			get { return this[index]; }
			set { this[index] = (T)value; }
		}
		T IList<T>.this[int index]
		{
			get
			{
				if (index >= this.count) ThrowIndexOutOfRangeException();
				return this.data[index];
			}
			set
			{
				if (index >= this.count) ThrowIndexOutOfRangeException();
				this.data[index] = value;
			}
		}
		T IReadOnlyList<T>.this[int index]
		{
			get
			{
				if (index >= this.count) ThrowIndexOutOfRangeException();
				return this.data[index];
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
		int IList.Add(object value)
		{
			this.Add((T)value);
			return this.count - 1;
		}
		bool IList.Contains(object value)
		{
			return this.Contains((T)value);
		}
		int IList.IndexOf(object value)
		{
			return this.IndexOf((T)value);
		}
		void IList.Insert(int index, object value)
		{
			this.Insert(index, (T)value);
		}
		void IList.Remove(object value)
		{
			this.Remove((T)value);
		}
		void ICollection.CopyTo(Array array, int index)
		{
			this.CopyTo((T[])array, index);
		}
		#endregion


		internal sealed class DebuggerTypeProxy
		{
			private RawList<T> rawList;

			public DebuggerTypeProxy(RawList<T> rawList)
			{
				if (rawList == null) throw new ArgumentNullException("rawList");
				this.rawList = rawList;
			}

			[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
			public T[] Items
			{
				get
				{
					T[] array = new T[this.rawList.Count];
					this.rawList.CopyTo(array, 0);
					return array;
				}
			}
		}
	}
}
