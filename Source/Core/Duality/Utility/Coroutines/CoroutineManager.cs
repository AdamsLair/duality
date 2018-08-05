﻿using System.Collections.Generic;

namespace Duality
{
	public class CoroutineManager
	{
		private static Queue<Coroutine> pool = new Queue<Coroutine>();
		private List<Coroutine> coroutines = new List<Coroutine>();
		private List<Coroutine> trashcan = new List<Coroutine>();

		public Coroutine StartNew(IEnumerable<CoroutineAction> method)
		{
			Coroutine coroutine = null;

			if (pool.Count > 0)
				coroutine = pool.Dequeue();
			else
				coroutine = new Coroutine();

			coroutine.Setup(method);
			this.coroutines.Add(coroutine);
			return coroutine;
		}

		public void Clear()
		{
			foreach (Coroutine c in this.coroutines)
				c.Cancel();

			this.coroutines.Clear();
		}
		public void Update()
		{
			foreach (Coroutine c in this.coroutines)
			{
				if (c.IsComplete)
					this.trashcan.Add(c);
				else
					c.Update();
			}

			foreach (Coroutine c in this.trashcan)
			{
				this.coroutines.Remove(c);
				pool.Enqueue(c);
			}

			this.trashcan.Clear();
		}
	}
}
