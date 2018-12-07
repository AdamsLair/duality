using Duality;
using Duality.Components;
using Duality.Components.Renderers;
using Duality.Drawing;
using Duality.Resources;
using System;

namespace BasicMenu
{
	[RequiredComponent(typeof(TextRenderer))]
	public class VolumeRenderer : Component, ICmpUpdatable
	{
		[DontSerialize]
		private TextRenderer volumeText;

		void ICmpUpdatable.OnUpdate()
		{
			if (this.volumeText == null)
			{
				// still don't have it.. grab the TextRenderer
				this.volumeText = this.GameObj.GetComponent<TextRenderer>();
			}

			// update the volume value
			volumeText.Text.SourceText = String.Format("Volume {0:0.0}", DualityApp.UserData.SoundMasterVol);
		}
	}
}