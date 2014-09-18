using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using Aga.Controls.Tree;
using Aga.Controls.Tree.NodeControls;

using Duality.Editor.Plugins.PackageManagerFrontend.TreeModels;

namespace Duality.Editor.Plugins.PackageManagerFrontend
{
	public class DualityPackageSummaryNodeControl : NodeControl
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
			string headline = null;
			string summary = null;
			if (item != null)
			{
				headline = item.Title;
				if (item.ItemPackageInfo != null)
				{
					summary = item.ItemPackageInfo.Summary;
				}
			}

			// Calculate drawing layout and data
			StringFormat headlineFormat = new StringFormat { Trimming = StringTrimming.EllipsisCharacter, FormatFlags = StringFormatFlags.NoWrap };
			StringFormat summaryFormat = new StringFormat { Trimming = StringTrimming.EllipsisCharacter, FormatFlags = StringFormatFlags.LineLimit };
			Rectangle headlineRect;
			Rectangle summaryRect;
			{
				SizeF headlineSize;
				SizeF summarySize;
				// Base info
				{
					headlineSize = g.MeasureString(headline, context.Font, targetRect.Width, headlineFormat);
					headlineRect = new Rectangle(targetRect.X, targetRect.Y, targetRect.Width, (int)headlineSize.Height + 2);
					summaryRect = new Rectangle(targetRect.X, targetRect.Y + headlineRect.Height, targetRect.Width, targetRect.Height - headlineRect.Height);
					summarySize = g.MeasureString(summary, context.Font, summaryRect.Size, summaryFormat);
				}
				// Alignment info
				{
					Size totelContentSize = new Size(Math.Max(headlineRect.Width, summaryRect.Width), headlineRect.Height + (int)summarySize.Height);
					Point alignAdjust = new Point(0, Math.Max((targetRect.Height - totelContentSize.Height) / 2, 0));
					headlineRect.X += alignAdjust.X;
					headlineRect.Y += alignAdjust.Y;
					summaryRect.X += alignAdjust.X;
					summaryRect.Y += alignAdjust.Y;
				}
			}

			Color textColor = this.Parent.ForeColor;

			g.DrawString(headline, context.Font, new SolidBrush(Color.FromArgb(context.Enabled ? 255 : 128, textColor)), headlineRect, headlineFormat);
			g.DrawString(summary, context.Font, new SolidBrush(Color.FromArgb(context.Enabled ? 128 : 64, textColor)), summaryRect, summaryFormat);
		}
		public override Size MeasureSize(TreeNodeAdv node, DrawContext context)
		{
			return new Size(250, 48);
		}
	}
}
