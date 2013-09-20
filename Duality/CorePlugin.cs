using System;
using System.Linq;
using System.Reflection;

namespace Duality
{
	public abstract class CorePlugin
	{
		private	bool		disposed	= false;
		private	Assembly	assembly	= null;
		private	string		asmName		= null;
		private	string 		filePath	= null;
		private	int			fileHash	= 0;

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
			internal set
			{
				this.filePath = value;
				this.fileHash = PathHelper.GetFileHash(this.filePath);
			}
		}
		public int FileHash
		{
			get { return this.fileHash; }
		}

		protected CorePlugin()
		{
			this.assembly = this.GetType().Assembly;
			this.asmName = this.assembly.GetShortAssemblyName();
		}
		internal void Dispose()
		{
			if (this.disposed) return;

			this.OnDisposePlugin();

			this.disposed = true;
		}
		/// <summary>
		/// Called when initializing the plugin.
		/// </summary>
		internal protected virtual void InitPlugin() {}
		/// <summary>
		/// Called when unloading / disposing the plugin.
		/// </summary>
		protected virtual void OnDisposePlugin() {}
		/// <summary>
		/// Called before Duality updates the game scene
		/// </summary>
		internal protected virtual void OnBeforeUpdate() {}
		/// <summary>
		/// Called after Duality updates the game scene
		/// </summary>
		internal protected virtual void OnAfterUpdate() {}
		/// <summary>
		/// Called when Dualitys <see cref="DualityApp.ExecutionContext"/> changes.
		/// </summary>
		internal protected virtual void OnExecContextChanged(DualityApp.ExecutionContext previousContext) {}
	}
}
