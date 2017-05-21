using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

using Duality.IO;

using NUnit.Framework;

namespace Duality.Editor.PackageManagement.Tests
{
	/// <summary>
	/// Describes a test case for a package installation test in <see cref="PackageManagerTest"/>.
	/// </summary>
	public class PackageInstallTestCase
	{
		private string testCaseName;
		private MockPackageSpec testInstallPackage;
		private List<MockPackageSpec> expectedPackages;
		private List<MockPackageSpec> expectedOtherPackages;
		private List<MockPackageSpec> expectedDualityPackages;
		private HashSet<MockPackageSpec> repositoryPackages;


		/// <summary>
		/// [GET] The package that is installed as part of the test.
		/// </summary>
		public MockPackageSpec InstallPackage
		{
			get { return this.testInstallPackage; }
		}
		/// <summary>
		/// [GET] A list of all packages that are expected to be installed after 
		/// the <see cref="InstallPackage"/> installation is completed.
		/// </summary>
		public IReadOnlyList<MockPackageSpec> ExpectedPackages
		{
			get { return this.expectedPackages; }
		}
		/// <summary>
		/// [GET] A list of all non-Duality packages that are expected to be installed after 
		/// the <see cref="InstallPackage"/> installation is completed. 
		/// This is a subset of <see cref="ExpectedPackages"/>.
		/// </summary>
		public IReadOnlyList<MockPackageSpec> ExpectedOtherPackages
		{
			get { return this.expectedOtherPackages; }
		}
		/// <summary>
		/// [GET] A list of all Duality packages that are expected to be installed after 
		/// the <see cref="InstallPackage"/> installation is completed.
		/// This is a subset of <see cref="ExpectedPackages"/>.
		/// </summary>
		public IReadOnlyList<MockPackageSpec> ExpectedDualityPackages
		{
			get { return this.expectedDualityPackages; }
		}
		/// <summary>
		/// [GET] Enumerates all packages that need to be available in the remote mock
		/// repository in order to perform this test.
		/// </summary>
		public IEnumerable<MockPackageSpec> RepositoryPackages
		{
			get { return this.repositoryPackages; }
		}


		public PackageInstallTestCase(string testCaseName, MockPackageSpec installPackage, IEnumerable<MockPackageSpec> expectedInstall)
		{
			this.testCaseName = testCaseName;
			this.testInstallPackage = installPackage;
			this.expectedPackages = expectedInstall.ToList();
			this.expectedDualityPackages = this.expectedPackages
				.Where(p => p.Tags.Contains(PackageManager.DualityTag))
				.ToList();
			this.expectedOtherPackages = this.expectedPackages
				.Where(p => !p.Tags.Contains(PackageManager.DualityTag))
				.ToList();

			this.repositoryPackages = new HashSet<MockPackageSpec>(this.expectedPackages);
			this.repositoryPackages.Add(this.testInstallPackage);
		}

		public override string ToString()
		{
			return this.testCaseName;
		}
	}
}
