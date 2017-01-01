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
		private List<VisualLog> targetLogs         = null;
		private CanvasBuffer    vertexBufferScreen = new CanvasBuffer();
		private CanvasBuffer    vertexBufferWorld  = new CanvasBuffer();

		public List<VisualLog> TargetLogs
		{
			get { return this.targetLogs; }
			set { this.targetLogs = value; }
		}
		float ICmpRenderer.BoundRadius
		{
			get { return 0.0f; }
		}

		bool ICmpRenderer.IsVisible(IDrawDevice device)
		{
			return 
				!device.IsPicking && 
				(device.VisibilityMask & VisibilityFlag.AllGroups) != VisibilityFlag.None;
		}
		void ICmpRenderer.Draw(IDrawDevice device)
		{
			// Render a specific set of logs when defined, or the global logs otherwise
			IEnumerable<VisualLog> logs = 
				this.targetLogs ?? 
				VisualLogs.All;

			if (device.VisibilityMask.HasFlag(VisibilityFlag.ScreenOverlay))
			{
				Canvas target = new Canvas(device, this.vertexBufferScreen);
				target.State.SetMaterial(new BatchInfo(DrawTechnique.Alpha, ColorRgba.White));
				foreach (VisualLog log in logs)
				{
					if (!log.Visible) continue;
					if (log.BaseColor.A == 0) continue;
					if ((log.VisibilityGroup & device.VisibilityMask & VisibilityFlag.AllGroups) == VisibilityFlag.None) continue;

					target.State.SetMaterial(new BatchInfo(DrawTechnique.Alpha, log.BaseColor));
					foreach (VisualLogEntry logEntry in log.Entries)
					{
						if (logEntry.Anchor != VisualLogAnchor.Screen) continue;
						target.PushState();
						if (logEntry.LifetimeAsAlpha)
							target.State.ColorTint = new ColorRgba(1.0f, logEntry.LifetimeRatio);
						else
							target.State.ColorTint = ColorRgba.White;
						logEntry.Draw(target);
						target.PopState();
					}
				}
			}
			else
			{
				Canvas target = new Canvas(device, this.vertexBufferWorld);
				target.State.SetMaterial(new BatchInfo(DrawTechnique.Alpha, ColorRgba.White));
				target.State.ZOffset = -1;
				foreach (VisualLog log in logs)
				{
					if (!log.Visible) continue;
					if (log.BaseColor.A == 0) continue;
					if ((log.VisibilityGroup & device.VisibilityMask & VisibilityFlag.AllGroups) == VisibilityFlag.None) continue;

					target.State.SetMaterial(new BatchInfo(DrawTechnique.Alpha, log.BaseColor));
					foreach (VisualLogEntry logEntry in log.Entries)
					{
						if (logEntry.Anchor == VisualLogAnchor.Screen) continue;
						target.PushState();
						target.State.ZOffset += logEntry.DepthOffset;
						target.State.ColorTint = new ColorRgba(1.0f, logEntry.LifetimeRatio);
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
