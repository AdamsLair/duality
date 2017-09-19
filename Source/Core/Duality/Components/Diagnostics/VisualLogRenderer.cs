using System;
using System.Collections.Generic;
using System.Linq;

using Duality.Drawing;
using Duality.Editor;
using Duality.Resources;

namespace Duality.Components.Diagnostics
{
	[EditorHintFlags(MemberFlags.Invisible)]
	public class VisualLogRenderer : Component, ICmpRenderer, ICmpInitializable
	{
		private bool            overlay      = false;
		private List<VisualLog> targetLogs   = null;
		private CanvasBuffer    vertexBuffer = new CanvasBuffer();

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
			
			Canvas target = new Canvas(device, this.vertexBuffer);
			target.State.SetMaterial(new BatchInfo(DrawTechnique.Alpha, ColorRgba.White));
			target.State.DepthOffset = this.overlay ? 0.0f : -1.0f;
			
			foreach (VisualLog log in logs)
			{
				if (!log.Visible) continue;
				if (log.BaseColor.A == 0) continue;
				if ((log.VisibilityGroup & device.VisibilityMask & VisibilityFlag.AllGroups) == VisibilityFlag.None) continue;

				target.State.SetMaterial(new BatchInfo(DrawTechnique.Alpha, log.BaseColor));
				foreach (VisualLogEntry logEntry in log.Entries)
				{
					bool isOverlayLog = logEntry.Anchor == VisualLogAnchor.Screen;
					if (isOverlayLog != this.overlay) continue;

					target.PushState();
					target.State.DepthOffset += logEntry.DepthOffset;
					if (logEntry.LifetimeAsAlpha)
						target.State.ColorTint = new ColorRgba(1.0f, logEntry.LifetimeRatio);
					else
						target.State.ColorTint = ColorRgba.White;
					
					if (logEntry.Anchor == VisualLogAnchor.Object && logEntry.AnchorObj != null && logEntry.AnchorObj.Transform != null)
					{
						Transform anchorTransform = logEntry.AnchorObj.Transform;
						logEntry.Draw(target, anchorTransform.Pos, anchorTransform.Angle, anchorTransform.Scale);
					}
					else
					{
						logEntry.Draw(target);
					}

					target.PopState();
				}
			}
		}
		void ICmpInitializable.OnInit(InitContext context) {}
		void ICmpInitializable.OnShutdown(ShutdownContext context)
		{
			if (context == ShutdownContext.Deactivate || context == ShutdownContext.Saving)
			{
				this.GameObj.Dispose();
			}
		}
	}
}
