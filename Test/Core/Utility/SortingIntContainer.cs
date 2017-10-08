using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

using Duality;

using NUnit.Framework;

namespace Duality.Tests.Utility
{
	internal class SortingIntContainer : IEquatable<SortingIntContainer>, IComparable<SortingIntContainer>
	{
		public int Value = 0;

		public SortingIntContainer(int value)
		{
			this.Value = value;
		}

		public bool Equals(SortingIntContainer other)
		{
			return other.Value == this.Value;
		}
		public int CompareTo(SortingIntContainer other)
		{
			return this.Value.CompareTo(other.Value);
		}

		public override bool Equals(object obj)
		{
			if (obj is SortingIntContainer)
				return this.Equals((SortingIntContainer)obj);
			else
				return false;
		}
		public override int GetHashCode()
		{
			return this.Value;
		}
		public override string ToString()
		{
			return this.Value.ToString();
		}
	}
}
