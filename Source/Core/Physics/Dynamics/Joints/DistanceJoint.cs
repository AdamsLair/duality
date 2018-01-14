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
	// 1-D rained system
	// m (v2 - v1) = lambda
	// v2 + (beta/h) * x1 + gamma * lambda = 0, gamma has units of inverse mass.
	// x2 = x1 + h * v2

	// 1-D mass-damper-spring system
	// m (v2 - v1) + h * d * v2 + h * k * 

	// C = norm(p2 - p1) - L
	// u = (p2 - p1) / norm(p2 - p1)
	// Cdot = dot(u, v2 + cross(w2, r2) - v1 - cross(w1, r1))
	// J = [-u -cross(r1, u) u cross(r2, u)]
	// K = J * invM * JT
	//   = invMass1 + invI1 * cross(r1, u)^2 + invMass2 + invI2 * cross(r2, u)^2

	/// <summary>
	/// A distance joint rains two points on two bodies
	/// to remain at a fixed distance from each other. You can view
	/// this as a massless, rigid rod.
	/// </summary>
	public class DistanceJoint : Joint
	{
		/// <summary>
		/// The local anchor point relative to bodyA's origin.
		/// </summary>
		public Vector2 LocalAnchorA;

		/// <summary>
		/// The local anchor point relative to bodyB's origin.
		/// </summary>
		public Vector2 LocalAnchorB;

		private float _bias;
		private float _gamma;
		private float _impulse;
		private float _mass;
		private float _tmpFloat1;
		private Vector2 _tmpVector1;
		private Vector2 _u;

		internal DistanceJoint()
		{
			this.JointType = JointType.Distance;
		}

		/// <summary>
		/// This requires defining an
		/// anchor point on both bodies and the non-zero length of the
		/// distance joint. If you don't supply a length, the local anchor points
		/// is used so that the initial configuration can violate the constraint
		/// slightly. This helps when saving and loading a game.
		/// @warning Do not use a zero or short length.
		/// </summary>
		/// <param name="bodyA">The first body</param>
		/// <param name="bodyB">The second body</param>
		/// <param name="localAnchorA">The first body anchor</param>
		/// <param name="localAnchorB">The second body anchor</param>
		public DistanceJoint(Body bodyA, Body bodyB, Vector2 localAnchorA, Vector2 localAnchorB)
			: base(bodyA, bodyB)
		{
			this.JointType = JointType.Distance;

			this.LocalAnchorA = localAnchorA;
			this.LocalAnchorB = localAnchorB;

			Vector2 d = this.WorldAnchorB - this.WorldAnchorA;
			this.Length = d.Length;
		}

		/// <summary>
		/// The natural length between the anchor points.
		/// Manipulating the length can lead to non-physical behavior when the frequency is zero.
		/// </summary>
		public float Length { get; set; }

		/// <summary>
		/// The mass-spring-damper frequency in Hertz.
		/// </summary>
		public float Frequency { get; set; }

		/// <summary>
		/// The damping ratio. 0 = no damping, 1 = critical damping.
		/// </summary>
		public float DampingRatio { get; set; }

		public override sealed Vector2 WorldAnchorA
		{
			get { return this.BodyA.GetWorldPoint(this.LocalAnchorA); }
		}

		public override sealed Vector2 WorldAnchorB
		{
			get { return this.BodyB.GetWorldPoint(this.LocalAnchorB); }
			set { Debug.Assert(false, "You can't set the world anchor on this joint type."); }
		}

		public override Vector2 GetReactionForce(float inv_dt)
		{
			Vector2 F = (inv_dt * this._impulse) * this._u;
			return F;
		}

		public override float GetReactionTorque(float inv_dt)
		{
			return 0.0f;
		}

		internal override void InitVelocityConstraints(ref TimeStep step)
		{
			Body b1 = this.BodyA;
			Body b2 = this.BodyB;

			// Compute the effective mass matrix.
			Vector2 r1 = MathUtils.Multiply(ref b1.Xf.R, this.LocalAnchorA - b1.LocalCenter);
			Vector2 r2 = MathUtils.Multiply(ref b2.Xf.R, this.LocalAnchorB - b2.LocalCenter);
			this._u = b2.Sweep.C + r2 - b1.Sweep.C - r1;

			// Handle singularity.
			float length = this._u.Length;
			if (length > Settings.LinearSlop)
			{
				this._u *= 1.0f / length;
			}
			else
			{
				this._u = Vector2.Zero;
			}

			float cr1u, cr2u;
			MathUtils.Cross(ref r1, ref this._u, out cr1u);
			MathUtils.Cross(ref r2, ref this._u, out cr2u);
			float invMass = b1.InvMass + b1.InvI * cr1u * cr1u + b2.InvMass + b2.InvI * cr2u * cr2u;
			Debug.Assert(invMass > Settings.Epsilon);
			this._mass = invMass != 0.0f ? 1.0f / invMass : 0.0f;

			if (this.Frequency > 0.0f)
			{
				float C = length - this.Length;

				// Frequency
				float omega = 2.0f * Settings.Pi * this.Frequency;

				// Damping coefficient
				float d = 2.0f * this._mass * this.DampingRatio * omega;

				// Spring stiffness
				float k = this._mass * omega * omega;

				// magic formulas
				this._gamma = step.dt * (d + step.dt * k);
				this._gamma = this._gamma != 0.0f ? 1.0f / this._gamma : 0.0f;
				this._bias = C * step.dt * k * this._gamma;

				this._mass = invMass + this._gamma;
				this._mass = this._mass != 0.0f ? 1.0f / this._mass : 0.0f;
			}

#pragma warning disable 0162 // Unreachable code detected
			if (Settings.EnableWarmstarting)
			{
				// Scale the impulse to support a variable time step.
				this._impulse *= step.dtRatio;

				Vector2 P = this._impulse * this._u;
				b1.LinearVelocityInternal -= b1.InvMass * P;
				MathUtils.Cross(ref r1, ref P, out this._tmpFloat1);
				b1.AngularVelocityInternal -= b1.InvI * /* r1 x P */ this._tmpFloat1;
				b2.LinearVelocityInternal += b2.InvMass * P;
				MathUtils.Cross(ref r2, ref P, out this._tmpFloat1);
				b2.AngularVelocityInternal += b2.InvI * /* r2 x P */ this._tmpFloat1;
			}
			else
			{
				this._impulse = 0.0f;
			}
#pragma warning restore 0162 // Unreachable code detected
		}

		internal override void SolveVelocityConstraints(ref TimeStep step)
		{
			Body b1 = this.BodyA;
			Body b2 = this.BodyB;

			Transform xf1, xf2;
			b1.GetTransform(out xf1);
			b2.GetTransform(out xf2);

			Vector2 r1 = MathUtils.Multiply(ref xf1.R, this.LocalAnchorA - b1.LocalCenter);
			Vector2 r2 = MathUtils.Multiply(ref xf2.R, this.LocalAnchorB - b2.LocalCenter);

			// Cdot = dot(u, v + cross(w, r))
			MathUtils.Cross(b1.AngularVelocityInternal, ref r1, out this._tmpVector1);
			Vector2 v1 = b1.LinearVelocityInternal + this._tmpVector1;
			MathUtils.Cross(b2.AngularVelocityInternal, ref r2, out this._tmpVector1);
			Vector2 v2 = b2.LinearVelocityInternal + this._tmpVector1;
			float Cdot = Vector2.Dot(this._u, v2 - v1);

			float impulse = -this._mass * (Cdot + this._bias + this._gamma * this._impulse);
			this._impulse += impulse;

			Vector2 P = impulse * this._u;
			b1.LinearVelocityInternal -= b1.InvMass * P;
			MathUtils.Cross(ref r1, ref P, out this._tmpFloat1);
			b1.AngularVelocityInternal -= b1.InvI * this._tmpFloat1;
			b2.LinearVelocityInternal += b2.InvMass * P;
			MathUtils.Cross(ref r2, ref P, out this._tmpFloat1);
			b2.AngularVelocityInternal += b2.InvI * this._tmpFloat1;
		}

		internal override bool SolvePositionConstraints()
		{
			if (this.Frequency > 0.0f)
			{
				// There is no position correction for soft distance constraints.
				return true;
			}

			Body b1 = this.BodyA;
			Body b2 = this.BodyB;

			Transform xf1, xf2;
			b1.GetTransform(out xf1);
			b2.GetTransform(out xf2);

			Vector2 r1 = MathUtils.Multiply(ref xf1.R, this.LocalAnchorA - b1.LocalCenter);
			Vector2 r2 = MathUtils.Multiply(ref xf2.R, this.LocalAnchorB - b2.LocalCenter);

			Vector2 d = b2.Sweep.C + r2 - b1.Sweep.C - r1;

			float length = d.Length;

			if (length == 0.0f)
				return true;

			d /= length;
			float C = length - this.Length;
			C = MathUtils.Clamp(C, -Settings.MaxLinearCorrection, Settings.MaxLinearCorrection);

			float impulse = -this._mass * C;
			this._u = d;
			Vector2 P = impulse * this._u;

			b1.Sweep.C -= b1.InvMass * P;
			MathUtils.Cross(ref r1, ref P, out this._tmpFloat1);
			b1.Sweep.A -= b1.InvI * this._tmpFloat1;
			b2.Sweep.C += b2.InvMass * P;
			MathUtils.Cross(ref r2, ref P, out this._tmpFloat1);
			b2.Sweep.A += b2.InvI * this._tmpFloat1;

			b1.SynchronizeTransform();
			b2.SynchronizeTransform();

			return Math.Abs(C) < Settings.LinearSlop;
		}
	}
}