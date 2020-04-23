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
		private readonly Func<bool> exitCondition;

		public WaitCondition(Func<bool> condition)
		{
			this.exitCondition = condition;
		}

		public bool Update()
		{
			return this.exitCondition();
		}
	}

	public class WaitCondition<T> : IWaitCondition
	{
		private readonly Func<T, T> update;
		private readonly Func<T, bool> exitCondition;
		private readonly T startingValue;
		private T currentValue;

		public WaitCondition(Func<T, T> update, Func<T, bool> exitCondition, T startingValue)
		{
			this.update = update;
			this.exitCondition = exitCondition;
			this.currentValue = this.startingValue = startingValue;
		}

		public IWaitCondition Reset()
		{
			this.currentValue = this.startingValue;
			return this;
		}

		public bool Update()
		{
			this.currentValue = this.update(this.currentValue);
			return this.exitCondition(this.currentValue);
		}
	}
}
