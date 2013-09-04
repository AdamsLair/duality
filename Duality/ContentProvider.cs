using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.IO;
using System.Diagnostics;

using Duality.Resources;

namespace Duality
{
	/// <summary>
	/// <para>
	/// The ContentProvider is Duality's main instance for content management. If you need any kind of <see cref="Resource"/>,
	/// simply request it from the ContentProvider. It keeps track of which Resources are loaded and valid and prevents
	/// Resources from being loaded more than once at a time, thus reducing loading times and redundancy.
	/// </para>
	/// <para>
	/// You can also manually <see cref="AddContent">register Resources</see> that have been created at runtime 
	/// using a string alias of your choice.
	/// </para>
	/// </summary>
	/// <seealso cref="Resource"/>
	/// <seealso cref="ContentRef{T}"/>
	/// <seealso cref="IContentRef"/>
	public static class ContentProvider
	{
		/// <summary>
		/// (Virtual) base path for Duality's embedded default content.
		/// </summary>
		public const string	VirtualContentPath = "Default:";

		private	static	bool						defaultContentInitialized	= false;
		private	static	Dictionary<string,Resource>	resLibrary					= new Dictionary<string,Resource>();
		private	static	List<Resource>				defaultContent				= new List<Resource>();

		/// <summary>
		/// Initializes Dualitys embedded default content.
		/// </summary>
		public static void InitDefaultContent()
		{
			if (defaultContentInitialized) return;
			Log.Core.Write("Initializing default content..");
			Log.Core.PushIndent();

			var oldResLib = resLibrary.Values.ToArray();

			VertexShader.InitDefaultContent();
			FragmentShader.InitDefaultContent();
			ShaderProgram.InitDefaultContent();
			DrawTechnique.InitDefaultContent();
			Pixmap.InitDefaultContent();
			Texture.InitDefaultContent();
			Material.InitDefaultContent();
			Font.InitDefaultContent();
			AudioData.InitDefaultContent();
			Sound.InitDefaultContent();

			// Make a list of all default content available
			foreach (var pair in resLibrary)
			{
				if (oldResLib.Contains(pair.Value)) continue;
				defaultContent.Add(pair.Value);
			}

			defaultContentInitialized = true;
			Log.Core.PopIndent();
			Log.Core.Write("..done");
		}
		/// <summary>
		/// Returns a list of embedded default content that matches the specified type.
		/// </summary>
		/// <returns></returns>
		public static List<ContentRef<T>> GetDefaultContent<T>() where T : Resource
		{
			return defaultContent.OfType<T>().Select(r => new ContentRef<T>(r)).ToList();
		}
		/// <summary>
		/// Returns a list of embedded default content that matches the specified type.
		/// </summary>
		/// <returns></returns>
		public static List<IContentRef> GetDefaultContent(Type t)
		{
			return defaultContent.Where(r => t.IsInstanceOfType(r)).Select(r => new ContentRef<Resource>(r) as IContentRef).ToList();
		}
		/// <summary>
		/// Returns a list of all currently loaded content matching the specified Type
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static List<ContentRef<T>> GetLoadedContent<T>() where T : Resource
		{
			return resLibrary.Values
				.OfType<T>()
				.Where(r => !r.Disposed)
				.Select(r => new ContentRef<T>(r))
				.ToList();
		}
		/// <summary>
		/// Returns a list of all currently loaded content matching the specified Type
		/// </summary>
		/// <param name="t"></param>
		/// <returns></returns>
		public static List<IContentRef> GetLoadedContent(Type t)
		{
			return resLibrary.Values
				.Where(r => t.IsInstanceOfType(r) && !r.Disposed)
				.Select(r => r.GetContentRef())
				.ToList();
		}
		/// <summary>
		/// Returns a list of all available / existing content matching the specified Type
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="baseDirectory">The base directory to search in. Defaults to <see cref="DualityApp.DataDirectory"/> if not specified otherwise.</param>
		/// <returns></returns>
		public static List<ContentRef<T>> GetAvailableContent<T>(string baseDirectory = null) where T : Resource
		{
			if (baseDirectory == null) baseDirectory = DualityApp.DataDirectory;
			IEnumerable<string> resFiles = Directory.EnumerateFiles(baseDirectory, "*" + Resource.FileExt, SearchOption.AllDirectories);
			return resFiles
				.Select(p => new ContentRef<Resource>(null, p))
				.Where(r => r.Is<T>())
				.Select(r => r.As<T>())
				.Concat(defaultContent.OfType<T>().Select(r => new ContentRef<T>(r)))
				.ToList();
		}
		/// <summary>
		/// Returns a list of all available / existing content matching the specified Type
		/// </summary>
		/// <param name="t"></param>
		/// <param name="baseDirectory">The base directory to search in. Defaults to <see cref="DualityApp.DataDirectory"/> if not specified otherwise.</param>
		/// <returns></returns>
		public static List<IContentRef> GetAvailableContent(Type t, string baseDirectory = null)
		{
			if (baseDirectory == null) baseDirectory = DualityApp.DataDirectory;
			IEnumerable<string> resFiles = Directory.EnumerateFiles(DualityApp.DataDirectory, "*" + Resource.FileExt, SearchOption.AllDirectories);
			return resFiles
				.Select(p => new ContentRef<Resource>(null, p) as IContentRef)
				.Where(r => r.Is(t))
				.Concat(defaultContent.Where(r => t.IsInstanceOfType(r)).Select(r => new ContentRef<Resource>(r) as IContentRef))
				.ToList();
		}
		/// <summary>
		/// Clears all non-default content.
		/// </summary>
		/// <param name="dispose">If true, unregistered content is also disposed.</param>
		public static void ClearContent(bool dispose = true)
		{
			var nonDefaultContent = resLibrary.Where(p => !p.Key.Contains(':')).ToArray();
			foreach (var pair in nonDefaultContent)
				RemoveContent(pair.Key, dispose);
		}

