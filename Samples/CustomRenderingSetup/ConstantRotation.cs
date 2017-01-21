using System;
using System.Collections.Generic;
using System.Linq;

using Duality;
using Duality.Components;

namespace CustomRenderingSetup
{
	[RequiredComponent(typeof(Transform))]
	public class ConstantRotation : Component, ICmpUpdatable
	{
		private float rotationSpeed;

		public float RotationSpeed
		{
			get { return this.rotationSpeed; }
			set { this.rotationSpeed = value; }
		}

		void ICmpUpdatable.OnUpdate()
		{
			this.GameObj.Transform.TurnBy(Time.DeltaTime * this.rotationSpeed);
		}
	}
}