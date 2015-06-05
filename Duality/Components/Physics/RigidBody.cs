using System;
using System.Collections.Generic;
using System.Linq;

using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;

using Duality.Editor;
using Duality.Cloning;
using Duality.Resources;
using Duality.Properties;

namespace Duality.Components.Physics
{
	/// <summary>
	/// Represents a body instance for physical simulation, collision detection and response.
	/// </summary>
	[ManuallyCloned]
	[RequiredComponent(typeof(Transform))]
	[EditorHintCategory(typeof(CoreRes), CoreResNames.CategoryPhysics)]
	[EditorHintImage(typeof(CoreRes), CoreResNames.ImageRigidBody)]
	public sealed class RigidBody : Component, ICmpInitializable, ICmpUpdatable, ICmpEditorUpdatable
	{
		private struct ColEvent
		{
			public enum EventType
			{
				Collision,
				Separation,
				PostSolve
			}

			public	EventType		Type;
			public	Fixture			FixtureA;
			public	Fixture			FixtureB;
			public	CollisionData	Data;

			public ColEvent(EventType type, Fixture fxA, Fixture fxB, CollisionData data)
			{
				this.Type = type;
				this.FixtureA = fxA;
				this.FixtureB = fxB;
				this.Data = data;
			}
		}


		private	BodyType			bodyType		= BodyType.Dynamic;
		private	float				linearDamp		= 0.3f;
		private	float				angularDamp		= 0.3f;
		private	bool				fixedAngle		= false;
		private	bool				ignoreGravity	= false;
		private	bool				continous		= false;
		private	Vector2				linearVel		= Vector2.Zero;
		private	float				angularVel		= 0.0f;
		private	float				revolutions		= 0.0f;
		private	float				explicitMass	= 0.0f;
		private	CollisionCategory	colCat			= CollisionCategory.Cat1;
		private	CollisionCategory	colWith			= CollisionCategory.All;
		private	List<ShapeInfo>		shapes			= null;
		private	List<JointInfo>		joints			= null;

		[DontSerialize] private	float			lastScale			= 1.0f;
		[DontSerialize] private	InitState		bodyInitState		= InitState.Disposed;
		[DontSerialize] private	bool			schedUpdateBody		= false;
		[DontSerialize] private	bool			isUpdatingBody		= false;
		[DontSerialize] private	bool			isProcessingEvents	= false;
		[DontSerialize] private	Body			body				= null;
		[DontSerialize] private	List<ColEvent>	eventBuffer			= new List<ColEvent>();


		internal Body PhysicsBody
		{
			get { return this.body; }
		}
		/// <summary>
		/// [GET / SET] The type of the physical body.
		/// </summary>
		public BodyType BodyType
		{
			get { return this.bodyType; }
			set 
			{
				if (this.body != null)
				{
					this.body.BodyType = (value == BodyType.Static ? FarseerPhysics.Dynamics.BodyType.Static : FarseerPhysics.Dynamics.BodyType.Dynamic);
					this.FlagBodyShape();
				}
				this.bodyType = value;
			}
		}
		/// <summary>
		/// [GET / SET] The damping that is applied to the bodies velocity.
		/// </summary>
		[EditorHintRange(0.0f, 10000.0f, 0.0f, 10.0f)]
		public float LinearDamping
		{
			get { return this.linearDamp; }
			set 
			{
				if (this.body != null) this.body.LinearDamping = value;
				this.linearDamp = value;
			}
		}
		/// <summary>
		/// [GET / SET] The damping that is applied to the bodies angular velocity.
		/// </summary>
		[EditorHintRange(0.0f, 10000.0f, 0.0f, 10.0f)]
		public float AngularDamping
		{
			get { return this.angularDamp; }
			set 
			{
				if (this.body != null) this.body.AngularDamping = value;
				this.angularDamp = value;
			}
		}
		/// <summary>
		/// [GET / SET] Whether the bodies rotation is fixed.
		/// </summary>
		public bool FixedAngle
		{
			get { return this.fixedAngle; }
			set 
			{
				if (this.body != null) this.body.FixedRotation = value;
				this.fixedAngle = value;
			}
		}
		/// <summary>
		/// [GET / SET] Whether the body ignores gravity.
		/// </summary>
		public bool IgnoreGravity
		{
			get { return this.ignoreGravity; }
			set 
			{
				if (this.body != null) this.body.IgnoreGravity = value;
				this.ignoreGravity = value;
			}
		}
		/// <summary>
		/// [GET / SET] Whether the body is included in continous collision detection or not.
		/// It prevents the body from moving through others at high speeds at the cost of Profile.
		/// </summary>
		public bool ContinousCollision
		{
			get { return this.continous; }
			set 
			{
				if (this.body != null) this.body.IsBullet = value;
				this.continous = value;
			}
		}
		/// <summary>
		/// [GET / SET] The Colliders current linear velocity.
		/// </summary>
		public Vector2 LinearVelocity
		{
			get { return this.linearVel; }
			set
			{
				if (this.body != null) this.body.LinearVelocity = PhysicsUnit.VelocityToPhysical * value;
				this.linearVel = value;
			}
		}
		/// <summary>
		/// [GET / SET] The Colliders current angular velocity.
		/// </summary>
		[EditorHintIncrement(MathF.RadAngle1 * 0.1f)]
		[EditorHintDecimalPlaces(3)]
		public float AngularVelocity
		{
			get { return this.angularVel; }
			set
			{
				if (this.body != null) this.body.AngularVelocity = PhysicsUnit.AngularVelocityToPhysical * value;
				this.angularVel = value;
			}
		}
		/// <summary>
		/// [GET / SET] The bodies overall friction value. Usually a value between 0.0 and 1.0, but higher values can be used to indicate unusually strong friction.
		/// </summary>
		[EditorHintIncrement(0.05f)]
		[EditorHintRange(0.0f, 10000.0f, 0.0f, 1.0f)]
		public float Friction
		{
			get { return this.shapes == null || this.shapes.Count == 0 ? 0.0f : this.shapes.Average(s => s.Friction); }
			set
			{
				if (this.shapes != null)
				{
					foreach (var s in this.shapes)
						s.Friction = value;
				}
			}
		}
		/// <summary>
		/// [GET / SET] The bodies overall restitution value. Should be a value between 0.0 and 1.0.
		/// </summary>
		[EditorHintIncrement(0.05f)]
		[EditorHintRange(0.0f, 1.0f)]
		public float Restitution
		{
			get { return this.shapes == null || this.shapes.Count == 0 ? 0.0f : this.shapes.Average(s => s.Restitution); }
			set
			{
				if (this.shapes != null)
				{
					foreach (var s in this.shapes)
						s.Restitution = value;
				}
			}
		}
		/// <summary>
		/// [GET / SET] The bodies overall mass. This is usually calculated automatically. You may however
		/// assign an explicit, fixed value to override the automatically calculated mass. To reset to
		/// automated calculation, set to zero.
		/// </summary>
		public float Mass
		{
			get 
			{
				if (this.explicitMass <= 0.0f && this.body != null)
					return PhysicsUnit.MassToDuality * this.body.Mass;
				else
					return this.explicitMass;
			}
			set
			{
				this.explicitMass = value;
				this.UpdateBodyMass();
			}
		}
		/// <summary>
		/// [GET / SET] A bitmask that specifies the collision categories to which this Collider belongs.
		/// </summary>
		public CollisionCategory CollisionCategory
		{
			get { return this.colCat; }
			set
			{
				this.colCat = value;
				if (this.body != null) this.body.CollisionCategories = (Category)value;
			}
		}
		/// <summary>
		/// [GET / SET] A bitmask that specifies which collision categories this Collider interacts with.
		/// </summary>
		public CollisionCategory CollidesWith
		{
			get { return this.colWith; }
			set
			{
				this.colWith = value;
				if (this.body != null) this.body.CollidesWith = (Category)value;
			}
		}
		/// <summary>
		/// [GET] The bodies total number of revolutions.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public float Revolutions
		{
			get { return this.revolutions; }
		}
		/// <summary>
		/// [GET] The bodies center of mass in world coordinates.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public Vector2 WorldMassCenter
		{
			get
			{
				return this.body != null ? 
					PhysicsUnit.LengthToDuality * this.body.WorldCenter : 
					this.GameObj.Transform.Pos.Xy;
			}
		}
		/// <summary>
		/// [GET] The bodies center of mass in local coordinates.
		/// </summary>
		public Vector2 LocalMassCenter
		{
			get
			{
				// Need to apply scale to make it actual Transform-local coordinates
				// instead of RigidBody-local coordinates.
				return (this.body != null ? 
					PhysicsUnit.LengthToDuality * this.body.LocalCenter : 
					Vector2.Zero) / this.gameobj.Transform.Scale;
			}
		}
		/// <summary>
		/// [GET] Enumerates all <see cref="ShapeInfo">primitive shapes</see> which this body consists of.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public IEnumerable<ShapeInfo> Shapes
		{
			get { return this.shapes; }
			set { this.SetShapes(value); }
		}
		/// <summary>
		/// [GET] Enumerates all <see cref="JointInfo">joints</see> that are connected to this Collider.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public IEnumerable<JointInfo> Joints
		{
			get { return this.joints; }
			set { this.SetJoints(value); }
		}
		/// <summary>
		/// [GET] The physical bodys bounding radius.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public float BoundRadius
		{
			get
			{
				if (this.shapes == null || this.shapes.Count == 0) return 0.0f;

				Rect boundRect = this.shapes[0].AABB;
				foreach (ShapeInfo info in this.shapes.Skip(1))
					boundRect = boundRect.ExpandedToContain(info.AABB);

				float scale = this.GameObj.Transform.Scale;
				return boundRect.Transformed(scale, scale).BoundingRadius;
			}
		}
		/// <summary>
		/// [GET] Whether the body is currently awake i.e. actively simulated.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public bool IsAwake
		{
			get { return this.body != null && this.body.Awake; }
		}
		internal bool IsFlaggedForSync
		{
			get { return this.schedUpdateBody; }
		}


