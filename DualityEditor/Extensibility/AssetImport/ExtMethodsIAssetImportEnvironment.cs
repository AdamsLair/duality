using System;
using System.Collections.Generic;
using System.Linq;

using Duality;

namespace Duality.Editor
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
		public static IEnumerable<AssetImportInput> HandleAllInput(this IAssetImportEnvironment env, Predicate<AssetImportInput> predicate = null)
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
		/// the specified name (see <see cref="Duality.Editor.AssetImportInput.AssetName"/>).
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="assetName">The name of the generated output <see cref="Duality.Resource"/> (see <see cref="Duality.Editor.AssetImportInput.AssetName"/>).</param>
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
	}
}
