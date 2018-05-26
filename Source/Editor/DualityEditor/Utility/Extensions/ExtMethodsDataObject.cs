using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using Duality.Cloning;
using Duality.Drawing;

namespace Duality.Editor
{
	public static class ExtMethodsDataObject
	{
		private const string ReferencePrefix = "SerializableReferenceWrapper:";
		private const string ValuePrefix = "SerializableWrapper:";

		public static readonly string ComponentFormat = typeof(Component).FullName;
		public static readonly string GameObjectFormat = typeof(GameObject).FullName;
		public static readonly string ContentRefFormat = typeof(IContentRef).FullName;
		public static readonly string BatchInfoFormat = typeof(BatchInfo).FullName;
		public static readonly string ColorDataFormat = typeof(IColorData).FullName;

		private static string GetWrappedDataFormat(string dataFormat, DataObjectStorage storage)
		{
			string prefix = (storage == DataObjectStorage.Reference) ? ReferencePrefix : ValuePrefix;
			return prefix + dataFormat;
		}

		/// <summary>
		/// Stores the specified non-<see cref="SerializableAttribute"/> data inside the specified data object using a serializable wrapper.
		/// </summary>
		/// <param name="dataFormat">The format of the data being stored</param>
		/// <param name="storage">The type of data being stored</param>
		public static void SetWrappedData(this IDataObject data, IEnumerable<object> values, string dataFormat, DataObjectStorage storage)
		{
			string wrappedFormat = GetWrappedDataFormat(dataFormat, storage);
			SerializableWrapper wrapper = storage == DataObjectStorage.Reference
				? new SerializableReferenceWrapper(values)
				: new SerializableWrapper(values);

			data.SetData(wrappedFormat, wrapper);
		}
		/// <summary>
		/// Determines whether the specified format of wrapped non-<see cref="SerializableAttribute"/> data is available in the data object.
		/// </summary>
		/// <param name="dataFormat">The format of data to look for</param>
		/// <param name="storage">Whether to look for stored references or values</param>
		public static bool GetWrappedDataPresent(this IDataObject data, string dataFormat, DataObjectStorage storage)
		{
			string wrappedFormat = GetWrappedDataFormat(dataFormat, storage);
			bool defaultFormatPresent = data.GetDataPresent(wrappedFormat);
			if (defaultFormatPresent) return true;

			// If retrieving by-value failed, try retrieving a reference which can be cloned later
			if (storage == DataObjectStorage.Value && data.GetWrappedDataPresent(dataFormat, DataObjectStorage.Reference))
				return true;

			return false;
		}
		/// <summary>
		/// Retrieves the specified non-<see cref="SerializableAttribute"/> data from the specified data object using a serializable wrapper.
		/// </summary>
		/// <param name="dataFormat">The format of the data being retrieved</param>
		/// <param name="storage">Whether to look for stored references or values</param>
		public static IEnumerable<object> GetWrappedData(this IDataObject data, string dataFormat, DataObjectStorage storage)
		{
			IEnumerable<object> wrappedData;
			if (!data.TryGetWrappedData(dataFormat, storage, out wrappedData))
				return null;
			else
				return wrappedData;
		}
		public static bool TryGetWrappedData(this IDataObject data, string dataFormat, DataObjectStorage storage, out IEnumerable<object> wrappedData)
		{
			// Retrieve wrapped data of the specified format and storage type
			string wrappedFormat = GetWrappedDataFormat(dataFormat, storage);
			SerializableWrapper wrapper = data.GetData(wrappedFormat) as SerializableWrapper;
			if (wrapper != null && wrapper.Data != null && wrapper.Data.Count > 0)
			{
				wrappedData = wrapper.Data;
				return true;
			}

			// If retrieving by-value failed, try retrieving by-reference and cloning the result
			if (storage == DataObjectStorage.Value)
			{
				IEnumerable<object> byReferenceData;
				if (data.TryGetWrappedData(dataFormat, DataObjectStorage.Reference, out byReferenceData))
				{
					wrappedData = byReferenceData.Select(obj => obj.DeepClone()).ToArray();
					return true;
				}
			}

			wrappedData = null;
			return false;
		}

		public static void SetAllowedConvertOp(this IDataObject data, ConvertOperation.Operation allowedOp)
		{
			data.SetData(typeof(ConvertOperation.Operation), allowedOp);
		}
		public static ConvertOperation.Operation GetAllowedConvertOp(this IDataObject data)
		{
			if (data.GetDataPresent(typeof(ConvertOperation.Operation)))
				return (ConvertOperation.Operation)data.GetData(typeof(ConvertOperation.Operation));
			else
				return ConvertOperation.Operation.All;
		}

