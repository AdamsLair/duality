using System;

using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics.Joints;

using Duality.Editor;
using Duality.Resources;

namespace Duality.Components.Physics
{
	/// <summary>
	/// This joint allows the RigidBody to travel on a specific axis relative to another body. It can be limited to a certain area and driven by a motor force.
	/// </summary>
	public sealed class PrismaticJointInfo : JointInfo
	{
		private	Vector2		localAnchorA	= Vector2.Zero;
		private	Vector2		localAnchorB	= Vector2.Zero;
		private	Vector2		moveAxis		= Vector2.UnitX;
		private	bool		limitEnabled	= false;
		private	float		lowerLimit		= 0.0f;
		private	float		upperLimit		= 0.0f;
		private	bool		motorEnabled	= false;
		private float		maxMotorForce	= 0.0f;
		private float		motorSpeed		= 0.0f;
		private	float		refAngle		= 0.0f;


		/// <summary>
		/// [GET / SET] The local anchor point on the first RigidBody.
		/// </summary>
		[EditorHintIncrement(1)]
		public Vector2 LocalAnchorA
		{
			get { return this.localAnchorA; }
			set { this.localAnchorA = value; this.UpdateJoint(); }
		}
		/// <summary>
		/// [GET / SET] The local anchor point on the second RigidBody.
		/// </summary>
		[EditorHintIncrement(1)]
		public Vector2 LocalAnchorB
		{
			get { return this.localAnchorB; }
			set { this.localAnchorB = value; this.UpdateJoint(); }
		}
		/// <summary>
		/// [GET / SET] The axis on which the body may move.
		/// </summary>
		public Vector2 MovementAxis
		{
			get { return this.moveAxis; }
			set { this.moveAxis = value == Vector2.Zero ? Vector2.UnitX : value.Normalized; this.UpdateJoint(); }
		}
		/// <summary>
		/// [GET / SET] Is the joint limited in its movement?
		/// </summary>
		public bool LimitEnabled
		{
			get { return this.limitEnabled; }
			set { this.limitEnabled = value; this.UpdateJoint(); }
		}
		/// <summary>
		/// [GET / SET] The lower joint limit.
		/// </summary>
		[EditorHintIncrement(1)]
		public float LowerLimit
		{
			get { return this.lowerLimit; }
			set { this.lowerLimit = MathF.Min(value, this.upperLimit); this.UpdateJoint(); }
		}
		/// <summary>
		/// [GET / SET] The upper joint limit.
		/// </summary>
		[EditorHintIncrement(1)]
		public float UpperLimit
		{
			get { return this.upperLimit; }
			set { this.upperLimit = MathF.Max(value, this.lowerLimit); this.UpdateJoint(); }
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
		/// [GET / SET] The maximum motor force.
		/// </summary>
		public float MaxMotorForce
		{
			get { return this.maxMotorForce; }
			set { this.maxMotorForce = value; this.UpdateJoint(); }
		}
		/// <summary>
		/// [GET / SET] The desired motor speed.
		/// </summary>
		[EditorHintIncrement(1)]
		public float MotorSpeed
		{
			get { return this.motorSpeed; }
			set { this.motorSpeed = value; this.UpdateJoint(); }
		}
		/// <summary>
		/// [GET] The current joint speed.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public float JointSpeed
		{
			get { return this.joint == null ? 0.0f : (PhysicsUnit.VelocityToDuality * (this.joint as PrismaticJoint).JointSpeed); }
		}
		/// <summary>
		/// [GET] The current joint translation.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public float JointTranslation
		{
			get { return this.joint == null ? 0.0f : (PhysicsUnit.LengthToDuality * (this.joint as PrismaticJoint).JointTranslation); }
		}
		/// <summary>
		/// [GET] The current joint motor force.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public float MotorForce
		{
			get { return this.joint == null ? 0.0f : (PhysicsUnit.ForceToDuality * (this.joint as PrismaticJoint).MotorForce * Time.SPFMult); }
		}
		/// <summary>
		/// [GET / SET] The reference angle that is used to constrain the bodies angle.
		/// </summary>
		[EditorHintIncrement(MathF.RadAngle1)]
		public float ReferenceAngle
		{
			get { return this.refAngle; }
			set { this.refAngle = MathF.NormalizeAngle(value); this.UpdateJoint(); }
		}


		protected override Joint CreateJoint(Body bodyA, Body bodyB)
		{
			return bodyA != null && bodyB != null ? JointFactory.CreatePrismaticJoint(Scene.PhysicsWorld, bodyA, bodyB, Vector2.Zero, Vector2.UnitX) : null;
		}
		internal override void UpdateJoint()
		{
			base.UpdateJoint();
			if (this.joint == null) return;

			PrismaticJoint j = this.joint as PrismaticJoint;
			j.LocalAnchorA = GetFarseerPoint(this.ParentBody, this.localAnchorA);
			j.LocalAnchorB = GetFarseerPoint(this.OtherBody, this.localAnchorB);
			j.ReferenceAngle = this.refAngle;
			j.LocalXAxis1 = this.ParentBody.GameObj.Transform.GetWorldVector(this.moveAxis).Normalized;
			j.LimitEnabled = this.limitEnabled;
			j.LowerLimit = -PhysicsUnit.LengthToPhysical * this.upperLimit;
			j.UpperLimit = -PhysicsUnit.LengthToPhysical * this.lowerLimit;
			j.MotorEnabled = this.motorEnabled;
			j.MotorSpeed = -PhysicsUnit.VelocityToPhysical * this.motorSpeed;
			j.MaxMotorForce = PhysicsUnit.ForceToPhysical * this.maxMotorForce;
		}
	}
}