		/// <summary>
		/// Registers a <see cref="Resource"/> and maps it to the specified path key.
		/// </summary>
		/// <param name="path">The path key to map the Resource to</param>
		/// <param name="content">The Resource to register.</param>
		public static void AddContent(string path, Resource content)
		{
			if (String.IsNullOrEmpty(path)) return;
			if (String.IsNullOrEmpty(content.Path)) content.Path = path;
			resLibrary[path] = content;
		}
		/// <summary>
		/// Returns whether or not there is any content currently registered under the specified path key.
		/// </summary>
		/// <param name="path">The path key to look for content</param>
		/// <returns>True, if there is content available for that path key, false if not.</returns>
		public static bool IsContentAvailable(string path)
		{
			if (String.IsNullOrEmpty(path)) return false;
			Resource res;
			return resLibrary.TryGetValue(path, out res) && !res.Disposed;
		}
		
		/// <summary>
		/// Explicitly removes a specific Resource from the ContentProvider.
		/// </summary>
		/// <param name="res"></param>
		/// <param name="dispose"></param>
		/// <returns></returns>
		public static bool RemoveContent(Resource res, bool dispose = true)
		{
			if (res == null) return false;
			if (res.IsRuntimeResource) return false;
			if (res.IsDefaultContent) return false;

			// Remove content from library
			Resource entry;
			bool success = resLibrary.TryGetValue(res.Path, out entry) && entry == res && resLibrary.Remove(res.Path);

			// Dispose cached content
			if (dispose) res.Dispose();
			return success;
		}
		/// <summary>
		/// Unregisters content that has been registered using the specified path key.
		/// </summary>
		/// <param name="path">The path key to unregister.</param>
		/// <param name="dispose">If true, unregistered content is also disposed.</param>
		/// <returns>True, if the content has been found and successfully removed. False, if no</returns>
		public static bool RemoveContent(string path, bool dispose = true)
		{
			if (String.IsNullOrEmpty(path)) return false;
			if (path.Contains(':')) return false;

			// Dispose cached content
			Resource res;
			if (dispose && resLibrary.TryGetValue(path, out res)) res.Dispose();

			return resLibrary.Remove(path);
		}
		/// <summary>
		/// Unregisters all content that has been registered using paths contained within
		/// the specified directory.
		/// </summary>
		/// <param name="dir">The directory to unregister</param>
		/// <param name="dispose">If true, unregistered content is also disposed.</param>
		public static void RemoveContentTree(string dir, bool dispose = true)
		{
			if (String.IsNullOrEmpty(dir)) return;

			List<string> unregisterList = new List<string>(
				from p in resLibrary.Keys
				where !p.Contains(':') && PathHelper.IsPathLocatedIn(p, dir)
				select p);

			foreach (string p in unregisterList)
				RemoveContent(p, dispose);
		}
		/// <summary>
		/// Unregisters all content of the specified Type or subclassed Types.
		/// </summary>
		/// <typeparam name="T">The content Type to look for.</typeparam>
		/// <param name="dispose">If true, unregistered content is also disposed.</param>
		public static void RemoveAllContent<T>(bool dispose = true) where T : Resource
		{
			var affectedContent = GetLoadedContent<T>().Where(c => !c.IsDefaultContent);
			foreach (ContentRef<T> content in affectedContent)
				RemoveContent(content.Path, dispose);
		}
		/// <summary>
		/// Unregisters all content of the specified Type or subclassed Types.
		/// </summary>
		/// <param name="t">The content Type to look for.</param>
		/// <param name="dispose">If true, unregistered content is also disposed.</param>
		public static void RemoveAllContent(Type t, bool dispose = true)
		{
			var affectedContent = GetLoadedContent(t).Where(c => !c.IsDefaultContent).ToArray();
			foreach (IContentRef content in affectedContent)
				RemoveContent(content.Path, dispose);
		}
		internal static IEnumerable<Resource> EnumeratePluginContent()
		{
			return resLibrary.Values.Where(res => res is Prefab || res is Scene || res.GetType().Assembly != typeof(ContentProvider).Assembly);
		}

