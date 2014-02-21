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
	/// Constrains two RigidBodies to keep their distance to each other in a certain range.
	/// You can view this as a massless, rigid rod.
	/// </summary>
	[Serializable]
	public sealed class SliderJointInfo : JointInfo
	{
		private	Vector2		localAnchorA	= Vector2.Zero;
		private	Vector2		localAnchorB	= Vector2.Zero;
		private	float		maxLength		= 500.0f;
		private	float		minLength		= 500.0f;


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
		/// [GET / SET] The maximum distance between both RigidBodies.
		/// </summary>
		[EditorHintIncrement(1)]
		[EditorHintRange(0.0f, 10000000.0f)]
		public float MaxLength
		{
			get { return this.maxLength; }
			set { this.maxLength = value; this.UpdateJoint(); }
		}
		/// <summary>
		/// [GET / SET] The minimum distance between both RigidBodies.
		/// </summary>
		[EditorHintIncrement(1)]
		[EditorHintRange(0.0f, 10000000.0f)]
		public float MinLength
		{
			get { return this.minLength; }
			set { this.minLength = value; this.UpdateJoint(); }
		}


		protected override Joint CreateJoint(Body bodyA, Body bodyB)
		{
			return bodyA != null && bodyB != null ? JointFactory.CreateSliderJoint(Scene.PhysicsWorld, bodyA, bodyB, Vector2.Zero, Vector2.Zero, 0.0f, 0.0f) : null;
		}
		internal override void UpdateJoint()
		{
			base.UpdateJoint();
			if (this.joint == null) return;

			SliderJoint j = this.joint as SliderJoint;
			j.LocalAnchorB = GetFarseerPoint(this.BodyB, this.localAnchorB);
			j.LocalAnchorA = GetFarseerPoint(this.BodyA, this.localAnchorA);
			j.MaxLength = PhysicsConvert.ToPhysicalUnit(this.maxLength);
			j.MinLength = PhysicsConvert.ToPhysicalUnit(this.minLength);
		}

		protected override void CopyTo(JointInfo target)
		{
			base.CopyTo(target);
			SliderJointInfo c = target as SliderJointInfo;
			c.localAnchorB = this.localAnchorB;
			c.localAnchorA = this.localAnchorA;
			c.maxLength = this.maxLength;
			c.minLength = this.minLength;
		}
	}
}
