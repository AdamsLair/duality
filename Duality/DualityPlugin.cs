using System;
using System.Linq;
using System.Reflection;

namespace Duality
{
	public abstract class DualityPlugin
	{
		private bool     disposed = false;
		private Assembly assembly = null;
		private string   asmName  = null;
		private string   filePath = null;
		private int      fileHash = 0;

		public bool Disposed
		{
			get { return this.disposed; }
		}
		public Assembly PluginAssembly
		{
			get { return this.assembly; }
		}
		public string AssemblyName
		{
			get { return this.asmName; }
		}
		public string FilePath
		{
			get { return this.filePath; }
			internal set { this.filePath = value; }
		}
		public int FileHash
		{
			get { return this.fileHash; }
			internal set { this.fileHash = value; }
		}

		protected DualityPlugin()
		{
			this.assembly = this.GetType().GetTypeInfo().Assembly;
			this.asmName = this.assembly.GetShortAssemblyName();
		}
		internal void Dispose()
		{
			if (this.disposed) return;

			this.OnDisposePlugin();

			this.disposed = true;
		}
		/// <summary>
		/// Called when unloading / disposing the plugin.
		/// </summary>
		protected virtual void OnDisposePlugin() {}
	}
}
