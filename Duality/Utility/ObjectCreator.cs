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
	public static class ObjectCreator
	{
		private delegate object CreateMethod();

		private	static readonly CreateMethod nullObjectActivator = () => null;
		private	static Dictionary<TypeInfo,CreateMethod> createInstanceMethodCache = new Dictionary<TypeInfo,CreateMethod>();


		/// <summary>
		/// Creates an instance of a Type. Attempts to use the Types default empty constructor, but will
		/// return an uninitialized object in case no constructor is available.
		/// </summary>
		/// <param name="typeInfo">The Type to create an instance of.</param>
		/// <returns>An instance of the Type. Null, if instanciation wasn't possible.</returns>
		public static object CreateInstanceOf(this TypeInfo typeInfo)
		{
			CreateMethod activator;
			if (createInstanceMethodCache.TryGetValue(typeInfo, out activator))
			{
				return activator();
			}
			else
			{
				// To prevent recursion, assume failed object initialization until proven otherwise
				createInstanceMethodCache[typeInfo] = nullObjectActivator;

				// Determine how to create an object of this type
				object firstResult;
				activator = CreateObjectActivator(typeInfo, out firstResult);
				createInstanceMethodCache[typeInfo] = activator;
				return firstResult;
			}
		}
		/// <summary>
		/// Returns the default instance of a Type. Equals <c>default(T)</c>, but works for Reflection.
		/// </summary>
		/// <param name="instanceType">The Type to create a default instance of.</param>
		/// <returns></returns>
		public static object GetDefaultOf(this TypeInfo typeInfo)
		{
			if (typeInfo.IsValueType)
				return typeInfo.CreateInstanceOf();
			else
				return null;
		}


		private static CreateMethod CreateObjectActivator(TypeInfo typeInfo, out object firstResult)
		{
			CreateMethod activator;
			firstResult = null;

			// Filter out non-instantiatable Types
			if (typeInfo.IsAbstract || typeInfo.IsInterface || typeInfo.IsGenericTypeDefinition)
			{
				activator = nullObjectActivator;
			}
			// If the caller wants a string, just return an empty one
			else if (typeInfo.AsType() == typeof(string))
			{
				activator = () => "";
			}
			// If the caller wants an array, create an empty one
			else if (typeInfo.IsArray && typeInfo.GetArrayRank() == 1)
			{
				activator = () => Array.CreateInstance(typeInfo.GetElementType(), 0);
			}
			// For structs, boxing a default(T) is sufficient
			else if (typeInfo.IsValueType)
			{
				var lambda = Expression.Lambda<CreateMethod>(Expression.Convert(Expression.Default(typeInfo.AsType()), typeof(object)));
				activator = lambda.Compile();
			}
			else
			{
				activator = nullObjectActivator;

				// Retrieve constructors, sorted from trivial to parameter-rich
				ConstructorInfo[] constructors = typeInfo.DeclaredConstructors
					.Where(c => !c.IsStatic)
					.Select(c => new { Info = c, ParamCount = c.GetParameters().Length })
					.OrderBy(s => s.ParamCount)
					.Select(s => s.Info)
					.ToArray();

				Exception lastError = null;
				foreach (ConstructorInfo con in constructors)
				{
					// Prepare constructor argument values - just use default(T) for all of them.
					ParameterInfo[] conParams = con.GetParameters();
					Expression[] args = new Expression[conParams.Length];
					for (int i = 0; i < args.Length; i++)
					{
						Type paramType = conParams[i].ParameterType;
						args[i] = Expression.Default(paramType);
					}

					// Compile a lambda method invoking the constructor
					var lambda = Expression.Lambda<CreateMethod>(Expression.New(con, args));
					activator = lambda.Compile();

					// Does it work?
					firstResult = CheckActivator(activator, out lastError);
					if (firstResult != null)
						break;
				}

				// If all constructors failed, inform someone. This is not ideal.
				if (firstResult == null)
				{
					Log.Core.WriteWarning("Failed to create object of Type {0}. Make sure there is a trivial constructor.", Log.Type(typeInfo));
				}
			}

			// Test whether our activation method really works, replace with dummy if not
			if (firstResult == null)
			{
				Exception error;
				firstResult = CheckActivator(activator, out error);

				// If we fail to initialize the Type due to a problem in its static constructor, it's likely a user problem. Let him know.
				if (error is TypeInitializationException)
				{
					Log.Core.WriteError("Failed to initialize Type {0}: {1}",
						Log.Type(typeInfo),
						Log.Exception(error.InnerException));
				}
			}

			// If we still don't have anything, just use a dummy.
			if (firstResult == null)
				activator = nullObjectActivator;

			return activator;
		}

		private static object CheckActivator(CreateMethod activator)
		{
			Exception error;
			return CheckActivator(activator, out error);
		}
		[System.Diagnostics.DebuggerStepThrough]
		private static object CheckActivator(CreateMethod activator, out Exception error)
		{
			try
			{
				error = null;
				return activator();
			}
			catch (Exception e)
			{
				error = e;
				return null;
			}
		}

		/// <summary>
		/// Clears the ReflectionHelpers Type cache.
		/// </summary>
		internal static void ClearTypeCache()
		{
			createInstanceMethodCache.Clear();
		}
	}
}
