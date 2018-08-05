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
		/// <param name="method"></param>
		/// <returns></returns>
		public static Coroutine Start(Scene scene, IEnumerable<CoroutineAction> method)
		{
			return scene.Coroutines.StartNew(method);
		}


		private IEnumerator<CoroutineAction> enumerator = null;


		/// <summary>
		/// Returns true if the Current element in the Coroutine's enumerator is StopAction.Value 
		/// (automatically added at the end of the Coroutine itself)
		/// </summary>
		public bool IsComplete
		{
			get { return this.Current == StopAction.Value; }
		}
		internal IEnumerator<CoroutineAction> Enumerator
		{
			get { return this.enumerator; }
		}
		internal CoroutineAction Current
		{
			get { return this.enumerator.Current; }
		}


		internal void Setup(IEnumerable<CoroutineAction> values)
		{
			if (this.enumerator != null)
				this.enumerator.Dispose();

			this.enumerator = values.Concat(StopAction.Finalizer).GetEnumerator();
			this.enumerator.MoveNext();
		}
		/// <summary>
		/// Restarts the coroutine by moving the enumerator back to the first element.
		/// </summary>
		internal void Restart()
		{
			this.enumerator.Reset();
			this.enumerator.MoveNext();
		}

		/// <summary>
		/// Cancels the coroutine without executing any further code.
		/// </summary>
		public void Cancel()
		{
			this.enumerator.Dispose();
			this.enumerator = StopAction.Finalizer.GetEnumerator();
			this.enumerator.MoveNext();
		}

		public void Dispose()
		{
			this.enumerator.Dispose();
		}
	}
}