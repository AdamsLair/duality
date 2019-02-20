﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;

using NuGet;
using NuGet.Resources;

namespace Duality.Editor.PackageManagement.Internal
{
	/// <summary>
	/// A custom version of the NuGet <see cref="NuGet.PackageManager"/> based on a cleaned up decompile.
	/// This was necessary in order to add support for framework-dependent dependencies via target framework
	/// parameter in its internal package dependency walkers.
	/// </summary>
	public class NuGetTargetedPackageManager
	{
		private ILogger logger;
		private readonly FrameworkName targetFrameWork;


		public event EventHandler<PackageOperationEventArgs> PackageInstalling;
		public event EventHandler<PackageOperationEventArgs> PackageInstalled;
		public event EventHandler<PackageOperationEventArgs> PackageUninstalling;
		public event EventHandler<PackageOperationEventArgs> PackageUninstalled;


		public DependencyVersion DependencyVersion { get; set; }
		public bool WhatIf { get; set; }
		public IFileSystem FileSystem { get; set; }
		public IPackageRepository SourceRepository { get; private set; }
		public IPackageRepository LocalRepository { get; private set; }
		public IPackagePathResolver PathResolver { get; private set; }
		public ILogger Logger
		{
			get { return this.logger ?? NullLogger.Instance; }
			set { this.logger = value; }
		}


		public NuGetTargetedPackageManager(FrameworkName targetFrameWork, IPackageRepository sourceRepository, string path) : this(
			sourceRepository, 
			path)
		{
			this.targetFrameWork = targetFrameWork;
		}
		public NuGetTargetedPackageManager(IPackageRepository sourceRepository, string path) : this(
			sourceRepository, 
			new DefaultPackagePathResolver(path), 
			new PhysicalFileSystem(path)) { }
		public NuGetTargetedPackageManager(IPackageRepository sourceRepository, IPackagePathResolver pathResolver, IFileSystem fileSystem) : this(
			sourceRepository, 
			pathResolver, 
			fileSystem, 
			new LocalPackageRepository(pathResolver, fileSystem)) { }
		public NuGetTargetedPackageManager(IPackageRepository sourceRepository, IPackagePathResolver pathResolver, IFileSystem fileSystem, IPackageRepository localRepository)
		{
			if (sourceRepository == null) throw new ArgumentNullException("sourceRepository");
			if (pathResolver == null) throw new ArgumentNullException("pathResolver");
			if (fileSystem == null) throw new ArgumentNullException("fileSystem");
			if (localRepository == null) throw new ArgumentNullException("localRepository");

			this.SourceRepository = sourceRepository;
			this.PathResolver = pathResolver;
			this.FileSystem = fileSystem;
			this.LocalRepository = localRepository;
			this.DependencyVersion = DependencyVersion.Lowest;
		}

		public void InstallPackage(IPackage package, bool ignoreDependencies, bool allowPrereleaseVersions)
		{
			this.Execute(package, new InstallWalker(
				this.LocalRepository,
				this.SourceRepository,
				this.targetFrameWork,
				this.Logger,
				ignoreDependencies,
				allowPrereleaseVersions,
				this.DependencyVersion));
		}
		public void UninstallPackage(IPackage package, bool forceRemove, bool removeDependencies = false)
		{
			this.Execute(package, new UninstallWalker(
				this.LocalRepository, 
				new DependentsWalker(this.LocalRepository, this.targetFrameWork),
				this.targetFrameWork, 
				this.Logger, 
				removeDependencies, 
				forceRemove));
		}
		public void UpdatePackage(IPackage newPackage, bool updateDependencies, bool allowPrereleaseVersions)
		{
			this.Execute(newPackage, new UpdateWalker(
				this.LocalRepository, 
				this.SourceRepository,
				new DependentsWalker(this.LocalRepository, this.targetFrameWork),
				NullConstraintProvider.Instance,
				this.targetFrameWork,
				this.Logger, updateDependencies,
				allowPrereleaseVersions));
		}


