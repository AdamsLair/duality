using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Duality.Tests.Serialization
{
	public class ReferenceType
	{
		public int Value { get; set; }

		public ReferenceType(int value)
		{
			this.Value = value;
		}
	}
}
