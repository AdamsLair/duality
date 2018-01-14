/*
* Farseer Physics Engine based on Box2D.XNA port:
* Copyright (c) 2010 Ian Qvist
* 
* Box2D.XNA port of Box2D:
* Copyright (c) 2009 Brandon Furtwangler, Nathan Furtwangler
*
* Original source Box2D:
* Copyright (c) 2006-2009 Erin Catto http://www.gphysics.com 
* 
* This software is provided 'as-is', without any express or implied 
* warranty.  In no event will the authors be held liable for any damages 
* arising from the use of this software. 
* Permission is granted to anyone to use this software for any purpose, 
* including commercial applications, and to alter it and redistribute it 
* freely, subject to the following restrictions: 
* 1. The origin of this software must not be misrepresented; you must not 
* claim that you wrote the original software. If you use this software 
* in a product, an acknowledgment in the product documentation would be 
* appreciated but is not required. 
* 2. Altered source versions must be plainly marked as such, and must not be 
* misrepresented as being the original software. 
* 3. This notice may not be removed or altered from any source distribution. 
*/

using System;
using System.Diagnostics;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Dynamics.Joints;
using Duality;

namespace FarseerPhysics.Dynamics
{
	/// <summary>
	/// This is an internal class.
	/// </summary>
	public class Island
	{
		public Body[] Bodies;
		public int BodyCount;
		public int ContactCount;
		public int JointCount;
		private int _bodyCapacity;
		private int _contactCapacity;
		private ContactManager _contactManager;
		private ContactSolver _contactSolver = new ContactSolver();
		private Contact[] _contacts;
		private int _jointCapacity;
		private Joint[] _joints;
		public float JointUpdateTime;

		private const float LinTolSqr = Settings.LinearSleepTolerance * Settings.LinearSleepTolerance;
		private const float AngTolSqr = Settings.AngularSleepTolerance * Settings.AngularSleepTolerance;

		private Stopwatch _watch = new Stopwatch();

		public void Reset(int bodyCapacity, int contactCapacity, int jointCapacity, ContactManager contactManager)
		{
			this._bodyCapacity = bodyCapacity;
			this._contactCapacity = contactCapacity;
			this._jointCapacity = jointCapacity;

			this.BodyCount = 0;
			this.ContactCount = 0;
			this.JointCount = 0;

			this._contactManager = contactManager;

			if (this.Bodies == null || this.Bodies.Length < bodyCapacity)
			{
				this.Bodies = new Body[bodyCapacity];
			}

			if (this._contacts == null || this._contacts.Length < contactCapacity)
			{
				this._contacts = new Contact[contactCapacity * 2];
			}

			if (this._joints == null || this._joints.Length < jointCapacity)
			{
				this._joints = new Joint[jointCapacity * 2];
			}
		}

		public void Clear()
		{
			this.BodyCount = 0;
			this.ContactCount = 0;
			this.JointCount = 0;
		}

		private float _tmpTime;

