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
	/// Applies relative friction to RigidBodies.
	/// </summary>
	[Serializable]
	public sealed class FrictionJointInfo : JointInfo
	{
		private	Vector2	localAnchorA	= Vector2.Zero;
		private	Vector2	localAnchorB	= Vector2.Zero;
		private	float	maxForce		= 200.0f;
		private	float	maxTorque		= 100.0f;


		public override bool DualJoint
		{
			get { return true; }
		}
		/// <summary>
		/// [GET / SET] The first RigidBodys local anchor point.
		/// </summary>
		[EditorHintIncrement(1)]
		public Vector2 LocalAnchorA
		{
			get { return this.localAnchorA; }
			set { this.localAnchorA = value; this.UpdateJoint(); }
		}
		/// <summary>
		/// [GET / SET] The second RigidBodys local anchor point.
		/// </summary>
		[EditorHintIncrement(1)]
		public Vector2 LocalAnchorB
		{
			get { return this.localAnchorB; }
			set { this.localAnchorB = value; this.UpdateJoint(); }
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
			return bodyA != null && bodyB != null ? JointFactory.CreateFrictionJoint(Scene.PhysicsWorld, bodyA, bodyB, Vector2.Zero, Vector2.Zero) : null;
		}
		internal override void UpdateJoint()
		{
			base.UpdateJoint();
			if (this.joint == null) return;

			FrictionJoint j = this.joint as FrictionJoint;
			j.LocalAnchorA = GetFarseerPoint(this.BodyA, this.localAnchorA);
			j.LocalAnchorB = GetFarseerPoint(this.BodyB, this.localAnchorB);
			j.MaxForce = PhysicsConvert.ToPhysicalUnit(this.maxForce) / Time.SPFMult;
			j.MaxTorque = PhysicsConvert.ToPhysicalUnit(this.maxTorque) / Time.SPFMult;
		}

		protected override void CopyTo(JointInfo target)
		{
			base.CopyTo(target);
			FrictionJointInfo c = target as FrictionJointInfo;
			c.localAnchorA = this.localAnchorA;
			c.localAnchorB = this.localAnchorB;
			c.maxForce = this.maxForce;
			c.maxTorque = this.maxTorque;
		}
	}
}