		/// <summary>
		/// Adds a new shape to the body.
		/// </summary>
		/// <param name="shape"></param>
		public void AddShape(ShapeInfo shape)
		{
			if (shape == null) throw new ArgumentNullException("shape");
			if (this.shapes != null && this.shapes.Contains(shape)) return;

			if (shape.Parent != null && shape.Parent != this)
				shape.Parent.RemoveShape(shape);

			if (this.shapes == null) this.shapes = new List<ShapeInfo>();
			this.shapes.Add(shape);
			shape.Parent = this;

			this.FlagBodyShape();
		}
		/// <summary>
		/// Removes an existing shape from the body.
		/// </summary>
		/// <param name="shape"></param>
		public void RemoveShape(ShapeInfo shape)
		{
			if (shape == null) throw new ArgumentNullException("shape");
			if (shape.Parent != this) return;
			if (this.shapes == null || !this.shapes.Contains(shape)) return;

			this.shapes.Remove(shape);
			shape.Parent = null;

			if (this.body != null) shape.DestroyFixture(this.body, false);
		}
		/// <summary>
		/// Removes all existing shapes from the body.
		/// </summary>
		public void ClearShapes()
		{
			if (this.shapes == null) return;
			foreach (ShapeInfo shape in this.shapes)
			{
				if (this.body != null) shape.DestroyFixture(this.body, false);
				shape.Parent = null;
			}
			this.shapes.Clear();
		}
		private void SetShapes(IEnumerable<ShapeInfo> shapes)
		{
			if (shapes == this.shapes) return;
			this.ClearShapes();

			if (shapes == null) return;
			foreach (ShapeInfo shape in shapes)
				this.AddShape(shape);
		}
		
		/// <summary>
		/// Removes an existing joint from the body.
		/// </summary>
		/// <param name="joint"></param>
		public void RemoveJoint(JointInfo joint)
		{
			if (joint == null) throw new ArgumentNullException("joint");
			if (joint.ParentBody != this) return;

			this.joints.Remove(joint);
			this.AwakeBody();

			if (joint.OtherBody != null)
				joint.OtherBody.AwakeBody();

			joint.ParentBody = null;
			joint.OtherBody = null;
			joint.DestroyJoint();
		}
		/// <summary>
		/// Adds a new joint to the body.
		/// </summary>
		/// <param name="joint"></param>
		public void AddJoint(JointInfo joint, RigidBody other = null)
		{
			if (joint == null) throw new ArgumentNullException("joint");

			if (joint.ParentBody != null)
				joint.ParentBody.RemoveJoint(joint);

			joint.ParentBody = this;
			joint.OtherBody = other;

			if (this.joints == null) this.joints = new List<JointInfo>();
			this.joints.Add(joint);
			this.AwakeBody();

			if (joint.OtherBody != null)
				joint.OtherBody.AwakeBody();

			joint.UpdateJoint();
		}
		/// <summary>
		/// Removes all existing joints from the body.
		/// </summary>
		public void ClearJoints()
		{
			if (this.joints == null) return;
			while (this.joints.Count > 0) this.RemoveJoint(this.joints[0]);
		}
		private void SetJoints(IEnumerable<JointInfo> targetJoints)
		{
			JointInfo[] targetArray = targetJoints != null ? targetJoints.ToArray() : null;
	
			// Remove joints that are not in the new collection
			if (this.joints != null)
			{
				for (int i = this.joints.Count - 1; i >= 0; i--)
				{
					if (targetArray != null && targetArray.Contains(this.joints[i])) continue;
					this.RemoveJoint(this.joints[i]);
				}
			}

			// Add joints that are not in the old collection
			if (targetArray != null)
			{
				for (int i = 0; i < targetArray.Length; i++)
				{
					if (this.joints != null && this.joints.Contains(targetArray[i])) continue;
					JointInfo joint = targetArray[i];
					if (joint.ParentBody != this)
						this.AddJoint(joint, joint.OtherBody);
				}
			}
		}

