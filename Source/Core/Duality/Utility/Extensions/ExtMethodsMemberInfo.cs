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
		public static bool IsEquivalent(this MemberInfo first, MemberInfo second)
		{
			if (first == second)
				return true;

			// Early-out the basic stuff
			if (first.DeclaringType != second.DeclaringType)
				return false;
			if (first.Module != second.Module)
				return false;
			if (first.Name != second.Name)
				return false;

			// Check if they're both the same kind of member
			if (first is FieldInfo)
			{
				if (!(second is FieldInfo))
					return false;
			}
			else if (first is PropertyInfo)
			{
				if (!(second is PropertyInfo))
					return false;
			}
			else if (first is MethodInfo)
			{
				if (!(second is MethodInfo))
					return false;
			}
			else if (first is ConstructorInfo)
			{
				if (!(second is ConstructorInfo))
					return false;
			}
			else if (first is EventInfo)
			{
				if (!(second is EventInfo))
					return false;
			}
			else if (first is TypeInfo)
			{
				if (!(second is TypeInfo))
					return false;
			}

			if (first is MethodBase)
			{
				MethodBase firstMethod = first as MethodBase;
				MethodBase secondMethod = second as MethodBase;

				// If its a generic method, check its generic Type parameters
				if (firstMethod.IsGenericMethod)
				{
					if (!secondMethod.IsGenericMethod)
						return false;

					Type[] firstGenArgs = firstMethod.GetGenericArguments();
					Type[] secondGenArgs = secondMethod.GetGenericArguments();
					for (int i = 0; i < secondGenArgs.Length; i++)
					{
						if (firstGenArgs[i] != secondGenArgs[i])
							return false;
					}
				}

				// Check its parameters
				ParameterInfo[] firstParams = firstMethod.GetParameters();
				ParameterInfo[] secondParams = secondMethod.GetParameters();
				if (firstParams.Length != secondParams.Length)
					return false;

				for (int i = 0; i < firstParams.Length; i++)
				{
					if (firstParams[i].ParameterType != secondParams[i].ParameterType)
						return false;
					if (firstParams[i].IsOut != secondParams[i].IsOut)
						return false;
					if (firstParams[i].IsIn != secondParams[i].IsIn)
						return false;
					if (firstParams[i].IsRetval != secondParams[i].IsRetval)
						return false;
				}
			}

			return true;
		}
	}
}
