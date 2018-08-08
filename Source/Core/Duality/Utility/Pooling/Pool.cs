using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Duality.Utility.Pooling
{
	public class Pool<T> where T : class, new()
	{
		private Queue<T> pool = new Queue<T>();
		private int poolIndex = 0;
		private int searchIndex = 0;

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

			this.pool.Enqueue(obj);
		}
	}
}
