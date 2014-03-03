using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Duality.Editor.Plugins.CamView.Properties
{
	/// <summary>
	/// Since directly accessing code generated from .resx files will result in a deserialization on
	/// each Resource access, this class allows cached Resource access.
	/// </summary>
	public static class CamViewResCache
	{
		public static readonly Bitmap	CursorArrowCreateCircle		= CamViewRes.CursorArrowCreateCircle;
		public static readonly Bitmap	CursorArrowCreateEdge		= CamViewRes.CursorArrowCreateEdge;
		public static readonly Bitmap	CursorArrowCreateLoop		= CamViewRes.CursorArrowCreateLoop;
		public static readonly Bitmap	CursorArrowCreatePolygon	= CamViewRes.CursorArrowCreatePolygon;
		public static readonly Bitmap	IconCmpCircleCollider		= CamViewRes.IconCmpCircleCollider;
		public static readonly Bitmap	IconCmpEdgeCollider			= CamViewRes.IconCmpEdgeCollider;
		public static readonly Bitmap	IconCmpLoopCollider			= CamViewRes.IconCmpLoopCollider;
		public static readonly Bitmap	IconCmpRectCollider			= CamViewRes.IconCmpRectCollider;
		public static readonly Icon		IconEye						= CamViewRes.IconEye;
	}
}
