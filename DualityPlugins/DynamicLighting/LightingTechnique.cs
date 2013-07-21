using System;
using OpenTK;
using Duality;
using Duality.Resources;

namespace DynamicLighting
{
	[Serializable]
	public class LightingTechnique : DrawTechnique
	{
		public override bool NeedsPreparation
		{
			get { return true; }
		}

		protected override void PrepareRendering(IDrawDevice device, BatchInfo material)
		{
			base.PrepareRendering(device, material);

			Vector3 camPos = device.RefCoord;
			float camRefDist = MathF.Abs(device.FocusDist);

			// Don't pass RefDist, see note in Light.cs
			material.SetUniform("_camRefDist", camRefDist);
			material.SetUniform("_camWorldPos", camPos.X, camPos.Y, camPos.Z);

			DynamicLighting.Light.SetupLighting(device, material);
		}
	}
}
