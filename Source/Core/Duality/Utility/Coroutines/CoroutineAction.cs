using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duality
{
	public interface ICoroutineAction
	{
		bool IsComplete { get; }
	}

	internal sealed class StopAction : ICoroutineAction
	{
		public static StopAction Value = new StopAction();
		public static IEnumerable<StopAction> Finalizer = new StopAction[] { StopAction.Value };

		private StopAction() { }

		public bool IsComplete
		{
			get { return true; }
		}
	}

	/// <summary>
	/// Waits a defined amount of time before continuing.
	/// </summary>
	public sealed class WaitForTime : ICoroutineAction
	{
		private float time;

		public WaitForTime(TimeSpan time)
		{
			this.time = (float)time.TotalSeconds;
		}

		public bool IsComplete
		{
			get { this.time -= Time.DeltaTime; return this.time <= float.Epsilon; }
		}
	}

	/// <summary>
	/// Waits a defined amount of frames before continuing.
	/// </summary>
	public sealed class WaitForFrames : ICoroutineAction
	{
		private int frames;
		public WaitForFrames(int frames)
		{
			this.frames = frames;
		}

		public bool IsComplete
		{
			get { this.frames--; return this.frames <= 0; }
		}
	}

	/// <summary>
	/// Waits for a signal to be set before continuing.
	/// </summary>
	public sealed class WaitForSignal : ICoroutineAction
	{
		private string signal;
		public WaitForSignal(string signal)
		{
			this.signal = signal;
		}

		public bool IsComplete
		{
			get { return CoroutineManager.IsSet(this.signal); }
		}
	}

	/// <summary>
	/// Consumes a signal before continuing (no priority is guaranteed in case multiple coroutines are waiting for the same signal).
	/// </summary>
	public sealed class ConsumeSignal : ICoroutineAction
	{
		private string signal;
		public ConsumeSignal(string signal)
		{
			this.signal = signal;
		}

		public bool IsComplete
		{
			get { return CoroutineManager.ConsumeSignal(this.signal); }
		}
	}

	/// <summary>
	/// Emits a signal. Can be stopped until the signal can actually be emitted.
	/// </summary>
	public sealed class EmitSignal : ICoroutineAction
	{
		private string signal;
		private bool continueIfCantEmit;

		public EmitSignal(string signal, bool waitUntilCanEmit = false)
		{
			this.signal = signal;
			this.continueIfCantEmit = !waitUntilCanEmit;
		}

		public bool IsComplete
		{
			get { return CoroutineManager.EmitSignal(this.signal) || this.continueIfCantEmit; }
		}
	}

	/// <summary>
	/// Waits until at least one condition is satisfied.
	/// </summary>
	public sealed class WaitOne : ICoroutineAction
	{
		IEnumerable<ICoroutineAction> conditions;
		public WaitOne(params ICoroutineAction[] conditions)
		{
			this.conditions = conditions;
		}

		public bool IsComplete
		{
			get
			{
				bool result = false;
				foreach (ICoroutineAction c in this.conditions)
					result |= c.IsComplete;

				return result;
			}
		}
	}

	/// <summary>
	/// Waits until all conditions are satisfied.
	/// </summary>
	public sealed class WaitAll : ICoroutineAction
	{
		IEnumerable<ICoroutineAction> conditions;
		public WaitAll(params ICoroutineAction[] conditions)
		{
			this.conditions = conditions;
		}

		public bool IsComplete
		{
			get
			{
				bool result = true;
				foreach (ICoroutineAction c in this.conditions)
					result &= c.IsComplete;

				return result;
			}
		}
	}

	/// <summary>
	/// Calls a method every frame, until said action returns true, signalling its completion
	/// </summary>
	public sealed class ContinuousAction : ICoroutineAction
	{
		private Func<bool> action;
		public ContinuousAction(Func<bool> action)
		{
			this.action = action;
		}

		public bool IsComplete
		{
			get { return this.action(); }
		}
	}

	/// <summary>
	/// Launches a parallel task and waits for its completion.
	/// Usual threading limitations (no Texture creation, etc.) apply
	/// </summary>
	
	/* Commented until safely useable
	public sealed class LongRunningTask : ICoroutineAction
	{
		private Task task;
		public LongRunningTask(Task task)
		{
			this.task = task;
		}

		public bool IsComplete
		{
			get
			{
				if(this.task.Status == TaskStatus.Created)
					this.task.Start();

				return this.task.IsCompleted || this.task.IsCanceled || this.task.IsFaulted;
			}
		}
	}
	*/
}
