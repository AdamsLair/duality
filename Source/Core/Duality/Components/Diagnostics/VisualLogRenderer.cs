using System;
using System.Collections.Generic;
using System.Linq;

using Duality.Drawing;
using Duality.Editor;
using Duality.Resources;

namespace Duality.Components.Diagnostics
{
	[EditorHintFlags(MemberFlags.Invisible)]
	public class VisualLogRenderer : Component, ICmpInitializable
	{
		private List<VisualLog> targetLogs = null;
		private VisualLogLayerRenderer worldLayer = null;
		private VisualLogLayerRenderer overlayLayer = null;

		public List<VisualLog> TargetLogs
		{
			get { return this.targetLogs; }
			set
			{
				this.targetLogs = value;
				if (this.worldLayer != null) this.worldLayer.TargetLogs = this.targetLogs;
				if (this.overlayLayer != null) this.overlayLayer.TargetLogs = this.targetLogs;
			}
		}
		
		void ICmpInitializable.OnInit(InitContext context)
		{
			if (context == InitContext.Activate)
			{
				GameObject worldRendererObj = new GameObject("World", this.GameObj);
				GameObject overlayRendererObj = new GameObject("Overlay", this.GameObj);
				this.worldLayer = worldRendererObj.AddComponent<VisualLogLayerRenderer>();
				this.overlayLayer = overlayRendererObj.AddComponent<VisualLogLayerRenderer>();
				this.worldLayer.Overlay = false;
				this.worldLayer.TargetLogs = this.targetLogs;
				this.overlayLayer.Overlay = true;
				this.overlayLayer.TargetLogs = this.targetLogs;
			}
		}
		void ICmpInitializable.OnShutdown(ShutdownContext context)
		{
			if (context == ShutdownContext.Deactivate || context == ShutdownContext.Saving)
			{
				this.GameObj.Dispose();
			}
		}
	}
}
