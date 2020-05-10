using System;
using System.Collections.Generic;
using System.Linq;
using Duality.Resources;

namespace Duality.Utility.Coroutines
{
	/// <summary>
	/// A Coroutine's management class
	/// </summary>
	public sealed class Coroutine
	{
		private IEnumerator<WaitUntil> enumerator = null;
		private WaitUntil currentCondition;

		/// <summary>
		/// The Coroutine's current <see cref="CoroutineStatus">execution status</see>
		/// </summary>
		public CoroutineStatus Status { get; private set; }
		/// <summary>
		/// The Exception encountered during the last frame
		/// </summary>
		public Exception LastException { get; private set; }
		/// <summary>
		/// The Coroutine's Name
		/// </summary>
		public string Name { get; private set; }
		/// <summary>
		/// True if this Coroutine's Status is <see cref="CoroutineStatus.Paused"/> or <see cref="CoroutineStatus.Running"/>
		/// </summary>
		public bool IsAlive
		{
			get { return this.Status == CoroutineStatus.Paused || this.Status == CoroutineStatus.Running; }
		}

		internal void Setup(IEnumerable<WaitUntil> values, string name)
		{
			this.Status = CoroutineStatus.Running;

			this.LastException = null;
			this.Name = name;

			if (this.enumerator != null)
				this.enumerator.Dispose();

			this.enumerator = values.GetEnumerator();
		}
		internal void Update()
		{
			if (this.Status != CoroutineStatus.Running) return;

			try
			{
				this.currentCondition.Update();
				if (this.currentCondition.IsComplete)
				{
					if (this.enumerator.MoveNext())
						this.currentCondition = this.enumerator.Current;
					else
						this.Status = CoroutineStatus.Complete;
				}
			}
			catch (Exception e)
			{
				Logs.Core.WriteError("An error occurred while processing Coroutine '{0}': {1}", this.Name, LogFormat.Exception(e));

				this.LastException = e;
				this.Status = CoroutineStatus.Error;
			}
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
	}
}