		public static void SetComponents(this IDataObject data, IEnumerable<Component> cmp, DataObjectStorage storage = DataObjectStorage.Reference)
		{
			Component[] cmpArray = cmp.ToArray();
			if (cmpArray.Length > 0) data.SetWrappedData(cmpArray, ComponentFormat, storage);
		}
		public static bool ContainsComponents(this IDataObject data, Type cmpType, DataObjectStorage storage = DataObjectStorage.Reference)
		{
			IEnumerable<object> wrappedData;
			if (!data.TryGetWrappedData(ComponentFormat, storage, out wrappedData))
				return false;

			if (cmpType == null) cmpType = typeof(Component);
			return wrappedData.Any(c => cmpType.IsInstanceOfType(c));
		}
		public static Component[] GetComponents(this IDataObject data, Type cmpType, DataObjectStorage storage = DataObjectStorage.Reference)
		{
			Component[] components;
			if (!data.TryGetComponents(cmpType, storage, out components))
				return null;
			else
				return components;
		}
		public static bool TryGetComponents(this IDataObject data, Type cmpType, DataObjectStorage storage, out Component[] components)
		{
			if (cmpType == null) cmpType = typeof(Component);

			// Retrieve all kinds of components from the data object
			IEnumerable<object> wrappedData;
			if (!data.TryGetWrappedData(ComponentFormat, storage, out wrappedData))
			{
				components = null;
				return false;
			}

			// Filter and return components that match the specified type
			components = wrappedData.Where(c => cmpType.IsInstanceOfType(c)).OfType<Component>().ToArray();
			return components.Any();
		}

		public static void SetGameObjects(this IDataObject data, IEnumerable<GameObject> obj, DataObjectStorage storage = DataObjectStorage.Reference)
		{
			GameObject[] objArray = obj.ToArray();
			if (objArray.Length > 0) data.SetWrappedData(objArray, GameObjectFormat, storage);
		}
		public static bool ContainsGameObjects(this IDataObject data, DataObjectStorage storage = DataObjectStorage.Reference)
		{
			return data.GetWrappedDataPresent(GameObjectFormat, storage);
		}
		public static GameObject[] GetGameObjects(this IDataObject data, DataObjectStorage storage = DataObjectStorage.Reference)
		{
			GameObject[] objects;
			if (!data.TryGetGameObjects(storage, out objects))
				return null;
			else
				return objects;
		}
		public static bool TryGetGameObjects(this IDataObject data, DataObjectStorage storage, out GameObject[] objects)
		{
			// Retrieve all GameObjects from the data object
			IEnumerable<object> wrappedData;
			if (!data.TryGetWrappedData(GameObjectFormat, storage, out wrappedData))
			{
				objects = null;
				return false;
			}

			// Return the retrieved GameObjects
			objects = wrappedData.OfType<GameObject>().ToArray();
			return objects.Any();
		}

		public static void SetContentRefs(this IDataObject data, IEnumerable<IContentRef> content)
		{
			IContentRef[] contentArr = content.ToArray();
			if (contentArr.Length > 0) data.SetWrappedData(contentArr, ContentRefFormat, DataObjectStorage.Value);
		}
		public static bool ContainsContentRefs(this IDataObject data, Type resType = null)
		{
			IEnumerable<object> wrappedData;
			if (!data.TryGetWrappedData(ContentRefFormat, DataObjectStorage.Value, out wrappedData))
				return false;

			if (resType == null) resType = typeof(Resource);
			return wrappedData.OfType<IContentRef>().Any(r => r.Is(resType));
		}
		public static IContentRef[] GetContentRefs(this IDataObject data, Type resType = null)
		{
			IContentRef[] contentRefs;
			if (!data.TryGetContentRefs(resType, out contentRefs))
				return null;
			else
				return contentRefs;
		}
		public static bool TryGetContentRefs(this IDataObject data, Type resType, out IContentRef[] content)
		{
			if (resType == null) resType = typeof(Resource);

			// Retrieve all kinds of resources from the data object
			IEnumerable<object> wrappedData;
			if (!data.TryGetWrappedData(ContentRefFormat, DataObjectStorage.Value, out wrappedData))
			{
				content = null;
				return false;
			}

			// Filter and return resources that match the specified type
			content = wrappedData.OfType<IContentRef>().Where(r => r.Is(resType)).ToArray();
			return content.Any();
		}

