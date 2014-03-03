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
		public static readonly Bitmap	IconSpeakerBlack			= EditorBaseRes.IconSpeakerBlack;
		public static readonly Bitmap	IconSpeakerWhite			= EditorBaseRes.IconSpeakerWhite;
		public static readonly Bitmap	IconAbortCross				= EditorBaseRes.IconAbortCross;
	}
}
