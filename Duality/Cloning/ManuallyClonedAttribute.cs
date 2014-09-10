using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Duality.Cloning
{
	/// <summary>
	/// Flags a field or class to be skipped by automatic cloning, because it has been handled by an <see cref="ICloneExplicit"/> implementation.
	/// Applying this attribute to a class has the same effect as applying it to all locally declared fields (without inheritance).
	/// </summary>
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public class ManuallyClonedAttribute : Attribute {}
}
