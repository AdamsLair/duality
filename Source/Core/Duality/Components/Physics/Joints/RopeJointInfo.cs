﻿using System;

using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics.Joints;

using Duality.Editor;
using Duality.Resources;

namespace Duality.Components.Physics
{
	/// <summary>
	/// Constrains two RigidBodies to not exceed a maximum distance to each other.
	/// </summary>
	public sealed class RopeJointInfo : JointInfo
	{
		private	Vector2		localAnchorA	= Vector2.Zero;
		private	Vector2		localAnchorB	= Vector2.Zero;
		private	float		maxLength		= 500.0f;


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
		/// [GET / SET] The maximum distance between both RigidBodies i.e. the "rope length".
		/// </summary>
		[EditorHintIncrement(1)]
		[EditorHintRange(0.0f, 10000000.0f)]
		public float MaxLength
		{
			get { return this.maxLength; }
			set { this.maxLength = value; this.UpdateJoint(); }
		}


		protected override Joint CreateJoint(Body bodyA, Body bodyB)
		{
			if (bodyA == null || bodyB == null) return null;
			RopeJoint joint = new RopeJoint(bodyA, bodyB, Vector2.Zero, Vector2.Zero);
			Scene.PhysicsWorld.AddJoint(joint);
			return joint;
		}
		internal override void UpdateJoint()
		{
			base.UpdateJoint();
			if (this.joint == null) return;

			RopeJoint j = this.joint as RopeJoint;
			j.LocalAnchorB = GetFarseerPoint(this.OtherBody, this.localAnchorB);
			j.LocalAnchorA = GetFarseerPoint(this.ParentBody, this.localAnchorA);
			j.MaxLength = PhysicsUnit.LengthToPhysical * this.maxLength;
		}
	}
}
