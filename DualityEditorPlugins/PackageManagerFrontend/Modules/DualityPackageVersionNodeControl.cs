using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using Aga.Controls.Tree;
using Aga.Controls.Tree.NodeControls;

using Duality.Editor.PackageManagement;
using Duality.Editor.Plugins.PackageManagerFrontend.TreeModels;

namespace Duality.Editor.Plugins.PackageManagerFrontend
{
	public class DualityPackageVersionNodeControl : NodeControl
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
			bool highlightItemVersion = item is LocalPackageItem;
			bool isInstalled = false;
			bool isUpToDate = false;
			if (item != null)
			{
				isInstalled = item.InstalledPackageInfo != null;

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

			// Determine background color based on versioning
			Brush backgroundBrush = null;
			if (isInstalled)
			{
				if (newVersion != null)
				{
					if (itemVersion >= newVersion)
						backgroundBrush = new SolidBrush(Color.FromArgb(32, 160, 255, 0));
					else
						backgroundBrush = new SolidBrush(Color.FromArgb(32, 255, 160, 0));
				}
				else
				{
					backgroundBrush = new SolidBrush(Color.FromArgb(32, 128, 128, 128));
				}
			}

			// Calculate drawing layout and data
			StringFormat stringFormat = new StringFormat { Trimming = StringTrimming.EllipsisCharacter, Alignment = StringAlignment.Center, FormatFlags = StringFormatFlags.NoWrap };
			Rectangle currentVersionRect;
			Rectangle newestVersionRect;
			{
				SizeF currentVersionSize;
				SizeF newestVersionSize;
				// Base info
				{
					currentVersionSize = g.MeasureString(itemVersionText, context.Font, targetRect.Width, stringFormat);
					newestVersionSize = g.MeasureString(newVersionText, context.Font, targetRect.Width, stringFormat);
					currentVersionRect = new Rectangle(targetRect.X, targetRect.Y, targetRect.Width, (int)currentVersionSize.Height);
					newestVersionRect = new Rectangle(targetRect.X, targetRect.Y + currentVersionRect.Height, targetRect.Width, targetRect.Height - currentVersionRect.Height);
				}
				// Alignment info
				{
					Size totelContentSize = new Size(Math.Max(currentVersionRect.Width, newestVersionRect.Width), currentVersionRect.Height + (int)newestVersionSize.Height);
					Point alignAdjust = new Point(0, Math.Max((targetRect.Height - totelContentSize.Height) / 2, 0));
					currentVersionRect.X += alignAdjust.X;
					currentVersionRect.Y += alignAdjust.Y;
					newestVersionRect.X += alignAdjust.X;
					newestVersionRect.Y += alignAdjust.Y;
				}
			}

			// Draw background and version texts
			if (backgroundBrush != null)
			{
				g.FillRectangle(backgroundBrush, targetRect);
			}
			{
				bool bothVisible = !string.IsNullOrWhiteSpace(itemVersionText) && !string.IsNullOrWhiteSpace(newVersionText);
				highlightItemVersion = highlightItemVersion || !bothVisible;
				g.DrawString(itemVersionText, context.Font, new SolidBrush(Color.FromArgb((highlightItemVersion ? 255 : 128) / (context.Enabled ? 1 : 2), this.Parent.ForeColor)), currentVersionRect, stringFormat);
				g.DrawString(newVersionText, context.Font, new SolidBrush(Color.FromArgb((highlightItemVersion ? 128 : 255) / (context.Enabled ? 1 : 2), this.Parent.ForeColor)), newestVersionRect, stringFormat);
			}
		}
		public override Size MeasureSize(TreeNodeAdv node, DrawContext context)
		{
			return new Size(60, 48);
		}
	}
}
