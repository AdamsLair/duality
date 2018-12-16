using System;
using System.Collections.Generic;
using Duality.Utility.Pooling;

namespace Duality
{
	public class CoroutineManager
	{
		private static QueuedPool<Coroutine> pool = new QueuedPool<Coroutine>();
		private List<Coroutine> coroutines = new List<Coroutine>();
		private List<Coroutine> scheduled = new List<Coroutine>();
		private List<Coroutine> trashcan = new List<Coroutine>();
		private Dictionary<Coroutine, Exception> lastFrameErrors = new Dictionary<Coroutine, Exception>();

		public Coroutine StartNew(IEnumerator<IWaitCondition> enumerator, string name)
		{
			Coroutine coroutine = pool.GetOne();
			coroutine.Setup(enumerator, name);

			this.scheduled.Add(coroutine);
			return coroutine;
		}

		public void Clear()
		{
			foreach (Coroutine c in this.coroutines)
				c.Cancel();

			foreach (Coroutine c in this.coroutines)
				c.Cancel();

			this.scheduled.Clear();
			this.coroutines.Clear();
		}

		public void Update()
		{
			this.lastFrameErrors.Clear();

			this.coroutines.AddRange(this.scheduled);
			this.scheduled.Clear();

			foreach (Coroutine c in this.coroutines)
			{
				c.Update();

				if (c.Status == CoroutineStatus.Error)
					this.lastFrameErrors.Add(c, c.LastException);
				if(c.Status != CoroutineStatus.Running && c.Status != CoroutineStatus.Paused)
					this.trashcan.Add(c);
			}

			foreach (Coroutine c in this.trashcan)
			{
				this.coroutines.Remove(c);
				pool.ReturnOne(c);
			}

			this.trashcan.Clear();
		}
	}
}
