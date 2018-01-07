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
using Duality;

namespace FarseerPhysics.Dynamics.Joints
{
	// Linear constraint (point-to-line)
	// d = p2 - p1 = x2 + r2 - x1 - r1
	// C = dot(perp, d)
	// Cdot = dot(d, cross(w1, perp)) + dot(perp, v2 + cross(w2, r2) - v1 - cross(w1, r1))
	//      = -dot(perp, v1) - dot(cross(d + r1, perp), w1) + dot(perp, v2) + dot(cross(r2, perp), v2)
	// J = [-perp, -cross(d + r1, perp), perp, cross(r2,perp)]
	//
	// Angular constraint
	// C = a2 - a1 + a_initial
	// Cdot = w2 - w1
	// J = [0 0 -1 0 0 1]
	//
	// K = J * invM * JT
	//
	// J = [-a -s1 a s2]
	//     [0  -1  0  1]
	// a = perp
	// s1 = cross(d + r1, a) = cross(p2 - x1, a)
	// s2 = cross(r2, a) = cross(p2 - x2, a)
	// Motor/Limit linear constraint
	// C = dot(ax1, d)
	// Cdot = = -dot(ax1, v1) - dot(cross(d + r1, ax1), w1) + dot(ax1, v2) + dot(cross(r2, ax1), v2)
	// J = [-ax1 -cross(d+r1,ax1) ax1 cross(r2,ax1)]
	// Block Solver
	// We develop a block solver that includes the joint limit. This makes the limit stiff (inelastic) even
	// when the mass has poor distribution (leading to large torques about the joint anchor points).
	//
	// The Jacobian has 3 rows:
	// J = [-uT -s1 uT s2] // linear
	//     [0   -1   0  1] // angular
	//     [-vT -a1 vT a2] // limit
	//
	// u = perp
	// v = axis
	// s1 = cross(d + r1, u), s2 = cross(r2, u)
	// a1 = cross(d + r1, v), a2 = cross(r2, v)
	// M * (v2 - v1) = JT * df
	// J * v2 = bias
	//
	// v2 = v1 + invM * JT * df
	// J * (v1 + invM * JT * df) = bias
	// K * df = bias - J * v1 = -Cdot
	// K = J * invM * JT
	// Cdot = J * v1 - bias
	//
	// Now solve for f2.
	// df = f2 - f1
	// K * (f2 - f1) = -Cdot
	// f2 = invK * (-Cdot) + f1
	//
	// Clamp accumulated limit impulse.
	// lower: f2(3) = max(f2(3), 0)
	// upper: f2(3) = min(f2(3), 0)
	//
	// Solve for correct f2(1:2)
	// K(1:2, 1:2) * f2(1:2) = -Cdot(1:2) - K(1:2,3) * f2(3) + K(1:2,1:3) * f1
	//                       = -Cdot(1:2) - K(1:2,3) * f2(3) + K(1:2,1:2) * f1(1:2) + K(1:2,3) * f1(3)
	// K(1:2, 1:2) * f2(1:2) = -Cdot(1:2) - K(1:2,3) * (f2(3) - f1(3)) + K(1:2,1:2) * f1(1:2)
	// f2(1:2) = invK(1:2,1:2) * (-Cdot(1:2) - K(1:2,3) * (f2(3) - f1(3))) + f1(1:2)
	//
	// Now compute impulse to be applied:
	// df = f2 - f1

	/// <summary>
	/// A prismatic joint. This joint provides one degree of freedom: translation
	/// along an axis fixed in body1. Relative rotation is prevented. You can
	/// use a joint limit to restrict the range of motion and a joint motor to
	/// drive the motion or to model joint friction.
	/// </summary>
	public class PrismaticJoint : Joint
	{
		public Vector2 LocalAnchorA;

		public Vector2 LocalAnchorB;
		private Mat33 _K;
		private float _a1, _a2;
		private Vector2 _axis;
		private bool _enableLimit;
		private bool _enableMotor;
		private Vector3 _impulse;
		private LimitState _limitState;
		private Vector2 _localXAxis1;
		private Vector2 _localYAxis1;
		private float _lowerTranslation;
		private float _maxMotorForce;
		private float _motorImpulse;
		private float _motorMass; // effective mass for motor/limit translational constraint.
		private float _motorSpeed;
		private Vector2 _perp;
		private float _refAngle;
		private float _s1, _s2;
		private float _upperTranslation;

