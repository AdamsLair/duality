using Aga.Controls.Tree;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Duality.Editor.Controls
{
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
				autoExpandTimer.Stop();
				autoExpandTimer.Interval = AutoExpandDelay;
				
				autoExpandTimer.Start();
			}
		}

		private void StopAutoExpandTimer()
		{
			if (AutoExpandDelay > 0)
			{
				if (autoExpandNode != null)
				{
					autoExpandNode.Expand();
				}
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

				if ((autoExpandNode == null || autoExpandNode != hoveredNode))
				{
					autoExpandNode = hoveredNode;
					StartAutoExpandTimer();
				}
			}

		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);

			autoExpandTimer.Tick -= OnAutoExpandTimerTick;
		}
	}
}
