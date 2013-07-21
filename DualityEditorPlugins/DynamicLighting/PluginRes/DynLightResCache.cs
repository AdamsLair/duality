using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace DynamicLighting.PluginRes
{
	/// <summary>
	/// Since directly accessing code generated from .resx files will result in a deserialization on
	/// each Resource access, this class allows cached Resource access.
	/// </summary>
	public static class DynLightResCache
	{
		public static readonly Bitmap	IconCmpLightingSpriteRenderer	= DynLightRes.IconCmpLightingSpriteRenderer;
		public static readonly Bitmap	IconLight						= DynLightRes.IconLight;
		public static readonly Bitmap	IconResLightingTechnique		= DynLightRes.IconResLightingTechnique;
	}
}
