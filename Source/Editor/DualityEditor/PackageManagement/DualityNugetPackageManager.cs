using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using NuGet;
using NuGet.Resources;

namespace Duality.Editor.PackageManagement
{
	/// <summary>
	/// Unfortunately nuget does not provide the ability to use a targetframework out of the box.
	/// This is the sole reason this class exists as it adds this missing functionality solves.
	/// Due to some shortcomings in the nuget API it had to use some less desirable code such as hiding a method.
	/// </summary>
	public class NuGetTargetedPackageManager : NuGet.PackageManager
	{
		private readonly FrameworkName _targetFrameWork;

		public NuGetTargetedPackageManager(FrameworkName targetFrameWork, IPackageRepository sourceRepository, string path) : base(sourceRepository, path)
		{
			this._targetFrameWork = targetFrameWork;
		}

		/// <summary>
		/// The original InstallPackage method does not use a targetframework. This one does.
		/// </summary>
		/// <param name="newPackage"></param>
		/// <param name="updateDependencies"></param>
		/// <param name="allowPrereleaseVersions"></param>
		public override void InstallPackage(IPackage package, bool ignoreDependencies, bool allowPrereleaseVersions)
		{
			this.InstallPackage(package, this._targetFrameWork, ignoreDependencies, allowPrereleaseVersions, false);
		}

		/// <summary>
		/// The original UpdatePackage method does not use a targetframework. This one does.
		/// </summary>
		/// <param name="newPackage"></param>
		/// <param name="updateDependencies"></param>
		/// <param name="allowPrereleaseVersions"></param>
		public new void UpdatePackage(IPackage newPackage, bool updateDependencies, bool allowPrereleaseVersions)
		{
			this.Execute(
				newPackage,
				new UpdateWalker(this.LocalRepository, this.SourceRepository,
				new DependentsWalker(this.LocalRepository, this._targetFrameWork),
				NullConstraintProvider.Instance,
				this._targetFrameWork,
				this.Logger,
				updateDependencies,
				allowPrereleaseVersions));
		}

		/// <summary>
		/// Decompiled from Nuget.Core. This method was private and we needed it for <see cref="UpdatePackage"/>.
		/// </summary>
		/// <param name="package"></param>
		/// <param name="resolver"></param>
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