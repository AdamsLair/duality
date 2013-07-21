using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace EditorBase.PluginRes
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
		public static readonly Bitmap	IconCmpCamera				= EditorBaseRes.IconCmpCamera;
		public static readonly Bitmap	IconCmpCapsuleCollider		= EditorBaseRes.IconCmpCapsuleCollider;
		public static readonly Bitmap	IconCmpCircleCollider		= EditorBaseRes.IconCmpCircleCollider;
		public static readonly Bitmap	IconCmpEdgeCollider			= EditorBaseRes.IconCmpEdgeCollider;
		public static readonly Bitmap	IconCmpLoopCollider			= EditorBaseRes.IconCmpLoopCollider;
		public static readonly Bitmap	IconCmpRectCollider			= EditorBaseRes.IconCmpRectCollider;
		public static readonly Bitmap	IconCmpSoundListener		= EditorBaseRes.IconCmpSoundListener;
		public static readonly Bitmap	IconCmpSpriteRenderer		= EditorBaseRes.IconCmpSpriteRenderer;
		public static readonly Bitmap	IconCmpTransform			= EditorBaseRes.IconCmpTransform;
		public static readonly Bitmap	IconCmpUnknown				= EditorBaseRes.IconCmpUnknown;
		public static readonly Icon		IconEye						= EditorBaseRes.IconEye;
		public static readonly Bitmap	IconGameObj					= EditorBaseRes.IconGameObj;
		public static readonly Icon		IconLogView					= EditorBaseRes.IconLogView;
		public static readonly Icon		IconObjView					= EditorBaseRes.IconObjView;
		public static readonly Icon		IconProjectView				= EditorBaseRes.IconProjectView;
		public static readonly Bitmap	IconResAudioData			= EditorBaseRes.IconResAudioData;
		public static readonly Bitmap	IconResDrawTechnique		= EditorBaseRes.IconResDrawTechnique;
		public static readonly Bitmap	IconResFont					= EditorBaseRes.IconResFont;
		public static readonly Bitmap	IconResFragmentShader		= EditorBaseRes.IconResFragmentShader;
		public static readonly Bitmap	IconResMaterial				= EditorBaseRes.IconResMaterial;
		public static readonly Bitmap	IconResPixmap				= EditorBaseRes.IconResPixmap;
		public static readonly Bitmap	IconResPrefabEmpty			= EditorBaseRes.IconResPrefabEmpty;
		public static readonly Bitmap	IconResPrefabFull			= EditorBaseRes.IconResPrefabFull;
		public static readonly Bitmap	IconResRenderTarget			= EditorBaseRes.IconResRenderTarget;
		public static readonly Bitmap	IconResScene				= EditorBaseRes.IconResScene;
		public static readonly Bitmap	IconResShaderProgram		= EditorBaseRes.IconResShaderProgram;
		public static readonly Bitmap	IconResSound				= EditorBaseRes.IconResSound;
		public static readonly Bitmap	IconResTexture				= EditorBaseRes.IconResTexture;
		public static readonly Bitmap	IconResUnknown				= EditorBaseRes.IconResUnknown;
		public static readonly Bitmap	IconResVertexShader			= EditorBaseRes.IconResVertexShader;
		public static readonly Icon		IconSceneView				= EditorBaseRes.IconSceneView;
		public static readonly Bitmap	IconSpeakerBlack			= EditorBaseRes.IconSpeakerBlack;
		public static readonly Bitmap	IconSpeakerWhite			= EditorBaseRes.IconSpeakerWhite;
		public static readonly Bitmap	OverlayLink					= EditorBaseRes.OverlayLink;
		public static readonly Bitmap	OverlayLinkBroken			= EditorBaseRes.OverlayLinkBroken;
		public static readonly Bitmap	IconLogCore					= EditorBaseRes.IconLogCore;
		public static readonly Bitmap	IconLogEditor				= EditorBaseRes.IconLogEditor;
		public static readonly Bitmap	IconLogError				= EditorBaseRes.IconLogError;
		public static readonly Bitmap	IconLogGame					= EditorBaseRes.IconLogGame;
		public static readonly Bitmap	IconLogMessage				= EditorBaseRes.IconLogMessage;
		public static readonly Bitmap	IconLogWarning				= EditorBaseRes.IconLogWarning;
		public static readonly Bitmap	IconAbortCross				= EditorBaseRes.IconAbortCross;
		public static readonly Bitmap	IconEyeCross				= EditorBaseRes.IconEyeCross;
		public static readonly Bitmap	IconLock					= EditorBaseRes.IconLock;
	}
}
