using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Duality
{
	public static class ExtMethodsTypeInfo
	{
		/// <summary>
		/// Returns whether the specified object is an instance of the specified TypeInfo.
		/// </summary>
		public static bool IsInstanceOfType(this TypeInfo type, object instance)
		{
			if (instance == null) return false;
			return type.IsAssignableFrom(instance.GetType().GetTypeInfo());
		}
		/// <summary>
		/// Returns a TypeInfos BaseType as a TypeInfo, or null if it was null.
		/// </summary>
		/// <param name="type"></param>
		public static TypeInfo GetBaseTypeInfo(this TypeInfo type)
		{
			return type.BaseType != null ? type.BaseType.GetTypeInfo() : null;
		}
		/// <summary>
		/// Returns a Types inheritance level. The <c>object</c>-Type has an inheritance level of
		/// zero, each subsequent inheritance increases it by one.
		/// </summary>
		public static int GetInheritanceDepth(this TypeInfo type)
		{
			int level = 0;
			while (type.BaseType != null)
			{
				type = type.BaseType.GetTypeInfo();
				level++;
			}
			return level;
		}

		/// <summary>
		/// Returns all fields that are declared within this Type, or any of its base Types.
		/// Includes public, non-public, static and instance fields.
		/// </summary>
		public static IEnumerable<FieldInfo> DeclaredFieldsDeep(this TypeInfo type)
		{
			IEnumerable<FieldInfo> result = Enumerable.Empty<FieldInfo>();

			while (type != null)
			{
				result = result.Concat(type.DeclaredFields);
				type = type.GetBaseTypeInfo();
			}

			return result;
		}
		/// <summary>
		/// Returns all properties that are declared within this Type, or any of its base Types.
		/// Includes public, non-public, static and instance properties.
		/// </summary>
		public static IEnumerable<PropertyInfo> DeclaredPropertiesDeep(this TypeInfo type)
		{
			IEnumerable<PropertyInfo> result = Enumerable.Empty<PropertyInfo>();

			while (type != null)
			{
				result = result.Concat(type.DeclaredProperties);
				type = type.GetBaseTypeInfo();
			}

			return result;
		}
		/// <summary>
		/// Returns all members that are declared within this Type, or any of its base Types.
		/// Includes public, non-public, static and instance fields.
		/// </summary>
		public static IEnumerable<MemberInfo> DeclaredMembersDeep(this TypeInfo type)
		{
			IEnumerable<MemberInfo> result = Enumerable.Empty<MemberInfo>();

			while (type != null)
			{
				result = result.Concat(type.DeclaredMembers);
				type = type.GetBaseTypeInfo();
			}

			return result;
		}
	}
}
