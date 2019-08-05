using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Packaging.Signing;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Resolver;
using NuGet.Versioning;

namespace Duality.Editor.PackageManagement.Internal
{
	public interface INuGetTargetedPackageManager
	{
		event EventHandler<PackageInstallingOperation> PackageInstalling;
		event EventHandler<PackageInstalledOperation> PackageInstalled;
		event EventHandler<PackageOperationEventArgs> PackageUninstalling;
		event EventHandler<PackageOperationEventArgs> PackageUninstalled;
		void InstallPackage(PackageIdentity package);
		void UninstallPackage(PackageIdentity package, bool forceRemove, bool removeDependencies = false);
		void UpdatePackage(string id, bool allowPrereleaseVersions);
		IEnumerable<IPackageSearchMetadata> Search(string searchTerm, bool includePrereleases = false);
		IPackageSearchMetadata GetMetadata(PackageIdentity packageIdentity);
		ISet<PackageIdentity> GetInstalledPackages();

		CanUninstallResult CanUninstallPackage(PackageIdentity packageIdentity);
	}

	public class PackageInstalledOperation : EventArgs
	{
		public PackageIdentity Package { get; }
		public DownloadResourceResult Result { get; }

		public PackageInstalledOperation(PackageIdentity package, DownloadResourceResult result)
		{
			this.Package = package;
			this.Result = result;
		}
	}

	public class PackageInstallingOperation : EventArgs
	{
		public PackageIdentity Package { get; }
		public bool Cancel { get; set; }

		public PackageInstallingOperation(PackageIdentity package)
		{
			this.Package = package;
		}
	}

	public class PackageOperationEventArgs : EventArgs
	{
        public PackageIdentity Package { get; }
		public PackageReaderBase Reader { get; }

        public PackageOperationEventArgs(PackageIdentity package, PackageReaderBase reader)
        {
			this.Package = package;
			this.Reader = reader;
        }
	}

	public class NuGetTargetedPackageManager : INuGetTargetedPackageManager
	{
		private readonly ILogger _logger;
		private readonly ISettings _settings;
		private readonly NuGetFramework _nuGetFramework;

		private readonly PackagePathResolver _packagePathResolver;
		private readonly string _globalPackagesFolder;

		//private readonly string _packageConfigPath;
		private readonly PackageConfig _packageConfig;

		private readonly SourceRepository[] _repositories;

		public NuGetTargetedPackageManager(NuGetFramework nuGetFramework, ILogger logger, ISettings settings, string packagePath)
		{
			this._logger = logger;
			this._settings = settings;
			this._nuGetFramework = nuGetFramework;
			this._packagePathResolver = new PackagePathResolver(packagePath);
			var sourceRepositoryProvider = new SourceRepositoryProvider(this._settings, Repository.Provider.GetCoreV3());
			this._repositories = sourceRepositoryProvider.GetRepositories().ToArray();
			this._globalPackagesFolder = SettingsUtility.GetGlobalPackagesFolder(this._settings);
			this._packageConfig = new PackageConfig("packages.xml");
		}

		public event EventHandler<PackageInstallingOperation> PackageInstalling;
		public event EventHandler<PackageInstalledOperation> PackageInstalled;
		public event EventHandler<PackageOperationEventArgs> PackageUninstalling;
		public event EventHandler<PackageOperationEventArgs> PackageUninstalled;

		public void InstallPackage(PackageIdentity packageIdentity)
		{
			Task.Run(async () => await this.InstallPackageAsync(packageIdentity)).Wait();
		}

		public void UpdatePackage(string id, bool allowPrereleaseVersions)
		{
			Task.Run(async () => await this.UpdatePackageAsync(id, allowPrereleaseVersions)).Wait();
		}

		public IPackageSearchMetadata GetMetadata(PackageIdentity packageIdentity)
		{
			return Task.Run(async () => await this.GetMetadataAsync(packageIdentity)).Result;
		}

		public IEnumerable<IPackageSearchMetadata> Search(string searchTerm, bool includePrereleases = false)
		{
            return Task.Run(async () => await this.SearchAsync(searchTerm, includePrereleases)).Result;
		}

		public void UninstallPackage(PackageIdentity package, bool forceRemove, bool removeDependencies = false)
		{
			Task.Run(async () => await this.UninstallPackageAsync(package, forceRemove, removeDependencies)).Wait();
		}

