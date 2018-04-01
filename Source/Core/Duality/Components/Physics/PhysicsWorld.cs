using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Diagnostics;

using FarseerPhysics;
using FarseerPhysics.Dynamics;
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
		/// Destroys all physics objects in this world.
		/// </summary>
		public void Clear()
		{
			this.native.Clear();
		}
		/// <summary>
		/// Advances this physics world simulation by <paramref name="timestep"/> seconds.
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

		private void UpdateNativeSettings()
		{
			Settings.VelocityThreshold = PhysicsUnit.VelocityToPhysical * DualityApp.AppData.PhysicsVelocityThreshold;
		}
	}
}
