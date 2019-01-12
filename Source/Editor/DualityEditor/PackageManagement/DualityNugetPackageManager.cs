using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using NuGet;
using NuGet.Resources;

namespace Duality.Editor.PackageManagement
{
	/// <summary>
	/// Provides nuget package functionality with targetframeworks.
	/// Hides the hacks in <see cref="NugetFixedInternal"/> 
	/// </summary>
	public class DualityNugetPackageManager
	{
		public IPackageRepository LocalRepository
		{
			get { return this._packageManager.LocalRepository; }
		}

		public ILogger Logger
		{
			get { return this._packageManager.Logger; }
			set
			{
				this._packageManager.Logger = value;
			}
		}

		public bool WhatIf
		{
			get { return this._packageManager.WhatIf; }
			set { this._packageManager.WhatIf = value; }
		}

		public event EventHandler<PackageOperationEventArgs> PackageInstalling
		{
			add { this._packageManager.PackageInstalling += value; }
			remove { this._packageManager.PackageInstalling -= value; }
		}

		public event EventHandler<PackageOperationEventArgs> PackageInstalled
		{
			add { this._packageManager.PackageInstalled += value; }
			remove { this._packageManager.PackageInstalled -= value; }
		}

		public event EventHandler<PackageOperationEventArgs> PackageUninstalled
		{
			add { this._packageManager.PackageUninstalled += value; }
			remove { this._packageManager.PackageUninstalled -= value; }
		}

		private readonly NugetFixedInternal _packageManager;

		public DualityNugetPackageManager(FrameworkName targetFrameWork, IPackageRepository sourceRepository, string path)
		{
			this._packageManager = new NugetFixedInternal(targetFrameWork, sourceRepository, path);
		}

		public void InstallPackage(IPackage package, bool ignoreDependencies, bool allowPrereleaseVersions)
		{
			this._packageManager.InstallPackage(package, ignoreDependencies, allowPrereleaseVersions);
		}

		public void UpdatePackage(IPackage newPackage, bool updateDependencies, bool allowPrereleaseVersions)
		{
			this._packageManager.UpdatePackage(newPackage, updateDependencies, allowPrereleaseVersions);
		}

		public void UninstallPackage(IPackage package, bool forceRemove, bool removeDependencies = false)
		{
			this._packageManager.UninstallPackage(package, forceRemove, removeDependencies);
		}

		/// <summary>
		/// Unfortunately nuget does not provide the ability to use a targetframework out of the box.
		/// This is what this private class solves but due to some shortcomings in the nuget API it had to use some less desirable code such as hiding a method.
		/// This is also why this class is private to prevent misuse.
		/// </summary>
		private class NugetFixedInternal : NuGet.PackageManager
		{
			private readonly FrameworkName _targetFrameWork;

			public NugetFixedInternal(FrameworkName targetFrameWork, IPackageRepository sourceRepository, string path) : base(sourceRepository, path)
			{
				this._targetFrameWork = targetFrameWork;
			}

			public override void InstallPackage(IPackage package, bool ignoreDependencies, bool allowPrereleaseVersions)
			{
				this.InstallPackage(package, this._targetFrameWork, ignoreDependencies, allowPrereleaseVersions, false);
			}

			public new void UpdatePackage(IPackage newPackage, bool updateDependencies, bool allowPrereleaseVersions)
			{
				this.Execute(newPackage, new UpdateWalker(this.LocalRepository, this.SourceRepository, new DependentsWalker(this.LocalRepository, this._targetFrameWork), NullConstraintProvider.Instance, this._targetFrameWork, this.Logger, updateDependencies, allowPrereleaseVersions));
			}

			private void Execute(IPackage package, IPackageOperationResolver resolver)
			{
				IEnumerable<PackageOperation> source = resolver.ResolveOperations(package);
				if (source.Any())
				{
					foreach (PackageOperation operation in source)
						this.Execute(operation);
				}
				else
				{
					if (!this.LocalRepository.Exists(package))
						return;
					this.Logger.Log(MessageLevel.Info, NuGetResources.Log_PackageAlreadyInstalled, (object)package.GetFullName());
				}
			}
		}
	}
}