using System;
using System.Collections.Generic;
using System.Linq;

using Duality;

namespace Duality.Editor.AssetManagement
{
	public static class ExtMethodsIAssetImportEnvironment
	{
		/// <summary>
		/// This is a shortcut method for interating over the available input and ask to handle each item
		/// that matches the specified predicate. If no predicate is specified, this method will try to handle
		/// all available input.
		/// </summary>
		/// <param name="env"></param>
		/// <param name="predicate"></param>
		/// <returns>Enumerates all input items which the requesting importer is allowed to handle.</returns>
		public static IEnumerable<AssetImportInput> HandleAllInput(this IAssetImportEnvironment env, Predicate<AssetImportInput> predicate)
		{
			List<AssetImportInput> handledInput = new List<AssetImportInput>();
			foreach (AssetImportInput input in env.Input)
			{
				if ((predicate == null || predicate(input)) && env.HandleInput(input.Path))
				{
					handledInput.Add(input);
				}
			}
			return handledInput;
		}
		
		/// <summary>
		/// Specifies that the current importer will create or modify a <see cref="Duality.Resource"/> with 
		/// the specified name (see <see cref="Duality.Editor.AssetManagement.AssetImportInput.AssetName"/>).
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="assetName">The name of the generated output <see cref="Duality.Resource"/> (see <see cref="Duality.Editor.AssetManagement.AssetImportInput.AssetName"/>).</param>
		/// <param name="inputPath">The input path that is used to generate this output <see cref="Duality.Resource"/>.</param>
		public static void AddOutput<T>(this IAssetImportEnvironment env, string assetName, string inputPath) where T : Resource
		{
			env.AddOutput<T>(assetName, new string[] { inputPath });
		}
		/// <summary>
		/// Submits the specified <see cref="Duality.Resource"/> as a generated output of the current importer.
		/// </summary>
		/// <param name="resource">A reference to the generated output <see cref="Duality.Resource"/>.</param>
		/// <param name="inputPath">The input path that is used to generate this output <see cref="Duality.Resource"/>.</param>
		public static void AddOutput(this IAssetImportEnvironment env, IContentRef resource, string inputPath)
		{
			env.AddOutput(resource, new string[] { inputPath });
		}
		
		/// <summary>
		/// Retrieves the value of an import parameter for the specified <see cref="Resource"/>.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="env"></param>
		/// <param name="resource">A reference to the <see cref="Duality.Resource"/> that is parameterized.</param>
		/// <param name="parameterName">The name of the parameter.</param>
		/// <param name="defaultValue">If the value wasn't defined for the given Resource, the default value will be used instead.</param>
		/// <returns>The retrieved value, or the default value of the expected value type.</returns>
		public static T GetParameter<T>(this IAssetImportEnvironment env, IContentRef resource, string parameterName, T defaultValue)
		{
			T value;
			if (env.GetParameter<T>(resource, parameterName, out value))
				return value;
			else
				return defaultValue;
		}
		/// <summary>
		/// Retrieves the value of an import parameter for the specified <see cref="Resource"/>.
		/// If the parameter was undefined, it will be (persistently) initialized with the specified default value.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="env"></param>
		/// <param name="resource">A reference to the <see cref="Duality.Resource"/> that is parameterized.</param>
		/// <param name="parameterName">The name of the parameter.</param>
		/// <param name="defaultValue">If the value wasn't defined for the given Resource, the default value will be used instead.</param>
		/// <returns>The retrieved value, or the default value that was specified.</returns>
		public static T GetOrInitParameter<T>(this IAssetImportEnvironment env, IContentRef resource, string parameterName, T defaultValue)
		{
			T value;
			if (env.GetParameter<T>(resource, parameterName, out value))
				return value;

			env.SetParameter<T>(resource, parameterName, defaultValue);
			return defaultValue;
		}
	}
}
