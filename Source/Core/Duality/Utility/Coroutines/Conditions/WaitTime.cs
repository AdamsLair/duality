using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Duality.Utility.Pooling;

namespace Duality
{
	public class WaitTime : WaitCondition<float>
	{
		public WaitTime(TimeSpan time) : base(
			(ts) => ts - Time.DeltaTime, 
			(ts) => ts <= float.Epsilon, 
			(float)time.TotalSeconds
		) { }
	}
}
