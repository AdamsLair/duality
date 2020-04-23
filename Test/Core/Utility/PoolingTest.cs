using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Duality.Resources;
using Duality.Utility.Pooling;
using NUnit.Framework;

namespace Duality.Tests.Utility
{
	public class TestObject
	{
		public string Guid { get; private set; } = System.Guid.NewGuid().ToString();
		public int TakenTimes { get; set; }
	}

	public class DerivedObjectA : TestObject
	{
		public int Integer { get; set; }
	}

	public class DerivedObjectB : TestObject
	{
		public float Float { get; set; }
	}

	public class TestPoolableObject : IPoolable
	{
		public int TakenTimes { get; private set; }
		public void OnPickup()
		{
			this.TakenTimes += 2;
		}

		public void OnReturn()
		{
			this.TakenTimes -= 1;
		}
	}

	[TestFixture]
	public class PoolingTest
	{
		[Test] public void QueuedPoolTest()
		{
			QueuedPool<TestObject> pool = new QueuedPool<TestObject>();

			TestObject firstObject = pool.GetOne();
			string firstGuid = firstObject.Guid;
			pool.ReturnOne(firstObject);

			// check that, no matter how many times I take an object and send it back, 
			// it's always the same
			for(int i = 0; i < 100; i++)
			{
				TestObject obj = pool.GetOne();
				Assert.AreEqual(firstGuid, obj.Guid);
				pool.ReturnOne(obj);
			}

			int poolContents = this.GetUnderlyingPoolCount(pool);
			Assert.AreEqual(1, poolContents);
		}

		[Test]
		public void QueuedPoolableTest()
		{
			QueuedPool<TestPoolableObject> pool = new QueuedPool<TestPoolableObject>();

			for (int i = 0; i < 100; i++)
			{
				TestPoolableObject obj = pool.GetOne();

				// 101 = 99 times [+2/-1] + this time only +2 because it was not returned yet
				if (i == 99)
					Assert.AreEqual(101, obj.TakenTimes); 

				pool.ReturnOne(obj);
			}

			int poolContents = this.GetUnderlyingPoolCount(pool);
			Assert.AreEqual(1, poolContents);
		}

		[Test]
		public void DerivedObjectsPoolTest()
		{
			DerivedObjectsPool<TestObject> pool = new DerivedObjectsPool<TestObject>();
			TestObject obj;

			for (int i = 0; i < 100; i++)
			{
				if (i % 2 == 0)
				{
					obj = pool.GetOne<DerivedObjectA>();
					(obj as DerivedObjectA).Integer += i;
				}
				else
				{
					obj = pool.GetOne<DerivedObjectB>();
					(obj as DerivedObjectB).Float += i;
				}

				obj.TakenTimes++;
				pool.ReturnOne(obj);
			}

			int poolContents = this.GetUnderlyingPoolCount(pool);
			Assert.AreEqual(2, poolContents);

			DerivedObjectA a = pool.GetOne<DerivedObjectA>();
			DerivedObjectB b = pool.GetOne<DerivedObjectB>();

			Assert.AreEqual(100, a.TakenTimes + b.TakenTimes);

			poolContents = this.GetUnderlyingPoolCount(pool);
			Assert.AreEqual(0, poolContents);
		}

		private int GetUnderlyingPoolCount<T>(QueuedPool<T> pool) where T : class, new()
		{
			// hacking at internal bits to recover the actual amount of objects in the pool
			Queue<T> internalQueue = pool.GetType()
				.GetField("pool", BindingFlags.Instance | BindingFlags.NonPublic)
				.GetValue(pool) as Queue<T>;

			return internalQueue.Count;
		}

		private int GetUnderlyingPoolCount<T>(DerivedObjectsPool<T> pool) where T : class, new()
		{
			// hacking at internal bits to recover the actual amount of objects in the pool
			List<T> internalList = pool.GetType()
				.GetField("pool", BindingFlags.Instance | BindingFlags.NonPublic)
				.GetValue(pool) as List<T>;

			return internalList.Count;
		}
	}
}
