using System;
using System.Collections.Generic;
using System.Linq;

using Duality;

namespace Duality.Editor
{
	public static class ExtMethodsIAssetImportEnvironment
	{
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
