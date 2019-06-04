using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Duality.Utility.Pooling;

namespace Duality
{
	public interface IWaitCondition
	{
		bool Update();
	}

	public class WaitCondition : IWaitCondition
	{
		private Func<bool> condition;

		public WaitCondition(Func<bool> condition)
		{
			this.condition = condition;
		}

		public bool Update()
		{
			return this.condition();
		}
	}

	public class WaitCondition<T> : IWaitCondition
	{
		private readonly Func<T, T> update;
		private readonly Func<T, bool> condition;
		private T parameter;

		public WaitCondition(Func<T, T> update, Func<T, bool> condition, T startingValue)
		{
			this.update = update;
			this.condition = condition;
			this.parameter = startingValue;
		}

		public bool Update()
		{
			this.parameter = this.update(this.parameter);
			return this.condition(this.parameter);
		}
	}
}
