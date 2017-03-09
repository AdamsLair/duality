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

		public AutoExpandTreeView(int autoExpandDelay = 1000)
		{
			this.autoExpandTimer.Interval = autoExpandDelay;
			this.autoExpandTimer.Tick += this.OnAutoExpandTimerTick;
		}

		private void StartAutoExpandTimer()
		{
			if (this.AutoExpandDelay > 0)
			{
				this.autoExpandTimer.Interval = this.AutoExpandDelay;
				this.autoExpandTimer.Start();
			}
		}
		private void StopAutoExpandTimer()
		{
			if (this.AutoExpandDelay > 0)
			{
				this.autoExpandNode.Expand();
			}

			this.autoExpandTimer.Stop();
		}

		private void OnAutoExpandTimerTick(object sender, EventArgs e)
		{
			this.StopAutoExpandTimer();
		}
		protected override void OnDragOver(DragEventArgs dragEvent)
		{
			base.OnDragOver(dragEvent);
			if (this.AutoExpandDelay > 0)
			{
				Point locationHoverPosition = PointToClient(new Point(dragEvent.X, dragEvent.Y));
				TreeNodeAdv hoveredNode = GetNodeAt(locationHoverPosition);
				TreeNodeAdv draggedNode = SelectedNode;

				// If we are null, hover over something null, hover over ourselves, hover over an unexpandable node or are the parent of the hovered node then clear ane exit.
				if (draggedNode == null || hoveredNode == null || draggedNode == hoveredNode || !hoveredNode.CanExpand || draggedNode.IsParentOf(hoveredNode))
				{
					this.autoExpandTimer.Stop();
					this.autoExpandNode = null;
					return;
				}

				// If we are different to the currently timing out node, then start it again.
				if (this.autoExpandNode != hoveredNode)
				{
					this.autoExpandNode = hoveredNode;
					this.StartAutoExpandTimer();
				}
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
