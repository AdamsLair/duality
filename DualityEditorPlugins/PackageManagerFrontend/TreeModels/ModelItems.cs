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
					defaultPackageIcon = (PackageManagerFrontendResCache.IconPackageBig as Bitmap)
						.ScaleToFit(32, 32, 
						System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic);
				}
				return defaultPackageIcon;
			}
		}

		private PackageInfo	packageInfo			= null;
		private PackageInfo	newestPackageInfo	= null;
		private Image		icon				= DefaultPackageIcon;
		
		public PackageInfo PackageInfo
		{
			get { return this.packageInfo; }
		}
		public PackageInfo NewestPackageInfo
		{
			get { return this.newestPackageInfo; }
		}
		public override Image Icon
		{
			get { return this.icon; }
		}
		public override string Title
		{
			get { return !string.IsNullOrWhiteSpace(this.packageInfo.Title) ? this.packageInfo.Title : this.packageInfo.Id; }
		}
		public Version Version
		{
			get { return this.packageInfo.Version; }
		}
		public int Downloads
		{
			get
			{
				if (this.newestPackageInfo == null)
					return this.packageInfo.DownloadCount;
				else
					return this.newestPackageInfo.DownloadCount;
			}
		}
		public string Id
		{
			get { return this.packageInfo.Id; }
		}

		public PackageItem(PackageInfo packageInfo, BaseItem parent) : base(parent)
		{
			this.packageInfo = packageInfo;
		}
		public override void RetrieveOnlineData(PackageManager manager)
		{
			// Retrieve Icon
			{
				this.icon = null;
				if (this.packageInfo != null) this.icon = this.RetrieveIcon(this.packageInfo.IconUrl);
				if (this.icon == null) this.icon = DefaultPackageIcon;
			}

			// Retrieve info about newest online version
			this.newestPackageInfo = manager.QueryPackageInfo(this.packageInfo.Id);
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
