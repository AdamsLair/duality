using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Drawing;
using System.IO;

using AdamsLair.PropertyGrid;

using Duality;
using Duality.Resources;
using Duality.Serialization;
using Duality.EditorHints;

using DualityEditor.CorePluginInterface;

namespace DualityEditor.CorePluginInterface
{
	public static class CorePluginRegistry
	{
		public const int Priority_None			= 0;
		public const int Priority_General		= 20;
		public const int Priority_Specialized	= 50;
		public const int Priority_Override		= 100;

		#region Resource Entries
		private interface IResEntry {}
		private struct ImageResEntry : IResEntry
		{
			public	Image	img;
			public	string	context;
			public ImageResEntry(Image img, string context)
			{
				this.img = img;
				this.context = context;
			}
		}
		private struct PropertyEditorProviderResEntry : IResEntry
		{
			public	IPropertyEditorProvider	provider;
			public PropertyEditorProviderResEntry(IPropertyEditorProvider provider)
			{
				this.provider = provider;
			}
		}
		private struct EditorActionEntry : IResEntry
		{
			public	IEditorAction	action;
			public	string			context;
			public EditorActionEntry(IEditorAction action, string context)
			{
				this.action = action;
				this.context = context;
			}
		}
		private	struct CategoryEntry : IResEntry
		{
			public	string[]	categoryTree;
			public	string		context;
			public CategoryEntry(string category, string context)
			{
				this.categoryTree = category.Split(new[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries);
				this.context = context;
			}
		}
		private struct DataSelectorEntry : IResEntry
		{
			public	DataConverter	selector;
			public DataSelectorEntry(DataConverter selector)
			{
				this.selector = selector;
			}
		}
		private struct PreviewGeneratorEntry : IResEntry
		{
			public	IPreviewGenerator	generator;
			public PreviewGeneratorEntry(IPreviewGenerator generator)
			{
				this.generator = generator;
			}
		}
		private struct FileImporterEntry : IResEntry
		{
			public	IFileImporter	importer;
			public FileImporterEntry(IFileImporter importer)
			{
				this.importer = importer;
			}
		}
		#endregion


		public const string ImageContext_Icon				= "Icon";
		public const string CategoryContext_General			= "General";
		public const string ActionContext_ContextMenu		= "ContextMenu";
		public const string ActionContext_OpenRes			= "OpenRes";

		private	static	XmlCodeDoc								corePluginDoc	= new XmlCodeDoc();
		private	static	Dictionary<string,List<IResEntry>>		corePluginRes	= new Dictionary<string,List<IResEntry>>();
		private	static	DesignTimeObjectDataManager				designTimeData	= new DesignTimeObjectDataManager();
		

		internal static void Init()
		{
			Scene.Leaving += Scene_Leaving;
		}
		internal static void Terminate()
		{
			Scene.Leaving -= Scene_Leaving;
		}
		private static void Scene_Leaving(object sender, EventArgs e)
		{
			designTimeData.CleanupDesignTimeData();
		}


