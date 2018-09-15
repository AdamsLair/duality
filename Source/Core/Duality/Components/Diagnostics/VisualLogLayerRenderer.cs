using System;
using System.Collections.Generic;
using System.Linq;

using Duality.Drawing;
using Duality.Editor;
using Duality.Resources;

namespace Duality.Components.Diagnostics
{
	[EditorHintFlags(MemberFlags.Invisible)]
	public class VisualLogLayerRenderer : Component, ICmpRenderer
	{
		private bool            overlay    = false;
		private List<VisualLog> targetLogs = null;
		private Canvas          canvas     = new Canvas();

		public bool Overlay
		{
			get { return this.overlay; }
			set { this.overlay = value; }
		}
		public List<VisualLog> TargetLogs
		{
			get { return this.targetLogs; }
			set { this.targetLogs = value; }
		}

		void ICmpRenderer.GetCullingInfo(out CullingInfo info)
		{
			info.Position = Vector3.Zero;
			info.Radius = float.MaxValue;
			info.Visibility = VisibilityFlag.AllGroups;
			if (this.overlay)
			{
				info.Visibility |= VisibilityFlag.ScreenOverlay;
			}
		}
		void ICmpRenderer.Draw(IDrawDevice device)
		{
			if (device.IsPicking) return;

			// Render a specific set of logs when defined, or the global logs otherwise
			IEnumerable<VisualLog> logs = 
				this.targetLogs ?? 
				VisualLogs.All;
			
			this.canvas.Begin(device);
			this.canvas.State.SetMaterial(DrawTechnique.Alpha);
			this.canvas.State.DepthOffset = this.overlay ? 0.0f : -1.0f;
			
			foreach (VisualLog log in logs)
			{
				if (!log.Visible) continue;
				if (log.BaseColor.A == 0) continue;
				if ((log.VisibilityGroup & device.VisibilityMask & VisibilityFlag.AllGroups) == VisibilityFlag.None) continue;

				this.canvas.State.SetMaterial(DrawTechnique.Alpha);
				this.canvas.State.ColorTint = log.BaseColor;
				foreach (VisualLogEntry logEntry in log.Entries)
				{
					bool isOverlayLog = logEntry.Anchor == VisualLogAnchor.Screen;
					if (isOverlayLog != this.overlay) continue;

					this.canvas.PushState();
					this.canvas.State.DepthOffset += logEntry.DepthOffset;
					if (logEntry.LifetimeAsAlpha)
					{
						this.canvas.State.ColorTint *= new ColorRgba(1.0f, logEntry.LifetimeRatio);
					}
					
					if (logEntry.Anchor == VisualLogAnchor.Object && logEntry.AnchorObj != null && logEntry.AnchorObj.Transform != null)
					{
						Transform anchorTransform = logEntry.AnchorObj.Transform;
						logEntry.Draw(this.canvas, anchorTransform.Pos, anchorTransform.Angle, anchorTransform.Scale);
					}
					else
					{
						logEntry.Draw(this.canvas);
					}

					this.canvas.PopState();
				}
			}

			this.canvas.End();
		}
	}
}
