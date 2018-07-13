using System;
using System.Collections.Generic;
using System.Linq;

namespace Duality
{
	public sealed class Coroutine : ICoroutineAction, IDisposable
	{
		public static Coroutine Start(IEnumerable<ICoroutineAction> coroutine)
		{
			Coroutine c = new Coroutine(coroutine);
			CoroutineManager.Register(c);
			return c;
		}

		internal IEnumerator<ICoroutineAction> Enumerator { get; private set; }

		internal ICoroutineAction Current
		{
			get { return this.Enumerator.Current; }
		}

		public bool IsComplete
		{
			get { return this.Current == StopAction.Value; }
		}

		private Coroutine(IEnumerable<ICoroutineAction> values)
		{
			this.Enumerator = values.Concat(StopAction.Finalizer).GetEnumerator();
		}

		internal void Restart()
		{
			this.Enumerator.Reset();
			this.Enumerator.MoveNext();
		}

		public void Abort()
		{
			Coroutine current = this.Enumerator.Current as Coroutine;

			if (current != null)
				current.Abort();

			this.Enumerator.Dispose();
			this.Enumerator = StopAction.Finalizer.GetEnumerator();
			this.Enumerator.MoveNext();
		}

		public void Dispose()
		{
			this.Enumerator.Dispose();
		}
	}
}