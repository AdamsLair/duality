using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Duality
{
	/// <summary>
	/// Flags a class, struct or field as inappropriate for serialization.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	public class DontSerializeAttribute : Attribute { }
}
