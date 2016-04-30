using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

using Duality;
using Duality.IO;

namespace Duality.Editor.AssetManagement
{
	/// <summary>
	/// Provides some internal helper functions for asset-related tasks.
	/// 
	/// If there is any other, more specific place a given function fits, that place should be preferred.
	/// Due to the fact that it's a general collection of helper methods, in an ideal world, this class 
	/// wouldn't even exist. Don't add stuff if it can be avoided.
	/// </summary>
	internal static class AssetInternalHelper
	{
		/// <summary>
		/// Determines the expected base directory of the specified <see cref="Resource"/> instances source files,
		/// which were used during the most recent import and can be re-used during export and re-import operations 
		/// of that <see cref="Resource"/>.
		/// </summary>
		/// <param name="resource"></param>
		/// <returns></returns>
		public static string GetSourceMediaBaseDir(ContentRef<Resource> resource)
		{
			string resFullNameInData = PathHelper.MakeFilePathRelative(resource.FullName, DualityApp.DataDirectory);
			string resDirInData = Path.GetDirectoryName(resFullNameInData);
			string sourceMediaDir = Path.Combine(EditorHelper.SourceMediaDirectory, resDirInData);
			return sourceMediaDir;
		}
	}
}
