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
	/// <summary>
	/// Describes the local working environment of a <see cref="PackageManager"/>.
	/// </summary>
	public class PackageManagerEnvironment
	{
		private string rootPath            = null;
		private string targetDataPath      = null;
		private string targetSourcePath    = null;
		private string targetPluginPath    = null;
		private string localRepoPath       = null;
		private string configFilePath      = null;
		private string updateFilePath      = null;
		private string updaterExecFilePath = null;


		/// <summary>
		/// [GET] The root path to work in, which is usually the Duality project folder.
		/// </summary>
		public string RootPath
		{
			get { return this.rootPath; }
		}
		/// <summary>
		/// [GET] The path where <see cref="Resource"/> package contents are copied to after installation.
		/// </summary>
		public string TargetDataPath
		{
			get { return this.targetDataPath; }
		}
		/// <summary>
		/// [GET] The path where source code package contents are copied to after installation.
		/// </summary>
		public string TargetSourcePath
		{
			get { return this.targetSourcePath; }
		}
		/// <summary>
		/// [GET] The path where plugin package contents are copied to after installation.
		/// </summary>
		public string TargetPluginPath
		{
			get { return this.targetPluginPath; }
		}
		/// <summary>
		/// [GET] The path of the local package repository where packages will be installed to by NuGet.
		/// After NuGet completed its installation, Duality will take care of copying each file to
		/// its intended destination.
		/// </summary>
		public string RepositoryPath
		{
			get { return this.localRepoPath; }
		}
		/// <summary>
		/// [GET] The path where the local <see cref="PackageSetup"/> config file is located.
		/// </summary>
		public string ConfigFilePath
		{
			get { return this.configFilePath; }
		}
		/// <summary>
		/// [GET] The path where the <see cref="PackageUpdateSchedule"/> file is located.
		/// </summary>
		public string UpdateFilePath
		{
			get { return this.updateFilePath; }
		}
		/// <summary>
		/// [GET] The path of the binary that will be executed in order to apply a pending
		/// update as scheduled in the <see cref="PackageUpdateSchedule"/> at the <see cref="UpdateFilePath"/>.
		/// </summary>
		public string UpdaterExecFilePath
		{
			get { return this.updaterExecFilePath; }
		}

		/// <summary>
		/// [GET] The (<see cref="RootPath"/>-)relative path where plugin package contents are copied to after installation.
		/// See <see cref="TargetPluginPath"/> for a non-relative version.
		/// </summary>
		public string TargetPluginPathRelative
		{
			get { return DualityApp.PluginDirectory; }
		}
		/// <summary>
		/// [GET] The (<see cref="RootPath"/>-)relative path of the source code solution file where source code 
		/// project files from packages will be integrated after installation.
		/// </summary>
		public string TargetSolutionPathRelative
		{
			get { return DualityEditorApp.SolutionFileName; }
		}


		public PackageManagerEnvironment(string rootPath)
		{
			this.rootPath = rootPath ?? string.Empty;
			this.UpdateFromRootPath();
		}

		private void UpdateFromRootPath()
		{
			this.targetDataPath      = Path.Combine(this.rootPath, DualityApp.DataDirectory);
			this.targetSourcePath    = Path.Combine(this.rootPath, EditorHelper.SourceCodeDirectory);
			this.targetPluginPath    = Path.Combine(this.rootPath, this.TargetPluginPathRelative);
			this.localRepoPath       = Path.Combine(this.rootPath, EditorHelper.SourceDirectory + @"\Packages");
			this.configFilePath      = Path.Combine(this.rootPath, "PackageConfig.xml");
			this.updateFilePath      = Path.Combine(this.rootPath, "ApplyUpdate.xml");
			this.updaterExecFilePath = Path.Combine(this.rootPath, "DualityUpdater.exe");
		}
	}
}
