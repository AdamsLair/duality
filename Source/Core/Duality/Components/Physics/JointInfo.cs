using System;

using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics.Joints;

using Duality.Editor;
using Duality.Resources;
using Duality.Cloning;

namespace Duality.Components.Physics
{
	/// <summary>
	/// Describes a <see cref="RigidBody"/> joint. Joints limit a Colliders degree of freedom 
	/// by connecting it to fixed world coordinates or other Colliders.
	/// </summary>
	public abstract class JointInfo
	{
		[DontSerialize]
		internal protected Joint joint = null;
		[CloneBehavior(CloneBehavior.WeakReference)]
		private RigidBody parentBody = null;
		[CloneBehavior(CloneBehavior.WeakReference)]
		private RigidBody otherBody  = null;
		private bool      collide    = false;
		private bool      enabled    = true;
		private float     breakPoint = -1.0f;


		/// <summary>
		/// [GET] Whether this joint can be considered disposed. This is the case when either of its 
		/// connecting <see cref="RigidBody"/> instances is invalid or has been disposed.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public bool Disposed
		{
			get { return (this.parentBody != null && this.parentBody.Disposed) || (this.otherBody != null && this.otherBody.Disposed); }
		}
		/// <summary>
		/// [GET] The <see cref="RigidBody"/> from which this joint originates.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public RigidBody ParentBody
		{
			// Return null, if disposed - someone might access it before cleanup.
			get { return this.parentBody != null && !this.parentBody.Disposed ? this.parentBody : null; }
			internal set { this.parentBody = value; }
		}
		/// <summary>
		/// [GET] The <see cref="RigidBody"/> to which this joint connects to.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public RigidBody OtherBody
		{
			// Return null, if disposed - someone might access it before cleanup.
			get { return this.otherBody != null && !this.otherBody.Disposed ? this.otherBody : null; }
			internal set { this.otherBody = value; }
		}
		/// <summary>
		/// [GET / SET] Specifies whether the connected Colliders will collide with each other.
		/// </summary>
		public bool CollideConnected
		{
			get { return this.collide; }
			set { this.collide = value; this.UpdateJoint(); }
		}
		/// <summary>
		/// [GET / SET] Whether or not the joint is active.
		/// </summary>
		public bool Enabled
		{
			get { return this.enabled; }
			set { this.enabled = value; this.UpdateJoint(); }
		}
		/// <summary>
		/// [GET / SET] Maximum joint error value before the joint break. Breaking does not remove the joint, but disable it.
		/// A value of zero or lower is interpreted as unbreakable. Note that some joints might not have breaking point support 
		/// and will ignore this value.
		/// </summary>
		[EditorHintRange(-1.0f, float.MaxValue)]
		public float BreakPoint
		{
			get { return this.breakPoint; }
			set { this.breakPoint = value; this.UpdateJoint(); }
		}

			
		protected abstract Joint CreateJoint(World world, Body parentBody, Body otherBody);
		internal void DestroyJoint()
		{
			if (this.joint == null) return;
			Scene.PhysicsWorld.RemoveJoint(this.joint);
			this.joint.Broke -= this.joint_Broke;
			this.joint = null;
		}
		internal virtual void UpdateJoint()
		{
			if (this.joint == null)
			{
				if (this.parentBody != null && this.otherBody != null)
				{
					this.parentBody.PrepareForJoint();
					this.otherBody.PrepareForJoint();
					this.joint = this.CreateJoint(Scene.PhysicsWorld, this.parentBody.PhysicsBody, this.otherBody.PhysicsBody);
				}
				if (this.joint == null) return;

				this.joint.UserData = this;
				this.joint.Broke += this.joint_Broke;
			}

			this.joint.CollideConnected = this.collide;
			this.joint.Enabled = this.enabled;
			this.joint.Breakpoint = this.breakPoint <= 0.0f ? float.MaxValue : this.breakPoint;
		}
		private void joint_Broke(Joint arg1, float arg2)
		{
			this.enabled = false;
		}

		protected static Vector2 GetFarseerPoint(RigidBody c, Vector2 dualityPoint)
		{
			if (c == null) return PhysicsUnit.LengthToPhysical * dualityPoint;

			float scale = (c.GameObj != null && c.GameObj.Transform != null) ? c.GameObj.Transform.Scale : 1.0f;
			return PhysicsUnit.LengthToPhysical * dualityPoint * scale;
		}
		protected static Vector2 GetDualityPoint(RigidBody c, Vector2 farseerPoint)
		{
			if (c == null) return PhysicsUnit.LengthToDuality * farseerPoint;

			float scale = (c.GameObj != null && c.GameObj.Transform != null) ? c.GameObj.Transform.Scale : 1.0f;
			return PhysicsUnit.LengthToDuality * farseerPoint / scale;
		}
	}
}
