using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Duality.Utility.Pooling;

namespace Duality
{
	public class WaitTime : WaitCondition<float>
	{
		public WaitTime(float seconds) : base(
			(ts) => ts - Time.DeltaTime,
			(ts) => ts <= float.Epsilon,
			(float)seconds
		)
		{ }

		public WaitTime(TimeSpan time) : this((float)time.TotalSeconds)
		{ }
	}
}
