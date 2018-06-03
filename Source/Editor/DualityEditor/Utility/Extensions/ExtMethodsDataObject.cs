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
			object[] valuesArr = values.ToArray();
			string wrappedFormat = GetWrappedDataFormat(dataFormat, storage);
			SerializableWrapper wrapper = storage == DataObjectStorage.Reference
				? new SerializableReferenceWrapper(valuesArr)
				: new SerializableWrapper(valuesArr);

			data.SetData(wrappedFormat, wrapper);

			// Reference data is automatically stored as a value as well
			if (storage == DataObjectStorage.Reference)
				data.SetWrappedData(valuesArr, dataFormat, DataObjectStorage.Value);
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


		#region Obsolete API

		/// <summary>
		/// Stores the specified non-<see cref="SerializableAttribute"/> data inside the specified data object using a serializable wrapper.
		/// </summary>
		/// <param name="data"></param>
		/// <param name="value"></param>
		/// <param name="byReference">Whether or not to store the value as a reference or to perform a clone of the value</param>
		[Obsolete("Obselete and will be removed in v3.0. Please use SetWrappedData(IEnumerable<object>, string, DataObjectStorage) instead.")]
		public static void SetWrappedData(this IDataObject data, object value, bool byReference = false)
		{
			data.SetWrappedData(new []{value}, value.GetType().FullName, byReference ? DataObjectStorage.Reference : DataObjectStorage.Value);
		}
		/// <summary>
		/// Determines whether the specified type of wrapped non-<see cref="SerializableAttribute"/> data is available in the data object.
		/// </summary>
		/// <param name="data"></param>
		/// <param name="format"></param>
		/// <returns></returns>
		[Obsolete("Obselete and will be removed in v3.0. Please use GetWrappedDataPresent(string, DataObjectStorage) instead.")]
		public static bool GetWrappedDataPresent(this IDataObject data, Type format)
		{
			return data.GetWrappedDataPresent(format.FullName, DataObjectStorage.Reference);
		}
		/// <summary>
		/// Determines whether the specified type of wrapped non-<see cref="SerializableAttribute"/> data is available in the data object.
		/// </summary>
		/// <param name="data"></param>
		/// <param name="format"></param>
		/// <returns></returns>
		[Obsolete("Obselete and will be removed in v3.0. Please use GetWrappedDataPresent(string, DataObjectStorage) instead.")]
		public static bool GetWrappedDataPresent(this IDataObject data, string format)
		{
			return data.GetWrappedDataPresent(format, DataObjectStorage.Reference);
		}
		/// <summary>
		/// Retrieves the specified non-<see cref="SerializableAttribute"/> data from the specified data object using a serializable wrapper.
		/// </summary>
		/// <param name="data"></param>
		/// <param name="format"></param>
		/// <returns></returns>
		[Obsolete("Obselete and will be removed in v3.0. Please use GetWrappedData(string, DataObjectStorage) instead.")]
		public static object GetWrappedData(this IDataObject data, Type format)
		{
			return data.GetWrappedData(format.FullName, DataObjectStorage.Reference).First();
		}
		/// <summary>
		/// Retrieves the specified non-<see cref="SerializableAttribute"/> data from the specified data object using a serializable wrapper.
		/// </summary>
		/// <param name="data"></param>
		/// <param name="format"></param>
		/// <returns></returns>
		[Obsolete("Obselete and will be removed in v3.0. Please use GetWrappedData(string, DataObjectStorage) instead.")]
		public static object GetWrappedData(this IDataObject data, string format)
		{
			return data.GetWrappedData(format, DataObjectStorage.Reference).First();
		}
		[Obsolete("Obselete and will be removed in v3.0. Please use SetComponents(IEnumerable<Component>, DataObjectStorage) instead.")]
		public static void SetComponentRefs(this IDataObject data, IEnumerable<Component> cmp)
		{
			Component[] cmpArray = cmp.ToArray();
			if (cmpArray.Length > 0) data.SetWrappedData(cmpArray, true);
		}
		[Obsolete("Obselete and will be removed in v3.0. Please use ContainsComponents(Type, DataObjectStorage) instead.")]
		public static bool ContainsComponentRefs<T>(this IDataObject data) where T : Component
		{
			if (!data.GetWrappedDataPresent(typeof(Component[]))) return false;
			Component[] refArray = data.GetWrappedData(typeof(Component[])) as Component[];
			return refArray != null && refArray.Any(c => c is T);
		}
		[Obsolete("Obselete and will be removed in v3.0. Please use ContainsComponents(Type, DataObjectStorage) instead.")]
		public static bool ContainsComponentRefs(this IDataObject data, Type cmpType = null)
		{
			if (cmpType == null) cmpType = typeof(Component);
			if (!data.GetWrappedDataPresent(typeof(Component[]))) return false;
			Component[] refArray = data.GetWrappedData(typeof(Component[])) as Component[];
			return refArray != null && refArray.Any(c => cmpType.IsInstanceOfType(c));
		}
		[Obsolete("Obselete and will be removed in v3.0. Please use GetComponents(Type, DataObjectStorage) instead.")]
		public static T[] GetComponentRefs<T>(this IDataObject data) where T : Component
		{
			if (!data.GetWrappedDataPresent(typeof(Component[]))) return null;
			Component[] refArray = data.GetWrappedData(typeof(Component[])) as Component[] ?? new Component[0];
			return (
				from r in refArray
				where r is T
				select r as T
			).ToArray();
		}
		[Obsolete("Obselete and will be removed in v3.0. Please use GetComponents(Type, DataObjectStorage) instead.")]
		public static Component[] GetComponentRefs(this IDataObject data, Type cmpType = null)
		{
			if (cmpType == null) cmpType = typeof(Component);
			if (!data.GetWrappedDataPresent(typeof(Component[]))) return null;
			Component[] refArray = data.GetWrappedData(typeof(Component[])) as Component[] ?? new Component[0];
			return (
				from c in refArray
				where cmpType.IsInstanceOfType(c)
				select c
			).ToArray();
		}
		[Obsolete("Obselete and will be removed in v3.0. Please use SetGameObjects(IEnumerable<GameObject>, DataObjectStorage) instead.")]
		public static void SetGameObjectRefs(this IDataObject data, IEnumerable<GameObject> obj)
		{
			GameObject[] objArray = obj.ToArray();
			if (objArray.Length > 0) data.SetWrappedData(objArray, true);
		}
		[Obsolete("Obselete and will be removed in v3.0. Please use ContainsGameObjects(DataObjectStorage) instead.")]
		public static bool ContainsGameObjectRefs(this IDataObject data)
		{
			return data.GetWrappedDataPresent(typeof(GameObject[]));
		}
		[Obsolete("Obselete and will be removed in v3.0. Please use GetGameObjects(DataObjectStorage) instead.")]
		public static GameObject[] GetGameObjectRefs(this IDataObject data)
		{
			return data.GetWrappedData(typeof(GameObject[])) as GameObject[] ?? new GameObject[0];
		}
		[Obsolete("Obselete and will be removed in v3.0. Please use CotnainsContentRefs(Type) instead.")]
		public static bool ContainsContentRefs<T>(this IDataObject data) where T : Resource
		{
			if (!data.GetWrappedDataPresent(typeof(IContentRef[]))) return false;
			IContentRef[] refArray = data.GetWrappedData(typeof(IContentRef[])) as IContentRef[];
			return refArray != null && refArray.Any(r => r.Is<T>());
		}
		[Obsolete("Obselete and will be removed in v3.0. Please use GetContentRefs(Type) instead.")]
		public static ContentRef<T>[] GetContentRefs<T>(this IDataObject data) where T : Resource
		{
			if (!data.GetWrappedDataPresent(typeof(IContentRef[]))) return null;
			IContentRef[] refArray = data.GetWrappedData(typeof(IContentRef[])) as IContentRef[] ?? new IContentRef[0];
			return (
				from r in refArray
				where r.Is<T>()
				select r.As<T>()
			).ToArray();
		}

		#endregion
	}
}
