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
	/// Constrains the Collider to obtain a fixed distance to a world coordinate
	/// </summary>
	[Serializable]
	public sealed class FixedDistanceJointInfo : JointInfo
	{
		private	Vector2		localAnchor		= Vector2.Zero;
		private	Vector2		worldAnchor		= Vector2.Zero;
		private	float		dampingRatio	= 0.5f;
		private	float		frequency		= 1.0f;
		private	float		length			= 200.0f;


		public override bool DualJoint
		{
			get { return false; }
		}
		/// <summary>
		/// [GET / SET] The Colliders local anchor point.
		/// </summary>
		[EditorHintIncrement(1)]
		public Vector2 LocalAnchor
		{
			get { return this.localAnchor; }
			set { this.localAnchor = value; this.UpdateJoint(); }
		}
		/// <summary>
		/// [GET / SET] The world anchor point to which the Collider will be attached.
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
			return bodyA != null ? JointFactory.CreateFixedDistanceJoint(Scene.PhysicsWorld, bodyA, Vector2.Zero, Vector2.Zero) : null;
		}
		internal override void UpdateJoint()
		{
			base.UpdateJoint();
			if (this.joint == null) return;

			FixedDistanceJoint j = this.joint as FixedDistanceJoint;
			j.WorldAnchorB = PhysicsConvert.ToPhysicalUnit(this.worldAnchor);
			j.LocalAnchorA = GetFarseerPoint(this.BodyA, this.localAnchor);
			j.DampingRatio = this.dampingRatio;
			j.Frequency = this.frequency;
			j.Length = PhysicsConvert.ToPhysicalUnit(this.length);
		}

		protected override void CopyTo(JointInfo target)
		{
			base.CopyTo(target);
			FixedDistanceJointInfo c = target as FixedDistanceJointInfo;
			c.worldAnchor = this.worldAnchor;
			c.localAnchor = this.localAnchor;
			c.dampingRatio = this.dampingRatio;
			c.frequency = this.frequency;
			c.length = this.length;
		}
	}
}
