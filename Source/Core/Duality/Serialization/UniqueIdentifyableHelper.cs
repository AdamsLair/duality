using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Duality.Serialization
{
	/// <summary>
	/// A static class that provides helper function related to generating serialization IDs
	/// for interface implementations of <see cref="IUniqueIdentifyable"/>.
	/// </summary>
	public static class UniqueIdentifyableHelper
	{
		/// <summary>
		/// Creates a unique identifier based on the specified string.
		/// It is guaranteed to remain the same across .NET runtimes, libraries and platforms.
		/// </summary>
		/// <param name="value"></param>
		public static int GetIdentifier(string value)
		{
			// Based on https://stackoverflow.com/a/2351171
			int result = 0;
			unchecked
			{
				for (int i = 0; i < value.Length; i++)
				{
					result = value[i] + 37 * result;
				}
			}
			return result;
		}
		/// <summary>
		/// Creates a unique identifier based on the specified <see cref="Guid"/>.
		/// It is guaranteed to remain the same across .NET runtimes, libraries and platforms.
		/// </summary>
		/// <param name="guid"></param>
		public static int GetIdentifier(Guid guid)
		{
			// Based on https://stackoverflow.com/a/2351171
			int result = 0;
			unchecked
			{
				byte[] data = guid.ToByteArray();
				for (int i = 0; i < data.Length; i++)
				{
					result = data[i] + 37 * result;
				}
			}
			return result;
		}
	}
}