		/// <summary>
		/// Applies a Transform-local angular impulse to the object. You don't usually need to apply <see cref="Time.TimeMult"/> here because it is inteded to be a one-time force impact.
		/// </summary>
		/// <param name="angularImpulse"></param>
		public void ApplyLocalImpulse(float angularImpulse)
		{
			MathF.CheckValidValue(angularImpulse);
			if (this.body == null) return;

			this.body.ApplyAngularImpulse(PhysicsUnit.AngularImpulseToPhysical * angularImpulse);
			this.angularVel = PhysicsUnit.AngularVelocityToDuality * this.body.AngularVelocity;
		}
		/// <summary>
		/// Applies a Transform-local impulse to the objects mass center. You don't usually need to apply <see cref="Time.TimeMult"/> here because it is inteded to be a one-time force impact.
		/// </summary>
		/// <param name="impulse"></param>
		public void ApplyLocalImpulse(Vector2 impulse)
		{
			MathF.CheckValidValue(impulse);
			if (this.body == null) return;

			impulse = this.gameobj.Transform.GetWorldVector(impulse);

			this.body.ApplyLinearImpulse(PhysicsUnit.ImpulseToPhysical * impulse);
			this.linearVel = PhysicsUnit.VelocityToDuality * this.body.LinearVelocity;
		}
		/// <summary>
		/// Applies a Transform-local impulse to the specified point. You don't usually need to apply <see cref="Time.TimeMult"/> here because it is inteded to be a one-time force impact.
		/// </summary>
		/// <param name="impulse"></param>
		/// <param name="applyAt"></param>
		public void ApplyLocalImpulse(Vector2 impulse, Vector2 applyAt)
		{
			MathF.CheckValidValue(impulse);
			MathF.CheckValidValue(applyAt);
			if (this.body == null) return;

			impulse = this.gameobj.Transform.GetWorldVector(impulse);
			applyAt = this.gameobj.Transform.GetWorldPoint(applyAt);

			this.body.ApplyLinearImpulse(
				PhysicsUnit.ImpulseToPhysical * impulse, 
				PhysicsUnit.LengthToPhysical * applyAt);
			this.linearVel = PhysicsUnit.VelocityToDuality * this.body.LinearVelocity;
			this.angularVel = PhysicsUnit.AngularVelocityToDuality * this.body.AngularVelocity;
		}
		/// <summary>
		/// Applies a world impulse to the objects mass center. You don't usually need to apply <see cref="Time.TimeMult"/> here because it is inteded to be a one-time force impact.
		/// </summary>
		/// <param name="impulse"></param>
		public void ApplyWorldImpulse(Vector2 impulse)
		{
			MathF.CheckValidValue(impulse);
			if (this.body == null) return;

			this.body.ApplyLinearImpulse(PhysicsUnit.ImpulseToPhysical * impulse);
			this.linearVel = PhysicsUnit.VelocityToDuality * this.body.LinearVelocity;
		}
		/// <summary>
		/// Applies a world impulse to the specified world point. You don't usually need to apply <see cref="Time.TimeMult"/> here because it is inteded to be a one-time force impact.
		/// </summary>
		/// <param name="impulse"></param>
		/// <param name="applyAt"></param>
		public void ApplyWorldImpulse(Vector2 impulse, Vector2 applyAt)
		{
			MathF.CheckValidValue(impulse);
			MathF.CheckValidValue(applyAt);
			if (this.body == null) return;

			this.body.ApplyLinearImpulse(
				PhysicsUnit.ImpulseToPhysical * impulse, 
				PhysicsUnit.LengthToPhysical * applyAt);
			this.linearVel = PhysicsUnit.VelocityToDuality * this.body.LinearVelocity;
			this.angularVel = PhysicsUnit.AngularVelocityToDuality * this.body.AngularVelocity;
		}
		
		/// <summary>
		/// Applies a Transform-local angular force to the object. You don't need to apply <see cref="Time.TimeMult"/> here, the physics simulation takes care of this.
		/// </summary>
		/// <param name="angularForce"></param>
		public void ApplyLocalForce(float angularForce)
		{
			MathF.CheckValidValue(angularForce);
			if (this.body == null) return;

			if (Scene.PhysicsFixedTime) angularForce *= Time.TimeMult;
			this.body.ApplyTorque(PhysicsUnit.TorqueToPhysical * angularForce);
		}
		/// <summary>
		/// Applies a Transform-local force to the objects mass center. You don't need to apply <see cref="Time.TimeMult"/> here, the physics simulation takes care of this.
		/// </summary>
		/// <param name="force"></param>
		public void ApplyLocalForce(Vector2 force)
		{
			MathF.CheckValidValue(force);
			if (this.body == null) return;

			force = this.gameobj.Transform.GetWorldVector(force);
			if (Scene.PhysicsFixedTime) force *= Time.TimeMult;
			this.body.ApplyForce(PhysicsUnit.ForceToPhysical * force);
		}
		/// <summary>
		/// Applies a Transform-local force to the specified local point. You don't need to apply <see cref="Time.TimeMult"/> here, the physics simulation takes care of this.
		/// </summary>
		/// <param name="force"></param>
		/// <param name="applyAt"></param>
		public void ApplyLocalForce(Vector2 force, Vector2 applyAt)
		{
			MathF.CheckValidValue(force);
			MathF.CheckValidValue(applyAt);
			if (this.body == null) return;

			force = this.gameobj.Transform.GetWorldVector(force);
			applyAt = this.gameobj.Transform.GetWorldPoint(applyAt);
			if (Scene.PhysicsFixedTime) force *= Time.TimeMult;
			this.body.ApplyForce(
				PhysicsUnit.ForceToPhysical * force, 
				PhysicsUnit.LengthToPhysical * applyAt);
		}
		/// <summary>
		/// Applies a world force to the objects mass center. You don't need to apply <see cref="Time.TimeMult"/> here, the physics simulation takes care of this.
		/// </summary>
		/// <param name="force"></param>
		public void ApplyWorldForce(Vector2 force)
		{
			MathF.CheckValidValue(force);
			if (this.body == null) return;

			if (Scene.PhysicsFixedTime) force *= Time.TimeMult;
			this.body.ApplyForce(PhysicsUnit.ForceToPhysical * force);
		}
		/// <summary>
		/// Applies a world force to the specified world point. You don't need to apply <see cref="Time.TimeMult"/> here, the physics simulation takes care of this.
		/// </summary>
		/// <param name="force"></param>
		/// <param name="applyAt"></param>
		public void ApplyWorldForce(Vector2 force, Vector2 applyAt)
		{
			MathF.CheckValidValue(force);
			MathF.CheckValidValue(applyAt);
			if (this.body == null) return;

			if (Scene.PhysicsFixedTime) force *= Time.TimeMult;
			this.body.ApplyForce(
				PhysicsUnit.ForceToPhysical * force, 
				PhysicsUnit.LengthToPhysical * applyAt);
		}

