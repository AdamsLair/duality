using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Duality.Editor.PackageManagement
{
	public class PackageLicenseAgreementEventArgs : PackageEventArgs
	{
		private	Uri		licenseUrl					= null;
		private bool	requireLicenseAcceptance	= false;
		private bool	isLicenseAccepted			= false;
		
		public Uri LicenseUrl
		{
			get { return this.licenseUrl; }
		}
		public bool RequireLicenseAcceptance
		{
			get { return this.requireLicenseAcceptance; }
		}
		public bool IsLicenseAccepted
		{
			get { return this.isLicenseAccepted; }
		}

		public PackageLicenseAgreementEventArgs(PackageName package, Uri licenseUrl, bool requireAccept) : base(package)
		{
			this.licenseUrl = licenseUrl;
			this.requireLicenseAcceptance = requireAccept;
		}

		/// <summary>
		/// Marks the license as accepted. Only ever call this when the end user has seen and explicitly accepted the license terms.
		/// </summary>
		public void AcceptLicense()
		{
			this.isLicenseAccepted = true;
		}
	}
}
