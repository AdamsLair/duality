using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using Aga.Controls.Tree;
using Aga.Controls.Tree.NodeControls;

using Duality.Editor.PackageManagement;
using Duality.Editor.Plugins.PackageManagerFrontend.Properties;
using Duality.Editor.Plugins.PackageManagerFrontend.TreeModels;

namespace Duality.Editor.Plugins.PackageManagerFrontend
{
	public class DualityPackageVersionNodeControl : NodeControl, IToolTipProvider
	{
		private PackageManager manager = null;

		public PackageManager PackageManager
		{
			get { return this.manager; }
			set { this.manager = value; }
		}

		public override void Draw(TreeNodeAdv node, DrawContext context)
		{
			Graphics g = context.Graphics;
			Rectangle targetRect = new Rectangle(
				context.Bounds.X + this.LeftMargin,
				context.Bounds.Y,
				context.Bounds.Width - this.LeftMargin,
				context.Bounds.Height);

			// Retrieve item information
			PackageItem item = node.Tag as PackageItem;
			Version itemVersion = null;
			Version newVersion = null;
			PackageCompatibility updateCompatibility = PackageCompatibility.None;
			bool highlightItemVersion = item is LocalPackageItem;
			bool isInstalled = false;
			bool isUpToDate = false;
			if (item != null)
			{
				isInstalled = item.InstalledPackageInfo != null;
				updateCompatibility = item.UpdateCompatibility;

				if (item.InstalledPackageInfo != null)
					itemVersion = item.InstalledPackageInfo.Version;
				else if (item.ItemPackageInfo != null)
					itemVersion = item.ItemPackageInfo.Version;

				if (item.NewestPackageInfo != null)
					newVersion = item.NewestPackageInfo.Version;

				if (itemVersion != null && newVersion != null)
					isUpToDate = itemVersion >= newVersion;
			}
			string itemVersionText = PackageManager.GetDisplayedVersion(itemVersion);
			string newVersionText = isInstalled && !isUpToDate ? PackageManager.GetDisplayedVersion(newVersion) : string.Empty;

			// Determine background color and icon based on versioning
			Brush backgroundBrush = null;
			Image icon = null;
			if (isInstalled) backgroundBrush = new SolidBrush(Color.FromArgb(32, 128, 128, 128));
			if (isInstalled && newVersion != null && itemVersion != null)
			{
				if (newVersion <= itemVersion)
					icon = Properties.PackageManagerFrontendResCache.IconUpToDate;
				else if (updateCompatibility == PackageCompatibility.Definite)
					icon = Properties.PackageManagerFrontendResCache.IconSafeUpdate;
				else if (updateCompatibility == PackageCompatibility.Likely)
					icon = Properties.PackageManagerFrontendResCache.IconLikelySafeUpdate;
				else if (updateCompatibility == PackageCompatibility.Unlikely)
					icon = Properties.PackageManagerFrontendResCache.IconLikelyUnsafeUpdate;
				else
					icon = Properties.PackageManagerFrontendResCache.IconIncompatibleUpdate;
			}

			// Calculate drawing layout and data
			StringFormat stringFormat = new StringFormat { Trimming = StringTrimming.EllipsisCharacter, Alignment = StringAlignment.Near, FormatFlags = StringFormatFlags.NoWrap };
			Rectangle currentVersionRect;
			Rectangle newestVersionRect;
			Rectangle iconRect;
			{
				SizeF currentVersionSize;
				SizeF newestVersionSize;
				Size iconSize;
				// Base info
				{
					newestVersionSize = g.MeasureString(newVersionText, context.Font, targetRect.Width, stringFormat);
					currentVersionSize = g.MeasureString(itemVersionText, context.Font, targetRect.Width, stringFormat);
					iconSize = icon != null ? icon.Size : Size.Empty;
				}
				// Alignment info
				{
					Size totalTextSize = new Size(
						(int)Math.Max(currentVersionSize.Width, newestVersionSize.Width), 
						(int)(currentVersionSize.Height + newestVersionSize.Height));
					int leftSpacing = (targetRect.Width - totalTextSize.Width - iconSize.Width - 4) / 2;
					int iconIndent = iconSize.Width + 4 + leftSpacing;

					iconRect = new Rectangle(
						targetRect.X + leftSpacing,
						targetRect.Y + targetRect.Height / 2 - iconSize.Height / 2,
						iconSize.Width,
						iconSize.Height);
					newestVersionRect = new Rectangle(
						targetRect.X + iconIndent, 
						targetRect.Y + Math.Max((targetRect.Height - totalTextSize.Height) / 2, 0), 
						targetRect.Width - iconIndent, 
						(int)newestVersionSize.Height);
					currentVersionRect = new Rectangle(
						targetRect.X + iconIndent, 
						targetRect.Y + (int)newestVersionSize.Height + Math.Max((targetRect.Height - totalTextSize.Height) / 2, 0), 
						targetRect.Width - iconIndent, 
						targetRect.Height - (int)newestVersionSize.Height);
				}
			}

			// Draw background and version texts
			if (backgroundBrush != null)
			{
				g.FillRectangle(backgroundBrush, targetRect);
			}
			if (icon != null)
			{
				g.DrawImageUnscaledAndClipped(icon, iconRect);
			}
			{
				bool bothVisible = !string.IsNullOrWhiteSpace(itemVersionText) && !string.IsNullOrWhiteSpace(newVersionText);
				highlightItemVersion = highlightItemVersion || !bothVisible;
				g.DrawString(newVersionText, context.Font, new SolidBrush(Color.FromArgb((highlightItemVersion ? 128 : 255) / (context.Enabled ? 1 : 2), this.Parent.ForeColor)), newestVersionRect, stringFormat);
				g.DrawString(itemVersionText, context.Font, new SolidBrush(Color.FromArgb((highlightItemVersion ? 255 : 128) / (context.Enabled ? 1 : 2), this.Parent.ForeColor)), currentVersionRect, stringFormat);
			}
		}
		public override Size MeasureSize(TreeNodeAdv node, DrawContext context)
		{
			return new Size(60, 48);
		}

		string IToolTipProvider.GetToolTip(TreeNodeAdv node, NodeControl nodeControl)
		{
			PackageItem item = node.Tag as PackageItem;
			if (item == null) return null;

			if (item.IsUpdatable)
			{
				switch (item.UpdateCompatibility)
				{
					case PackageCompatibility.Definite:
						return Properties.PackageManagerFrontendRes.TooltipUpdateDefiniteCompatibility;
					case PackageCompatibility.Likely:
						return Properties.PackageManagerFrontendRes.TooltipUpdateLikelyCompatibility;
					case PackageCompatibility.Unlikely:
						return Properties.PackageManagerFrontendRes.TooltipUpdateUnlikelyCompatibility;
					case PackageCompatibility.None:
						return Properties.PackageManagerFrontendRes.TooltipUpdateNoCompatibility;
				}
			}

			return null;
		}
	}
}
