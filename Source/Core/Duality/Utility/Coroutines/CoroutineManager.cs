using System;
using System.Collections.Generic;
using System.Linq;
using Duality.Utility.Pooling;

namespace Duality
{
	public class CoroutineManager
	{
		// shared pool between all CoroutineManagers (one per scene)
		private static QueuedPool<Coroutine> pool = new QueuedPool<Coroutine>();

		private readonly Queue<Coroutine> coroutines = new Queue<Coroutine>(256);
		private readonly Queue<Coroutine> nextCycle = new Queue<Coroutine>(256);
		private readonly Queue<Coroutine> scheduled = new Queue<Coroutine>();
		private readonly Queue<Coroutine> trashcan = new Queue<Coroutine>();

		private readonly Dictionary<Coroutine, Exception> lastFrameErrors = new Dictionary<Coroutine, Exception>();

        private int count;

		public IEnumerable<Coroutine> Coroutines
		{
			get { return this.coroutines.Concat(this.scheduled); }
		}

		public IEnumerable<KeyValuePair<Coroutine, Exception>> LastFrameErrors
		{
			get { return this.lastFrameErrors; }
		}

		public Coroutine StartNew(IEnumerator<IWaitCondition> enumerator, string name)
		{
			Coroutine coroutine = pool.GetOne();
			coroutine.Setup(enumerator, name);

			this.scheduled.Enqueue(coroutine);
			return coroutine;
		}

		public bool IsCoroutineListed(Coroutine c)
		{
			return this.Coroutines.Contains(c);
		}

		public void Clear()
		{
			foreach (Coroutine c in this.coroutines)
				c.Cancel();

			this.scheduled.Clear();
			this.coroutines.Clear();
		}

		public void Update()
		{
			this.lastFrameErrors.Clear();

			// add any newly scheduled coroutine to the main queue
			this.count = this.scheduled.Count;
			for(int i = 0; i < this.count; i++)
				this.coroutines.Enqueue(this.scheduled.Dequeue());

            this.count = this.coroutines.Count;
			for (int i = 0; i < this.count; i++)
			{
				Coroutine c = this.coroutines.Dequeue();
				c.Update();

				if (c.Status == CoroutineStatus.Error)
					this.lastFrameErrors.Add(c, c.LastException);

				if (c.Status == CoroutineStatus.Running || c.Status == CoroutineStatus.Paused)
					this.nextCycle.Enqueue(c);
				else
					this.trashcan.Enqueue(c);
			}

            // put back the coroutines that are still alive
            this.count = this.nextCycle.Count;
			for (int i = 0; i < this.count; i++)
				this.coroutines.Enqueue(this.nextCycle.Dequeue());

            // cleaning up trashcan
            this.count = this.trashcan.Count;
			for (int i = 0; i < this.count; i++)
				pool.ReturnOne(this.trashcan.Dequeue());
		}
	}
}
