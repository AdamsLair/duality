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
	// Point-to-point constraint
	// C = p2 - p1
	// Cdot = v2 - v1
	//      = v2 + cross(w2, r2) - v1 - cross(w1, r1)
	// J = [-I -r1_skew I r2_skew ]
	// Identity used:
	// w k % (rx i + ry j) = w * (-ry i + rx j)

	// Angle constraint
	// C = angle2 - angle1 - referenceAngle
	// Cdot = w2 - w1
	// J = [0 0 -1 0 0 1]
	// K = invI1 + invI2

	/// <summary>
	/// A weld joint essentially glues two bodies together. A weld joint may
	/// distort somewhat because the island constraint solver is approximate.
	/// </summary>
	public class WeldJoint : Joint
	{
		public Vector2 LocalAnchorA;
		public Vector2 LocalAnchorB;
		/// <summary>
		/// The body2 angle minus body1 angle in the reference state (radians).
		/// </summary>
		public float ReferenceAngle;
		private Vector3 _impulse;
		private Mat33 _mass;

		internal WeldJoint()
		{
			this.JointType = JointType.Weld;
		}

		/// <summary>
		/// You need to specify a local anchor point
		/// where they are attached and the relative body angle. The position
		/// of the anchor point is important for computing the reaction torque.
		/// You can change the anchor points relative to bodyA or bodyB by changing LocalAnchorA
		/// and/or LocalAnchorB.
		/// </summary>
		/// <param name="bodyA">The first body</param>
		/// <param name="bodyB">The second body</param>
		/// <param name="localAnchorA">The first body anchor.</param>
		/// <param name="localAnchorB">The second body anchor.</param>
		public WeldJoint(Body bodyA, Body bodyB, Vector2 localAnchorA, Vector2 localAnchorB)
			: base(bodyA, bodyB)
		{
			this.JointType = JointType.Weld;

			this.LocalAnchorA = localAnchorA;
			this.LocalAnchorB = localAnchorB;
			this.ReferenceAngle = this.BodyB.Rotation - this.BodyA.Rotation;
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

		public override Vector2 GetReactionForce(float inv_dt)
		{
			return inv_dt * new Vector2(this._impulse.X, this._impulse.Y);
		}

		public override float GetReactionTorque(float inv_dt)
		{
			return inv_dt * this._impulse.Z;
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

			this._mass.Col1.X = mA + mB + rA.Y * rA.Y * iA + rB.Y * rB.Y * iB;
			this._mass.Col2.X = -rA.Y * rA.X * iA - rB.Y * rB.X * iB;
			this._mass.Col3.X = -rA.Y * iA - rB.Y * iB;
			this._mass.Col1.Y = this._mass.Col2.X;
			this._mass.Col2.Y = mA + mB + rA.X * rA.X * iA + rB.X * rB.X * iB;
			this._mass.Col3.Y = rA.X * iA + rB.X * iB;
			this._mass.Col1.Z = this._mass.Col3.X;
			this._mass.Col2.Z = this._mass.Col3.Y;
			this._mass.Col3.Z = iA + iB;

#pragma warning disable CS0162 // Unreachable code detected
			if (Settings.EnableWarmstarting)
			{
				// Scale impulses to support a variable time step.
				this._impulse *= step.dtRatio;

				Vector2 P = new Vector2(this._impulse.X, this._impulse.Y);

				bA.LinearVelocityInternal -= mA * P;
				bA.AngularVelocityInternal -= iA * (MathUtils.Cross(rA, P) + this._impulse.Z);

				bB.LinearVelocityInternal += mB * P;
				bB.AngularVelocityInternal += iB * (MathUtils.Cross(rB, P) + this._impulse.Z);
			}
			else
			{
				this._impulse = Vector3.Zero;
			}
#pragma warning restore CS0162 // Unreachable code detected
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

			//  Solve point-to-point constraint
			Vector2 Cdot1 = vB + MathUtils.Cross(wB, rB) - vA - MathUtils.Cross(wA, rA);
			float Cdot2 = wB - wA;
			Vector3 Cdot = new Vector3(Cdot1.X, Cdot1.Y, Cdot2);

			Vector3 impulse = this._mass.Solve33(-Cdot);
			this._impulse += impulse;

			Vector2 P = new Vector2(impulse.X, impulse.Y);

			vA -= mA * P;
			wA -= iA * (MathUtils.Cross(rA, P) + impulse.Z);

			vB += mB * P;
			wB += iB * (MathUtils.Cross(rB, P) + impulse.Z);

			bA.LinearVelocityInternal = vA;
			bA.AngularVelocityInternal = wA;
			bB.LinearVelocityInternal = vB;
			bB.AngularVelocityInternal = wB;
		}

		internal override bool SolvePositionConstraints()
		{
			Body bA = this.BodyA;
			Body bB = this.BodyB;

			float mA = bA.InvMass, mB = bB.InvMass;
			float iA = bA.InvI, iB = bB.InvI;

			Transform xfA;
			Transform xfB;
			bA.GetTransform(out xfA);
			bB.GetTransform(out xfB);

			Vector2 rA = MathUtils.Multiply(ref xfA.R, this.LocalAnchorA - bA.LocalCenter);
			Vector2 rB = MathUtils.Multiply(ref xfB.R, this.LocalAnchorB - bB.LocalCenter);

			Vector2 C1 = bB.Sweep.C + rB - bA.Sweep.C - rA;
			float C2 = bB.Sweep.A - bA.Sweep.A - this.ReferenceAngle;

			// Handle large detachment.
			const float k_allowedStretch = 10.0f * Settings.LinearSlop;
			float positionError = C1.Length;
			float angularError = Math.Abs(C2);
			if (positionError > k_allowedStretch)
			{
				iA *= 1.0f;
				iB *= 1.0f;
			}

			this._mass.Col1.X = mA + mB + rA.Y * rA.Y * iA + rB.Y * rB.Y * iB;
			this._mass.Col2.X = -rA.Y * rA.X * iA - rB.Y * rB.X * iB;
			this._mass.Col3.X = -rA.Y * iA - rB.Y * iB;
			this._mass.Col1.Y = this._mass.Col2.X;
			this._mass.Col2.Y = mA + mB + rA.X * rA.X * iA + rB.X * rB.X * iB;
			this._mass.Col3.Y = rA.X * iA + rB.X * iB;
			this._mass.Col1.Z = this._mass.Col3.X;
			this._mass.Col2.Z = this._mass.Col3.Y;
			this._mass.Col3.Z = iA + iB;

			Vector3 C = new Vector3(C1.X, C1.Y, C2);

			Vector3 impulse = this._mass.Solve33(-C);

			Vector2 P = new Vector2(impulse.X, impulse.Y);

			bA.Sweep.C -= mA * P;
			bA.Sweep.A -= iA * (MathUtils.Cross(rA, P) + impulse.Z);

			bB.Sweep.C += mB * P;
			bB.Sweep.A += iB * (MathUtils.Cross(rB, P) + impulse.Z);

			bA.SynchronizeTransform();
			bB.SynchronizeTransform();

			return positionError <= Settings.LinearSlop && angularError <= Settings.AngularSlop;
		}
	}
}