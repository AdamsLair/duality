using Aga.Controls.Tree;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Duality.Editor.Controls
{
	public class AutoExpandTreeView : TreeViewAdv
	{
		public int DragOverExpandDelay
		{
			get;
			set;
		} = 1000;

		private TreeNodeAdv autoExpandNode = null;

		private Timer timer = new Timer();

		private void StartTimer()
		{
			if (DragOverExpandDelay > 0)
			{
				timer.Stop();
				timer.Interval = DragOverExpandDelay;
				timer.Tick += OnTimerTick;
				timer.Start();
			}
		}

		private void StopTimer()
		{
			if (DragOverExpandDelay > 0)
			{
				if (autoExpandNode != null)
				{
					autoExpandNode.Expand();
				}
			}

			timer.Stop();
		}

		private void OnTimerTick(object sender, EventArgs e)
		{
			StopTimer();
		}

		protected override void OnDragOver(DragEventArgs drgevent)
		{
			base.OnDragOver(drgevent);

			if (DragOverExpandDelay > 0)
			{

				Point p = PointToClient(new Point(drgevent.X, drgevent.Y));
				var node = GetNodeAt(p);

				if (autoExpandNode == null || autoExpandNode != node)
				{
					autoExpandNode = node;
					StartTimer();
				}
			}

		}
	}
}
