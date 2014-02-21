using System;
using System.Collections.Generic;
using System.Linq;

using OpenTK;

using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;

using Duality.Editor;
using Duality.Resources;
using Duality.Properties;

namespace Duality.Components.Physics
{
	/// <summary>
	/// Represents a body instance for physical simulation, collision detection and response.
	/// </summary>
	[Serializable]
	[RequiredComponent(typeof(Transform))]
	[EditorHintCategory(typeof(CoreRes), CoreResNames.Category_Physics)]
	public partial class RigidBody : Component, ICmpInitializable, ICmpUpdatable, ICmpEditorUpdatable
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


		private	BodyType	bodyType		= BodyType.Dynamic;
		private	float		linearDamp		= 0.3f;
		private	float		angularDamp		= 0.3f;
		private	bool		fixedAngle		= false;
		private	bool		ignoreGravity	= false;
		private	bool		continous		= false;
		private	Vector2		linearVel		= Vector2.Zero;
		private	float		angularVel		= 0.0f;
		private	float		revolutions		= 0.0f;
		private	float		explicitMass	= 0.0f;
		private	Category	colCat			= Category.Cat1;
		private	Category	colWith			= Category.All;
		private	List<ShapeInfo>	shapes		= null;
		private	List<JointInfo>	joints		= null;