		private void Execute(IPackage package, IPackageOperationResolver resolver)
		{
			IEnumerable<PackageOperation> source = resolver.ResolveOperations(package);
			if (source.Any())
			{
				foreach (PackageOperation operation in source)
				{
					this.Execute(operation);
				}
			}
			else
			{
				if (!this.LocalRepository.Exists(package))
					return;
				this.Logger.Log(MessageLevel.Info, NuGetResources.Log_PackageAlreadyInstalled, (object)package.GetFullName());
			}
		}
		private void Execute(PackageOperation operation)
		{
			bool isPackageInstalled = this.LocalRepository.Exists(operation.Package);
			if (operation.Action == PackageAction.Install)
			{
				if (isPackageInstalled)
					this.Logger.Log(MessageLevel.Info, NuGetResources.Log_PackageAlreadyInstalled, (object)operation.Package.GetFullName());
				else if (this.WhatIf)
					this.Logger.Log(MessageLevel.Info, NuGetResources.Log_InstallPackage, (object)operation.Package);
				else
					this.ExecuteInstall(operation.Package);
			}
			else
			{
				if (!isPackageInstalled)
					return;
				if (this.WhatIf)
					this.Logger.Log(MessageLevel.Info, NuGetResources.Log_UninstallPackage, (object)operation.Package);
				else
					this.ExecuteUninstall(operation.Package);
			}
		}

		private void ExecuteInstall(IPackage package)
		{
			string fullName = package.GetFullName();
			this.Logger.Log(MessageLevel.Info, NuGetResources.Log_BeginInstallPackage, (object)fullName);
			PackageOperationEventArgs operation = this.CreateOperation(package);
			this.OnInstalling(operation);
			if (operation.Cancel)
				return;
			this.ExpandFiles(operation.Package);
			this.LocalRepository.AddPackage(package);
			this.Logger.Log(MessageLevel.Info, NuGetResources.Log_PackageInstalledSuccessfully, (object)fullName);
			this.OnInstalled(operation);
		}
		private void ExecuteUninstall(IPackage package)
		{
			string fullName = package.GetFullName();
			this.Logger.Log(MessageLevel.Info, NuGetResources.Log_BeginUninstallPackage, (object)fullName);
			PackageOperationEventArgs operation = this.CreateOperation(package);
			this.OnUninstalling(operation);
			if (operation.Cancel)
				return;
			this.RemoveFiles(operation.Package);
			this.LocalRepository.RemovePackage(package);
			this.Logger.Log(MessageLevel.Info, NuGetResources.Log_SuccessfullyUninstalledPackage, (object)fullName);
			this.OnUninstalled(operation);
		}

		private void ExpandFiles(IPackage package)
		{
			IBatchProcessor<string> fileSystem = this.FileSystem as IBatchProcessor<string>;
			try
			{
				List<IPackageFile> list = package.GetFiles().ToList();
				if (fileSystem != null)
					fileSystem.BeginProcessing(
						list.Select(p => p.Path),
						PackageAction.Install);
				string packageDirectory = this.PathResolver.GetPackageDirectory(package);
				this.FileSystem.AddFiles(list, packageDirectory);
				IPackage runtimePackage;
				if (!PackageHelper.IsSatellitePackage(package, this.LocalRepository, this.targetFrameWork, out runtimePackage))
					return;
				this.FileSystem.AddFiles(package.GetSatelliteFiles(), this.PathResolver.GetPackageDirectory(runtimePackage));
			}
			finally
			{
				if (fileSystem != null) fileSystem.EndProcessing();
			}
		}
		private void RemoveFiles(IPackage package)
		{
			string packageDirectory = this.PathResolver.GetPackageDirectory(package);
			IPackage runtimePackage;
			if (PackageHelper.IsSatellitePackage(package, this.LocalRepository, this.targetFrameWork, out runtimePackage))
				this.FileSystem.DeleteFiles(package.GetSatelliteFiles(), this.PathResolver.GetPackageDirectory(runtimePackage));
			this.FileSystem.DeleteFiles(package.GetFiles(), packageDirectory);
		}

		private void OnInstalling(PackageOperationEventArgs e)
		{
			if (this.PackageInstalling != null)
				this.PackageInstalling(this, e);
		}
		private void OnInstalled(PackageOperationEventArgs e)
		{
			if (this.PackageInstalled != null)
				this.PackageInstalled(this, e);
		}
		private void OnUninstalling(PackageOperationEventArgs e)
		{
			if (this.PackageUninstalling != null)
				this.PackageUninstalling(this, e);
		}
		private void OnUninstalled(PackageOperationEventArgs e)
		{
			if (this.PackageUninstalled != null)
				this.PackageUninstalled(this, e);
		}

		private PackageOperationEventArgs CreateOperation(IPackage package)
		{
			return new PackageOperationEventArgs(package, this.FileSystem, this.PathResolver.GetInstallPath(package));
		}
	}
}