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
	/// Constrains two RigidBodies as though connected using a rope that is fixed at
	/// two world anchor points.
	/// </summary>
	[Serializable]
	public sealed class PulleyJointInfo : JointInfo
	{
		private	Vector2		localAnchorA	= Vector2.Zero;
		private	Vector2		localAnchorB	= Vector2.Zero;
		private	Vector2		worldAnchorA	= Vector2.Zero;
		private	Vector2		worldAnchorB	= Vector2.Zero;
		private	float		maxLengthA		= 300.0f;
		private	float		maxLengthB		= 300.0f;
		private	float		totalLength		= 600.0f;
		private	float		ratio			= 1.0f;


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
		/// [GET / SET] The first bodies world anchor point.
		/// </summary>
		[EditorHintIncrement(1)]
		public Vector2 WorldAnchorA
		{
			get { return this.worldAnchorA; }
			set { this.worldAnchorA = value; this.UpdateJoint(); }
		}
		/// <summary>
		/// [GET / SET] The second bodies world anchor point.
		/// </summary>
		[EditorHintIncrement(1)]
		public Vector2 WorldAnchorB
		{
			get { return this.worldAnchorB; }
			set { this.worldAnchorB = value; this.UpdateJoint(); }
		}
		/// <summary>
		/// [GET / SET] The maximum "rope length" on the first bodies side of the pulley.
		/// </summary>
		[EditorHintIncrement(1)]
		[EditorHintRange(0.0f, 10000000.0f)]
		public float MaxLengthA
		{
			get { return this.maxLengthA; }
			set { this.maxLengthA = value; this.UpdateJoint(); }
		}
		/// <summary>
		/// [GET / SET] The maximum "rope length" on the second bodies side of the pulley.
		/// </summary>
		[EditorHintIncrement(1)]
		[EditorHintRange(0.0f, 10000000.0f)]
		public float MaxLengthB
		{
			get { return this.maxLengthB; }
			set { this.maxLengthB = value; this.UpdateJoint(); }
		}
		/// <summary>
		/// [GET] The current "rope length" on the first bodies side of the pulley.
		/// </summary>
		public float LengthA
		{
			get
			{
				if (this.BodyA == null || this.BodyA.GameObj == null || this.BodyA.GameObj.Transform == null) return 0.0f;
				Vector2 v = this.BodyA.GameObj.Transform.GetWorldPoint(this.localAnchorA);
				return (v - this.worldAnchorA).Length;
			}
		}
		/// <summary>
		/// [GET] The current "rope length" on the second bodies side of the pulley.
		/// </summary>
		public float LengthB
		{
			get
			{
				if (this.BodyB == null || this.BodyB.GameObj == null || this.BodyB.GameObj.Transform == null) return 0.0f;
				Vector2 v = this.BodyB.GameObj.Transform.GetWorldPoint(this.localAnchorB);
				return (v - this.worldAnchorB).Length;
			}
		}
		/// <summary>
		/// [GET / SET] The ratio by which the second rope end is enlarged or shrinked on size changes of the first rope end.
		/// </summary>
		[EditorHintRange(0.0f, 1000.0f)]
		public float Ratio
		{
			get { return this.ratio; }
			set { this.ratio = value; this.UpdateJoint(); }
		}
		/// <summary>
		/// [GET / SET] The total length of the rope.
		/// </summary>
		[EditorHintRange(0.0f, 10000000.0f)]
		public float TotalLength
		{
			get { return this.totalLength; }
			set { this.totalLength = value; this.UpdateJoint(); }
		}


		protected override Joint CreateJoint(Body bodyA, Body bodyB)
		{
			if (bodyA == null || bodyB == null) return null;
			PulleyJoint joint = new PulleyJoint(bodyA, bodyB, Vector2.Zero, Vector2.Zero, Vector2.Zero, Vector2.Zero, 1.0f);
			Scene.PhysicsWorld.AddJoint(joint);
			return joint;
		}
		internal override void UpdateJoint()
		{
			base.UpdateJoint();
			if (this.joint == null) return;

			PulleyJoint j = this.joint as PulleyJoint;
			j.LocalAnchorB = GetFarseerPoint(this.BodyB, this.localAnchorB);
			j.LocalAnchorA = GetFarseerPoint(this.BodyA, this.localAnchorA);
			j.GroundAnchorB = PhysicsConvert.ToPhysicalUnit(this.worldAnchorB);
			j.GroundAnchorA = PhysicsConvert.ToPhysicalUnit(this.worldAnchorA);
			j.MaxLengthA = PhysicsConvert.ToPhysicalUnit(this.maxLengthA);
			j.MaxLengthB = PhysicsConvert.ToPhysicalUnit(this.maxLengthB);
			j.TotalLength = PhysicsConvert.ToPhysicalUnit(this.totalLength);
			j.Ratio = this.ratio;
		}

		protected override void CopyTo(JointInfo target)
		{
			base.CopyTo(target);
			PulleyJointInfo c = target as PulleyJointInfo;
			c.localAnchorB = this.localAnchorB;
			c.localAnchorA = this.localAnchorA;
			c.worldAnchorB = this.worldAnchorB;
			c.worldAnchorA = this.worldAnchorA;
			c.maxLengthA = this.maxLengthA;
			c.maxLengthB = this.maxLengthB;
			c.totalLength = this.totalLength;
			c.ratio = this.ratio;
		}
	}
}
