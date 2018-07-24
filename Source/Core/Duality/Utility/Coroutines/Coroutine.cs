using System;
using System.Collections.Generic;
using System.Linq;
using Duality.Resources;

namespace Duality
{
	public sealed class Coroutine : IDisposable
	{
		public static Coroutine Start(Scene scene, IEnumerable<CoroutineAction> actions)
		{
			return scene.RegisterCoroutine(actions);
		}

		internal IEnumerator<CoroutineAction> Enumerator { get; private set; }

		internal CoroutineAction Current
		{
			get { return this.Enumerator.Current; }
		}

		public bool IsComplete
		{
			get { return this.Current == StopAction.Value; }
		}

		internal void Setup (IEnumerable<CoroutineAction> values)
		{
			if (this.Enumerator != null)
				this.Enumerator.Dispose();

			this.Enumerator = values.Concat(StopAction.Finalizer).GetEnumerator();
			this.Enumerator.MoveNext();
		}

		internal void Restart()
		{
			this.Enumerator.Reset();
			this.Enumerator.MoveNext();
		}

		public void Abort()
		{
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