		public void Solve(ref TimeStep step, ref Vector2 gravity)
		{
			// Integrate velocities and apply damping.
			for (int i = 0; i < this.BodyCount; ++i)
			{
				Body b = this.Bodies[i];

				if (b.BodyType != BodyType.Dynamic)
				{
					continue;
				}

				// Integrate velocities.
				// FPE 3 only - Only apply gravity if the body wants it.
				if (b.IgnoreGravity)
				{
					b.LinearVelocityInternal.X += step.dt * (b.InvMass * b.Force.X);
					b.LinearVelocityInternal.Y += step.dt * (b.InvMass * b.Force.Y);
					b.AngularVelocityInternal += step.dt * b.InvI * b.Torque;
				}
				else
				{
					b.LinearVelocityInternal.X += step.dt * (gravity.X + b.InvMass * b.Force.X);
					b.LinearVelocityInternal.Y += step.dt * (gravity.Y + b.InvMass * b.Force.Y);
					b.AngularVelocityInternal += step.dt * b.InvI * b.Torque;
				}

				// Apply damping.
				// ODE: dv/dt + c * v = 0
				// Solution: v(t) = v0 * exp(-c * t)
				// Time step: v(t + dt) = v0 * exp(-c * (t + dt)) = v0 * exp(-c * t) * exp(-c * dt) = v * exp(-c * dt)
				// v2 = exp(-c * dt) * v1
				// Taylor expansion:
				// v2 = (1.0f - c * dt) * v1
				b.LinearVelocityInternal *= MathUtils.Clamp(1.0f - step.dt * b.LinearDamping, 0.0f, 1.0f);
				b.AngularVelocityInternal *= MathUtils.Clamp(1.0f - step.dt * b.AngularDamping, 0.0f, 1.0f);
			}

			// Partition contacts so that contacts with static bodies are solved last.
			int i1 = -1;
			for (int i2 = 0; i2 < this.ContactCount; ++i2)
			{
				Fixture fixtureA = this._contacts[i2].FixtureA;
				Fixture fixtureB = this._contacts[i2].FixtureB;
				Body bodyA = fixtureA.Body;
				Body bodyB = fixtureB.Body;
				bool nonStatic = bodyA.BodyType != BodyType.Static && bodyB.BodyType != BodyType.Static;
				if (nonStatic)
				{
					++i1;

					//TODO: Only swap if they are not the same? see http://code.google.com/p/box2d/issues/detail?id=162
					Contact tmp = this._contacts[i1];
					this._contacts[i1] = this._contacts[i2];
					this._contacts[i2] = tmp;
				}
			}

			// Initialize velocity constraints.
			this._contactSolver.Reset(this._contacts, this.ContactCount, step.dtRatio, Settings.EnableWarmstarting);
			this._contactSolver.InitializeVelocityConstraints();

#pragma warning disable 0162 // Unreachable code detected
			if (Settings.EnableWarmstarting)
			{
				this._contactSolver.WarmStart();
			}
#pragma warning restore 0162 // Unreachable code detected

			if (Settings.EnableDiagnostics)
			{
				this._watch.Start();
				this._tmpTime = 0;
			}

			for (int i = 0; i < this.JointCount; ++i)
			{
				if (this._joints[i].Enabled)
					this._joints[i].InitVelocityConstraints(ref step);
			}

			if (Settings.EnableDiagnostics)
			{
				this._tmpTime += this._watch.ElapsedTicks;
			}

			// Solve velocity constraints.
			for (int i = 0; i < Settings.VelocityIterations; ++i)
			{
				if (Settings.EnableDiagnostics)
					this._watch.Start();
				for (int j = 0; j < this.JointCount; ++j)
				{
					Joint joint = this._joints[j];

					if (!joint.Enabled)
						continue;

					joint.SolveVelocityConstraints(ref step);
					joint.Validate(step.inv_dt);
				}

				if (Settings.EnableDiagnostics)
				{
					this._watch.Stop();
					this._tmpTime += this._watch.ElapsedTicks;
					this._watch.Reset();
				}

				this._contactSolver.SolveVelocityConstraints();
			}

			// Post-solve (store impulses for warm starting).
			this._contactSolver.StoreImpulses();

			// Integrate positions.
			for (int i = 0; i < this.BodyCount; ++i)
			{
				Body b = this.Bodies[i];

				if (b.BodyType == BodyType.Static)
				{
					continue;
				}

				// Check for large velocities.
				float translationX = step.dt * b.LinearVelocityInternal.X;
				float translationY = step.dt * b.LinearVelocityInternal.Y;
				float result = translationX * translationX + translationY * translationY;

				if (result > Settings.MaxTranslationSquared)
				{
					float sq = (float)Math.Sqrt(result);

					float ratio = Settings.MaxTranslation / sq;
					b.LinearVelocityInternal.X *= ratio;
					b.LinearVelocityInternal.Y *= ratio;
				}

				float rotation = step.dt * b.AngularVelocityInternal;
				if (rotation * rotation > Settings.MaxRotationSquared)
				{
					float ratio = Settings.MaxRotation / Math.Abs(rotation);
					b.AngularVelocityInternal *= ratio;
				}

				// Store positions for continuous collision.
				b.Sweep.C0.X = b.Sweep.C.X;
				b.Sweep.C0.Y = b.Sweep.C.Y;
				b.Sweep.A0 = b.Sweep.A;

				// Integrate
				b.Sweep.C.X += step.dt * b.LinearVelocityInternal.X;
				b.Sweep.C.Y += step.dt * b.LinearVelocityInternal.Y;
				b.Sweep.A += step.dt * b.AngularVelocityInternal;

				// Compute new transform
				b.SynchronizeTransform();

				// Note: shapes are synchronized later.
			}

			// Iterate over constraints.
			for (int i = 0; i < Settings.PositionIterations; ++i)
			{
				bool contactsOkay = this._contactSolver.SolvePositionConstraints(Settings.ContactBaumgarte);
				bool jointsOkay = true;

				if (Settings.EnableDiagnostics)
					this._watch.Start();
				for (int j = 0; j < this.JointCount; ++j)
				{
					Joint joint = this._joints[j];
					if (!joint.Enabled)
						continue;

					bool jointOkay = joint.SolvePositionConstraints();
					jointsOkay = jointsOkay && jointOkay;
				}

				if (Settings.EnableDiagnostics)
				{
					this._watch.Stop();
					this._tmpTime += this._watch.ElapsedTicks;
					this._watch.Reset();
				}
				if (contactsOkay && jointsOkay)
				{
					// Exit early if the position errors are small.
					break;
				}
			}

			if (Settings.EnableDiagnostics)
			{
				this.JointUpdateTime = this._tmpTime;
			}

			Report(this._contactSolver.Constraints);

			if (Settings.AllowSleep)
			{
				float minSleepTime = Settings.MaxFloat;

				for (int i = 0; i < this.BodyCount; ++i)
				{
					Body b = this.Bodies[i];
					if (b.BodyType == BodyType.Static)
					{
						continue;
					}

					if ((b.Flags & BodyFlags.AutoSleep) == 0)
					{
						b.SleepTime = 0.0f;
						minSleepTime = 0.0f;
					}

					if ((b.Flags & BodyFlags.AutoSleep) == 0 ||
						b.AngularVelocityInternal * b.AngularVelocityInternal > AngTolSqr ||
						Vector2.Dot(b.LinearVelocityInternal, b.LinearVelocityInternal) > LinTolSqr)
					{
						b.SleepTime = 0.0f;
						minSleepTime = 0.0f;
					}
					else
					{
						b.SleepTime += step.dt;
						minSleepTime = Math.Min(minSleepTime, b.SleepTime);
					}
				}

				if (minSleepTime >= Settings.TimeToSleep)
				{
					for (int i = 0; i < this.BodyCount; ++i)
					{
						Body b = this.Bodies[i];
						b.Awake = false;
					}
				}
			}
		}

