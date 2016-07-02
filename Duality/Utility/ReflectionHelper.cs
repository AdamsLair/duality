using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq.Expressions;

using Duality.Serialization;

namespace Duality
{
	/// <summary>
	/// Provides reflection-related helper methods.
	/// </summary>
	public static class ReflectionHelper
	{
		private const char MemberTokenUndefined			= 'U';
		private const char MemberTokenTypeInfo			= 'T';
		private const char MemberTokenFieldInfo			= 'F';
		private const char MemberTokenPropertyInfo		= 'P';
		private const char MemberTokenEventInfo			= 'E';
		private const char MemberTokenMethodInfo		= 'M';
		private const char MemberTokenConstructorInfo	= 'C';

		private	static	Dictionary<string,Type>				typeResolveCache			= new Dictionary<string,Type>();
		private	static	Dictionary<string,MemberInfo>		memberResolveCache			= new Dictionary<string,MemberInfo>();
		private	static	Dictionary<TypeInfo,bool>			plainOldDataTypeCache		= new Dictionary<TypeInfo,bool>();
		private	static	Dictionary<MemberInfo,Attribute[]>	customMemberAttribCache		= new Dictionary<MemberInfo,Attribute[]>();
		private	static	Dictionary<KeyValuePair<Type,Type>,bool>	resRefCache			= new Dictionary<KeyValuePair<Type,Type>,bool>();

		/// <summary>
		/// Fired when automatically resolving a certain Type has failed. Allows any subscriber to provide a suitable match.
		/// </summary>
		public static event EventHandler<ResolveMemberEventArgs> TypeResolve	= null;
		/// <summary>
		/// Fired when automatically resolving a certain Member has failed. Allows any subscriber to provide a suitable match.
		/// </summary>
		public static event EventHandler<ResolveMemberEventArgs> MemberResolve	= null;

		/// <summary>
		/// Returns all custom attributes of the specified Type that are attached to the specified member.
		/// Inherites attributes are returned as well. This method is usually faster than comparable .Net methods,
		/// because it caches previous results internally.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="member"></param>
		/// <returns></returns>
		public static IEnumerable<T> GetAttributesCached<T>(this MemberInfo member) where T : Attribute
		{
			Attribute[] result;
			if (!customMemberAttribCache.TryGetValue(member, out result))
			{
				result = member.GetCustomAttributes(true).OfType<Attribute>().ToArray();

				// If it's a Type, also check implemented interfaces for (EditorHint) attributes
				if (member is TypeInfo)
				{
					TypeInfo typeInfo = member as TypeInfo;
					IEnumerable<Attribute> query = result;
					Type[] interfaces = typeInfo.ImplementedInterfaces.ToArray();
					if (interfaces.Length > 0)
					{
						bool addedAny = false;
						foreach (Type interfaceType in interfaces)
						{
							TypeInfo interfaceTypeInfo = interfaceType.GetTypeInfo();
							IEnumerable<Attribute> subQuery = GetAttributesCached<Editor.EditorHintAttribute>(interfaceTypeInfo);
							if (subQuery.Any())
							{
								query = query.Concat(subQuery);
								addedAny = true;
							}
						}
						if (addedAny)
						{
							result = query.Distinct().ToArray();
						}
					}
				}
				// If it's a member, check if it is an interface implementation and add their (EditorHint) attributes as well
				else
				{
					TypeInfo declaringTypeInfo = member.DeclaringType == null ? null : member.DeclaringType.GetTypeInfo();
					if (declaringTypeInfo != null && !declaringTypeInfo.IsInterface)
					{
						IEnumerable<Attribute> query = result;
						Type[] interfaces = declaringTypeInfo.ImplementedInterfaces.ToArray();
						if (interfaces.Length > 0)
						{
							bool addedAny = false;
							foreach (Type interfaceType in interfaces)
							{
								TypeInfo interfaceTypeInfo = interfaceType.GetTypeInfo();
								foreach (MemberInfo interfaceMemberInfo in interfaceTypeInfo.DeclaredMembersDeep())
								{
									if (interfaceMemberInfo.Name != member.Name) continue;
									IEnumerable<Attribute> subQuery = GetAttributesCached<Editor.EditorHintAttribute>(interfaceMemberInfo);
									if (subQuery.Any())
									{
										query = query.Concat(subQuery);
										addedAny = true;
									}
								}
							}
							if (addedAny)
							{
								result = query.Distinct().ToArray();
							}
						}
					}
				}

				// Mind the result for later. Don't do this twice.
				customMemberAttribCache[member] = result;
			}

			if (typeof(T) == typeof(Attribute))
				return result as IEnumerable<T>;
			else
				return result.OfType<T>();
		}
		/// <summary>
		/// Returns all custom attributes of the specified Type that are attached to the specified member.
		/// Inherites attributes are returned as well. This method is usually faster than comparable .Net methods, 
		/// because it caches previous results internally.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="member"></param>
		/// <returns></returns>
		public static bool HasAttributeCached<T>(this MemberInfo member) where T : Attribute
		{
			return GetAttributesCached<T>(member).Any();
		}

