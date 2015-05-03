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
				if (this.ParentBody == null || this.ParentBody.GameObj == null || this.ParentBody.GameObj.Transform == null) return 0.0f;
				Vector2 v = this.ParentBody.GameObj.Transform.GetWorldPoint(this.localAnchorA);
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
				if (this.OtherBody == null || this.OtherBody.GameObj == null || this.OtherBody.GameObj.Transform == null) return 0.0f;
				Vector2 v = this.OtherBody.GameObj.Transform.GetWorldPoint(this.localAnchorB);
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
			j.LocalAnchorB = GetFarseerPoint(this.OtherBody, this.localAnchorB);
			j.LocalAnchorA = GetFarseerPoint(this.ParentBody, this.localAnchorA);
			j.GroundAnchorB = PhysicsUnit.LengthToPhysical * this.worldAnchorB;
			j.GroundAnchorA = PhysicsUnit.LengthToPhysical * this.worldAnchorA;
			j.MaxLengthA = PhysicsUnit.LengthToPhysical * this.maxLengthA;
			j.MaxLengthB = PhysicsUnit.LengthToPhysical * this.maxLengthB;
			j.TotalLength = PhysicsUnit.LengthToPhysical * this.totalLength;
			j.Ratio = this.ratio;
		}
	}
}
