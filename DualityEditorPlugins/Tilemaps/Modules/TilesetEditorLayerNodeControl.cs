using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using Aga.Controls.Tree;
using Aga.Controls.Tree.NodeControls;

namespace Duality.Editor.Plugins.Tilemaps
{
	public class TilesetEditorLayerNodeControl : NodeControl
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
			TilesetEditorLayerNode item = node.Tag as TilesetEditorLayerNode;
			if (item == null) return;

			string headline = item.Title;
			string summary = item.Description;

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
				return new Size(100, 48);
			}
	}
}
