using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Diagnostics;

using Duality.IO;
using Duality.Resources;

namespace Duality
{
	/// <summary>
	/// <para>
	/// The <see cref="ContentProvider"/> is Duality's main place for content management. If you need any kind of <see cref="Resource"/>,
	/// simply request it from the <see cref="ContentProvider"/> directly, or indirectly via <see cref="ContentRef{T}"/>. It keeps 
	/// track of which Resources are loaded and valid and prevents Resources from being loaded more than once at a time, thus 
	/// reducing loading times and redundancy.
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
		private static List<Resource>              defaultContent = new List<Resource>();
		private static Dictionary<string,Resource> resLibrary     = new Dictionary<string,Resource>();

		public static event EventHandler<ResourceResolveEventArgs> ResourceResolve = null;


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
			TypeInfo typeInfo = t.GetTypeInfo();
			return defaultContent.Where(r => typeInfo.IsInstanceOfType(r)).Select(r => new ContentRef<Resource>(r) as IContentRef).ToList();
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
			TypeInfo typeInfo = t.GetTypeInfo();
			return resLibrary.Values
				.Where(r => typeInfo.IsInstanceOfType(r) && !r.Disposed)
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
			IEnumerable<string> resFiles = Resource.GetResourceFiles(baseDirectory);
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
			TypeInfo typeInfo = t.GetTypeInfo();
			IEnumerable<string> resFiles = Resource.GetResourceFiles(baseDirectory);
			return resFiles
				.Select(p => new ContentRef<Resource>(null, p) as IContentRef)
				.Where(r => r.Is(t))
				.Concat(defaultContent.Where(r => typeInfo.IsInstanceOfType(r)).Select(r => new ContentRef<Resource>(r) as IContentRef))
				.ToList();
		}

		/// <summary>
		/// Clears all content registered within this <see cref="ContentProvider"/>, and
		/// disposes it, if requested.
		/// </summary>
		/// <param name="dispose">If true, the removed content is disposed as well.</param>
		public static void ClearContent(bool dispose = true)
		{
			if (dispose)
			{
				List<Resource> allResources = resLibrary.Values.ToList();
				foreach (Resource resource in allResources)
				{
					resource.Dispose();
				}
			}
			resLibrary.Clear();
			defaultContent.Clear();
		}

		/// <summary>
		/// Registers a <see cref="Resource"/> and maps it to the specified path key.
		/// 
		/// If a different <see cref="Resource"/> was already registered with that path key,
		/// it will be disposed and replaced with the specified one. <see cref="ContentRef{T}"/>
		/// instances referring to the path will re-map to the newly registered <see cref="Resource"/>
		/// on their next access.
		/// </summary>
		/// <param name="path">The path key to map the Resource to</param>
		/// <param name="content">The Resource to register.</param>
		public static void AddContent(string path, Resource content)
		{
			if (string.IsNullOrEmpty(path)) return;

			// If there's already a Resource registered for that path, dispose it
			// in order to force a remap of all ContentRefs to the new Resource
			Resource oldContent;
			if (resLibrary.TryGetValue(path, out oldContent))
				RemoveContent(path, true);

			// Assign the registered path to the Resource itself, if no other
			// path was defined before. This enables the Resource to auto-generate
			// ContentRefs to itself after registering.
			if (string.IsNullOrEmpty(content.Path))
				content.Path = path;

			// Register the resource by its specified path
			resLibrary[path] = content;

			// If it's a default content path, store the Resource as a default resource
			if (content.IsDefaultContent)
			{
				if (!defaultContent.Contains(content))
					defaultContent.Add(content);
			}
		}
		/// <summary>
		/// Returns whether or not there is any content currently registered under the specified path key.
		/// </summary>
		/// <param name="path">The path key to look for content</param>
		/// <returns>True, if there is content available for that path key, false if not.</returns>
		public static bool HasContent(string path)
		{
			if (string.IsNullOrEmpty(path)) return false;
			Resource res;
			return resLibrary.TryGetValue(path, out res) && !res.Disposed;
		}
		
		/// <summary>
		/// Explicitly removes a specific Resource from the ContentProvider.
		/// </summary>
		/// <param name="content"></param>
		/// <param name="dispose"></param>
		/// <returns></returns>
		public static bool RemoveContent(Resource content, bool dispose = true)
		{
			if (content == null) return false;
			return RemoveContent(content.Path);
		}
		/// <summary>
		/// Unregisters content that has been registered using the specified path key.
		/// </summary>
		/// <param name="path">The path key to unregister.</param>
		/// <param name="dispose">If true, unregistered content is also disposed.</param>
		/// <returns>True, if the content has been found and successfully removed. False, if no</returns>
		public static bool RemoveContent(string path, bool dispose = true)
		{
			if (string.IsNullOrEmpty(path)) return false;

			Resource content;
			if (!resLibrary.TryGetValue(path, out content)) return false;

			// Remove content from library
			resLibrary.Remove(path);
			if (content.IsDefaultContent)
			{
				defaultContent.Remove(content);
			}

			// Dispose removed content when requested
			if (dispose)
				content.Dispose();

			// If we're continuing to use the resource, flag it as runtime-only again by
			// resetting its registered path. Note that we're not doing that for disposed
			// resources in order to allow a reload-recovery for ContentRefs that have an
			// outdated path, or were created from the disposed Resource.
			if (!content.Disposed && content.Path == path)
				content.Path = null;

			return true;
		}
		/// <summary>
		/// Unregisters all content that has been registered using paths contained within
		/// the specified directory.
		/// </summary>
		/// <param name="dir">The directory to unregister</param>
		/// <param name="dispose">If true, unregistered content is also disposed.</param>
		public static void RemoveContentTree(string dir, bool dispose = true)
		{
			if (string.IsNullOrEmpty(dir)) return;

			List<string> unregisterList = new List<string>(
				from p in resLibrary.Keys
				where !Resource.IsDefaultContentPath(p) && PathOp.IsPathLocatedIn(p, dir)
				select p);

			foreach (string p in unregisterList)
				RemoveContent(p, dispose);
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
			if (string.IsNullOrEmpty(path)) return false;

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
			if (string.IsNullOrEmpty(dir)) return;

			// Normalize directory names
			dir = dir.Replace(PathOp.AltDirectorySeparatorChar, PathOp.DirectorySeparatorChar);
			newDir = newDir.Replace(PathOp.AltDirectorySeparatorChar, PathOp.DirectorySeparatorChar);

			// Ensure we're ending with directory separator chars
			if (dir[dir.Length - 1] != PathOp.DirectorySeparatorChar) dir += PathOp.DirectorySeparatorChar;
			if (newDir[newDir.Length - 1] != PathOp.DirectorySeparatorChar) newDir += PathOp.DirectorySeparatorChar;

			List<string> renameList = new List<string>(
				from p in resLibrary.Keys
				where !Resource.IsDefaultContentPath(p) && PathOp.IsPathLocatedIn(p, dir)
				select p);

			foreach (string path in renameList)
			{
				string newPath = newDir + path.Remove(0, dir.Length);
				RenameContent(path, newPath);
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
			if (string.IsNullOrEmpty(path)) return null;

			// Return cached content
			Resource res;
			if (resLibrary.TryGetValue(path, out res) && !res.Disposed)
			{
				return new ContentRef<T>(res as T, path);
			}

			// Load new content
			res = ResolveContent(path) ?? LoadContent(path);
			return new ContentRef<T>(res as T, path);
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

		private static Resource ResolveContent(string path)
		{
			if (DualityApp.ExecContext == DualityApp.ExecutionContext.Terminated) return null;
			if (string.IsNullOrEmpty(path) || ResourceResolve == null) return null;

			ResourceResolveEventArgs args = new ResourceResolveEventArgs(path);
			try
			{
				ResourceResolve(null, args);
			}
			catch (Exception e)
			{
				Logs.Core.WriteError("An error occurred in custom ResourceResolve code: {0}", LogFormat.Exception(e));
			}

			if (args.Handled)
			{
				if (string.IsNullOrEmpty(args.Result.Path))
				{
					args.Result.Path = path;
				}
				AddContent(path, args.Result);
				return args.Result;
			}
			else
			{
				return null;
			}
		}
		private static Resource LoadContent(string path)
		{
			if (DualityApp.ExecContext == DualityApp.ExecutionContext.Terminated) return null;
			if (string.IsNullOrEmpty(path) || Resource.IsDefaultContentPath(path) || !FileOp.Exists(path)) return null;

			Logs.Core.Write("Loading Resource '{0}'", path);
			Logs.Core.PushIndent();

			// Load the Resource and register it
			Resource res = Resource.Load<Resource>(path, r => 
			{ 
				// Registering takes place in the callback - before initializing the Resource, because
				// that may result in requesting more Resources and an infinite loop if two Resources request
				// each other in their initialization code.
				if (r != null) AddContent(path, r);
			});

			Logs.Core.PopIndent();
			return res;
		}
	}
}