		/// <summary>
		/// Awakes the body if it has been in a resting state that is now being left, such as
		/// when changing physical properties at runtime. You usually don't need to call this.
		/// </summary>
		public void AwakeBody()
		{
			if (this.body != null) this.body.Awake = true;
		}

		/// <summary>
		/// Performs a physical picking operation and returns the <see cref="ShapeInfo">shape</see> in which
		/// the specified world coordinate is located in.
		/// </summary>
		/// <param name="worldCoord"></param>
		/// <returns></returns>
		public ShapeInfo PickShape(Vector2 worldCoord)
		{
			if (this.body == null) return null;

			Vector2 fsWorldCoord = PhysicsUnit.LengthToPhysical * worldCoord;

			for (int i = 0; i < this.body.FixtureList.Count; i++)
			{
				Fixture f = this.body.FixtureList[i];
				if (f.TestPoint(ref fsWorldCoord)) return f.UserData as ShapeInfo;
			}
			return null;
		}
		/// <summary>
		/// Performs a physical picking operation and returns the <see cref="ShapeInfo">shapes</see> that
		/// intersect the specified world coordinate.
		/// </summary>
		/// <param name="worldCoord"></param>
		/// <returns></returns>
		public List<ShapeInfo> PickShapes(Vector2 worldCoord)
		{
			if (this.body == null) return new List<ShapeInfo>();

			List<ShapeInfo> picked = new List<ShapeInfo>();
			Vector2 fsWorldCoord = PhysicsUnit.LengthToPhysical * worldCoord;

			for (int i = 0; i < this.body.FixtureList.Count; i++)
			{
				Fixture f = this.body.FixtureList[i];
				if (f.TestPoint(ref fsWorldCoord)) picked.Add(f.UserData as ShapeInfo);
			}

			return picked;
		}
		/// <summary>
		/// Performs a physical picking operation and returns the <see cref="ShapeInfo">shapes</see> that
		/// intersect the specified world coordinate area.
		/// </summary>
		/// <param name="worldCoord"></param>
		/// <param name="size"></param>
		/// <returns></returns>
		public List<ShapeInfo> PickShapes(Vector2 worldCoord, Vector2 size)
		{
			if (this.body == null) return new List<ShapeInfo>();

			Vector2 fsWorldCoord = PhysicsUnit.LengthToPhysical * worldCoord;
			FarseerPhysics.Collision.AABB fsWorldAABB = new FarseerPhysics.Collision.AABB(fsWorldCoord, PhysicsUnit.LengthToPhysical * (worldCoord + size));

			List<ShapeInfo> picked = new List<ShapeInfo>();
			for (int i = 0; i < this.body.FixtureList.Count; i++)
			{
				Fixture f = this.body.FixtureList[i];
				ShapeInfo s = f.UserData as ShapeInfo;

				FarseerPhysics.Collision.AABB fAABB;
				FarseerPhysics.Common.Transform transform;
				this.body.GetTransform(out transform);
				f.Shape.ComputeAABB(out fAABB, ref transform, 0);
				
				if (fsWorldAABB.Contains(ref fAABB))
				{
					picked.Add(s);
					continue;
				}
				else if (!FarseerPhysics.Collision.AABB.TestOverlap(ref fsWorldAABB, ref fAABB))
					continue;

				FarseerPhysics.Collision.AABB fAABBIntersect;
				fAABBIntersect.LowerBound = Vector2.Max(fAABB.LowerBound, fsWorldAABB.LowerBound);
				fAABBIntersect.UpperBound = Vector2.Min(fAABB.UpperBound, fsWorldAABB.UpperBound);

				Vector2 fsWorldCoordStep = PhysicsUnit.LengthToPhysical * (new Vector2(MathF.Max(s.AABB.W, 1.0f), MathF.Max(s.AABB.H, 1.0f)) * 0.05f);
				Vector2 fsTemp = fAABBIntersect.LowerBound;
				do
				{
					if (f.TestPoint(ref fsTemp))
					{
						picked.Add(s);
						break;
					}

					fsTemp.X += fsWorldCoordStep.X;
					if (fsTemp.X > fAABBIntersect.UpperBound.X)
					{
						fsTemp.X = fAABBIntersect.LowerBound.X;
						fsTemp.Y += fsWorldCoordStep.Y;
					}
					if (fsTemp.Y > fAABBIntersect.UpperBound.Y) break;
				} while (true);
			}

			return picked;
		}
		
		
		internal bool FlagBodyShape()
		{
			if (this.body == null) return false;
			if (this.isUpdatingBody) return false;

			this.schedUpdateBody = true;

			return true;
		}
		/// <summary>
		/// Forces previously scheduled body shape updates to execute. Changes to a RigidBodies shape
		/// are normally cached and executed in the following frame. Calling this method guarantes all
		/// scheduled updates to be performed immediately.
		/// </summary>
		public void SynchronizeBodyShape()
		{
			if (!this.schedUpdateBody) return;
			bool wasEnabled = this.body != null && this.body.Enabled;
			if (wasEnabled) this.body.Enabled = false;

			this.UpdateBodyShape();

			if (wasEnabled) this.body.Enabled = true;
			this.schedUpdateBody = false;
		}
		/// <summary>
		/// Prepares this RigidBody for a large-scale shape update. This isn't required but might boost update performance.
		/// </summary>
		public void BeginUpdateBodyShape()
		{
			if (this.body == null) return;
			this.body.Enabled = false;
		}
		/// <summary>
		/// Restores this RigidBody after a large-scale shape update. See <see cref="BeginUpdateBodyShape"/>.
		/// </summary>
		public void EndUpdateBodyShape()
		{
			if (this.body == null) return;
			this.body.Enabled = true;
		}
		private void UpdateBodyShape()
		{
			if (this.body == null) return;
			this.isUpdatingBody = true;

			this.lastScale = this.gameobj.Transform.Scale;
			if (this.shapes != null)
			{
				foreach (ShapeInfo info in this.shapes) info.UpdateFixture();
			}
			this.body.CollisionCategories = (Category)this.colCat;
			this.body.CollidesWith = (Category)this.colWith;
			this.UpdateBodyMass();

			this.AwakeBody();
			this.isUpdatingBody = false;
		}
		private void UpdateBodyMass()
		{
			if (this.body == null) return;

			this.body.ResetMassData();
			if (this.explicitMass > 0.0f)
				this.body.Mass = PhysicsUnit.MassToPhysical * this.explicitMass;
		}

