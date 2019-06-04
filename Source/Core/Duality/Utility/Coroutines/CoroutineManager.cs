using System;
using System.Collections.Generic;
using System.Linq;
using Duality.Utility.Pooling;

namespace Duality
{
	public class CoroutineManager
	{
		private static QueuedPool<Coroutine> pool = new QueuedPool<Coroutine>();
		private readonly List<Coroutine> coroutines = new List<Coroutine>();
		private readonly List<Coroutine> scheduled = new List<Coroutine>();
		private readonly List<Coroutine> trashcan = new List<Coroutine>();
		private readonly Dictionary<Coroutine, Exception> lastFrameErrors = new Dictionary<Coroutine, Exception>();

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

			this.scheduled.Add(coroutine);
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

			this.coroutines.AddRange(this.scheduled);
			this.scheduled.Clear();

			foreach (Coroutine c in this.coroutines)
			{
				c.Update();

				if (c.Status == CoroutineStatus.Error)
					this.lastFrameErrors.Add(c, c.LastException);
				if (c.Status != CoroutineStatus.Running && c.Status != CoroutineStatus.Paused)
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
