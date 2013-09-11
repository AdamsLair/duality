using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.IO;
using System.Reflection;

using Duality.Serialization;
using Duality.EditorHints;
using Duality.Cloning;

using ICloneable = Duality.Cloning.ICloneable;

namespace Duality
{
	/// <summary>
	/// The abstract Resource class is inherited by any kind of Duality content. Instances of it or one of its subclasses
	/// are usually handled wrapped inside a <see cref="ContentRef{T}"/> and requested from the <see cref="ContentProvider"/>.
	/// </summary>
	/// <seealso cref="ContentRef{T}"/>
	/// <seealso cref="ContentProvider"/>
	[Serializable]
	public abstract class Resource : IManageableObject, IDisposable, ICloneable
	{
		/// <summary>
		/// A Resource files extension.
		/// </summary>
		public const string FileExt = ".res";

		private	static	List<Resource>	finalizeSched	= new List<Resource>();

		public static event EventHandler<ResourceEventArgs>	ResourceDisposing = null;
		public static event EventHandler<ResourceEventArgs>	ResourceLoaded = null;
		public static event EventHandler<ResourceSaveEventArgs>	ResourceSaved = null;
		public static event EventHandler<ResourceSaveEventArgs>	ResourceSaving = null;
		
		/// <summary>
		/// The path of the file from which the Resource has been originally imported or initialized.
		/// </summary>
		protected	string	sourcePath	= null;
		/// <summary>
		/// The path of this Resource.
		/// </summary>
		[NonSerialized]	protected	string		path		= null;
		[NonSerialized]	private		InitState	initState	= InitState.Initialized;

		/// <summary>
		/// [GET] Returns whether the Resource has been disposed. 
		/// Disposed Resources are not to be used and are treated the same as a null value by most methods.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public bool Disposed
		{
			get { return this.initState == InitState.Disposed; }
		}
		/// <summary>
		/// [GET] The path where this Resource has been originally loaded from or was first saved to.
		/// It is also the path under which this Resource is registered at the ContentProvider.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public string Path
		{
			get { return this.path; }
			internal set { this.path = value; }
		}
		/// <summary>
		/// [GET / SET] The path of the file from which the Resource has been originally imported or initialized.
		/// Setting this does not affect the Resource in any way.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public string SourcePath
		{
			get { return this.sourcePath; }
			set { this.sourcePath = value; }
		}
		/// <summary>
		/// [GET] The name of the Resource.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public string Name
		{
			get
			{
				if (this.IsRuntimeResource) return this.GetHashCode().ToString(CultureInfo.InvariantCulture);
				string nameTemp = this.path;
				if (this.IsDefaultContent) nameTemp = nameTemp.Replace(':', '/');
				return System.IO.Path.GetFileNameWithoutExtension(System.IO.Path.GetFileNameWithoutExtension(nameTemp));
			}
		}
		/// <summary>
		/// [GET] The full name of the Resource, including its path but not its file extension
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public string FullName
		{
			get
			{
				if (this.IsRuntimeResource) return this.GetHashCode().ToString(CultureInfo.InvariantCulture);
				string nameTemp = this.path;
				if (this.IsDefaultContent) nameTemp = nameTemp.Replace(':', '/');
				return System.IO.Path.Combine(System.IO.Path.GetDirectoryName(nameTemp), this.Name);
			}
		}
		/// <summary>
		/// [GET] Returns whether the Resource is part of Duality's embedded default content.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public bool IsDefaultContent
		{
			get { return this.path != null && ContentProvider.IsDefaultContentPath(this.path); }
		}
		/// <summary>
		/// [GET] Returns whether the Resource has been generated at runtime and  cannot be retrieved via content path.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public bool IsRuntimeResource
		{
			get { return string.IsNullOrEmpty(this.path); }
		}
		bool IManageableObject.Active
		{
			get { return !this.Disposed; }
		}

