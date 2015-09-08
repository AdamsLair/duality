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
	}
}
