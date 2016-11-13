using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Duality.Editor.Plugins.ObjectInspector.Properties
{
	/// <summary>
	/// Since directly accessing code generated from .resx files will result in a deserialization on
	/// each Resource access, this class allows cached Resource access.
	/// </summary>
	public static class ObjectInspectorResCache
	{
		public static readonly Icon		IconObjView		= ObjectInspectorRes.IconObjView;
	}
}
