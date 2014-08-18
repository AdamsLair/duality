using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Net;

using Duality;
using Duality.Editor.PackageManagement;
using Duality.Editor.Plugins.PackageManagerFrontend.Properties;

namespace Duality.Editor.Plugins.PackageManagerFrontend.TreeModels
{
	public abstract class BaseItem
	{
		private BaseItem parent = null;
		public BaseItem Parent
		{
			get { return parent; }
		}
		public abstract Image Icon { get; }
		public abstract string Title { get; }

		public BaseItem(BaseItem parent)
		{
			this.parent = parent;
		}

		public abstract void RetrieveOnlineData(PackageManager manager);
		protected Image RetrieveIcon(Uri iconUrl)
		{
			if (iconUrl == null) return null;

			try
			{
				HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(iconUrl);
				httpWebRequest.Timeout = 1000;
				using (HttpWebResponse httpWebReponse = (HttpWebResponse)httpWebRequest.GetResponse())
				{
					using (Stream stream = httpWebReponse.GetResponseStream())
					{
						Bitmap rawIcon = Bitmap.FromStream(stream) as Bitmap;
						return rawIcon.ScaleToFit(32, 32, System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic);
					}
				}
			}
			catch (Exception) {}

			return null;
		}
	}
	public abstract class PackageItem : BaseItem
	{
		private static Image defaultPackageIcon = null;
		public static Image DefaultPackageIcon
		{
			get
			{
				if (defaultPackageIcon == null)
				{
					defaultPackageIcon = (PackageManagerFrontendResCache.IconPackageMedium as Bitmap)
						.ScaleToFit(32, 32, 
						System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic);
				}
				return defaultPackageIcon;
			}
		}

		private PackageInfo	packageInfo			= null;
		private PackageInfo	newestPackageInfo	= null;
		private Image		icon				= DefaultPackageIcon;
		private	object		asyncDataLock		= new object();
		
		public PackageInfo PackageInfo
		{
			get { return this.packageInfo; }
		}
		public PackageInfo NewestPackageInfo
		{
			get
			{
				lock (this.asyncDataLock)
				{
					return this.newestPackageInfo;
				}
			}
		}
		public override Image Icon
		{
			get
			{
				lock (this.asyncDataLock)
				{
					return this.icon;
				}
			}
		}
		public override string Title
		{
			get
			{
				lock (this.asyncDataLock)
				{
					return this.packageInfo != null && !string.IsNullOrWhiteSpace(this.packageInfo.Title) ? this.packageInfo.Title : this.packageInfo.Id;
				}
			}
		}
		public Version Version
		{
			get { return this.packageInfo != null ? this.packageInfo.Version : null; }
		}
		public string DisplayedVersion
		{
			get
			{
				Version version = this.Version;
				if (version == null)
					return string.Empty;
				else if (version.Build == 0)
					return string.Format("{0}.{1}", version.Major, version.Minor);
				else
					return string.Format("{0}.{1}.{2}", version.Major, version.Minor, version.Build);
			}
		}
		public int? Downloads
		{
			get
			{
				lock (this.asyncDataLock)
				{
					if (this.newestPackageInfo == null)
						return null;
					else
						return this.newestPackageInfo.DownloadCount;
				}
			}
		}
		public string Id
		{
			get { return this.packageInfo != null ? this.packageInfo.Id : null; }
		}

		public PackageItem(PackageInfo packageInfo, BaseItem parent) : base(parent)
		{
			this.packageInfo = packageInfo;
		}
		public override void RetrieveOnlineData(PackageManager manager)
		{
			// Retrieve Icon
			Image newIcon = null;
			if (this.packageInfo != null) newIcon = this.RetrieveIcon(this.packageInfo.IconUrl);
			if (newIcon == null) newIcon = DefaultPackageIcon;

			// Retrieve info about newest online version
			PackageInfo newestPackage = manager.QueryPackageInfo(this.packageInfo.Id);

			// Apply data
			lock (this.asyncDataLock)
			{
				this.icon = newIcon;
				this.newestPackageInfo = newestPackage;
			}
		}
	}
	public class LocalPackageItem : PackageItem
	{
		private LocalPackage	package		= null;
		
		public LocalPackage Package
		{
			get { return this.package; }
		}

		public LocalPackageItem(LocalPackage package, BaseItem parent) : base(package.Info, parent)
		{
			this.package = package;
		}
	}
	public class OnlinePackageItem : PackageItem
	{
		public OnlinePackageItem(PackageInfo package, BaseItem parent) : base(package, parent) {}
	}
}
