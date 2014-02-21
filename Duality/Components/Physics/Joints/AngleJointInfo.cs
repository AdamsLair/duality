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
	/// Constrains two RigidBodies to a fixed relative angle
	/// </summary>
	[Serializable]
	public sealed class AngleJointInfo : JointInfo
	{
		private	float	angle		= 0.0f;
		private	float	biasFactor	= 0.2f;
		private	float	softness	= 0.0f;
		private	float	maxImpulse	= -1.0f;


		public override bool DualJoint
		{
			get { return true; }
		}
		/// <summary>
		/// [GET / SET] The Colliders target angle.
		/// </summary>
		[EditorHintIncrement(MathF.RadAngle360 / 360.0f)]
		public float TargetAngle
		{
			get { return this.angle; }
			set { this.angle = value; this.UpdateJoint(); }
		}
		/// <summary>
		/// [GET / SET] The bias factor determines how strong the joint reacts to the difference between target and actual angle.
		/// </summary>
		[EditorHintRange(0.0f, 1.0f)]
		public float BiasFactor
		{
			get { return this.biasFactor; }
			set { this.biasFactor = value; this.UpdateJoint(); }
		}
		/// <summary>
		/// [GET / SET] The softness of the joint determines how easy it is to turn it away from its ideal angle.
		/// </summary>
		[EditorHintRange(0.0f, 1.0f)]
		[EditorHintDecimalPlaces(3)]
		[EditorHintIncrement(0.01f)]
		public float Softness
		{
			get { return this.softness; }
			set { this.softness = value; this.UpdateJoint(); }
		}
		/// <summary>
		/// [GET / SET] The maximum angular impulse to apply to the RigidBody. A negative value equals infinity.
		/// </summary>
		public float MaxImpulse
		{
			get { return this.maxImpulse; }
			set { this.maxImpulse = value; this.UpdateJoint(); }
		}


		protected override Joint CreateJoint(Body bodyA, Body bodyB)
		{
			return bodyA != null && bodyB != null ? JointFactory.CreateAngleJoint(Scene.PhysicsWorld, bodyA, bodyB) : null;
		}
		internal override void UpdateJoint()
		{
			base.UpdateJoint();
			if (this.joint == null) return;

			AngleJoint j = this.joint as AngleJoint;
			j.TargetAngle = this.angle;
			j.BiasFactor = this.biasFactor;
			j.Softness = this.softness;
			j.MaxImpulse = this.maxImpulse < 0.0f ? float.MaxValue : PhysicsConvert.ToPhysicalUnit(this.maxImpulse);
		}

		protected override void CopyTo(JointInfo target)
		{
			base.CopyTo(target);
			AngleJointInfo c = target as AngleJointInfo;
			c.angle = this.angle;
			c.biasFactor = this.biasFactor;
			c.softness = this.softness;
			c.maxImpulse = this.maxImpulse;
		}
	}
}
