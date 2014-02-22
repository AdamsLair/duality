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
	/// Pins two locally anchored RigidBodies together without constraining rotation.
	/// </summary>
	[Serializable]
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


		public override bool DualJoint
		{
			get { return true; }
		}
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
		/// [GET / SET] The desired motor speed in radians per frame.
		/// </summary>
		[EditorHintIncrement(MathF.RadAngle1)]
		public float MotorSpeed
		{
			get { return this.motorSpeed; }
			set { this.motorSpeed = value; this.UpdateJoint(); }
		}
		/// <summary>
		/// [GET] The current joint angle speed in radians per frame.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public float JointSpeed
		{
			get { return this.joint == null ? 0.0f : (this.joint as RevoluteJoint).JointSpeed * Time.SPFMult; }
		}
		/// <summary>
		/// [GET] The current joint angle in radians.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public float JointAngle
		{
			get { return this.joint == null ? 0.0f : (this.joint as RevoluteJoint).JointAngle; }
		}
		/// <summary>
		/// [GET] The current joint motor torque.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public float MotorTorque
		{
			get { return this.joint == null ? 0.0f : PhysicsConvert.ToDualityUnit((this.joint as RevoluteJoint).MotorTorque) * MathF.RadAngle360 * Time.SPFMult; }
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
			j.LocalAnchorB = GetFarseerPoint(this.BodyB, this.localAnchorB);
			j.LocalAnchorA = GetFarseerPoint(this.BodyA, this.localAnchorA);
			j.MotorEnabled = this.motorEnabled;
			j.MotorSpeed = -this.motorSpeed / Time.SPFMult;
			j.MaxMotorTorque = PhysicsConvert.ToPhysicalUnit(this.maxMotorTorque) / Time.SPFMult;
			j.LimitEnabled = this.limitEnabled;
			j.LowerLimit = -this.upperLimit;
			j.UpperLimit = -this.lowerLimit;
			j.ReferenceAngle = -this.refAngle;
		}

		protected override void CopyTo(JointInfo target)
		{
			base.CopyTo(target);
			RevoluteJointInfo c = target as RevoluteJointInfo;
			c.localAnchorB = this.localAnchorB;
			c.localAnchorA = this.localAnchorA;
			c.motorSpeed = this.motorSpeed;
			c.maxMotorTorque = this.maxMotorTorque;
			c.motorEnabled = this.motorEnabled;
			c.limitEnabled = this.limitEnabled;
			c.lowerLimit = this.lowerLimit;
			c.upperLimit = this.upperLimit;
			c.refAngle = this.refAngle;
		}
	}
}