		/// <summary>
		/// Saves the Resource to the specified path. 
		/// </summary>
		/// <param name="saveAsPath">The path to which this Resource is saved to. If null, the Resources <see cref="Path"/> is used as destination.</param>
		/// <param name="makePermanent">
		/// When true, the Resource will be made permanently available from now on. If it has been generated at runtime 
		/// or was loaded explicitly outside the ContentProvider, this will set the Resources <see cref="Path"/> Property
		/// and register it in the ContentProvider. If the Resource already is a permanent, this parameter will be ignored.
		/// </param>
		public void Save(string saveAsPath = null, bool makePermanent = true)
		{
			if (this.Disposed) throw new ApplicationException("Can't save a Ressource that has been disposed.");
			if (string.IsNullOrWhiteSpace(saveAsPath))
			{
				saveAsPath = this.path;
				if (string.IsNullOrWhiteSpace(saveAsPath))
					throw new ArgumentException("Can't save a Resource to an undefined path.", "saveAsPath");
			}

			this.CheckedOnSaving(saveAsPath);

			// We're saving a new Ressource for the first time: Register it in the library
			if (makePermanent && string.IsNullOrWhiteSpace(this.path))
			{
				this.path = saveAsPath;
				ContentProvider.AddContent(this.path, this);
			}

			string streamName;
			string dirName = System.IO.Path.GetDirectoryName(saveAsPath);
			if (!string.IsNullOrEmpty(dirName) && !Directory.Exists(dirName)) Directory.CreateDirectory(dirName);
			using (FileStream str = File.Open(saveAsPath, FileMode.Create))
			{
				this.WriteToStream(str, out streamName);
			}
			this.CheckedOnSaved(saveAsPath);
		}
		/// <summary>
		/// Saves the Resource to the specified stream.
		/// </summary>
		/// <param name="str"></param>
		public void Save(Stream str)
		{
			if (this.Disposed) throw new ApplicationException("Can't save a Ressource that has been disposed.");

			string streamName;

			this.CheckedOnSaving(null);
			this.WriteToStream(str, out streamName);
			this.CheckedOnSaved(null);
		}
		private void WriteToStream(Stream str, out string streamName)
		{
			if (str is FileStream)
			{
				FileStream fileStream = str as FileStream;
				if (PathHelper.IsPathLocatedIn(fileStream.Name, "."))
					streamName = PathHelper.MakeFilePathRelative(fileStream.Name);
				else
					streamName = fileStream.Name;
			}
			else
				streamName = str.ToString();

			using (var formatter = Formatter.Create(str))
			{
				formatter.AddFieldBlocker(Resource.NonSerializedResourceBlocker);
				if (this is Duality.Resources.Scene) // This is an unfortunate hack. Refactor when necessary.
					formatter.AddFieldBlocker(Resource.PrefabLinkedFieldBlocker);
				formatter.WriteObject(this);
			}
		}
		private bool CheckedOnSaving(string saveAsPath)
		{
			if (this.initState != InitState.Initialized) return true;
			try
			{
				if (ResourceSaving != null) ResourceSaving(this, new ResourceSaveEventArgs(this, saveAsPath));
				this.OnSaving(saveAsPath);
				return true;
			}
			catch (Exception e)
			{
				Log.Core.WriteError("OnSaving() of {0} failed: {1}", this, Log.Exception(e));
				return false;
			}
		}
		private bool CheckedOnSaved(string saveAsPath)
		{
			if (this.initState != InitState.Initialized) return true;
			try
			{
				this.OnSaved(saveAsPath);
				if (ResourceSaved != null) ResourceSaved(this, new ResourceSaveEventArgs(this, saveAsPath));
				return true;
			}
			catch (Exception e)
			{
				Log.Core.WriteError("OnSaved() of {0} failed: {1}", this, Log.Exception(e));
				return false;
			}
		}

		/// <summary>
		/// Creates a deep copy of this Resource.
		/// </summary>
		/// <returns></returns>
		public Resource Clone()
		{
			return CloneProvider.DeepClone(this);
		}
		/// <summary>
		/// Deep-copies this Resource to the specified target Resource. The target Resource's Type must
		/// match this Resource's Type.
		/// </summary>
		/// <param name="r">The target Resource to copy this Resource's data to</param>
		public void CopyTo(Resource r)
		{
			CloneProvider.DeepCopyTo(this, r);
		}
		void ICloneable.CopyDataTo(object targetObj, CloneProvider provider)
		{
			Resource target = targetObj as Resource;
			this.OnCopyTo(target, provider);
		}
		
