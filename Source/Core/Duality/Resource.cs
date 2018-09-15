using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.IO;
using System.Reflection;

using Duality.Serialization;
using Duality.Cloning;
using Duality.Properties;
using Duality.IO;
using Duality.Editor;
using Duality.Editor.AssetManagement;

namespace Duality
{
	/// <summary>
	/// The abstract Resource class is inherited by any kind of Duality content. Instances of it or one of its subclasses
	/// are usually handled wrapped inside a <see cref="ContentRef{T}"/> and requested from the <see cref="ContentProvider"/>.
	/// </summary>
	/// <seealso cref="ContentRef{T}"/>
	/// <seealso cref="ContentProvider"/>
	[CloneBehavior(CloneBehavior.Reference)]
	[EditorHintImage(CoreResNames.ImageResource)]
	public abstract class Resource : IManageableObject, IDisposable, ICloneExplicit
	{
		/// <summary>
		/// A Resource files extension.
		/// </summary>
		internal static readonly string FileExt = ".res";
		/// <summary>
		/// (Virtual) base path for Duality's embedded default content.
		/// </summary>
		public static readonly string DefaultContentBasePath = "Default:";

		private	static	List<Resource>	finalizeSched	= new List<Resource>();

		public static event EventHandler<ResourceEventArgs>	ResourceDisposing = null;
		public static event EventHandler<ResourceEventArgs>	ResourceLoaded = null;
		public static event EventHandler<ResourceSaveEventArgs>	ResourceSaved = null;
		public static event EventHandler<ResourceSaveEventArgs>	ResourceSaving = null;
		
