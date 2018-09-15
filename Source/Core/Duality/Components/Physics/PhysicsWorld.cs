using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Diagnostics;

using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Common;
using FarseerPhysics.Collision.Shapes;
using FarseerBodyType = FarseerPhysics.Dynamics.BodyType;

using Duality.Resources;

namespace Duality.Components.Physics
{
	/// <summary>
	/// A physics world is a distinct simulation space for physics objects, as represented by 
	/// <see cref="RigidBody"/> components. Always used in conjunction with a <see cref="Scene"/>
	/// that contains the components representing each physics object.
	/// </summary>
	[DontSerialize]
	public class PhysicsWorld
	{
		private World   native           = new World(Vector2.Zero);
		private Vector2 gravity          = Vector2.UnitY * 33.0f;
		private float   fixedStepPeriod  = Time.SecondsPerFrame;
		private float   fixedStepTimer   = 0.0f;
		private bool    lowFramerateMode = false;

		/// <summary>
		/// The physics worlds native implementation. Don't use this, unless you know exactly what you're doing.
		/// </summary>
		public World Native
		{
			get { return this.native; }
		}
		/// <summary>
		/// The worlds global gravity force.
		/// </summary>
		public Vector2 Gravity
		{
			get { return this.gravity; }
			set
			{
				this.gravity = value;
				this.native.Gravity = PhysicsUnit.ForceToPhysical * value;
				foreach (Body b in this.native.BodyList)
				{
					if (b.IgnoreGravity || b.BodyType != FarseerBodyType.Dynamic) continue;
					b.Awake = true;
				}
			}
		}
		/// <summary>
		/// [GET] When using fixed-timestep physics, the alpha value [0.0 - 1.0] indicates how
		/// complete the next step is. This is used for linear interpolation inbetween fixed physics steps.
		/// </summary>
		public float InterpolateAlpha
		{
			get { return this.fixedStepTimer / Time.SecondsPerFrame; }
		}
		/// <summary>
		/// [GET] Is fixed-timestep physics calculation currently active?
		/// </summary>
		public bool IsFixedTimestep
		{
			get { return DualityApp.AppData.PhysicsFixedTime && !this.lowFramerateMode; }
		}


