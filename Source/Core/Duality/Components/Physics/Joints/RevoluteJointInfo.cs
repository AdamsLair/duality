﻿using System;

using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics.Joints;

using Duality.Editor;
using Duality.Resources;

namespace Duality.Components.Physics
{
	/// <summary>
	/// Pins two locally anchored RigidBodies together without constraining rotation.
	/// </summary>
	public sealed class RevoluteJointInfo : JointInfo
	{
		private	Vector2		localAnchorA	= Vector2.Zero;
		private	Vector2		localAnchorB	= Vector2.Zero;
		private	float		lowerLimit		= 0.0f;
		private	float		upperLimit		= 0.0f;
		private	bool		limitEnabled	= false;
		private	float		refAngle		= 0.0f;
		private	bool		motorEnabled	= false;
		private float		maxMotorTorque	= 0.0f;
		private float		motorSpeed		= 0.0f;


		/// <summary>
		/// [GET / SET] The first RigidBodies local anchor point.
		/// </summary>
		[EditorHintIncrement(1)]
		public Vector2 LocalAnchorA
		{
			get { return this.localAnchorA; }
			set { this.localAnchorA = value; this.UpdateJoint(); }
		}
		/// <summary>
		/// [GET / SET] The second RigidBodies local anchor point.
		/// </summary>
		[EditorHintIncrement(1)]
		public Vector2 LocalAnchorB
		{
			get { return this.localAnchorB; }
			set { this.localAnchorB = value; this.UpdateJoint(); }
		}
		/// <summary>
		/// [GET / SET] Is the joint limited in its angle?
		/// </summary>
		public bool LimitEnabled
		{
			get { return this.limitEnabled; }
			set { this.limitEnabled = value; this.UpdateJoint(); }
		}
		/// <summary>
		/// [GET / SET] The lower joint limit in radians.
		/// </summary>
		[EditorHintIncrement(MathF.RadAngle1)]
		public float LowerLimit
		{
			get { return this.lowerLimit; }
			set { this.lowerLimit = MathF.Min(value, this.upperLimit); this.UpdateJoint(); }
		}
		/// <summary>
		/// [GET / SET] The upper joint limit in radians.
		/// </summary>
		[EditorHintIncrement(MathF.RadAngle1)]
		public float UpperLimit
		{
			get { return this.upperLimit; }
			set { this.upperLimit = MathF.Max(value, this.lowerLimit); this.UpdateJoint(); }
		}
		/// <summary>
		/// [GET / SET] The joint's reference angle.
		/// </summary>
		[EditorHintIncrement(MathF.RadAngle1)]
		public float ReferenceAngle
		{
			get { return this.refAngle; }
			set { this.refAngle = value; this.UpdateJoint(); }
		}
		/// <summary>
		/// [GET / SET] Is the joint motor enabled?
		/// </summary>
		public bool MotorEnabled
		{
			get { return this.motorEnabled; }
			set { this.motorEnabled = value; this.UpdateJoint(); }
		}
		/// <summary>
		/// [GET / SET] The maximum motor torque.
		/// </summary>
		public float MaxMotorTorque
		{
			get { return this.maxMotorTorque; }
			set { this.maxMotorTorque = value; this.UpdateJoint(); }
		}
		/// <summary>
		/// [GET / SET] The desired motor speed in degree per frame.
		/// </summary>
		[EditorHintIncrement(MathF.RadAngle1)]
		public float MotorSpeed
		{
			get { return MathF.RadToDeg(this.motorSpeed); }
			set { this.motorSpeed = MathF.DegToRad(value); this.UpdateJoint(); }
		}
		/// <summary>
		/// [GET] The current joint angle speed in radians per frame.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public float JointSpeed
		{
			get { return this.joint == null ? 0.0f : (PhysicsUnit.AngularVelocityToDuality * (this.joint as RevoluteJoint).JointSpeed); }
		}
		/// <summary>
		/// [GET] The current joint angle in radians.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public float JointAngle
		{
			get { return this.joint == null ? 0.0f : (PhysicsUnit.AngleToDuality * (this.joint as RevoluteJoint).JointAngle); }
		}
		/// <summary>
		/// [GET] The current joint motor torque.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public float MotorTorque
		{
			get { return this.joint == null ? 0.0f : (PhysicsUnit.TorqueToDuality * (this.joint as RevoluteJoint).MotorTorque); }
		}


		protected override Joint CreateJoint(Body bodyA, Body bodyB)
		{
			return bodyA != null && bodyB != null ? JointFactory.CreateRevoluteJoint(Scene.PhysicsWorld, bodyA, bodyB, Vector2.Zero) : null;
		}
		internal override void UpdateJoint()
		{
			base.UpdateJoint();
			if (this.joint == null) return;

			RevoluteJoint j = this.joint as RevoluteJoint;
			j.LocalAnchorB = GetFarseerPoint(this.OtherBody, this.localAnchorB);
			j.LocalAnchorA = GetFarseerPoint(this.ParentBody, this.localAnchorA);
			j.MotorEnabled = this.motorEnabled;
			j.MotorSpeed = -this.motorSpeed / Time.SPFMult;
			j.MaxMotorTorque = PhysicsUnit.TorqueToPhysical * this.maxMotorTorque;
			j.LimitEnabled = this.limitEnabled;
			j.LowerLimit = -this.upperLimit;
			j.UpperLimit = -this.lowerLimit;
			j.ReferenceAngle = -this.refAngle;
		}
	}
}
