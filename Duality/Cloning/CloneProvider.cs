using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Duality.Cloning.Surrogates;

namespace Duality.Cloning
{
	public class CloneProvider
	{
		private Dictionary<object, object>	objToClone		= new Dictionary<object,object>();
		private	List<Type>					explicitUnwrap	= new List<Type>();
		private	CloneProviderContext		context			= CloneProviderContext.Default;
		

		/// <summary>
		/// [GET] Provides information about the context in which the operation is performed.
		/// </summary>
		public CloneProviderContext Context
		{
			get { return this.context; }
		}
		/// <summary>
		/// [GET / SET] When set, this CloneProvider will not unwrap any value that isn't assignable to one of the 
		/// specified Types, essentially shallow-copying all except them.
		/// </summary>
		public List<Type> ExplicitUnwrap
		{
			get { return this.explicitUnwrap; }
		}
		

		public CloneProvider(CloneProviderContext context = null)
		{
			if (context != null) this.context = context;
		}


		/// <summary>
		/// Clears all existing object mappings.
		/// </summary>
		public void ClearObjectMap()
		{
			this.objToClone.Clear();
		}
		/// <summary>
		/// Requests the clone(d) object mapped to the specified base object.
		/// </summary>
		/// <param name="baseObj"></param>
		/// <returns></returns>
		public T RequestObjectClone<T>(T baseObj)
		{
			if (baseObj == null) return default(T);

			object clone;
			if (this.objToClone.TryGetValue(baseObj, out clone)) return (T)clone;

			return (T)this.CloneObject(baseObj);
		}
		/// <summary>
		/// Returns an already registered clone object, if existing.
		/// </summary>
		/// <param name="baseObj"></param>
		/// <returns></returns>
		public T GetRegisteredObjectClone<T>(T baseObj)
		{
			if (baseObj == null) return default(T);

			object clone;
			if (this.objToClone.TryGetValue(baseObj, out clone)) return (T)clone;

			return default(T);
		}
		/// <summary>
		/// Returns whether the specified object is a registered original / base object.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public bool IsOriginalObject(object obj)
		{
			return obj != null ? this.objToClone.ContainsKey(obj) : false;
		}
		/// <summary>
		/// Returns whether the specified object is a registered clone object.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public bool IsCloneObject(object obj)
		{
			return obj != null ? this.objToClone.ContainsValue(obj) : false;
		}
		/// <summary>
		/// Copies the base objects data to the specified target object.
		/// </summary>
		/// <param name="baseObj"></param>
		/// <param name="targetObj"></param>
		/// <param name="fields"></param>
		public void CopyObjectTo<T>(T baseObj, T targetObj, IEnumerable<FieldInfo> fields = null)
		{
			if (fields == null)
			{
				Type objType = baseObj.GetType();
				if (!this.DoesUnwrapType(objType)) return;

				// IClonables
				if (baseObj is ICloneExplicit)
				{
					(baseObj as ICloneExplicit).CopyDataTo(targetObj, this);
					return;
				}

				// ISurrogate
				ICloneSurrogate surrogate = GetSurrogateFor(objType);
				if (surrogate != null)
				{
					surrogate.RealObject = baseObj;
					surrogate.CopyDataTo(targetObj, this);
					return;
				}

				fields = objType.GetAllFields(ReflectionHelper.BindInstanceAll);
			}
			foreach (FieldInfo f in fields)
				f.SetValue(targetObj, this.RequestObjectClone(f.GetValue(baseObj)));
		}
		/// <summary>
		/// Registers a new base-clone mapping.
		/// </summary>
		/// <param name="baseObj"></param>
		/// <param name="clone"></param>
		public void RegisterObjectClone<T>(T baseObj, T clone)
		{
			if (baseObj == null) throw new ArgumentNullException("baseObj");
			if (clone == null) throw new ArgumentNullException("clone");
			this.objToClone[baseObj] = clone;
		}
		
		private bool DoesUnwrapType(Type type)
		{
			bool unwrap = !type.IsDeepByValueType();
			if (this.explicitUnwrap.Count > 0)
			{
				unwrap = unwrap && type.IsValueType;
				if (!unwrap) unwrap = this.explicitUnwrap.Any(t => t.IsAssignableFrom(type));
			}
			return unwrap;
		}
		private object CloneObject(object baseObj)
		{
			Type objType = baseObj.GetType();
			if (!this.DoesUnwrapType(objType)) return baseObj;

			// IClonables
			if (baseObj is ICloneExplicit)
			{
				object copy = objType.CreateInstanceOf() ?? objType.CreateInstanceOf(true);
				if (objType.IsClass) this.RegisterObjectClone(baseObj, copy);
				(baseObj as ICloneExplicit).CopyDataTo(copy, this);
				return copy;
			}

