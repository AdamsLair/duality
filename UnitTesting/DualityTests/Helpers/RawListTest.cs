using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

using Duality;

using NUnit.Framework;

namespace DualityTests
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
			CollectionAssert.AreEqual(new int[] { 10, 17, 42, 94 }, intList.Data.Take(4));

			intList.ShrinkToFit();
			Assert.AreEqual(intList.Count, intList.Capacity);

			intList.Remove(42);
			Assert.AreEqual(3, intList.Count);
			Assert.IsTrue(!intList.Contains(42));
			Assert.AreEqual(-1, intList.IndexOf(42));
			CollectionAssert.AreEqual(new int[] { 10, 17, 94 }, intList);
			CollectionAssert.AreEqual(new int[] { 10, 17, 94 }, intList.Data.Take(3));

			intList.Insert(1, 100);
			CollectionAssert.AreEqual(new int[] { 10, 100, 17, 94 }, intList);
			CollectionAssert.AreEqual(new int[] { 10, 100, 17, 94 }, intList.Data.Take(4));

			intList.InsertRange(2, new int[] { 150, 200, 250, 300 });
			CollectionAssert.AreEqual(new int[] { 10, 100, 150, 200, 250, 300, 17, 94 }, intList);
			CollectionAssert.AreEqual(new int[] { 10, 100, 150, 200, 250, 300, 17, 94 }, intList.Data.Take(8));

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

			intList.AddRange(testArray.Reverse().ToArray());
			CollectionAssert.AreEqual(testArray.Reverse(), intList);

			intList.Sort();
			CollectionAssert.AreEqual(testArray, intList);
		}
	}
}