		private void CleanupBody()
		{
			if (this.body == null) return;
			
			if (this.shapes != null)
			{
				foreach (ShapeInfo info in this.shapes)
					info.DestroyFixture(this.body, true);
			}
			
			this.body.Collision -= this.body_OnCollision;
			this.body.Separation -= this.body_OnSeparation;
			this.body.PostSolve -= this.body_PostSolve;

			// Manually generate OnSeparation events directy in-place, since 
			// we won't receive next frames Farseer events anymore
			ContactEdge edge = this.body.ContactList;
			while (edge != null)
			{
				if (edge.Contact != null && edge.Contact.IsTouching())
				{
					Fixture fixtureA = edge.Contact.FixtureA;
					Fixture fixtureB = edge.Contact.FixtureB;
					if (fixtureA != null && fixtureA.Body != null && fixtureA.Body.UserData == this)
						this.eventBuffer.Add(new ColEvent(ColEvent.EventType.Separation, fixtureA, fixtureB, null)); 
					else if (fixtureB != null && fixtureB.Body != null && fixtureB.Body.UserData == this)
						this.eventBuffer.Add(new ColEvent(ColEvent.EventType.Separation, fixtureB, fixtureA, null)); 
				}
				edge = edge.Next;
			}

			this.body.Dispose();
			this.body = null;
		}
		private void InitBody()
		{
			if (this.body != null) this.CleanupBody();
			Transform t = this.GameObj != null ? this.GameObj.Transform : null;

			this.body = new Body(Scene.PhysicsWorld, this);
			this.body.BodyType = (this.bodyType == BodyType.Static ? FarseerPhysics.Dynamics.BodyType.Static : FarseerPhysics.Dynamics.BodyType.Dynamic);
			this.body.LinearDamping = this.linearDamp;
			this.body.AngularDamping = this.angularDamp;
			this.body.FixedRotation = this.fixedAngle;
			this.body.IgnoreGravity = this.ignoreGravity;
			this.body.IsBullet = this.continous;
			this.body.CollisionCategories = (Category)this.colCat;
			this.body.CollidesWith = (Category)this.colWith;

			this.UpdateBodyShape();

			if (t != null)
			{
				this.body.SetTransform(PhysicsUnit.LengthToPhysical * t.Pos.Xy, PhysicsUnit.AngleToPhysical * t.Angle);
				this.body.LinearVelocity = PhysicsUnit.VelocityToPhysical * this.linearVel;
				this.body.AngularVelocity = PhysicsUnit.AngularVelocityToPhysical * this.angularVel;
			}

			this.body.Collision += this.body_OnCollision;
			this.body.Separation += this.body_OnSeparation;
			this.body.PostSolve += this.body_PostSolve;
		}

		private void CleanupJoints()
		{
			if (this.joints == null) return;
			this.RemoveDisposedJoints();
			foreach (JointInfo j in this.joints) j.DestroyJoint();
		}
		private void RemoveDisposedJoints()
		{
			if (this.joints == null) return;
			for (int i = this.joints.Count - 1; i >= 0; i--)
			{
				JointInfo joint = this.joints[i];
				if (this.Disposed || (joint.OtherBody != null && (joint.OtherBody.Disposed || joint.OtherBody.GameObj.Disposed)))
					this.RemoveJoint(joint);
			}
		}

		internal void PrepareForJoint()
		{
			this.Initialize();
		}
		private void Initialize()
		{
			if (this.bodyInitState != InitState.Disposed) return;
			this.bodyInitState = InitState.Initializing;

			// Register for tranformation changes to keep the RigidBody in sync. Make sure to register only once.
			this.GameObj.Transform.EventTransformChanged -= this.OnTransformChanged;
			this.GameObj.Transform.EventTransformChanged += this.OnTransformChanged;

			// Initialize body and joints
			this.InitBody();
			if (this.joints != null)
			{
				foreach (JointInfo info in this.joints) info.UpdateJoint();
			}

			this.bodyInitState = InitState.Initialized;
		}
		private void Shutdown()
		{
			if (this.bodyInitState != InitState.Initialized) return;
			this.bodyInitState = InitState.Disposing;
			
			// Clean up body and joints
			this.CleanupJoints();
			this.CleanupBody();

			// Unregister for transformation change events.
			this.GameObj.Transform.EventTransformChanged -= this.OnTransformChanged;

			// Finally process all collision events we didn't get around to yet.
			this.ProcessCollisionEvents();

			this.bodyInitState = InitState.Disposed;
		}

		private bool body_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
		{
			this.eventBuffer.Add(new ColEvent(ColEvent.EventType.Collision, fixtureA, fixtureB, null));
			return true;
		}
		private void body_OnSeparation(Fixture fixtureA, Fixture fixtureB)
		{
			this.eventBuffer.Add(new ColEvent(ColEvent.EventType.Separation, fixtureA, fixtureB, null));
		}
		private void body_PostSolve(Contact contact, ContactConstraint impulse)
		{
			int count = contact.Manifold.PointCount;
			for (int i = 0; i < count; ++i)
			{
				if (impulse.Points[i].NormalImpulse != 0.0f || impulse.Points[i].TangentImpulse != 0.0f)
				{
					CollisionData colData = new CollisionData(this.body, impulse, i);
					if (contact.FixtureA.Body == this.body)
						this.eventBuffer.Add(new ColEvent(ColEvent.EventType.PostSolve, contact.FixtureA, contact.FixtureB, colData));
					else
						this.eventBuffer.Add(new ColEvent(ColEvent.EventType.PostSolve, contact.FixtureB, contact.FixtureA, colData));
				}
			}
		}
		private void ProcessCollisionEvents()
		{
			if (this.isProcessingEvents) return;
			this.isProcessingEvents = true;
			{
				// Don't use foreach here in case someone decides to add something at the end while iterating..
				for (int i = 0; i < this.eventBuffer.Count; i++)
				{
					this.ProcessSingleCollisionEvent(this.eventBuffer[i]);
				}
				this.eventBuffer.Clear();
			}
			this.isProcessingEvents = false;
		}
		private void ProcessSingleCollisionEvent(ColEvent colEvent)
		{
			// Ignore disposed fixtures / bodies
			if (colEvent.FixtureA.Body == null) return;
			if (colEvent.FixtureB.Body == null) return;

			Component otherComponent = colEvent.FixtureB.Body.UserData as Component;
			GameObject otherObject = otherComponent != null ? otherComponent.GameObj : colEvent.FixtureB.Body.UserData as GameObject;

			RigidBodyCollisionEventArgs args = new RigidBodyCollisionEventArgs(
				otherObject,
				colEvent.Data,
				colEvent.FixtureA.UserData as ShapeInfo,
				colEvent.FixtureB.UserData as ShapeInfo);

			if (colEvent.Type == ColEvent.EventType.Collision)
				this.NotifyCollisionBegin(args);
			else if (colEvent.Type == ColEvent.EventType.Separation)
				this.NotifyCollisionEnd(args);
			else if (colEvent.Type == ColEvent.EventType.PostSolve)
				this.NotifyCollisionSolve(args);
		}
		