		internal PrismaticJoint()
		{
			this.JointType = JointType.Prismatic;
		}

		/// <summary>
		/// This requires defining a line of
		/// motion using an axis and an anchor point. The definition uses local
		/// anchor points and a local axis so that the initial configuration
		/// can violate the constraint slightly. The joint translation is zero
		/// when the local anchor points coincide in world space. Using local
		/// anchors and a local axis helps when saving and loading a game.
		/// </summary>
		/// <param name="bodyA">The first body.</param>
		/// <param name="bodyB">The second body.</param>
		/// <param name="localAnchorA">The first body anchor.</param>
		/// <param name="localAnchorB">The second body anchor.</param>
		/// <param name="axis">The axis.</param>
		public PrismaticJoint(Body bodyA, Body bodyB, Vector2 localAnchorA, Vector2 localAnchorB, Vector2 axis)
			: base(bodyA, bodyB)
		{
			this.JointType = JointType.Prismatic;

			this.LocalAnchorA = localAnchorA;
			this.LocalAnchorB = localAnchorB;

			this._localXAxis1 = this.BodyA.GetLocalVector(axis);
			this._localYAxis1 = MathUtils.Cross(1.0f, this._localXAxis1);
			this._refAngle = this.BodyB.Rotation - this.BodyA.Rotation;

			this._limitState = LimitState.Inactive;
		}

		public override Vector2 WorldAnchorA
		{
			get { return this.BodyA.GetWorldPoint(this.LocalAnchorA); }
		}

		public override Vector2 WorldAnchorB
		{
			get { return this.BodyB.GetWorldPoint(this.LocalAnchorB); }
			set { Debug.Assert(false, "You can't set the world anchor on this joint type."); }
		}

		/// <summary>
		/// Get the current joint translation, usually in meters.
		/// </summary>
		/// <value></value>
		public float JointTranslation
		{
			get
			{
				Vector2 d = this.BodyB.GetWorldPoint(this.LocalAnchorB) - this.BodyA.GetWorldPoint(this.LocalAnchorA);
				Vector2 axis = this.BodyA.GetWorldVector(ref this._localXAxis1);

				return Vector2.Dot(d, axis);
			}
		}

		/// <summary>
		/// Get the current joint translation speed, usually in meters per second.
		/// </summary>
		/// <value></value>
		public float JointSpeed
		{
			get
			{
				Transform xf1, xf2;
				this.BodyA.GetTransform(out xf1);
				this.BodyB.GetTransform(out xf2);

				Vector2 r1 = MathUtils.Multiply(ref xf1.R, this.LocalAnchorA - this.BodyA.LocalCenter);
				Vector2 r2 = MathUtils.Multiply(ref xf2.R, this.LocalAnchorB - this.BodyB.LocalCenter);
				Vector2 p1 = this.BodyA.Sweep.C + r1;
				Vector2 p2 = this.BodyB.Sweep.C + r2;
				Vector2 d = p2 - p1;
				Vector2 axis = this.BodyA.GetWorldVector(ref this._localXAxis1);

				Vector2 v1 = this.BodyA.LinearVelocityInternal;
				Vector2 v2 = this.BodyB.LinearVelocityInternal;
				float w1 = this.BodyA.AngularVelocityInternal;
				float w2 = this.BodyB.AngularVelocityInternal;

				float speed = Vector2.Dot(d, MathUtils.Cross(w1, axis)) +
							  Vector2.Dot(axis, v2 + MathUtils.Cross(w2, r2) - v1 - MathUtils.Cross(w1, r1));
				return speed;
			}
		}

