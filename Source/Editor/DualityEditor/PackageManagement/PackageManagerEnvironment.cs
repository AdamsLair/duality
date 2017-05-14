using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Windows.Forms;
using System.Diagnostics;

using NuGet;

using Duality.IO;
using Duality.Editor.Properties;
using Duality.Editor.Forms;

namespace Duality.Editor.PackageManagement
{
	public class PackageManagerEnvironment
	{
		private string rootPath         = null;
		private string targetDataPath   = null;
		private string targetSourcePath = null;
		private string targetPluginPath = null;
		private string localRepoPath    = null;
		private string configFilePath   = null;
		private string updateFilePath   = null;


		public string RootPath
		{
			get { return this.rootPath; }
		}
		public string TargetDataPath
		{
			get { return this.targetDataPath; }
		}
		public string TargetSourcePath
		{
			get { return this.targetSourcePath; }
		}
		public string TargetPluginPath
		{
			get { return this.targetPluginPath; }
		}
		public string RepositoryPath
		{
			get { return this.localRepoPath; }
		}
		public string ConfigFilePath
		{
			get { return this.configFilePath; }
		}
		public string UpdateFilePath
		{
			get { return this.updateFilePath; }
		}

		public string TargetPluginPathRelative
		{
			get { return DualityApp.PluginDirectory; }
		}
		public string TargetSolutionPathRelative
		{
			get { return EditorHelper.SourceCodeSolutionFile; }
		}


		public PackageManagerEnvironment(string rootPath)
		{
			this.rootPath = rootPath ?? Environment.CurrentDirectory;
			this.UpdateFromRootPath();
		}

		private void UpdateFromRootPath()
		{
			this.targetDataPath   = Path.Combine(this.rootPath, DualityApp.DataDirectory);
			this.targetSourcePath = Path.Combine(this.rootPath, EditorHelper.SourceCodeDirectory);
			this.targetPluginPath = Path.Combine(this.rootPath, this.TargetPluginPathRelative);
			this.localRepoPath    = Path.Combine(this.rootPath, EditorHelper.SourceDirectory + @"\Packages");
			this.configFilePath   = Path.Combine(this.rootPath, "PackageConfig.xml");
			this.updateFilePath   = Path.Combine(this.rootPath, "ApplyUpdate.xml");
		}
	}
}
