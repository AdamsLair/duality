using System;
using System.Collections.Generic;
using System.Linq;
using Duality.Resources;

namespace Duality
{
	public sealed class Coroutine : IDisposable
	{
        /// <summary>
        /// Starts a coroutine and registers it in the scene's CoroutineManager
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="coroutine"></param>
        /// <returns></returns>
        public static Coroutine Start(Scene scene, IEnumerable<CoroutineAction> actions)
		{
			return scene.RegisterCoroutine(actions);
		}

		internal IEnumerator<CoroutineAction> Enumerator { get; private set; }

		internal CoroutineAction Current
		{
			get { return this.Enumerator.Current; }
		}

		/// <summary>
		/// Returns true if the Current element in the Coroutine's enumerator is StopAction.Value 
		/// (automatically added at the end of the Coroutine itself)
		/// </summary>
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

		/// <summary>
		/// Restarts the corouting by moving the enumerator back to the first element
		/// </summary>
		internal void Restart()
		{
			this.Enumerator.Reset();
			this.Enumerator.MoveNext();
		}

		/// <summary>
		/// Aborts the coroutine
		/// </summary>
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