		/// <summary>
		/// Visits all fields of an object and all its sub-objects in order to find and / or adjust values of a certain Type. This is likely to be a very expensive operation.
		/// </summary>
		/// <typeparam name="T">The value type that is searched for.</typeparam>
		/// <param name="obj">The root object where the search begins.</param>
		/// <param name="visitor">An object field visitor. Returns a new value for the visited object.</param>
		/// <param name="stopAtTarget">If true, visited objects of the target type won't be investigated deeply. If false, even target objects are unwrapped in order to check their sub-objects.</param>
		public static void VisitObjectsDeep<T>(object obj, Func<T,T> visitor, bool stopAtTarget = true)
		{
			VisitObjectsDeep<T>(obj, visitor, stopAtTarget, new HashSet<object>(), new Dictionary<Type,bool>());
		}
		private static object VisitObjectsDeep<T>(object obj, Func<T,T> visitor, bool stopAtTarget, HashSet<object> visitedGraph, Dictionary<Type,bool> exploreTypeCache)
		{
			if (obj == null) return obj;
			if (visitedGraph.Contains(obj)) return obj;

			Type objType = obj.GetType();
			TypeInfo objTypeInfo = objType.GetTypeInfo();
			TypeInfo targetTypeInfo = typeof(T).GetTypeInfo();

			// Visit object
			if (objTypeInfo.IsClass) visitedGraph.Add(obj);
			if (obj is T)
			{
				obj = visitor((T)obj);
				if (stopAtTarget) return obj;
			}

			// Check if object type should be explored
			bool explore = false;
			if (!exploreTypeCache.TryGetValue(objType, out explore))
			{
				explore = VisitObjectsDeep_ExploreType(typeof(T), objType, exploreTypeCache);
				exploreTypeCache[objType] = explore;
			}
			if (!explore) return obj;

			// Arrays
			if (objType.IsArray)
			{
				Array baseArray = (Array)obj;
				Type elemType = objType.GetElementType();
				TypeInfo elemTypeInfo = elemType.GetTypeInfo();
				int length = baseArray.Length;

				// Explore elements
				if (!elemTypeInfo.IsClass || targetTypeInfo.IsAssignableFrom(elemTypeInfo))
				{
					for (int i = 0; i < length; ++i)
					{
						object val = baseArray.GetValue(i);
						val = VisitObjectsDeep<T>(val, visitor, stopAtTarget, visitedGraph, exploreTypeCache);
						baseArray.SetValue(val, i);
					}
				}
				else
				{
					for (int i = 0; i < length; ++i)
					{
						object val = baseArray.GetValue(i);
						VisitObjectsDeep<T>(val, visitor, stopAtTarget, visitedGraph, exploreTypeCache);
					}
				}
			}
			// Complex objects
			else
			{
				// Explore fields
				var fields = objTypeInfo.DeclaredFieldsDeep();
				foreach (FieldInfo field in fields)
				{
					if (field.IsStatic) continue;
					TypeInfo fieldTypeInfo = field.FieldType.GetTypeInfo();

					object val = field.GetValue(obj);

					val = VisitObjectsDeep<T>(val, visitor, stopAtTarget, visitedGraph, exploreTypeCache);

					if (!objTypeInfo.IsClass || targetTypeInfo.IsAssignableFrom(fieldTypeInfo))
						field.SetValue(obj, val);
				}
			}

			return obj;
		}
		private static bool VisitObjectsDeep_ExploreType(Type searchForType, Type variableType, Dictionary<Type,bool> exploreTypeCache)
		{
			// Check the cache and calculate the value, if we missed
			bool explore = false;
			if (!exploreTypeCache.TryGetValue(variableType, out explore))
			{
				TypeInfo searchForTypeInfo = searchForType.GetTypeInfo();
				TypeInfo variableTypeInfo = variableType.GetTypeInfo();

				// Found a variable of the searched type? Done.
				if (searchForTypeInfo.IsAssignableFrom(variableTypeInfo))
					explore = true;

				// Don't explore primitives
				else if (variableTypeInfo.IsPrimitive)
					explore = false;
				else if (variableTypeInfo.IsEnum)
					explore = false;

				// Some hardcoded early-outs for well known types
				else if (variableType == typeof(decimal))
					explore = false;
				else if (variableType == typeof(string))
					explore = false;
				else if (typeof(MemberInfo).GetTypeInfo().IsAssignableFrom(variableTypeInfo))
					explore = false;
				else if (typeof(Delegate).GetTypeInfo().IsAssignableFrom(variableTypeInfo))
					explore = false;

				// We also need to explore (for example) all "object" variables, because they could contain anything
				else if (variableTypeInfo.IsAssignableFrom(searchForTypeInfo))
					explore = true;

				// Perform a deep check unless trivially true
				else
				{
					// Don't get trapped in circles: Assume false, until we found out otherwise
					exploreTypeCache[variableType] = explore;
				
					// Check element type
					if (variableType.IsArray)
					{
						explore = VisitObjectsDeep_ExploreType(searchForType, variableType.GetElementType(), exploreTypeCache);
					}
					// Check referred fields
					else
					{
						explore = variableType.GetTypeInfo().DeclaredFieldsDeep().Any(f => 
							!f.IsStatic &&
							!string.Equals(f.Name, "_syncRoot", StringComparison.OrdinalIgnoreCase) && 
							VisitObjectsDeep_ExploreType(searchForType, f.FieldType, exploreTypeCache));
					}
				}
				exploreTypeCache[variableType] = explore;
			}
			return explore;
		}