		/// <summary>
		/// This method Performs the <see cref="CopyTo"/> operation for custom Resource Types.
		/// It uses reflection to copy each field that is declared inside a Duality plugin automatically.
		/// However, you may override this method to specify your own behaviour or simply speed things
		/// up a bit by not using Reflection.
		/// </summary>
		/// <param name="target">The target Resource where this Resources data is copied to.</param>
		protected virtual void OnCopyTo(Resource target, CloneProvider provider)
		{
			target.path			= provider.Context.PreserveIdentity ? null : this.path;
			target.sourcePath	= provider.Context.PreserveIdentity ? null : this.sourcePath;

			// If any derived Resource type doesn't override OnCopyTo, use a reflection-driven default behavior.
			CloneProvider.PerformReflectionFallback("OnCopyTo", this, target, provider);
		}
		/// <summary>
		/// Called when this Resource is now beginning to be saved.
		/// </summary>
		protected virtual void OnSaving(string saveAsPath) {}
		/// <summary>
		/// Called when this Resource has just been saved.
		/// </summary>
		protected virtual void OnSaved(string saveAsPath) {}
		/// <summary>
		/// Called when this Resource has just been loaded.
		/// </summary>
		protected virtual void OnLoaded() {}

		~Resource()
		{
			finalizeSched.Add(this);
		}
		/// <summary>
		/// Disposes the Resource.
		/// </summary>
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}
		private void Dispose(bool manually)
		{
			if (this.initState == InitState.Initialized)
			{
				this.initState = InitState.Disposing;
				if (ResourceDisposing != null) ResourceDisposing(this, new ResourceEventArgs(this));
				this.OnDisposing(manually);
				ContentProvider.RemoveContent(this, false);
				this.initState = InitState.Disposed;
			}
		}
		/// <summary>
		/// Called when beginning to dispose the Resource.
		/// </summary>
		/// <param name="manually"></param>
		protected virtual void OnDisposing(bool manually) {}

		/// <summary>
		/// Creates a <see cref="ContentRef{T}"/> referring to this Resource.
		/// </summary>
		/// <returns>A <see cref="ContentRef{T}"/> referring to this Resource.</returns>
		public IContentRef GetContentRef()
		{
			Type refType = typeof(ContentRef<>).MakeGenericType(this.GetType());
			return Activator.CreateInstance(refType, this) as IContentRef;
		}
		
		public override string ToString()
		{
			return string.Format("{0} \"{1}\"", this.GetType().Name, this.FullName);
		}

