using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Duality.Utility.Pooling
{
	internal class QueuedPool<T> where T : class, new()
	{
		private Queue<T> pool = new Queue<T>();

		public T GetOne()
		{
			T result = null;

			if (this.pool.Count > 0)
				result = this.pool.Dequeue();
			else
				result = new T();

			if (result is IPoolable)
				(result as IPoolable).OnPickup();

			return result;
		}

		public void ReturnOne(T obj)
		{
			if (obj is IPoolable)
				(obj as IPoolable).OnReturn();

			if (obj != null)
				this.pool.Enqueue(obj);
		}
	}
}
