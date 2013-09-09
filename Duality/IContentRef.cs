using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Duality
{
	/// <summary>
	/// IContentRef is a general interface for <see cref="ContentRef{T}">content references</see> of any <see cref="Resource"/> type.
	/// </summary>
	/// <seealso cref="Resource"/>
	/// <seealso cref="ContentProvider"/>
	/// <seealso cref="ContentRef{T}"/>
	public interface IContentRef
	{
		/// <summary>
		/// [GET] Returns the actual <see cref="Resource"/>. If currently unavailable, it is loaded and then returned.
		/// Because of that, this Property is only null if the references Resource is missing, invalid, or
		/// this content reference has been explicitly set to null. Never returns disposed Resources.
		/// </summary>
		Resource Res { get; }
		/// <summary>
		/// [GET] Returns the current reference to the Resource that is stored locally. No attemp is made to load or reload
		/// the Resource if currently unavailable.
		/// </summary>
		Resource ResWeak { get; }
		/// <summary>
		/// [GET] The <see cref="System.Type"/> of the referenced Resource. If currently unavailable, this is determined by
		/// the Resource file path.
		/// </summary>
		Type ResType { get; }
		/// <summary>
		/// [GET / SET] The path where to look for the Resource, if it is currently unavailable.
		/// </summary>
		string Path { get; set; }
		/// <summary>
		/// [GET] Returns whether this content reference has been explicitly set to null.
		/// </summary>
		bool IsExplicitNull { get; }
		/// <summary>
		/// [GET] Returns whether this content reference is available in general. This may trigger loading it, if currently unavailable.
		/// </summary>
		bool IsAvailable { get; }
		/// <summary>
		/// [GET] Returns whether the referenced Resource is currently loaded.
		/// </summary>
		bool IsLoaded { get; }
		/// <summary>
		/// [GET] Returns whether the referenced Resource is part of Duality's embedded default content.
		/// </summary>
		bool IsDefaultContent { get; }
		/// <summary>
		/// [GET] Returns whether the Resource has been generated at runtime and cannot be retrieved via content path.
		/// </summary>
		bool IsRuntimeResource { get; }
		/// <summary>
		/// [GET] The name of the referenced Resource.
		/// </summary>
		string Name { get; }
		/// <summary>
		/// [GET] The full name of the referenced Resource, including its path but not its file extension
		/// </summary>
		string FullName { get; }

		/// <summary>
		/// Determines if the references Resource's Type is assignable to the specified Type.
		/// </summary>
		/// <param name="resType">The Resource Type in question.</param>
		/// <returns>True, if the referenced Resource is of the specified Type or subclassing it.</returns>
		bool Is(Type resType);
		/// <summary>
		/// Determines if the references Resource's Type is assignable to the specified Type.
		/// </summary>
		/// <typeparam name="U">The Resource Type in question.</typeparam>
		/// <returns>True, if the referenced Resource is of the specified Type or subclassing it.</returns>
		bool Is<U>() where U : Resource;
		/// <summary>
		/// Creates a <see cref="ContentRef{T}"/> of the specified Type, referencing the same Resource.
		/// </summary>
		/// <typeparam name="U">The Resource Type to create a reference of.</typeparam>
		/// <returns>
		/// A <see cref="ContentRef{T}"/> of the specified Type, referencing the same Resource.
		/// Returns a <see cref="ContentRef{T}.Null">null reference</see> if the Resource is not assignable
		/// to the specified Type.
		/// </returns>
		ContentRef<U> As<U>() where U : Resource;
		
		/// <summary>
		/// Loads the associated content as if it was accessed now.
		/// You don't usually need to call this method. It is invoked implicitly by trying to access the ContentRef.
		/// </summary>
		void MakeAvailable();
		/// <summary>
		/// Discards the resolved content reference cache to allow garbage-collecting the Resource
		/// without losing its reference. Accessing it will result in reloading the Resource.
		/// </summary>
		void Detach();
	}
}
