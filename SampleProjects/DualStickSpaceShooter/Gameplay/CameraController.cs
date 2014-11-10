using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;
using OpenTK.Input;

using Duality;
using Duality.Editor;
using Duality.Components;
using Duality.Resources;

namespace DualStickSpaceShooter
{
	[Serializable]
	[RequiredComponent(typeof(Camera))]
	public class CameraController : Component, ICmpUpdatable, ICmpInitializable
	{
		private const float ReferenceFocusDist		= 500.0f;
		private const float ReferenceScreenDiameter	= 1500.0f;

		private Transform		microphone			= null;
		private List<Transform>	followObjects		= null;
		private float			zoomFactor			= 1.0f;
		private float			softness			= 1.0f;
		private float			zoomOutScale		= 1.0f;
		private float			maxZoomOutDist		= 350.0f;
		private float			screenShakeStrength	= 0.0f;
		private Vector2			screenShakeOffset	= Vector2.Zero;

		public List<Transform> FollowObjects
		{
			get { return this.followObjects; }
			set { this.followObjects = value; }
		}
		[EditorHintRange(0.0f, 2.0f)]
		public float Softness
		{
			get { return this.softness; }
			set { this.softness = value; }
		}
		public float MaxZoomOutDist
		{
			get { return this.maxZoomOutDist; }
			set { this.maxZoomOutDist = value; }
		}
		[EditorHintRange(0.1f, 2.0f)]
		public float ZoomFactor
		{
			get { return this.zoomFactor; }
			set { this.zoomFactor = value; }
		}
		public float ZoomOutScale
		{
			get { return this.zoomOutScale; }
			set { this.zoomOutScale = value; }
		}
		public Transform Microphone
		{
			get { return this.microphone; }
			set { this.microphone = value; }
		}

		public void ShakeScreen(float strength)
		{
			this.screenShakeStrength += strength;
		}

		private void AdjustToScreenSize()
		{
			Vector2 screenSize = DualityApp.TargetResolution;
			Camera camera = this.GameObj.Camera;
			camera.FocusDist = ReferenceFocusDist * screenSize.Length * this.zoomFactor / ReferenceScreenDiameter;
		}
		private Vector3 GetTargetOffset(float maxDistFromCenter)
		{
			float zoomThreshold = 200.0f;
			float zoomOutDistance = this.zoomOutScale * MathF.Max(0, maxDistFromCenter - zoomThreshold);
			zoomOutDistance = MathF.Min(this.maxZoomOutDist, zoomOutDistance);
			return -new Vector3(0.0f, 0.0f, ReferenceFocusDist + zoomOutDistance);
		}

		void ICmpUpdatable.OnUpdate()
		{
			if (this.followObjects == null) return;
			if (this.followObjects.Count == 0) return;

			Camera camera = this.GameObj.Camera;
			Transform transform = this.GameObj.Transform;

			// Update screen shake behavior
			Vector2 lastScreenShakeOffset = this.screenShakeOffset;
			this.screenShakeStrength *= MathF.Pow(0.9f, Time.TimeMult);
			if (this.screenShakeStrength <= 0.1f) this.screenShakeStrength = 0.0f;
			if (this.screenShakeOffset != Vector2.Zero)
			{
				float oldAngle = this.screenShakeOffset.Angle;
				this.screenShakeOffset = Vector2.FromAngleLength(
					oldAngle + MathF.Rnd.NextFloat(MathF.DegToRad(120.0f), MathF.DegToRad(240.0f)), 
					this.screenShakeStrength);
			}
			else
			{
				this.screenShakeOffset = MathF.Rnd.NextVector2(this.screenShakeStrength);
			}

			// Remove old screen shake offset
			transform.Pos -= new Vector3(lastScreenShakeOffset);

			// Let the camera follow its objects.
			Transform[] activeFollowObjects = this.followObjects.Where(obj => obj.Active).ToArray();
			if (activeFollowObjects.Length > 0)
			{
				// Determine the position to focus on. It's the average of all follow object positions.
				Vector3 focusPos = Vector3.Zero;
				foreach (Transform obj in activeFollowObjects)
				{
					focusPos += obj.Pos;
				}
				focusPos /= activeFollowObjects.Length;

				// Determine how far these objects are away from each other
				float maxDistFromCenter = 0.0f;
				foreach (Transform obj in activeFollowObjects)
				{
					maxDistFromCenter = MathF.Max((obj.Pos - focusPos).Length, maxDistFromCenter);
				}

				// Move the camera so it can most likely see all of the required objects
				Vector3 targetPos = focusPos + this.GetTargetOffset(maxDistFromCenter);
				transform.MoveByAbs((targetPos - transform.Pos) * MathF.Pow(10.0f, -this.softness) * Time.TimeMult);
			}

			// Apply new screen shake offset
			transform.Pos += new Vector3(this.screenShakeOffset);

			// Make sure the microphone is always at the Z-0 plane
			if (this.microphone != null)
			{
				this.microphone.Pos = new Vector3(this.microphone.Pos.Xy, 0.0f);
			}
		}
		void ICmpInitializable.OnInit(Component.InitContext context)
		{
			if (context == InitContext.Activate)
			{
				this.AdjustToScreenSize();

				// Move near initial spawn point
				Transform transform = this.GameObj.Transform;
				transform.MoveToAbs(SpawnPoint.SpawnPos + this.GetTargetOffset(0.0f));
			}
		}
		void ICmpInitializable.OnShutdown(Component.ShutdownContext context) {}
	}
}
