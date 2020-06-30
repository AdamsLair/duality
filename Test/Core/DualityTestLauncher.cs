using System;
using Duality.Launcher;
using System.IO;

namespace Duality.Tests
{
	/// <summary>
	/// Launcher for duality for a test environment (such as unit tests, benchmarks etc).
	/// </summary>
	public class DualityTestLauncher : IDisposable
	{
		public string CodeBasePath { get; }
		private string oldEnvDir;
		private DualityLauncher launcher;

		/// <summary>
		/// Setups duality for testing.
		/// </summary>
		public DualityTestLauncher()
		{
			// Set environment directory to Duality binary directory
			this.oldEnvDir = Environment.CurrentDirectory;
			string codeBaseURI = typeof(DualityApp).Assembly.CodeBase;
			this.CodeBasePath = (codeBaseURI.StartsWith("file:") ? codeBaseURI.Remove(0, "file:".Length) : codeBaseURI).TrimStart('/');
			Console.WriteLine("Testing Core Assembly: {0}", this.CodeBasePath);
			Environment.CurrentDirectory = Path.GetDirectoryName(this.CodeBasePath);

			this.launcher = new DualityLauncher();
		}

		public void Dispose()
		{
			this.launcher.Dispose();

			Environment.CurrentDirectory = this.oldEnvDir;
		}
	}
}