		/// <summary>
		/// Loads the Resource that is located at the specified path. You shouldn't need this method in almost all cases.
		/// Only use it when you know exactly what you're doing. Consider requesting the Resource from the <see cref="ContentProvider"/> instead.
		/// </summary>
		/// <typeparam name="T">
		/// Desired Type of the returned reference. Does not affect the loaded Resource in any way - it is simply returned as T.
		/// Results in returning null if the loaded Resource's Type isn't assignable to T.
		/// </typeparam>
		/// <param name="path">The path to load the Resource from.</param>
		/// <param name="loadCallback">An optional callback that is invoked right after loading the Resource, but before initializing it.</param>
		/// <param name="initResource">
		/// Specifies whether or not the Resource is initialized by calling <see cref="Resource.OnLoaded"/>. Never attempt to use
		/// uninitialized Resources or register them in the ContentProvider.
		/// </param>
		/// <returns>The Resource that has been loaded.</returns>
		public static T Load<T>(string path, Action<T> loadCallback = null, bool initResource = true) where T : Resource
		{
			if (!File.Exists(path)) return null;

			T newContent;
			using (FileStream str = File.OpenRead(path))
			{
				newContent = Load<T>(str, path, loadCallback, initResource);
			}
			return newContent;
		}
		/// <summary>
		/// Loads the Resource from the specified <see cref="Stream"/>. You shouldn't need this method in almost all cases.
		/// Only use it when you know exactly what you're doing. Consider requesting the Resource from the <see cref="ContentProvider"/> instead.
		/// </summary>
		/// <typeparam name="T">
		/// Desired Type of the returned reference. Does not affect the loaded Resource in any way - it is simply returned as T.
		/// Results in returning null if the loaded Resource's Type isn't assignable to T.
		/// </typeparam>
		/// <param name="str">The stream to load the Resource from.</param>
		/// <param name="resPath">The path that is assumed as the loaded Resource's origin.</param>
		/// <param name="loadCallback">An optional callback that is invoked right after loading the Resource, but before initializing it.</param>
		/// <param name="initResource">
		/// Specifies whether or not the Resource is initialized by calling <see cref="Resource.OnLoaded"/>. Never attempt to use
		/// uninitialized Resources or register them in the ContentProvider.
		/// </param>
		/// <returns>The Resource that has been loaded.</returns>
		public static T Load<T>(Stream str, string resPath = null, Action<T> loadCallback = null, bool initResource = true) where T : Resource
		{
			using (var formatter = Formatter.Create(str))
			{
				return Load<T>(formatter, resPath, loadCallback, initResource);
			}
		}
		/// <summary>
		/// Loads the Resource from the specified <see cref="Stream"/>. You shouldn't need this method in almost all cases.
		/// Only use it when you know exactly what you're doing. Consider requesting the Resource from the <see cref="ContentProvider"/> instead.
		/// </summary>
		/// <typeparam name="T">
		/// Desired Type of the returned reference. Does not affect the loaded Resource in any way - it is simply returned as T.
		/// Results in returning null if the loaded Resource's Type isn't assignable to T.
		/// </typeparam>
		/// <param name="str">The stream to load the Resource from.</param>
		/// <param name="resPath">The path that is assumed as the loaded Resource's origin.</param>
		/// <param name="loadCallback">An optional callback that is invoked right after loading the Resource, but before initializing it.</param>
		/// <param name="initResource">
		/// Specifies whether or not the Resource is initialized by calling <see cref="Resource.OnLoaded"/>. Never attempt to use
		/// uninitialized Resources or register them in the ContentProvider.
		/// </param>
		/// <returns>The Resource that has been loaded.</returns>
		public static T Load<T>(Formatter formatter, string resPath = null, Action<T> loadCallback = null, bool initResource = true) where T : Resource
		{
			T newContent = null;

			try
			{
				Resource res = formatter.ReadObject() as Resource;
				if (res == null) throw new ApplicationException("Loading Resource failed");

				res.initState = InitState.Initializing;
				res.path = resPath;
				if (loadCallback != null) loadCallback(res as T); // Callback before initializing.
				if (initResource) Init(res);
				newContent = res as T;
			}
			catch (Exception e)
			{
				Log.Core.WriteError("Can't load {0} from '{1}', because an error occurred: {3}{2}",
					Log.Type(typeof(T)),
					resPath ?? formatter.ToString(),
					Log.Exception(e),
					Environment.NewLine);
			}

			return newContent;
		}

		/// <summary>
		/// Initializes a Resource that has been <see cref="Load{T}">loaded</see> without initialization. You shouldn't need this method in almost all cases.
		/// Only use it when you know exactly what you're doing. Consider requesting the Resource from the <see cref="ContentProvider"/> instead.
		/// </summary>
		/// <param name="res">The Resource to initialize.</param>
		public static void Init(Resource res)
		{
			if (res.initState != InitState.Initializing) return;
			res.OnLoaded();
			if (ResourceLoaded != null) ResourceLoaded(res, new ResourceEventArgs(res));
			res.initState = InitState.Initialized;
		}

