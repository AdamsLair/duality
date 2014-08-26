using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Duality.Cloning
{
	public interface ICloneBehavior
	{
		CloneMode Mode { get; }
		CloneFlags Flags { get; }
	}
}
