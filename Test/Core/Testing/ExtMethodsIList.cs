using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

using Duality.Drawing;

namespace Duality.Tests
{
	public static class ExtMethodsIList
	{
		public static bool IsSorted<T>(this IList<T> values, int index, int count, Comparer<T> comparer = null)
		{
			if (comparer == null)
				comparer = Comparer<T>.Default;

			for (int i = index + 1; i < index + count; i++)
			{
				T last = values[i - 1];
				T current = values[i];

				if (comparer.Compare(last, current) > 0)
					return false;
			}

			return true;
		}
		public static bool IsStable<T>(this IList<T> values, int index, int count, IList<T> valuesInOriginalOrder, Comparer<T> comparer = null)
		{
			if (comparer == null)
				comparer = Comparer<T>.Default;

			// Create a map from original values to their index
			Dictionary<T,int> originalIndex = new Dictionary<T,int>(new IdentityComparer<T>());
			for (int i = index; i < index + count; i++)
			{
				originalIndex[valuesInOriginalOrder[i]] = i;
			}

			// Make sure the provided value enumeration still retains its
			// original order when two values are equivalent.
			for (int i = index + 1; i < index + count; i++)
			{
				T last = values[i - 1];
				T current = values[i];

				if (comparer.Compare(last, current) == 0 && originalIndex[last] > originalIndex[current])
					return false;
			}

			return true;
		}
	}
}
