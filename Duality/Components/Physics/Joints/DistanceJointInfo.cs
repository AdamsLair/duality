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
	/// Constrains two RigidBodies to obtain a fixed distance to each other
	/// </summary>
	[Serializable]
	public sealed class DistanceJointInfo : JointInfo
	{
		private	Vector2		localAnchorA	= Vector2.Zero;
		private	Vector2		localAnchorB	= Vector2.Zero;
		private	float		dampingRatio	= 0.5f;
		private	float		frequency		= 1.0f;
		private	float		length			= 200.0f;


		public override bool DualJoint
		{
			get { return true; }
		}
		/// <summary>
		/// [GET / SET] The first bodies local anchor point.
		/// </summary>
		[EditorHintIncrement(1)]
		public Vector2 LocalAnchorA
		{
			get { return this.localAnchorA; }
			set { this.localAnchorA = value; this.UpdateJoint(); }
		}
		/// <summary>
		/// [GET / SET] The second bodies local anchor point.
		/// </summary>
		[EditorHintIncrement(1)]
		public Vector2 LocalAnchorB
		{
			get { return this.localAnchorB; }
			set { this.localAnchorB = value; this.UpdateJoint(); }
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
		/// [GET / SET] The target distance between local and world anchor
		/// </summary>
		[EditorHintIncrement(1)]
		[EditorHintRange(0.0f, 10000000.0f)]
		public float TargetDistance
		{
			get { return this.length; }
			set { this.length = value; this.UpdateJoint(); }
		}


		protected override Joint CreateJoint(Body bodyA, Body bodyB)
		{
			return bodyA != null && bodyB != null ? JointFactory.CreateDistanceJoint(Scene.PhysicsWorld, bodyA, bodyB, Vector2.Zero, Vector2.Zero) : null;
		}
		internal override void UpdateJoint()
		{
			base.UpdateJoint();
			if (this.joint == null) return;

			DistanceJoint j = this.joint as DistanceJoint;
			j.LocalAnchorB = GetFarseerPoint(this.BodyB, this.localAnchorB);
			j.LocalAnchorA = GetFarseerPoint(this.BodyA, this.localAnchorA);
			j.DampingRatio = this.dampingRatio;
			j.Frequency = this.frequency;
			j.Length = PhysicsConvert.ToPhysicalUnit(this.length);
		}

		protected override void CopyTo(JointInfo target)
		{
			base.CopyTo(target);
			DistanceJointInfo c = target as DistanceJointInfo;
			c.localAnchorB = this.localAnchorB;
			c.localAnchorA = this.localAnchorA;
			c.dampingRatio = this.dampingRatio;
			c.frequency = this.frequency;
			c.length = this.length;
		}
	}
}
