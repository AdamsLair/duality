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
		public static Coroutine Start(Scene scene, IEnumerable<IWaitCondition> method, string name = null)
		{
			return scene.Coroutines.StartNew(method.GetEnumerator(), name);
		}

		/// <summary>
		/// Starts a coroutine and registers it in the scene's CoroutineManager
		/// </summary>
		/// <param name="scene"></param>
		/// <param name="method"></param>
		/// <returns></returns>
		public static Coroutine Start(Scene scene, IEnumerator<IWaitCondition> enumerator, string name = null)
		{
			return scene.Coroutines.StartNew(enumerator, name);
		}

		private IEnumerator<IWaitCondition> enumerator = null;

		/// <summary>
		/// Returns true if there are no more CoroutineActions to be performed
		/// </summary>
		public CoroutineStatus Status { get; private set; }

		public Exception LastException { get; private set; }

		private string name;

		internal void Setup(IEnumerator<IWaitCondition> values, string name)
		{
			this.Status = CoroutineStatus.Running;
			this.LastException = null;
			this.name = name;

			if (this.enumerator != null)
				this.enumerator.Dispose();

			this.enumerator = values;

			if (!this.enumerator.MoveNext())
				this.Status = CoroutineStatus.Complete;
		}

		internal void Update()
		{
			if (this.Status != CoroutineStatus.Running) return;

			try
			{
				IWaitCondition currentYield = this.enumerator.Current;
				if (currentYield == null || currentYield.Update())
				{
					if (!this.enumerator.MoveNext())
						this.Status = CoroutineStatus.Complete;
				}
			}
			catch(Exception e)
			{
				Logs.Core.WriteError("An error occurred while processing Coroutine '{0}': {1}", this.name, LogFormat.Exception(e));

				this.LastException = e;
				this.Status = CoroutineStatus.Error;
			}
		}
		/// <summary>
		/// Restarts the coroutine by moving the enumerator back to the first element.
		/// </summary>
		internal void Restart()
		{
			this.enumerator.Reset();
			this.enumerator.MoveNext();
		}

		public void Pause()
		{
			if (this.Status == CoroutineStatus.Running)
				this.Status = CoroutineStatus.Paused;
		}

		public void Resume()
		{
			if (this.Status == CoroutineStatus.Paused)
				this.Status = CoroutineStatus.Running;
		}

		/// <summary>
		/// Cancels the coroutine without executing any further code.
		/// </summary>
		public void Cancel()
		{
			this.enumerator.Dispose();
			this.Status = CoroutineStatus.Cancelled;
		}

		public void Dispose()
		{
			this.enumerator.Dispose();
			this.Status = CoroutineStatus.Disposed;
		}
	}
}