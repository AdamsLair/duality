using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Duality.Serialization
{
	public static class UniqueIdentifyableHelper
	{
		// Based on https://stackoverflow.com/a/2351171
		private const int Multipler = 37;
		public static int GetIdentifier(string value)
		{
			int result = 0;
			unchecked { foreach (char c in value) result = (Multipler * result) + c; }
			return result;
		}

		public static int GetIdentifier(Guid guid)
		{
			int result = 0;
			unchecked { foreach (byte b in guid.ToByteArray()) result = (Multipler * result) + b; }
			return result;
		}
	}
}