		public static void SetBatchInfos(this IDataObject data, IEnumerable<BatchInfo> obj)
		{
			BatchInfo[] objArray = obj.ToArray();
			if (objArray.Length > 0) data.SetWrappedData(objArray, BatchInfoFormat, DataObjectStorage.Value);
		}
		public static bool ContainsBatchInfos(this IDataObject data)
		{
			return data.GetWrappedDataPresent(BatchInfoFormat, DataObjectStorage.Value);
		}
		public static BatchInfo[] GetBatchInfos(this IDataObject data)
		{
			BatchInfo[] materials;
			if (!data.TryGetBatchInfos(out materials))
				return null;
			else
				return materials;
		}
		public static bool TryGetBatchInfos(this IDataObject data, out BatchInfo[] batches)
		{
			// Retrieve all materials from the data object
			IEnumerable<object> wrappedData;
			if (!data.TryGetWrappedData(BatchInfoFormat, DataObjectStorage.Value, out wrappedData))
			{
				batches = null;
				return false;
			}

			// Return the retrieved materials
			batches = wrappedData.OfType<BatchInfo>().ToArray();
			return batches.Any();
		}

		public static void SetIColorData(this IDataObject data, IEnumerable<IColorData> color)
		{
			IColorData[] clrArray = color.ToArray();
			if (clrArray.Length == 0) return;
			data.SetWrappedData(clrArray, ColorDataFormat, DataObjectStorage.Value);

			IEnumerable<ColorRgba> rgbaQuery = clrArray.Select(c => c.ConvertTo<ColorRgba>());
			data.SetString(rgbaQuery.ToString(c => string.Format("{0},{1},{2},{3}", c.R, c.G, c.B, c.A), ", "));
		}
		public static bool ContainsIColorData(this IDataObject data)
		{
			if (data.GetWrappedDataPresent(ColorDataFormat, DataObjectStorage.Value))
				return true;

			if (data.ContainsString())
			{
				IColorData[] clrArray;
				if (TryParseIColorData(data.GetString(), out clrArray))
					return true;
			}

			return false;
		}
		public static T[] GetIColorData<T>(this IDataObject data) where T : IColorData
		{
			T[] colors;
			if (!data.TryGetIColorData<T>(out colors))
				return null;
			else
				return colors;
		}
		public static bool TryGetIColorData<T>(this IDataObject data, out T[] colorData) where T : IColorData
		{
			IColorData[] colors = null;
			IEnumerable<object> wrappedData;
			if (data.TryGetWrappedData(ColorDataFormat, DataObjectStorage.Value, out wrappedData))
				colors = wrappedData.OfType<IColorData>().ToArray();
			else if (data.ContainsString())
				colors = ParseIColorData(data.GetString());

			if (colors == null)
			{
				colorData = null;
				return false;
			}

			// Don't care which format? Great, just return the array as is
			if (typeof(T) == typeof(IColorData))
				colorData = colors.OfType<T>().ToArray();
			// Otherwise, convert to the requested color format
			else
				colorData = colors.Select(c => c is T ? (T)c : c.ConvertTo<T>()).ToArray();

			return colorData.Any();
		}

		public static void SetString(this IDataObject data, string text)
		{
			data.SetData(text);
			data.SetData("Text", true, text);
			data.SetData("UnicodeText", true, text);
		}
		public static bool ContainsString(this IDataObject data)
		{
			return data.GetDataPresent(typeof(string)) ||
				data.GetDataPresent("Text", true) ||
				data.GetDataPresent("UnicodeText", true);
		}
		public static string GetString(this IDataObject data)
		{
			if (data.GetDataPresent(typeof(string)))
				return data.GetData(typeof(string)) as string;
			else if (data.GetDataPresent("UnicodeText", true))
				return data.GetData("UnicodeText", true) as string;
			else if (data.GetDataPresent("Text", true))
				return data.GetData("Text", true) as string;
			else
				return null;
		}

		public static void SetFiles(this DataObject data, IEnumerable<string> files)
		{
			StringCollection sc = new StringCollection();
			foreach (string file in files)
			{
				string path = Path.GetFullPath(file);
				if (File.Exists(path) || Directory.Exists(path))
				{
					sc.Add(path);
				}
			}
			if (sc.Count > 0) data.SetFileDropList(sc);
		}

		private static bool TryParseIColorData(string valString, out IColorData[] colorArray)
		{
			colorArray = ParseIColorData(valString);
			return colorArray != null;
		}
		private static IColorData[] ParseIColorData(string valString)
		{
			string[] token = valString.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
			byte[] valToken = new byte[4];
			valToken[3] = 255;

			// Prevent incorrect lengths of comma-separated values 
			// (which couldn't be a color) from being parsed
			if (!(token.Length == 3 || token.Length == 4))
				return null;

			for (int i = 0; i < token.Length; i++)
			{
				token[i] = token[i].Trim();
				if (!byte.TryParse(token[i], out valToken[i]))
				{
					return null;
				}
			}

			return new IColorData[] { new ColorRgba(valToken[0], valToken[1], valToken[2], valToken[3]) };
		}
	}
}
