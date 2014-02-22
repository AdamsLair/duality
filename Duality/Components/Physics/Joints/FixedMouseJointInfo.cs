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
	/// Constrains the RigidBody to track a specific world point, such as a mouse cursor.
	/// </summary>
	[Serializable]
	public sealed class FixedMouseJointInfo : JointInfo
	{
		private	Vector2		localAnchor		= Vector2.Zero;
		private	Vector2		worldAnchor		= Vector2.Zero;
		private	float		dampingRatio	= 0.7f;
		private	float		frequency		= 5.0f;
		private	float		maxForce		= 10.0f;


		public override bool DualJoint
		{
			get { return false; }
		}
		/// <summary>
		/// [GET / SET] The RigidBodies local anchor point.
		/// </summary>
		[EditorHintIncrement(1)]
		public Vector2 LocalAnchor
		{
			get { return this.localAnchor; }
			set { this.localAnchor = value; this.UpdateJoint(); }
		}
		/// <summary>
		/// [GET / SET] The world anchor point to which the RigidBody will be attached.
		/// </summary>
		[EditorHintIncrement(1)]
		public Vector2 WorldAnchor
		{
			get { return this.worldAnchor; }
			set { this.worldAnchor = value; this.UpdateJoint(); }
		}
		/// <summary>
		/// [GET / SET] The damping ratio. Zero means "no damping", one means "critical damping".
		/// </summary>
		[EditorHintRange(0.0f, 1.0f)]
		public float DampingRatio
		{
			get { return this.dampingRatio; }
			set { this.dampingRatio = value; this.UpdateJoint(); }
		}
		/// <summary>
		/// [GET / SET] The mass spring damper frequency in hertz.
		/// </summary>
		[EditorHintRange(0.01f, 1000.0f)]
		public float Frequency
		{
			get { return this.frequency; }
			set { this.frequency = value; this.UpdateJoint(); }
		}
		/// <summary>
		/// [GET / SET] The maximum tracking force.
		/// </summary>
		public float MaxForce
		{
			get { return this.maxForce; }
			set { this.maxForce = value; this.UpdateJoint(); }
		}


		protected override Joint CreateJoint(Body bodyA, Body bodyB)
		{
			if (bodyA == null) return null;
			FixedMouseJoint mj = new FixedMouseJoint(bodyA, Vector2.Zero);
			Scene.PhysicsWorld.AddJoint(mj);
			return mj;
		}
		internal override void UpdateJoint()
		{
			base.UpdateJoint();
			if (this.joint == null) return;

			FixedMouseJoint j = this.joint as FixedMouseJoint;
			j.WorldAnchorB = PhysicsConvert.ToPhysicalUnit(this.worldAnchor);
			j.LocalAnchorA = GetFarseerPoint(this.BodyA, this.localAnchor);
			j.DampingRatio = this.dampingRatio;
			j.Frequency = this.frequency;
			j.MaxForce = PhysicsConvert.ToPhysicalUnit(this.maxForce) / Time.SPFMult;
		}

		protected override void CopyTo(JointInfo target)
		{
			base.CopyTo(target);
			FixedMouseJointInfo c = target as FixedMouseJointInfo;
			c.worldAnchor = this.worldAnchor;
			c.localAnchor = this.localAnchor;
			c.dampingRatio = this.dampingRatio;
			c.frequency = this.frequency;
			c.maxForce = this.maxForce;
		}
	}
}
