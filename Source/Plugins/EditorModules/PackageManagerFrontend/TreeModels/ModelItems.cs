using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Net;

using Duality;
using Duality.IO;
using Duality.Editor.PackageManagement;
using Duality.Editor.Plugins.PackageManagerFrontend.Properties;

namespace Duality.Editor.Plugins.PackageManagerFrontend.TreeModels
{
	public abstract class BaseItem
	{
		private BaseItem parent = null;
		public BaseItem Parent
		{
			get { return this.parent; }
		}
		public abstract Image Icon { get; }
		public abstract string Title { get; }

		public BaseItem(BaseItem parent)
		{
			this.parent = parent;
		}

		public abstract void RetrieveAsyncData(PackageManager manager);
		public abstract void RetrieveIcon();
	}
	public abstract class PackageItem : BaseItem
	{
		private static string packageIconCacheDir = null;
		private static Image defaultPackageIcon = null;

		public static string PackageIconCacheDir
		{
			get
			{
				if (packageIconCacheDir == null)
				{
					packageIconCacheDir = Path.Combine(
						DualityEditorApp.PackageManager.LocalEnvironment.RepositoryPath, 
						"IconCache");
				}
				return packageIconCacheDir;
			}
		}
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

		public enum PackageType
		{
			Core,
			Editor,
			Sample,
			Other
		}

		protected	object					asyncDataLock		= new object();
		protected	PackageInfo				itemPackageInfo		= null;
		private		Image					icon				= DefaultPackageIcon;
		private		PackageCompatibility	compatibility		= PackageCompatibility.Unknown;
		
		public abstract PackageInfo InstalledPackageInfo { get; }
		public abstract PackageInfo NewestPackageInfo { get; }
		public PackageCompatibility Compatibility
		{
			get { return this.compatibility; }
		}
		public bool IsInstalled
		{
			get { return this.InstalledPackageInfo != null; }
		}
		public bool IsUpdatable
		{
			get { return this.IsInstalled && this.NewestPackageInfo != null && this.InstalledPackageInfo.Version < this.NewestPackageInfo.Version; }
		}
		public PackageInfo ItemPackageInfo
		{
			get { return this.itemPackageInfo; }
		}
		public PackageType Type
		{
			get
			{
				if (this.itemPackageInfo.IsSamplePackage)
					return PackageType.Sample;
				else if (this.itemPackageInfo.IsEditorPackage)
					return PackageType.Editor;
				else if (this.itemPackageInfo.IsCorePackage)
					return PackageType.Core;
				else
					return PackageType.Other;
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
					return this.itemPackageInfo != null && !string.IsNullOrWhiteSpace(this.itemPackageInfo.Title) ? this.itemPackageInfo.Title : this.itemPackageInfo.Id;
				}
			}
		}
		public Version Version
		{
			get { return this.itemPackageInfo != null ? this.itemPackageInfo.Version : null; }
		}
		public long? Downloads
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
		public override void RetrieveIcon()
		{
			PackageInfo info = this.itemPackageInfo;
			Image newIcon = null;

			if (info != null && info.IconUrl != null)
			{
				// Determine the local cache name of the icon.
				string cacheFileName = GetLocalUrlCacheFileName(info.IconUrl);
				string cacheFilePath = Path.Combine(PackageIconCacheDir, cacheFileName);

				// Attempt to load the icon from the local cache.
				if (newIcon == null)
				{
					try
					{
						if (File.Exists(cacheFilePath))
						{
							newIcon = Bitmap.FromFile(cacheFilePath) as Bitmap;
						}
					}
					catch (Exception) {}
				}

				// Download the image from the specified URL and save it.
				if (newIcon == null)
				{
					try
					{
						ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
						HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(info.IconUrl);
						httpWebRequest.Timeout = 3000;
						using (HttpWebResponse httpWebReponse = (HttpWebResponse)httpWebRequest.GetResponse())
						{
							using (Stream stream = httpWebReponse.GetResponseStream())
							{
								Bitmap rawIcon = Bitmap.FromStream(stream) as Bitmap;
								newIcon = rawIcon.ScaleToFit(32, 32, System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic);

								// Save the scaled image to the cache
								Directory.CreateDirectory(PackageIconCacheDir);
								newIcon.Save(cacheFilePath);
							}
						}
					}
					catch (Exception) {}
				}
			}

			// Fall back to the default icon
			newIcon = newIcon ?? DefaultPackageIcon;

			// Apply the icon image back to the model item
			lock (this.asyncDataLock)
			{
				this.icon = newIcon;
			}
		}
		protected void GetUpdateCompatibility(PackageManager manager)
		{
			PackageCompatibility compatibility = manager.GetCompatibilityLevel(this.NewestPackageInfo);
			lock (this.asyncDataLock)
			{
				this.compatibility = compatibility;
			}
		}

		private static string GetLocalUrlCacheFileName(Uri url)
		{
			string extension;
			try { extension = Path.GetExtension(url.AbsolutePath); }
			catch (Exception) { extension = ""; }

			string absoluteUri = url.AbsoluteUri;
			ulong hashedValue = 3074457345618258791ul;
			for(int i = 0; i < absoluteUri.Length; i++)
			{
				hashedValue += absoluteUri[i];
				hashedValue *= 3074457345618258799ul;
			}
			string hashedUrl = Convert.ToBase64String(BitConverter.GetBytes(hashedValue));
			return PathOp.GetValidFileName(hashedUrl) + extension;
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
			// Retrieve info about newest online version
			PackageInfo newestPackage = manager.GetPackage(this.itemPackageInfo.Name);
			lock (this.asyncDataLock)
			{
				this.newestPackageInfo = newestPackage;
			}
			this.GetUpdateCompatibility(manager);
		}

		public override string ToString()
		{
			return string.Format("LocalPackage {0} {1}, {2}", this.itemPackageInfo.Id, this.itemPackageInfo.Version, this.itemPackageInfo.PublishDate);
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
			this.UpdateLocalPackageData(manager);
			this.GetUpdateCompatibility(manager);
		}
		public void UpdateLocalPackageData(PackageManager manager)
		{
			lock (this.asyncDataLock)
			{
				LocalPackage installedPackage = manager.LocalSetup.GetPackage(this.itemPackageInfo.Id);
				this.installedPackageInfo = (installedPackage != null) ? installedPackage.Info : null;
			}
		}

		public override string ToString()
		{
			return string.Format("OnlinePackage {0} {1}, {2}", this.itemPackageInfo.Id, this.itemPackageInfo.Version, this.itemPackageInfo.PublishDate);
		}
	}
}