		/// <summary>
		/// Determines whether or not the specified path points to a Duality Resource file.
		/// </summary>
		/// <param name="filePath"></param>
		/// <returns></returns>
		public static bool IsResourceFile(string filePath)
		{
			return System.IO.Path.GetExtension(filePath).ToLower() == FileExt;
		}
		/// <summary>
		/// Returns all Resource files that are located in the specified directory. This doesn't affect
		/// any actual content- or load states.
		/// </summary>
		/// <param name="folderPath"></param>
		/// <returns></returns>
		public static List<string> GetResourceFiles(string folderPath = null)
		{
			if (string.IsNullOrEmpty(folderPath)) folderPath = DualityApp.DataDirectory;
			return Directory.EnumerateFiles(folderPath, "*" + Resource.FileExt, SearchOption.AllDirectories).ToList();
		}
		/// <summary>
		/// Returns the Resource file extension for a specific Resource Type.
		/// </summary>
		/// <param name="resType">The Resource Type to return the file extension from.</param>
		/// <returns>The specified Resource Type's file extension.</returns>
		public static string GetFileExtByType(Type resType)
		{
			return "." + resType.Name + FileExt;
		}
		/// <summary>
		/// Returns the Resource Type that is associated with the specified file, based on its extension.
		/// </summary>
		/// <param name="filePath">Path to the file of whichs Resource Type will be returned</param>
		/// <returns>The Resource Type of the specified file</returns>
		public static Type GetTypeByFileName(string filePath)
		{
			if (string.IsNullOrEmpty(filePath) || ContentProvider.IsDefaultContentPath(filePath)) return null;
			filePath = System.IO.Path.GetFileNameWithoutExtension(filePath);
			string[] token = filePath.Split('.');
			if (token.Length < 2) return null;
			return DualityApp.GetAvailDualityTypes(typeof(Resource)).FirstOrDefault(t => t.Name == token[token.Length - 1]);
		}

		/// <summary>
		/// A <see cref="Duality.Serialization.Formatter.FieldBlockers">FieldBlocker</see> to prevent
		/// fields flagged with a <see cref="NonSerializedResourceAttribute"/> from being serialized.
		/// </summary>
		/// <param name="field"></param>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static bool NonSerializedResourceBlocker(FieldInfo field, object obj)
		{
			return field.GetCustomAttributes(typeof(NonSerializedResourceAttribute), true).Any();
		}
		/// <summary>
		/// A <see cref="Duality.Serialization.Formatter.FieldBlockers">FieldBlocker</see> to prevent
		/// fields of <see cref="Duality.Resources.PrefabLink">PrefabLink-ed</see> objects from being serialized unnecessarily.
		/// </summary>
		/// <param name="field"></param>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static bool PrefabLinkedFieldBlocker(FieldInfo field, object obj)
		{
			Component cmp = obj as Component;
			return 
				cmp != null && 
				cmp.GameObj != null && 
				cmp.GameObj.PrefabLink != null && 
				field.DeclaringType != typeof(Component);
		}

		internal static void RunCleanup()
		{
			while (finalizeSched.Count > 0)
			{
				if (finalizeSched[finalizeSched.Count - 1] != null)
					finalizeSched[finalizeSched.Count - 1].Dispose(false);
				finalizeSched.RemoveAt(finalizeSched.Count - 1);
			}
		}
	}

	/// <summary>
	/// Indicates that a field will be assumed null when serializing it as part of a Resource serialization.
	/// </summary>
	[AttributeUsage(AttributeTargets.Field)]
	public class NonSerializedResourceAttribute : Attribute
	{
	}

	/// <summary>
	/// Allows to explicitly specify what kinds of Resources a certain Resource Type is able to reference (both directly and indirectly).
	/// This is an optional attribute that is used for certain runtime optimizations. Keep in mind that you'll need to specify both direct
	/// and indirect references.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public class ExplicitResourceReferenceAttribute : Attribute
	{
		private	Type[]	referencedTypes	= null;

		public IEnumerable<Type> ReferencedTypes
		{
			get { return this.referencedTypes; }
		}

		public ExplicitResourceReferenceAttribute(params Type[] referencedTypes)
		{
			if (referencedTypes == null) throw new ArgumentNullException("referencedTypes");
			for (int i = 0; i < referencedTypes.Length; ++i)
			{
				if (referencedTypes[i] == null || !typeof(Resource).IsAssignableFrom(referencedTypes[i]))
					throw new ArgumentException("Only Resource Types are valied in this Attribute");
			}
			this.referencedTypes = referencedTypes;
		}
	}
}