		public CanUninstallResult CanUninstallPackage(PackageIdentity packageIdentity)
		{
			return Task.Run(async () => await this.CanUninstallPackageAsync(packageIdentity)).Result;
		}

		private async Task<IPackageSearchMetadata> GetMetadataAsync(PackageIdentity packageIdentity)
		{
			PackageMetadataResource[] packageMetadataResources = this._repositories.Select(x => x.GetResource<PackageMetadataResource>()).ToArray();

			using (var cacheContext = new SourceCacheContext())
			{
				foreach (PackageMetadataResource packageMetadataResource in packageMetadataResources)
				{
					IPackageSearchMetadata metadata = await packageMetadataResource.GetMetadataAsync(packageIdentity,
						cacheContext, this._logger, CancellationToken.None);
					if (metadata != null) return metadata;
				}

				throw new Exception($"Could not find metadata for package {packageIdentity}");
			}
		}

		private async Task<IEnumerable<IPackageSearchMetadata>> SearchAsync(string searchTerm, bool includePrereleases = false)
		{
			PackageSearchResource[] packageMetadataResources = this._repositories.Select(x => x.GetResource<PackageSearchResource>()).ToArray();

			IEnumerable<IPackageSearchMetadata> result = Enumerable.Empty<IPackageSearchMetadata>();
			foreach (PackageSearchResource packageMetadataResource in packageMetadataResources)
			{
				result = result.Concat(await packageMetadataResource.SearchAsync(searchTerm, new SearchFilter(includePrereleases), 0, 1000, this._logger, CancellationToken.None));
			}

			return result;
		}

		private async Task UpdatePackageAsync(string id, bool allowPrereleaseVersions = false)
		{
			IPackageSearchMetadata packageMetadata = (await this.SearchAsync($"id:{id}", allowPrereleaseVersions)).First();
			NuGetVersion latestVersion = (await packageMetadata.GetVersionsAsync()).Max(x => x.Version);
			await this.InstallPackageAsync(new PackageIdentity(id, latestVersion));
		}

		private async Task InstallPackageAsync(PackageIdentity packageIdentity)
		{
			using (var cacheContext = new SourceCacheContext())
			{
				HashSet<SourcePackageDependencyInfo> availablePackages =
					await this.GetPackageDependencies(packageIdentity, cacheContext);

				var resolverContext = new PackageResolverContext(
					DependencyBehavior.Lowest,
					new[] {packageIdentity.Id},
					Enumerable.Empty<string>(),
					Enumerable.Empty<PackageReference>(),
					Enumerable.Empty<PackageIdentity>(),
					availablePackages,
					this._repositories.Select(s => s.PackageSource),
					NullLogger.Instance);

				var resolver = new PackageResolver();
				PackageIdentity[] packagesToInstall =
					resolver.Resolve(resolverContext, CancellationToken.None).ToArray();

				foreach (PackageIdentity identity in packagesToInstall)
				{
					PackageIdentity[] previouslyInstalledPackages =
						this._packageConfig.Packages.Where(x => x.Id == identity.Id)
							.ToArray(); //Take a copy to avoid modifying the enumerable
					foreach (PackageIdentity previouslyInstalledPackage in previouslyInstalledPackages)
					{
						if (previouslyInstalledPackage.Version == identity.Version)
							continue; //Nothing changed so just skip to save some time.
						this.TryRemovePackageFolder(previouslyInstalledPackage);
						this._packageConfig.Remove(previouslyInstalledPackage);
					}

					this._packageConfig.Add(identity);
				}

				this._packageConfig.Serialize();

				var packageExtractionContext = new PackageExtractionContext(
					PackageSaveMode.Defaultv3,
					XmlDocFileSaveMode.None,
					ClientPolicyContext.GetClientPolicy(this._settings, this._logger),
					this._logger);

				DependencyInfoResource[] dependencyInfoResources =
					this._repositories.Select(x => x.GetResource<DependencyInfoResource>()).ToArray();
				foreach (PackageIdentity packageToInstall in packagesToInstall)
				{
					await this.RestorePackage(dependencyInfoResources, packageToInstall, cacheContext,
						packageExtractionContext);
				}
			}
		}

