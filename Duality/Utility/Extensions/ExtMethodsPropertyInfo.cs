using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Duality
{
	public static class ExtMethodsPropertyInfo
	{
		public static bool IsPublic(this PropertyInfo property)
		{
			return 
				(property.CanRead && property.GetGetMethod(true).IsPublic) || 
				(property.CanWrite && property.GetSetMethod(true).IsPublic);
		}
		public static bool IsStatic(this PropertyInfo property)
		{
			return 
				(property.CanRead && property.GetGetMethod(true).IsStatic) || 
				(property.CanWrite && property.GetSetMethod(true).IsStatic);
		}
	}
}
