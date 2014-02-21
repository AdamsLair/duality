using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Duality.Editor.Plugins.ResourceHacker.Properties
{
	/// <summary>
	/// Since directly accessing code generated from .resx files will result in a deserialization on
	/// each Resource access, this class allows cached Resource access.
	/// </summary>
	public static class ResourceHackerResCache
	{
		public static readonly Bitmap	IconArray			= ResourceHackerRes.IconArray;
		public static readonly Bitmap	IconClass			= ResourceHackerRes.IconClass;
		public static readonly Bitmap	IconDelegate		= ResourceHackerRes.IconDelegate;
		public static readonly Bitmap	IconEvent			= ResourceHackerRes.IconEvent;
		public static readonly Bitmap	IconField			= ResourceHackerRes.IconField;
		public static readonly Bitmap	IconMethod			= ResourceHackerRes.IconMethod;
		public static readonly Bitmap	IconObject			= ResourceHackerRes.IconObject;
		public static readonly Bitmap	IconObjectRef		= ResourceHackerRes.IconObjectRef;
		public static readonly Bitmap	IconPrimitive		= ResourceHackerRes.IconPrimitive;
		public static readonly Bitmap	IconProperty		= ResourceHackerRes.IconProperty;
		public static readonly Bitmap	IconResourceHacker	= ResourceHackerRes.IconResourceHacker;
	}
}
