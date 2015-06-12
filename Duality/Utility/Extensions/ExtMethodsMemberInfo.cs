using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Duality
{
	public static class ExtMethodsMemberInfo
	{
		/// <summary>
		/// Determines whether two <see cref="MemberInfo"/> instances refer to the same member,
		/// regardless of the context in which each instance was obtained.
		/// </summary>
		/// <param name="first"></param>
		/// <param name="second"></param>
		/// <returns></returns>
		public static bool IsEquivalent(this MemberInfo first, MemberInfo second)
		{
			if (first == second)
				return true;
 
			if (first.DeclaringType != second.DeclaringType)
				return false;
 
			// Methods on arrays do not have metadata tokens but their ReflectedType
			// always equals their DeclaringType
			if (first.DeclaringType != null && first.DeclaringType.IsArray)
				return false;
 
			if (first.MetadataToken != second.MetadataToken || first.Module != second.Module)
				return false;
 
			if (first is MethodInfo)
			{
				MethodInfo lhsMethod = first as MethodInfo;
 
				if (lhsMethod.IsGenericMethod)
				{
					MethodInfo rhsMethod = second as MethodInfo;
 
					Type[] lhsGenArgs = lhsMethod.GetGenericArguments();
					Type[] rhsGenArgs = rhsMethod.GetGenericArguments();
					for (int i = 0; i < rhsGenArgs.Length; i++)
					{
						if (lhsGenArgs[i] != rhsGenArgs[i])
							return false;
					}
				}
			}
			return true;
		}
	}
}