		/// <summary>
		/// Is the joint limit enabled?
		/// </summary>
		/// <value><c>true</c> if [limit enabled]; otherwise, <c>false</c>.</value>
		public bool LimitEnabled
		{
			get { return this._enableLimit; }
			set
			{
				Debug.Assert(this.BodyA.FixedRotation == false || this.BodyB.FixedRotation == false,
							 "Warning: limits does currently not work with fixed rotation");

				WakeBodies();
				this._enableLimit = value;
			}
		}

		/// <summary>
		/// Get the lower joint limit, usually in meters.
		/// </summary>
		/// <value></value>
		public float LowerLimit
		{
			get { return this._lowerTranslation; }
			set
			{
				WakeBodies();
				this._lowerTranslation = value;
			}
		}

		/// <summary>
		/// Get the upper joint limit, usually in meters.
		/// </summary>
		/// <value></value>
		public float UpperLimit
		{
			get { return this._upperTranslation; }
			set
			{
				WakeBodies();
				this._upperTranslation = value;
			}
		}

		/// <summary>
		/// Is the joint motor enabled?
		/// </summary>
		/// <value><c>true</c> if [motor enabled]; otherwise, <c>false</c>.</value>
		public bool MotorEnabled
		{
			get { return this._enableMotor; }
			set
			{
				WakeBodies();
				this._enableMotor = value;
			}
		}

		/// <summary>
		/// Set the motor speed, usually in meters per second.
		/// </summary>
		/// <value>The speed.</value>
		public float MotorSpeed
		{
			set
			{
				WakeBodies();
				this._motorSpeed = value;
			}
			get { return this._motorSpeed; }
		}

		/// <summary>
		/// Set the maximum motor force, usually in N.
		/// </summary>
		/// <value>The force.</value>
		public float MaxMotorForce
		{
			get { return this._maxMotorForce; }
			set
			{
				WakeBodies();
				this._maxMotorForce = value;
			}
		}

		/// <summary>
		/// Get the current motor force, usually in N.
		/// </summary>
		/// <value></value>
		public float MotorForce
		{
			get { return this._motorImpulse; }
			set { this._motorImpulse = value; }
		}

		public Vector2 LocalXAxis1
		{
			get { return this._localXAxis1; }
			set
			{
				this._localXAxis1 = this.BodyA.GetLocalVector(value);
				this._localYAxis1 = MathUtils.Cross(1.0f, this._localXAxis1);
			}
		}

		public float ReferenceAngle
		{
			get { return this._refAngle; }
			set { WakeBodies(); this._refAngle = value; }
		}

		public override Vector2 GetReactionForce(float inv_dt)
		{
			return inv_dt * (this._impulse.X * this._perp + (this._motorImpulse + this._impulse.Z) * this._axis);
		}

		public override float GetReactionTorque(float inv_dt)
		{
			return inv_dt * this._impulse.Y;
		}

