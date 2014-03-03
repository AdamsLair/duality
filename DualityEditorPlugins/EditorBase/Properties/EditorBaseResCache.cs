using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Duality.Editor.Plugins.Base.Properties
{
	/// <summary>
	/// Since directly accessing code generated from .resx files will result in a deserialization on
	/// each Resource access, this class allows cached Resource access.
	/// </summary>
	public static class EditorBaseResCache
	{
		public static readonly Bitmap	CursorArrowCreateCircle		= EditorBaseRes.CursorArrowCreateCircle;
		public static readonly Bitmap	CursorArrowCreateEdge		= EditorBaseRes.CursorArrowCreateEdge;
		public static readonly Bitmap	CursorArrowCreateLoop		= EditorBaseRes.CursorArrowCreateLoop;
		public static readonly Bitmap	CursorArrowCreatePolygon	= EditorBaseRes.CursorArrowCreatePolygon;
		public static readonly Bitmap	DropdownSettingsBlack		= EditorBaseRes.DropdownSettingsBlack;
		public static readonly Bitmap	IconCmpCircleCollider		= EditorBaseRes.IconCmpCircleCollider;
		public static readonly Bitmap	IconCmpEdgeCollider			= EditorBaseRes.IconCmpEdgeCollider;
		public static readonly Bitmap	IconCmpLoopCollider			= EditorBaseRes.IconCmpLoopCollider;
		public static readonly Bitmap	IconCmpRectCollider			= EditorBaseRes.IconCmpRectCollider;
		public static readonly Icon		IconEye						= EditorBaseRes.IconEye;
		public static readonly Icon		IconObjView					= EditorBaseRes.IconObjView;
		public static readonly Icon		IconProjectView				= EditorBaseRes.IconProjectView;
		public static readonly Icon		IconSceneView				= EditorBaseRes.IconSceneView;
		public static readonly Bitmap	IconSpeakerBlack			= EditorBaseRes.IconSpeakerBlack;
		public static readonly Bitmap	IconSpeakerWhite			= EditorBaseRes.IconSpeakerWhite;
		public static readonly Bitmap	OverlayLink					= EditorBaseRes.OverlayLink;
		public static readonly Bitmap	OverlayLinkBroken			= EditorBaseRes.OverlayLinkBroken;
		public static readonly Bitmap	IconAbortCross				= EditorBaseRes.IconAbortCross;
		public static readonly Bitmap	IconEyeCross				= EditorBaseRes.IconEyeCross;
		public static readonly Bitmap	IconLock					= EditorBaseRes.IconLock;
	}
}
