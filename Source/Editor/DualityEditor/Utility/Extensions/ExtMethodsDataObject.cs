using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;

using Duality;
using Duality.Drawing;
using Duality.Editor.Utility;
using Duality.Resources;

namespace Duality.Editor
{
	public static class ExtMethodsDataObject
	{
		private const string WrapperPrefix = "SerializableWrapper:";

		/// <summary>
		/// Stores the specified non-<see cref="SerializableAttribute"/> data inside the specified data object using a serializable wrapper.
		/// </summary>
		/// <param name="data"></param>
		/// <param name="value"></param>
		/// <param name="reference">Whether or not to store the value as a reference or to perform a clone of the value</param>
		public static void SetWrappedData(this IDataObject data, object value, bool reference = true)
		{
			data.SetData(WrapperPrefix + value.GetType().FullName, 
				reference 
					? new SerializableReferenceWrapper(value) 
					: new SerializableWrapper(value));
		}
		/// <summary>
		/// Determines whether the specified type of wrapped non-<see cref="SerializableAttribute"/> data is available in the data object.
		/// </summary>
		/// <param name="data"></param>
		/// <param name="format"></param>
		/// <returns></returns>
		public static bool GetWrappedDataPresent(this IDataObject data, Type format)
		{
			return GetWrappedDataPresent(data, format.FullName);
		}
		/// <summary>
		/// Determines whether the specified type of wrapped non-<see cref="SerializableAttribute"/> data is available in the data object.
		/// </summary>
		/// <param name="data"></param>
		/// <param name="format"></param>
		/// <returns></returns>
		public static bool GetWrappedDataPresent(this IDataObject data, string format)
		{
			return data.GetDataPresent(WrapperPrefix + format);
		}
		/// <summary>
		/// Retrieves the specified non-<see cref="SerializableAttribute"/> data from the specified data object using a serializable wrapper.
		/// </summary>
		/// <param name="data"></param>
		/// <param name="format"></param>
		/// <returns></returns>
		public static object GetWrappedData(this IDataObject data, Type format)
		{
			return GetWrappedData(data, format.FullName);
		}
		/// <summary>
		/// Retrieves the specified non-<see cref="SerializableAttribute"/> data from the specified data object using a serializable wrapper.
		/// </summary>
		/// <param name="data"></param>
		/// <param name="format"></param>
		/// <returns></returns>
		public static object GetWrappedData(this IDataObject data, string format)
		{
			SerializableWrapper wrapper = data.GetData(WrapperPrefix + format) as SerializableWrapper;
			if (wrapper != null)
				return wrapper.Data;
			else
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

		public static void SetComponentRefs(this IDataObject data, IEnumerable<Component> cmp)
		{
			Component[] cmpArray = cmp.ToArray();
			if (cmpArray.Length > 0) data.SetWrappedData(cmpArray);
		}
		public static bool ContainsComponentRefs<T>(this IDataObject data) where T : Component
		{
			if (!data.GetWrappedDataPresent(typeof(Component[]))) return false;
			Component[] refArray = data.GetWrappedData(typeof(Component[])) as Component[];
			return refArray.Any(c => c is T);
		}
		public static bool ContainsComponentRefs(this IDataObject data, Type cmpType = null)
		{
			if (cmpType == null) cmpType = typeof(Component);
			if (!data.GetWrappedDataPresent(typeof(Component[]))) return false;
			Component[] refArray = data.GetWrappedData(typeof(Component[])) as Component[];
			return refArray.Any(c => cmpType.IsInstanceOfType(c));
		}
		public static T[] GetComponentRefs<T>(this IDataObject data) where T : Component
		{
			if (!data.GetWrappedDataPresent(typeof(Component[]))) return null;
			Component[] refArray = data.GetWrappedData(typeof(Component[])) as Component[];
			return (
				from r in refArray
				where r is T
				select r as T
				).ToArray();
		}
		public static Component[] GetComponentRefs(this IDataObject data, Type cmpType = null)
		{
			if (cmpType == null) cmpType = typeof(Component);
			if (!data.GetWrappedDataPresent(typeof(Component[]))) return null;
			Component[] refArray = data.GetWrappedData(typeof(Component[])) as Component[];
			return (
				from c in refArray
				where cmpType.IsInstanceOfType(c)
				select c
				).ToArray();
		}

