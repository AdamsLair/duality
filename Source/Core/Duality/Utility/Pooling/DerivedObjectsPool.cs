using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Duality.Utility.Pooling
{
	internal class DerivedObjectsPool<T> where T : class
	{
		private List<T> pool = new List<T>();
		private int poolIndex = 0;
		private int searchIndex = 0;

		public V GetOne<V>() where V : class, T, new()
		{
			V result = null;

			for (int i = this.poolIndex; i < this.pool.Count + this.poolIndex; i++)
			{
				this.searchIndex = i % this.pool.Count;

				result = this.pool[this.searchIndex] as V;

				if (result != null)
				{
					this.poolIndex = this.searchIndex;
					this.pool.Remove(result);

					break;
				}
			}

			if (result == null)
				result = new V();

			if(result is IPoolable)
				(result as IPoolable).OnPickup();

			return result;
		}

		public void ReturnOne(T obj)
		{
			if (obj is IPoolable)
				(obj as IPoolable).OnReturn();

			if (obj != null)
				this.pool.Add(obj);
		}
	}
}