		private static void RegisterCorePluginRes(Type type, IResEntry res)
		{
			if (type == null) throw new ArgumentNullException("type");
			string typeString = type.GetTypeId();

			List<IResEntry> resList;
			if (!corePluginRes.TryGetValue(typeString, out resList))
			{
				resList = new List<IResEntry>();
				corePluginRes[typeString] = resList;
			}
			if (!resList.Contains(res)) resList.Add(res);
		}
		private static IEnumerable<T> QueryPluginResCandidates<T>(Type type, Predicate<T> predicate) where T : IResEntry
		{
			string typeString = type.GetTypeId();
			List<IResEntry> resList;
			if (corePluginRes.TryGetValue(typeString, out resList))
			{
				foreach (IResEntry res in resList)
				{
					if (res is T)
					{
						T casted = (T)res;
						if (predicate == null || predicate(casted)) yield return casted;
					}
				}
			}
			yield break;
		}
		private static T GetCorePluginRes<T>(Type type, bool contravariantType, Predicate<T> predicate) where T : IResEntry
		{
			if (type == null) return default(T);
			if (contravariantType)
			{
				List<Type> contravariantTypes = new List<Type>();
				contravariantTypes.Add(type);
				foreach (string key in corePluginRes.Keys)
				{
					Type keyType = ReflectionHelper.ResolveType(key);
					if (type.IsAssignableFrom(keyType) && !contravariantTypes.Contains(keyType)) contravariantTypes.Add(keyType);
				}
				foreach (Type contra in contravariantTypes)
				{
					foreach (T entry in QueryPluginResCandidates<T>(contra, predicate))
						return entry;
				}
				return default(T);
			}
			else
			{
				foreach (T entry in QueryPluginResCandidates<T>(type, predicate))
					return entry;

				if (type != typeof(object))
					return GetCorePluginRes<T>(type.BaseType, contravariantType, predicate);
				else
					return default(T);
			}
		}
		private static List<T> GetAllCorePluginRes<T>(Type type, bool contravariantType, Predicate<T> predicate) where T : IResEntry
		{
			if (contravariantType)
			{
				List<Type> contravariantTypes = new List<Type>();
				contravariantTypes.Add(type);
				foreach (string key in corePluginRes.Keys)
				{
					Type keyType = ReflectionHelper.ResolveType(key);
					if (type.IsAssignableFrom(keyType) && !contravariantTypes.Contains(keyType)) contravariantTypes.Add(keyType);
				}
				List<T> result = null;
				foreach (Type contra in contravariantTypes)
				{
					foreach (T entry in QueryPluginResCandidates<T>(contra, predicate))
					{
						if (result == null) result = new List<T>();
						result.Add(entry);
					}
				}
				return result ?? new List<T>();
			}
			else
			{
				List<T> result = null;

				while (type != null)
				{
					foreach (T entry in QueryPluginResCandidates<T>(type, predicate))
					{
						if (result == null) result = new List<T>();
						result.Add(entry);
					}
					type = type.BaseType;
				}

				return result ?? new List<T>();
			}
		}


		public static void RegisterTypeImage(Type type, Image image, string context = ImageContext_Icon)
		{
			RegisterCorePluginRes(type, new ImageResEntry(image, context));
		}
		public static Image GetTypeImage(Type type, string context = ImageContext_Icon)
		{
			return GetCorePluginRes<ImageResEntry>(type, false, e => e.context == context).img;
		}

		public static void RegisterTypeCategory(Type type, string category, string context = CategoryContext_General)
		{
			RegisterCorePluginRes(type, new CategoryEntry(category, context));
		}
		public static string[] GetTypeCategory(Type type, string context = CategoryContext_General)
		{
			string[] tree = GetCorePluginRes<CategoryEntry>(type, false, e => e.context == context).categoryTree;
			if (tree == null)
			{
				foreach (var attrib in type.GetEditorHints<EditorHintCategoryAttribute>())
				{
					if (attrib.Context == context || (string.IsNullOrEmpty(attrib.Context) && context == CategoryContext_General))
					{
						tree = attrib.Category;
						if (tree != null) break;
					}
				}
			}
			if (tree == null) tree = type.Namespace.Split('.');
			return tree;
		}

		public static void RegisterPropertyEditorProvider(IPropertyEditorProvider provider)
		{
			RegisterCorePluginRes(typeof(object), new PropertyEditorProviderResEntry(provider));
		}
		public static IEnumerable<IPropertyEditorProvider> GetPropertyEditorProviders()
		{
			return GetAllCorePluginRes<PropertyEditorProviderResEntry>(typeof(object), false, null).Select(e => e.provider);
		}

