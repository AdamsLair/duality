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
		public static readonly Bitmap	DropdownSettingsBlack		= EditorBaseRes.DropdownSettingsBlack;
		public static readonly Icon		IconEye						= EditorBaseRes.IconEye;
		public static readonly Bitmap	IconReferenceInput			= EditorBaseRes.ReferenceInput;
		public static readonly Bitmap	IconSpeakerBlack			= EditorBaseRes.IconSpeakerBlack;
		public static readonly Bitmap	IconSpeakerWhite			= EditorBaseRes.IconSpeakerWhite;
		public static readonly Bitmap	IconAbortCross				= EditorBaseRes.IconAbortCross;
		public static readonly Bitmap	IconDownloadCodeIDE			= EditorBaseRes.IconDownloadCodeIDE;
		public static readonly Bitmap	IconCommunity				= EditorBaseRes.IconCommunity;
		public static readonly Bitmap	IconTutorial				= EditorBaseRes.IconTutorial;
		public static readonly Bitmap	IconAcceptCheck				= EditorBaseRes.IconAcceptCheck;
		public static readonly Bitmap	IconCancel					= EditorBaseRes.IconCancel;
		public static readonly Bitmap	IconHideIndices				= EditorBaseRes.IconHideIndices;
		public static readonly Bitmap	IconRevealIndices			= EditorBaseRes.IconRevealIndices;
		public static readonly Bitmap	IconShowIndices				= EditorBaseRes.IconShowIndices;
		public static readonly Bitmap	IconPixmapSlicer			= EditorBaseRes.IconPixmapSlicer;
		public static readonly Bitmap	IconSquareAdd				= EditorBaseRes.IconSquareAdd;
		public static readonly Bitmap	IconSquareDelete			= EditorBaseRes.IconSquareDelete;
		public static readonly Bitmap	IconSquareDeleteMany		= EditorBaseRes.IconSquareDeleteMany;
		public static readonly Bitmap	IconSquareNumbers			= EditorBaseRes.IconSquareNumbers;
		public static readonly Bitmap	IconViewBrightness			= EditorBaseRes.IconViewBrightness;
		public static readonly Bitmap	IconZoomDefault				= EditorBaseRes.IconZoomDefault;
		public static readonly Bitmap	IconZoomIn					= EditorBaseRes.IconZoomIn;
		public static readonly Bitmap	IconZoomOut					= EditorBaseRes.IconZoomOut;
	}
}