		private void NotifyCollisionBegin(CollisionEventArgs args)
		{
			this.gameobj.IterateComponents<ICmpCollisionListener>(
				l => l.OnCollisionBegin(this, args), 
				l => (l as Component).Active);
		}
		private void NotifyCollisionEnd(CollisionEventArgs args)
		{
			this.gameobj.IterateComponents<ICmpCollisionListener>(
				l => l.OnCollisionEnd(this, args), 
				l => (l as Component).Active);
		}
		private void NotifyCollisionSolve(CollisionEventArgs args)
		{
			this.gameobj.IterateComponents<ICmpCollisionListener>(
				l => l.OnCollisionSolve(this, args), 
				l => (l as Component).Active);
		}

		void ICmpUpdatable.OnUpdate()
		{
			this.CheckValidTransform();

			// Synchronize physical body / perform shape updates, etc.
			this.RemoveDisposedJoints();
			this.SynchronizeBodyShape();

			// Update velocity and transform values
			if (this.body != null)
			{
				this.linearVel = PhysicsUnit.VelocityToDuality * this.body.LinearVelocity;
				this.angularVel = PhysicsUnit.AngularVelocityToDuality * this.body.AngularVelocity;
				this.revolutions = this.body.Revolutions;
				Transform t = this.gameobj.Transform;
				if (this.bodyType == BodyType.Dynamic)
				{
					// Make sure we're not overwriting any previously occuring changes
					t.CommitChanges();

					// The current PhysicsAlpha interpolation probably isn't the best one. Maybe replace later.
					Vector2 bodyVel = this.body.LinearVelocity;
					Vector2 bodyPos = this.body.Position - bodyVel * (1.0f - Scene.PhysicsAlpha) * Time.SPFMult;
					float bodyAngleVel = this.body.AngularVelocity;
					float bodyAngle = this.body.Rotation - bodyAngleVel * (1.0f - Scene.PhysicsAlpha) * Time.SPFMult;
					t.IgnoreParent = true; // Force ignore parent!
					t.MoveToAbs(new Vector3(
						PhysicsUnit.LengthToDuality * bodyPos.X, 
						PhysicsUnit.LengthToDuality * bodyPos.Y, 
						t.Pos.Z));
					t.TurnToAbs(bodyAngle);
					t.CommitChanges(this);
				}
			}

			// Process events
			this.ProcessCollisionEvents();

			this.CheckValidTransform();
		}
		void ICmpEditorUpdatable.OnUpdate()
		{
			this.CheckValidTransform();

			// Synchronize physical body / perform shape updates, etc.
			this.RemoveDisposedJoints();
			this.SynchronizeBodyShape();

			this.CheckValidTransform();
		}

		private void OnTransformChanged(object sender, TransformChangedEventArgs e)
		{
			// Don't react to events triggered by this Component, or while no physics body is available
			if (sender == this) return;
			if (this.body == null) return;

			// Apply transform changes to the physics body
			Transform t = e.Component as Transform;
			if ((e.Changes & Transform.DirtyFlags.Pos) != Transform.DirtyFlags.None)
			{
				this.body.Position = PhysicsUnit.LengthToPhysical * t.Pos.Xy;
			}
			if ((e.Changes & Transform.DirtyFlags.Angle) != Transform.DirtyFlags.None)
			{
				this.body.Rotation = t.Angle;
			}
			if ((e.Changes & Transform.DirtyFlags.Scale) != Transform.DirtyFlags.None)
			{
				float scale = t.Scale;
				if (scale == 0.0f || this.lastScale == 0.0f)
				{
					this.FlagBodyShape();
				}
				else
				{
					const float pixelLimit = 2;
					float boundRadius = this.BoundRadius;
					float upper = (boundRadius + pixelLimit) / boundRadius;
					float lower = (boundRadius - pixelLimit) / boundRadius;
					if (scale / this.lastScale >= upper || scale / this.lastScale <= lower)
					{
						this.FlagBodyShape();
					}
				}
			}

			// Make sure we're simulating this body, if something has changed
			if (e.Changes != Transform.DirtyFlags.None)
			{
				this.body.Awake = true;
			}
		}
		void ICmpInitializable.OnInit(InitContext context)
		{
			if (context == InitContext.Activate)
			{
				// Do some cleanup before updating again
				this.RemoveDisposedJoints();
				// Initialize the backing Farseer objects upon activation
				this.Initialize();
			}
			else if (context == InitContext.AddToGameObject && DualityApp.ExecContext == DualityApp.ExecutionContext.Editor)
			{
				// Add a default shape when creating a new RigidBody in the editor
				this.AddShape(new CircleShapeInfo(128.0f, Vector2.Zero, 1.0f));
			}
		}
		void ICmpInitializable.OnShutdown(ShutdownContext context)
		{
			if (context == ShutdownContext.Deactivate)
				this.Shutdown();
			else if (context == ShutdownContext.Saving)
				this.RemoveDisposedJoints();
		}

