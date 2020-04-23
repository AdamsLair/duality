using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Duality.Utility.Pooling
{
	public interface IPoolable
	{
		void OnPickup();
		void OnReturn();
	}
}
