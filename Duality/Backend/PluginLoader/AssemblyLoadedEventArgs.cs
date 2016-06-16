using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace Duality.Backend
{
	/// <summary>
	/// Event arguments for notifications about the runtime loading an <see cref="Assembly"/>.
	/// </summary>
	public class AssemblyLoadedEventArgs : EventArgs
	{
		private Assembly loadedAssembly;

		/// <summary>
		/// [GET] The <see cref="Assembly"/> that was loaded by the runtime.
		/// </summary>
		public Assembly LoadedAssembly
		{
			get { return this.loadedAssembly; }
		}

		public AssemblyLoadedEventArgs(Assembly assembly)
		{
			this.loadedAssembly = assembly;
		}
	}
}
