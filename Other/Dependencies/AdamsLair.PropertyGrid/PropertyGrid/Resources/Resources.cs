using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Reflection;

namespace AdamsLair.PropertyGrid.EmbeddedResources
{
	public static class Resources
	{
		private const string Folder = "CustomPropertyGrid.Resources.";
		public readonly	static Bitmap ImageAdd					= new Bitmap(typeof(Resources).Assembly.GetManifestResourceStream(Folder + "add.png"));
		public readonly	static Bitmap ImageDelete				= new Bitmap(typeof(Resources).Assembly.GetManifestResourceStream(Folder + "cross.png"));
		public readonly	static Bitmap NumberGripIcon			= new Bitmap(typeof(Resources).Assembly.GetManifestResourceStream(Folder + "NumberGripIcon.png"));
		public readonly	static Bitmap DropDownIcon				= new Bitmap(typeof(Resources).Assembly.GetManifestResourceStream(Folder + "DropDownIcon.png"));
		public readonly	static Bitmap ExpandNodeClosedDisabled	= new Bitmap(typeof(Resources).Assembly.GetManifestResourceStream(Folder + "ExpandNodeClosedDisabled.png"));
		public readonly	static Bitmap ExpandNodeClosedNormal	= new Bitmap(typeof(Resources).Assembly.GetManifestResourceStream(Folder + "ExpandNodeClosedNormal.png"));
		public readonly	static Bitmap ExpandNodeClosedHot		= new Bitmap(typeof(Resources).Assembly.GetManifestResourceStream(Folder + "ExpandNodeClosedHot.png"));
		public readonly	static Bitmap ExpandNodeClosedPressed	= new Bitmap(typeof(Resources).Assembly.GetManifestResourceStream(Folder + "ExpandNodeClosedPressed.png"));
		public readonly	static Bitmap ExpandNodeOpenedDisabled	= new Bitmap(typeof(Resources).Assembly.GetManifestResourceStream(Folder + "ExpandNodeOpenedDisabled.png"));
		public readonly	static Bitmap ExpandNodeOpenedNormal	= new Bitmap(typeof(Resources).Assembly.GetManifestResourceStream(Folder + "ExpandNodeOpenedNormal.png"));
		public readonly	static Bitmap ExpandNodeOpenedHot		= new Bitmap(typeof(Resources).Assembly.GetManifestResourceStream(Folder + "ExpandNodeOpenedHot.png"));
		public readonly	static Bitmap ExpandNodeOpenedPressed	= new Bitmap(typeof(Resources).Assembly.GetManifestResourceStream(Folder + "ExpandNodeOpenedPressed.png"));
		public readonly static Font DefaultFont = SystemFonts.DefaultFont;
		public readonly static Font DefaultFontSmall = new Font(DefaultFont.FontFamily, DefaultFont.Size * 0.8f, DefaultFont.Unit);
		public readonly static Font DefaultFontBold = new Font(DefaultFont, FontStyle.Bold);
		public readonly static Font DefaultFontBoldSmall = new Font(DefaultFontSmall, FontStyle.Bold);

		public readonly static string PropertyName_Default		= "Properties";
		public readonly static string PropertyGrid_N_Objects	= "{0} Objects";
	}
}