		public static void SetGameObjectRefs(this IDataObject data, IEnumerable<GameObject> obj)
		{
			GameObject[] objArray = obj.ToArray();
			if (objArray.Length > 0) data.SetWrappedData(objArray);
		}
		public static bool ContainsGameObjectRefs(this IDataObject data)
		{
			return data.GetWrappedDataPresent(typeof(GameObject[]));
		}
		public static GameObject[] GetGameObjectRefs(this IDataObject data)
		{
			return data.GetWrappedData(typeof(GameObject[])) as GameObject[];
		}

		public static void SetContentRefs(this IDataObject data, IEnumerable<IContentRef> content)
		{
			if (!content.Any()) return;
			data.SetWrappedData(content.ToArray());
		}
		public static bool ContainsContentRefs<T>(this IDataObject data) where T : Resource
		{
			if (!data.GetWrappedDataPresent(typeof(IContentRef[]))) return false;
			IContentRef[] refArray = data.GetWrappedData(typeof(IContentRef[])) as IContentRef[];
			return refArray.Any(r => r.Is<T>());
		}
		public static bool ContainsContentRefs(this IDataObject data, Type resType = null)
		{
			if (resType == null) resType = typeof(Resource);
			if (!data.GetWrappedDataPresent(typeof(IContentRef[]))) return false;
			IContentRef[] refArray = data.GetWrappedData(typeof(IContentRef[])) as IContentRef[];
			return refArray.Any(r => r.Is(resType));
		}
		public static ContentRef<T>[] GetContentRefs<T>(this IDataObject data) where T : Resource
		{
			if (!data.GetWrappedDataPresent(typeof(IContentRef[]))) return null;
			IContentRef[] refArray = data.GetWrappedData(typeof(IContentRef[])) as IContentRef[];
			return (
				from r in refArray
				where r.Is<T>()
				select r.As<T>()
				).ToArray();
		}
		public static IContentRef[] GetContentRefs(this IDataObject data, Type resType = null)
		{
			if (resType == null) resType = typeof(Resource);
			if (!data.GetWrappedDataPresent(typeof(IContentRef[]))) return null;
			IContentRef[] refArray = data.GetWrappedData(typeof(IContentRef[])) as IContentRef[];
			return (
				from r in refArray
				where r.Is(resType)
				select r
				).ToArray();
		}
		
		public static void SetBatchInfos(this IDataObject data, IEnumerable<BatchInfo> obj)
		{
			BatchInfo[] objArray = obj.ToArray();
			if (objArray.Length > 0) data.SetWrappedData(objArray);
		}
		public static bool ContainsBatchInfos(this IDataObject data)
		{
			return data.GetWrappedDataPresent(typeof(BatchInfo[]));
		}
		public static BatchInfo[] GetBatchInfos(this IDataObject data)
		{
			return (data.GetWrappedData(typeof(BatchInfo[])) as BatchInfo[]).Select(b => new BatchInfo(b)).ToArray();
		}

		public static void SetIColorData(this IDataObject data, IEnumerable<IColorData> color)
		{
			if (!color.Any()) return;
			data.SetWrappedData(color.ToArray());

			DataObject dataObj = data as DataObject;
			if (dataObj != null)
			{
				var rgbaQuery = color.Select(c => c.ConvertTo<ColorRgba>());
				dataObj.SetText(rgbaQuery.ToString(c => string.Format("{0},{1},{2},{3}", c.R, c.G, c.B, c.A), ", "));
			}
		}
		public static bool ContainsIColorData(this IDataObject data)
		{
			if (data.GetWrappedDataPresent(typeof(IColorData[])))
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
			if (data.GetWrappedDataPresent(typeof(IColorData[])))
			{
				clrArray = data.GetWrappedData(typeof(IColorData[])) as IColorData[];
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