		public async Task RestorePackages()
		{
			DependencyInfoResource[] dependencyInfoResources = this._repositories.Select(x => x.GetResource<DependencyInfoResource>()).ToArray();

			using (var cacheContext = new SourceCacheContext())
			{

				var packageExtractionContext = new PackageExtractionContext(
					PackageSaveMode.Defaultv3,
					XmlDocFileSaveMode.None,
					ClientPolicyContext.GetClientPolicy(this._settings, this._logger),
					this._logger);

				foreach (PackageIdentity packageIdentity in this._packageConfig.Packages)
				{
					await this.RestorePackage(dependencyInfoResources, packageIdentity, cacheContext,
						packageExtractionContext);
				}
			}
		}

		private async Task RestorePackage(DependencyInfoResource[] dependencyInfoResources, PackageIdentity packageIdentity, SourceCacheContext cacheContext, PackageExtractionContext packageExtractionContext)
		{
			PackageInstallingOperation installingArgs = new PackageInstallingOperation(packageIdentity);
			this.PackageInstalling?.Invoke(this, installingArgs);
			foreach (DependencyInfoResource dependencyInfoResource in dependencyInfoResources)
			{
				SourcePackageDependencyInfo dependencyInfo = await dependencyInfoResource.ResolvePackage(packageIdentity, this._nuGetFramework, cacheContext, this._logger, CancellationToken.None);
				if (dependencyInfo != null)
				{
					DownloadResource downloadResource = await dependencyInfo.Source.GetResourceAsync<DownloadResource>(CancellationToken.None);

					using (DownloadResourceResult downloadResult = await downloadResource.GetDownloadResourceResultAsync(dependencyInfo, new PackageDownloadContext(cacheContext), this._globalPackagesFolder, NullLogger.Instance, CancellationToken.None))
					{
						PackageInstalledOperation installedArgs = new PackageInstalledOperation(packageIdentity, downloadResult);
						this.PackageInstalled?.Invoke(this, installedArgs);
					}

					//await PackageExtractor.ExtractPackageAsync(
					//	downloadResult.PackageSource,
					//	downloadResult.PackageStream,
					//	this._packagePathResolver,
					//	packageExtractionContext,
					//	CancellationToken.None);
					break;
				}
			}
		}

		private void TryRemovePackageFolder(PackageIdentity packageIdentity)
		{
			string path = this._packagePathResolver.GetInstallPath(packageIdentity);
			if (Directory.Exists(path)) Directory.Delete(path, true);
		}

		public ISet<PackageIdentity> GetInstalledPackages()
		{
			return this._packageConfig.Packages.ToHashSet();
		}

		private async Task UninstallPackageAsync(PackageIdentity packageIdentity, bool forceRemove, bool removeDependencies = false)
		{
			if (!forceRemove)
			{
				CanUninstallResult result = await this.CanUninstallPackage(packageIdentity, this._packageConfig);
				if (!result)
				{
					throw new Exception(
						$"Cannot uninstall {packageIdentity} because {result.DependentPackage} depends on it");
				}
			}

			this.TryRemovePackageFolder(packageIdentity);
			this._packageConfig.Remove(packageIdentity);
			this._packageConfig.Serialize();
		}

		private async Task<CanUninstallResult> CanUninstallPackageAsync(PackageIdentity packageIdentity)
		{
			return await this.CanUninstallPackage(packageIdentity, this._packageConfig);
		}

		private async Task<CanUninstallResult> CanUninstallPackage(PackageIdentity packageIdentity, PackageConfig packageConfig)
		{
			using (var cacheContext = new SourceCacheContext())
			{
				foreach (PackageIdentity installedPackage in packageConfig.Packages)
				{
					if (installedPackage.Equals(packageIdentity)) continue;

					HashSet<SourcePackageDependencyInfo> dependencies = await this.GetPackageDependencies(installedPackage, cacheContext);

					if (dependencies.Contains(packageIdentity))
						return new CanUninstallResult(packageIdentity);
				}
			}
			return CanUninstallResult.Succes;
		}

