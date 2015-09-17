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
	public class DualityPackageDateNodeControl : NodeControl
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
			if (item == null) return;

			DateTime date = item.ItemPackageInfo.PublishDate;
			bool isOld = (DateTime.Now - date).TotalDays > 180;
			string yearText = date.ToString("yyyy");
			string dayText = date.ToString("MMM dd");

			// Determine layout and drawing info
			SizeF yearTextSize = g.MeasureString(yearText, context.Font);
			SizeF dayTextSize = g.MeasureString(dayText, context.Font);
			Size totalTextSize = new Size(
				(int)Math.Max(yearTextSize.Width, dayTextSize.Width), 
				(int)(yearTextSize.Height + dayTextSize.Height));
			Rectangle yearTextRect = new Rectangle(
				targetRect.X, 
				targetRect.Y + Math.Max((targetRect.Height - totalTextSize.Height) / 2, 0), 
				targetRect.Width, 
				(int)yearTextSize.Height);
			Rectangle dayTextRect = new Rectangle(
				targetRect.X, 
				targetRect.Y + (int)yearTextSize.Height + Math.Max((targetRect.Height - totalTextSize.Height) / 2, 0), 
				targetRect.Width, 
				targetRect.Height - (int)yearTextSize.Height);
			StringFormat stringFormat = new StringFormat
			{
				Trimming = StringTrimming.EllipsisCharacter, 
				Alignment = StringAlignment.Center, 
				FormatFlags = StringFormatFlags.NoWrap
			};

			// Draw date text
			g.DrawString(yearText, context.Font, new SolidBrush(Color.FromArgb((isOld ? 128 : 255) / (context.Enabled ? 1 : 2), this.Parent.ForeColor)), yearTextRect, stringFormat);
			g.DrawString(dayText, context.Font, new SolidBrush(Color.FromArgb((isOld ? 128 : 255) / (context.Enabled ? 1 : 2), this.Parent.ForeColor)), dayTextRect, stringFormat);
		}
		public override Size MeasureSize(TreeNodeAdv node, DrawContext context)
		{
			return new Size(60, 48);
		}
	}
}