		internal override void InitVelocityConstraints(ref TimeStep step)
		{
			Body b1 = this.BodyA;
			Body b2 = this.BodyB;

			this.LocalCenterA = b1.LocalCenter;
			this.LocalCenterB = b2.LocalCenter;

			Transform xf1, xf2;
			b1.GetTransform(out xf1);
			b2.GetTransform(out xf2);

			// Compute the effective masses.
			Vector2 r1 = MathUtils.Multiply(ref xf1.R, this.LocalAnchorA - this.LocalCenterA);
			Vector2 r2 = MathUtils.Multiply(ref xf2.R, this.LocalAnchorB - this.LocalCenterB);
			Vector2 d = b2.Sweep.C + r2 - b1.Sweep.C - r1;

			this.InvMassA = b1.InvMass;
			this.InvIA = b1.InvI;
			this.InvMassB = b2.InvMass;
			this.InvIB = b2.InvI;

			// Compute motor Jacobian and effective mass.
			{
				this._axis = MathUtils.Multiply(ref xf1.R, this._localXAxis1);
				this._a1 = MathUtils.Cross(d + r1, this._axis);
				this._a2 = MathUtils.Cross(r2, this._axis);

				this._motorMass = this.InvMassA + this.InvMassB + this.InvIA * this._a1 * this._a1 + this.InvIB * this._a2 * this._a2;

				if (this._motorMass > Settings.Epsilon)
				{
					this._motorMass = 1.0f / this._motorMass;
				}
			}

			// Prismatic constraint.
			{
				this._perp = MathUtils.Multiply(ref xf1.R, this._localYAxis1);

				this._s1 = MathUtils.Cross(d + r1, this._perp);
				this._s2 = MathUtils.Cross(r2, this._perp);

				float m1 = this.InvMassA, m2 = this.InvMassB;
				float i1 = this.InvIA, i2 = this.InvIB;

				float k11 = m1 + m2 + i1 * this._s1 * this._s1 + i2 * this._s2 * this._s2;
				float k12 = i1 * this._s1 + i2 * this._s2;
				float k13 = i1 * this._s1 * this._a1 + i2 * this._s2 * this._a2;
				float k22 = i1 + i2;
				float k23 = i1 * this._a1 + i2 * this._a2;
				float k33 = m1 + m2 + i1 * this._a1 * this._a1 + i2 * this._a2 * this._a2;

				this._K.Col1 = new Vector3(k11, k12, k13);
				this._K.Col2 = new Vector3(k12, k22, k23);
				this._K.Col3 = new Vector3(k13, k23, k33);
			}

			// Compute motor and limit terms.
			if (this._enableLimit)
			{
				float jointTranslation = Vector2.Dot(this._axis, d);
				if (Math.Abs(this._upperTranslation - this._lowerTranslation) < 2.0f * Settings.LinearSlop)
				{
					this._limitState = LimitState.Equal;
				}
				else if (jointTranslation <= this._lowerTranslation)
				{
					if (this._limitState != LimitState.AtLower)
					{
						this._limitState = LimitState.AtLower;
						this._impulse.Z = 0.0f;
					}
				}
				else if (jointTranslation >= this._upperTranslation)
				{
					if (this._limitState != LimitState.AtUpper)
					{
						this._limitState = LimitState.AtUpper;
						this._impulse.Z = 0.0f;
					}
				}
				else
				{
					this._limitState = LimitState.Inactive;
					this._impulse.Z = 0.0f;
				}
			}
			else
			{
				this._limitState = LimitState.Inactive;
			}

			if (this._enableMotor == false)
			{
				this._motorImpulse = 0.0f;
			}

#pragma warning disable CS0162 // Unreachable code detected
			if (Settings.EnableWarmstarting)
			{
				// Account for variable time step.
				this._impulse *= step.dtRatio;
				this._motorImpulse *= step.dtRatio;

				Vector2 P = this._impulse.X * this._perp + (this._motorImpulse + this._impulse.Z) * this._axis;
				float L1 = this._impulse.X * this._s1 + this._impulse.Y + (this._motorImpulse + this._impulse.Z) * this._a1;
				float L2 = this._impulse.X * this._s2 + this._impulse.Y + (this._motorImpulse + this._impulse.Z) * this._a2;

				b1.LinearVelocityInternal -= this.InvMassA * P;
				b1.AngularVelocityInternal -= this.InvIA * L1;

				b2.LinearVelocityInternal += this.InvMassB * P;
				b2.AngularVelocityInternal += this.InvIB * L2;
			}
			else
			{
				this._impulse = Vector3.Zero;
				this._motorImpulse = 0.0f;
			}
#pragma warning restore CS0162 // Unreachable code detected
		}

