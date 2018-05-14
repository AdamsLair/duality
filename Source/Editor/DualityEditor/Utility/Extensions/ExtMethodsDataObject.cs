using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using Duality;
using Duality.Cloning;
using Duality.Drawing;
using Duality.Resources;

namespace Duality.Editor
{
	/// <summary>
	/// Data formats than can be used when storing data
	/// inside a <see cref="DataObject"/>
	/// </summary>
	public enum DataFormat
	{
		/// <summary>
		/// Use this format when storing an object reference in a DataObject.
		/// The reference will be maintained even after serialization.
		/// Note that storing a value type (ex. Vector3) in reference format
		/// will still lead to a copy of the data. Data stored in this format
		/// can be automatically converted to value formatted data (uses deep cloning).
		/// </summary>
		Reference,
		/// <summary>
		/// Use this format when storing an object in a DataObject that is 
		/// not meant to be a reference to an existing object. This format 
		/// of data cannot be automatically converted to reference format.
		/// </summary>
		Value
	}

	// TODO: document parameters and such
	public static class ExtMethodsDataObject
	{
		private const string ReferencePrefix = "SerializableReferenceWrapper:";
		private const string ValuePrefix = "SerializableWrapper:";

		/// <summary>
		/// Stores the specified non-<see cref="SerializableAttribute"/> data inside the specified data object using a serializable wrapper.
		/// </summary>
		/// <param name="elementType">The type of the given IEnumerable elements</param>
		/// <param name="format">The format to store the data in</param>
		public static void SetWrappedData(this IDataObject data, IEnumerable<object> values, Type elementType, DataFormat format)
		{
			string prefix = format == DataFormat.Reference ? ReferencePrefix : ValuePrefix;
			SerializableWrapper wrapper = format == DataFormat.Reference
				? new SerializableReferenceWrapper(values)
				: new SerializableWrapper(values);

			data.SetData(prefix + elementType.FullName, wrapper);
		}
		/// <summary>
		/// Determines whether the specified type of wrapped non-<see cref="SerializableAttribute"/> data is available in the data object.
		/// </summary>
		/// <param name="data"></param>
		/// <param name="format"></param>
		/// <param name="formatType"></param>
		/// <param name="allowConversion"></param>
		/// <returns></returns>
		public static bool GetWrappedDataPresent(this IDataObject data, Type elementType, DataFormat format, bool allowConversion = true)
		{
			return GetWrappedDataPresent(data, elementType.FullName, format, allowConversion);
		}
		/// <summary>
		/// Determines whether the specified type of wrapped non-<see cref="SerializableAttribute"/> data is available in the data object.
		/// </summary>
		/// <param name="data"></param>
		/// <param name="format"></param>
		/// <param name="formatType"></param>
		/// <param name="allowConversion"></param>
		/// <returns></returns>
		public static bool GetWrappedDataPresent(this IDataObject data, string format, DataFormat formatType, bool allowConversion = true)
		{
			string prefix = formatType == DataFormat.Reference ? ReferencePrefix : ValuePrefix;
			bool defaultFormatPresent = data.GetDataPresent(prefix + format);
			if (defaultFormatPresent || !allowConversion)
				return defaultFormatPresent;

			// Try convertible formats
			if (formatType == DataFormat.Value && data.GetWrappedDataPresent(format, DataFormat.Reference, false))
				return true;

			return false;
		}
		/// <summary>
		/// Retrieves the specified non-<see cref="SerializableAttribute"/> data from the specified data object using a serializable wrapper.
		/// </summary>
		/// <param name="data"></param>
		/// <param name="elementType"></param>
		/// <param name="formatType"></param>
		/// <param name="allowConversion"></param>
		/// <returns></returns>
		public static object[] GetWrappedData(this IDataObject data, Type elementType, DataFormat formatType, bool allowConversion = true)
		{
			return GetWrappedData(data, elementType.FullName, formatType, allowConversion);
		}
		/// <summary>
		/// Retrieves the specified non-<see cref="SerializableAttribute"/> data from the specified data object using a serializable wrapper.
		/// </summary>
		/// <param name="data"></param>
		/// <param name="format"></param>
		/// <param name="formatType"></param>
		/// <param name="allowConversion"></param>
		/// <returns></returns>
		public static object[] GetWrappedData(this IDataObject data, string format, DataFormat formatType, bool allowConversion = true)
		{
			string prefix = formatType == DataFormat.Reference ? ReferencePrefix : ValuePrefix;
			SerializableWrapper wrapper = data.GetData(prefix + format) as SerializableWrapper;
			if (wrapper != null) return wrapper.Data.ToArray();
			if (!allowConversion) return null;

