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
	/// The pulley joint is connected to two bodies and two fixed ground points.
	/// The pulley supports a ratio such that:
	/// length1 + ratio * length2 <!--<-->= ant
	/// Yes, the force transmitted is scaled by the ratio.
	/// The pulley also enforces a maximum length limit on both sides. This is
	/// useful to prevent one side of the pulley hitting the top.
	/// </summary>
	public class PulleyJoint : Joint
	{
		/// <summary>
		/// Get the first ground anchor.
		/// </summary>
		/// <value></value>
		public Vector2 GroundAnchorA;

		/// <summary>
		/// Get the second ground anchor.
		/// </summary>
		/// <value></value>
		public Vector2 GroundAnchorB;

		public Vector2 LocalAnchorA;
		public Vector2 LocalAnchorB;

		public float MinPulleyLength = 2.0f;
		private float _ant;
		private float _impulse;
		private float _lengthA;
		private float _lengthB;
		private float _limitImpulse1;
		private float _limitImpulse2;
		private float _limitMass1;
		private float _limitMass2;
		private LimitState _limitState1;
		private LimitState _limitState2;
		private float _maxLengthA;
		private float _maxLengthB;

		// Effective masses
		private float _pulleyMass;
		private LimitState _state;
		private Vector2 _u1;
		private Vector2 _u2;

		internal PulleyJoint()
		{
			this.JointType = JointType.Pulley;
		}

		/// <summary>
		/// Initialize the bodies, anchors, lengths, max lengths, and ratio using the world anchors.
		/// This requires two ground anchors,
		/// two dynamic body anchor points, max lengths for each side,
		/// and a pulley ratio.
		/// </summary>
		/// <param name="bodyA">The first body.</param>
		/// <param name="bodyB">The second body.</param>
		/// <param name="groundAnchorA">The ground anchor for the first body.</param>
		/// <param name="groundAnchorB">The ground anchor for the second body.</param>
		/// <param name="localAnchorA">The first body anchor.</param>
		/// <param name="localAnchorB">The second body anchor.</param>
		/// <param name="ratio">The ratio.</param>
		public PulleyJoint(Body bodyA, Body bodyB,
						   Vector2 groundAnchorA, Vector2 groundAnchorB,
						   Vector2 localAnchorA, Vector2 localAnchorB,
						   float ratio)
			: base(bodyA, bodyB)
		{
			this.JointType = JointType.Pulley;

			this.GroundAnchorA = groundAnchorA;
			this.GroundAnchorB = groundAnchorB;
			this.LocalAnchorA = localAnchorA;
			this.LocalAnchorB = localAnchorB;

			Vector2 d1 = this.BodyA.GetWorldPoint(localAnchorA) - groundAnchorA;
			this._lengthA = d1.Length;

			Vector2 d2 = this.BodyB.GetWorldPoint(localAnchorB) - groundAnchorB;
			this._lengthB = d2.Length;

			Debug.Assert(ratio != 0.0f);
			Debug.Assert(ratio > Settings.Epsilon);
			this.Ratio = ratio;

			float C = this._lengthA + this.Ratio * this._lengthB;

			this.MaxLengthA = C - this.Ratio * this.MinPulleyLength;
			this.MaxLengthB = (C - this.MinPulleyLength) / this.Ratio;

			this._ant = this._lengthA + this.Ratio * this._lengthB;

			this.MaxLengthA = Math.Min(this.MaxLengthA, this._ant - this.Ratio * this.MinPulleyLength);
			this.MaxLengthB = Math.Min(this.MaxLengthB, (this._ant - this.MinPulleyLength) / this.Ratio);

			this._impulse = 0.0f;
			this._limitImpulse1 = 0.0f;
			this._limitImpulse2 = 0.0f;
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
		/// Get the current length of the segment attached to body1.
		/// </summary>
		/// <value></value>
		public float LengthA
		{
			get
			{
				Vector2 d = this.BodyA.GetWorldPoint(this.LocalAnchorA) - this.GroundAnchorA;
				return d.Length;
			}
			set { this._lengthA = value; }
		}

		/// <summary>
		/// Get the current length of the segment attached to body2.
		/// </summary>
		/// <value></value>
		public float LengthB
		{
			get
			{
				Vector2 d = this.BodyB.GetWorldPoint(this.LocalAnchorB) - this.GroundAnchorB;
				return d.Length;
			}
			set { this._lengthB = value; }
		}

		/// <summary>
		/// Get the pulley ratio.
		/// </summary>
		/// <value></value>
		public float Ratio { get; set; }

		public float TotalLength
		{
			get { return this._ant; }
			set { this._ant = value; }
		}
		public float MaxLengthA
		{
			get { return this._maxLengthA; }
			set { this._maxLengthA = value; }
		}

		public float MaxLengthB
		{
			get { return this._maxLengthB; }
			set { this._maxLengthB = value; }
		}

		public override Vector2 GetReactionForce(float inv_dt)
		{
			Vector2 P = this._impulse * this._u2;
			return inv_dt * P;
		}

		public override float GetReactionTorque(float inv_dt)
		{
			return 0.0f;
		}

		internal override void InitVelocityConstraints(ref TimeStep step)
		{
			Body b1 = this.BodyA;
			Body b2 = this.BodyB;

			Transform xf1, xf2;
			b1.GetTransform(out xf1);
			b2.GetTransform(out xf2);

			Vector2 r1 = MathUtils.Multiply(ref xf1.R, this.LocalAnchorA - b1.LocalCenter);
			Vector2 r2 = MathUtils.Multiply(ref xf2.R, this.LocalAnchorB - b2.LocalCenter);

			Vector2 p1 = b1.Sweep.C + r1;
			Vector2 p2 = b2.Sweep.C + r2;

			Vector2 s1 = this.GroundAnchorA;
			Vector2 s2 = this.GroundAnchorB;

			// Get the pulley axes.
			this._u1 = p1 - s1;
			this._u2 = p2 - s2;

			float length1 = this._u1.Length;
			float length2 = this._u2.Length;

			if (length1 > Settings.LinearSlop)
			{
				this._u1 *= 1.0f / length1;
			}
			else
			{
				this._u1 = Vector2.Zero;
			}

			if (length2 > Settings.LinearSlop)
			{
				this._u2 *= 1.0f / length2;
			}
			else
			{
				this._u2 = Vector2.Zero;
			}

			float C = this._ant - length1 - this.Ratio * length2;
			if (C > 0.0f)
			{
				this._state = LimitState.Inactive;
				this._impulse = 0.0f;
			}
			else
			{
				this._state = LimitState.AtUpper;
			}

			if (length1 < this.MaxLengthA)
			{
				this._limitState1 = LimitState.Inactive;
				this._limitImpulse1 = 0.0f;
			}
			else
			{
				this._limitState1 = LimitState.AtUpper;
			}

			if (length2 < this.MaxLengthB)
			{
				this._limitState2 = LimitState.Inactive;
				this._limitImpulse2 = 0.0f;
			}
			else
			{
				this._limitState2 = LimitState.AtUpper;
			}

			// Compute effective mass.
			float cr1u1 = MathUtils.Cross(r1, this._u1);
			float cr2u2 = MathUtils.Cross(r2, this._u2);

			this._limitMass1 = b1.InvMass + b1.InvI * cr1u1 * cr1u1;
			this._limitMass2 = b2.InvMass + b2.InvI * cr2u2 * cr2u2;
			this._pulleyMass = this._limitMass1 + this.Ratio * this.Ratio * this._limitMass2;
			Debug.Assert(this._limitMass1 > Settings.Epsilon);
			Debug.Assert(this._limitMass2 > Settings.Epsilon);
			Debug.Assert(this._pulleyMass > Settings.Epsilon);
			this._limitMass1 = 1.0f / this._limitMass1;
			this._limitMass2 = 1.0f / this._limitMass2;
			this._pulleyMass = 1.0f / this._pulleyMass;

			if (Settings.EnableWarmstarting)
			{
				// Scale impulses to support variable time steps.
				this._impulse *= step.dtRatio;
				this._limitImpulse1 *= step.dtRatio;
				this._limitImpulse2 *= step.dtRatio;

				// Warm starting.
				Vector2 P1 = -(this._impulse + this._limitImpulse1) * this._u1;
				Vector2 P2 = (-this.Ratio * this._impulse - this._limitImpulse2) * this._u2;
				b1.LinearVelocityInternal += b1.InvMass * P1;
				b1.AngularVelocityInternal += b1.InvI * MathUtils.Cross(r1, P1);
				b2.LinearVelocityInternal += b2.InvMass * P2;
				b2.AngularVelocityInternal += b2.InvI * MathUtils.Cross(r2, P2);
			}
			else
			{
				this._impulse = 0.0f;
				this._limitImpulse1 = 0.0f;
				this._limitImpulse2 = 0.0f;
			}
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

			if (this._state == LimitState.AtUpper)
			{
				Vector2 v1 = b1.LinearVelocityInternal + MathUtils.Cross(b1.AngularVelocityInternal, r1);
				Vector2 v2 = b2.LinearVelocityInternal + MathUtils.Cross(b2.AngularVelocityInternal, r2);

				float Cdot = -Vector2.Dot(this._u1, v1) - this.Ratio * Vector2.Dot(this._u2, v2);
				float impulse = this._pulleyMass * (-Cdot);
				float oldImpulse = this._impulse;
				this._impulse = Math.Max(0.0f, this._impulse + impulse);
				impulse = this._impulse - oldImpulse;

				Vector2 P1 = -impulse * this._u1;
				Vector2 P2 = -this.Ratio * impulse * this._u2;
				b1.LinearVelocityInternal += b1.InvMass * P1;
				b1.AngularVelocityInternal += b1.InvI * MathUtils.Cross(r1, P1);
				b2.LinearVelocityInternal += b2.InvMass * P2;
				b2.AngularVelocityInternal += b2.InvI * MathUtils.Cross(r2, P2);
			}

			if (this._limitState1 == LimitState.AtUpper)
			{
				Vector2 v1 = b1.LinearVelocityInternal + MathUtils.Cross(b1.AngularVelocityInternal, r1);

				float Cdot = -Vector2.Dot(this._u1, v1);
				float impulse = -this._limitMass1 * Cdot;
				float oldImpulse = this._limitImpulse1;
				this._limitImpulse1 = Math.Max(0.0f, this._limitImpulse1 + impulse);
				impulse = this._limitImpulse1 - oldImpulse;

				Vector2 P1 = -impulse * this._u1;
				b1.LinearVelocityInternal += b1.InvMass * P1;
				b1.AngularVelocityInternal += b1.InvI * MathUtils.Cross(r1, P1);
			}

			if (this._limitState2 == LimitState.AtUpper)
			{
				Vector2 v2 = b2.LinearVelocityInternal + MathUtils.Cross(b2.AngularVelocityInternal, r2);

				float Cdot = -Vector2.Dot(this._u2, v2);
				float impulse = -this._limitMass2 * Cdot;
				float oldImpulse = this._limitImpulse2;
				this._limitImpulse2 = Math.Max(0.0f, this._limitImpulse2 + impulse);
				impulse = this._limitImpulse2 - oldImpulse;

				Vector2 P2 = -impulse * this._u2;
				b2.LinearVelocityInternal += b2.InvMass * P2;
				b2.AngularVelocityInternal += b2.InvI * MathUtils.Cross(r2, P2);
			}
		}

		internal override bool SolvePositionConstraints()
		{
			Body b1 = this.BodyA;
			Body b2 = this.BodyB;

			Vector2 s1 = this.GroundAnchorA;
			Vector2 s2 = this.GroundAnchorB;

			float linearError = 0.0f;

			if (this._state == LimitState.AtUpper)
			{
				Transform xf1, xf2;
				b1.GetTransform(out xf1);
				b2.GetTransform(out xf2);

				Vector2 r1 = MathUtils.Multiply(ref xf1.R, this.LocalAnchorA - b1.LocalCenter);
				Vector2 r2 = MathUtils.Multiply(ref xf2.R, this.LocalAnchorB - b2.LocalCenter);

				Vector2 p1 = b1.Sweep.C + r1;
				Vector2 p2 = b2.Sweep.C + r2;

				// Get the pulley axes.
				this._u1 = p1 - s1;
				this._u2 = p2 - s2;

				float length1 = this._u1.Length;
				float length2 = this._u2.Length;

				if (length1 > Settings.LinearSlop)
				{
					this._u1 *= 1.0f / length1;
				}
				else
				{
					this._u1 = Vector2.Zero;
				}

				if (length2 > Settings.LinearSlop)
				{
					this._u2 *= 1.0f / length2;
				}
				else
				{
					this._u2 = Vector2.Zero;
				}

				float C = this._ant - length1 - this.Ratio * length2;
				linearError = Math.Max(linearError, -C);

				C = MathUtils.Clamp(C + Settings.LinearSlop, -Settings.MaxLinearCorrection, 0.0f);
				float impulse = -this._pulleyMass * C;

				Vector2 P1 = -impulse * this._u1;
				Vector2 P2 = -this.Ratio * impulse * this._u2;

				b1.Sweep.C += b1.InvMass * P1;
				b1.Sweep.A += b1.InvI * MathUtils.Cross(r1, P1);
				b2.Sweep.C += b2.InvMass * P2;
				b2.Sweep.A += b2.InvI * MathUtils.Cross(r2, P2);

				b1.SynchronizeTransform();
				b2.SynchronizeTransform();
			}

			if (this._limitState1 == LimitState.AtUpper)
			{
				Transform xf1;
				b1.GetTransform(out xf1);

				Vector2 r1 = MathUtils.Multiply(ref xf1.R, this.LocalAnchorA - b1.LocalCenter);
				Vector2 p1 = b1.Sweep.C + r1;

				this._u1 = p1 - s1;
				float length1 = this._u1.Length;

				if (length1 > Settings.LinearSlop)
				{
					this._u1 *= 1.0f / length1;
				}
				else
				{
					this._u1 = Vector2.Zero;
				}

				float C = this.MaxLengthA - length1;
				linearError = Math.Max(linearError, -C);
				C = MathUtils.Clamp(C + Settings.LinearSlop, -Settings.MaxLinearCorrection, 0.0f);
				float impulse = -this._limitMass1 * C;

				Vector2 P1 = -impulse * this._u1;
				b1.Sweep.C += b1.InvMass * P1;
				b1.Sweep.A += b1.InvI * MathUtils.Cross(r1, P1);

				b1.SynchronizeTransform();
			}

			if (this._limitState2 == LimitState.AtUpper)
			{
				Transform xf2;
				b2.GetTransform(out xf2);

				Vector2 r2 = MathUtils.Multiply(ref xf2.R, this.LocalAnchorB - b2.LocalCenter);
				Vector2 p2 = b2.Sweep.C + r2;

				this._u2 = p2 - s2;
				float length2 = this._u2.Length;

				if (length2 > Settings.LinearSlop)
				{
					this._u2 *= 1.0f / length2;
				}
				else
				{
					this._u2 = Vector2.Zero;
				}

				float C = this.MaxLengthB - length2;
				linearError = Math.Max(linearError, -C);
				C = MathUtils.Clamp(C + Settings.LinearSlop, -Settings.MaxLinearCorrection, 0.0f);
				float impulse = -this._limitMass2 * C;

				Vector2 P2 = -impulse * this._u2;
				b2.Sweep.C += b2.InvMass * P2;
				b2.Sweep.A += b2.InvI * MathUtils.Cross(r2, P2);

				b2.SynchronizeTransform();
			}

			return linearError < Settings.LinearSlop;
		}
	}
}