		internal override void SolveVelocityConstraints(ref TimeStep step)
		{
			Body b1 = this.BodyA;
			Body b2 = this.BodyB;

			Vector2 v1 = b1.LinearVelocityInternal;
			float w1 = b1.AngularVelocityInternal;
			Vector2 v2 = b2.LinearVelocityInternal;
			float w2 = b2.AngularVelocityInternal;

			// Solve linear motor constraint.
			if (this._enableMotor && this._limitState != LimitState.Equal)
			{
				float Cdot = Vector2.Dot(this._axis, v2 - v1) + this._a2 * w2 - this._a1 * w1;
				float impulse = this._motorMass * (this._motorSpeed - Cdot);
				float oldImpulse = this._motorImpulse;
				float maxImpulse = step.dt * this._maxMotorForce;
				this._motorImpulse = MathUtils.Clamp(this._motorImpulse + impulse, -maxImpulse, maxImpulse);
				impulse = this._motorImpulse - oldImpulse;

				Vector2 P = impulse * this._axis;
				float L1 = impulse * this._a1;
				float L2 = impulse * this._a2;

				v1 -= this.InvMassA * P;
				w1 -= this.InvIA * L1;

				v2 += this.InvMassB * P;
				w2 += this.InvIB * L2;
			}

			Vector2 Cdot1 = new Vector2(Vector2.Dot(this._perp, v2 - v1) + this._s2 * w2 - this._s1 * w1, w2 - w1);

			if (this._enableLimit && this._limitState != LimitState.Inactive)
			{
				// Solve prismatic and limit constraint in block form.
				float Cdot2 = Vector2.Dot(this._axis, v2 - v1) + this._a2 * w2 - this._a1 * w1;
				Vector3 Cdot = new Vector3(Cdot1.X, Cdot1.Y, Cdot2);

				Vector3 f1 = this._impulse;
				Vector3 df = this._K.Solve33(-Cdot);
				this._impulse += df;

				if (this._limitState == LimitState.AtLower)
				{
					this._impulse.Z = Math.Max(this._impulse.Z, 0.0f);
				}
				else if (this._limitState == LimitState.AtUpper)
				{
					this._impulse.Z = Math.Min(this._impulse.Z, 0.0f);
				}

				// f2(1:2) = invK(1:2,1:2) * (-Cdot(1:2) - K(1:2,3) * (f2(3) - f1(3))) + f1(1:2)
				Vector2 b = -Cdot1 - (this._impulse.Z - f1.Z) * new Vector2(this._K.Col3.X, this._K.Col3.Y);
				Vector2 f2r = this._K.Solve22(b) + new Vector2(f1.X, f1.Y);
				this._impulse.X = f2r.X;
				this._impulse.Y = f2r.Y;

				df = this._impulse - f1;

				Vector2 P = df.X * this._perp + df.Z * this._axis;
				float L1 = df.X * this._s1 + df.Y + df.Z * this._a1;
				float L2 = df.X * this._s2 + df.Y + df.Z * this._a2;

				v1 -= this.InvMassA * P;
				w1 -= this.InvIA * L1;

				v2 += this.InvMassB * P;
				w2 += this.InvIB * L2;
			}
			else
			{
				// Limit is inactive, just solve the prismatic constraint in block form.
				Vector2 df = this._K.Solve22(-Cdot1);
				this._impulse.X += df.X;
				this._impulse.Y += df.Y;

				Vector2 P = df.X * this._perp;
				float L1 = df.X * this._s1 + df.Y;
				float L2 = df.X * this._s2 + df.Y;

				v1 -= this.InvMassA * P;
				w1 -= this.InvIA * L1;

				v2 += this.InvMassB * P;
				w2 += this.InvIB * L2;
			}

			b1.LinearVelocityInternal = v1;
			b1.AngularVelocityInternal = w1;
			b2.LinearVelocityInternal = v2;
			b2.AngularVelocityInternal = w2;
		}

