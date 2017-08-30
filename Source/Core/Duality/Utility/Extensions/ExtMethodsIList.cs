using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Duality
{
	/// <summary>
	/// Provides extension methods for lists.
	/// </summary>
	public static class ExtMethodsIList
	{
		/// <summary>
		/// Performs a stable sort.
		/// </summary>
		/// <typeparam name="T">The lists object type.</typeparam>
		/// <param name="list">List to perform the sort operation on.</param>
		public static void StableSort<T>(this IList<T> list)
		{
			StableSort<T>(list, Comparer<T>.Default);
		}
		/// <summary>
		/// Performs a stable sort.
		/// </summary>
		/// <typeparam name="T">The lists object type.</typeparam>
		/// <param name="list">List to perform the sort operation on.</param>
		/// <param name="comparer">The comparer to use.</param>
		public static void StableSort<T>(this IList<T> list, IComparer<T> comparer)
		{
			StableSort<T>(list, comparer.Compare);
		}
		/// <summary>
		/// Performs a stable sort.
		/// </summary>
		/// <typeparam name="T">The lists object type.</typeparam>
		/// <param name="list">List to perform the sort operation on.</param>
		/// <param name="comparison">The comparison to use.</param>
		public static void StableSort<T>(this IList<T> list, Comparison<T> comparison)
		{
			StableSort<T>(list, 0, list.Count, comparison);
		}
		/// <summary>
		/// Performs a stable sort.
		/// </summary>
		/// <typeparam name="T">The lists object type.</typeparam>
		/// <param name="list">List to perform the sort operation on.</param>
		/// <param name="index"></param>
		/// <param name="count"></param>
		public static void StableSort<T>(this IList<T> list, int index, int count)
		{
			StableSort<T>(list, index, count, Comparer<T>.Default.Compare);
		}
		/// <summary>
		/// Performs a stable sort.
		/// </summary>
		/// <typeparam name="T">The lists object type.</typeparam>
		/// <param name="list">List to perform the sort operation on.</param>
		/// <param name="index"></param>
		/// <param name="count"></param>
		/// <param name="comparer">The comparer to use.</param>
		public static void StableSort<T>(this IList<T> list, int index, int count, IComparer<T> comparer)
		{
			StableSort<T>(list, index, count, comparer.Compare);
		}
		/// <summary>
		/// Performs a stable sort.
		/// </summary>
		/// <typeparam name="T">The lists object type.</typeparam>
		/// <param name="list">List to perform the sort operation on.</param>
		/// <param name="index"></param>
		/// <param name="count"></param>
		/// <param name="comparison">The comparison to use.</param>
		public static void StableSort<T>(this IList<T> list, int index, int count, Comparison<T> comparison)
		{
			if (count < 2) return;

			int middle = index + count / 2;
			T[] left = new T[middle - index];
			T[] right = new T[count - left.Length];

			if (list is T[])
			{
				T[] array = list as T[];
				Array.Copy(array, index, left, 0, left.Length);
				Array.Copy(array, middle, right, 0, right.Length);
			}
			else
			{
				for (int i = 0; i < middle; i++)
					left[i] = list[i + index];
				for (int i = 0; i < count - middle; i++)
					right[i] = list[i + middle];
			}

			StableSort(left, 0, left.Length, comparison);
			StableSort(right, 0, right.Length, comparison);

			int leftptr = 0;
			int rightptr = 0;
			for (int k = index ; k < index + count; k++)
			{
				if (rightptr == right.Length || ((leftptr < left.Length ) && comparison(left[leftptr], right[rightptr]) <= 0))
				{
					list[k] = left[leftptr];
					leftptr++;
				}
				else if (leftptr == left.Length || ((rightptr < right.Length ) && comparison(right[rightptr], left[leftptr]) <= 0))
				{
					list[k] = right[rightptr];
					rightptr++;
				}
			}
		}
		
		/// <summary>
		/// Performs an optimized zero-alloc stable sort on the specified array. Requires
		/// a buffer of at least the size of the array that is to be sorted.
		/// </summary>
		/// <typeparam name="T">The array element type.</typeparam>
		/// <param name="array">Array to perform the sort operation on.</param>
		/// <param name="buffer"></param>
		public static void StableSortZeroAlloc<T>(this IList<T> list, IList<T> buffer)
		{
			StableSortZeroAlloc<T>(list, buffer, 0, list.Count, Comparer<T>.Default.Compare);
		}
		/// <summary>
		/// Performs an optimized zero-alloc stable sort on the specified array. Requires
		/// a buffer of at least the size of the array that is to be sorted.
		/// </summary>
		/// <typeparam name="T">The array element type.</typeparam>
		/// <param name="array">Array to perform the sort operation on.</param>
		/// <param name="buffer"></param>
		/// <param name="comparer"></param>
		public static void StableSortZeroAlloc<T>(this IList<T> list, IList<T> buffer, IComparer<T> comparer)
		{
			StableSortZeroAlloc<T>(list, buffer, 0, list.Count, (comparer ?? Comparer<T>.Default).Compare);
		}
		/// <summary>
		/// Performs an optimized zero-alloc stable sort on the specified array. Requires
		/// a buffer of at least the size of the array that is to be sorted.
		/// </summary>
		/// <typeparam name="T">The array element type.</typeparam>
		/// <param name="array">Array to perform the sort operation on.</param>
		/// <param name="buffer"></param>
		/// <param name="comparison"></param>
		public static void StableSortZeroAlloc<T>(this IList<T> list, IList<T> buffer, Comparison<T> comparison)
		{
			StableSortZeroAlloc<T>(list, buffer, 0, list.Count, comparison);
		}
		/// <summary>
		/// Performs an optimized zero-alloc stable sort on the specified array. Requires
		/// a buffer of at least the size of the array that is to be sorted.
		/// </summary>
		/// <typeparam name="T">The array element type.</typeparam>
		/// <param name="array">Array to perform the sort operation on.</param>
		/// <param name="buffer"></param>
		/// <param name="index"></param>
		/// <param name="count"></param>
		public static void StableSortZeroAlloc<T>(this IList<T> list, IList<T> buffer, int index, int count)
		{
			StableSortZeroAlloc<T>(list, buffer, index, count, Comparer<T>.Default.Compare);
		}
		/// <summary>
		/// Performs an optimized zero-alloc stable sort on the specified array. Requires
		/// a buffer of at least the size of the array that is to be sorted.
		/// </summary>
		/// <typeparam name="T">The array element type.</typeparam>
		/// <param name="array">Array to perform the sort operation on.</param>
		/// <param name="buffer"></param>
		/// <param name="index"></param>
		/// <param name="count"></param>
		/// <param name="comparer"></param>
		public static void StableSortZeroAlloc<T>(this IList<T> list, IList<T> buffer, int index, int count, IComparer<T> comparer)
		{
			StableSortZeroAlloc<T>(list, buffer, index, count, (comparer ?? Comparer<T>.Default).Compare);
		}
		/// <summary>
		/// Performs an optimized zero-alloc stable sort on the specified array. Requires
		/// a buffer of at least the size of the array that is to be sorted.
		/// </summary>
		/// <typeparam name="T">The array element type.</typeparam>
		/// <param name="array">Array to perform the sort operation on.</param>
		/// <param name="buffer"></param>
		/// <param name="index"></param>
		/// <param name="count"></param>
		/// <param name="comparison"></param>
		public static void StableSortZeroAlloc<T>(this IList<T> list, IList<T> buffer, int index, int count, Comparison<T> comparison)
		{
			if (list == null) throw new ArgumentNullException("array");
			if (buffer == null) throw new ArgumentNullException("buffer");
			if (buffer.Count < list.Count) throw new ArgumentException("Zero-alloc stable sort requires a buffer of at least the sorted arrays size.", "buffer");
			if (index < 0) throw new ArgumentOutOfRangeException("index");
			if (index + count > list.Count) throw new ArgumentOutOfRangeException("count");

			// Fall back to default comparison when null
			comparison = comparison ?? Comparer<T>.Default.Compare;

			// Use an optimized array-based version when possible
			if (list is T[] && buffer is T[])
			{
				StableSortZeroAllocArray<T>(
					list as T[], 
					buffer as T[], 
					index, count, comparison);
			}
			else if (list is RawList<T> && buffer is RawList<T>)
			{
				StableSortZeroAllocArray<T>(
					(list as RawList<T>).Data, 
					(buffer as RawList<T>).Data, 
					index, count, comparison);
			}
			else
			{
				StableSortZeroAllocGeneric<T>(list, buffer, index, count, comparison);
			}
		}

		private static void StableSortZeroAllocArray<T>(T[] list, T[] buffer, int index, int count, Comparison<T> comparison)
		{
			// This is a variant of merge sort that skips the split step and
			// instead merges back and forth between two arrays of equal size.
			// In this case, iteration is a better fit than recursion.
			int iterationCount = (int)Math.Ceiling(Math.Log(list.Length, 2));
			T[] source = list;
			T[] target = buffer;

			// In each iteration, process source segments in pairs of two
			// and merge them into one target segment each.
			for (int i = 0; i < iterationCount; i++)
			{
				// Determine how big a single segment will be, and how many target segments
				// we'll have to generate by merging from two source segments each.
				int segmentSize = 1 << (i + 1);
				int segmentCount = (int)Math.Ceiling((float)count / (float)segmentSize);
				for (int s = 0; s < segmentCount; s++)
				{
					// First determine the target segment we'll work on
					int segmentOffset = s * segmentSize;
					int baseIndex = index + segmentOffset;
					int baseCount = segmentSize;

					if (s == segmentCount - 1)
						baseCount = count - segmentOffset;

					// Determine the two source segments we'll construct the
					// target segment from. Note that this needs to match
					// the previous iterations's target segments.
					int leftCount = segmentSize / 2;
					int rightCount = baseCount - leftCount;

					// If we're only spanning a single previous source segment,
					// skip merge and copy source to target to keep results.
					if (leftCount <= 0 || rightCount <= 0)
					{
						Array.Copy(source, baseIndex, target, baseIndex, baseCount);
						continue;
					}

					// Merge two segments from source into one segment of target
					int leftIndex = 0;
					int rightIndex = 0;
					for (int k = 0; k < baseCount; k++)
					{
						// If we reach the end of one of the segments, copy the rest of the other
						// as a single block without any further checks. This is an optimization.
						if (rightIndex == rightCount)
						{
							Array.Copy(
								source, baseIndex + leftIndex, 
								target, baseIndex + k, baseCount - k);
							break;
						}
						else if (leftIndex == leftCount)
						{
							Array.Copy(
								source, baseIndex + leftCount + rightIndex, 
								target, baseIndex + k, baseCount - k);
							break;
						}

						// Copy the smaller element of the two source segments to target
						if (comparison(source[baseIndex + leftIndex], source[baseIndex + leftCount + rightIndex]) <= 0)
						{
							target[baseIndex + k] = source[baseIndex + leftIndex];
							leftIndex++;
						}
						else
						{
							target[baseIndex + k] = source[baseIndex + leftCount + rightIndex];
							rightIndex++;
						}
					}
				}

				// Swap source and target
				MathF.Swap(ref source, ref target);
			}

			// If the last result ended up in the buffer, copy the results back to the original array
			if (source != list)
			{
				Array.Copy(source, index, list, index, count);
			}
		}
		private static void StableSortZeroAllocGeneric<T>(IList<T> list, IList<T> buffer, int index, int count, Comparison<T> comparison)
		{
			// This is a variant of merge sort that skips the split step and
			// instead merges back and forth between two arrays of equal size.
			// In this case, iteration is a better fit than recursion.
			int iterationCount = (int)Math.Ceiling(Math.Log(list.Count, 2));
			IList<T> source = list;
			IList<T> target = buffer;

			// In each iteration, process source segments in pairs of two
			// and merge them into one target segment each.
			for (int iteration = 0; iteration < iterationCount; iteration++)
			{
				// Determine how big a single segment will be, and how many target segments
				// we'll have to generate by merging from two source segments each.
				int segmentSize = 1 << (iteration + 1);
				int segmentCount = (int)Math.Ceiling((float)count / (float)segmentSize);
				for (int s = 0; s < segmentCount; s++)
				{
					// First determine the target segment we'll work on
					int segmentOffset = s * segmentSize;
					int baseIndex = index + segmentOffset;
					int baseCount = segmentSize;

					if (s == segmentCount - 1)
						baseCount = count - segmentOffset;

					// Determine the two source segments we'll construct the
					// target segment from. Note that this needs to match
					// the previous iterations's target segments.
					int leftCount = segmentSize / 2;
					int rightCount = baseCount - leftCount;

					// If we're only spanning a single previous source segment,
					// skip merge and copy source to target to keep results.
					if (leftCount <= 0 || rightCount <= 0)
					{
						ListCopy(source, baseIndex, target, baseIndex, baseCount);
						continue;
					}

					// Merge two segments from source into one segment of target
					int leftIndex = 0;
					int rightIndex = 0;
					for (int k = 0; k < baseCount; k++)
					{
						// If we reach the end of one of the segments, copy the rest of the other
						// as a single block without any further checks. This is an optimization.
						if (rightIndex == rightCount)
						{
							ListCopy(
								source, baseIndex + leftIndex, 
								target, baseIndex + k, baseCount - k);
							break;
						}
						else if (leftIndex == leftCount)
						{
							ListCopy(
								source, baseIndex + leftCount + rightIndex, 
								target, baseIndex + k, baseCount - k);
							break;
						}

						// Copy the smaller element of the two source segments to target
						if (comparison(source[baseIndex + leftIndex], source[baseIndex + leftCount + rightIndex]) <= 0)
						{
							target[baseIndex + k] = source[baseIndex + leftIndex];
							leftIndex++;
						}
						else
						{
							target[baseIndex + k] = source[baseIndex + leftCount + rightIndex];
							rightIndex++;
						}
					}
				}

				// Swap source and target
				MathF.Swap(ref source, ref target);
			}

			// If the last result ended up in the buffer, copy the results back to the original array
			if (source != list)
			{
				ListCopy(source, index, list, index, count);
			}
		}
		private static void ListCopy<T>(IList<T> source, int sourceIndex, IList<T> target, int targetIndex, int count)
		{
			for (int i = 0; i < count; i++)
			{
				target[targetIndex + i] = source[sourceIndex + i];
			}
		}

		/// <summary>
		/// Returns the index of the first object matching the specified one.
		/// </summary>
		/// <typeparam name="T">The lists object type.</typeparam>
		/// <param name="collection">List to perform the sort operation on.</param>
		/// <param name="val">Object to compare the lists contents to.</param>
		/// <returns></returns>
		public static int IndexOfFirst<T>(this IList<T> collection, T val)
		{
			var cmp = EqualityComparer<T>.Default;
			for (int i = 0; i < collection.Count; i++)
				if (cmp.Equals(collection[i], val)) return i;
			return -1;
		}
		/// <summary>
		/// Returns the index of the first object matching the specified predicate.
		/// </summary>
		/// <typeparam name="T">The lists object type.</typeparam>
		/// <param name="collection">List to perform the sort operation on.</param>
		/// <param name="pred">The predicate to use on the lists contents.</param>
		/// <returns></returns>
		public static int IndexOfFirst<T>(this IList<T> collection, Predicate<T> pred)
		{
			for (int i = 0; i < collection.Count; i++)
				if (pred(collection[i])) return i;
			return -1;
		}
		/// <summary>
		/// Returns the index of the last object matching the specified one.
		/// </summary>
		/// <typeparam name="T">The lists object type.</typeparam>
		/// <param name="collection">List to perform the sort operation on.</param>
		/// <param name="val">Object to compare the lists contents to.</param>
		/// <returns></returns>
		public static int IndexOfLast<T>(this IList<T> collection, T val)
		{
			var cmp = EqualityComparer<T>.Default;
			for (int i = collection.Count - 1; i >= 0; i--)
				if (cmp.Equals(collection[i], val)) return i;
			return -1;
		}
		/// <summary>
		/// Returns the index of the last object matching the specified predicate.
		/// </summary>
		/// <typeparam name="T">The lists object type.</typeparam>
		/// <param name="collection">List to perform the sort operation on.</param>
		/// <param name="pred">The predicate to use on the lists contents.</param>
		/// <returns></returns>
		public static int IndexOfLast<T>(this IList<T> collection, Predicate<T> pred)
		{
			for (int i = collection.Count - 1; i >= 0; i--)
				if (pred(collection[i])) return i;
			return -1;
		}

		/// <summary>
		/// Returns the combined hash code of the specified byte list.
		/// </summary>
		/// <param name="list"></param>
		/// <returns></returns>
		public static int GetCombinedHashCode(this IList<byte> list, int firstIndex = 0, int length = -1)
		{
			if (length == -1) length = list.Count;
			int endIndex = length < 0 ? list.Count - firstIndex : firstIndex + length;
			unchecked
			{
				const int p = 16777619;
				int hash = (int)2166136261;

				for (int i = firstIndex; i < endIndex; i++)
					hash = (hash ^ list[i]) * p;
			
				return hash;
			}
		}
		/// <summary>
		/// Returns the combined hash code of the specified list.
		/// </summary>
		/// <param name="list"></param>
		/// <returns></returns>
		public static int GetCombinedHashCode<T>(this IList<T> list, int firstIndex = 0, int length = -1)
		{
			if (length == -1) length = list.Count;
			int endIndex = length < 0 ? list.Count - firstIndex : firstIndex + length;
			unchecked
			{
				const int p = 16777619;
				int hash = (int)2166136261;

				for (int i = firstIndex; i < endIndex; i++)
					hash = (hash ^ list[i].GetHashCode()) * p;
			
				return hash;
			}
		}

		/// <summary>
		/// Determines the bounding box of a list of vectors.
		/// </summary>
		/// <param name="list"></param>
		/// <returns></returns>
		public static Rect BoundingBox(this IList<Vector2> list)
		{
			Rect pointBoundingRect = new Rect(
				float.MaxValue, 
				float.MaxValue, 
				float.MinValue, 
				float.MinValue);
			if (list is Vector2[])
			{
				Vector2[] array = list as Vector2[];
				for (int i = 0; i < array.Length; i++)
				{
					pointBoundingRect.X = MathF.Min(array[i].X, pointBoundingRect.X);
					pointBoundingRect.Y = MathF.Min(array[i].Y, pointBoundingRect.Y);
					pointBoundingRect.W = MathF.Max(array[i].X, pointBoundingRect.W);
					pointBoundingRect.H = MathF.Max(array[i].Y, pointBoundingRect.H);
				}
			}
			else
			{
				for (int i = 0; i < list.Count; i++)
				{
					pointBoundingRect.X = MathF.Min(list[i].X, pointBoundingRect.X);
					pointBoundingRect.Y = MathF.Min(list[i].Y, pointBoundingRect.Y);
					pointBoundingRect.W = MathF.Max(list[i].X, pointBoundingRect.W);
					pointBoundingRect.H = MathF.Max(list[i].Y, pointBoundingRect.H);
				}
			}
			pointBoundingRect.W -= pointBoundingRect.X;
			pointBoundingRect.H -= pointBoundingRect.Y;
			return pointBoundingRect;
		}
	}
}
