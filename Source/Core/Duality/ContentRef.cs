﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;

using Duality.Resources;
using Duality.Cloning;

namespace Duality
{
	
	/// <summary>
	/// This lightweight struct references <see cref="Resource">Resources</see> in an abstract way. It
	/// is tightly connected to the <see cref="ContentProvider"/> and takes care of keeping or making 
	/// the referenced content available when needed. Never store actual Resource references permanently,
	/// instead use a ContentRef to it. However, you may retrieve and store a direct Resource reference
	/// temporarily, although this is only recommended at method-local scope.
	/// </summary>
	/// <seealso cref="Resource"/>
	/// <seealso cref="ContentProvider"/>
	/// <seealso cref="IContentRef"/>
	[DebuggerTypeProxy(typeof(ContentRef<>.DebuggerTypeProxy))]
	public struct ContentRef<T> : IEquatable<ContentRef<T>>, IContentRef where T : Resource
	{
		[DontSerialize, CloneBehavior(CloneBehavior.Reference), CloneField(CloneFieldFlags.DontSkip)]
		private T      contentInstance;
		private string contentPath;
		
		/// <summary>
		/// [GET / SET] The actual <see cref="Resource"/>. If currently unavailable, it is loaded and then returned.
		/// Because of that, this Property is only null if the references Resource is missing, invalid, or
		/// this content reference has been explicitly set to null. Never returns disposed Resources.
		/// </summary>
		public T Res
		{
			get 
			{ 
				if (this.contentInstance == null || this.contentInstance.Disposed) this.RetrieveInstance();
				return this.contentInstance;
			}
			set
			{
				this.contentPath = value == null ? null : value.Path;
				this.contentInstance = value;
			}
		}
		/// <summary>
		/// [GET] Returns the current reference to the Resource that is stored locally. No attemp is made to load or reload
		/// the Resource if currently unavailable.
		/// </summary>
		public T ResWeak
		{
			get { return (this.contentInstance == null || this.contentInstance.Disposed) ? null : this.contentInstance; }
		}
		/// <summary>
		/// [GET] The <see cref="System.Type"/> of the referenced Resource. If currently unavailable, this is determined by
		/// the Resource file path.
		/// </summary>
		public Type ResType
		{
			get
			{
				// If we have a valid instance, ask for its type.
				if (this.contentInstance != null && !this.contentInstance.Disposed)
					return this.contentInstance.GetType();
				
				// Otherwise, try to derive the type from its path.
				Type result = Resource.GetTypeByFileName(this.contentPath);
				if (result != null) return result;

				// If that fails, load or retrieve a Resource instance and ask for its type.
				this.RetrieveInstance();
				if (this.contentInstance != null && !this.contentInstance.Disposed)
					return this.contentInstance.GetType();

				// If that fails too, just remain unspecific by stating that it's a Resource.
				return typeof(Resource);
			}
		}
		/// <summary>
		/// [GET / SET] The path where to look for the Resource, if it is currently unavailable.
		/// </summary>
		public string Path
		{
			get { return this.contentPath; }
			set
			{
				this.contentPath = value;
				if (this.contentInstance != null && this.contentInstance.Path != value)
					this.contentInstance = null;
			}
		}
		/// <summary>
		/// [GET] Returns whether this content reference has been explicitly set to null.
		/// </summary>
		public bool IsExplicitNull
		{
			get
			{
				return this.contentInstance == null && string.IsNullOrEmpty(this.contentPath);
			}
		}
		/// <summary>
		/// [GET] Returns whether this content reference is available in general. This may trigger loading it, if currently unavailable.
		/// </summary>
		public bool IsAvailable
		{
			get
			{
				if (this.contentInstance != null && !this.contentInstance.Disposed) return true;
				this.RetrieveInstance();
				return this.contentInstance != null;
			}
		}
		/// <summary>
		/// [GET] Returns whether the referenced Resource is currently loaded.
		/// </summary>
		public bool IsLoaded
		{
			get
			{
				if (this.contentInstance != null && !this.contentInstance.Disposed) return true;
				return ContentProvider.HasContent(this.contentPath);
			}
		}
		/// <summary>
		/// [GET] Returns whether the referenced Resource is part of Duality's embedded default content.
		/// </summary>
		public bool IsDefaultContent
		{
			get { return Resource.IsDefaultContentPath(this.contentPath); }
		}
		/// <summary>
		/// [GET] Returns whether the Resource has been generated at runtime and cannot be retrieved via content path.
		/// </summary>
		public bool IsRuntimeResource
		{
			get { return this.contentInstance != null && string.IsNullOrEmpty(this.contentPath); }
		}
		/// <summary>
		/// [GET] The name of the referenced Resource.
		/// </summary>
		public string Name
		{
			get
			{
				if (this.IsRuntimeResource)
					return this.contentInstance.GetHashCode().ToString(CultureInfo.InvariantCulture);
				else
					return Resource.GetNameFromPath(this.contentPath);
			}
		}
		/// <summary>
		/// [GET] The full name of the referenced Resource, including its path but not its file extension
		/// </summary>
		public string FullName
		{
			get
			{
				if (this.IsRuntimeResource)
					return this.contentInstance.GetHashCode().ToString(CultureInfo.InvariantCulture);
				else
					return Resource.GetFullNameFromPath(this.contentPath);
			}
		}


