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
		private const uint Multipler = 37;
		public static uint GetIdentifier(string value)
		{
			uint result = 0;
			foreach (char c in value) result = (Multipler * result) + c;
			return result;
		}

		public static uint GetIdentifier(Guid guid)
		{
			uint result = 0;
			foreach (byte b in guid.ToByteArray()) result = (Multipler * result) + b;
			return result;
		}
	}
}
