﻿using System;

using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics.Joints;

using Duality.Editor;
using Duality.Resources;

namespace Duality.Components.Physics
{
	/// <summary>
	/// "Welds" two Colliders together so they share a common point and relative angle.
	/// </summary>
	public sealed class WeldJointInfo : JointInfo
	{
		private Vector2 localAnchorA	= Vector2.Zero;
		private	Vector2	localAnchorB	= Vector2.Zero;
		private	float	refAngle		= 0.0f;
			

		/// <summary>
		/// [GET / SET] The welding point, locally to the first object.
		/// </summary>
		[EditorHintIncrement(1)]
		public Vector2 LocalAnchorA
		{
			get { return this.localAnchorA; }
			set { this.localAnchorA = value; this.UpdateJoint(); }
		}
		/// <summary>
		/// [GET / SET] The welding point, locally to the second object.
		/// </summary>
		[EditorHintIncrement(1)]
		public Vector2 LocalAnchorB
		{
			get { return this.localAnchorB; }
			set { this.localAnchorB = value; this.UpdateJoint(); }
		}
		/// <summary>
		/// [GET / SET] The relative angle both objects need to keep.
		/// </summary>
		[EditorHintIncrement(MathF.RadAngle1)]
		public float RefAngle
		{
			get { return this.refAngle; }
			set { this.refAngle = value; this.UpdateJoint(); }
		}


		protected override Joint CreateJoint(Body bodyA, Body bodyB)
		{
			return bodyA != null && bodyB != null ? JointFactory.CreateWeldJoint(Scene.PhysicsWorld, bodyA, bodyB, Vector2.Zero) : null;
		}
		internal override void UpdateJoint()
		{
			base.UpdateJoint();
			if (this.joint == null) return;

			WeldJoint j = this.joint as WeldJoint;
			j.LocalAnchorA = GetFarseerPoint(this.ParentBody, this.localAnchorA);
			j.LocalAnchorB = GetFarseerPoint(this.OtherBody, this.localAnchorB);
			j.ReferenceAngle = this.refAngle;
		}
	}
}
