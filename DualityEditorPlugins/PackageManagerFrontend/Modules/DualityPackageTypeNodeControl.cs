using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using Aga.Controls.Tree;
using Aga.Controls.Tree.NodeControls;

using Duality.Editor.Plugins.PackageManagerFrontend.TreeModels;
using Duality.Editor.Plugins.PackageManagerFrontend.Properties;

namespace Duality.Editor.Plugins.PackageManagerFrontend
{
	public class DualityPackageTypeNodeControl : NodeControl
	{
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
			Image typeIcon = null;
			if (item != null)
			{
				switch (item.Type)
				{
					case PackageItem.PackageType.Core:
						typeIcon = PackageManagerFrontendResCache.IconCore;
						break;
					case PackageItem.PackageType.Editor:
						typeIcon = PackageManagerFrontendResCache.IconEditor;
						break;
					case PackageItem.PackageType.Sample:
						typeIcon = PackageManagerFrontendResCache.IconSample;
						break;
				}
			}
			if (typeIcon == null) return;

			// Draw icon centered
			Point iconPos = new Point(
				targetRect.X + targetRect.Width / 2 - typeIcon.Width / 2,
				targetRect.Y + targetRect.Height / 2 - typeIcon.Height / 2);
			g.DrawImageUnscaled(typeIcon, iconPos.X, iconPos.Y);
		}
		public override Size MeasureSize(TreeNodeAdv node, DrawContext context)
		{
			return new Size(40, 20);
		}
	}
}
