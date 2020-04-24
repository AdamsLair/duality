using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Duality.Utility.Pooling;

namespace Duality
{
	public class WaitFrames : WaitCondition<int>
	{
		public WaitFrames(int frames) : base(
			(i) => i - 1,
			(i) => i <= 0,
			frames
		)
		{ }
	}
}
