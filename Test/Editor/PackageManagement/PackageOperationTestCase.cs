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
	/// Describes a test case for a package install / uninstall / update test in <see cref="PackageManagerTest"/>.
	/// </summary>
	public class PackageOperationTestCase
	{
		private string name;
		private List<MockPackageSpec> setup;
		private MockPackageSpec target;
		private List<MockPackageSpec> installed;
		private List<MockPackageSpec> uninstalled;
		private List<MockPackageSpec> results;
		private List<MockPackageSpec> dualityResults;
		private HashSet<MockPackageSpec> repository;

		
		/// <summary>
		/// [GET] A list of all packages that are prepared to be present when 
		/// the <see cref="Target"/> operation is performed.
		/// </summary>
		public IReadOnlyList<MockPackageSpec> Setup
		{
			get { return this.setup; }
		}
		/// <summary>
		/// [GET] The package on which the test operation will be performed.
		/// </summary>
		public MockPackageSpec Target
		{
			get { return this.target; }
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
		/// the <see cref="Target"/> operation is completed.
		/// </summary>
		public IReadOnlyList<MockPackageSpec> Results
		{
			get { return this.results; }
		}
		/// <summary>
		/// [GET] A list of all Duality packages that are expected to be present after 
		/// the <see cref="Target"/> operation is completed.
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
		public IEnumerable<MockPackageSpec> Repository
		{
			get { return this.repository; }
		}


		public PackageOperationTestCase(string name, MockPackageSpec target, IEnumerable<MockPackageSpec> results) : this(name, null, target, results) { }
		public PackageOperationTestCase(string name, IEnumerable<MockPackageSpec> setup, MockPackageSpec target, IEnumerable<MockPackageSpec> results)
		{
			this.name = name;
			this.target = target;
			this.setup = (setup ?? Enumerable.Empty<MockPackageSpec>()).ToList();
			this.results = (results ?? Enumerable.Empty<MockPackageSpec>()).ToList();
			this.installed = this.results.Except(this.setup).ToList();
			this.uninstalled = this.setup.Except(this.results).ToList();
			this.dualityResults = this.results
				.Where(p => p.Tags.Contains(PackageManager.DualityTag))
				.ToList();

			this.repository = new HashSet<MockPackageSpec>();
			this.repository.Add(this.target);
			foreach (MockPackageSpec packageSpec in this.results)
				this.repository.Add(packageSpec);
			foreach (MockPackageSpec packageSpec in this.setup)
				this.repository.Add(packageSpec);
		}

		public override string ToString()
		{
			return this.name;
		}
	}
}
