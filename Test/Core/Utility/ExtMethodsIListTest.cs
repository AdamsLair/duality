using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

using Duality;

using NUnit.Framework;

namespace Duality.Tests.Utility
{
	[TestFixture]
	public class ExtMethodsIListTest
	{
		private static IEnumerable<TestCaseData> SortingTestCases
		{
			get
			{
				yield return new TestCaseData(100, 0, 100);
				yield return new TestCaseData(100, 0, 30);
				yield return new TestCaseData(100, 70, 30);
				yield return new TestCaseData(100, 30, 30);
				yield return new TestCaseData(0, 0, 0);
				yield return new TestCaseData(1, 0, 1);
				yield return new TestCaseData(2, 0, 2);
			}
		}

		[TestCaseSource("SortingTestCases")]
		[Test] public void StableSortPrimitive(int itemCount, int sortIndex, int sortCount)
		{
			// Generate a shuffled sequence of numbers
			Random rnd = new Random(1);
			int[] values = 
				Enumerable.Range(0, itemCount)
				.Select(i => i / 3)
				.ToArray();
			rnd.Shuffle(values);
			
			// Test sorting algorithm on various list container types
			TestStableSort(values.ToArray(), sortIndex, sortCount);
			TestStableSort(values.ToList(), sortIndex, sortCount);
			TestStableSort(new RawList<int>(values.ToArray()), sortIndex, sortCount);
		}
		[TestCaseSource("SortingTestCases")]
		[Test] public void StableSortClass(int itemCount, int sortIndex, int sortCount)
		{
			// Generate a shuffled sequence of numbers, wrapped in a container instance each
			Random rnd = new Random(1);
			SortingIntContainer[] values = 
				Enumerable.Range(0, itemCount)
				.Select(i => i / 3)
				.Select(i => new SortingIntContainer(i))
				.ToArray();
			rnd.Shuffle(values);

			// Test sorting algorithm on various list container types
			TestStableSort(values.ToArray(), sortIndex, sortCount);
			TestStableSort(values.ToList(), sortIndex, sortCount);
			TestStableSort(new RawList<SortingIntContainer>(values.ToArray()), sortIndex, sortCount);
		}
		[TestCaseSource("SortingTestCases")]
		[Test] public void StableSortZeroAllocPrimitive(int itemCount, int sortIndex, int sortCount)
		{
			// Generate a shuffled sequence of numbers
			Random rnd = new Random(1);
			int[] values = 
				Enumerable.Range(0, itemCount)
				.Select(i => i / 3)
				.ToArray();
			rnd.Shuffle(values);

			// Test sorting algorithm on various list container types
			TestStableSortZeroAlloc(values.ToArray(), sortIndex, sortCount);
			TestStableSortZeroAlloc(values.ToList(), sortIndex, sortCount);
			TestStableSortZeroAlloc(new RawList<int>(values.ToArray()), sortIndex, sortCount);
		}
		[TestCaseSource("SortingTestCases")]
		[Test] public void StableSortZeroAllocClass(int itemCount, int sortIndex, int sortCount)
		{
			// Generate a shuffled sequence of numbers, wrapped in a container instance each
			Random rnd = new Random(1);
			SortingIntContainer[] values = 
				Enumerable.Range(0, itemCount)
				.Select(i => i / 3)
				.Select(i => new SortingIntContainer(i))
				.ToArray();
			rnd.Shuffle(values);

			// Test sorting algorithm on various list container types
			TestStableSortZeroAlloc(values.ToArray(), sortIndex, sortCount);
			TestStableSortZeroAlloc(values.ToList(), sortIndex, sortCount);
			TestStableSortZeroAlloc(new RawList<SortingIntContainer>(values.ToArray()), sortIndex, sortCount);
		}
		
		private void TestStableSortZeroAlloc<T>(IList<T> values, int sortIndex, int sortCount)
		{
			Comparer<T> comparer = Comparer<T>.Default;

			// Create a list of wrapped numbers where each knows its original place
			IList<T> originalList = values.ToArray();
			IList<T> workingList = values;
			IList<T> buffer = new T[workingList.Count];

			// Test precondition: The numbers really are out of order, but each item knows its original place
			Assert.IsTrue(workingList.Count < 10 || !workingList.IsSorted(sortIndex, sortCount, comparer), "Items shuffled before sort.");
			Assert.IsTrue(workingList.IsStableOrder(sortIndex, sortCount, originalList, comparer), "Equivalent items in sequential order before sort.");

			// Sort the shuffled numbers again
			workingList.StableSortZeroAlloc(buffer, sortIndex, sortCount, comparer);

			// Assert that we didn't lose any items, and that they're now stable-sorted
			CollectionAssert.AreEquivalent(originalList, workingList, "Same items before and after sort.");
			Assert.IsTrue(workingList.IsSorted(sortIndex, sortCount, comparer), "Items in order after sort.");
			Assert.IsTrue(workingList.IsStableOrder(sortIndex, sortCount, originalList, comparer), "Equivalent items in same order after sort than before.");

			// If we only sorted part of the list, assert that all the other parts stayed the same
			if (sortIndex > 0)
			{
				CollectionAssert.AreEqual(
					originalList.Take(sortIndex), 
					workingList.Take(sortIndex), 
					comparer, 
					"Did not modify items outside the sorted range.");
			}
			if (sortIndex + sortCount < values.Count)
			{
				CollectionAssert.AreEqual(
					originalList.Skip(sortIndex + sortCount), 
					workingList.Skip(sortIndex + sortCount), 
					comparer, 
					"Did not modify items outside the sorted range.");
			}
		}
		private void TestStableSort<T>(IList<T> values, int sortIndex, int sortCount)
		{
			Comparer<T> comparer = Comparer<T>.Default;

			// Create a list of wrapped numbers where each knows its original place
			IList<T> originalList = values.ToArray();
			IList<T> workingList = values;

			// Test precondition: The numbers really are out of order, but each item knows its original place
			Assert.IsTrue(workingList.Count < 10 || !workingList.IsSorted(sortIndex, sortCount, comparer), "Items shuffled before sort.");
			Assert.IsTrue(workingList.IsStableOrder(sortIndex, sortCount, originalList, comparer), "Equivalent items in sequential order before sort.");

			// Sort the shuffled numbers again
			workingList.StableSort(sortIndex, sortCount, comparer);

			// Assert that we didn't lose any items, and that they're now stable-sorted
			CollectionAssert.AreEquivalent(originalList, workingList, "Same items before and after sort.");
			Assert.IsTrue(workingList.IsSorted(sortIndex, sortCount, comparer), "Items in order after sort.");
			Assert.IsTrue(workingList.IsStableOrder(sortIndex, sortCount, originalList, comparer), "Equivalent items in same order after sort than before.");

			// If we only sorted part of the list, assert that all the other parts stayed the same
			if (sortIndex > 0)
			{
				CollectionAssert.AreEqual(
					originalList.Take(sortIndex), 
					workingList.Take(sortIndex), 
					comparer, 
					"Did not modify items outside the sorted range.");
			}
			if (sortIndex + sortCount < values.Count)
			{
				CollectionAssert.AreEqual(
					originalList.Skip(sortIndex + sortCount), 
					workingList.Skip(sortIndex + sortCount), 
					comparer, 
					"Did not modify items outside the sorted range.");
			}
		}
	}
}