		protected override void OnSetupCloneTargets(object targetObj, ICloneTargetSetup setup)
		{
			base.OnSetupCloneTargets(targetObj, setup);
			RigidBody target = targetObj as RigidBody;

			// Handle ownership of shapes and joints
			if (this.shapes != null)
			{
				for (int i = 0; i < this.shapes.Count; i++)
				{
					ShapeInfo sourceShape = this.shapes[i];
					ShapeInfo targetShape = (target.shapes != null && target.shapes.Count > i) ? target.shapes[i] : null;
					setup.HandleObject(sourceShape, targetShape, CloneBehavior.ChildObject);
				}
			}
			if (this.joints != null)
			{
				for (int i = 0; i < this.joints.Count; i++)
				{
					JointInfo sourceJoint = this.joints[i];
					JointInfo targetJoint = (target.joints != null && target.joints.Count > i) ? target.joints[i] : null;
					setup.HandleObject(sourceJoint, targetJoint, CloneBehavior.ChildObject);
				}
			}
		}
		protected override void OnCopyDataTo(object targetObj, ICloneOperation operation)
		{
			base.OnCopyDataTo(targetObj, operation);
			RigidBody target = targetObj as RigidBody;
			
			// If we're cloning an initialized RigidBody, shut down the targets physics body.
			bool wasInitialized = target.bodyInitState == InitState.Initialized;
			if (wasInitialized) target.Shutdown();

			target.bodyType			= this.bodyType;
			target.linearDamp		= this.linearDamp;
			target.angularDamp		= this.angularDamp;
			target.fixedAngle		= this.fixedAngle;
			target.ignoreGravity	= this.ignoreGravity;
			target.continous		= this.continous;
			target.linearVel		= this.linearVel;
			target.angularVel		= this.angularVel;
			target.revolutions		= this.revolutions;
			target.explicitMass		= this.explicitMass;
			target.colCat			= this.colCat;
			target.colWith			= this.colWith;

			if (this.shapes != null)
			{
				if (target.shapes == null)
				{
					target.shapes = new List<ShapeInfo>(this.shapes.Count);
				}
				else if (target.shapes.Count > this.shapes.Count)
				{
					// Remove exceeding shapes
					for (int i = target.shapes.Count - 1; i >= this.shapes.Count; i--)
					{
						target.RemoveShape(target.shapes[i]);
					}
				}

				// Synchronize existing shapes
				for (int i = 0; i < this.shapes.Count; i++)
				{
					bool isNew = (target.shapes.Count <= i);
					ShapeInfo sourceShape = this.shapes[i];
					ShapeInfo targetShape = !isNew ? target.shapes[i] : null;
					if (operation.HandleObject(sourceShape, ref targetShape))
					{
						if (isNew)
							target.shapes.Add(targetShape);
						else
							target.shapes[i] = targetShape;
					}
				}
			}
			else
			{
				target.ClearShapes();
			}

			if (this.joints != null)
			{
				if (target.joints == null)
				{
					target.joints = new List<JointInfo>(this.joints.Count);
				}
				else if (target.joints.Count > this.joints.Count)
				{
					// Remove exceeding joints
					for (int i = target.joints.Count - 1; i >= this.joints.Count; i--)
					{
						target.RemoveJoint(target.joints[i]);
					}
				}

				// Synchronize existing joints
				for (int i = 0; i < this.joints.Count; i++)
				{
					bool isNew = (target.joints.Count <= i);
					JointInfo sourceJoint = this.joints[i];
					JointInfo targetJoint = !isNew ? target.joints[i] : null;
					if (operation.HandleObject(sourceJoint, ref targetJoint))
					{
						if (isNew)
							target.joints.Add(targetJoint);
						else
							target.joints[i] = targetJoint;
					}
				}
			}
			else
			{
				target.ClearJoints();
			}

			// Make sure to re-initialize the targets body, if it was shut down
			if (wasInitialized) target.Initialize();
		}
		
		/// <summary>
		/// Performs a 2d physical raycast in world coordinates.
		/// </summary>
		/// <param name="start">The starting point in world coordinates.</param>
		/// <param name="end">The desired end point in world coordinates.</param>
		/// <param name="callback">
		/// The callback that is invoked for each hit on the raycast. Note that the order in which each hit occurs isn't deterministic
		/// and may appear random. Return -1 to ignore the curret shape, 0 to terminate the raycast, data.Fraction to clip the ray for current hit, or 1 to continue.
		/// </param>
		public static void RayCast(Vector2 start, Vector2 end, RayCastCallback callback)
		{
			if (callback == null) callback = Raycast_DefaultCallback;

			Vector2 fsWorldCoordA = PhysicsUnit.LengthToPhysical * start;
			Vector2 fsWorldCoordB = PhysicsUnit.LengthToPhysical * end;

			Scene.PhysicsWorld.RayCast(delegate(Fixture fixture, Vector2 pos, Vector2 normal, float fraction)
			{
				return callback(new RayCastData(
					fixture.UserData as ShapeInfo, 
					PhysicsUnit.LengthToDuality * pos, 
					normal, 
					fraction));
			}, fsWorldCoordA, fsWorldCoordB);
		}
		/// <summary>
		/// Performs a 2d physical raycast in world coordinates.
		/// </summary>
		/// <param name="start">The starting point in world coordinates.</param>
		/// <param name="end">The desired end point in world coordinates.</param>
		/// <param name="callback">
		/// The callback that is invoked for each hit on the raycast. Note that the order in which each hit occurs isn't deterministic
		/// and may appear random. Return -1 to ignore the curret shape, 0 to terminate the raycast, data.Fraction to clip the ray for current hit, or 1 to continue.
		/// </param>
		/// <param name="hits">Returns a list of all occurred hits, ordered by their Fraction value.</param>
		public static void RayCast(Vector2 start, Vector2 end, RayCastCallback callback, out RawList<RayCastData> hits)
		{
			if (callback == null) callback = Raycast_DefaultCallback;

			Vector2 fsWorldCoordA = PhysicsUnit.LengthToPhysical * start;
			Vector2 fsWorldCoordB = PhysicsUnit.LengthToPhysical * end;

			RawList<RayCastData> localHits = new RawList<RayCastData>();
			Scene.PhysicsWorld.RayCast(delegate(Fixture fixture, Vector2 pos, Vector2 normal, float fraction)
			{
				int oldCount = localHits.Count++;
				RayCastData[] data = localHits.Data;

				data[oldCount] = new RayCastData(
					fixture.UserData as ShapeInfo, 
					PhysicsUnit.LengthToDuality * pos, 
					normal, 
					fraction);

				float result = callback(data[oldCount]);
				if (result < 0.0f)
					localHits.Count--;

				return result;
			}, fsWorldCoordA, fsWorldCoordB);
			localHits.Data.StableSort(0, localHits.Count, (d1, d2) => (int)(1000000.0f * (d1.Fraction - d2.Fraction)));

			hits = localHits;
		}
		/// <summary>
		/// Performs a 2d physical raycast in world coordinates.
		/// </summary>
		/// <param name="start">The starting point in world coordinates.</param>
		/// <param name="end">The desired end point in world coordinates.</param>
		/// <param name="callback">
		/// The callback that is invoked for each hit on the raycast. Note that the order in which each hit occurs isn't deterministic
		/// and may appear random. Return -1 to ignore the curret shape, 0 to terminate the raycast, data.Fraction to clip the ray for current hit, or 1 to continue.
		/// </param>
		/// <param name="firstHit">Returns the first hit that occurs, i.e. the one with the highest proximity to the starting point.</param>
		/// <returns>Returns whether anything has been hit.</returns>
		public static bool RayCast(Vector2 start, Vector2 end, RayCastCallback callback, out RayCastData firstHit)
		{
			if (callback == null) callback = Raycast_DefaultCallback;

			Vector2 fsWorldCoordA = PhysicsUnit.LengthToPhysical * start;
			Vector2 fsWorldCoordB = PhysicsUnit.LengthToPhysical * end;

			float firstHitFraction = float.MaxValue;
			RayCastData firstHitLocal = default(RayCastData);

			Scene.PhysicsWorld.RayCast(delegate(Fixture fixture, Vector2 pos, Vector2 normal, float fraction)
			{
				RayCastData data = new RayCastData(
					fixture.UserData as ShapeInfo, 
					PhysicsUnit.LengthToDuality * pos, 
					normal, 
					fraction);

				float result = callback(data);
				if (result >= 0.0f && data.Fraction < firstHitFraction)
				{
					firstHitLocal = data;
					firstHitFraction = data.Fraction;
				}

				return result;
			}, fsWorldCoordA, fsWorldCoordB);

			firstHit = firstHitLocal;
			return firstHitFraction != float.MaxValue;
		}
		private static float Raycast_DefaultCallback(RayCastData data)
		{
			return 1.0f;
		}

