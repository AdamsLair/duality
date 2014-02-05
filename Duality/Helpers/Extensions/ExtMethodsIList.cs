using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using OpenTK;

namespace Duality
{
	/// <summary>
	/// Provides extension methods for lists.
	/// </summary>
	public static class ExtMethodsIList
	{
		/// <summary>
		/// Clears a collection of objects from all disposed and null references. 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		public static void FlushDisposedObj<T>(this IList<T> list) where T : IManageableObject
		{
			for (int i = list.Count - 1; i >= 0; i--)
			{
				if (list[i] == null || list[i].Disposed)
				{
					list.RemoveAt(i);
				}
			}
		}
		/// <summary>
		/// Clears a collection of objects from all disposed and null references. 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		public static void FlushDisposedObj<T>(this IList<T> list, out List<T> removedObj) where T : IManageableObject
		{
			removedObj = new List<T>();
			for (int i = list.Count - 1; i >= 0; i--)
			{
				if (list[i] == null || list[i].Disposed)
				{
					if (list[i] != null) removedObj.Add(list[i]);
					list.RemoveAt(i);
				}
			}
		}
		
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
		public static void StableSort<T>(this IList<T> list, Comparer<T> comparer)
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
		public static void StableSort<T>(this IList<T> list, int index, int count, Comparer<T> comparer)
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
			// System.Threading.Tasks.Parallel is not a good idea in WinForms because it triggers the message loop.
			// Thus, parallel merge sorting isn't used anymore. Maybe it could be re-activated later.
			//if (list.Count >= 512 && DualityApp.ExecEnvironment == DualityApp.ExecutionEnvironment.Launcher)
			//	StableSort_Parallel(list, index, count, comparison);
			//else
				StableSort_Sequential(list, index, count, comparison);
		}

		private static void StableSort_Sequential<T>(this IList<T> list, int index, int count, Comparison<T> comparison)
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

			StableSort_Sequential(left, 0, left.Length, comparison);
			StableSort_Sequential(right, 0, right.Length, comparison);

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
		private static void StableSort_Parallel<T>(IList<T> list, int index, int count, Comparison<T> comparison)
		{
			if (list.Count < 2) return;

			int middle = index + count / 2;
			T[] left = null;
			T[] right = null;

			Parallel.Invoke(
				() =>
				{
					left = new T[middle - index];
					if (list is T[])
					{
						T[] array = list as T[];
						Array.Copy(array, index, left, 0, left.Length);
					}
					else
					{
						for (int i = 0; i < middle; i++)
							left[i] = list[i + index];
					}
					StableSort(left, 0, left.Length, comparison);
				},
				() => 
				{
					right = new T[count - (middle - index)];
					if (list is T[])
					{
						T[] array = list as T[];
						Array.Copy(array, middle, right, 0, right.Length);
					}
					else
					{
						for (int i = 0; i < count - middle; i++)
							right[i] = list[i + middle];
					}
					StableSort(right, 0, right.Length, comparison);
				});
			
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