			// ISurrogate
			ICloneSurrogate surrogate = GetSurrogateFor(objType);
			if (surrogate != null)
			{
				surrogate.RealObject = baseObj;
				object copy = surrogate.CreateTargetObject(this);
				if (objType.IsClass) this.RegisterObjectClone(baseObj, copy);
				surrogate.CopyDataTo(copy, this);
				return copy;
			}

			// Shallow types, cloned by assignment
			if (objType.IsDeepByValueType())
			{
				return baseObj;
			}
			// Arrays
			else if (objType.IsArray)
			{
				Array baseArray = (Array)baseObj;
				Type elemType = objType.GetElementType();
				int length = baseArray.Length;
				Array copy = Array.CreateInstance(elemType, length);
				this.RegisterObjectClone(baseObj, copy);

				bool unwrap = this.DoesUnwrapType(elemType);
				if (unwrap)
				{
					for (int i = 0; i < length; ++i)
						copy.SetValue(this.RequestObjectClone(baseArray.GetValue(i)), i);
				}
				else if (!elemType.IsValueType)
				{
					for (int i = 0; i < length; ++i)
					{
						object obj = baseArray.GetValue(i);
						copy.SetValue(this.GetRegisteredObjectClone(obj) ?? obj, i);
					}
				}
				else
				{
					baseArray.CopyTo(copy, 0);
				}

				return copy;
			}
			// Reference types / complex objects
			else
			{
				object copy = objType.CreateInstanceOf() ?? objType.CreateInstanceOf(true);
				if (objType.IsClass) this.RegisterObjectClone(baseObj, copy);

				this.CopyObjectTo(baseObj, copy, objType.GetAllFields(ReflectionHelper.BindInstanceAll));

				return copy;
			}
		}


		private	static List<ICloneSurrogate>	surrogates	= null;
		private static ICloneSurrogate GetSurrogateFor(Type type)
		{
			if (surrogates == null)
			{
				surrogates = 
					DualityApp.GetAvailDualityTypes(typeof(ICloneSurrogate))
					.Select(t => t.CreateInstanceOf())
					.OfType<ICloneSurrogate>()
					.NotNull()
					.ToList();
				surrogates.StableSort((s1, s2) => s1.Priority - s2.Priority);
			}
			return surrogates.FirstOrDefault(s => s.MatchesType(type));
		}

		public static T DeepClone<T>(T baseObj, CloneProviderContext context = null)
		{
			CloneProvider provider = new CloneProvider(context);
			return (T)provider.RequestObjectClone(baseObj);
		}
		public static void DeepCopyTo<T>(T baseObj, T targetObj, CloneProviderContext context = null)
		{
			Type objType = baseObj.GetType();
			CloneProvider provider = new CloneProvider(context);
			if (objType.IsClass) provider.RegisterObjectClone(baseObj, targetObj);
			provider.CopyObjectTo(baseObj, targetObj);
		}

		internal static void PerformReflectionFallback<T>(string copyMethodName, T baseObj, T targetObj, CloneProvider provider)
		{
			if (copyMethodName == null) throw new ArgumentNullException("copyMethodName");
			if (baseObj == null) throw new ArgumentNullException("baseObj");
			if (targetObj == null) throw new ArgumentNullException("targetObj");

			// Use explicit unwrapping: Only unwrap (deep-copy) collection types, shallow-copy others.
			provider.ExplicitUnwrap.Add(typeof(System.Collections.ICollection));

			// Travel up the inheritance hierarchy
			// Don't fallback for types from the Duality Assembly. Those are required to do explicit copying.
			Type curType = baseObj.GetType();
			while (curType.Assembly != typeof(DualityApp).Assembly && typeof(T).IsAssignableFrom(curType))
			{
				// Retrieve OnCopyTo method, if declared in this specific class (local override)
				MethodInfo localOnCopyTo = curType.GetMethod(
					copyMethodName, 
					BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly, 
					null, 
					new[] { typeof(T), typeof(CloneProvider) }, 
					null);
				// Apply default behaviour to any class that doesn't have its own OnCopyTo override
				if (localOnCopyTo == null)
				{
					provider.CopyObjectTo(
						baseObj, 
						targetObj, 
						curType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));
				}

				curType = curType.BaseType;
			}

			// Deactivate explicit unwrapping again
			provider.ExplicitUnwrap.Remove(typeof(System.Collections.ICollection));
		}
		internal static void ClearTypeCache()
		{
			surrogates = null;
		}
	}

	public static class ExtMethodsCloning
	{
		public static T DeepClone<T>(this T baseObj, CloneProviderContext context = null)
		{
			return CloneProvider.DeepClone<T>(baseObj, context);
		}
		public static void DeepCopyTo<T>(this T baseObj, T targetObj, CloneProviderContext context = null)
		{
			CloneProvider.DeepCopyTo<T>(baseObj, targetObj, context);
		}
	}
}