		/// <summary>
		/// Performs a global physical picking operation and returns the <see cref="ShapeInfo">shape</see> in which
		/// the specified world coordinate is located in.
		/// </summary>
		/// <param name="worldCoord"></param>
		/// <returns></returns>
		public static ShapeInfo PickShapeGlobal(Vector2 worldCoord)
		{
			Vector2 fsWorldCoord = PhysicsUnit.LengthToPhysical * worldCoord;
			Fixture f = Scene.PhysicsWorld.TestPoint(fsWorldCoord);

			return f != null && f.UserData is ShapeInfo ? (f.UserData as ShapeInfo) : null;
		}
		/// <summary>
		/// Performs a global physical picking operation and returns the <see cref="ShapeInfo">shapes</see> that
		/// intersect the specified world coordinate.
		/// </summary>
		/// <param name="worldCoord"></param>
		/// <returns></returns>
		public static List<ShapeInfo> PickShapesGlobal(Vector2 worldCoord)
		{
			Vector2 fsWorldCoord = PhysicsUnit.LengthToPhysical * worldCoord;
			List<Fixture> fixtureList = Scene.PhysicsWorld.TestPointAll(fsWorldCoord);
			return new List<ShapeInfo>(fixtureList.Where(f => f != null && f.UserData is ShapeInfo).Select(f => f.UserData as ShapeInfo));
		}
		/// <summary>
		/// Performs a global physical picking operation and returns the <see cref="ShapeInfo">shapes</see> that
		/// intersect the specified world coordinate area.
		/// </summary>
		/// <param name="worldCoord"></param>
		/// <param name="size"></param>
		/// <returns></returns>
		public static List<ShapeInfo> PickShapesGlobal(Vector2 worldCoord, Vector2 size)
		{
			List<ShapeInfo> picked = new List<ShapeInfo>();
			List<RigidBody> potentialBodies = QueryRectGlobal(worldCoord, size);

			foreach (RigidBody c in potentialBodies)
				picked.AddRange(c.PickShapes(worldCoord, size));

			return picked;
		}
		/// <summary>
		/// Performs a global physical AABB query and returns the <see cref="RigidBody">bodies</see> that
		/// might be roughly contained or intersected by the specified region.
		/// </summary>
		/// <param name="worldCoord"></param>
		/// <param name="size"></param>
		/// <returns></returns>
		public static List<RigidBody> QueryRectGlobal(Vector2 worldCoord, Vector2 size)
		{
			List<RigidBody> bodies = new List<RigidBody>();
			
			Vector2 fsWorldCoord = PhysicsUnit.LengthToPhysical * worldCoord;
			FarseerPhysics.Collision.AABB fsWorldAABB = new FarseerPhysics.Collision.AABB(fsWorldCoord, PhysicsUnit.LengthToPhysical * (worldCoord + size));
			Scene.PhysicsWorld.QueryAABB(fixture =>
				{
					ShapeInfo shape = fixture.UserData as ShapeInfo;
					if (shape != null && shape.Parent != null && shape.Parent.Active)
					{
						if (!bodies.Contains(shape.Parent))
							bodies.Add(shape.Parent);
					}
					return true;
				},
				ref fsWorldAABB);

			return bodies;
		}
		/// <summary>
		/// Awakes all currently existing RigidBodies.
		/// </summary>
		public static void AwakeAll()
		{
			Scene.AwakePhysics();
		}


		
		[System.Diagnostics.Conditional("DEBUG")]
		internal void CheckValidTransform()
		{
			if (this.body == null) return;

			MathF.CheckValidValue(this.body.Position.X);
			MathF.CheckValidValue(this.body.Position.Y);
			MathF.CheckValidValue(this.body.Rotation);
			MathF.CheckValidValue(this.body.LinearVelocity.X);
			MathF.CheckValidValue(this.body.LinearVelocity.Y);
			MathF.CheckValidValue(this.body.AngularVelocity);
		}
	}
	
	/// <summary>
	/// The type of a <see cref="RigidBody">Colliders</see> physical body.
	/// </summary>
	public enum BodyType
	{
		/// <summary>
		/// A static body. It will never move due to physical forces.
		/// </summary>
		Static,
		/// <summary>
		/// A dynamic body. Its movement is determined by physical effects.
		/// </summary>
		Dynamic
	}

	/// <summary>
	/// Called for each shape found in the query. You control how the ray cast proceeds by returning a float.
	/// </summary>
	/// <returns>-1 to ignore the curret shape, 0 to terminate the raycast, data.Fraction to clip the ray for current hit, or 1 to continue.</returns>
	public delegate float RayCastCallback(RayCastData data);

	/// <summary>
	///  Provides data about a <see cref="RigidBody.RayCast"/>.
	/// </summary>
	public struct RayCastData
	{
		private ShapeInfo	shape;
		private Vector2		pos;
		private Vector2		normal;
		private float		fraction;

		/// <summary>
		/// [GET] The shape that was hit.
		/// </summary>
		public ShapeInfo Shape
		{
			get { return this.shape; }
		}
		/// <summary>
		/// [GET] The RigidBody that was hit.
		/// </summary>
		public RigidBody Body
		{
			get { return shape != null ? shape.Parent : null; }
		}
		/// <summary>
		/// [GET] The GameObject that was hit.
		/// </summary>
		public GameObject GameObj
		{
			get { return shape != null && shape.Parent != null ? shape.Parent.GameObj : null; }
		}
		/// <summary>
		/// [GET] The world position at which the shape was hit.
		/// </summary>
		public Vector2 Pos
		{
			get { return this.pos; }
		}
		/// <summary>
		/// [GET] The normal of the ray / shape collision.
		/// </summary>
		public Vector2 Normal
		{
			get { return this.normal; }
		}
		/// <summary>
		/// [GET] The fraction (0.0f - 1.0f) of the ray at which the hit occurred.
		/// </summary>
		public float Fraction
		{
			get { return this.fraction; }
		}

		public RayCastData(ShapeInfo shape, Vector2 pos, Vector2 normal, float fraction)
		{
			this.shape = shape;
			this.pos = pos;
			this.normal = normal;
			this.fraction = fraction;
		}
	}

	public class RigidBodyCollisionEventArgs : CollisionEventArgs
	{
		private	ShapeInfo	colShapeA;
		private	ShapeInfo	colShapeB;

		public ShapeInfo MyShape
		{
			get { return this.colShapeA; }
		}
		public ShapeInfo OtherShape
		{
			get { return this.colShapeB; }
		}


		public RigidBodyCollisionEventArgs(GameObject obj, CollisionData data, ShapeInfo shapeA, ShapeInfo shapeB) : base(obj, data)
		{
			this.colShapeA = shapeA;
			this.colShapeB = shapeB;
		}
	}
}
