using System;
using System.Collections.Generic;
using System.Linq;

using FarseerPhysics.Dynamics;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;

using Duality.Editor;
using Duality.Cloning;

namespace Duality.Components.Physics
{
	/// <summary>
	/// Describes a <see cref="RigidBody">Colliders</see> primitive shape. A Colliders overall shape may be combined of any number of primitive shapes.
	/// </summary>
	public abstract class ShapeInfo
	{
		protected static readonly Vector2[] EmptyVertices = new Vector2[0];
		
		[CloneBehavior(CloneBehavior.WeakReference)]
		private RigidBody parent      = null;
		protected float   density     = 1.0f;
		protected float   friction    = 0.3f;
		protected float   restitution = 0.3f;
		protected bool    sensor      = false;
		protected int     userTag     = 0;
			
		/// <summary>
		/// [GET] The shape's parent <see cref="RigidBody"/>.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public RigidBody Parent
		{
			get { return this.parent; }
			internal set { this.parent = value; }
		}

		/// <summary>
		/// [GET] A transform to transfrom the shapes defintion to worldcoordinates
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public Transform Transform
		{
			get
			{
				Transform transform;
				if (this.Parent != null)
					transform = this.Parent.GameObj.Transform;
				else transform = Transform.ZeroTransform;
				return transform;
			}
		}

		/// <summary>
		/// [GET / SET] The shapes density.
		/// </summary>
		[EditorHintIncrement(0.05f)]
		[EditorHintRange(0.0f, 100.0f)]
		public float Density
		{
			get { return this.density; }
			set 
			{
				this.density = value;
				if (this.parent != null) // Full update to recalculate mass
					this.parent.FlagBodyShape();
				else
					this.UpdateInternalShape(false);
			}
		}
		/// <summary>
		/// [GET / SET] Whether or not the shape acts as sensor i.e. is not part of a rigid body.
		/// </summary>
		public bool IsSensor
		{
			get { return this.sensor; }
			set { this.sensor = value; this.UpdateInternalShape(false); }
		}
		/// <summary>
		/// [GET / SET] A user-specified tag that can be used to identify this shape
		/// in collision handling code.
		/// </summary>
		public int UserTag
		{
			get { return this.userTag; }
			set { this.userTag = value; }
		}
		/// <summary>
		/// [GET / SET] The shapes friction value. Usually a value between 0.0 and 1.0, but higher values can be used to indicate unusually strong friction.
		/// </summary>
		[EditorHintIncrement(0.05f)]
		[EditorHintRange(0.0f, 10000.0f)]
		public float Friction
		{
			get { return this.friction; }
			set { this.friction = value; this.UpdateInternalShape(false); }
		}
		/// <summary>
		/// [GET / SET] The shapes restitution value. Should be a value between 0.0 and 1.0.
		/// </summary>
		[EditorHintIncrement(0.05f)]
		[EditorHintRange(0.0f, 1.0f)]
		public float Restitution
		{
			get { return this.restitution; }
			set { this.restitution = value; this.UpdateInternalShape(false); }
		}
		/// <summary>
		/// [GET] Whether or not the shape is a valid part of the physical simulation
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public virtual bool IsValid
		{
			get { return this.IsInternalShapeCreated || (this.parent != null && this.parent.IsFlaggedForSync); }
		}
		/// <summary>
		/// [GET] Returns the Shapes axis-aligned bounding box
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public abstract Rect AABB { get; }
		/// <summary>
		/// [GET] Whether the internal physics shape was successfully created and is now active.
		/// </summary>
		protected abstract bool IsInternalShapeCreated { get; }
		/// <summary>
		/// [GET] The scale factor of the parent object to be applied to all created shapes.
		/// </summary>
		protected float ParentScale
		{
			get
			{
				Transform transform = (this.parent != null && this.parent.gameobj != null) ? this.parent.gameobj.Transform : null;
				return transform != null ? transform.Scale : 1.0f;
			}
		}

		
		/// <summary>
		/// Updates the internal <see cref="RigidBody"/> Shape according to its properties.
		/// </summary>
		public void UpdateShape()
		{
			this.UpdateInternalShape(true);
		}

		public abstract bool IntersectsWith(Box box);
		
		internal void DestroyInternalShape()
		{
			this.DestroyFixtures();
		}
		internal void UpdateInternalShape(bool geometryChanged)
		{
			// When there is no parent object, destroy all fixtures and return
			if (this.parent == null)
			{
				this.DestroyFixtures();
				return;
			}
			
			// When updating fixture shapes at runtime, we'll need to re-initialize the whole body
			// as Farseer doesn't like shape changes of already active / simulated bodies.
			// Also note that the physical properties of the body need to be recalculated.
			if (geometryChanged && this.RequireFullBodyUpdate())
				return;

			this.SyncFixtures();
		}

		protected bool RequireFullBodyUpdate()
		{
			bool isPhysicsActive = 
				DualityApp.ExecContext == DualityApp.ExecutionContext.Game || 
				DualityApp.ExecEnvironment == DualityApp.ExecutionEnvironment.Editor;

			// This might happen in the middle of an update frame, so unless
			// we're already in the process of updating the body, schedule the
			// internal update for later and destroy fixtures so they'll be re-created.
			if (isPhysicsActive && this.IsInternalShapeCreated && this.parent != null && this.parent.FlagBodyShape())
			{
				this.DestroyFixtures();
				return true;
			}

			return false;
		}

		protected abstract void DestroyFixtures();
		protected abstract void SyncFixtures();
	}
}