		public async Task<HashSet<SourcePackageDependencyInfo>> GetPackageDependencies(PackageIdentity package,
			SourceCacheContext cacheContext)
		{
			DependencyInfoResource[] dependencyInfoResources = this._repositories.Select(x => x.GetResource<DependencyInfoResource>()).ToArray();
			HashSet<SourcePackageDependencyInfo> availablePackages = new HashSet<SourcePackageDependencyInfo>(PackageIdentityComparer.Default);

			await this.GetPackageDependencies(package, this._nuGetFramework, cacheContext, this._logger, dependencyInfoResources,
				availablePackages);

			return availablePackages;
		}

		private async Task GetPackageDependencies(PackageIdentity package,
			NuGetFramework framework,
			SourceCacheContext cacheContext, ILogger logger, DependencyInfoResource[] dependencyInfoResources, HashSet<SourcePackageDependencyInfo> availablePackages)
		{
			if (availablePackages.Contains(package)) return;

			foreach (DependencyInfoResource dependencyInfoResource in dependencyInfoResources)
			{
				SourcePackageDependencyInfo dependencyInfo = await dependencyInfoResource.ResolvePackage(package, framework, cacheContext, logger, CancellationToken.None);
				if (dependencyInfo != null)
				{
					availablePackages.Add(dependencyInfo);

					foreach (PackageDependency dependency in dependencyInfo.Dependencies)
					{
						await this.GetPackageDependencies(
							new PackageIdentity(dependency.Id, dependency.VersionRange.MinVersion),
							framework, cacheContext, logger, dependencyInfoResources, availablePackages);
					}
				}
			}
		}
	}

	public struct CanUninstallResult
	{
		public static readonly CanUninstallResult Succes = new CanUninstallResult();
		public PackageIdentity DependentPackage;

		public CanUninstallResult(PackageIdentity dependentPackage)
		{
			this.DependentPackage = dependentPackage;
		}

		public static implicit operator bool(CanUninstallResult result) => result.DependentPackage == null;
	}

	public class PackageConfig
	{
		public HashSet<PackageIdentity> Packages { get; } = new HashSet<PackageIdentity>(PackageIdentityComparer.Default);

		public void Add(string id, string version) => this.Add(PackageIdentityParser.Parse(id, version));

		public void Add(PackageIdentity packageIdentity) => this.Packages.Add(packageIdentity);

		public void Remove(PackageIdentity packageIdentity) => this.Packages.Remove(packageIdentity);

		private readonly string _path;

		public PackageConfig(string path)
		{
			this._path = path;
			if (File.Exists(path))
			{
				XElement packagesXml = XElement.Load(path);

				foreach (XNode variable in packagesXml.Nodes())
				{
					if (variable is XElement xElement)
					{
						XAttribute idAttribute = xElement.Attribute("id");
						XAttribute versionAttribute = xElement.Attribute("version");

						PackageIdentity packageIdentity =
							PackageIdentityParser.Parse(idAttribute.Value, versionAttribute.Value);
						this.Packages.Add(packageIdentity);
					}
				}
			}
		}

		public void Serialize()
		{
			IEnumerable<XElement> packagesXml = this.Packages.Select(x => new XElement("package", new XAttribute("id", x.Id), new XAttribute("version", x.Version)));

			var rootXml = new XElement("packages", packagesXml);

			new XDocument(rootXml).Save(this._path);
		}
	}

	public static class PackageIdentityParser
	{
		public static PackageIdentity Parse(string id, string version)
		{
			NuGetVersion nugetVersion = NuGetVersion.Parse(version);
			return new PackageIdentity(id, nugetVersion);
		}

		public static PackageIdentity Parse(string id, Version version)
		{
			var nugetVersion = new NuGetVersion(version.ToString());
			return new PackageIdentity(id, nugetVersion);
		}

		public static PackageIdentity Parse(string fullname)
		{
			int dotIndex = fullname.Length;
			int dotCount = 0;
			while (true)
			{
				dotIndex = fullname.LastIndexOf('.', dotIndex - 1);
				if (dotIndex == -1) break;

				dotCount++;
				if (dotCount < 3) continue;

				string potentialVersionString = fullname.Substring(
					dotIndex + 1,
					fullname.Length - dotIndex - 1);
				NuGetVersion version;
				if (!NuGetVersion.TryParse(potentialVersionString, out version))
					continue;

				string packageName = fullname.Remove(dotIndex);
				return new PackageIdentity(packageName, version);
			}
			throw new Exception($"{fullname} is not a correct package identity");
		}
	}
}