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
		public static string GetSourceMediaBaseDir(ContentRef<Resource> resource)
		{
			string resFullNameInData = PathHelper.MakeFilePathRelative(resource.FullName, DualityApp.DataDirectory);
			string resDirInData = Path.GetDirectoryName(resFullNameInData);
			string sourceMediaDir = Path.Combine(EditorHelper.AssetsDirectory, resDirInData);
			return sourceMediaDir;
		}
		
		/// <summary>
		/// Retrieves a custom data value from the specified Resource's <see cref="AssetInfo"/> data.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="resource"></param>
		/// <param name="parameterName"></param>
		/// <param name="value"></param>
		public static bool GetAssetInfoCustomValue<T>(Resource resource, string parameterName, out T value)
		{
			// Otherwise, perform a defensive lookup
			AssetInfo assetInfo = (resource != null) ? resource.AssetInfo : null;
			Dictionary<string,object> customData = (assetInfo != null) ? assetInfo.CustomData : null;
			object valueObj;
			if (customData == null || !customData.TryGetValue(parameterName, out valueObj))
			{
				value = default(T);
				return false;
			}

			// If we have a matching object, return it directly
			if (valueObj is T)
			{
				value = (T)valueObj;
				return true;
			}

			// Otherwise, try to cast it
			try
			{
				value = (T)Convert.ChangeType(valueObj, typeof(T));
				return true;
			}
			catch (Exception) { }

			// If everything failed, return false and a default value
			value = default(T);
			return false;
		}
		/// <summary>
		/// Sets a custom data value in the specified Resource's <see cref="AssetInfo"/> data.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="resource"></param>
		/// <param name="parameterName"></param>
		/// <param name="value"></param>
		public static void SetAssetInfoCustomValue<T>(Resource resource, string parameterName, T value)
		{
			// Don't allow the usage of editor types here. They'll be serialized as part
			// of the Resource, which can be loaded in a non-editor context
			Type valueType = (value != null) ? value.GetType() : typeof(T);
			IEnumerable<Assembly> editorAssemblies = DualityEditorApp.GetDualityEditorAssemblies();
			if (editorAssemblies.Contains(valueType.Assembly))
				throw new ArgumentException("Editor-related types cannot be used as import / export parameters.", "value");

			// If the target Resource doesn't exist, fail silently
			if (resource == null) return;

			// Retrieve and / or initialize the Resource's asset info custom data
			AssetInfo assetInfo = resource.AssetInfo ?? new AssetInfo();
			Dictionary<string,object> customData = assetInfo.CustomData ?? new Dictionary<string,object>();
			
			// Assign the new parameter value and overwrite old values in the process
			customData[parameterName] = value;

			// Assign the values that we may have initialized just now
			assetInfo.CustomData = customData;
			resource.AssetInfo = assetInfo;
		}
	}
}