		/// <summary>
		/// Contains information on how this <see cref="Resource"/> should be treated during
		/// Asset import operations in the editor.
		/// </summary>
		[CloneField(CloneFieldFlags.Skip)]
		protected AssetInfo assetInfo = null;
		/// <summary>
		/// The path of this Resource.
		/// </summary>
		[DontSerialize]
		protected string path = null;
		/// <summary>
		/// The initialization state of the Resource. Also specifies a disposed-state.
		/// </summary>
		[DontSerialize]
		private InitState initState = InitState.Initialized;

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
		/// It is also the path under which this Resource is registered at the <see cref="ContentProvider"/>.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public string Path
		{
			get { return this.path; }
			internal set { this.path = value; }
		}
		/// <summary>
		/// [GET / SET] Provides information on the way this <see cref="Duality.Resource"/> should be treated during
		/// Asset import operations in the editor. This information is not available at runtime and can only be
		/// accessed or set outside a <see cref="DualityApp.ExecutionEnvironment.Launcher"/> environment.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public AssetInfo AssetInfo
		{
			get
			{
				if (DualityApp.ExecEnvironment == DualityApp.ExecutionEnvironment.Launcher)
					return null;
				else
					return this.assetInfo;
			}
			set
			{
				if (DualityApp.ExecEnvironment == DualityApp.ExecutionEnvironment.Launcher)
					throw new NotSupportedException("This property cannot be set at runtime.");
				else
					this.assetInfo = value;
			}
		}
		/// <summary>
		/// [GET] The name of the Resource.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public string Name
		{
			get
			{
				if (this.IsRuntimeResource)
					return this.GetHashCode().ToString(CultureInfo.InvariantCulture);
				else
					return Resource.GetNameFromPath(this.path);
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
				if (this.IsRuntimeResource)
					return this.GetHashCode().ToString(CultureInfo.InvariantCulture);
				else
					return Resource.GetFullNameFromPath(this.path);
			}
		}
		/// <summary>
		/// [GET] Returns whether the Resource is part of Duality's embedded default content.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public bool IsDefaultContent
		{
			get { return Resource.IsDefaultContentPath(this.path); }
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
		/// and register it in the <see cref="ContentProvider"/>. If the Resource already is a permanent, this parameter will be ignored.
		/// </param>
		public void Save(string saveAsPath = null, bool makePermanent = true)
		{
			if (this.Disposed) throw new ObjectDisposedException("Can't save a Resource that has been disposed.");
			if (string.IsNullOrWhiteSpace(saveAsPath))
			{
				saveAsPath = this.path;
				if (string.IsNullOrWhiteSpace(saveAsPath))
					throw new ArgumentException("Can't save a Resource to an undefined path.", "saveAsPath");
			}

			// Prepare saving the Resource and abort if an error occurred in the process
			bool preparedSuccessfully = this.CheckedOnSaving(saveAsPath);
			if (!preparedSuccessfully) return;

			// We're saving a new Resource for the first time: Register it in the library
			bool isPermanent = !string.IsNullOrWhiteSpace(this.path);
			if (makePermanent && !isPermanent)
			{
				this.path = saveAsPath;
				ContentProvider.AddContent(this.path, this);
			}

			string dirName = PathOp.GetDirectoryName(saveAsPath);
			if (!string.IsNullOrEmpty(dirName) && !DirectoryOp.Exists(dirName)) DirectoryOp.Create(dirName);
			using (Stream str = FileOp.Create(saveAsPath))
			{
				this.WriteToStream(str);
			}
			this.CheckedOnSaved(saveAsPath);
		}
		/// <summary>
		/// Saves the Resource to the specified stream.
		/// </summary>
		/// <param name="str"></param>
		public void Save(Stream str)
		{
			if (this.Disposed) throw new ObjectDisposedException("Can't save a Resource that has been disposed.");

			this.CheckedOnSaving(null);
			this.WriteToStream(str);
			this.CheckedOnSaved(null);
		}
		private void WriteToStream(Stream str)
		{
			using (Serializer serializer = Serializer.Create(str, Serializer.DefaultType))
			{
				serializer.AddFieldBlocker(Resource.DontSerializeResourceBlocker);
				if (this is Duality.Resources.Scene) // This is an unfortunate hack. Refactor when necessary.
					serializer.AddFieldBlocker(Resource.PrefabLinkedFieldBlocker);
				serializer.WriteObject(this);
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
				Logs.Core.WriteError("OnSaving() of {0} failed: {1}", this, LogFormat.Exception(e));
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
				Logs.Core.WriteError("OnSaved() of {0} failed: {1}", this, LogFormat.Exception(e));
				return false;
			}
		}

		/// <summary>
		/// Creates a deep copy of this Resource.
		/// </summary>
		/// <returns></returns>
		public Resource Clone()
		{
			return this.DeepClone();
		}
		/// <summary>
		/// Deep-copies this Resource to the specified target Resource. The target Resource's Type must
		/// match this Resource's Type.
		/// </summary>
		/// <param name="target">The target Resource to copy this Resource's data to</param>
		public void CopyTo(Resource target)
		{
			this.DeepCopyTo(target);
		}
		
		void ICloneExplicit.SetupCloneTargets(object target, ICloneTargetSetup setup)
		{
			setup.HandleObject(this, target);
			this.OnSetupCloneTargets(target, setup);
		}
		void ICloneExplicit.CopyDataTo(object target, ICloneOperation operation)
		{
			operation.HandleObject(this, target);
			this.OnCopyDataTo(target, operation);
		}
		/// <summary>
		/// This method prepares the <see cref="CopyTo"/> operation for custom Resource Types.
		/// It uses reflection to prepare the cloning operation automatically, but you can implement
		/// this method in order to handle certain fields and cases manually. See <see cref="ICloneExplicit.SetupCloneTargets"/>
		/// for a more thorough explanation.
		/// </summary>
		/// <param name="setup"></param>
		protected virtual void OnSetupCloneTargets(object target, ICloneTargetSetup setup) {}
		/// <summary>
		/// This method performs the <see cref="CopyTo"/> operation for custom Resource Types.
		/// It uses reflection to perform the cloning operation automatically, but you can implement
		/// this method in order to handle certain fields and cases manually. See <see cref="ICloneExplicit.CopyDataTo"/>
		/// for a more thorough explanation.
		/// </summary>
		/// <param name="target">The target Resource where this Resources data is copied to.</param>
		/// <param name="operation"></param>
		protected virtual void OnCopyDataTo(object target, ICloneOperation operation) {}
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
			lock (finalizeSched)
			{
				finalizeSched.Add(this);
			}
		}
		/// <summary>
		/// Disposes the Resource. Please don't do something silly, like disposing a Scene while it is updated.. use <see cref="ExtMethodsIManageableObject.DisposeLater"/> instead!
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

				// Invoke external and internal handlers for resource disposal
				if (ResourceDisposing != null)
					ResourceDisposing(this, new ResourceEventArgs(this));
				this.OnDisposing(manually);

				this.initState = InitState.Disposed;

