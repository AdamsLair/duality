using System;

using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics.Joints;

using Duality.Editor;
using Duality.Resources;

namespace Duality.Components.Physics
{
	/// <summary>
	/// The line joint is also called "wheel joint", because it behaves like the spring of a car tire:
	/// A body is only allowed to travel on a specific world axis relative to the other one but can rotate
	/// freely or accelerated by a motor.
	/// </summary>
	public sealed class LineJointInfo : JointInfo
	{
		private	Vector2		localAnchorA	= Vector2.Zero;
		private	Vector2		localAnchorB	= Vector2.Zero;
		private	Vector2		moveAxis		= Vector2.UnitY;
		private	bool		motorEnabled	= false;
		private float		maxMotorTorque	= 0.0f;
		private float		motorSpeed		= 0.0f;
		private	float		dampingRatio	= 0.5f;
		private	float		frequency		= 5.0f;


		/// <summary>
		/// [GET / SET] The car RigidBodies local anchor point.
		/// </summary>
		[EditorHintIncrement(1)]
		public Vector2 CarAnchor
		{
			get { return this.localAnchorA; }
			set { this.localAnchorA = value; this.UpdateJoint(); }
		}
		/// <summary>
		/// [GET / SET] The wheel RigidBodies local anchor point.
		/// </summary>
		[EditorHintIncrement(1)]
		public Vector2 WheelAnchor
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
			set { this.moveAxis = value; this.UpdateJoint(); }
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
			get { return this.joint == null ? 0.0f : (PhysicsUnit.AngularVelocityToDuality * (this.joint as LineJoint).JointSpeed); }
		}
		/// <summary>
		/// [GET] The current joint translation.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public float JointTranslation
		{
			get { return this.joint == null ? 0.0f : (PhysicsUnit.LengthToDuality * (this.joint as LineJoint).JointTranslation); }
		}
		/// <summary>
		/// [GET] The current joint motor torque.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public float MotorTorque
		{
			get { return this.joint == null ? 0.0f : (PhysicsUnit.TorqueToDuality * (this.joint as LineJoint).GetMotorTorque(1.0f)); }
		}


		protected override Joint CreateJoint(Body bodyA, Body bodyB)
		{
			return bodyA != null && bodyB != null ? JointFactory.CreateLineJoint(Scene.PhysicsWorld, bodyA, bodyB, Vector2.Zero, Vector2.UnitY) : null;
		}
		internal override void UpdateJoint()
		{
			base.UpdateJoint();
			if (this.joint == null) return;

			LineJoint j = this.joint as LineJoint;
			j.LocalAnchorB = GetFarseerPoint(this.OtherBody, this.localAnchorB);
			j.LocalAnchorA = GetFarseerPoint(this.ParentBody, this.localAnchorA);
			j.LocalXAxis = this.moveAxis;
			j.MotorEnabled = this.motorEnabled;
			j.MotorSpeed = this.motorSpeed / Time.SecondsPerFrame;
			j.MaxMotorTorque = PhysicsUnit.TimeToPhysical * this.maxMotorTorque;
			j.DampingRatio = this.dampingRatio;
			j.Frequency = this.frequency;
		}
	}
}
