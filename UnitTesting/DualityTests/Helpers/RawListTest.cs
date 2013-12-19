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

			Assert.IsTrue(intList.Count == 4);
			Assert.IsTrue(intList.Contains(42));
			Assert.IsTrue(intList.IndexOf(42) == 2);
			Assert.IsTrue(intList[2] == 42);
			Assert.IsTrue(intList.Data[2] == 42);

			intList.ShrinkToFit();
			Assert.IsTrue(intList.Count == intList.Capacity);

			intList.Remove(42);
			Assert.IsTrue(intList.Count == 3);
			Assert.IsTrue(!intList.Contains(42));
			Assert.IsTrue(intList.IndexOf(42) == -1);

			intList.Clear();
			Assert.IsTrue(intList.Count == 0);
			Assert.IsTrue(!intList.Contains(94));
		}
		[Test] public void Move()
		{
			int[] testArray = Enumerable.Range(0, 10).ToArray();
			RawList<int> intList = new RawList<int>();

			intList.AddRange(testArray);
			intList.Move(0, 3, 1);
			Assert.IsTrue(intList.SequenceEqual(new int[] { 0, 0, 1, 2, 4, 5, 6, 7, 8, 9 }));
			intList.Clear();

			intList.AddRange(testArray);
			intList.Move(0, 3, 3);
			Assert.IsTrue(intList.SequenceEqual(new int[] { 0, 1, 2, 0, 1, 2, 6, 7, 8, 9 }));
			intList.Clear();

			intList.AddRange(testArray);
			intList.Move(0, 3, 5);
			Assert.IsTrue(intList.SequenceEqual(new int[] { 0, 1, 2, 3, 4, 0, 1, 2, 8, 9 }));
			intList.Clear();

			intList.AddRange(testArray);
			intList.Move(7, 3, -1);
			Assert.IsTrue(intList.SequenceEqual(new int[] { 0, 1, 2, 3, 4, 5, 7, 8, 9, 9 }));
			intList.Clear();

			intList.AddRange(testArray);
			intList.Move(7, 3, -3);
			Assert.IsTrue(intList.SequenceEqual(new int[] { 0, 1, 2, 3, 7, 8, 9, 7, 8, 9 }));
			intList.Clear();

			intList.AddRange(testArray);
			intList.Move(7, 3, -5);
			Assert.IsTrue(intList.SequenceEqual(new int[] { 0, 1, 7, 8, 9, 5, 6, 7, 8, 9 }));
			intList.Clear();
		}
		[Test] public void Resize()
		{
			int[] testArray = Enumerable.Range(0, 10).ToArray();
			RawList<int> intList = new RawList<int>();

			intList.AddRange(testArray);
			Assert.IsTrue(intList.SequenceEqual(testArray));

			intList.Count = 20;
			Assert.IsTrue(intList.Count == 20);
			Assert.IsTrue(intList.SequenceEqual(testArray.Concat(new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 })));

			intList[19] = 19;
			Assert.IsTrue(intList[19] == 19);
			Assert.IsTrue(intList.Data[19] == 19);
		}
		[Test] public void Sort()
		{
			int[] testArray = Enumerable.Range(0, 10).ToArray();
			RawList<int> intList = new RawList<int>();

			intList.AddRange(testArray.Reverse().ToArray());
			Assert.IsTrue(intList.SequenceEqual(testArray.Reverse()));

			intList.Sort();
			Assert.IsTrue(intList.SequenceEqual(testArray));
		}
	}
}
