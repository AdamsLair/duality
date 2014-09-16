using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Duality.Editor.Plugins.PackageManagerFrontend.Properties
{
	public static class PackageManagerFrontendResCache
	{
		public static readonly Icon IconPackage = PackageManagerFrontendRes.IconPackage;
		public static readonly Image IconPackageMedium = PackageManagerFrontendRes.IconPackageMedium;
		public static readonly Image IconPackageBig = Resources.packagebig;
		public static readonly Image IconUpToDate = PackageManagerFrontendRes.IconUpToDate;
		public static readonly Image IconSafeUpdate = PackageManagerFrontendRes.IconSafeUpdate;
		public static readonly Image IconLikelySafeUpdate = PackageManagerFrontendRes.IconLikelySafeUpdate;
		public static readonly Image IconLikelyUnsafeUpdate = PackageManagerFrontendRes.IconLikelyUnsafeUpdate;
		public static readonly Image IconIncompatibleUpdate = PackageManagerFrontendRes.IconIncompatibleUpdate;
	}
}