				// Remove the resource from the central registry
				ContentProvider.RemoveContent(this, false);
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
		/// uninitialized Resources or register them in the <see cref="ContentProvider"/>.
		/// </param>
		/// <returns>The Resource that has been loaded.</returns>
		public static T Load<T>(string path, Action<T> loadCallback = null, bool initResource = true) where T : Resource
		{
			if (!FileOp.Exists(path)) return null;

			T newContent;
			using (Stream str = FileOp.Open(path, FileAccessMode.Read))
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
		/// uninitialized Resources or register them in the <see cref="ContentProvider"/>.
		/// </param>
		/// <returns>The Resource that has been loaded.</returns>
		public static T Load<T>(Stream str, string resPath = null, Action<T> loadCallback = null, bool initResource = true) where T : Resource
		{
			using (Serializer serializer = Serializer.Create(str))
			{
				return Load<T>(serializer, resPath, loadCallback, initResource);
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
		/// uninitialized Resources or register them in the <see cref="ContentProvider"/>.
		/// </param>
		/// <returns>The Resource that has been loaded.</returns>
		public static T Load<T>(Serializer formatter, string resPath = null, Action<T> loadCallback = null, bool initResource = true) where T : Resource
		{
			T newContent = null;

			try
			{
				Resource res = formatter.ReadObject<Resource>();
				if (res == null) throw new Exception("Deserializing Resource failed.");

				res.initState = InitState.Initializing;
				res.path = resPath;
				if (loadCallback != null) loadCallback(res as T); // Callback before initializing.
				if (initResource) Init(res);
				newContent = res as T;
			}
			catch (Exception e)
			{
				Logs.Core.WriteError("Can't load {0} from '{1}', because an error occurred: {3}{2}",
					LogFormat.Type(typeof(T)),
					resPath ?? formatter.ToString(),
					LogFormat.Exception(e),
					Environment.NewLine);
			}

			return newContent;
		}

		/// <summary>
		/// Initializes a Resource that has been loaded without initialization. You shouldn't need this method in almost all cases.
		/// Only use it when you know exactly what you're doing. Consider requesting the Resource from the <see cref="ContentProvider"/> instead.
		/// </summary>
		/// <param name="res">The Resource to initialize.</param>
		public static void Init(Resource res)
		{
			if (res.initState != InitState.Initializing) return;

			res.OnLoaded();
			res.initState = InitState.Initialized;

			if (ResourceLoaded != null)
				ResourceLoaded(res, new ResourceEventArgs(res));
		}

		/// <summary>
		/// Determines the name of a Resource based on its path.
		/// </summary>
		/// <param name="resPath"></param>
		/// <returns></returns>
		public static string GetNameFromPath(string resPath)
		{
			if (string.IsNullOrEmpty(resPath)) return "null";
			if (IsDefaultContentPath(resPath)) resPath = resPath.Replace(':', PathOp.DirectorySeparatorChar);
			return PathOp.GetFileNameWithoutExtension(PathOp.GetFileNameWithoutExtension(resPath));
		}
		/// <summary>
		/// Determines the full (hierarchical) name of a Resource based on its path.
		/// </summary>
		/// <param name="resPath"></param>
		/// <returns></returns>
		public static string GetFullNameFromPath(string resPath)
		{
			if (string.IsNullOrEmpty(resPath)) return "null";
			if (IsDefaultContentPath(resPath)) resPath = resPath.Replace(':', PathOp.DirectorySeparatorChar);
			return PathOp.Combine(PathOp.GetDirectoryName(resPath), GetNameFromPath(resPath));
		}

		/// <summary>
		/// Returns whether or not the specified path points to Duality default content.
		/// </summary>
		/// <param name="resPath"></param>
		/// <returns></returns>
		public static bool IsDefaultContentPath(string resPath)
		{
			return
				resPath != null &&
				resPath.StartsWith(DefaultContentBasePath, StringComparison.Ordinal);
		}

		/// <summary>
		/// Determines whether or not the specified path points to a Duality Resource file.
		/// </summary>
		/// <param name="filePath"></param>
		/// <returns></returns>
		public static bool IsResourceFile(string filePath)
		{
			return string.Equals(PathOp.GetExtension(filePath), FileExt, StringComparison.OrdinalIgnoreCase);
		}
		/// <summary>
		/// Returns all Resource files that are located in the specified directory. This doesn't affect
		/// any actual content- or load states.
		/// </summary>
		/// <param name="folderPath"></param>
		/// <returns></returns>
		public static IEnumerable<string> GetResourceFiles(string folderPath = null)
		{
			if (string.IsNullOrEmpty(folderPath)) folderPath = DualityApp.DataDirectory;
			IEnumerable<string> resFiles = DirectoryOp.GetFiles(folderPath, true);
			return resFiles.Where(path => path.EndsWith(Resource.FileExt, StringComparison.OrdinalIgnoreCase));
		}
		/// <summary>
		/// Returns the Resource file extension for a specific Resource Type.
		/// </summary>
		/// <param name="resType">The Resource Type to return the file extension from.</param>
		/// <returns>The specified Resource Type's file extension.</returns>
		public static string GetFileExtByType(Type resType)
		{
			if (resType == null || resType == typeof(Resource))
				return FileExt;
			else
				return "." + resType.Name + FileExt;
		}
		/// <summary>
		/// Returns the Resource file extension for a specific Resource Type.
		/// </summary>
		/// <param name="resType">The Resource Type to return the file extension from.</param>
		/// <returns>The specified Resource Type's file extension.</returns>
		public static string GetFileExtByType<T>() where T : Resource
		{
			if (typeof(T) == typeof(Resource))
				return FileExt;
			else
				return "." + typeof(T).Name + FileExt;
		}
		/// <summary>
		/// Returns the Resource Type that is associated with the specified file, based on its extension.
		/// </summary>
		/// <param name="filePath">Path to the file of whichs Resource Type will be returned</param>
		/// <returns>The Resource Type of the specified file</returns>
		public static Type GetTypeByFileName(string filePath)
		{
			// Early-out if we don't have a valid resource path
			if (string.IsNullOrEmpty(filePath) || Resource.IsDefaultContentPath(filePath))
				return null;

			// Determine the (double) extension of the resource path
			filePath = PathOp.GetFileNameWithoutExtension(filePath);
			string[] token = filePath.Split('.');
			if (token.Length < 2)
				return null;

			// Extract the type extension and match it with the available resource types
			string typeName = token[token.Length - 1];
			TypeInfo matchingInfo = 
				DualityApp.GetAvailDualityTypes(typeof(Resource))
				.FirstOrDefault(t => t.Name == typeName);
			if (matchingInfo == null)
				return null;

			// Return the result
			return matchingInfo.AsType();
		}

		/// <summary>
		/// A <see cref="Duality.Serialization.Serializer.FieldBlockers">FieldBlocker</see> to prevent
		/// fields flagged with a <see cref="DontSerializeResourceAttribute"/> from being serialized.
		/// </summary>
		/// <param name="field"></param>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static bool DontSerializeResourceBlocker(FieldInfo field, object obj)
		{
			return field.HasAttributeCached<DontSerializeResourceAttribute>();
		}
		/// <summary>
		/// A <see cref="Duality.Serialization.Serializer.FieldBlockers">FieldBlocker</see> to prevent
		/// fields of <see cref="Duality.Resources.PrefabLink">PrefabLink-ed</see> objects from being serialized unnecessarily.
		/// </summary>
		/// <param name="field"></param>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static bool PrefabLinkedFieldBlocker(FieldInfo field, object obj)
		{
			Component cmp = obj as Component;
			if (cmp == null || cmp.GameObj == null) return false;

			Resources.PrefabLink link = cmp.GameObj.AffectedByPrefabLink;
			if (link == null || !link.AffectsObject(cmp)) return false;

			return field.DeclaringType != typeof(Component);
		}

		internal static void RunCleanup()
		{
			Resource[] finalizeSchedArray;
			lock (finalizeSched)
			{
				if (finalizeSched.Count == 0) return;
				finalizeSchedArray = finalizeSched.ToArray();
				finalizeSched.Clear();
			}

			foreach (Resource res in finalizeSchedArray)
			{
				if (res == null) continue;
				res.Dispose(false);
			}
		}
	}

	/// <summary>
	/// Indicates that a field will be assumed null when serializing it as part of a Resource serialization.
	/// </summary>
	[AttributeUsage(AttributeTargets.Field)]
	public class DontSerializeResourceAttribute : Attribute { }

	/// <summary>
	/// Allows to explicitly specify what kinds of Resources a certain Resource Type is able to reference.
	/// This is an optional attribute that is used for certain runtime optimizations. 
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
			TypeInfo resourceTypeInfo = typeof(Resource).GetTypeInfo();
			for (int i = 0; i < referencedTypes.Length; ++i)
			{
				if (referencedTypes[i] == null || !resourceTypeInfo.IsAssignableFrom(referencedTypes[i].GetTypeInfo()))
					throw new ArgumentException("Only Resource Types are valied in this Attribute");
			}
			this.referencedTypes = referencedTypes;
		}
	}
}