		public static void RegisterEditorAction<T>(EditorAction<T> action, string context)
		{
			RegisterCorePluginRes(typeof(T), new EditorActionEntry(action, context));
		}
		public static void RegisterEditorAction<T>(EditorGroupAction<T> action, string context)
		{
			RegisterCorePluginRes(typeof(T), new EditorActionEntry(action, context));
		}
		public static IEnumerable<IEditorAction> GetEditorActions<T>(string context, IEnumerable<object> forGroup = null)
		{
			return GetEditorActions(typeof(T), context, forGroup);
		}
		public static IEnumerable<IEditorAction> GetEditorActions(Type type, string context, IEnumerable<object> forGroup = null)
		{
			return GetAllCorePluginRes<EditorActionEntry>(type, false, e => e.context == context && e.action.CanPerformOn(forGroup)).Select(e => e.action);
		}

		public static void RegisterDataConverter<T>(DataConverter selector)
		{
			RegisterCorePluginRes(typeof(T), new DataSelectorEntry(selector));
		}
		public static IEnumerable<DataConverter> GetDataConverters<T>()
		{
			return GetDataConverters(typeof(T));
		}
		public static IEnumerable<DataConverter> GetDataConverters(Type type)
		{
			return GetAllCorePluginRes<DataSelectorEntry>(type, true, null).Select(e => e.selector);
		}

		public static void RegisterPreviewGenerator(IPreviewGenerator generator)
		{
			RegisterCorePluginRes(typeof(object), new PreviewGeneratorEntry(generator));
		}
		public static IEnumerable<IPreviewGenerator> GetPreviewGenerators()
		{
			return GetAllCorePluginRes<PreviewGeneratorEntry>(typeof(object), false, null).Select(e => e.generator);
		}

		public static void RegisterFileImporter(IFileImporter importer)
		{
			RegisterCorePluginRes(typeof(object), new FileImporterEntry(importer));
		}
		public static IFileImporter GetFileImporter(Predicate<IFileImporter> predicate = null)
		{
			return GetCorePluginRes<FileImporterEntry>(typeof(object), false, e => predicate(e.importer)).importer;
		}
		public static IEnumerable<IFileImporter> GetFileImporters(Predicate<IFileImporter> predicate = null)
		{
			return GetAllCorePluginRes<FileImporterEntry>(typeof(object), false, e => predicate(e.importer)).Select(e => e.importer);
		}

		public static void RegisterXmlCodeDoc(XmlCodeDoc doc)
		{
			corePluginDoc.AppendDoc(doc);
		}
		public static XmlCodeDoc.Entry GetXmlCodeDoc(MemberInfo info)
		{
			return corePluginDoc.GetMemberDoc(info);
		}

		public static DesignTimeObjectData GetDesignTimeData(Guid objId)
		{
			return designTimeData.RequestDesignTimeData(objId);
		}
		public static DesignTimeObjectData GetDesignTimeData(GameObject obj)
		{
			return designTimeData.RequestDesignTimeData(obj.Id);
		}
		internal static void SaveDesignTimeData(string filePath)
		{
			Log.Editor.Write("Saving designtime object data data...");
			Log.Editor.PushIndent();

			try
			{
				using (FileStream str = File.Create(filePath))
				{
					using (var formatter = Formatter.Create(str, FormattingMethod.Binary))
					{
						formatter.SerializationLog = Log.Editor;
						formatter.WriteObject(designTimeData);
					}
				}
			}
			catch (Exception e) { Log.Editor.WriteError(Log.Exception(e)); }

			Log.Editor.PopIndent();
		}
		internal static void LoadDesignTimeData(string filePath)
		{
			Log.Editor.Write("Loading designtime object data data...");
			Log.Editor.PushIndent();

			designTimeData = null;
			if (File.Exists(filePath))
			{
				try
				{
					using (FileStream str = File.OpenRead(filePath))
					{
						using (var formatter = Formatter.Create(str, FormattingMethod.Binary))
						{
							formatter.SerializationLog = Log.Editor;
							designTimeData = formatter.ReadObject<DesignTimeObjectDataManager>();
						}
					}
				}
				catch (Exception e) { Log.Editor.WriteError(Log.Exception(e)); }
			}

			if (designTimeData == null)
			{
				designTimeData = new DesignTimeObjectDataManager();
			}

			Log.Editor.PopIndent();
		}
	}
}
