using System;
using System.Collections.Generic;
using System.Linq;

namespace Duality.IO
{
	/// <summary>
	/// Determines equality of path strings.
	/// </summary>
	/// <seealso cref="Duality.IO.PathOp"/>
	public class PathEqualityComparer : IEqualityComparer<string>
	{
		public bool Equals(string x, string y)
		{
			return PathOp.ArePathsEqual(x, y);
		}
		public int GetHashCode(string obj)
		{
			if (string.IsNullOrWhiteSpace(obj))
				return 0;
			else
				return PathOp.GetFullPath(obj).GetHashCode();
		}
	}
}
