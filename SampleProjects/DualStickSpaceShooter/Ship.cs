using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;

using Duality;
using Duality.Editor;
using Duality.Components;
using Duality.Components.Physics;

namespace DualStickSpaceShooter
{
	[Serializable]
	[RequiredComponent(typeof(Transform))]
	[RequiredComponent(typeof(RigidBody))]
	public class Ship : Component, ICmpUpdatable
	{
		private	Vector2	targetThrust	= Vector2.Zero;
		private	float	targetAngle		= 0.0f;

		[EditorHintFlags(MemberFlags.Invisible)]
		public Vector2 TargetThrust
		{
			get { return this.targetThrust; }
			set { this.targetThrust = value; }
		}
		[EditorHintFlags(MemberFlags.Invisible)]
		public float TargetAngle
		{
			get { return this.targetAngle; }
			set { this.targetAngle = value; }
		}

		public virtual void OnUpdate()
		{
			Transform transform = this.GameObj.Transform;
			RigidBody body		= this.GameObj.RigidBody;

			// Turn to the desired fire angle
			float turnDirection		= MathF.TurnDir(transform.Angle, this.targetAngle);
			float turnLength		= MathF.CircularDist(transform.Angle, this.targetAngle);
			float turnVelocity		= MathF.Min(turnLength, MathF.RadAngle30) * 0.5f * turnDirection;
			body.AngularVelocity	= turnVelocity;

			// Apply force according to the desired thrust
			body.ApplyWorldForce(this.targetThrust * 0.5f);
		}
	}
}
