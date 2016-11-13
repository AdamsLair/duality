using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Duality.Editor.Plugins.LogView.Properties
{
	/// <summary>
	/// Since directly accessing code generated from .resx files will result in a deserialization on
	/// each Resource access, this class allows cached Resource access.
	/// </summary>
	public static class LogViewResCache
	{
		public static readonly Icon		IconLogView					= LogViewRes.IconLogView;
		public static readonly Bitmap	IconLogCore					= LogViewRes.IconLogCore;
		public static readonly Bitmap	IconLogEditor				= LogViewRes.IconLogEditor;
		public static readonly Bitmap	IconLogError				= LogViewRes.IconLogError;
		public static readonly Bitmap	IconLogGame					= LogViewRes.IconLogGame;
		public static readonly Bitmap	IconLogMessage				= LogViewRes.IconLogMessage;
		public static readonly Bitmap	IconLogWarning				= LogViewRes.IconLogWarning;
	}
}
