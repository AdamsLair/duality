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
	public class RigidCameraController : Component, ICmpInitializable, ICameraController
	{
		private GameObject targetObj = null;
		public GameObject TargetObject
		{
			get { return this.targetObj; }
			set { this.targetObj = value; }
		}

		void ICmpInitializable.OnActivate()
		{
			Camera camera = this.GameObj.GetComponent<Camera>();
			this.GameObj.Parent = this.targetObj;
			this.GameObj.Transform.LocalPos = new Vector3(0.0f, 0.0f, -camera.FocusDist);
		}
		void ICmpInitializable.OnDeactivate()
		{
			this.GameObj.Parent = null;
		}
	}
}
