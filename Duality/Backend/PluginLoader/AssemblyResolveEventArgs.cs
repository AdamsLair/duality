using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace Duality.Backend
{
	/// <summary>
	/// Event arguments for handling an <see cref="Assembly"/> resolve operation.
	/// </summary>
	public class AssemblyResolveEventArgs : EventArgs
	{
		private string fullAssemblyName;
		private string assemblyName;
		private Assembly resolvedAssembly;

		/// <summary>
		/// [GET] The full name of the <see cref="Assembly"/> to resolve.
		/// </summary>
		public string FullAssemblyName
		{
			get { return this.fullAssemblyName; }
		}
		/// <summary>
		/// [GET] The short name of the <see cref="Assembly"/> to resolve.
		/// </summary>
		public string AssemblyName
		{
			get { return this.assemblyName; }
		}
		/// <summary>
		/// [GET] The <see cref="Assembly"/> to which the name was resolved.
		/// </summary>
		public Assembly ResolvedAssembly
		{
			get { return this.resolvedAssembly; }
		}
		/// <summary>
		/// [GET] Whether the <see cref="Assembly"/> was resolved successfully.
		/// </summary>
		public bool IsResolved
		{
			get { return this.resolvedAssembly != null; }
		}

		public AssemblyResolveEventArgs(string fullAssemblyName)
		{
			this.fullAssemblyName = fullAssemblyName;
			this.assemblyName = ReflectionHelper.GetShortAssemblyName(fullAssemblyName);
		}
		/// <summary>
		/// Resolves the queried <see cref="AssemblyResolveEventArgs.AssemblyName"/> with the specified <see cref="Assembly"/>.
		/// </summary>
		/// <param name="assembly"></param>
		public void Resolve(Assembly assembly)
		{
			this.resolvedAssembly = assembly ?? this.resolvedAssembly;
		}
	}
}
