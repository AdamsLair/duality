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

		public abstract void RetrieveAsyncData(PackageManager manager);
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

		protected	object		asyncDataLock		= new object();
		protected	PackageInfo	itemPackageInfo		= null;
		private		Image		icon				= DefaultPackageIcon;
		
		public abstract PackageInfo InstalledPackageInfo { get; }
		public abstract PackageInfo NewestPackageInfo { get; }
		public PackageInfo ItemPackageInfo
		{
			get { return this.itemPackageInfo; }
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
					return this.itemPackageInfo != null && !string.IsNullOrWhiteSpace(this.itemPackageInfo.Title) ? this.itemPackageInfo.Title : this.itemPackageInfo.Id;
				}
			}
		}
		public Version Version
		{
			get { return this.itemPackageInfo != null ? this.itemPackageInfo.Version : null; }
		}
		public int? Downloads
		{
			get
			{
				lock (this.asyncDataLock)
				{
					if (this.NewestPackageInfo == null)
						return null;
					else
						return this.NewestPackageInfo.DownloadCount;
				}
			}
		}
		public string Id
		{
			get { return this.itemPackageInfo != null ? this.itemPackageInfo.Id : null; }
		}

		public PackageItem(PackageInfo packageInfo, BaseItem parent) : base(parent)
		{
			this.itemPackageInfo = packageInfo;
		}
		public override void RetrieveAsyncData(PackageManager manager)
		{
			// Retrieve Icon
			Image newIcon = null;
			if (this.itemPackageInfo != null) newIcon = this.RetrieveIcon(this.itemPackageInfo.IconUrl);
			if (newIcon == null) newIcon = DefaultPackageIcon;
			lock (this.asyncDataLock)
			{
				this.icon = newIcon;
			}
		}
	}
	public class LocalPackageItem : PackageItem
	{
		private	PackageInfo	newestPackageInfo	= null;
		
		public override PackageInfo InstalledPackageInfo
		{
			get { return this.itemPackageInfo; }
		}
		public override PackageInfo NewestPackageInfo
		{
			get { return this.newestPackageInfo; }
		}

		public LocalPackageItem(LocalPackage package, BaseItem parent) : base(package.Info, parent) {}
		public override void RetrieveAsyncData(PackageManager manager)
		{
			base.RetrieveAsyncData(manager);

			// Retrieve info about newest online version
			PackageInfo newestPackage = manager.QueryPackageInfo(this.itemPackageInfo.Id);
			lock (this.asyncDataLock)
			{
				this.newestPackageInfo = newestPackage;
			}
		}
	}
	public class OnlinePackageItem : PackageItem
	{
		private	PackageInfo	installedPackageInfo	= null;
		
		public override PackageInfo InstalledPackageInfo
		{
			get { return this.installedPackageInfo; }
		}
		public override PackageInfo NewestPackageInfo
		{
			get { return this.itemPackageInfo; }
		}

		public OnlinePackageItem(PackageInfo package, BaseItem parent) : base(package, parent) {}
		public override void RetrieveAsyncData(PackageManager manager)
		{
			base.RetrieveAsyncData(manager);
			this.UpdateLocalPackageData(manager);
		}
		public void UpdateLocalPackageData(PackageManager manager)
		{
			lock (this.asyncDataLock)
			{
				LocalPackage installedPackage = manager.LocalPackages.FirstOrDefault(p => p.Id == this.itemPackageInfo.Id);
				this.installedPackageInfo = (installedPackage != null) ? installedPackage.Info : null;
			}
		}
	}
}
