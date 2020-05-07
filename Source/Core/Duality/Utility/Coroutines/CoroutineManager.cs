using System;
using System.Collections.Generic;
using System.Linq;

namespace Duality.Utility.Coroutines
{
	/// <summary>
	/// The Coroutine Manager
	/// </summary>
	public class CoroutineManager
	{
		private readonly Queue<Coroutine> pool = new Queue<Coroutine>(64);
		
		private Queue<Coroutine> currentCycle = new Queue<Coroutine>(256);
		private Queue<Coroutine> nextCycle = new Queue<Coroutine>(256);

		private readonly Dictionary<Coroutine, Exception> lastFrameErrors = new Dictionary<Coroutine, Exception>();

		/// <summary>
		/// Returns an IEnumerable of all currently active and scheduled Coroutines
		/// </summary>
		public IEnumerable<Coroutine> Coroutines
		{
			get { return this.currentCycle.Concat(this.nextCycle); }
		}

		/// <summary>
		/// Returns an IEnumerable of the Exception encountered in the last cycle, with the relative Coroutine
		/// </summary>
		public IReadOnlyDictionary<Coroutine, Exception> LastFrameErrors
		{
			get { return this.lastFrameErrors; }
		}

		/// <summary>
		/// Prepares a new Coroutine and places it in the scheduled queue, to be started in the next cycle
		/// </summary>
		/// <param name="enumerator">The Coroutine's execution body</param>
		/// <param name="name">The name of the Coroutine</param>
		/// <returns>The prepared Coroutine</returns>
		internal Coroutine StartNew(IEnumerator<WaitUntil> enumerator, string name)
		{
			Coroutine coroutine;
			if (this.pool.Count > 0)
				coroutine = this.pool.Dequeue();
			else
				coroutine = new Coroutine();

			coroutine.Setup(enumerator, name);

			this.nextCycle.Enqueue(coroutine);
			return coroutine;
		}

		/// <summary>
		/// Cancels all currently active and scheduled Coroutines
		/// </summary>
		public void Clear()
		{
			foreach (Coroutine c in this.currentCycle)
				c.Cancel();

			this.nextCycle.Clear();
			this.currentCycle.Clear();
		}

		internal void Update()
		{
			this.lastFrameErrors.Clear();

			// swap around the queues
			Queue<Coroutine> swap = this.currentCycle;
			this.currentCycle = this.nextCycle;
			this.nextCycle = swap;

			int count = this.currentCycle.Count;
			for (int i = 0; i < count; i++)
			{
				Coroutine c = this.currentCycle.Dequeue();
				c.Update();

				if (c.Status == CoroutineStatus.Error)
					this.lastFrameErrors.Add(c, c.LastException);

				if (c.Status == CoroutineStatus.Running || c.Status == CoroutineStatus.Paused)
					this.nextCycle.Enqueue(c);
				else
					this.pool.Enqueue(c);
			}
		}
	}
}
