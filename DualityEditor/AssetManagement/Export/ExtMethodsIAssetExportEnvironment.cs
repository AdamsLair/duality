using System;
using System.Collections.Generic;
using System.Linq;

using Duality;

namespace Duality.Editor.AssetManagement
{
	public static class ExtMethodsIAssetExportEnvironment
	{
		/// <summary>
		/// Retrieves the value of an export parameter for the exported <see cref="Resource"/>.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="env"></param>
		/// <param name="parameterName">The name of the parameter.</param>
		/// <param name="defaultValue">If the value wasn't defined for the given Resource, the default value will be used instead.</param>
		/// <returns>The retrieved value, or the default value that was specified.</returns>
		public static T GetParameter<T>(this IAssetExportEnvironment env, string parameterName, T defaultValue)
		{
			T value;
			if (!env.GetParameter<T>(parameterName, out value))
				return defaultValue;
			else
				return value;
		}
		/// <summary>
		/// Retrieves the value of an export parameter for the specified <see cref="Resource"/>.
		/// If the parameter was undefined, it will be (persistently) initialized with the specified default value.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="env"></param>
		/// <param name="parameterName">The name of the parameter.</param>
		/// <param name="defaultValue">If the value wasn't defined for the given Resource, the default value will be used instead.</param>
		/// <returns>The retrieved value, or the default value that was specified.</returns>
		public static T GetOrInitParameter<T>(this IAssetExportEnvironment env, string parameterName, T defaultValue)
		{
			T value;
			if (env.GetParameter<T>(parameterName, out value))
				return value;

			env.SetParameter<T>(parameterName, defaultValue);
			return defaultValue;
		}
	}
}
