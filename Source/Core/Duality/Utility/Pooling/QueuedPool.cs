using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Duality.Utility.Pooling
{
	public sealed class QueuedPool<T> where T : class, new()
	{
		private readonly Queue<T> pool = new Queue<T>();

		public T GetOne()
		{
			T result = null;

			if (this.pool.Count > 0)
				result = this.pool.Dequeue();
			else
				result = new T();

			if (result is IPoolable poolable)
				poolable.OnPickup();

			return result;
		}

		public void ReturnOne(T obj)
		{
			if (obj is IPoolable poolable)
				poolable.OnReturn();

			if (obj != null)
				this.pool.Enqueue(obj);
		}
	}
}
