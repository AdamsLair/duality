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

		public const string ComponentFormat = "Component";
		public const string GameObjectFormat = "GameObject";
		public const string ContentRefFormat = "ContentRef";
		public const string BatchInfoFormat = "BatchInfo";
		public const string ColorDataFormat = "ColorData";

		/// <summary>
		/// Stores the specified non-<see cref="SerializableAttribute"/> data inside the specified data object using a serializable wrapper.
		/// </summary>
		/// <param name="dataFormat">The format of the data being stored</param>
		/// <param name="storage">The type of data being stored</param>
		public static void SetWrappedData(this IDataObject data, IEnumerable<object> values, string dataFormat, DataObjectStorage storage)
		{
			string prefix = storage == DataObjectStorage.Reference ? ReferencePrefix : ValuePrefix;
			SerializableWrapper wrapper = storage == DataObjectStorage.Reference
				? new SerializableReferenceWrapper(values)
				: new SerializableWrapper(values);

			data.SetData(prefix + dataFormat, wrapper);
		}
		/// <summary>
		/// Determines whether the specified format of wrapped non-<see cref="SerializableAttribute"/> data is available in the data object.
		/// </summary>
		/// <param name="data"></param>
		/// <param name="dataFormat">The format of data to look for</param>
		/// <param name="storage">Whether to look for stored references or values</param>
		/// <returns></returns>
		public static bool GetWrappedDataPresent(this IDataObject data, string dataFormat, DataObjectStorage storage)
		{
			string prefix = storage == DataObjectStorage.Reference ? ReferencePrefix : ValuePrefix;
			bool defaultFormatPresent = data.GetDataPresent(prefix + dataFormat);
			if (defaultFormatPresent)
				return defaultFormatPresent;

			// If retrieving by-value failed, try retrieving by-reference and cloning the result
			if (storage == DataObjectStorage.Value && data.GetWrappedDataPresent(dataFormat, DataObjectStorage.Reference))
				return true;

			return false;
		}
		/// <summary>
		/// Retrieves the specified non-<see cref="SerializableAttribute"/> data from the specified data object using a serializable wrapper.
		/// </summary>
		/// <param name="data"></param>
		/// <param name="dataFormat">The format of the data being retrieved</param>
		/// <param name="storage">Whether to look for stored references or values</param>
		/// <param name="allowStorageConversion">Whether or not to attempt converting data from other <see cref="DataObjectStorage"/>s</param>
		/// <returns></returns>
		public static IEnumerable<object> GetWrappedData(this IDataObject data, string dataFormat, DataObjectStorage storage)
		{
			string prefix = storage == DataObjectStorage.Reference ? ReferencePrefix : ValuePrefix;
			SerializableWrapper wrapper = data.GetData(prefix + dataFormat) as SerializableWrapper;
			if (wrapper != null) return wrapper.Data;

			// If retrieving by-value failed, try retrieving by-reference and cloning the result
			IEnumerable<object> converted;
			if (storage == DataObjectStorage.Value && (converted = data.GetWrappedData(dataFormat, DataObjectStorage.Reference)) != null)
			{
				return converted.Select(obj => obj.DeepClone());
			}

			return null;
		}
		public static bool TryGetWrappedData(this IDataObject data, string dataFormat, DataObjectStorage storage, out IEnumerable<object> wrappedData)
		{
			if (!data.GetWrappedDataPresent(dataFormat, storage))
			{
				wrappedData = null;
				return false;
			}
			wrappedData = data.GetWrappedData(dataFormat, storage);
			return wrappedData != null;
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
		public static Component[] GetComponents(this IDataObject data, DataObjectStorage storage = DataObjectStorage.Reference)
		{
			return data.GetComponents(typeof(Component), storage);
		}
		public static Component[] GetComponents(this IDataObject data, Type cmpType, DataObjectStorage storage = DataObjectStorage.Reference)
		{
			IEnumerable<object> wrappedData = data.GetWrappedData(ComponentFormat, storage);
			if (wrappedData == null) return null;

			if (cmpType == null) cmpType = typeof(Component);
			return wrappedData.Where(c => cmpType.IsInstanceOfType(c)).OfType<Component>().ToArray();
		}
		public static bool TryGetComponents(this IDataObject data, DataObjectStorage storage, out Component[] comps)
		{
			comps = data.GetComponents(storage);
			return comps != null;
		}
		public static bool TryGetComponents(this IDataObject data, Type cmpType, DataObjectStorage storage, out Component[] comps)
		{
			comps = data.GetComponents(cmpType, storage);
			return comps != null;
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
			IEnumerable<object> wrappedData = data.GetWrappedData(GameObjectFormat, storage);
			return wrappedData == null ? null : wrappedData.OfType<GameObject>().ToArray();
		}
		public static bool TryGetGameObjects(this IDataObject data, DataObjectStorage storage, out GameObject[] objects)
		{
			if (!data.ContainsGameObjects(storage))
			{
				objects = null;
				return false;
			}
			objects = data.GetGameObjects(storage);
			return objects != null;
		}

		public static void SetContentRefs(this IDataObject data, IEnumerable<IContentRef> content)
		{
			IContentRef[] contentArr = content.ToArray();
			if (contentArr.Length > 0) data.SetWrappedData(contentArr, ContentRefFormat, DataObjectStorage.Value);
		}
		public static bool ContainsContentRefs<T>(this IDataObject data) where T : Resource
		{
			IEnumerable<object> wrappedData;
			if (!data.TryGetWrappedData(ContentRefFormat, DataObjectStorage.Value, out wrappedData))
				return false;
			return wrappedData.OfType<IContentRef>().Any(r => r.Is<T>());
		}
		public static bool ContainsContentRefs(this IDataObject data, Type resType = null)
		{
			IEnumerable<object> wrappedData;
			if (!data.TryGetWrappedData(ContentRefFormat, DataObjectStorage.Value, out wrappedData))
				return false;

			if (resType == null) resType = typeof(Resource);
			return wrappedData.OfType<IContentRef>().Any(r => r.Is(resType));
		}
		public static ContentRef<T>[] GetContentRefs<T>(this IDataObject data) where T : Resource
		{
			IEnumerable<object> wrappedData;
			if (!data.TryGetWrappedData(ContentRefFormat, DataObjectStorage.Value, out wrappedData))
				return null;

			return wrappedData.OfType<IContentRef>()
				.Where(r => r.Is<T>()).Select(r => r.As<T>()).ToArray();
		}
		public static IContentRef[] GetContentRefs(this IDataObject data, Type resType = null)
		{
			IEnumerable<object> wrappedData;
			if (!data.TryGetWrappedData(ContentRefFormat, DataObjectStorage.Value, out wrappedData))
				return null;

			if (resType == null) resType = typeof(Resource);
			return wrappedData.OfType<IContentRef>()
				.Where(r => r.Is(resType)).ToArray();
		}
		public static bool TryGetContentRefs(this IDataObject data, Type resType, out IContentRef[] content)
		{
			content = data.GetContentRefs(resType);
			return content != null;
		}
		public static bool TryGetContentRefs<T>(this IDataObject data, out ContentRef<T>[] content) where T : Resource
		{
			content = data.GetContentRefs<T>();
			return content != null;
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
			IEnumerable<object> wrappedData = data.GetWrappedData(BatchInfoFormat, DataObjectStorage.Value);
			return wrappedData == null ? null : wrappedData.OfType<BatchInfo>().Select(b => new BatchInfo(b)).ToArray();
		}
		public static bool TryGetBatchInfos(this IDataObject data, out BatchInfo[] batches)
		{
			if (!data.ContainsBatchInfos())
			{
				batches = null;
				return false;
			}
			batches = data.GetBatchInfos();
			return batches != null;
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
			IColorData[] clrArray = null;
			IEnumerable<object> wrappedData;
			if (data.TryGetWrappedData(ColorDataFormat, DataObjectStorage.Value, out wrappedData))
				clrArray = wrappedData.OfType<IColorData>().ToArray();
			else if (data.ContainsString())
				clrArray = ParseIColorData(data.GetString());

			if (clrArray != null)
			{
				// Don't care which format? Great, just return the array as is
				if (typeof(T) == typeof(IColorData)) return clrArray.OfType<T>().ToArray();
				// Convert to specific format
				return clrArray.Select(ic => ic is T ? (T)ic : ic.ConvertTo<T>()).ToArray();
			}
			else
				return null;
		}
		public static bool TryGetIColorData<T>(this IDataObject data, out T[] colorData) where T : IColorData
		{
			if (!data.ContainsIColorData())
			{
				colorData = null;
				return false;
			}
			colorData = data.GetIColorData<T>();
			return true;
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