		[NonSerialized]	private	InitState		bodyInitState	= InitState.Disposed;
		[NonSerialized]	private	bool			schedUpdateBody	= false;
		[NonSerialized]	private	bool			isUpdatingBody	= false;
		[NonSerialized]	private	Body			body			= null;
		[NonSerialized]	private	List<ColEvent>	eventBuffer		= new List<ColEvent>();


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
		[EditorHintRange(0.0f, 100.0f)]
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
		[EditorHintRange(0.0f, 100.0f)]
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
				if (this.body != null) this.body.LinearVelocity = PhysicsConvert.ToPhysicalUnit(value) / Time.SPFMult;
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
				if (this.body != null) this.body.AngularVelocity = value / Time.SPFMult;
				this.angularVel = value;
			}
		}
		/// <summary>
		/// [GET / SET] The bodies overall friction value. Usually a value between 0.0 and 1.0, but higher values can be used to indicate unusually strong friction.
		/// </summary>
		[EditorHintIncrement(0.05f)]
		[EditorHintRange(0.0f, 10000.0f)]
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
					return PhysicsConvert.ToDualityUnit(this.body.Mass);
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
		public Category CollisionCategory
		{
			get { return this.colCat; }
			set
			{
				this.colCat = value;
				if (this.body != null) this.body.CollisionCategories = value;
			}
		}
		/// <summary>
		/// [GET / SET] A bitmask that specifies which collision categories this Collider interacts with.
		/// </summary>
		public Category CollidesWith
		{
			get { return this.colWith; }
			set
			{
				this.colWith = value;
				if (this.body != null) this.body.CollidesWith = value;
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
					PhysicsConvert.ToDualityUnit(this.body.WorldCenter) : 
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
					PhysicsConvert.ToDualityUnit(this.body.LocalCenter) : 
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
					boundRect = boundRect.ExpandToContain(info.AABB);

				float scale = this.GameObj.Transform.Scale;
				return boundRect.Transform(scale, scale).BoundingRadius;
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

		public RigidBody()
		{
			// Default shape
			this.AddShape(new CircleShapeInfo(128.0f, Vector2.Zero, 1.0f));
		}

		/// <summary>
		/// Adds a new shape to the Collider
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
		/// Removes an existing shape from the Collider.
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
		/// Removes all existing shapes from the Collider.
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
		/// Removes an existing joint from the Collider.
		/// </summary>
		/// <param name="joint"></param>
		public void RemoveJoint(JointInfo joint)
		{
			if (joint == null) throw new ArgumentNullException("joint");
			if (joint.BodyA != this && joint.BodyB != this) return;

			if (joint.BodyA != null)
			{
				if (joint.BodyA.joints != null) joint.BodyA.joints.Remove(joint);
				joint.BodyA.AwakeBody();
				joint.BodyA = null;
			}
			if (joint.BodyB != null)
			{
				if (joint.BodyB.joints != null) joint.BodyB.joints.Remove(joint);
				joint.BodyB.AwakeBody();
				joint.BodyB = null;
			}

			joint.DestroyJoint();
		}
		/// <summary>
		/// Adds a new joint to the Collider
		/// </summary>
		/// <param name="joint"></param>
		public void AddJoint(JointInfo joint, RigidBody other = null)
		{
			if (joint == null) throw new ArgumentNullException("joint");

			if (joint.BodyA != null)		joint.BodyA.RemoveJoint(joint);
			else if (joint.BodyB != null)	joint.BodyB.RemoveJoint(joint);

			joint.BodyA = this;
			joint.BodyB = other;

			if (joint.BodyA != null)
			{
				if (joint.BodyA.joints == null) joint.BodyA.joints = new List<JointInfo>();
				joint.BodyA.joints.Add(joint);
				joint.BodyA.AwakeBody();
			}
			if (joint.BodyB != null)
			{
				if (joint.BodyB.joints == null) joint.BodyB.joints = new List<JointInfo>();
				joint.BodyB.joints.Add(joint);
				joint.BodyB.AwakeBody();
			}

			joint.UpdateJoint();
		}
		/// <summary>
		/// Removes all existing joints from the Collider.
		/// </summary>
		public void ClearJoints()
		{
			if (this.joints == null) return;
			while (this.joints.Count > 0) this.RemoveJoint(this.joints[0]);
		}
		private void SetJoints(IEnumerable<JointInfo> joints)
		{
			JointInfo[] jointArray = joints != null ? joints.ToArray() : null;
	
			// Remove joints that are not in the new collection
			if (this.joints != null)
			{
				for (int i = this.joints.Count - 1; i >= 0; i--)
				{
					if (jointArray != null && jointArray.Contains(this.joints[i])) continue;
					this.RemoveJoint(this.joints[i]);
				}
			}

			// Add joints that are not in the old collection
			if (jointArray != null)
			{
				for (int i = 0; i < jointArray.Length; i++)
				{
					if (this.joints != null && this.joints.Contains(jointArray[i])) continue;
					JointInfo joint = jointArray[i];
					if (joint.BodyA != null)
						joint.BodyA.AddJoint(joint, joint.BodyB); // Allow reverse-add.
					else
						this.AddJoint(joint, null);
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
			this.body.ApplyAngularImpulse(angularImpulse / Time.SPFMult);
		}
		/// <summary>
		/// Applies a Transform-local impulse to the objects mass center. You don't usually need to apply <see cref="Time.TimeMult"/> here because it is inteded to be a one-time force impact.
		/// </summary>
		/// <param name="impulse"></param>
		public void ApplyLocalImpulse(Vector2 impulse)
		{
			if (this.body == null) return;
			this.ApplyWorldImpulse(
				this.gameobj.Transform.GetWorldVector(new Vector3(impulse)).Xy, 
				this.gameobj.Transform.GetWorldPoint(this.LocalMassCenter));
		}
		/// <summary>
		/// Applies a Transform-local impulse to the specified point. You don't usually need to apply <see cref="Time.TimeMult"/> here because it is inteded to be a one-time force impact.
		/// </summary>
		/// <param name="impulse"></param>
		/// <param name="applyAt"></param>
		public void ApplyLocalImpulse(Vector2 impulse, Vector2 applyAt)
		{
			this.ApplyWorldImpulse(
				this.gameobj.Transform.GetWorldVector(new Vector3(impulse)).Xy,
				this.gameobj.Transform.GetWorldPoint(new Vector3(applyAt)).Xy);
		}
		/// <summary>
		/// Applies a world impulse to the objects mass center. You don't usually need to apply <see cref="Time.TimeMult"/> here because it is inteded to be a one-time force impact.
		/// </summary>
		/// <param name="impulse"></param>
		public void ApplyWorldImpulse(Vector2 impulse)
		{
			MathF.CheckValidValue(impulse);
			if (this.body == null) return;
			this.body.ApplyLinearImpulse(
				PhysicsConvert.ToPhysicalUnit(impulse) / Time.SPFMult);
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
				PhysicsConvert.ToPhysicalUnit(impulse) / Time.SPFMult, 
				PhysicsConvert.ToPhysicalUnit(applyAt));
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
			this.body.ApplyTorque(angularForce / Time.SPFMult);
		}
		/// <summary>
		/// Applies a Transform-local force to the objects mass center. You don't need to apply <see cref="Time.TimeMult"/> here, the physics simulation takes care of this.
		/// </summary>
		/// <param name="force"></param>
		public void ApplyLocalForce(Vector2 force)
		{
			if (this.body == null) return;
			this.ApplyWorldForce(
				this.gameobj.Transform.GetWorldVector(force), 
				this.gameobj.Transform.GetWorldPoint(this.LocalMassCenter));
		}
		/// <summary>
		/// Applies a Transform-local force to the specified local point. You don't need to apply <see cref="Time.TimeMult"/> here, the physics simulation takes care of this.
		/// </summary>
		/// <param name="force"></param>
		/// <param name="applyAt"></param>
		public void ApplyLocalForce(Vector2 force, Vector2 applyAt)
		{
			if (this.body == null) return;
			this.ApplyWorldForce(
				this.gameobj.Transform.GetWorldVector(force), 
				this.gameobj.Transform.GetWorldPoint(applyAt));
		}
		/// <summary>
		/// Applies a world force to the objects mass center. You don't need to apply <see cref="Time.TimeMult"/> here, the physics simulation takes care of this.
		/// </summary>
		/// <param name="force"></param>
		public void ApplyWorldForce(Vector2 force)
		{
			if (this.body == null) return;
			this.ApplyWorldForce(
				force, 
				this.gameobj.Transform.GetWorldPoint(this.LocalMassCenter));
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
				PhysicsConvert.ToPhysicalUnit(force) / Time.SPFMult, 
				PhysicsConvert.ToPhysicalUnit(applyAt));
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

			Vector2 fsWorldCoord = PhysicsConvert.ToPhysicalUnit(worldCoord);

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
			Vector2 fsWorldCoord = PhysicsConvert.ToPhysicalUnit(worldCoord);

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

			Vector2 fsWorldCoord = PhysicsConvert.ToPhysicalUnit(worldCoord);
			FarseerPhysics.Collision.AABB fsWorldAABB = new FarseerPhysics.Collision.AABB(fsWorldCoord, PhysicsConvert.ToPhysicalUnit(worldCoord + size));

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
				fAABBIntersect.LowerBound = Vector2.ComponentMax(fAABB.LowerBound, fsWorldAABB.LowerBound);
				fAABBIntersect.UpperBound = Vector2.ComponentMin(fAABB.UpperBound, fsWorldAABB.UpperBound);

				Vector2 fsWorldCoordStep = PhysicsConvert.ToPhysicalUnit(new Vector2(MathF.Max(s.AABB.W, 1.0f), MathF.Max(s.AABB.H, 1.0f)) * 0.05f);
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

			if (this.shapes != null)
			{
				foreach (ShapeInfo info in this.shapes) info.UpdateFixture();
			}
			this.body.CollisionCategories = this.colCat;
			this.body.CollidesWith = this.colWith;
			this.UpdateBodyMass();

			this.AwakeBody();
			this.isUpdatingBody = false;
		}
		private void UpdateBodyMass()
		{
			if (this.body == null) return;

			this.body.ResetMassData();
			if (this.explicitMass > 0.0f)
				this.body.Mass = PhysicsConvert.ToPhysicalUnit(this.explicitMass);
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
			this.body.CollisionCategories = this.colCat;
			this.body.CollidesWith = this.colWith;

			this.UpdateBodyShape();

			if (t != null)
			{
				this.body.SetTransform(PhysicsConvert.ToPhysicalUnit(t.Pos.Xy), t.Angle);
				this.body.LinearVelocity = PhysicsConvert.ToPhysicalUnit(this.linearVel) / Time.SPFMult;
				this.body.AngularVelocity = this.angularVel / Time.SPFMult;
			}

			this.body.Collision += this.body_OnCollision;
			this.body.Separation += this.body_OnSeparation;
			this.body.PostSolve += this.body_PostSolve;

			//var testJoint = JointFactory.CreateFixedAngleJoint(Scene.CurrentPhysics, this.body);
			//var testJoint = JointFactory.CreateFixedDistanceJoint(Scene.CurrentPhysics, this.body, Vector2.Zero, Vector2.Zero);
			//var testJoint = JointFactory.CreateFixedFrictionJoint(Scene.CurrentPhysics, this.body, Vector2.Zero);
			//var testJoint = JointFactory.CreateFixedPrismaticJoint(Scene.CurrentPhysics, this.body, Vector2.Zero, Vector2.UnitX);
			//var testJoint = JointFactory.CreateFixedRevoluteJoint(Scene.CurrentPhysics, this.body, Vector2.Zero, Vector2.Zero);
			
			// etc.
			//var testJoint = JointFactory.CreateAngleJoint(
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
				if ((joint.BodyA != null && (joint.BodyA.Disposed || joint.BodyA.GameObj.Disposed)) || 
					(joint.BodyB != null && (joint.BodyB.Disposed || joint.BodyB.GameObj.Disposed)))
					this.RemoveJoint(joint);
			}
		}

		private void Initialize()
		{
			if (this.bodyInitState != InitState.Disposed) return;
			this.bodyInitState = InitState.Initializing;

			this.GameObj.Transform.EventTransformChanged += this.OnTransformChanged;
			this.InitBody();
			// Initialize joints
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
			
			this.CleanupJoints();
			this.CleanupBody();
			this.GameObj.Transform.EventTransformChanged -= this.OnTransformChanged;
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
			// Don't use foreach here in case someone decides to add something at the end while iterating..
			for (int i = 0; i < this.eventBuffer.Count; i++)
				this.ProcessSingleCollisionEvent(this.eventBuffer[i]);
			this.eventBuffer.Clear();
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
			foreach (ICmpCollisionListener c in this.gameobj.GetComponents<ICmpCollisionListener>())
			{
				if (!(c as Component).Active) continue;
				c.OnCollisionBegin(this, args);
			}
		}
		private void NotifyCollisionEnd(CollisionEventArgs args)
		{
			foreach (ICmpCollisionListener c in this.gameobj.GetComponents<ICmpCollisionListener>())
			{
				if (!(c as Component).Active) continue;
				c.OnCollisionEnd(this, args);
			}
		}
		private void NotifyCollisionSolve(CollisionEventArgs args)
		{
			foreach (ICmpCollisionListener c in this.gameobj.GetComponents<ICmpCollisionListener>())
			{
				if (!(c as Component).Active) continue;
				c.OnCollisionSolve(this, args);
			}
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
				this.linearVel = PhysicsConvert.ToDualityUnit(this.body.LinearVelocity) * Time.SPFMult;
				this.angularVel = this.body.AngularVelocity * Time.SPFMult;
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
						PhysicsConvert.ToDualityUnit(bodyPos.X), 
						PhysicsConvert.ToDualityUnit(bodyPos.Y), 
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
			if (sender == this) return;
			if (this.body == null) return;
			Transform t = e.Component as Transform;

			if ((e.Changes & Transform.DirtyFlags.Pos) != Transform.DirtyFlags.None)
				this.body.Position = PhysicsConvert.ToPhysicalUnit(t.Pos.Xy);
			if ((e.Changes & Transform.DirtyFlags.Angle) != Transform.DirtyFlags.None)
				this.body.Rotation = t.Angle;
			if ((e.Changes & Transform.DirtyFlags.Scale) != Transform.DirtyFlags.None)
				this.FlagBodyShape();

			if (e.Changes != Transform.DirtyFlags.None)
				this.body.Awake = true;
		}
		void ICmpInitializable.OnInit(InitContext context)
		{
			if (context == InitContext.Activate)
				this.Initialize();
			else if (context == InitContext.Loaded)
				this.RemoveDisposedJoints();
		}
		void ICmpInitializable.OnShutdown(ShutdownContext context)
		{
			if (context == ShutdownContext.Deactivate)
				this.Shutdown();
			else if (context == ShutdownContext.Saving)
				this.RemoveDisposedJoints();
		}

		protected override void OnCopyTo(Component target, Duality.Cloning.CloneProvider provider)
		{
			base.OnCopyTo(target, provider);
			RigidBody c = target as RigidBody;

			bool wasInitialized = c.bodyInitState == InitState.Initialized;
			if (wasInitialized) c.Shutdown();

			c.bodyType = this.bodyType;
			c.linearDamp = this.linearDamp;
			c.angularDamp = this.angularDamp;
			c.fixedAngle = this.fixedAngle;
			c.ignoreGravity = this.ignoreGravity;
			c.continous = this.continous;
			c.colCat = this.colCat;
			c.colWith = this.colWith;
			c.explicitMass = this.explicitMass;

			c.linearVel = this.linearVel;
			c.angularVel = this.angularVel;
			c.revolutions = this.revolutions;

			// Detach and copy Shapes
			c.shapes = null;
			if (this.shapes != null) c.SetShapes(this.shapes.Select(s => provider.RequestObjectClone(s)));

			// Detach and copy Joints
			c.joints = null;
			if (this.joints != null) c.SetJoints(this.joints.Select(j => 
			{
				// If there is a clone registered, just return the clone. Don't process a joint twice.
				if (provider.IsOriginalObject(j)) return provider.GetRegisteredObjectClone(j);

				JointInfo j2 = j.Clone();
				j2.BodyA = provider.GetRegisteredObjectClone(j.BodyA);
				j2.BodyB = provider.GetRegisteredObjectClone(j.BodyB);
				provider.RegisterObjectClone(j, j2);

				return j2;
			}));

			if (wasInitialized) c.Initialize();
		}
		

		/// <summary>
		/// Performs a 2d physical raycast in world coordinates.
		/// </summary>
		/// <param name="worldCoordA">The starting point.</param>
		/// <param name="worldCoordB">The desired end point.</param>
		/// <param name="callback">
		/// The callback that is invoked for each hit on the raycast. Note that the order in which each hit occurs isn't deterministic
		/// and may appear random. Return -1 to ignore the curret shape, 0 to terminate the raycast, data.Fraction to clip the ray for current hit, or 1 to continue.
		/// </param>
		/// <returns>Returns a list of all occurred hits, ordered by their Fraction value.</returns>
		public static List<RayCastData> RayCast(Vector2 worldCoordA, Vector2 worldCoordB, RayCastCallback callback = null)
		{
			if (callback == null) callback = Raycast_DefaultCallback;
			Vector2 fsWorldCoordA = PhysicsConvert.ToPhysicalUnit(worldCoordA);
			Vector2 fsWorldCoordB = PhysicsConvert.ToPhysicalUnit(worldCoordB);
			List<RayCastData> hitData = new List<RayCastData>();
			Scene.PhysicsWorld.RayCast(delegate(Fixture fixture, Vector2 pos, Vector2 normal, float fraction)
			{
				RayCastData data = new RayCastData(
					fixture.UserData as ShapeInfo, 
					PhysicsConvert.ToDualityUnit(pos), 
					normal, 
					fraction);
				float result = callback(data);
				if (result >= 0.0f) hitData.Add(data);
				return result;
			}, fsWorldCoordA, fsWorldCoordB);
			hitData.StableSort((d1, d2) => (int)(1000000.0f * (d1.Fraction - d2.Fraction)));
			return hitData;
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
			Vector2 fsWorldCoord = PhysicsConvert.ToPhysicalUnit(worldCoord);
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
			Vector2 fsWorldCoord = PhysicsConvert.ToPhysicalUnit(worldCoord);
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
			
			Vector2 fsWorldCoord = PhysicsConvert.ToPhysicalUnit(worldCoord);
			FarseerPhysics.Collision.AABB fsWorldAABB = new FarseerPhysics.Collision.AABB(fsWorldCoord, PhysicsConvert.ToPhysicalUnit(worldCoord + size));
			Scene.PhysicsWorld.QueryAABB(fixture =>
				{
					ShapeInfo shape = fixture.UserData as ShapeInfo;
					if (shape != null && shape.Parent != null && shape.Parent.Active)
						bodies.Add(shape.Parent);
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
	public class RayCastData
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
