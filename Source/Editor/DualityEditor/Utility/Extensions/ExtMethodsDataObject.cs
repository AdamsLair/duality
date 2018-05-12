using System;
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
	// TODO: document
	public enum DataFormat
	{
		Reference,
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
		/// <param name="format">The format to store the data in</param>
		public static void SetWrappedData(this IDataObject data, object value, DataFormat format)
		{
			string prefix = format == DataFormat.Reference ? ReferencePrefix : ValuePrefix;
			SerializableWrapper wrapper = format == DataFormat.Reference
				? new SerializableReferenceWrapper(value)
				: new SerializableWrapper(value);

			data.SetData(prefix + value.GetType().FullName, wrapper);
		}
		/// <summary>
		/// Determines whether the specified type of wrapped non-<see cref="SerializableAttribute"/> data is available in the data object.
		/// </summary>
		/// <param name="data"></param>
		/// <param name="format"></param>
		/// <param name="formatType"></param>
		/// <param name="allowConversion"></param>
		/// <returns></returns>
		public static bool GetWrappedDataPresent(this IDataObject data, Type type, DataFormat format, bool allowConversion = true)
		{
			return GetWrappedDataPresent(data, type.FullName, format);
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
		/// <param name="format"></param>
		/// <param name="formatType"></param>
		/// <param name="allowConversion"></param>
		/// <returns></returns>
		public static object GetWrappedData(this IDataObject data, Type format, DataFormat formatType, bool allowConversion = true)
		{
			return GetWrappedData(data, format.FullName, formatType, allowConversion);
		}
		/// <summary>
		/// Retrieves the specified non-<see cref="SerializableAttribute"/> data from the specified data object using a serializable wrapper.
		/// </summary>
		/// <param name="data"></param>
		/// <param name="format"></param>
		/// <param name="formatType"></param>
		/// <param name="allowConversion"></param>
		/// <returns></returns>
		public static object GetWrappedData(this IDataObject data, string format, DataFormat formatType, bool allowConversion = true)
		{
			string prefix = formatType == DataFormat.Reference ? ReferencePrefix : ValuePrefix;
			SerializableWrapper wrapper = data.GetData(prefix + format) as SerializableWrapper;
			if (wrapper != null) return wrapper.Data;
			if (!allowConversion) return null;

			// Getting in the given format failed.
			// Try converting from other formats.
			object converted;
			if (formatType == DataFormat.Value 
				&& (converted = data.GetWrappedData(format, DataFormat.Reference, false)) != null)
				return converted.DeepClone();

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
			if (cmpArray.Length > 0) data.SetWrappedData(cmpArray, format);
		}
		public static bool ContainsComponents<T>(this IDataObject data, DataFormat format = DataFormat.Reference) where T : Component
		{
			if (!data.GetWrappedDataPresent(typeof(Component[]), format)) return false;
			Component[] refArray = data.GetWrappedData(typeof(Component[]), format) as Component[];
			return refArray != null && refArray.Any(c => c is T);
		}
		public static bool ContainsComponents(this IDataObject data, DataFormat format = DataFormat.Reference)
		{
			return data.ContainsComponents(typeof(Component), format);
		}
		public static bool ContainsComponents(this IDataObject data, Type cmpType, DataFormat format = DataFormat.Reference)
		{
			if (cmpType == null) cmpType = typeof(Component);
			if (!data.GetWrappedDataPresent(typeof(Component[]), format)) return false;
			Component[] refArray = data.GetWrappedData(typeof(Component[]), format) as Component[];
			return refArray != null && refArray.Any(c => cmpType.IsInstanceOfType(c));
		}
		public static T[] GetComponents<T>(this IDataObject data, DataFormat format = DataFormat.Reference) where T : Component
		{
			if (!data.GetWrappedDataPresent(typeof(Component[]), format)) return null;
			Component[] refArray = data.GetWrappedData(typeof(Component[]), format) as Component[] ?? new Component[0];
			return refArray.OfType<T>().ToArray();
		}
		public static Component[] GetComponents(this IDataObject data, DataFormat format = DataFormat.Reference)
		{
			return data.GetComponents(typeof(Component), format);
		}
		public static Component[] GetComponents(this IDataObject data, Type cmpType, DataFormat format = DataFormat.Reference)
		{
			if (cmpType == null) cmpType = typeof(Component);
			if (!data.GetWrappedDataPresent(typeof(Component[]), format)) return null;
			Component[] refArray = data.GetWrappedData(typeof(Component[]), format) as Component[] ?? new Component[0];
			return (
				from c in refArray
				where cmpType.IsInstanceOfType(c)
				select c
				).ToArray();
		}

		public static void SetGameObjects(this IDataObject data, IEnumerable<GameObject> obj, DataFormat format = DataFormat.Reference)
		{
			GameObject[] objArray = obj.ToArray();
			if (objArray.Length > 0) data.SetWrappedData(objArray, format);
		}
		public static bool ContainsGameObjects(this IDataObject data, DataFormat format = DataFormat.Reference)
		{
			return data.GetWrappedDataPresent(typeof(GameObject[]), format);
		}
		public static GameObject[] GetGameObjects(this IDataObject data, DataFormat format = DataFormat.Reference)
		{
			return data.GetWrappedData(typeof(GameObject[]), format) as GameObject[] ?? new GameObject[0];
		}

		public static void SetContentRefs(this IDataObject data, IEnumerable<IContentRef> content)
		{
			if (!content.Any()) return;
			data.SetWrappedData(content.ToArray(), DataFormat.Value);
		}
		public static bool ContainsContentRefs<T>(this IDataObject data) where T : Resource
		{
			if (!data.GetWrappedDataPresent(typeof(IContentRef[]), DataFormat.Value)) return false;
			IContentRef[] refArray = data.GetWrappedData(typeof(IContentRef[]), DataFormat.Value) as IContentRef[];
			return refArray != null && refArray.Any(r => r.Is<T>());
		}
		public static bool ContainsContentRefs(this IDataObject data, Type resType = null)
		{
			if (resType == null) resType = typeof(Resource);
			if (!data.GetWrappedDataPresent(typeof(IContentRef[]), DataFormat.Value)) return false;
			IContentRef[] refArray = data.GetWrappedData(typeof(IContentRef[]), DataFormat.Value) as IContentRef[];
			return refArray != null && refArray.Any(r => r.Is(resType));
		}
		public static ContentRef<T>[] GetContentRefs<T>(this IDataObject data) where T : Resource
		{
			if (!data.GetWrappedDataPresent(typeof(IContentRef[]), DataFormat.Value)) return null;
			IContentRef[] refArray = data.GetWrappedData(typeof(IContentRef[]), DataFormat.Value) as IContentRef[] ?? new IContentRef[0];
			return (
				from r in refArray
				where r.Is<T>()
				select r.As<T>()
				).ToArray();
		}
		public static IContentRef[] GetContentRefs(this IDataObject data, Type resType = null)
		{
			if (resType == null) resType = typeof(Resource);
			if (!data.GetWrappedDataPresent(typeof(IContentRef[]), DataFormat.Value)) return null;
			IContentRef[] refArray = data.GetWrappedData(typeof(IContentRef[]), DataFormat.Value) as IContentRef[] ?? new IContentRef[0];
			return (
				from r in refArray
				where r.Is(resType)
				select r
				).ToArray();
		}
		
		public static void SetBatchInfos(this IDataObject data, IEnumerable<BatchInfo> obj)
		{
			BatchInfo[] objArray = obj.ToArray();
			if (objArray.Length > 0) data.SetWrappedData(objArray, DataFormat.Reference);
		}
		public static bool ContainsBatchInfos(this IDataObject data)
		{
			return data.GetWrappedDataPresent(typeof(BatchInfo[]), DataFormat.Reference);
		}
		public static BatchInfo[] GetBatchInfos(this IDataObject data)
		{
			return (data.GetWrappedData(typeof(BatchInfo[]), DataFormat.Reference) as BatchInfo[]).Select(b => new BatchInfo(b)).ToArray();
		}

		public static void SetIColorData(this IDataObject data, IEnumerable<IColorData> color)
		{
			if (!color.Any()) return;
			data.SetWrappedData(color.ToArray(), DataFormat.Value);

			DataObject dataObj = data as DataObject;
			if (dataObj != null)
			{
				var rgbaQuery = color.Select(c => c.ConvertTo<ColorRgba>());
				dataObj.SetText(rgbaQuery.ToString(c => string.Format("{0},{1},{2},{3}", c.R, c.G, c.B, c.A), ", "));
			}
		}
		public static bool ContainsIColorData(this IDataObject data)
		{
			if (data.GetWrappedDataPresent(typeof(IColorData[]), DataFormat.Value))
				return true;

			if (data.ContainsString())
			{
				string valString = data.GetString();
				string[] token = valString.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
				byte[] valToken = new byte[4];
				valToken[3] = 255;

				bool success = true;
				for (int i = 0; i < token.Length; i++)
				{
					token[i] = token[i].Trim();
					if (!byte.TryParse(token[i], out valToken[i]))
					{
						success = false;
						break;
					}
				}

				if (success) return true;
			}

			return false;
		}
		public static T[] GetIColorData<T>(this IDataObject data) where T : IColorData
		{
			IColorData[] clrArray = null;
			if (data.GetWrappedDataPresent(typeof(IColorData[]), DataFormat.Value))
			{
				clrArray = data.GetWrappedData(typeof(IColorData[]), DataFormat.Value) as IColorData[];
			}
			else if (data.ContainsString())
			{
				string valString = data.GetString();
				string[] token = valString.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
				byte[] valToken = new byte[4];
				valToken[3] = 255;

				bool success = true;
				for (int i = 0; i < token.Length; i++)
				{
					token[i] = token[i].Trim();
					if (!byte.TryParse(token[i], out valToken[i]))
					{
						success = false;
						break;
					}
				}

				if (success) clrArray = new IColorData[] { new ColorRgba(valToken[0], valToken[1], valToken[2], valToken[3]) };
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
	}
}
