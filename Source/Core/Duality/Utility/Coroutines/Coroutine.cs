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
		/// <param name="scene">The scene that will execute the coroutine</param>
		/// <param name="method">The coroutine implementation</param>
		/// <param name="name">The name of the coroutine, optional</param>
		/// <returns>The created corutine</returns>
		public static Coroutine Start(Scene scene, IEnumerable<IWaitCondition> method, string name = null)
		{
			return Start(scene, method.GetEnumerator(), name);
		}

		/// <summary>
		/// Starts a coroutine and registers it in the scene's CoroutineManager
		/// </summary>
		/// <param name="scene">The scene that will execute the coroutine</param>
		/// <param name="enumerator">The coroutine IWaitConditions enumerator</param>
		/// <param name="name">The name of the coroutine, optional</param>
		/// <returns>The created corutine</returns>
		public static Coroutine Start(Scene scene, IEnumerator<IWaitCondition> enumerator, string name = null)
		{
			return scene.CoroutineManager.StartNew(enumerator, name);
		}

		private IEnumerator<IWaitCondition> enumerator = null;

		public CoroutineStatus Status { get; private set; }

		public Exception LastException { get; private set; }

		public string Name { get; private set; }

		public object Tag { get; private set; }

		public bool IsAlive
		{
			get { return this.Status == CoroutineStatus.Paused || this.Status == CoroutineStatus.Running; }
		}

		internal void Setup(IEnumerator<IWaitCondition> values, string name)
		{
			this.Status = CoroutineStatus.Running;

			this.LastException = null;
			this.Name = name;

			if (this.enumerator != null)
				this.enumerator.Dispose();

			this.enumerator = values;

			if (!this.enumerator.MoveNext())
				this.Status = CoroutineStatus.Complete;
		}

		/// <summary>
		/// Updates the coroutine's current IWaitCondition and executes the coroutine's code until it finds the next one.
		/// </summary>
		/// <remarks>
		/// A null currentYield is ignored
		/// </remarks>
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
				Logs.Core.WriteError("An error occurred while processing Coroutine '{0}': {1}", this.Name, LogFormat.Exception(e));

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

		/// <summary>
		/// Puts the coroutine on hold, to be resumed or cancelled later
		/// </summary>
		public void Pause()
		{
			if (this.Status == CoroutineStatus.Running)
				this.Status = CoroutineStatus.Paused;
		}

		/// <summary>
		/// Resumes a Paused coroutine
		/// </summary>
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