		/// <summary>
		/// Cleans the specified object instance from event bindings that lead to a specific invalid Assembly.
		/// This method is used to sweep forgotten event bindings upon plugin reload.
		/// </summary>
		/// <param name="targetObject"></param>
		/// <param name="invalidAssembly"></param>
		/// <returns>Returns true, if any event binding was removed.</returns>
		public static bool CleanEventBindings(object targetObject, Assembly invalidAssembly)
		{
			bool cleanedAny = false;
			TypeInfo delegateTypeInfo = typeof(Delegate).GetTypeInfo();
			TypeInfo targetTypeInfo = targetObject.GetType().GetTypeInfo();
			IEnumerable<FieldInfo> targetFields = targetTypeInfo.DeclaredFieldsDeep();
			foreach (FieldInfo field in targetFields)
			{
				if (field.IsStatic) continue;
				TypeInfo fieldTypeInfo = field.FieldType.GetTypeInfo();
				if (delegateTypeInfo.IsAssignableFrom(fieldTypeInfo))
				{
					Delegate delOld = field.GetValue(targetObject) as Delegate;
					Delegate delNew = CleanEventBindings_Delegate(delOld, invalidAssembly);
					if (!object.ReferenceEquals(delNew, delOld))
					{
						cleanedAny = true;
						field.SetValue(targetObject, delNew);
					}
				}
			}
			return cleanedAny;
		}
		/// <summary>
		/// Cleans the specified class from static event bindings that lead to a specific invalid Assembly.
		/// This method is used to sweep forgotten event bindings upon plugin reload.
		/// </summary>
		/// <param name="targetType"></param>
		/// <param name="invalidAssembly"></param>
		/// <returns>Returns true, if any event binding was removed.</returns>
		public static bool CleanEventBindings(Type targetType, Assembly invalidAssembly)
		{
			bool cleanedAny = false;
			TypeInfo delegateTypeInfo = typeof(Delegate).GetTypeInfo();
			IEnumerable<FieldInfo> targetFields = targetType.GetTypeInfo().DeclaredFieldsDeep();
			foreach (FieldInfo field in targetFields)
			{
				if (!field.IsStatic) continue;
				TypeInfo fieldTypeInfo = field.FieldType.GetTypeInfo();
				if (delegateTypeInfo.IsAssignableFrom(fieldTypeInfo))
				{
					Delegate delOld = field.GetValue(null) as Delegate;
					Delegate delNew = CleanEventBindings_Delegate(delOld, invalidAssembly);
					if (!object.ReferenceEquals(delNew, delOld))
					{
						cleanedAny = true;
						field.SetValue(null, delNew);
					}
				}
			}
			return cleanedAny;
		}
		private static Delegate CleanEventBindings_Delegate(Delegate del, Assembly invalidAssembly)
		{
			if (del == null) return del;
			Delegate[] oldInvokeList = del.GetInvocationList();
			if (oldInvokeList.Any(e => e.GetMethodInfo().DeclaringType.GetTypeInfo().Assembly == invalidAssembly))
			{
				return Delegate.Combine(oldInvokeList.Where(e => e.GetMethodInfo().DeclaringType.GetTypeInfo().Assembly != invalidAssembly).ToArray());
			}
			return del;
		}

