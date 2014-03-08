using System;

using OpenTK;

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
	[Serializable]
	public abstract class JointInfo : ICloneExplicit
	{
		[NonSerialized]	
		internal protected	Joint	joint	= null;
		private		RigidBody	colA		= null;
		private		RigidBody	colB		= null;
		private		bool		collide		= false;
		private		bool		enabled		= true;
		private		float		breakPoint	= -1.0f;


		[EditorHintFlags(MemberFlags.Invisible)]
		public bool IsInitialized
		{
			get { return this.joint != null; }
		}
		[EditorHintFlags(MemberFlags.Invisible)]
		public bool Disposed
		{
			get { return (this.colA != null && this.colA.Disposed) || (this.colB != null && this.colB.Disposed); }
		}
		[EditorHintFlags(MemberFlags.Invisible)]
		public RigidBody BodyA
		{
			// Return null, if disposed - someone might access it before cleanup.
			get { return this.colA != null && !this.colA.Disposed ? this.colA : null; }
			internal set { this.colA = value; }
		}
		[EditorHintFlags(MemberFlags.Invisible)]
		public RigidBody BodyB
		{
			// Return null, if disposed - someone might access it before cleanup.
			get { return this.colB != null && !this.colB.Disposed ? this.colB : null; }
			internal set { this.colB = value; }
		}
		/// <summary>
		/// [GET] Returns whether the joint is connecting two Colliders (instead of connecting one to the world)
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public abstract bool DualJoint { get; }
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

			
		protected abstract Joint CreateJoint(Body bodyA, Body bodyB);
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
				this.joint = this.CreateJoint(this.colA != null ? this.colA.PhysicsBody : null, this.colB != null ? this.colB.PhysicsBody : null);
				if (this.joint == null) return; // Failed to create the joint? Return.

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

		/// <summary>
		/// Copies this JointInfos data to another one. It is assumed that both are of the same type.
		/// </summary>
		/// <param name="target"></param>
		protected virtual void CopyTo(JointInfo target)
		{
			// Don't copy the parents!
			target.collide = this.collide;
			target.enabled = this.enabled;
			target.breakPoint = this.breakPoint;
		}
		/// <summary>
		/// Clones the JointInfo.
		/// </summary>
		/// <returns></returns>
		public JointInfo Clone()
		{
			JointInfo newObj = this.GetType().CreateInstanceOf() as JointInfo;
			this.CopyTo(newObj);
			return newObj;
		}

		void ICloneExplicit.CopyDataTo(object targetObj, CloneProvider provider)
		{
			JointInfo targetJoint = targetObj as JointInfo;
			this.CopyTo(targetJoint);
		}

		protected static Vector2 GetFarseerPoint(RigidBody c, Vector2 dualityPoint)
		{
			if (c == null) return PhysicsConvert.ToPhysicalUnit(dualityPoint);

			float scale = (c.GameObj != null && c.GameObj.Transform != null) ? c.GameObj.Transform.Scale : 1.0f;
			return PhysicsConvert.ToPhysicalUnit(dualityPoint * scale);
		}
		protected static Vector2 GetDualityPoint(RigidBody c, Vector2 farseerPoint)
		{
			if (c == null) return PhysicsConvert.ToDualityUnit(farseerPoint);

			float scale = (c.GameObj != null && c.GameObj.Transform != null) ? c.GameObj.Transform.Scale : 1.0f;
			return PhysicsConvert.ToDualityUnit(farseerPoint / scale);
		}
	}
}
