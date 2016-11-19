using System;

using Duality;
using Duality.Resources;
using Duality.Editor;
using Duality.Properties;
using Duality.Drawing;
using Duality.Plugins.DynamicLighting.Properties;

namespace Duality.Plugins.DynamicLighting
{
	[EditorHintCategory(CoreResNames.CategoryGraphics)]
	[EditorHintImage(DynLightResNames.IconResourceLightingTechnique)]
	public class LightingTechnique : DrawTechnique
	{
		public override bool NeedsPreparation
		{
			get { return true; }
		}

		public override void PrepareRendering(IDrawDevice device, BatchInfo material)
		{
			base.PrepareRendering(device, material);
			DynamicLighting.Light.SetupLighting(device, material);
		}
	}
}
