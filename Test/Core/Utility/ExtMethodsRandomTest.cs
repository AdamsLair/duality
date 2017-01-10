using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

using Duality;

using NUnit.Framework;

namespace Duality.Tests.Utility
{
	[TestFixture]
	public class ExtMethodsRandomTest
	{
		[Test] public void Shuffle()
		{
			int[] numbers = Enumerable.Range(0, 10).ToArray();
			Assert.IsTrue(IsSorted(numbers));

			Random rnd = new Random(1);
			rnd.Shuffle(numbers);
			Assert.IsFalse(IsSorted(numbers));
		}

		private static bool IsSorted<T>(IEnumerable<T> values, Comparer<T> comparer = null)
		{
			if (comparer == null)
				comparer = Comparer<T>.Default;

			bool first = true;
			T last = default(T);
			foreach (T current in values)
			{
				if (first)
				{
					first = false;
					last = current;
					continue;
				}
				else
				{
					if (comparer.Compare(last, current) >= 1)
						return false;
					last = current;
				}
			}
			return true;
		}
	}
}