		/// <summary>
		/// Returns whether a certain <see cref="Duality.Resource"/> Type is able to reference another specific <see cref="Duality.Resource"/> Type.
		/// This is a pure optimization method that doesn't guarantee exact information in all cases - returns true, when in doubt.
		/// </summary>
		/// <param name="sourceResType"></param>
		/// <param name="targetResType"></param>
		/// <returns></returns>
		public static bool CanReferenceResource(Type sourceResType, Type targetResType)
		{
			bool result;
			if (!resRefCache.TryGetValue(new KeyValuePair<Type,Type>(sourceResType, targetResType), out result))
			{
				resRefCache[new KeyValuePair<Type,Type>(sourceResType, targetResType)] = false;
				
				TypeInfo resourceTypeInfo = typeof(Resource).GetTypeInfo();
				TypeInfo sourceResTypeInfo = sourceResType.GetTypeInfo();
				TypeInfo targetResTypeInfo = targetResType.GetTypeInfo();

				if (!resourceTypeInfo.IsAssignableFrom(sourceResTypeInfo)) throw new ArgumentException("Only Resource Types are valid.", "sourceResType");
				if (!resourceTypeInfo.IsAssignableFrom(targetResTypeInfo)) throw new ArgumentException("Only Resource Types are valid.", "targetResType");

				bool foundIt = false;
				bool foundAny = false;
				foreach (ExplicitResourceReferenceAttribute refAttrib in sourceResTypeInfo.GetAttributesCached<ExplicitResourceReferenceAttribute>())
				{
					foundAny = true;
					foreach (Type refType in refAttrib.ReferencedTypes)
					{
						TypeInfo refTypeInfo = refType.GetTypeInfo();
						if (refTypeInfo.IsAssignableFrom(targetResTypeInfo) || CanReferenceResource(refType, targetResType))
						{
							foundIt = true;
							break;
						}
					}
					if (foundIt) break;
				}

				result = foundIt || !foundAny;
				resRefCache[new KeyValuePair<Type,Type>(sourceResType, targetResType)] = result;
			}

			return result;
		}

		/// <summary>
		/// Returns whether the specified type is a primitive, enum, string, decimal, or struct that
		/// consists only of those types.
		/// </summary>
		/// <param name="baseObj"></param>
		/// <returns></returns>
		public static bool IsPlainOldData(this TypeInfo typeInfo)
		{
			// Early-out for some obvious cases
			if (typeInfo.IsArray) return false;
			if (typeInfo.IsPrimitive) return true;
			if (typeInfo.IsEnum) return true;

			// Special cases for some well-known classes
			Type type = typeInfo.AsType();
			if (type == typeof(string)) return true;
			if (type == typeof(decimal)) return true;

			// Otherwise, any class is not plain old data
			if (typeInfo.IsClass) return false;

			// If we have no evidence so far, check the cache and iterate fields
			bool isPlainOldData;
			if (plainOldDataTypeCache.TryGetValue(typeInfo, out isPlainOldData))
			{
				return isPlainOldData;
			}
			else
			{
				isPlainOldData = true;
				foreach (FieldInfo field in typeInfo.DeclaredFieldsDeep())
				{
					if (field.IsStatic) continue;
					TypeInfo fieldTypeInfo = field.FieldType.GetTypeInfo();
					if (!IsPlainOldData(fieldTypeInfo))
					{
						isPlainOldData = false;
						break;
					}
				}
				plainOldDataTypeCache[typeInfo] = isPlainOldData;
				return isPlainOldData;
			}
		}

		/// <summary>
		/// Resolves a Type based on its <see cref="GetTypeId">type id</see>.
		/// </summary>
		/// <param name="typeString">The type string to resolve.</param>
		/// <param name="declaringMethod">The generic method that is declaring the Type. Only necessary when resolving a generic methods parameter Type.</param>
		/// <returns></returns>
		public static Type ResolveType(string typeString, MethodInfo declaringMethod = null)
		{
			return ResolveType(typeString, null, declaringMethod);
		}
		/// <summary>
		/// Resolves a Member based on its <see cref="GetMemberId">member id</see>.
		/// </summary>
		/// <param name="memberString">The <see cref="GetMemberId">member id</see> of the member.</param>
		/// <param name="throwOnError">If true, an Exception is thrown on failure.</param>
		/// <returns></returns>
		public static MemberInfo ResolveMember(string memberString)
		{
			MemberInfo result;
			if (memberResolveCache.TryGetValue(memberString, out result)) return result;

			Assembly[] searchAsm = 
				DualityApp.PluginLoader.LoadedAssemblies
				.Except(DualityApp.PluginManager.DisposedPlugins)
				.ToArray();

			result = FindMember(memberString, searchAsm);
			if (result != null) memberResolveCache[memberString] = result;

			return result;
		}
		
		/// <summary>
		/// Returns the short version of an Assembly name.
		/// </summary>
		/// <param name="assembly"></param>
		/// <returns></returns>
		public static string GetShortAssemblyName(this Assembly assembly)
		{
			return assembly.FullName.Split(',')[0];
		}
		/// <summary>
		/// Returns the short version of an Assembly name.
		/// </summary>
		/// <param name="assemblyName"></param>
		/// <returns></returns>
		public static string GetShortAssemblyName(this AssemblyName assemblyName)
		{
			return assemblyName.FullName.Split(',')[0];
		}
		/// <summary>
		/// Returns the short version of an Assembly name.
		/// </summary>
		/// <param name="assemblyName"></param>
		/// <returns></returns>
		public static string GetShortAssemblyName(string assemblyName)
		{
			return assemblyName.Split(',')[0];
		}

