using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Aga.Controls;
using Aga.Controls.Tree;
using Aga.Controls.Tree.NodeControls;

namespace Duality.Editor.Forms
{
	public partial class WelcomeDialog : Form
	{
		private class ActionNode : Node
		{
			private IEditorAction action = null;
			public IEditorAction Action
			{
				get { return this.action; }
			}
			public ActionNode(IEditorAction action) : base(action.Name)
			{
				this.action = action;
				this.Image = action.Icon;
			}
		}
		private class SummaryNodeControl : NodeControl
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
				ActionNode item = node.Tag as ActionNode;
				if (item == null) return;

				string headline = item.Action.Name;
				string summary = item.Action.Description;

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

		private	TreeModel actionModel = null;
		private TreeNodeAdv hoveredNode = null;

		public bool IsEmpty
		{
			get { return this.actionModel == null || this.actionModel.Nodes.Count == 0; }
		}

		public WelcomeDialog()
		{
			this.InitializeComponent();
			this.actionModel = new TreeModel();
			this.treeColumnMain.DrawColHeaderBg += this.treeColumnMain_DrawColHeaderBg;

			IEnumerable<IEditorAction> welcomeActions = 
				DualityEditorApp.GetEditorActions(
					typeof(object), 
					Enumerable.Empty<object>(), 
					DualityEditorApp.ActionContextFirstSession);
			foreach (IEditorAction action in welcomeActions)
			{
				this.actionModel.Nodes.Add(new ActionNode(action));
			}
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			this.actionList_Resize(this, EventArgs.Empty);
		}
		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);
			this.actionList.Model = this.actionModel;
		}

		private void buttonOk_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
			this.Close();
		}
		private void actionList_NodeMouseClick(object sender, TreeNodeAdvMouseEventArgs e)
		{
			ActionNode node = (e.Node != null) ? (e.Node.Tag as ActionNode) : null;
			if (node == null) return;

			node.Action.Perform(new object());
		}
		private void actionList_MouseLeave(object sender, EventArgs e)
		{
			this.hoveredNode = null;
			this.actionList.Cursor = Cursors.Default;
			this.actionList.Invalidate();
		}
		private void actionList_MouseMove(object sender, MouseEventArgs e)
		{
			this.hoveredNode = this.actionList.GetNodeAt(e.Location);
			this.actionList.Cursor = this.hoveredNode != null ? Cursors.Hand : Cursors.Default;
			this.actionList.Invalidate();
		}
		private void actionList_RowDraw(object sender, TreeViewRowDrawEventArgs e)
		{
			if (e.Node == this.hoveredNode)
			{
				e.Graphics.FillRectangle(
					new SolidBrush(Color.FromArgb(212, 212, 212)),
					0,
					e.RowRect.Y,
					this.actionList.ClientSize.Width,
					e.RowRect.Height);
			}
		}
		private void actionList_Resize(object sender, EventArgs e)
		{
			this.treeColumnMain.Width = this.actionList.ClientSize.Width - SystemInformation.VerticalScrollBarWidth - 5;
		}
		private void actionList_SelectionChanged(object sender, EventArgs e)
		{
			if (this.actionList.SelectedNode != null)
				this.actionList.ClearSelection();
		}
		private void treeColumnMain_DrawColHeaderBg(object sender, DrawColHeaderBgEventArgs e)
		{
			e.Graphics.FillRectangle(new SolidBrush(this.actionList.BackColor), e.Bounds);
			e.Handled = true;
		}
	}
}