		/// <summary>
		/// Creates a ContentRef pointing to the specified <see cref="Resource"/>, assuming the
		/// specified path as its origin, if the Resource itsself is either null or doesn't
		/// provide a valid <see cref="Resource.Path"/>.
		/// </summary>
		/// <param name="res">The Resource to reference.</param>
		/// <param name="requestPath">The referenced Resource's file path.</param>
		public ContentRef(T res, string requestPath)
		{
			this.contentInstance = res;
			if (!string.IsNullOrEmpty(requestPath))
				this.contentPath = requestPath;
			else if (res != null && !string.IsNullOrEmpty(res.Path))
				this.contentPath = res.Path;
			else 
				this.contentPath = requestPath;
		}
		/// <summary>
		/// Creates a ContentRef pointing to the <see cref="Resource"/> at the specified path / using 
		/// the specified alias.
		/// </summary>
		/// <param name="path"></param>
		public ContentRef(string path)
		{
			this.contentInstance = null;
			this.contentPath = path;
		}
		/// <summary>
		/// Creates a ContentRef pointing to the specified <see cref="Resource"/>.
		/// </summary>
		/// <param name="res">The Resource to reference.</param>
		public ContentRef(T res)
		{
			this.contentInstance = res;
			this.contentPath = (res != null) ? res.Path : null;
		}
		
		/// <summary>
		/// Determines if the references Resource's Type is assignable to the specified Type.
		/// </summary>
		/// <param name="resType">The Resource Type in question.</param>
		/// <returns>True, if the referenced Resource is of the specified Type or subclassing it.</returns>
		public bool Is(Type resType)
		{
			TypeInfo resTypeInfo = resType.GetTypeInfo();
			return resTypeInfo.IsAssignableFrom(typeof(Resource).GetTypeInfo()) || resTypeInfo.IsAssignableFrom(this.ResType.GetTypeInfo());
		}
		/// <summary>
		/// Determines if the references Resource's Type is assignable to the specified Type.
		/// </summary>
		/// <typeparam name="U">The Resource Type in question.</typeparam>
		/// <returns>True, if the referenced Resource is of the specified Type or subclassing it.</returns>
		public bool Is<U>() where U : Resource
		{
			if (this.contentInstance != null && !this.contentInstance.Disposed)
			{
				return this.contentInstance is U;
			}
			else
			{
				TypeInfo typeInfoU = typeof(U).GetTypeInfo();
				return typeInfoU.IsAssignableFrom(typeof(Resource).GetTypeInfo()) || typeInfoU.IsAssignableFrom(this.ResType.GetTypeInfo());
			}
		}
		/// <summary>
		/// Creates a <see cref="ContentRef{T}"/> of the specified Type, referencing the same Resource.
		/// </summary>
		/// <typeparam name="U">The Resource Type to create a reference of.</typeparam>
		/// <returns>
		/// A <see cref="ContentRef{T}"/> of the specified Type, referencing the same Resource.
		/// Returns a null reference if the Resource is not assignable
		/// to the specified Type.
		/// </returns>
		public ContentRef<U> As<U>() where U : Resource
		{
			if (!Is<U>()) return null;
			return new ContentRef<U>(this.contentInstance as U, this.contentPath);
		}

