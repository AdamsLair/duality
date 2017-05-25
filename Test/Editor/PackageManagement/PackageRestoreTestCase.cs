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
	/// Describes a test case for a package restore / verify test in <see cref="PackageManagerTest"/>.
	/// </summary>
	public class PackageRestoreTestCase
	{
		private string name;
		private List<MockPackageSpec> preSetup;
		private List<PackageName> desiredSetup;
		private List<MockPackageSpec> installed;
		private List<MockPackageSpec> uninstalled;
		private List<MockPackageSpec> results;
		private List<MockPackageSpec> dualityResults;
		private List<MockPackageSpec> repository;

		
		/// <summary>
		/// [GET] A list of all packages that are prepared to be already present when 
		/// the restore operation is performed.
		/// </summary>
		public IReadOnlyList<MockPackageSpec> PreSetup
		{
			get { return this.preSetup; }
		}
		/// <summary>
		/// [GET] The desired package setup as specified in the config file.
		/// </summary>
		public IReadOnlyList<PackageName> DesiredSetup
		{
			get { return this.desiredSetup; }
		}
		/// <summary>
		/// [GET] A list of all packages that are expected to have been installed during the test.
		/// </summary>
		public IReadOnlyList<MockPackageSpec> Installed
		{
			get { return this.installed; }
		}
		/// <summary>
		/// [GET] A list of all packages that are expected to have been uninstalled during the test.
		/// </summary>
		public IReadOnlyList<MockPackageSpec> Uninstalled
		{
			get { return this.uninstalled; }
		}
		/// <summary>
		/// [GET] A list of all packages that are expected to be present after 
		/// the restore operation is completed.
		/// </summary>
		public IReadOnlyList<MockPackageSpec> Results
		{
			get { return this.results; }
		}
		/// <summary>
		/// [GET] A list of all Duality packages that are expected to be present after 
		/// the restore operation is completed.
		/// This is a subset of <see cref="Results"/>.
		/// </summary>
		public IReadOnlyList<MockPackageSpec> DualityResults
		{
			get { return this.dualityResults; }
		}
		/// <summary>
		/// [GET] Enumerates all packages that need to be available in the remote mock
		/// repository in order to perform this test.
		/// </summary>
		public IReadOnlyList<MockPackageSpec> Repository
		{
			get { return this.repository; }
		}


		public PackageRestoreTestCase(string name, IEnumerable<MockPackageSpec> repository, IEnumerable<MockPackageSpec> preSetup, IEnumerable<PackageName> desiredSetup, IEnumerable<MockPackageSpec> results)
			: this(name, repository, preSetup, desiredSetup, results, null, null) { }
		public PackageRestoreTestCase(string name, IEnumerable<MockPackageSpec> repository, IEnumerable<MockPackageSpec> preSetup, IEnumerable<PackageName> desiredSetup, IEnumerable<MockPackageSpec> results, IEnumerable<MockPackageSpec> installed, IEnumerable<MockPackageSpec> uninstalled)
		{
			this.name = name;
			this.preSetup = preSetup.ToList();
			this.desiredSetup = desiredSetup.ToList();
			this.results = results.ToList();
			this.installed = (installed ?? this.results.Except(this.preSetup)).ToList();
			this.uninstalled = (uninstalled ?? this.preSetup.Except(this.results)).ToList();
			this.dualityResults = this.results
				.Where(p => p.Tags.Contains(PackageManager.DualityTag))
				.ToList();

			this.repository = repository.ToList();
		}

		public override string ToString()
		{
			return this.name;
		}
	}
}
