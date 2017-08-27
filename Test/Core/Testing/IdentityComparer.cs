using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

using Duality;

using NUnit.Framework;

namespace Duality.Tests
{
	/// <summary>
	/// Compares two objects or values for their identity. For reference types,
	/// reference equality is checked. For value types, a fallback to their
	/// default equality comparer is used.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class IdentityComparer<T> : IEqualityComparer<T>
	{
		public bool Equals(T x, T y)
		{
			if (typeof(T).IsValueType)
				return EqualityComparer<T>.Default.Equals(x, y);
			else
				return RuntimeHelpers.Equals(x, y);
		}
		public int GetHashCode(T obj)
		{
			if (typeof(T).IsValueType)
				return EqualityComparer<T>.Default.GetHashCode(obj);
			else
				return RuntimeHelpers.GetHashCode(obj);
		}
	}
}