		/// <summary>
		/// Returns a string describing a certain Type.
		/// </summary>
		/// <param name="type">The Type to describe</param>
		/// <returns></returns>
		public static string GetTypeCSCodeName(this Type type, bool shortName = false)
		{
			StringBuilder typeStr = new StringBuilder();

			if (type.IsGenericParameter)
			{
				return type.Name;
			}
			if (type.IsArray)
			{
				typeStr.Append(GetTypeCSCodeName(type.GetElementType(), shortName));
				typeStr.Append('[');
				typeStr.Append(',', type.GetArrayRank() - 1);
				typeStr.Append(']');
			}
			else
			{
				TypeInfo typeInfo = type.GetTypeInfo();
				Type[] genArgs = typeInfo.IsGenericType ? typeInfo.GenericTypeArguments : null;

				if (type.IsNested)
				{
					Type declType = type.DeclaringType;
					TypeInfo declTypeInfo = declType.GetTypeInfo();

					if (declTypeInfo.IsGenericTypeDefinition)
					{
						Array.Resize(ref genArgs, declTypeInfo.GenericTypeArguments.Length);
						declType = declTypeInfo.MakeGenericType(genArgs);
						declTypeInfo = declType.GetTypeInfo();
						genArgs = type.GenericTypeArguments.Skip(genArgs.Length).ToArray();
					}
					string parentName = GetTypeCSCodeName(declType, shortName);

					string[] nestedNameToken = shortName ? type.Name.Split('+') : type.FullName.Split('+');
					string nestedName = nestedNameToken[nestedNameToken.Length - 1];
						
					int genTypeSepIndex = nestedName.IndexOf("[[", StringComparison.Ordinal);
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
						typeStr.Append(type.Name.Split(new[] {'`'}, StringSplitOptions.RemoveEmptyEntries)[0].Replace('+', '.'));
					else
						typeStr.Append(type.FullName.Split(new[] {'`'}, StringSplitOptions.RemoveEmptyEntries)[0].Replace('+', '.'));
				}

				if (genArgs != null && genArgs.Length > 0)
				{
					if (typeInfo.IsGenericTypeDefinition)
					{
						typeStr.Append('<');
						typeStr.Append(',', genArgs.Length - 1);
						typeStr.Append('>');
					}
					else if (typeInfo.IsGenericType)
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
		/// <summary>
		/// Returns a string describing a certain Type.
		/// </summary>
		/// <param name="T">The Type to describe</param>
		/// <returns></returns>
		public static string GetTypeId(this Type T)
		{
			return T.FullName != null ? Regex.Replace(T.FullName, @"(, [^\]\[]*)", "") : T.Name;
		}
		/// <summary>
		/// Returns a string describing a certain Member of a Type.
		/// </summary>
		/// <param name="member">The Member to describe.</param>
		/// <returns></returns>
		public static string GetMemberId(this MemberInfo member)
		{
			if (member is TypeInfo)
				return MemberTokenTypeInfo + ":" + GetTypeId((member as TypeInfo).AsType());

			string declType = member.DeclaringType.GetTypeId();

			FieldInfo field = member as FieldInfo;
			if (field != null)
				return MemberTokenFieldInfo + ":" + declType + ':' + field.Name;

			EventInfo ev = member as EventInfo;
			if (ev != null)
				return MemberTokenEventInfo + ":" + declType + ':' + ev.Name;

			PropertyInfo property = member as PropertyInfo;
			if (property != null)
			{
				ParameterInfo[] parameters = property.GetIndexParameters();
				if (parameters.Length == 0)
					return MemberTokenPropertyInfo + ":" + declType + ':' + property.Name;
				else
					return MemberTokenPropertyInfo + ":" + declType + ':' + property.Name + '(' + parameters.ToString(p => p.ParameterType.GetTypeId(), ",") + ')';
			}

			ConstructorInfo ctor = member as ConstructorInfo;
			if (ctor != null)
			{
				ParameterInfo[] parameters = ctor.GetParameters();

				string result = MemberTokenConstructorInfo + ":" + declType + ':' + (ctor.IsStatic ? "s" : "i");

				if (parameters.Length != 0)
					result += '(' + parameters.ToString(p => p.ParameterType.GetTypeId(), ", ") + ')';

				return result;
			}

			MethodInfo method = member as MethodInfo;
			if (method != null)
			{
				ParameterInfo[] parameters = method.GetParameters();
				Type[] genArgs = method.GetGenericArguments();

				string result = MemberTokenMethodInfo + ":" + declType + ':' + method.Name;
				
				if (genArgs.Length != 0)
				{
					if (method.IsGenericMethodDefinition)
						result += "``" + genArgs.Length.ToString(CultureInfo.InvariantCulture);
					else
						result += "``" + genArgs.Length.ToString(CultureInfo.InvariantCulture) + '[' + genArgs.ToString(t => "[" + t.GetTypeId() + "]", ",") + ']';
				}
				if (parameters.Length != 0)
					result += '(' + parameters.ToString(p => p.ParameterType.GetTypeId(), ",") + ')';

				return result;
			}

			throw new NotSupportedException(string.Format("Member Type '{0} not supported", Log.Type(member.GetType())));
		}

		/// <summary>
		/// Performs a selective split operation on the specified string. Intended to be used on hierarchial argument lists.
		/// </summary>
		/// <param name="argList">The argument list to split.</param>
		/// <param name="pushLevel">The char that increases the current hierarchy level.</param>
		/// <param name="popLevel">The char that decreases the current hierarchy level.</param>
		/// <param name="separator">The char that separates two arguments.</param>
		/// <param name="splitLevel">The hierarchy level at which to perform the split operation.</param>
		/// <returns></returns>
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
		
		/// <summary>
		/// Clears the ReflectionHelpers Type cache.
		/// </summary>
		internal static void ClearTypeCache()
		{
			typeResolveCache.Clear();
			memberResolveCache.Clear();
			plainOldDataTypeCache.Clear();
			resRefCache.Clear();
			customMemberAttribCache.Clear();
		}

		private static Type ResolveType(string typeString, IEnumerable<Assembly> searchAsm, MethodInfo declaringMethod)
		{
			if (typeString == null) return null;

			// If we already have resolved this before, just return the cached result
			Type result;
			if (typeResolveCache.TryGetValue(typeString, out result)) return result;
			
			// If not specified otherwise, we'll search across all loaded Assemblies
			if (searchAsm == null)
			{
				searchAsm = 
					DualityApp.PluginLoader.LoadedAssemblies
					.Except(DualityApp.PluginManager.DisposedPlugins)
					.ToArray();
			}

			// Perform the search
			result = FindType(typeString, searchAsm, declaringMethod);

			// Mind the result for later, if it's successful and generally applicable
			if (result != null && declaringMethod == null)
				typeResolveCache[typeString] = result;

			return result;
		}
		private static Type FindType(string typeName, IEnumerable<Assembly> asmSearch, MethodInfo declaringMethod = null)
		{
			typeName = typeName.Trim();
			if (string.IsNullOrEmpty(typeName)) return null;

			// Retrieve generic parameters
			Match genericParamsMatch = Regex.Match(typeName, @"(\[)(\[.+\])(\])");
			string[] genericParams = null;
			if (genericParamsMatch.Groups.Count > 3)
			{
				string genericParamList = genericParamsMatch.Groups[2].Value;
				genericParams = SplitArgs(genericParamList, '[', ']', ',', 0);
				for (int i = 0; i < genericParams.Length; i++)
					genericParams[i] = genericParams[i].Substring(1, genericParams[i].Length - 2);
			}

			// Process type name
			string[] token = Regex.Split(typeName, @"\[\[.+\]\]");
			string typeNameBase = token[0];
			string elementTypeName = typeName;

			// Handle Reference
			string lastToken = token[token.Length - 1];
			if (lastToken.Length > 0 && lastToken[lastToken.Length - 1] == '&')
			{
				Type elementType = ResolveType(typeName.Substring(0, typeName.Length - 1), asmSearch, declaringMethod);
				if (elementType == null) return null;
				return elementType.MakeByRefType();
			}

			// Retrieve array ranks
			int arrayRank = 0;
			MatchCollection arrayMatches = Regex.Matches(token[token.Length - 1], @"\[,*\]");
			if (arrayMatches.Count > 0)
			{
				string rankStr = arrayMatches[arrayMatches.Count - 1].Value;
				int commaCount = 0;
				for (int i = 0; i < rankStr.Length; i++)
				{
					if (rankStr[i] == ',') commaCount++;
				}
				arrayRank = 1 + commaCount;
				elementTypeName = elementTypeName.Substring(0, elementTypeName.Length - rankStr.Length);
			}
			
			// Handle Arrays
			if (arrayRank > 0)
			{
				Type elementType = ResolveType(elementTypeName, asmSearch, declaringMethod);
				if (elementType == null) return null;
				return arrayRank == 1 ? elementType.MakeArrayType() : elementType.MakeArrayType(arrayRank);
			}

			Type baseType = null;
			// Generic method parameter Types
			if (typeNameBase.StartsWith("``"))
			{
				if (declaringMethod != null)
				{
					int methodGenArgIndex = int.Parse(typeNameBase.Substring(2, typeNameBase.Length - 2));
					baseType = declaringMethod.GetGenericArguments()[methodGenArgIndex];
				}
			}
			else
			{
				// Retrieve base type
				foreach (Assembly assembly in asmSearch)
				{
					baseType = assembly.GetType(typeNameBase);
					if (baseType != null) break;
				}
				// Failed to retrieve base type? Try manually and ignore plus / dot difference.
				if (baseType == null)
				{
					string assemblyNameGuess = typeName.Split('.', '+').FirstOrDefault();
					IEnumerable<Assembly> sortedAsmSearch = asmSearch.OrderByDescending(a => a.GetShortAssemblyName() == assemblyNameGuess);
					foreach (Assembly assembly in sortedAsmSearch)
					{
						// Try to retrieve all Types from the current Assembly
						TypeInfo[] definedTypes;
						try { definedTypes = assembly.DefinedTypes.ToArray(); }
						catch (Exception) { continue; }

						// Iterate and manually compare names
						foreach (TypeInfo typeInfo in definedTypes)
						{
							if (IsFullTypeNameEqual(typeNameBase, typeInfo.FullName))
							{
								baseType = typeInfo.AsType();
								break;
							}
						}
						if (baseType != null) break;
					}
				}
				// Failed anyway? Try explicit resolve
				if (baseType == null)
				{
					ResolveMemberEventArgs args = new ResolveMemberEventArgs(typeNameBase);
					if (TypeResolve != null)
					{
						TypeResolve(null, args);
					}
					baseType = (args.ResolvedMember is TypeInfo) ? (args.ResolvedMember as TypeInfo).AsType() : null;
				}
			}
			
			// Retrieve generic type params
			if (genericParams != null)
			{
				Type[] genericParamTypes = new Type[genericParams.Length];

				for (int i = 0; i < genericParamTypes.Length; i++)
				{
					// Explicitly referring to generic type definition params: Don't attemp to make it generic.
					if ((genericParams[i].Length > 0 && genericParams[i][0] == '`') && 
						(genericParams[i].Length < 2 || genericParams[i][1] != '`')) return baseType;

					genericParamTypes[i] = ResolveType(genericParams[i], asmSearch, declaringMethod);

					// Can't find the generic type argument: Fail.
					if (genericParamTypes[i] == null) return null;
				}

				if (baseType == null)
					return null;
				else
					return baseType.MakeGenericType(genericParamTypes);
			}

			return baseType;
		}
		private static MemberInfo FindMember(string memberString, IEnumerable<Assembly> asmSearch)
		{
			string[] token = memberString.Split(':');

			char memberTypeToken;
			Type declaringType;

			// If there is no member type token, it is actually a type id.
			if (token.Length == 1)
			{
				Type type = ResolveType(token[0], asmSearch, null);
				declaringType = type != null ? type : null;
				memberTypeToken = MemberTokenTypeInfo;
			}
			// Otherwise, determine the member type using the member type token
			else if (token.Length > 1)
			{
				Type type = ResolveType(token[1], asmSearch, null);
				declaringType = type != null ? type : null;
				memberTypeToken = (token[0].Length > 0) ? token[0][0] : MemberTokenUndefined;
			}
			// If we have nothing (empty string, etc.), fail
			else
			{
				declaringType = null;
				memberTypeToken = MemberTokenUndefined;
			}

			if (declaringType != null && memberTypeToken != ' ')
			{
				TypeInfo declaringTypeInfo = declaringType.GetTypeInfo();

				if (memberTypeToken == MemberTokenTypeInfo)
				{
					return declaringType.GetTypeInfo();
				}
				else if (memberTypeToken == MemberTokenFieldInfo)
				{
					MemberInfo member = declaringType.GetRuntimeFields().FirstOrDefault(m => m.Name == token[2]);
					if (member != null) return member;
				}
				else if (memberTypeToken == MemberTokenEventInfo)
				{
					MemberInfo member = declaringType.GetRuntimeEvents().FirstOrDefault(m => m.Name == token[2]);
					if (member != null) return member;
				}
				else
				{
					int memberParamListStartIndex = token[2].IndexOf('(');
					int memberParamListEndIndex = token[2].IndexOf(')');
					string memberParamList = memberParamListStartIndex != -1 ? token[2].Substring(memberParamListStartIndex + 1, memberParamListEndIndex - memberParamListStartIndex - 1) : null;
					string[] memberParams = SplitArgs(memberParamList, '[', ']', ',', 0);
					string memberName = memberParamListStartIndex != -1 ? token[2].Substring(0, memberParamListStartIndex) : token[2];
					Type[] memberParamTypes = memberParams.Select(p => ResolveType(p, asmSearch, null)).ToArray();

					if (memberTypeToken == MemberTokenConstructorInfo)
					{
						bool lookForStatic = memberName == "s";
						ConstructorInfo[] availCtors = declaringTypeInfo.DeclaredConstructors.Where(m => 
							m.IsStatic == lookForStatic && 
							m.GetParameters().Length == memberParams.Length).ToArray();
						foreach (ConstructorInfo ctor in availCtors)
						{
							bool possibleMatch = true;
							ParameterInfo[] methodParams = ctor.GetParameters();
							for (int i = 0; i < methodParams.Length; i++)
							{
								string methodParamTypeName = methodParams[i].ParameterType.Name;
								if (methodParams[i].ParameterType != memberParamTypes[i] && methodParamTypeName != memberParams[i])
								{
									possibleMatch = false;
									break;
								}
							}
							if (possibleMatch) return ctor;
						}
					}
					else if (memberTypeToken == MemberTokenPropertyInfo)
					{
						PropertyInfo[] availProps = declaringType.GetRuntimeProperties().Where(m => 
							m.Name == memberName && 
							m.GetIndexParameters().Length == memberParams.Length).ToArray();
						foreach (PropertyInfo prop in availProps)
						{
							bool possibleMatch = true;
							ParameterInfo[] methodParams = prop.GetIndexParameters();
							for (int i = 0; i < methodParams.Length; i++)
							{
								string methodParamTypeName = methodParams[i].ParameterType.Name;
								if (methodParams[i].ParameterType != memberParamTypes[i] && methodParamTypeName != memberParams[i])
								{
									possibleMatch = false;
									break;
								}
							}
							if (possibleMatch) return prop;
						}
					}

					int genArgTokenStartIndex = token[2].IndexOf("``", StringComparison.Ordinal);
					int genArgTokenEndIndex = memberParamListStartIndex != -1 ? memberParamListStartIndex : token[2].Length;
					string genArgToken = genArgTokenStartIndex != -1 ? token[2].Substring(genArgTokenStartIndex + 2, genArgTokenEndIndex - genArgTokenStartIndex - 2) : "";
					if (genArgTokenStartIndex != -1) memberName = token[2].Substring(0, genArgTokenStartIndex);			

					int genArgListStartIndex = genArgToken.IndexOf('[');
					int genArgListEndIndex = genArgToken.LastIndexOf(']');
					string genArgList = genArgListStartIndex != -1 ? genArgToken.Substring(genArgListStartIndex + 1, genArgListEndIndex - genArgListStartIndex - 1) : null;
					string[] genArgs = SplitArgs(genArgList, '[', ']', ',', 0);
					for (int i = 0; i < genArgs.Length; i++) genArgs[i] = genArgs[i].Substring(1, genArgs[i].Length - 2);

					int genArgCount = genArgToken.Length > 0 ? int.Parse(genArgToken.Substring(0, genArgListStartIndex != -1 ? genArgListStartIndex : genArgToken.Length)) : 0;

					// Select the method that fits
					MethodInfo[] availMethods = declaringType.GetRuntimeMethods().Where(m => 
						m.Name == memberName && 
						m.GetGenericArguments().Length == genArgCount &&
						m.GetParameters().Length == memberParams.Length).ToArray();
					foreach (MethodInfo method in availMethods)
					{
						bool possibleMatch = true;
						ParameterInfo[] methodParams = method.GetParameters();
						for (int i = 0; i < methodParams.Length; i++)
						{
							string methodParamTypeName = methodParams[i].ParameterType.Name;
							if (methodParams[i].ParameterType != memberParamTypes[i] && methodParamTypeName != memberParams[i])
							{
								possibleMatch = false;
								break;
							}
						}
						if (possibleMatch) return method;
					}
				}
			}
			
			// Failed? Try explicit resolve
			ResolveMemberEventArgs args = new ResolveMemberEventArgs(memberString);
			if (MemberResolve != null) MemberResolve(null, args);
			return args.ResolvedMember;
		}
		private static bool IsFullTypeNameEqual(string typeNameA, string typeNameB)
		{
			// Not doing this for performance reasons:
			//string nameTemp = typeNameA.Replace('+', '.');
			//if (typeNameB == nameTemp)

			if (typeNameA.Length != typeNameB.Length) return false;

			for (int i = 0; i < typeNameA.Length; ++i)
			{
				if (typeNameA[i] != typeNameB[i])
				{
					if (typeNameA[i] == '.' && typeNameB[i] == '+')
						continue;
					if (typeNameA[i] == '+' && typeNameB[i] == '.')
						continue;
					return false;
				}
			}

			return true;
		}
	}

	public class ResolveMemberEventArgs : EventArgs
	{
		private string memberId = null;
		private MemberInfo resolvedMember = null;

		/// <summary>
		/// [GET] The Member id to resolve.
		/// </summary>
		public string MemberId
		{
			get { return this.memberId; }
		}
		/// <summary>
		/// [GET / SET] The resolved Member.
		/// </summary>
		public MemberInfo ResolvedMember
		{
			get { return this.resolvedMember; }
			set { this.resolvedMember = value; }
		}

		public ResolveMemberEventArgs(string memberId)
		{
			this.memberId = memberId;
		}
	}
}
