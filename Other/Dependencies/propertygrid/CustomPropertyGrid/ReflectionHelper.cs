using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace AdamsLair.PropertyGrid
{
	internal static class ReflectionHelper
	{
		public const BindingFlags BindInstanceAll = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
		public const BindingFlags BindStaticAll = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
		public const BindingFlags BindAll = BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

		public static object CreateInstanceOf(this Type instanceType, bool noConstructor = false)
		{
			try
			{
				if (instanceType == typeof(string))
					return "";
				else if (typeof(Array).IsAssignableFrom(instanceType) && instanceType.GetArrayRank() == 1)
					return Array.CreateInstance(instanceType.GetElementType(), 0);
				else if (noConstructor)
					return System.Runtime.Serialization.FormatterServices.GetUninitializedObject(instanceType);
				else
					return Activator.CreateInstance(instanceType, true);
			}
			catch (Exception)
			{
				return null;
			}
		}
		public static object GetDefaultInstanceOf(this Type instanceType)
		{
			if (instanceType.IsValueType)
				return Activator.CreateInstance(instanceType, true);
			else
				return null;
		}

		public static bool MemberInfoEquals(MemberInfo lhs, MemberInfo rhs)
		{
			if (lhs == rhs)
				return true;
 
			if (lhs.DeclaringType != rhs.DeclaringType)
				return false;
 
			// Methods on arrays do not have metadata tokens but their ReflectedType
			// always equals their DeclaringType
			if (lhs.DeclaringType != null && lhs.DeclaringType.IsArray)
				return false;
 
			if (lhs.MetadataToken != rhs.MetadataToken || lhs.Module != rhs.Module)
				return false;
 
			if (lhs is MethodInfo)
			{
				MethodInfo lhsMethod = lhs as MethodInfo;
 
				if (lhsMethod.IsGenericMethod)
				{
					MethodInfo rhsMethod = rhs as MethodInfo;
 
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
		public static int GetTypeHierarchyLevel(this Type t)
		{
			int level = 0;
			while (t.BaseType != null) { t = t.BaseType; level++; }
			return level;
		}


		public static string GetTypeKeyword(this Type T)
		{
			return T.Name.Split(new char[] {'`'}, StringSplitOptions.RemoveEmptyEntries)[0].Replace('+', '.');
		}
		public static string GetTypeCSCodeName(this Type T, bool shortName = false)
		{
			StringBuilder typeStr = new StringBuilder();

			if (T.IsGenericParameter)
			{
				return T.Name;
			}
			if (T.IsArray)
			{
				typeStr.Append(GetTypeCSCodeName(T.GetElementType(), shortName));
				typeStr.Append('[');
				typeStr.Append(',', T.GetArrayRank() - 1);
				typeStr.Append(']');
			}
			else
			{
				Type[] genArgs = T.IsGenericType ? T.GetGenericArguments() : null;

				if (T.IsNested)
				{
					Type declType = T.DeclaringType;
					if (declType.IsGenericTypeDefinition)
					{
						Array.Resize(ref genArgs, declType.GetGenericArguments().Length);
						declType = declType.MakeGenericType(genArgs);
						genArgs = T.GetGenericArguments().Skip(genArgs.Length).ToArray();
					}
					string parentName = GetTypeCSCodeName(declType, shortName);

					string[] nestedNameToken = shortName ? T.Name.Split('+') : T.FullName.Split('+');
					string nestedName = nestedNameToken[nestedNameToken.Length - 1];
						
					int genTypeSepIndex = nestedName.IndexOf("[[");
					if (genTypeSepIndex != -1) nestedName = nestedName.Substring(0, genTypeSepIndex);
					genTypeSepIndex = nestedName.IndexOf('`');
					if (genTypeSepIndex != -1) nestedName = nestedName.Substring(0, genTypeSepIndex);

					typeStr.Append(parentName);
					typeStr.Append('.');
					typeStr.Append(nestedName);
				}
				else
				{
					if (shortName)
						typeStr.Append(T.Name.Split(new char[] {'`'}, StringSplitOptions.RemoveEmptyEntries)[0].Replace('+', '.'));
					else
						typeStr.Append(T.FullName.Split(new char[] {'`'}, StringSplitOptions.RemoveEmptyEntries)[0].Replace('+', '.'));
				}

				if (genArgs != null && genArgs.Length > 0)
				{
					if (T.IsGenericTypeDefinition)
					{
						typeStr.Append('<');
						typeStr.Append(',', genArgs.Length - 1);
						typeStr.Append('>');
					}
					else if (T.IsGenericType)
					{
						typeStr.Append('<');
						for (int i = 0; i < genArgs.Length; i++)
						{
							typeStr.Append(GetTypeCSCodeName(genArgs[i], shortName));
							if (i < genArgs.Length - 1)
								typeStr.Append(',');
						}
						typeStr.Append('>');
					}
				}
			}

			return typeStr.Replace('+', '.').ToString();
		}

		public static string[] SplitArgs(string argList, char pushLevel, char popLevel, char separator, int splitLevel)
		{
			if (argList == null) return new string[0];

			// Splitting the parameter list without destroying generic arguments
			int lastSplitIndex = -1;
			int genArgLevel = 0;
			List<string> ptm = new List<string>();
			for (int i = 0; i < argList.Length; i++)
			{
				if (argList[i] == separator && genArgLevel == splitLevel)
				{
					ptm.Add(argList.Substring(lastSplitIndex + 1, i - lastSplitIndex - 1));
					lastSplitIndex = i;
				}
				else if (argList[i] == pushLevel)
					genArgLevel++;
				else if (argList[i] == popLevel)
					genArgLevel--;
			}
			ptm.Add(argList.Substring(lastSplitIndex + 1, argList.Length - lastSplitIndex - 1));
			return ptm.ToArray();
		}
	}
}