		/// <summary>
		/// Changes the path key under which a specific Resource can be found. If the target path is registered already, it will be replaced.
		/// If the source path isn't registered / is unknown, the operation will fail.
		/// </summary>
		/// <param name="path">The Resources current path key.</param>
		/// <param name="newPath">The Resources new path key.</param>
		/// <returns>True, if the renaming operation was successful. False, if not.</returns>
		public static bool RenameContent(string path, string newPath)
		{
			if (String.IsNullOrEmpty(path)) return false;

			Resource res;
			if (resLibrary.TryGetValue(path, out res))
			{
				res.Path = newPath;
				resLibrary[newPath] = res;
				resLibrary.Remove(path);
				return true;
			}
			else
				return false;
		}
		/// <summary>
		/// Changes the path key under which a set of Resource can be found, i.e.
		/// renames all path keys located inside the specified directory.
		/// </summary>
		/// <param name="dir">The Resources current directory</param>
		/// <param name="newDir">The Resources new directory</param>
		public static void RenameContentTree(string dir, string newDir)
		{
			if (String.IsNullOrEmpty(dir)) return;

			// Assure we're ending with directory separator chars.
			if (dir[dir.Length - 1] == Path.AltDirectorySeparatorChar) dir = dir.Remove(dir.Length - 1, 1);
			if (dir[dir.Length - 1] != Path.DirectorySeparatorChar) dir += Path.DirectorySeparatorChar;
			if (newDir[newDir.Length - 1] == Path.AltDirectorySeparatorChar) newDir = newDir.Remove(newDir.Length - 1, 1);
			if (newDir[newDir.Length - 1] != Path.DirectorySeparatorChar) newDir += Path.DirectorySeparatorChar;

			List<string> renameList = new List<string>(
				from p in resLibrary.Keys
				where !p.Contains(':') && PathHelper.IsPathLocatedIn(p, dir)
				select p);

			foreach (string p in renameList)
			{
				RenameContent(p, p.Replace(dir, newDir));
			}
		}

		/// <summary>
		/// Requests a <see cref="Resource"/>.
		/// </summary>
		/// <typeparam name="T">
		/// The requested Resource type. Does not affect actual data, only the kind of <see cref="ContentRef{T}"/> that is obtained.
		/// </typeparam>
		/// <param name="path">
		/// The path key to identify the Resource. If there is no matching Resource available yet, the ContentProvider attemps
		/// to load a Resource from that path.
		/// </param>
		/// <returns>A <see cref="ContentRef{T}"/> to the requested Resource.</returns>
		public static ContentRef<T> RequestContent<T>(string path) where T : Resource
		{
			if (String.IsNullOrEmpty(path)) return null;

			// Return cached content
			Resource res;
			if (resLibrary.TryGetValue(path, out res) && !res.Disposed)
			{
				return new ContentRef<T>(res as T, path);
			}

			// Load new content
			return new ContentRef<T>(LoadContent(path) as T, path);
		}
		/// <summary>
		/// Requests a <see cref="Resource"/>.
		/// </summary>
		/// <param name="path">
		/// The path key to identify the Resource. If there is no matching Resource available yet, the ContentProvider attemps
		/// to load a Resource from that path.
		/// </param>
		/// <returns>A <see cref="IContentRef"/> to the requested Resource.</returns>
		public static IContentRef RequestContent(string path)
		{
			return RequestContent<Resource>(path);
		}

		private static Resource LoadContent(string path)
		{
			if (string.IsNullOrEmpty(path) || !File.Exists(path)) return null;

			Log.Core.Write("Loading Ressource '{0}'...", path);
			Log.Core.PushIndent();

			// Load the Resource and register it
			Resource res = Resource.Load<Resource>(path, r => 
			{ 
				// Registering takes place in the callback - before initializing the Resource, because
				// that may result in requesting more Resources and an infinite loop if two Resources request
				// each other in their initialization code.
				if (r != null) AddContent(path, r);
			});

			Log.Core.PopIndent();
			return res;
		}
	}
}
