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

using System.Diagnostics;
using FarseerPhysics.Common;
using Duality;

namespace FarseerPhysics.Dynamics.Joints
{
	// Point-to-point constraint
	// Cdot = v2 - v1
	//      = v2 + cross(w2, r2) - v1 - cross(w1, r1)
	// J = [-I -r1_skew I r2_skew ]
	// Identity used:
	// w k % (rx i + ry j) = w * (-ry i + rx j)

	// Angle constraint
	// Cdot = w2 - w1
	// J = [0 0 -1 0 0 1]
	// K = invI1 + invI2

	/// <summary>
	/// Friction joint. This is used for top-down friction.
	/// It provides 2D translational friction and angular friction.
	/// </summary>
	public class FrictionJoint : Joint
	{
		public Vector2 LocalAnchorA;
		public Vector2 LocalAnchorB;
		private float _angularImpulse;
		private float _angularMass;
		private Vector2 _linearImpulse;
		private Mat22 _linearMass;

		internal FrictionJoint()
		{
			this.JointType = JointType.Friction;
		}

		public FrictionJoint(Body bodyA, Body bodyB, Vector2 localAnchorA, Vector2 localAnchorB)
			: base(bodyA, bodyB)
		{
			this.JointType = JointType.Friction;
			this.LocalAnchorA = localAnchorA;
			this.LocalAnchorB = localAnchorB;
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
		/// The maximum friction force in N.
		/// </summary>
		public float MaxForce { get; set; }

		/// <summary>
		/// The maximum friction torque in N-m.
		/// </summary>
		public float MaxTorque { get; set; }

		public override Vector2 GetReactionForce(float inv_dt)
		{
			return inv_dt * this._linearImpulse;
		}

		public override float GetReactionTorque(float inv_dt)
		{
			return inv_dt * this._angularImpulse;
		}

		internal override void InitVelocityConstraints(ref TimeStep step)
		{
			Body bA = this.BodyA;
			Body bB = this.BodyB;

			Transform xfA, xfB;
			bA.GetTransform(out xfA);
			bB.GetTransform(out xfB);

			// Compute the effective mass matrix.
			Vector2 rA = MathUtils.Multiply(ref xfA.R, this.LocalAnchorA - bA.LocalCenter);
			Vector2 rB = MathUtils.Multiply(ref xfB.R, this.LocalAnchorB - bB.LocalCenter);

			// J = [-I -r1_skew I r2_skew]
			//     [ 0       -1 0       1]
			// r_skew = [-ry; rx]

			// Matlab
			// K = [ mA+r1y^2*iA+mB+r2y^2*iB,  -r1y*iA*r1x-r2y*iB*r2x,          -r1y*iA-r2y*iB]
			//     [  -r1y*iA*r1x-r2y*iB*r2x, mA+r1x^2*iA+mB+r2x^2*iB,           r1x*iA+r2x*iB]
			//     [          -r1y*iA-r2y*iB,           r1x*iA+r2x*iB,                   iA+iB]

			float mA = bA.InvMass, mB = bB.InvMass;
			float iA = bA.InvI, iB = bB.InvI;

			Mat22 K1 = new Mat22();
			K1.Col1.X = mA + mB;
			K1.Col2.X = 0.0f;
			K1.Col1.Y = 0.0f;
			K1.Col2.Y = mA + mB;

			Mat22 K2 = new Mat22();
			K2.Col1.X = iA * rA.Y * rA.Y;
			K2.Col2.X = -iA * rA.X * rA.Y;
			K2.Col1.Y = -iA * rA.X * rA.Y;
			K2.Col2.Y = iA * rA.X * rA.X;

			Mat22 K3 = new Mat22();
			K3.Col1.X = iB * rB.Y * rB.Y;
			K3.Col2.X = -iB * rB.X * rB.Y;
			K3.Col1.Y = -iB * rB.X * rB.Y;
			K3.Col2.Y = iB * rB.X * rB.X;

			Mat22 K12;
			Mat22.Add(ref K1, ref K2, out K12);

			Mat22 K;
			Mat22.Add(ref K12, ref K3, out K);

			this._linearMass = K.Inverse;

			this._angularMass = iA + iB;
			if (this._angularMass > 0.0f)
			{
				this._angularMass = 1.0f / this._angularMass;
			}

			if (Settings.EnableWarmstarting)
			{
				// Scale impulses to support a variable time step.
				this._linearImpulse *= step.dtRatio;
				this._angularImpulse *= step.dtRatio;

				Vector2 P = new Vector2(this._linearImpulse.X, this._linearImpulse.Y);

				bA.LinearVelocityInternal -= mA * P;
				bA.AngularVelocityInternal -= iA * (MathUtils.Cross(rA, P) + this._angularImpulse);

				bB.LinearVelocityInternal += mB * P;
				bB.AngularVelocityInternal += iB * (MathUtils.Cross(rB, P) + this._angularImpulse);
			}
			else
			{
				this._linearImpulse = Vector2.Zero;
				this._angularImpulse = 0.0f;
			}
		}

		internal override void SolveVelocityConstraints(ref TimeStep step)
		{
			Body bA = this.BodyA;
			Body bB = this.BodyB;

			Vector2 vA = bA.LinearVelocityInternal;
			float wA = bA.AngularVelocityInternal;
			Vector2 vB = bB.LinearVelocityInternal;
			float wB = bB.AngularVelocityInternal;

			float mA = bA.InvMass, mB = bB.InvMass;
			float iA = bA.InvI, iB = bB.InvI;

			Transform xfA, xfB;
			bA.GetTransform(out xfA);
			bB.GetTransform(out xfB);

			Vector2 rA = MathUtils.Multiply(ref xfA.R, this.LocalAnchorA - bA.LocalCenter);
			Vector2 rB = MathUtils.Multiply(ref xfB.R, this.LocalAnchorB - bB.LocalCenter);

			// Solve angular friction
			{
				float Cdot = wB - wA;
				float impulse = -this._angularMass * Cdot;

				float oldImpulse = this._angularImpulse;
				float maxImpulse = step.dt * this.MaxTorque;
				this._angularImpulse = MathUtils.Clamp(this._angularImpulse + impulse, -maxImpulse, maxImpulse);
				impulse = this._angularImpulse - oldImpulse;

				wA -= iA * impulse;
				wB += iB * impulse;
			}

			// Solve linear friction
			{
				Vector2 Cdot = vB + MathUtils.Cross(wB, rB) - vA - MathUtils.Cross(wA, rA);

				Vector2 impulse = -MathUtils.Multiply(ref this._linearMass, Cdot);
				Vector2 oldImpulse = this._linearImpulse;
				this._linearImpulse += impulse;

				float maxImpulse = step.dt * this.MaxForce;

				if (this._linearImpulse.LengthSquared > maxImpulse * maxImpulse)
				{
					this._linearImpulse.Normalize();
					this._linearImpulse *= maxImpulse;
				}

				impulse = this._linearImpulse - oldImpulse;

				vA -= mA * impulse;
				wA -= iA * MathUtils.Cross(rA, impulse);

				vB += mB * impulse;
				wB += iB * MathUtils.Cross(rB, impulse);
			}

			bA.LinearVelocityInternal = vA;
			bA.AngularVelocityInternal = wA;
			bB.LinearVelocityInternal = vB;
			bB.AngularVelocityInternal = wB;
		}

		internal override bool SolvePositionConstraints()
		{
			return true;
		}
	}
}