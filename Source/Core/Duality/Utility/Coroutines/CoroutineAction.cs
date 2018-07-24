using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duality
{
	public abstract class CoroutineAction
	{
		private static List<CoroutineAction> _pool = new List<CoroutineAction>();
		private static int _poolIndex = 0;
		private static int _searchIndex = 0;

		public static T GetOne<T>() where T : CoroutineAction, new()
		{
			T result = null;

			for(int i = _poolIndex; i < _pool.Count + _poolIndex; i++)
			{
				_searchIndex = i % _pool.Count;

				result = _pool[_searchIndex] as T;
				
				if(result != null)
				{
					_poolIndex = _searchIndex;
					_pool.Remove(result);

					break;
				}
			}

			if (result == null)
				result = new T();

			result.OnPickup();
			return result;
		}

		public static void ReturnOne(CoroutineAction action)
		{
			action.OnReturn();
			_pool.Add(action);
		}

		public abstract bool IsComplete(CoroutineManager manager);

		public virtual void OnPickup() { }
		public virtual void OnReturn() { }
	}

	public abstract class CoroutineAction<TConfig> : CoroutineAction
	{
		public abstract CoroutineAction Setup(TConfig param);
	}

	public abstract class CoroutineAction<TConfig1, TConfig2> : CoroutineAction
	{
		public abstract CoroutineAction Setup(TConfig1 param1, TConfig2 param2);
	}

	public abstract class CoroutineAction<TConfig1, TConfig2, TConfig3> : CoroutineAction
	{
		public abstract CoroutineAction Setup(TConfig1 param1, TConfig2 param2, TConfig3 param3);
	}

	public abstract class CoroutineAction<TConfig1, TConfig2, TConfig3, TConfig4> : CoroutineAction
	{
		public abstract CoroutineAction Setup(TConfig1 param1, TConfig2 param2, TConfig3 param3, TConfig4 param4);
	}

	public abstract class CoroutineAction<TConfig1, TConfig2, TConfig3, TConfig4, TConfig5> : CoroutineAction
	{
		public abstract CoroutineAction Setup(TConfig1 param1, TConfig2 param2, TConfig3 param3, TConfig4 param4, TConfig5 param5);
	}

	internal sealed class StopAction : CoroutineAction
	{
		public static StopAction Value = new StopAction();
		public static IEnumerable<StopAction> Finalizer = new StopAction[] { StopAction.Value };

		public override bool IsComplete(CoroutineManager manager)
		{
			return true;
		}
	}

	/// <summary>
	/// Waits a defined amount of time before continuing.
	/// </summary>
	public sealed class WaitForTime : CoroutineAction<TimeSpan>
	{
		private float time;

		public override CoroutineAction Setup(TimeSpan timeToWait)
		{
			this.time = (float)timeToWait.TotalSeconds;
			return this;
		}

		public override bool IsComplete(CoroutineManager manager)
		{
			this.time -= Time.DeltaTime;
			return this.time <= float.Epsilon;
		}
	}

	/// <summary>
	/// Waits a defined amount of frames before continuing.
	/// </summary>
	public sealed class WaitForFrames : CoroutineAction<int>
	{
		private int frames;

		public override CoroutineAction Setup(int framesToWait)
		{
			this.frames = framesToWait;
			return this;
		}

		public override bool IsComplete(CoroutineManager manager)
		{
			this.frames--;
			return this.frames <= 0;
		}
	}

	/// <summary>
	/// Waits for a signal to be set before continuing.
	/// </summary>
	public sealed class WaitForSignal : CoroutineAction<string>
	{
		private string signal;

		public override CoroutineAction Setup(string signalToWait)
		{
			this.signal = signalToWait;
			return this;
		}

		public override bool IsComplete(CoroutineManager manager)
		{
			return manager.IsSet(this.signal);
		}
	}

	/// <summary>
	/// Consumes a signal before continuing (no priority is guaranteed in case multiple coroutines are waiting for the same signal).
	/// </summary>
	public sealed class ConsumeSignal : CoroutineAction<string>
	{
		private string signal;

		public override CoroutineAction Setup(string signalToConsume)
		{
			this.signal = signalToConsume;
			return this;
		}

		public override bool IsComplete(CoroutineManager manager)
		{
			return manager.ConsumeSignal(this.signal);
		}
	}

	/// <summary>
	/// Emits a signal. Can be stopped until the signal can actually be emitted.
	/// </summary>
	public sealed class EmitSignal : CoroutineAction<string, bool>
	{
		private string signal;
		private bool continueIfCantEmit;

		public override CoroutineAction Setup(string signalToEmit, bool waitUntilCanEmit = false)
		{
			this.signal = signalToEmit;
			this.continueIfCantEmit = !waitUntilCanEmit;
			return this;
		}

		public override bool IsComplete(CoroutineManager manager)
		{
			return manager.EmitSignal(this.signal) || this.continueIfCantEmit;
		}
	}

	/// <summary>
	/// Waits until at least one condition is satisfied.
	/// </summary>
	public sealed class WaitOne : CoroutineAction<CoroutineAction[]>
	{
		IEnumerable<CoroutineAction> conditions;

		public override CoroutineAction Setup(params CoroutineAction[] conditions)
		{
			this.conditions = conditions;
			return this;
		}

		public CoroutineAction SetupAsParams(params CoroutineAction[] conditions)
		{
			return this.Setup(conditions);
		}

		public override bool IsComplete(CoroutineManager manager)
		{
			bool result = false;
			foreach (CoroutineAction c in this.conditions)
				result |= c.IsComplete(manager);

			return result;
		}

		public override void OnReturn()
		{
			base.OnReturn();
			foreach (CoroutineAction action in this.conditions)
				CoroutineAction.ReturnOne(action);
		}
	}

	/// <summary>
	/// Waits until all conditions are satisfied.
	/// </summary>
	public sealed class WaitAll : CoroutineAction<CoroutineAction[]>
	{
		IEnumerable<CoroutineAction> conditions;

		public override CoroutineAction Setup(CoroutineAction[] conditions)
		{
			this.conditions = conditions;
			return this;
		}

		public CoroutineAction SetupAsParams(params CoroutineAction[] conditions)
		{
			return this.Setup(conditions);
		}

		public override bool IsComplete(CoroutineManager manager)
		{
			bool result = true;
			foreach (CoroutineAction c in this.conditions)
				result &= c.IsComplete(manager);

			return result;
		}

		public override void OnReturn()
		{
			base.OnReturn();
			foreach (CoroutineAction action in this.conditions)
				CoroutineAction.ReturnOne(action);
		}
	}

	/// <summary>
	/// Calls a method every frame, until said action returns true, signalling its completion
	/// </summary>
	public sealed class ContinuousAction : CoroutineAction<Func<bool>>
	{
		private Func<bool> action;

		public override CoroutineAction Setup(Func<bool> action)
		{
			this.action = action;
			return this;
		}

		public override bool IsComplete(CoroutineManager manager)
		{
			return this.action();
		}
	}

	/// <summary>
	/// Launches a parallel task and waits for its completion.
	/// Usual threading limitations (no Texture creation, etc.) apply
	/// </summary>

	/* Commented until safely useable
	public sealed class LongRunningTask : CoroutineAction<Task>
	{
		private Task task;

		public override CoroutineAction Setup(Task task)
		{
			this.task = task;
			return this;
		}

		public override bool IsComplete(CoroutineManager manager)
		{
			if(this.task.Status == TaskStatus.Created)
				this.task.Start();

			return this.task.IsCompleted || this.task.IsCanceled || this.task.IsFaulted;
		}
	}
	*/
}
