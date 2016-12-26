using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Duality.Resources;
using Duality.Cloning;
using Duality.Serialization;
using Duality.Editor;
using Duality.Properties;

namespace Duality
{
	public class ComponentTypeComparer : IEqualityComparer<Component>
	{
		public static readonly ComponentTypeComparer Default = new ComponentTypeComparer();

		public bool Equals(Component x, Component y)
		{
			if (x == y) return true;
			if (x == null || y == null) return false;
			return x.GetType() == y.GetType();
		}
		public int GetHashCode(Component obj)
		{
			return obj != null ? obj.GetType().GetHashCode() : 0;
		}
	}
}
