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
	/// <summary>
	/// A revolute joint rains to bodies to share a common point while they
	/// are free to rotate about the point. The relative rotation about the shared
	/// point is the joint angle. You can limit the relative rotation with
	/// a joint limit that specifies a lower and upper angle. You can use a motor
	/// to drive the relative rotation about the shared point. A maximum motor torque
	/// is provided so that infinite forces are not generated.
	/// </summary>
	public class RevoluteJoint : Joint
	{
		public Vector2 LocalAnchorA;

		public Vector2 LocalAnchorB;
		private bool _enableLimit;
		private bool _enableMotor;
		private Vector3 _impulse;
		private LimitState _limitState;
		private float _lowerAngle;
		private Mat33 _mass; // effective mass for point-to-point constraint.
		private float _maxMotorTorque;
		private float _motorImpulse;
		private float _motorMass; // effective mass for motor/limit angular constraint.
		private float _motorSpeed;
		private float _referenceAngle;
		private float _tmpFloat1;
		private Vector2 _tmpVector1, _tmpVector2;
		private float _upperAngle;

		internal RevoluteJoint()
		{
			this.JointType = JointType.Revolute;
		}

		/// <summary>
		/// Initialize the bodies and local anchor.
		/// This requires defining an
		/// anchor point where the bodies are joined. The definition
		/// uses local anchor points so that the initial configuration
		/// can violate the constraint slightly. You also need to
		/// specify the initial relative angle for joint limits. This
		/// helps when saving and loading a game.
		/// The local anchor points are measured from the body's origin
		/// rather than the center of mass because:
		/// 1. you might not know where the center of mass will be.
		/// 2. if you add/remove shapes from a body and recompute the mass,
		/// the joints will be broken.
		/// </summary>
		/// <param name="bodyA">The first body.</param>
		/// <param name="bodyB">The second body.</param>
		/// <param name="localAnchorA">The first body anchor.</param>
		/// <param name="localAnchorB">The second anchor.</param>
		public RevoluteJoint(Body bodyA, Body bodyB, Vector2 localAnchorA, Vector2 localAnchorB)
			: base(bodyA, bodyB)
		{
			this.JointType = JointType.Revolute;

			// Changed to local coordinates.
			this.LocalAnchorA = localAnchorA;
			this.LocalAnchorB = localAnchorB;

			this.ReferenceAngle = this.BodyB.Rotation - this.BodyA.Rotation;

			this._impulse = Vector3.Zero;

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

		public float ReferenceAngle
		{
			get { return this._referenceAngle; }
			set
			{
				WakeBodies();
				this._referenceAngle = value;
			}
		}

		/// <summary>
		/// Get the current joint angle in radians.
		/// </summary>
		/// <value></value>
		public float JointAngle
		{
			get { return this.BodyB.Sweep.A - this.BodyA.Sweep.A - this.ReferenceAngle; }
		}

		/// <summary>
		/// Get the current joint angle speed in radians per second.
		/// </summary>
		/// <value></value>
		public float JointSpeed
		{
			get { return this.BodyB.AngularVelocityInternal - this.BodyA.AngularVelocityInternal; }
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
				WakeBodies();
				this._enableLimit = value;
			}
		}

		/// <summary>
		/// Get the lower joint limit in radians.
		/// </summary>
		/// <value></value>
		public float LowerLimit
		{
			get { return this._lowerAngle; }
			set
			{
				WakeBodies();
				this._lowerAngle = value;
			}
		}

		/// <summary>
		/// Get the upper joint limit in radians.
		/// </summary>
		/// <value></value>
		public float UpperLimit
		{
			get { return this._upperAngle; }
			set
			{
				WakeBodies();
				this._upperAngle = value;
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
		/// Set the motor speed in radians per second.
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
		/// Set the maximum motor torque, usually in N-m.
		/// </summary>
		/// <value>The torque.</value>
		public float MaxMotorTorque
		{
			set
			{
				WakeBodies();
				this._maxMotorTorque = value;
			}
			get { return this._maxMotorTorque; }
		}

		/// <summary>
		/// Get the current motor torque, usually in N-m.
		/// </summary>
		/// <value></value>
		public float MotorTorque
		{
			get { return this._motorImpulse; }
			set
			{
				WakeBodies();
				this._motorImpulse = value;
			}
		}

		public override Vector2 GetReactionForce(float inv_dt)
		{
			Vector2 P = new Vector2(this._impulse.X, this._impulse.Y);
			return inv_dt * P;
		}

		public override float GetReactionTorque(float inv_dt)
		{
			return inv_dt * this._impulse.Z;
		}

		internal override void InitVelocityConstraints(ref TimeStep step)
		{
			Body b1 = this.BodyA;
			Body b2 = this.BodyB;

			if (this._enableMotor || this._enableLimit)
			{
				// You cannot create a rotation limit between bodies that
				// both have fixed rotation.
				Debug.Assert(b1.InvI > 0.0f || b2.InvI > 0.0f);
			}

			// Compute the effective mass matrix.
			/*Transform xf1, xf2;
            b1.GetTransform(out xf1);
            b2.GetTransform(out xf2);*/

			Vector2 r1 = MathUtils.Multiply(ref b1.Xf.R, this.LocalAnchorA - b1.LocalCenter);
			Vector2 r2 = MathUtils.Multiply(ref b2.Xf.R, this.LocalAnchorB - b2.LocalCenter);

			// J = [-I -r1_skew I r2_skew]
			//     [ 0       -1 0       1]
			// r_skew = [-ry; rx]

			// Matlab
			// K = [ m1+r1y^2*i1+m2+r2y^2*i2,  -r1y*i1*r1x-r2y*i2*r2x,          -r1y*i1-r2y*i2]
			//     [  -r1y*i1*r1x-r2y*i2*r2x, m1+r1x^2*i1+m2+r2x^2*i2,           r1x*i1+r2x*i2]
			//     [          -r1y*i1-r2y*i2,           r1x*i1+r2x*i2,                   i1+i2]

			float m1 = b1.InvMass, m2 = b2.InvMass;
			float i1 = b1.InvI, i2 = b2.InvI;

			this._mass.Col1.X = m1 + m2 + r1.Y * r1.Y * i1 + r2.Y * r2.Y * i2;
			this._mass.Col2.X = -r1.Y * r1.X * i1 - r2.Y * r2.X * i2;
			this._mass.Col3.X = -r1.Y * i1 - r2.Y * i2;
			this._mass.Col1.Y = this._mass.Col2.X;
			this._mass.Col2.Y = m1 + m2 + r1.X * r1.X * i1 + r2.X * r2.X * i2;
			this._mass.Col3.Y = r1.X * i1 + r2.X * i2;
			this._mass.Col1.Z = this._mass.Col3.X;
			this._mass.Col2.Z = this._mass.Col3.Y;
			this._mass.Col3.Z = i1 + i2;

			this._motorMass = i1 + i2;
			if (this._motorMass > 0.0f)
			{
				this._motorMass = 1.0f / this._motorMass;
			}

			if (this._enableMotor == false)
			{
				this._motorImpulse = 0.0f;
			}

			if (this._enableLimit)
			{
				float jointAngle = b2.Sweep.A - b1.Sweep.A - this.ReferenceAngle;
				if (Math.Abs(this._upperAngle - this._lowerAngle) < 2.0f * Settings.AngularSlop)
				{
					this._limitState = LimitState.Equal;
				}
				else if (jointAngle <= this._lowerAngle)
				{
					if (this._limitState != LimitState.AtLower)
					{
						this._impulse.Z = 0.0f;
					}
					this._limitState = LimitState.AtLower;
				}
				else if (jointAngle >= this._upperAngle)
				{
					if (this._limitState != LimitState.AtUpper)
					{
						this._impulse.Z = 0.0f;
					}
					this._limitState = LimitState.AtUpper;
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

#pragma warning disable 0162 // Unreachable code detected
			if (Settings.EnableWarmstarting)
			{
				// Scale impulses to support a variable time step.
				this._impulse *= step.dtRatio;
				this._motorImpulse *= step.dtRatio;

				Vector2 P = new Vector2(this._impulse.X, this._impulse.Y);

				b1.LinearVelocityInternal -= m1 * P;
				MathUtils.Cross(ref r1, ref P, out this._tmpFloat1);
				b1.AngularVelocityInternal -= i1 * ( /* r1 x P */this._tmpFloat1 + this._motorImpulse + this._impulse.Z);

				b2.LinearVelocityInternal += m2 * P;
				MathUtils.Cross(ref r2, ref P, out this._tmpFloat1);
				b2.AngularVelocityInternal += i2 * ( /* r2 x P */this._tmpFloat1 + this._motorImpulse + this._impulse.Z);
			}
			else
			{
				this._impulse = Vector3.Zero;
				this._motorImpulse = 0.0f;
			}
#pragma warning restore 0162 // Unreachable code detected
		}

		internal override void SolveVelocityConstraints(ref TimeStep step)
		{
			Body b1 = this.BodyA;
			Body b2 = this.BodyB;

			Vector2 v1 = b1.LinearVelocityInternal;
			float w1 = b1.AngularVelocityInternal;
			Vector2 v2 = b2.LinearVelocityInternal;
			float w2 = b2.AngularVelocityInternal;

			float m1 = b1.InvMass, m2 = b2.InvMass;
			float i1 = b1.InvI, i2 = b2.InvI;

			// Solve motor constraint.
			if (this._enableMotor && this._limitState != LimitState.Equal)
			{
				float Cdot = w2 - w1 - this._motorSpeed;
				float impulse = this._motorMass * (-Cdot);
				float oldImpulse = this._motorImpulse;
				float maxImpulse = step.dt * this._maxMotorTorque;
				this._motorImpulse = MathUtils.Clamp(this._motorImpulse + impulse, -maxImpulse, maxImpulse);
				impulse = this._motorImpulse - oldImpulse;

				w1 -= i1 * impulse;
				w2 += i2 * impulse;
			}

			// Solve limit constraint.
			if (this._enableLimit && this._limitState != LimitState.Inactive)
			{
				/*Transform xf1, xf2;
                b1.GetTransform(out xf1);
                b2.GetTransform(out xf2);*/

				Vector2 r1 = MathUtils.Multiply(ref b1.Xf.R, this.LocalAnchorA - b1.LocalCenter);
				Vector2 r2 = MathUtils.Multiply(ref b2.Xf.R, this.LocalAnchorB - b2.LocalCenter);

				// Solve point-to-point constraint
				MathUtils.Cross(w2, ref r2, out this._tmpVector2);
				MathUtils.Cross(w1, ref r1, out this._tmpVector1);
				Vector2 Cdot1 = v2 + /* w2 x r2 */ this._tmpVector2 - v1 - /* w1 x r1 */ this._tmpVector1;
				float Cdot2 = w2 - w1;
				Vector3 Cdot = new Vector3(Cdot1.X, Cdot1.Y, Cdot2);

				Vector3 impulse = this._mass.Solve33(-Cdot);

				if (this._limitState == LimitState.Equal)
				{
					this._impulse += impulse;
				}
				else if (this._limitState == LimitState.AtLower)
				{
					float newImpulse = this._impulse.Z + impulse.Z;
					if (newImpulse < 0.0f)
					{
						Vector2 reduced = this._mass.Solve22(-Cdot1);
						impulse.X = reduced.X;
						impulse.Y = reduced.Y;
						impulse.Z = -this._impulse.Z;
						this._impulse.X += reduced.X;
						this._impulse.Y += reduced.Y;
						this._impulse.Z = 0.0f;
					}
				}
				else if (this._limitState == LimitState.AtUpper)
				{
					float newImpulse = this._impulse.Z + impulse.Z;
					if (newImpulse > 0.0f)
					{
						Vector2 reduced = this._mass.Solve22(-Cdot1);
						impulse.X = reduced.X;
						impulse.Y = reduced.Y;
						impulse.Z = -this._impulse.Z;
						this._impulse.X += reduced.X;
						this._impulse.Y += reduced.Y;
						this._impulse.Z = 0.0f;
					}
				}

				Vector2 P = new Vector2(impulse.X, impulse.Y);

				v1 -= m1 * P;
				MathUtils.Cross(ref r1, ref P, out this._tmpFloat1);
				w1 -= i1 * ( /* r1 x P */this._tmpFloat1 + impulse.Z);

				v2 += m2 * P;
				MathUtils.Cross(ref r2, ref P, out this._tmpFloat1);
				w2 += i2 * ( /* r2 x P */this._tmpFloat1 + impulse.Z);
			}
			else
			{
				/*Transform xf1, xf2;
                b1.GetTransform(out xf1);
                b2.GetTransform(out xf2);*/

				this._tmpVector1 = this.LocalAnchorA - b1.LocalCenter;
				this._tmpVector2 = this.LocalAnchorB - b2.LocalCenter;
				Vector2 r1 = MathUtils.Multiply(ref b1.Xf.R, ref this._tmpVector1);
				Vector2 r2 = MathUtils.Multiply(ref b2.Xf.R, ref this._tmpVector2);

				// Solve point-to-point constraint
				MathUtils.Cross(w2, ref r2, out this._tmpVector2);
				MathUtils.Cross(w1, ref r1, out this._tmpVector1);
				Vector2 Cdot = v2 + /* w2 x r2 */ this._tmpVector2 - v1 - /* w1 x r1 */ this._tmpVector1;
				Vector2 impulse = this._mass.Solve22(-Cdot);

				this._impulse.X += impulse.X;
				this._impulse.Y += impulse.Y;

				v1 -= m1 * impulse;
				MathUtils.Cross(ref r1, ref impulse, out this._tmpFloat1);
				w1 -= i1 * /* r1 x impulse */ this._tmpFloat1;

				v2 += m2 * impulse;
				MathUtils.Cross(ref r2, ref impulse, out this._tmpFloat1);
				w2 += i2 * /* r2 x impulse */ this._tmpFloat1;
			}

			b1.LinearVelocityInternal = v1;
			b1.AngularVelocityInternal = w1;
			b2.LinearVelocityInternal = v2;
			b2.AngularVelocityInternal = w2;
		}

		internal override bool SolvePositionConstraints()
		{
			// TODO_ERIN block solve with limit. COME ON ERIN

			Body b1 = this.BodyA;
			Body b2 = this.BodyB;

			float angularError = 0.0f;
			float positionError;

			// Solve angular limit constraint.
			if (this._enableLimit && this._limitState != LimitState.Inactive)
			{
				float angle = b2.Sweep.A - b1.Sweep.A - this.ReferenceAngle;
				float limitImpulse = 0.0f;

				if (this._limitState == LimitState.Equal)
				{
					// Prevent large angular corrections
					float C = MathUtils.Clamp(angle - this._lowerAngle, -Settings.MaxAngularCorrection,
											  Settings.MaxAngularCorrection);
					limitImpulse = -this._motorMass * C;
					angularError = Math.Abs(C);
				}
				else if (this._limitState == LimitState.AtLower)
				{
					float C = angle - this._lowerAngle;
					angularError = -C;

					// Prevent large angular corrections and allow some slop.
					C = MathUtils.Clamp(C + Settings.AngularSlop, -Settings.MaxAngularCorrection, 0.0f);
					limitImpulse = -this._motorMass * C;
				}
				else if (this._limitState == LimitState.AtUpper)
				{
					float C = angle - this._upperAngle;
					angularError = C;

					// Prevent large angular corrections and allow some slop.
					C = MathUtils.Clamp(C - Settings.AngularSlop, 0.0f, Settings.MaxAngularCorrection);
					limitImpulse = -this._motorMass * C;
				}

				b1.Sweep.A -= b1.InvI * limitImpulse;
				b2.Sweep.A += b2.InvI * limitImpulse;

				b1.SynchronizeTransform();
				b2.SynchronizeTransform();
			}

			// Solve point-to-point constraint.
			{
				/*Transform xf1, xf2;
                b1.GetTransform(out xf1);
                b2.GetTransform(out xf2);*/

				Vector2 r1 = MathUtils.Multiply(ref b1.Xf.R, this.LocalAnchorA - b1.LocalCenter);
				Vector2 r2 = MathUtils.Multiply(ref b2.Xf.R, this.LocalAnchorB - b2.LocalCenter);

				Vector2 C = b2.Sweep.C + r2 - b1.Sweep.C - r1;
				positionError = C.Length;

				float invMass1 = b1.InvMass, invMass2 = b2.InvMass;
				float invI1 = b1.InvI, invI2 = b2.InvI;

				// Handle large detachment.
				const float k_allowedStretch = 10.0f * Settings.LinearSlop;
				if (C.LengthSquared > k_allowedStretch * k_allowedStretch)
				{
					// Use a particle solution (no rotation).
					Vector2 u = C;
					u.Normalize();
					float k = invMass1 + invMass2;
					Debug.Assert(k > Settings.Epsilon);
					float m = 1.0f / k;
					Vector2 impulse2 = m * (-C);
					const float k_beta = 0.5f;
					b1.Sweep.C -= k_beta * invMass1 * impulse2;
					b2.Sweep.C += k_beta * invMass2 * impulse2;

					C = b2.Sweep.C + r2 - b1.Sweep.C - r1;
				}

				Mat22 K1 = new Mat22(new Vector2(invMass1 + invMass2, 0.0f), new Vector2(0.0f, invMass1 + invMass2));
				Mat22 K2 = new Mat22(new Vector2(invI1 * r1.Y * r1.Y, -invI1 * r1.X * r1.Y),
									 new Vector2(-invI1 * r1.X * r1.Y, invI1 * r1.X * r1.X));
				Mat22 K3 = new Mat22(new Vector2(invI2 * r2.Y * r2.Y, -invI2 * r2.X * r2.Y),
									 new Vector2(-invI2 * r2.X * r2.Y, invI2 * r2.X * r2.X));

				Mat22 Ka;
				Mat22.Add(ref K1, ref K2, out Ka);

				Mat22 K;
				Mat22.Add(ref Ka, ref K3, out K);


				Vector2 impulse = K.Solve(-C);

				b1.Sweep.C -= b1.InvMass * impulse;
				MathUtils.Cross(ref r1, ref impulse, out this._tmpFloat1);
				b1.Sweep.A -= b1.InvI * /* r1 x impulse */ this._tmpFloat1;

				b2.Sweep.C += b2.InvMass * impulse;
				MathUtils.Cross(ref r2, ref impulse, out this._tmpFloat1);
				b2.Sweep.A += b2.InvI * /* r2 x impulse */ this._tmpFloat1;

				b1.SynchronizeTransform();
				b2.SynchronizeTransform();
			}

			return positionError <= Settings.LinearSlop && angularError <= Settings.AngularSlop;
		}
	}
}