using System;
using System.Collections.Generic;
using System.Linq;

using Duality;
using Duality.Input;
using Duality.Components;
using Duality.Drawing;

namespace CameraController
{
	[RequiredComponent(typeof(Camera))]
	[RequiredComponent(typeof(Transform))]
	public class SmoothPositionThresholdCameraController : Component, ICmpUpdatable, ICameraController
	{
		private float smoothness = 1.0f;
		private float thresholdDist = 150.0f;
		private GameObject targetObj = null;

		/// <summary>
		/// [GET / SET] How smooth the camera should follow its target.
		/// </summary>
		public float Smoothness
		{
			get { return this.smoothness; }
			set { this.smoothness = value; }
		}
		/// <summary>
		/// [GET / SET] The distance threshold that needs to be exceeded before the camera starts to move.
		/// </summary>
		public float ThresholdDist
		{
			get { return this.thresholdDist; }
			set { this.thresholdDist = value; }
		}
		public GameObject TargetObject
		{
			get { return this.targetObj; }
			set { this.targetObj = value; }
		}

		void ICmpUpdatable.OnUpdate()
		{
			Transform transform = this.GameObj.Transform;
			Camera camera = this.GameObj.GetComponent<Camera>();

			// The position to focus on.
			Vector3 focusPos = this.targetObj.Transform.Pos;
			// The position where the camera itself should move
			Vector3 targetPos = focusPos - new Vector3(0.0f, 0.0f, camera.FocusDist);
			// A relative movement vector that would place the camera directly at its target position.
			Vector3 posDiff = (targetPos - transform.Pos);
			// Add a threshold to the position difference, before it gets noticed by the camera
			{
				float posDiffLength = posDiff.Length;
				Vector3 posDiffDir = posDiff / MathF.Max(posDiffLength, 0.01f);
				posDiffLength = MathF.Max(0.0f, posDiffLength - this.thresholdDist);
				posDiff = posDiffDir * posDiffLength;
			}
			// A relative movement vector that doesn't go all the way, but just a bit towards its target.
			Vector3 targetVelocity = posDiff * 0.1f * MathF.Pow(2.0f, -this.smoothness);

			// Move the camera
			transform.MoveBy(targetVelocity * Time.TimeMult);
		}
	}
}