		internal override bool SolvePositionConstraints()
		{
			Body b1 = this.BodyA;
			Body b2 = this.BodyB;

			Vector2 c1 = b1.Sweep.C;
			float a1 = b1.Sweep.A;

			Vector2 c2 = b2.Sweep.C;
			float a2 = b2.Sweep.A;

			// Solve linear limit constraint.
			float linearError = 0.0f;
			bool active = false;
			float C2 = 0.0f;

			Mat22 R1 = new Mat22(a1);
			Mat22 R2 = new Mat22(a2);

			Vector2 r1 = MathUtils.Multiply(ref R1, this.LocalAnchorA - this.LocalCenterA);
			Vector2 r2 = MathUtils.Multiply(ref R2, this.LocalAnchorB - this.LocalCenterB);
			Vector2 d = c2 + r2 - c1 - r1;

			if (this._enableLimit)
			{
				this._axis = MathUtils.Multiply(ref R1, this._localXAxis1);

				this._a1 = MathUtils.Cross(d + r1, this._axis);
				this._a2 = MathUtils.Cross(r2, this._axis);

				float translation = Vector2.Dot(this._axis, d);
				if (Math.Abs(this._upperTranslation - this._lowerTranslation) < 2.0f * Settings.LinearSlop)
				{
					// Prevent large angular corrections
					C2 = MathUtils.Clamp(translation, -Settings.MaxLinearCorrection, Settings.MaxLinearCorrection);
					linearError = Math.Abs(translation);
					active = true;
				}
				else if (translation <= this._lowerTranslation)
				{
					// Prevent large linear corrections and allow some slop.
					C2 = MathUtils.Clamp(translation - this._lowerTranslation + Settings.LinearSlop,
										 -Settings.MaxLinearCorrection, 0.0f);
					linearError = this._lowerTranslation - translation;
					active = true;
				}
				else if (translation >= this._upperTranslation)
				{
					// Prevent large linear corrections and allow some slop.
					C2 = MathUtils.Clamp(translation - this._upperTranslation - Settings.LinearSlop, 0.0f,
										 Settings.MaxLinearCorrection);
					linearError = translation - this._upperTranslation;
					active = true;
				}
			}

			this._perp = MathUtils.Multiply(ref R1, this._localYAxis1);

			this._s1 = MathUtils.Cross(d + r1, this._perp);
			this._s2 = MathUtils.Cross(r2, this._perp);

			Vector3 impulse;
			Vector2 C1 = new Vector2(Vector2.Dot(this._perp, d), a2 - a1 - this.ReferenceAngle);

			linearError = Math.Max(linearError, Math.Abs(C1.X));
			float angularError = Math.Abs(C1.Y);

			if (active)
			{
				float m1 = this.InvMassA, m2 = this.InvMassB;
				float i1 = this.InvIA, i2 = this.InvIB;

				float k11 = m1 + m2 + i1 * this._s1 * this._s1 + i2 * this._s2 * this._s2;
				float k12 = i1 * this._s1 + i2 * this._s2;
				float k13 = i1 * this._s1 * this._a1 + i2 * this._s2 * this._a2;
				float k22 = i1 + i2;
				float k23 = i1 * this._a1 + i2 * this._a2;
				float k33 = m1 + m2 + i1 * this._a1 * this._a1 + i2 * this._a2 * this._a2;

				this._K.Col1 = new Vector3(k11, k12, k13);
				this._K.Col2 = new Vector3(k12, k22, k23);
				this._K.Col3 = new Vector3(k13, k23, k33);

				Vector3 C = new Vector3(-C1.X, -C1.Y, -C2);
				impulse = this._K.Solve33(C); // negated above
			}
			else
			{
				float m1 = this.InvMassA, m2 = this.InvMassB;
				float i1 = this.InvIA, i2 = this.InvIB;

				float k11 = m1 + m2 + i1 * this._s1 * this._s1 + i2 * this._s2 * this._s2;
				float k12 = i1 * this._s1 + i2 * this._s2;
				float k22 = i1 + i2;

				this._K.Col1 = new Vector3(k11, k12, 0.0f);
				this._K.Col2 = new Vector3(k12, k22, 0.0f);

				Vector2 impulse1 = this._K.Solve22(-C1);
				impulse.X = impulse1.X;
				impulse.Y = impulse1.Y;
				impulse.Z = 0.0f;
			}

			Vector2 P = impulse.X * this._perp + impulse.Z * this._axis;
			float L1 = impulse.X * this._s1 + impulse.Y + impulse.Z * this._a1;
			float L2 = impulse.X * this._s2 + impulse.Y + impulse.Z * this._a2;

			c1 -= this.InvMassA * P;
			a1 -= this.InvIA * L1;
			c2 += this.InvMassB * P;
			a2 += this.InvIB * L2;

			// TODO_ERIN remove need for this.
			b1.Sweep.C = c1;
			b1.Sweep.A = a1;
			b2.Sweep.C = c2;
			b2.Sweep.A = a2;
			b1.SynchronizeTransform();
			b2.SynchronizeTransform();

			return linearError <= Settings.LinearSlop && angularError <= Settings.AngularSlop;
		}
	}
}