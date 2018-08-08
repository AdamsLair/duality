using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Duality.Utility.Pooling;

namespace Duality
{
	public abstract class CoroutineAction : IPoolable
	{
		private static AbstractPool<CoroutineAction> _pool;

		public static T GetOne<T>() where T : CoroutineAction, new()
		{
			return _pool.GetOne<T>();
		}

		public static void ReturnOne(CoroutineAction action)
		{
			_pool.ReturnOne(action);
		}

		public abstract bool IsComplete();

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

		public override bool IsComplete()
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

		public override bool IsComplete()
		{
			this.frames--;
			return this.frames <= 0;
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

		public override bool IsComplete()
		{
			bool result = false;
			foreach (CoroutineAction c in this.conditions)
				result |= c.IsComplete();

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

		public override bool IsComplete()
		{
			bool result = true;
			foreach (CoroutineAction c in this.conditions)
				result &= c.IsComplete();

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

		public override bool IsComplete()
		{
			return this.action();
		}
	}
}
