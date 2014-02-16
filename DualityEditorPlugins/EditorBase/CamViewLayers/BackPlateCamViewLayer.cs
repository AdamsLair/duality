using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

using Duality;
using Duality.ColorFormat;
using Duality.Resources;
using Duality.Components.Physics;

using DualityEditor;
using DualityEditor.Forms;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace EditorBase.CamViewLayers
{
	public class BackPlateCamViewLayer : CamViewLayer
	{
		public override string LayerName
		{
			get { return PluginRes.EditorBaseRes.CamViewLayer_BackPlate_Name; }
		}
		public override string LayerDesc
		{
			get { return PluginRes.EditorBaseRes.CamViewLayer_BackPlate_Desc; }
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