		/// <summary>
		/// Loads the associated content as if it was accessed now.
		/// You don't usually need to call this method. It is invoked implicitly by trying to 
		/// access the <see cref="ContentRef{T}"/>.
		/// </summary>
		public void EnsureLoaded()
		{
			if (this.contentInstance == null || this.contentInstance.Disposed)
				this.RetrieveInstance();
		}
		/// <summary>
		/// Discards the resolved content reference cache to allow garbage-collecting the Resource
		/// without losing its reference. Accessing it will result in reloading the Resource.
		/// </summary>
		public void Detach()
		{
			this.contentInstance = null;
		}
		private void RetrieveInstance()
		{
			if (!string.IsNullOrEmpty(this.contentPath))
				this.contentInstance = ContentProvider.RequestContent<T>(this.contentPath).contentInstance;
			else if (this.contentInstance != null && !string.IsNullOrEmpty(this.contentInstance.Path))
				this = ContentProvider.RequestContent<T>(this.contentInstance.Path);
			else
				this.contentInstance = null;
		}

		public override string ToString()
		{
			Type resType = this.ResType;

			char stateChar;
			if (this.IsDefaultContent)
				stateChar = 'D';
			else if (this.IsRuntimeResource)
				stateChar = 'R';
			else if (this.IsExplicitNull)
				stateChar = 'N';
			else if (this.IsLoaded)
				stateChar = 'L';
			else
				stateChar = '_';

			return string.Format("[{2}] {0} \"{1}\"", resType.Name, this.FullName, stateChar);
		}
		public override bool Equals(object obj)
		{
			if (obj is ContentRef<T>)
				return this == (ContentRef<T>)obj;
			else
				return base.Equals(obj);
		}
		public override int GetHashCode()
		{
			if (this.contentPath != null) return this.contentPath.GetHashCode();
			else if (this.contentInstance != null) return this.contentInstance.GetHashCode();
			else return 0;
		}
		public bool Equals(ContentRef<T> other)
		{
			return this == other;
		}

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		Resource IContentRef.Res
		{
			get { return this.Res; }
		}
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		Resource IContentRef.ResWeak
		{
			get { return this.ResWeak; }
		}

		public static implicit operator ContentRef<T>(T res)
		{
			return new ContentRef<T>(res);
		}
		public static explicit operator T(ContentRef<T> res)
		{
			return res.Res;
		}

		/// <summary>
		/// Compares two ContentRefs for equality.
		/// </summary>
		/// <param name="first"></param>
		/// <param name="second"></param>
		/// <remarks>
		/// This is a two-step comparison. First, their actual Resources references are compared.
		/// If they're both not null and equal, true is returned. Otherwise, their Resource paths
		/// are compared for equality
		/// </remarks>
		public static bool operator ==(ContentRef<T> first, ContentRef<T> second)
		{
			// Old check, didn't work for XY == null when XY was a Resource created at runtime
			//if (first.contentInstance != null && second.contentInstance != null)
			//    return first.contentInstance == second.contentInstance;
			//else
			//    return first.contentPath == second.contentPath;

			// Completely identical
			if (first.contentInstance == second.contentInstance && first.contentPath == second.contentPath)
				return true;
			// Same instances
			else if (first.contentInstance != null && second.contentInstance != null)
				return first.contentInstance == second.contentInstance;
			// Null checks
			else if (first.IsExplicitNull) return second.IsExplicitNull;
			else if (second.IsExplicitNull) return first.IsExplicitNull;
			// Path comparison
			else
			{
				string firstPath = first.contentInstance != null ? first.contentInstance.Path : first.contentPath;
				string secondPath = second.contentInstance != null ? second.contentInstance.Path : second.contentPath;
				return firstPath == secondPath;
			}
		}
		/// <summary>
		/// Compares two ContentRefs for inequality.
		/// </summary>
		/// <param name="first"></param>
		/// <param name="second"></param>
		public static bool operator !=(ContentRef<T> first, ContentRef<T> second)
		{
			return !(first == second);
		}

		internal class DebuggerTypeProxy
		{
			private	ContentRef<T>	cr;

			[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
			public T Res
			{
				get { return this.cr.Res; }
			}

			public DebuggerTypeProxy(ContentRef<T> cr)
			{
				this.cr = cr;
			}
		}
	}
}
