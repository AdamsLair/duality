using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace Duality.Editor.Plugins.CamView.Properties
{
	/// <summary>
	/// Since directly accessing code generated from .resx files will result in a deserialization on
	/// each Resource access, this class allows cached Resource access.
	/// </summary>
	public static class CamViewResCache
	{
		public static readonly Bitmap   CursorImageArrowCreateCircle  = CamViewRes.CursorArrowCreateCircle;
		public static readonly Bitmap   CursorImageArrowCreateEdge    = CamViewRes.CursorArrowCreateEdge;
		public static readonly Bitmap   CursorImageArrowCreateLoop    = CamViewRes.CursorArrowCreateLoop;
		public static readonly Bitmap   CursorImageArrowCreatePolygon = CamViewRes.CursorArrowCreatePolygon;
		public static readonly Bitmap	CursorImageArrowEditVertices  = CamViewRes.CursorArrowEditVertices;
		public static readonly Bitmap   IconCmpCircleCollider         = CamViewRes.IconCmpCircleCollider;
		public static readonly Bitmap   IconCmpEdgeCollider           = CamViewRes.IconCmpEdgeCollider;
		public static readonly Bitmap   IconCmpLoopCollider           = CamViewRes.IconCmpLoopCollider;
		public static readonly Bitmap   IconCmpRectCollider           = CamViewRes.IconCmpRectCollider;
		public static readonly Bitmap   IconCmpEditVertices           = CamViewRes.IconCmpEditVertices;
		public static readonly Icon     IconEye                       = CamViewRes.IconEye;

		public static readonly Bitmap   ObjectVisibility              = Resources.ObjectVisibility;
		public static readonly Bitmap   ObjectVisibilityFiltered      = Resources.ObjectVisibilityFiltered;

		public static readonly Cursor   CursorArrowCreateCircle       = CursorHelper.CreateCursor(CursorImageArrowCreateCircle,  0, 0);
		public static readonly Cursor   CursorArrowCreateEdge         = CursorHelper.CreateCursor(CursorImageArrowCreateEdge,    0, 0);
		public static readonly Cursor   CursorArrowCreateLoop         = CursorHelper.CreateCursor(CursorImageArrowCreateLoop,    0, 0);
		public static readonly Cursor   CursorArrowCreatePolygon      = CursorHelper.CreateCursor(CursorImageArrowCreatePolygon, 0, 0);
		public static readonly Cursor   CursorArrowEditVertices		  = CursorHelper.CreateCursor(CursorImageArrowEditVertices, 0, 0);
	}
}
