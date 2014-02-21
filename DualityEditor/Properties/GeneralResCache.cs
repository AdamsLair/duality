using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Duality.Editor.Properties
{
	/// <summary>
	/// Since directly accessing code generated from .resx files will result in a deserialization on
	/// each Resource access, this class allows cached Resource access.
	/// </summary>
	public static class GeneralResCache
	{
		public static readonly Bitmap	arrow_undo				= GeneralRes.arrow_undo;
		public static readonly Bitmap	arrow_redo				= GeneralRes.arrow_redo;
		public static readonly Bitmap	ColorWheel				= GeneralRes.ColorWheel;
		public static readonly Bitmap	CursorArrow				= GeneralRes.CursorArrow;
		public static readonly Bitmap	CursorArrowAction		= GeneralRes.CursorArrowAction;
		public static readonly Bitmap	CursorArrowActionMove	= GeneralRes.CursorArrowActionMove;
		public static readonly Bitmap	CursorArrowActionRotate	= GeneralRes.CursorArrowActionRotate;
		public static readonly Bitmap	CursorArrowActionScale	= GeneralRes.CursorArrowActionScale;
		public static readonly Bitmap	CursorHandGrab			= GeneralRes.CursorHandGrab;
		public static readonly Bitmap	CursorHandGrabbing		= GeneralRes.CursorHandGrabbing;
		public static readonly Bitmap	ImageAppCreate			= GeneralRes.ImageAppCreate;
		public static readonly Bitmap	ImageTemplateCurrent	= GeneralRes.ImageTemplateCurrent;
		public static readonly Bitmap	ImageTemplateEmpty		= GeneralRes.ImageTemplateEmpty;
		public static readonly Icon		IconCog					= GeneralRes.IconCog;
		public static readonly Icon		IconWorkingFolder		= GeneralRes.IconWorkingFolder;
	}
}
