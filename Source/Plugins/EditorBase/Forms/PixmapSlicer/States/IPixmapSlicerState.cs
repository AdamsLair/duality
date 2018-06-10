using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Duality.Resources;

namespace Duality.Editor.Plugins.Base.Forms.PixmapSlicer.States
{
	public delegate void MouseTransformDelegate(Point mousePos, out float x, out float y);

	/// <summary>
	/// An operation state for the <see cref="PixmapSlicerForm"/>
	/// </summary>
	public interface IPixmapSlicerState : IHelpProvider
	{
		Pixmap TargetPixmap { get; set; }
		List<ToolStripItem> StateControls { get; }
		PixmapSlicingView View { get; set; }

		/// <summary>
		/// Occurs whenever a property of <see cref="TargetPixmap"/>
		/// is changed by this state
		/// </summary>
		event EventHandler StateCancelled;
		event EventHandler<PixmapSlicerStateEventArgs> StateChangeRequested;

		void OnStateEntered(EventArgs e);
		void OnStateLeaving(EventArgs e);
		void OnMouseDown(MouseEventArgs e);
		void OnMouseUp(MouseEventArgs e);
		void OnMouseMove(MouseEventArgs e);
		void OnKeyUp(KeyEventArgs e);
		void OnPaint(PaintEventArgs e);
	}
}
