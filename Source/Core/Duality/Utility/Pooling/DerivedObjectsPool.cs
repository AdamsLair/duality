using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Duality.Utility.Pooling
{
	public sealed class DerivedObjectsPool<T> where T : class
	{
		private readonly List<T> pool = new List<T>();
		private int poolIndex = 0;

		public V GetOne<V>() where V : class, T, new()
		{
			int searchIndex;
			V result = null;

			for (int i = this.poolIndex; i < this.pool.Count + this.poolIndex; i++)
			{
				searchIndex = i % this.pool.Count;

				result = this.pool[searchIndex] as V;

				if (result != null)
				{
					this.poolIndex = searchIndex;
					this.pool.RemoveAt(this.poolIndex);

					break;
				}
			}

			if (result == null)
				result = new V();

			if (result is IPoolable poolable)
				poolable.OnPickup();

			return result;
		}

		public void ReturnOne(T obj)
		{
			if (obj is IPoolable poolable)
				poolable.OnReturn();

			if (obj != null)
				this.pool.Add(obj);
		}
	}
}