		internal void SolveTOI(ref TimeStep subStep)
		{
			this._contactSolver.Reset(this._contacts, this.ContactCount, subStep.dtRatio, false);

			// Solve position constraints.
			const float kTOIBaumgarte = 0.75f;
			for (int i = 0; i < Settings.TOIPositionIterations; ++i)
			{
				bool contactsOkay = this._contactSolver.SolvePositionConstraints(kTOIBaumgarte);
				if (contactsOkay)
				{
					break;
				}

				if (i == Settings.TOIPositionIterations - 1)
				{
					i += 0;
				}
			}

			// Leap of faith to new safe state.
			for (int i = 0; i < this.BodyCount; ++i)
			{
				Body body = this.Bodies[i];
				body.Sweep.A0 = body.Sweep.A;
				body.Sweep.C0 = body.Sweep.C;
			}

			// No warm starting is needed for TOI events because warm
			// starting impulses were applied in the discrete solver.
			this._contactSolver.InitializeVelocityConstraints();

			// Solve velocity constraints.
			for (int i = 0; i < Settings.TOIVelocityIterations; ++i)
			{
				this._contactSolver.SolveVelocityConstraints();
			}

			// Don't store the TOI contact forces for warm starting
			// because they can be quite large.

			// Integrate positions.
			for (int i = 0; i < this.BodyCount; ++i)
			{
				Body b = this.Bodies[i];

				if (b.BodyType == BodyType.Static)
				{
					continue;
				}

				// Check for large velocities.
				float translationx = subStep.dt * b.LinearVelocityInternal.X;
				float translationy = subStep.dt * b.LinearVelocityInternal.Y;
				float dot = translationx * translationx + translationy * translationy;
				if (dot > Settings.MaxTranslationSquared)
				{
					float norm = 1f / (float)Math.Sqrt(dot);
					float value = Settings.MaxTranslation * subStep.inv_dt;
					b.LinearVelocityInternal.X = value * (translationx * norm);
					b.LinearVelocityInternal.Y = value * (translationy * norm);
				}

				float rotation = subStep.dt * b.AngularVelocity;
				if (rotation * rotation > Settings.MaxRotationSquared)
				{
					if (rotation < 0.0)
					{
						b.AngularVelocityInternal = -subStep.inv_dt * Settings.MaxRotation;
					}
					else
					{
						b.AngularVelocityInternal = subStep.inv_dt * Settings.MaxRotation;
					}
				}

				// Integrate
				b.Sweep.C.X += subStep.dt * b.LinearVelocityInternal.X;
				b.Sweep.C.Y += subStep.dt * b.LinearVelocityInternal.Y;
				b.Sweep.A += subStep.dt * b.AngularVelocityInternal;

				// Compute new transform
				b.SynchronizeTransform();

				// Note: shapes are synchronized later.
			}

			Report(this._contactSolver.Constraints);
		}

		public void Add(Body body)
		{
			Debug.Assert(this.BodyCount < this._bodyCapacity);
			this.Bodies[this.BodyCount++] = body;
		}

		public void Add(Contact contact)
		{
			Debug.Assert(this.ContactCount < this._contactCapacity);
			this._contacts[this.ContactCount++] = contact;
		}

		public void Add(Joint joint)
		{
			Debug.Assert(this.JointCount < this._jointCapacity);
			this._joints[this.JointCount++] = joint;
		}

		private void Report(ContactConstraint[] constraints)
		{
			if (this._contactManager == null)
				return;

			for (int i = 0; i < this.ContactCount; ++i)
			{
				Contact c = this._contacts[i];

				c.FixtureA.Body.OnAfterCollision(c.FixtureA, c.FixtureB, c);
				c.FixtureB.Body.OnAfterCollision(c.FixtureB, c.FixtureA, c);

				ContactConstraint cc = constraints[i];
				c.FixtureA.Body.OnPostSolve(c, cc);
				c.FixtureB.Body.OnPostSolve(c, cc);

				if (this._contactManager.PostSolve != null)
					this._contactManager.PostSolve(c, cc);
			}
		}
	}
}