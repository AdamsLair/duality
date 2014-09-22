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
	public class CameraController : Component, ICmpUpdatable
	{
		private	List<Transform>	followObjects	= null;
		private	float			softness		= 1.0f;
		private	float			zoomOutScale	= 1.0f;
		private	float			maxZoomOutDist	= 350.0f;

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
		public float ZoomOutScale
		{
			get { return this.zoomOutScale; }
			set { this.zoomOutScale = value; }
		}

		void ICmpUpdatable.OnUpdate()
		{
			if (this.followObjects == null) return;
			if (this.followObjects.Count == 0) return;

			Camera camera = this.GameObj.Camera;
			Transform transform = this.GameObj.Transform;

			const float ReferenceFocusDist = 500.0f;
			const float ReferenceScreenDiameter = 1000.0f;
			Vector2 screenSize = DualityApp.TargetResolution;
			camera.FocusDist = ReferenceFocusDist * screenSize.Length / ReferenceScreenDiameter;

			Vector3 focusPos = Vector3.Zero;
			foreach (Transform obj in this.followObjects)
			{
				focusPos += obj.Pos;
			}
			focusPos /= this.followObjects.Count;
			float maxDistFromCenter = 0.0f;
			foreach (Transform obj in this.followObjects)
			{
				maxDistFromCenter = MathF.Max((obj.Pos - focusPos).Length, maxDistFromCenter);
			}

			float zoomThreshold = 200.0f;
			float zoomOutDistance = this.zoomOutScale * MathF.Max(0, maxDistFromCenter - zoomThreshold);
			zoomOutDistance = MathF.Min(this.maxZoomOutDist, zoomOutDistance);
			Vector3 targetPos = focusPos - new Vector3(0.0f, 0.0f, ReferenceFocusDist + zoomOutDistance);
			transform.MoveByAbs((targetPos - transform.Pos) * MathF.Pow(10.0f, -this.softness) * Time.TimeMult);
		}
	}
}
