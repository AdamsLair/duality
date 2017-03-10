using Aga.Controls.Tree;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Duality.Editor.Controls
{
	/// <summary>
	/// An extended TreeViewAdv.
	/// Will automatically expand nodes that are being dragged over with another node.
	/// A value of 0 in the constructor or setting the property AutoExpandDelay to 0 will disable this behaviour.
	/// </summary>
	public class AutoExpandTreeView : TreeViewAdv
	{
		private TreeNodeAdv autoExpandNode = null;
		private Timer autoExpandTimer = new Timer();


		public int AutoExpandDelay
		{
			get { return this.autoExpandTimer.Interval; }
			set { this.autoExpandTimer.Interval = value; }
		}


		public AutoExpandTreeView()
		{
			this.autoExpandTimer.Interval = 750;
			this.autoExpandTimer.Tick += this.OnAutoExpandTimerTick;
		}

		private void StartAutoExpandTimer()
		{
			this.autoExpandTimer.Start();
		}
		private void ExecuteAutoExpand()
		{
			if (this.autoExpandNode != null)
				this.autoExpandNode.Expand();
			this.autoExpandTimer.Stop();
		}

		private void OnAutoExpandTimerTick(object sender, EventArgs e)
		{
			this.ExecuteAutoExpand();
		}
		protected override void OnDragOver(DragEventArgs dragEvent)
		{
			base.OnDragOver(dragEvent);
			TreeNodeAdv hoveredNode = this.DropPosition.Node;
			TreeNodeAdv draggedNode = this.SelectedNode;

			// Only allow expanding if we're hovering an expandable node that isn't the dragged 
			// one or one if its parent nodes.
			bool triggerAutoExpand = 
				this.AutoExpandDelay > 0 &&
				this.DropPosition.Position == NodePosition.Inside &&
				draggedNode != null && 
				hoveredNode != null && 
				draggedNode != hoveredNode && 
				hoveredNode.CanExpand && 
				!hoveredNode.IsChildOf(draggedNode);
				
			// If we want to trigger auto-expand for a new node, (re)start the timer
			if (triggerAutoExpand && this.autoExpandNode != hoveredNode)
			{
				this.autoExpandNode = hoveredNode;
				this.StartAutoExpandTimer();
			}
			// Cancel any scheduled expand if auto-expand is no longer a valid option
			else if (!triggerAutoExpand && this.autoExpandNode != null)
			{
				this.autoExpandTimer.Stop();
				this.autoExpandNode = null;
			}
		}
		protected override void OnDragLeave(EventArgs dragLeaveEvent)
		{
			base.OnDragLeave(dragLeaveEvent);
			this.autoExpandTimer.Stop();
			this.autoExpandNode = null;
		}
		protected override void OnDragDrop(DragEventArgs dragDropEvent)
		{
			base.OnDragDrop(dragDropEvent);
			this.autoExpandTimer.Stop();
			this.autoExpandNode = null;
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			this.autoExpandTimer.Tick -= OnAutoExpandTimerTick;
		}
	}
}
