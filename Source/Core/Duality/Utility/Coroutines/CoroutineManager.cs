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
		private readonly Queue<Coroutine> coroutines = new Queue<Coroutine>(256);
		private readonly Queue<Coroutine> nextCycle = new Queue<Coroutine>(256);
		private readonly Queue<Coroutine> scheduled = new Queue<Coroutine>();
		private readonly Queue<Coroutine> trashcan = new Queue<Coroutine>();

		private readonly Dictionary<Coroutine, Exception> lastFrameErrors = new Dictionary<Coroutine, Exception>();

		private int count;

		/// <summary>
		/// Returns an IEnumerable of all currently active and scheduled Coroutines
		/// </summary>
		public IEnumerable<Coroutine> Coroutines
		{
			get { return this.coroutines.Concat(this.scheduled); }
		}

		/// <summary>
		/// Returns an IEnumerable of the Exception encountered in the last cycle, with the relative Coroutine
		/// </summary>
		public IEnumerable<KeyValuePair<Coroutine, Exception>> LastFrameErrors
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

			this.scheduled.Enqueue(coroutine);
			return coroutine;
		}

		/// <summary>
		/// Returns true if the Coroutine is currently in execution or scheduled for execution in the next cycle
		/// </summary>
		/// <param name="c">The Coroutine</param>
		/// <returns>True if found</returns>
		public bool IsCoroutineListed(Coroutine c)
		{
			return this.Coroutines.Contains(c);
		}

		/// <summary>
		/// Cancels all currently active and scheduled Coroutines
		/// </summary>
		public void Clear()
		{
			foreach (Coroutine c in this.coroutines)
				c.Cancel();

			this.scheduled.Clear();
			this.coroutines.Clear();
		}

		internal void Update()
		{
			this.lastFrameErrors.Clear();

			// add any newly scheduled coroutine to the main queue
			this.count = this.scheduled.Count;
			for (int i = 0; i < this.count; i++)
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
				this.pool.Enqueue(this.trashcan.Dequeue());
		}
	}
}