		/// <summary>
		/// Awakes all currently existing physical objects.
		/// </summary>
		public void Awake()
		{
			foreach (Body b in this.native.BodyList)
				b.Awake = true;
		}
		/// <summary>
		/// Advances this physics world simulation by <paramref name="timestep"/> seconds.
		/// 
		/// If <see cref="IsFixedTimestep"/> is true, this may actually simulate multiple smaller
		/// steps, or none at all while accumulating the internal fixed step timer. Otherwise,
		/// it will perform exactly one physics stept with the specified <paramref name="timestep"/>.
		/// </summary>
		/// <param name="timestep"></param>
		public void Simulate(float timestep)
		{
			bool physUpdate = false;
			double physBegin = Time.MainTimer.TotalSeconds;
			if (this.IsFixedTimestep)
			{
				this.fixedStepTimer += timestep;
				int iterations = 0;
				if (this.fixedStepTimer >= this.fixedStepPeriod)
				{
					Profile.TimeUpdatePhysics.BeginMeasure();
					DualityApp.EditorGuard(() =>
					{
						double timeUpdateBegin = Time.MainTimer.TotalSeconds;
						while (this.fixedStepTimer >= this.fixedStepPeriod)
						{
							// Catch up on updating progress
							this.UpdateNativeSettings();
							this.native.Step(this.fixedStepPeriod);
							this.fixedStepTimer -= this.fixedStepPeriod;
							iterations++;

							double timeSpent = Time.MainTimer.TotalSeconds - timeUpdateBegin;
							if (timeSpent >= Time.SecondsPerFrame * 10.0f) break; // Emergency exit
						}
					});
					physUpdate = true;
					Profile.TimeUpdatePhysics.EndMeasure();
				}
			}
			else
			{
				Profile.TimeUpdatePhysics.BeginMeasure();
				DualityApp.EditorGuard(() =>
				{
					this.UpdateNativeSettings();
					this.native.Step(timestep);
					if (timestep == 0.0f) this.native.ClearForces(); // Complete freeze? Clear forces, so they don't accumulate.
					this.fixedStepTimer = this.fixedStepPeriod;
				});
				physUpdate = true;
				Profile.TimeUpdatePhysics.EndMeasure();
			}
			double physTime = Time.MainTimer.TotalSeconds - physBegin;

			// Apply Farseers internal measurements to Duality
			if (physUpdate)
			{
				Profile.TimeUpdatePhysicsAddRemove.Set(1000.0f * this.native.AddRemoveTime / Stopwatch.Frequency);
				Profile.TimeUpdatePhysicsContacts.Set(1000.0f * this.native.ContactsUpdateTime / Stopwatch.Frequency);
				Profile.TimeUpdatePhysicsContinous.Set(1000.0f * this.native.ContinuousPhysicsTime / Stopwatch.Frequency);
				Profile.TimeUpdatePhysicsController.Set(1000.0f * this.native.ControllersUpdateTime / Stopwatch.Frequency);
				Profile.TimeUpdatePhysicsSolve.Set(1000.0f * this.native.SolveUpdateTime / Stopwatch.Frequency);
			}

			// Update low fps physics state
			if (!this.lowFramerateMode)
				this.lowFramerateMode = Time.UnscaledDeltaTime > Time.SecondsPerFrame && physTime > Time.UnscaledDeltaTime * 0.85f;
			else
				this.lowFramerateMode = !(Time.UnscaledDeltaTime < Time.SecondsPerFrame * 0.9f || physTime < Time.UnscaledDeltaTime * 0.6f);
		}
		/// <summary>
		/// Resets physics simulation variables to ensure a fresh start on the next <see cref="Simulate"/>
		/// call. Does not affect the worlds contents.
		/// </summary>
		public void ResetSimulation()
		{
			this.lowFramerateMode = false;
			this.fixedStepTimer = this.fixedStepPeriod;
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
		public void RayCast(Vector2 start, Vector2 end, RayCastCallback callback)
		{
			if (callback == null) callback = RayCast_DefaultCallback;

			Vector2 fsWorldCoordA = PhysicsUnit.LengthToPhysical * start;
			Vector2 fsWorldCoordB = PhysicsUnit.LengthToPhysical * end;

			this.native.RayCast(delegate (Fixture fixture, Vector2 pos, Vector2 normal, float fraction)
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
		/// <param name="hits">
		/// A list that will be filled with all hits that were registered, ordered by their Fraction value. 
		/// The list will not be cleared before adding items.
		/// </param>
		/// <returns>Returns whether any new hit was registered.</returns>
		public bool RayCast(Vector2 start, Vector2 end, RayCastCallback callback, RawList<RayCastData> hits)
		{
			if (callback == null) callback = RayCast_DefaultCallback;

			Vector2 fsWorldCoordA = PhysicsUnit.LengthToPhysical * start;
			Vector2 fsWorldCoordB = PhysicsUnit.LengthToPhysical * end;

			int oldResultCount = hits.Count;
			this.native.RayCast(delegate (Fixture fixture, Vector2 pos, Vector2 normal, float fraction)
			{
				int index = hits.Count++;
				RayCastData[] data = hits.Data;

				data[index] = new RayCastData(
					fixture.UserData as ShapeInfo,
					PhysicsUnit.LengthToDuality * pos,
					normal,
					fraction);

				float result = callback(data[index]);
				if (result < 0.0f)
					hits.Count--;

				return result;
			}, fsWorldCoordA, fsWorldCoordB);

			hits.Data.StableSort(
				0,
				hits.Count,
				(d1, d2) => (int)(1000000.0f * (d1.Fraction - d2.Fraction)));
			return hits.Count > oldResultCount;
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
		public bool RayCast(Vector2 start, Vector2 end, RayCastCallback callback, out RayCastData firstHit)
		{
			if (callback == null) callback = RayCast_DefaultCallback;

			Vector2 fsWorldCoordA = PhysicsUnit.LengthToPhysical * start;
			Vector2 fsWorldCoordB = PhysicsUnit.LengthToPhysical * end;

			float firstHitFraction = float.MaxValue;
			RayCastData firstHitLocal = default(RayCastData);

			this.native.RayCast(delegate (Fixture fixture, Vector2 pos, Vector2 normal, float fraction)
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
		private static float RayCast_DefaultCallback(RayCastData data)
		{
			return 1.0f;
		}

		/// <summary>
		/// Performs a global physical picking operation and returns the <see cref="ShapeInfo">shape</see> in which
		/// the specified world coordinate is located in.
		/// </summary>
		/// <param name="worldCoord"></param>
		/// <returns></returns>
		public ShapeInfo PickShape(Vector2 worldCoord)
		{
			Vector2 fsWorldCoord = PhysicsUnit.LengthToPhysical * worldCoord;
			Fixture f = this.native.TestPoint(fsWorldCoord);

			return f != null && f.UserData is ShapeInfo ? (f.UserData as ShapeInfo) : null;
		}
		/// <summary>
		/// Performs a global physical picking operation and returns the <see cref="ShapeInfo">shapes</see> that
		/// intersect the specified world coordinate.
		/// </summary>
		/// <param name="worldCoord"></param>
		/// <param name="pickedShapes">
		/// A list that will be filled with all shapes that were found. 
		/// The list will not be cleared before adding items.
		/// </param>
		/// <returns>Returns whether any new shape was found.</returns>
		public bool PickShapes(Vector2 worldCoord, List<ShapeInfo> pickedShapes)
		{
			Vector2 fsWorldCoord = PhysicsUnit.LengthToPhysical * worldCoord;
			List<Fixture> fixtureList = this.native.TestPointAll(fsWorldCoord);

			int oldResultCount = pickedShapes.Count;
			foreach (Fixture fixture in fixtureList)
			{
				if (fixture == null) continue;

				ShapeInfo shape = fixture.UserData as ShapeInfo;
				if (shape == null) continue;

				pickedShapes.Add(shape);
			}

			return pickedShapes.Count > oldResultCount;
		}
		/// <summary>
		/// Performs a global physical picking operation and returns the <see cref="ShapeInfo">shapes</see> that
		/// intersect the specified world coordinate area.
		/// </summary>
		/// <param name="worldCoord"></param>
		/// <param name="size"></param>
		/// <param name="pickedShapes">
		/// A list that will be filled with all shapes that were found. 
		/// The list will not be cleared before adding items.
		/// </param>
		/// <returns>Returns whether any new shape was found.</returns>
		public bool PickShapes(Vector2 worldCoord, Vector2 size, List<ShapeInfo> pickedShapes)
		{
			List<RigidBody> potentialBodies = new List<RigidBody>();
			this.QueryRect(worldCoord, size, potentialBodies);
			if (potentialBodies.Count == 0) return false;

			PolygonShape boxShape = new PolygonShape(new Vertices(new List<Vector2>
			{
				PhysicsUnit.LengthToPhysical * worldCoord,
				PhysicsUnit.LengthToPhysical * new Vector2(worldCoord.X + size.X, worldCoord.Y),
				PhysicsUnit.LengthToPhysical * (worldCoord + size),
				PhysicsUnit.LengthToPhysical * new Vector2(worldCoord.X, worldCoord.Y + size.Y)
			}), 1);

			FarseerPhysics.Common.Transform boxTransform = new FarseerPhysics.Common.Transform();
			boxTransform.SetIdentity();

			int oldResultCount = pickedShapes.Count;
			foreach (RigidBody body in potentialBodies)
			{
				body.PickShapes(boxShape, boxTransform, pickedShapes);
			}

			return pickedShapes.Count > oldResultCount;
		}
		/// <summary>
		/// Performs a global physical AABB query and returns the <see cref="RigidBody">bodies</see> that
		/// might be roughly contained or intersected by the specified region.
		/// </summary>
		/// <param name="worldCoord"></param>
		/// <param name="size"></param>
		/// <param name="queriedBodies">
		/// A list that will be filled with all bodies that were found. 
		/// The list will not be cleared before adding items.
		/// </param>
		/// <returns>Returns whether any new body was found.</returns>
		public bool QueryRect(Vector2 worldCoord, Vector2 size, List<RigidBody> queriedBodies)
		{
			Vector2 fsWorldCoord = PhysicsUnit.LengthToPhysical * worldCoord;
			FarseerPhysics.Collision.AABB fsWorldAABB = new FarseerPhysics.Collision.AABB(fsWorldCoord, PhysicsUnit.LengthToPhysical * (worldCoord + size));

			int oldResultCount = queriedBodies.Count;
			this.native.QueryAABB(fixture =>
				{
					ShapeInfo shape = fixture.UserData as ShapeInfo;
					if (shape != null && shape.Parent != null && shape.Parent.Active)
					{
						if (!queriedBodies.Contains(shape.Parent))
							queriedBodies.Add(shape.Parent);
					}
					return true;
				},
				ref fsWorldAABB);

			return queriedBodies.Count > oldResultCount;
		}

		private void UpdateNativeSettings()
		{
			Settings.VelocityThreshold = PhysicsUnit.VelocityToPhysical * DualityApp.AppData.PhysicsVelocityThreshold;
		}
	}
}
