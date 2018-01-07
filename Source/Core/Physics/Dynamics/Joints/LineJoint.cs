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
	public class LineJoint : Joint
	{
		private Vector2 _ax, _ay;
		private float _bias;
		private bool _enableMotor;
		private float _gamma;
		private float _impulse;
		private Vector2 _localXAxis;
		private Vector2 _localYAxisA;
		private float _mass;
		private float _maxMotorTorque;
		private float _motorImpulse;
		private float _motorMass;
		private float _motorSpeed;

		private float _sAx;
		private float _sAy;
		private float _sBx;
		private float _sBy;

		private float _springImpulse;
		private float _springMass;

		// Linear constraint (point-to-line)
		// d = pB - pA = xB + rB - xA - rA
		// C = dot(ay, d)
		// Cdot = dot(d, cross(wA, ay)) + dot(ay, vB + cross(wB, rB) - vA - cross(wA, rA))
		//      = -dot(ay, vA) - dot(cross(d + rA, ay), wA) + dot(ay, vB) + dot(cross(rB, ay), vB)
		// J = [-ay, -cross(d + rA, ay), ay, cross(rB, ay)]

		// Spring linear constraint
		// C = dot(ax, d)
		// Cdot = = -dot(ax, vA) - dot(cross(d + rA, ax), wA) + dot(ax, vB) + dot(cross(rB, ax), vB)
		// J = [-ax -cross(d+rA, ax) ax cross(rB, ax)]

		// Motor rotational constraint
		// Cdot = wB - wA
		// J = [0 0 -1 0 0 1]

		internal LineJoint()
		{
			this.JointType = JointType.Line;
		}

		public LineJoint(Body bA, Body bB, Vector2 anchor, Vector2 axis)
			: base(bA, bB)
		{
			this.JointType = JointType.Line;

			this.LocalAnchorA = bA.GetLocalPoint(anchor);
			this.LocalAnchorB = bB.GetLocalPoint(anchor);
			this.LocalXAxis = bA.GetLocalVector(axis);
		}

		public Vector2 LocalAnchorA { get; set; }

		public Vector2 LocalAnchorB { get; set; }

		public override Vector2 WorldAnchorA
		{
			get { return this.BodyA.GetWorldPoint(this.LocalAnchorA); }
		}

		public override Vector2 WorldAnchorB
		{
			get { return this.BodyB.GetWorldPoint(this.LocalAnchorB); }
			set { Debug.Assert(false, "You can't set the world anchor on this joint type."); }
		}

		public float JointTranslation
		{
			get
			{
				Body bA = this.BodyA;
				Body bB = this.BodyB;

				Vector2 pA = bA.GetWorldPoint(this.LocalAnchorA);
				Vector2 pB = bB.GetWorldPoint(this.LocalAnchorB);
				Vector2 d = pB - pA;
				Vector2 axis = bA.GetWorldVector(this.LocalXAxis);

				float translation = Vector2.Dot(d, axis);
				return translation;
			}
		}

		public float JointSpeed
		{
			get
			{
				float wA = this.BodyA.AngularVelocityInternal;
				float wB = this.BodyB.AngularVelocityInternal;
				return wB - wA;
			}
		}

		public bool MotorEnabled
		{
			get { return this._enableMotor; }
			set
			{
				this.BodyA.Awake = true;
				this.BodyB.Awake = true;
				this._enableMotor = value;
			}
		}

		public float MotorSpeed
		{
			set
			{
				this.BodyA.Awake = true;
				this.BodyB.Awake = true;
				this._motorSpeed = value;
			}
			get { return this._motorSpeed; }
		}

		public float MaxMotorTorque
		{
			set
			{
				this.BodyA.Awake = true;
				this.BodyB.Awake = true;
				this._maxMotorTorque = value;
			}
			get { return this._maxMotorTorque; }
		}

		public float Frequency { get; set; }

		public float DampingRatio { get; set; }

		public Vector2 LocalXAxis
		{
			get { return this._localXAxis; }
			set
			{
				this._localXAxis = value;
				this._localYAxisA = MathUtils.Cross(1.0f, this._localXAxis);
			}
		}

		public override Vector2 GetReactionForce(float invDt)
		{
			return invDt * (this._impulse * this._ay + this._springImpulse * this._ax);
		}

		public override float GetReactionTorque(float invDt)
		{
			return invDt * this._motorImpulse;
		}

		internal override void InitVelocityConstraints(ref TimeStep step)
		{
			Body bA = this.BodyA;
			Body bB = this.BodyB;

			this.LocalCenterA = bA.LocalCenter;
			this.LocalCenterB = bB.LocalCenter;

			Transform xfA;
			bA.GetTransform(out xfA);
			Transform xfB;
			bB.GetTransform(out xfB);

			// Compute the effective masses.
			Vector2 rA = MathUtils.Multiply(ref xfA.R, this.LocalAnchorA - this.LocalCenterA);
			Vector2 rB = MathUtils.Multiply(ref xfB.R, this.LocalAnchorB - this.LocalCenterB);
			Vector2 d = bB.Sweep.C + rB - bA.Sweep.C - rA;

			this.InvMassA = bA.InvMass;
			this.InvIA = bA.InvI;
			this.InvMassB = bB.InvMass;
			this.InvIB = bB.InvI;

			// Point to line constraint
			{
				this._ay = MathUtils.Multiply(ref xfA.R, this._localYAxisA);
				this._sAy = MathUtils.Cross(d + rA, this._ay);
				this._sBy = MathUtils.Cross(rB, this._ay);

				this._mass = this.InvMassA + this.InvMassB + this.InvIA * this._sAy * this._sAy + this.InvIB * this._sBy * this._sBy;

				if (this._mass > 0.0f)
				{
					this._mass = 1.0f / this._mass;
				}
			}

			// Spring constraint
			this._springMass = 0.0f;
			if (this.Frequency > 0.0f)
			{
				this._ax = MathUtils.Multiply(ref xfA.R, this.LocalXAxis);
				this._sAx = MathUtils.Cross(d + rA, this._ax);
				this._sBx = MathUtils.Cross(rB, this._ax);

				float invMass = this.InvMassA + this.InvMassB + this.InvIA * this._sAx * this._sAx + this.InvIB * this._sBx * this._sBx;

				if (invMass > 0.0f)
				{
					this._springMass = 1.0f / invMass;

					float C = Vector2.Dot(d, this._ax);

					// Frequency
					float omega = 2.0f * Settings.Pi * this.Frequency;

					// Damping coefficient
					float da = 2.0f * this._springMass * this.DampingRatio * omega;

					// Spring stiffness
					float k = this._springMass * omega * omega;

					// magic formulas
					this._gamma = step.dt * (da + step.dt * k);
					if (this._gamma > 0.0f)
					{
						this._gamma = 1.0f / this._gamma;
					}

					this._bias = C * step.dt * k * this._gamma;

					this._springMass = invMass + this._gamma;
					if (this._springMass > 0.0f)
					{
						this._springMass = 1.0f / this._springMass;
					}
				}
			}
			else
			{
				this._springImpulse = 0.0f;
				this._springMass = 0.0f;
			}

			// Rotational motor
			if (this._enableMotor)
			{
				this._motorMass = this.InvIA + this.InvIB;
				if (this._motorMass > 0.0f)
				{
					this._motorMass = 1.0f / this._motorMass;
				}
			}
			else
			{
				this._motorMass = 0.0f;
				this._motorImpulse = 0.0f;
			}

			if (Settings.EnableWarmstarting)
			{
				// Account for variable time step.
				this._impulse *= step.dtRatio;
				this._springImpulse *= step.dtRatio;
				this._motorImpulse *= step.dtRatio;

				Vector2 P = this._impulse * this._ay + this._springImpulse * this._ax;
				float LA = this._impulse * this._sAy + this._springImpulse * this._sAx + this._motorImpulse;
				float LB = this._impulse * this._sBy + this._springImpulse * this._sBx + this._motorImpulse;

				bA.LinearVelocityInternal -= this.InvMassA * P;
				bA.AngularVelocityInternal -= this.InvIA * LA;

				bB.LinearVelocityInternal += this.InvMassB * P;
				bB.AngularVelocityInternal += this.InvIB * LB;
			}
			else
			{
				this._impulse = 0.0f;
				this._springImpulse = 0.0f;
				this._motorImpulse = 0.0f;
			}
		}

		internal override void SolveVelocityConstraints(ref TimeStep step)
		{
			Body bA = this.BodyA;
			Body bB = this.BodyB;

			Vector2 vA = bA.LinearVelocity;
			float wA = bA.AngularVelocityInternal;
			Vector2 vB = bB.LinearVelocityInternal;
			float wB = bB.AngularVelocityInternal;

			// Solve spring constraint
			{
				float Cdot = Vector2.Dot(this._ax, vB - vA) + this._sBx * wB - this._sAx * wA;
				float impulse = -this._springMass * (Cdot + this._bias + this._gamma * this._springImpulse);
				this._springImpulse += impulse;

				Vector2 P = impulse * this._ax;
				float LA = impulse * this._sAx;
				float LB = impulse * this._sBx;

				vA -= this.InvMassA * P;
				wA -= this.InvIA * LA;

				vB += this.InvMassB * P;
				wB += this.InvIB * LB;
			}

			// Solve rotational motor constraint
			{
				float Cdot = wB - wA - this._motorSpeed;
				float impulse = -this._motorMass * Cdot;

				float oldImpulse = this._motorImpulse;
				float maxImpulse = step.dt * this._maxMotorTorque;
				this._motorImpulse = MathUtils.Clamp(this._motorImpulse + impulse, -maxImpulse, maxImpulse);
				impulse = this._motorImpulse - oldImpulse;

				wA -= this.InvIA * impulse;
				wB += this.InvIB * impulse;
			}

			// Solve point to line constraint
			{
				float Cdot = Vector2.Dot(this._ay, vB - vA) + this._sBy * wB - this._sAy * wA;
				float impulse = this._mass * (-Cdot);
				this._impulse += impulse;

				Vector2 P = impulse * this._ay;
				float LA = impulse * this._sAy;
				float LB = impulse * this._sBy;

				vA -= this.InvMassA * P;
				wA -= this.InvIA * LA;

				vB += this.InvMassB * P;
				wB += this.InvIB * LB;
			}

			bA.LinearVelocityInternal = vA;
			bA.AngularVelocityInternal = wA;
			bB.LinearVelocityInternal = vB;
			bB.AngularVelocityInternal = wB;
		}

		internal override bool SolvePositionConstraints()
		{
			Body bA = this.BodyA;
			Body bB = this.BodyB;

			Vector2 xA = bA.Sweep.C;
			float angleA = bA.Sweep.A;

			Vector2 xB = bB.Sweep.C;
			float angleB = bB.Sweep.A;

			Mat22 RA = new Mat22(angleA);
			Mat22 RB = new Mat22(angleB);

			Vector2 rA = MathUtils.Multiply(ref RA, this.LocalAnchorA - this.LocalCenterA);
			Vector2 rB = MathUtils.Multiply(ref RB, this.LocalAnchorB - this.LocalCenterB);
			Vector2 d = xB + rB - xA - rA;

			Vector2 ay = MathUtils.Multiply(ref RA, this._localYAxisA);

			float sAy = MathUtils.Cross(d + rA, ay);
			float sBy = MathUtils.Cross(rB, ay);

			float C = Vector2.Dot(d, ay);

			float k = this.InvMassA + this.InvMassB + this.InvIA * this._sAy * this._sAy + this.InvIB * this._sBy * this._sBy;

			float impulse;
			if (k != 0.0f)
			{
				impulse = -C / k;
			}
			else
			{
				impulse = 0.0f;
			}

			Vector2 P = impulse * ay;
			float LA = impulse * sAy;
			float LB = impulse * sBy;

			xA -= this.InvMassA * P;
			angleA -= this.InvIA * LA;
			xB += this.InvMassB * P;
			angleB += this.InvIB * LB;

			// TODO_ERIN remove need for this.
			bA.Sweep.C = xA;
			bA.Sweep.A = angleA;
			bB.Sweep.C = xB;
			bB.Sweep.A = angleB;
			bA.SynchronizeTransform();
			bB.SynchronizeTransform();

			return Math.Abs(C) <= Settings.LinearSlop;
		}

		public float GetMotorTorque(float invDt)
		{
			return invDt * this._motorImpulse;
		}
	}
}