			// Getting in the given format failed.
			// Try converting from other formats.
			object[] converted;
			if (formatType == DataFormat.Value
				&& (converted = data.GetWrappedData(format, DataFormat.Reference, false)) != null)
			{
				return converted.Select(obj => obj.DeepClone()).ToArray();
			}

			return null;
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

		public static void SetComponents(this IDataObject data, IEnumerable<Component> cmp, DataFormat format = DataFormat.Reference)
		{
			Component[] cmpArray = cmp.ToArray();
			if (cmpArray.Length > 0) data.SetWrappedData(cmpArray, typeof(Component), format);
		}
		public static bool ContainsComponents<T>(this IDataObject data, DataFormat format = DataFormat.Reference) where T : Component
		{
			if (!data.GetWrappedDataPresent(typeof(Component), format)) return false;
			object[] refArray = data.GetWrappedData(typeof(Component), format);
			return refArray != null && refArray.Any(c => c is T);
		}
		public static bool ContainsComponents(this IDataObject data, DataFormat format = DataFormat.Reference)
		{
			return data.ContainsComponents(typeof(Component), format);
		}
		public static bool ContainsComponents(this IDataObject data, Type cmpType, DataFormat format = DataFormat.Reference)
		{
			if (cmpType == null) cmpType = typeof(Component);
			if (!data.GetWrappedDataPresent(typeof(Component), format)) return false;
			object[] refArray = data.GetWrappedData(typeof(Component), format);
			return refArray != null && refArray.Any(c => cmpType.IsInstanceOfType(c));
		}
		public static T[] GetComponents<T>(this IDataObject data, DataFormat format = DataFormat.Reference) where T : Component
		{
			object[] compArray = data.GetWrappedData(typeof(Component), format);
			return compArray == null ? null : compArray.OfType<T>().ToArray();
		}
		public static Component[] GetComponents(this IDataObject data, DataFormat format = DataFormat.Reference)
		{
			return data.GetComponents(typeof(Component), format);
		}
		public static Component[] GetComponents(this IDataObject data, Type cmpType, DataFormat format = DataFormat.Reference)
		{
			if (cmpType == null) cmpType = typeof(Component);

			object[] compArray = data.GetWrappedData(typeof(Component), format);
			if (compArray == null)
				return null;

			return (
				from c in compArray
				where cmpType.IsInstanceOfType(c)
				select c as Component
				).ToArray();
		}
		public static bool TryGetComponents<T>(this IDataObject data, out T[] comps) where T : Component
		{
			comps = data.GetComponents<T>();
			return comps != null;
		}
		public static bool TryGetComponents<T>(this IDataObject data, DataFormat format, out T[] comps) where T : Component
		{
			comps = data.GetComponents<T>(format);
			return comps != null;
		}
		public static bool TryGetComponents(this IDataObject data, out Component[] comps)
		{
			comps = data.GetComponents();
			return comps != null;
		}
		public static bool TryGetComponents(this IDataObject data, DataFormat format, out Component[] comps)
		{
			comps = data.GetComponents(format);
			return comps != null;
		}
		public static bool TryGetComponents(this IDataObject data, Type cmpType, out Component[] comps)
		{
			comps = data.GetComponents(cmpType);
			return comps != null;
		}
		public static bool TryGetComponents(this IDataObject data, Type cmpType, DataFormat format, out Component[] comps)
		{
			comps = data.GetComponents(cmpType, format);
			return comps != null;
		}

		public static void SetGameObjects(this IDataObject data, IEnumerable<GameObject> obj, DataFormat format = DataFormat.Reference)
		{
			GameObject[] objArray = obj.ToArray();
			if (objArray.Length > 0) data.SetWrappedData(objArray, typeof(GameObject), format);
		}
		public static bool ContainsGameObjects(this IDataObject data, DataFormat format = DataFormat.Reference)
		{
			return data.GetWrappedDataPresent(typeof(GameObject), format);
		}
		public static GameObject[] GetGameObjects(this IDataObject data, DataFormat format = DataFormat.Reference)
		{
			object[] objects = data.GetWrappedData(typeof(GameObject), format);
			if (objects == null) return null;
			return objects.OfType<GameObject>().ToArray();
		}
		public static bool TryGetGameObjects(this IDataObject data, out GameObject[] objects)
		{
			if (!data.ContainsGameObjects())
			{
				objects = null;
				return false;
			}
			objects = data.GetGameObjects();
			return objects != null;
		}
		public static bool TryGetGameObjects(this IDataObject data, DataFormat format, out GameObject[] objects)
		{
			if (!data.ContainsGameObjects(format))
			{
				objects = null;
				return false;
			}
			objects = data.GetGameObjects(format);
			return objects != null;
		}

