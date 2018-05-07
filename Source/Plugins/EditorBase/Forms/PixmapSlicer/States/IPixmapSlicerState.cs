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
	public interface IPixmapSlicerState
	{
		/// <summary>
		/// The bounds of the <see cref="PixmapSlicerForm"/> rendering area
		/// that this state can interact with
		/// </summary>
		Rectangle DisplayBounds { get; set; }
		Pixmap TargetPixmap { get; set; }
		Cursor Cursor { get; }
		int SelectedRectIndex { get; }
		List<ToolStripItem> StateControls { get; }

		/// <summary>
		/// A function that transforms client 
		/// mouse coordinates to display coordinates
		/// </summary>
		MouseTransformDelegate TransformMouseCoordinates { get; set; }
		/// <summary>
		/// A function that converts rectangles from display to atlas coordinates
		/// </summary>
		Func<Rect, Rect> GetAtlasRect { get; set; }
		/// <summary>
		/// A function that converts rectangles from atlas to display coordinates
		/// </summary>
		Func<Rect, Rect> GetDisplayRect { get; set; }

		/// <summary>
		/// Occurs whenever a property of <see cref="TargetPixmap"/>
		/// is changed by this state
		/// </summary>
		event EventHandler PixmapUpdated;
		event EventHandler CursorChanged;
		event EventHandler StateCancelled;
		event EventHandler SelectionChanged;
		event EventHandler<PixmapSlicerForm.PixmapSlicerStateEventArgs> StateChangeRequested;

		void ClearSelection();

		void OnMouseDown(MouseEventArgs e);
		void OnMouseUp(MouseEventArgs e);
		void OnMouseMove(MouseEventArgs e);
		void OnKeyUp(KeyEventArgs e);
		void OnPaint(PaintEventArgs e);
	}
}
