using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Duality;
using Duality.IO;

namespace Duality.Editor
{
	public static class ExtMethodsDataConverter
	{
		/// <summary>
		/// Given a source <see cref="Resource"/> and matching function, this method finds an already existing
		/// target <see cref="Resource"/> to be used in a <see cref="DataConverter"/> operation.
		/// </summary>
		/// <typeparam name="TTarget"></typeparam>
		/// <typeparam name="TSource"></typeparam>
		/// <param name="source"></param>
		/// <param name="matchFunc"></param>
		/// <returns></returns>
		public static IEnumerable<TTarget> FindMatchingResources<TSource,TTarget>(this DataConverter conv, TSource source, Func<TSource,TTarget,bool> matchFunc) 
			where TTarget : Resource 
			where TSource : Resource
		{
			if (source == null)
			{
				return Enumerable.Empty<TTarget>();
			}
			else if (source.IsDefaultContent)
			{
				var defaultContent = ContentProvider.GetDefaultContent<TTarget>();
				return defaultContent.Res().Where(r => matchFunc(source, r));
			}
			else
			{
				// First try a direct approach
				string fileExt = Resource.GetFileExtByType<TTarget>();
				string targetPath = source.FullName + fileExt;
				TTarget match = ContentProvider.RequestContent<TTarget>(targetPath).Res;
				if (match != null && matchFunc(source, match)) return new[] { match };

				// If that fails, search for other matches
				string targetName = source.Name + fileExt;
				string[] resFilePaths = Resource.GetResourceFiles().ToArray();
				IEnumerable<string> resNameMatch = resFilePaths.Where(p => PathOp.GetFileName(p) == targetName);
				IEnumerable<string> resQuery = resNameMatch.Concat(resFilePaths).Distinct();
				List<TTarget> matches = new List<TTarget>();
				foreach (string resFile in resQuery)
				{
					if (!resFile.EndsWith(fileExt)) continue;
					match = ContentProvider.RequestContent<TTarget>(resFile).Res;
					if (match != null && matchFunc(source, match))
						matches.Add(match);
				}

				// Return all matching results
				return matches;
			}
		}
	}
}