		public static void SetContentRefs(this IDataObject data, IEnumerable<IContentRef> content)
		{
			if (!content.Any()) return;
			data.SetWrappedData(content.ToArray(), typeof(IContentRef), DataFormat.Value);
		}
		public static bool ContainsContentRefs<T>(this IDataObject data) where T : Resource
		{
			if (!data.GetWrappedDataPresent(typeof(IContentRef), DataFormat.Value)) return false;
			object[] refArray = data.GetWrappedData(typeof(IContentRef), DataFormat.Value);
			return refArray != null && refArray.OfType<IContentRef>().Any(r => r.Is<T>());
		}
		public static bool ContainsContentRefs(this IDataObject data, Type resType = null)
		{
			if (resType == null) resType = typeof(Resource);
			if (!data.GetWrappedDataPresent(typeof(IContentRef), DataFormat.Value)) return false;
			object[] refArray = data.GetWrappedData(typeof(IContentRef), DataFormat.Value);
			return refArray != null && refArray.OfType<IContentRef>().Any(r => r.Is(resType));
		}
		public static ContentRef<T>[] GetContentRefs<T>(this IDataObject data) where T : Resource
		{
			if (!data.GetWrappedDataPresent(typeof(IContentRef), DataFormat.Value)) return null;
			object[] refArray = data.GetWrappedData(typeof(IContentRef), DataFormat.Value);
			return refArray == null ? null : refArray.OfType<IContentRef>()
				.Where(r => r.Is<T>()).Select(r => r.As<T>()).ToArray();
		}
		public static IContentRef[] GetContentRefs(this IDataObject data, Type resType = null)
		{
			if (resType == null) resType = typeof(Resource);
			if (!data.GetWrappedDataPresent(typeof(IContentRef), DataFormat.Value)) return null;
			object[] refArray = data.GetWrappedData(typeof(IContentRef), DataFormat.Value);
			return refArray == null ? null : refArray.OfType<IContentRef>()
				.Where(r => r.Is(resType)).ToArray();
		}
		public static bool TryGetContentRefs(this IDataObject data, out IContentRef[] content)
		{
			content = data.GetContentRefs();
			return content != null;
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
			if (objArray.Length > 0) data.SetWrappedData(objArray, typeof(BatchInfo), DataFormat.Reference);
		}
		public static bool ContainsBatchInfos(this IDataObject data)
		{
			return data.GetWrappedDataPresent(typeof(BatchInfo), DataFormat.Reference);
		}
		public static BatchInfo[] GetBatchInfos(this IDataObject data)
		{
			object[] batchArray = data.GetWrappedData(typeof(BatchInfo), DataFormat.Reference);
			return batchArray == null ? null : batchArray.OfType<BatchInfo>().Select(b => new BatchInfo(b)).ToArray();
		}

		public static void SetIColorData(this IDataObject data, IEnumerable<IColorData> color)
		{
			IColorData[] clrArray = color.ToArray();
			if (clrArray.Length == 0) return;
			data.SetWrappedData(clrArray, typeof(IColorData), DataFormat.Value);

			DataObject dataObj = data as DataObject;
			if (dataObj != null)
			{
				var rgbaQuery = clrArray.Select(c => c.ConvertTo<ColorRgba>());
				dataObj.SetText(rgbaQuery.ToString(c => string.Format("{0},{1},{2},{3}", c.R, c.G, c.B, c.A), ", "));
			}
		}
		public static bool ContainsIColorData(this IDataObject data)
		{
			if (data.GetWrappedDataPresent(typeof(IColorData), DataFormat.Value))
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
			if (data.GetWrappedDataPresent(typeof(IColorData), DataFormat.Value))
			{
				clrArray = data.GetWrappedData(typeof(IColorData), DataFormat.Value) as IColorData[];
			}
			else if (data.ContainsString())
			{
				clrArray = ParseIColorData(data.GetString());
			}

			if (clrArray != null)
			{
				// Don't care which format? Great, just return the array as is
				if (typeof(T) == typeof(IColorData)) return (T[])(object)clrArray;
				// Convert to specific format
				return clrArray.Select<IColorData,T>(ic => ic is T ? (T)ic : ic.ConvertTo<T>()).ToArray();
			}
			else
				return null;
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
			var sc = new System.Collections.Specialized.StringCollection();
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
