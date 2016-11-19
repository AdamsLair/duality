using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynamicLighting.Properties
{
	/// <summary>
	/// This static class contains constant string representations of certain resource names.
	/// </summary>
	public static class DynLightResNames
	{
		private const string ManifestBaseName	= "DynamicLighting.EmbeddedResources.";

		public const string IconComponentLightingSpriteRenderer	= ManifestBaseName + "iconCmpLightingSpriteRenderer.png";
		public const string IconResourceLightingTechnique		= ManifestBaseName + "iconResLightingTechnique.png";
		public const string IconResourceLight					= ManifestBaseName + "lightbulb.png";
	}
}
