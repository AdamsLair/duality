using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

using Duality;
using Duality.Drawing;
using Duality.Resources;
using Duality.Components.Physics;

using Duality.Editor;
using Duality.Editor.Forms;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Duality.Editor.Plugins.CamView.CamViewLayers
{
	public class BackPlateCamViewLayer : CamViewLayer
	{
		public override string LayerName
		{
			get { return Properties.CamViewRes.CamViewLayer_BackPlate_Name; }
		}
		public override string LayerDesc
		{
			get { return Properties.CamViewRes.CamViewLayer_BackPlate_Desc; }
		}
		public override int Priority
		{
			get { return base.Priority - 100; }
		}

		protected internal override void OnCollectBackgroundDrawcalls(Canvas canvas)
		{
			base.OnCollectBackgroundDrawcalls(canvas);
			canvas.State.SetMaterial(new BatchInfo(DrawTechnique.Alpha, this.BgColor.WithAlpha(0.75f)));
			canvas.FillRect(0, 0, canvas.DrawDevice.TargetSize.X, canvas.DrawDevice.TargetSize.Y);
		}
	}
}
