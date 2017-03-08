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
			get
			{
				return autoExpandTimer.Interval;
			}

			set
			{
				autoExpandTimer.Interval = value;
			}
		}

		public AutoExpandTreeView(int autoExpandDelay = 1000)
		{
			autoExpandTimer.Interval = autoExpandDelay;
			autoExpandTimer.Tick += OnAutoExpandTimerTick;
		}

		private void StartAutoExpandTimer()
		{
			if (AutoExpandDelay > 0)
			{
				autoExpandTimer.Interval = AutoExpandDelay;
				autoExpandTimer.Start();
			}
		}

		private void StopAutoExpandTimer()
		{
			if (AutoExpandDelay > 0)
			{
				autoExpandNode.Expand();
			}

			autoExpandTimer.Stop();
		}

		private void OnAutoExpandTimerTick(object sender, EventArgs e)
		{
			StopAutoExpandTimer();
		}

		protected override void OnDragOver(DragEventArgs dragEvent)
		{
			base.OnDragOver(dragEvent);
			if (AutoExpandDelay > 0)
			{
				Point locationHoverPosition = PointToClient(new Point(dragEvent.X, dragEvent.Y));
				TreeNodeAdv hoveredNode = GetNodeAt(locationHoverPosition);
				TreeNodeAdv draggedNode = SelectedNode;

				// If we hover over ourselves, null or a cihld stop the timer.
				if (draggedNode == null || hoveredNode == null || draggedNode == hoveredNode || draggedNode.Children.Contains(hoveredNode))
				{
					autoExpandTimer.Stop();
					autoExpandNode = null;
					return;
				}

				// If we are different to the currently timing out node, then start it again.
				if (autoExpandNode != hoveredNode)
				{
					autoExpandNode = hoveredNode;
					StartAutoExpandTimer();
				}
			}
		}

		protected override void OnDragLeave(EventArgs dragLeaveEvent)
		{
			base.OnDragLeave(dragLeaveEvent);
			autoExpandTimer.Stop();
			autoExpandNode = null;
		}

		protected override void OnDragDrop(DragEventArgs dragDropEvent)
		{
			base.OnDragDrop(dragDropEvent);
			autoExpandTimer.Stop();
			autoExpandNode = null;
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			autoExpandTimer.Tick -= OnAutoExpandTimerTick;
		}
	}
}
