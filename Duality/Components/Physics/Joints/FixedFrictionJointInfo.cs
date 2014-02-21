using System;

using OpenTK;

using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics.Joints;

using Duality.Editor;
using Duality.Resources;

namespace Duality.Components.Physics
{
	/// <summary>
	/// Applies friction to a specific point of the RigidBody.
	/// </summary>
	[Serializable]
	public sealed class FixedFrictionJointInfo : JointInfo
	{
		private	Vector2		localAnchor		= Vector2.Zero;
		private	float		maxForce		= 200.0f;
		private	float		maxTorque		= 100.0f;


		public override bool DualJoint
		{
			get { return false; }
		}
		/// <summary>
		/// [GET / SET] The RigidBodys local anchor point.
		/// </summary>
		[EditorHintIncrement(1)]
		public Vector2 LocalAnchor
		{
			get { return this.localAnchor; }
			set { this.localAnchor = value; this.UpdateJoint(); }
		}
		/// <summary>
		/// [GET / SET] The maximum friction force in the local anchor point.
		/// </summary>
		[EditorHintIncrement(1)]
		public float MaxForce
		{
			get { return this.maxForce; }
			set { this.maxForce = value; this.UpdateJoint(); }
		}
		/// <summary>
		/// [GET / SET] The maximum friction torque in the local anchor point.
		/// </summary>
		[EditorHintIncrement(1)]
		public float MaxTorque
		{
			get { return this.maxTorque; }
			set { this.maxTorque = value; this.UpdateJoint(); }
		}


		protected override Joint CreateJoint(Body bodyA, Body bodyB)
		{
			return bodyA != null ? JointFactory.CreateFixedFrictionJoint(Scene.PhysicsWorld, bodyA, Vector2.Zero) : null;
		}
		internal override void UpdateJoint()
		{
			base.UpdateJoint();
			if (this.joint == null) return;

			FixedFrictionJoint j = this.joint as FixedFrictionJoint;
			j.LocalAnchorA = GetFarseerPoint(this.BodyA, this.localAnchor);
			j.MaxForce = PhysicsConvert.ToPhysicalUnit(this.maxForce) / Time.SPFMult;
			j.MaxTorque = PhysicsConvert.ToPhysicalUnit(this.maxTorque) / Time.SPFMult;
		}

		protected override void CopyTo(JointInfo target)
		{
			base.CopyTo(target);
			FixedFrictionJointInfo c = target as FixedFrictionJointInfo;
			c.localAnchor = this.localAnchor;
			c.maxForce = this.maxForce;
			c.maxTorque = this.maxTorque;
		}
	}
}
