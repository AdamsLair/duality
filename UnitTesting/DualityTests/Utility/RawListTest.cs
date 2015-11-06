using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

using Duality;

using NUnit.Framework;

namespace Duality.Tests.Utility
{
	[TestFixture]
	public class RawListTest
	{
		[Test] public void Basics()
		{
			RawList<int> intList = new RawList<int>();
			intList.Add(10);
			intList.AddRange(new int[] { 17, 42, 94 });

			Assert.AreEqual(4, intList.Count);
			Assert.IsTrue(intList.Contains(42));
			Assert.AreEqual(2, intList.IndexOf(42));
			CollectionAssert.AreEqual(new int[] { 10, 17, 42, 94 }, intList);
			CollectionAssert.AreEqual(new int[] { 10, 17, 42, 94 }, intList.Data.Take(intList.Count));

			intList.ShrinkToFit();
			Assert.AreEqual(intList.Count, intList.Capacity);

			intList.Remove(42);
			Assert.AreEqual(3, intList.Count);
			Assert.IsTrue(!intList.Contains(42));
			Assert.AreEqual(-1, intList.IndexOf(42));
			CollectionAssert.AreEqual(new int[] { 10, 17, 94 }, intList);
			CollectionAssert.AreEqual(new int[] { 10, 17, 94 }, intList.Data.Take(intList.Count));

			intList.Insert(1, 100);
			CollectionAssert.AreEqual(new int[] { 10, 100, 17, 94 }, intList);
			CollectionAssert.AreEqual(new int[] { 10, 100, 17, 94 }, intList.Data.Take(intList.Count));

			intList.InsertRange(2, new int[] { 150, 200, 250, 300 });
			CollectionAssert.AreEqual(new int[] { 10, 100, 150, 200, 250, 300, 17, 94 }, intList);
			CollectionAssert.AreEqual(new int[] { 10, 100, 150, 200, 250, 300, 17, 94 }, intList.Data.Take(intList.Count));

			intList.RemoveAt(1);
			CollectionAssert.AreEqual(new int[] { 10, 150, 200, 250, 300, 17, 94 }, intList);
			CollectionAssert.AreEqual(new int[] { 10, 150, 200, 250, 300, 17, 94 }, intList.Data.Take(intList.Count));

			intList.Clear();
			Assert.AreEqual(0, intList.Count);
			Assert.IsTrue(!intList.Contains(94));
		}
		[Test] public void Move()
		{
			int[] testArray = Enumerable.Range(0, 10).ToArray();
			RawList<int> intList = new RawList<int>();

			intList.AddRange(testArray);
			intList.Move(0, 3, 1);
			CollectionAssert.AreEqual(new int[] { 0, 0, 1, 2, 4, 5, 6, 7, 8, 9 }, intList);
			intList.Clear();

			intList.AddRange(testArray);
			intList.Move(0, 3, 3);
			CollectionAssert.AreEqual(new int[] { 0, 1, 2, 0, 1, 2, 6, 7, 8, 9 }, intList);
			intList.Clear();

			intList.AddRange(testArray);
			intList.Move(0, 3, 5);
			CollectionAssert.AreEqual(new int[] { 0, 1, 2, 3, 4, 0, 1, 2, 8, 9 }, intList);
			intList.Clear();

			intList.AddRange(testArray);
			intList.Move(7, 3, -1);
			CollectionAssert.AreEqual(new int[] { 0, 1, 2, 3, 4, 5, 7, 8, 9, 9 }, intList);
			intList.Clear();

			intList.AddRange(testArray);
			intList.Move(7, 3, -3);
			CollectionAssert.AreEqual(new int[] { 0, 1, 2, 3, 7, 8, 9, 7, 8, 9 }, intList);
			intList.Clear();

			intList.AddRange(testArray);
			intList.Move(7, 3, -5);
			CollectionAssert.AreEqual(new int[] { 0, 1, 7, 8, 9, 5, 6, 7, 8, 9 }, intList);
			intList.Clear();
		}
		[Test] public void Resize()
		{
			int[] testArray = Enumerable.Range(0, 10).ToArray();
			RawList<int> intList = new RawList<int>();

			intList.AddRange(testArray);
			CollectionAssert.AreEqual(testArray, intList);

			intList.Count = 20;
			Assert.IsTrue(intList.Count == 20);
			CollectionAssert.AreEqual(testArray.Concat(new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }), intList);

			intList[19] = 19;
			Assert.IsTrue(intList[19] == 19);
			Assert.IsTrue(intList.Data[19] == 19);
		}
		[Test] public void Sort()
		{
			int[] testArray = Enumerable.Range(0, 10).ToArray();
			RawList<int> intList = new RawList<int>();

			// Sorting an empty array is a no-op, but entirely valid. No exceptions expected.
			intList.Sort();

			// Insert the reversed data
			intList.AddRange(testArray.Reverse().ToArray());
			CollectionAssert.AreEqual(testArray.Reverse(), intList);

			// Sort it and check if its equal to the original data
			intList.Sort();
			CollectionAssert.AreEqual(testArray, intList);
		}
		[Test] public void RemoveAll()
		{
			// Remove nothing
			{
				RawList<int> list = new RawList<int>(Enumerable.Range(0, 10));
				list.RemoveAll(i => false);
				CollectionAssert.AreEqual(Enumerable.Range(0, 10), list);
			}

			// Remove everything
			{
				RawList<int> list = new RawList<int>(Enumerable.Range(0, 10));
				list.RemoveAll(i => true);
				CollectionAssert.AreEqual(new int[0], list);
			}

			// Remove all even numbers
			{
				RawList<int> list = new RawList<int>(Enumerable.Range(0, 10));
				list.RemoveAll(i => i % 2 == 0);
				CollectionAssert.AreEqual(new int[] { 1, 3, 5, 7, 9 }, list);
			}
		}
	}
}
