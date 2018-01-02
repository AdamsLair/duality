/*
* Copyright (c) 2006-2010 Erin Catto http://www.gphysics.com
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
    // Limit:
    // C = norm(pB - pA) - L
    // u = (pB - pA) / norm(pB - pA)
    // Cdot = dot(u, vB + cross(wB, rB) - vA - cross(wA, rA))
    // J = [-u -cross(rA, u) u cross(rB, u)]
    // K = J * invM * JT
    //   = invMassA + invIA * cross(rA, u)^2 + invMassB + invIB * cross(rB, u)^2

    /// <summary>
    /// A rope joint enforces a maximum distance between two points
    /// on two bodies. It has no other effect.
    /// Warning: if you attempt to change the maximum length during
    /// the simulation you will get some non-physical behavior.
    /// A model that would allow you to dynamically modify the length
    /// would have some sponginess, so I chose not to implement it
    /// that way. See b2DistanceJoint if you want to dynamically
    /// control length.
    /// </summary>
    public class RopeJoint : Joint
    {
        public Vector2 LocalAnchorA;
        public Vector2 LocalAnchorB;

        private float _impulse;
        private float _length;

        private float _mass;
        private Vector2 _rA, _rB;
        private LimitState _state;
        private Vector2 _u;

        internal RopeJoint()
        {
            JointType = JointType.Rope;
        }

        public RopeJoint(Body bodyA, Body bodyB, Vector2 localAnchorA, Vector2 localAnchorB)
            : base(bodyA, bodyB)
        {
            JointType = JointType.Rope;
            LocalAnchorA = localAnchorA;
            LocalAnchorB = localAnchorB;

            Vector2 d = WorldAnchorB - WorldAnchorA;
            MaxLength = d.Length;

            _mass = 0.0f;
            _impulse = 0.0f;
            _state = LimitState.Inactive;
            _length = 0.0f;
        }

        /// Get the maximum length of the rope.
        public float MaxLength { get; set; }

        public LimitState State
        {
            get { return _state; }
        }

        public override sealed Vector2 WorldAnchorA
        {
            get { return BodyA.GetWorldPoint(LocalAnchorA); }
        }

        public override sealed Vector2 WorldAnchorB
        {
            get { return BodyB.GetWorldPoint(LocalAnchorB); }
            set { Debug.Assert(false, "You can't set the world anchor on this joint type."); }
        }

        public override Vector2 GetReactionForce(float invDt)
        {
            return (invDt * _impulse) * _u;
        }

        public override float GetReactionTorque(float invDt)
        {
            return 0;
        }

        internal override void InitVelocityConstraints(ref TimeStep step)
        {
            Body bA = BodyA;
            Body bB = BodyB;

            Transform xf1;
            bA.GetTransform(out xf1);

            Transform xf2;
            bB.GetTransform(out xf2);

            _rA = MathUtils.Multiply(ref xf1.R, LocalAnchorA - bA.LocalCenter);
            _rB = MathUtils.Multiply(ref xf2.R, LocalAnchorB - bB.LocalCenter);

            // Rope axis
            _u = bB.Sweep.C + _rB - bA.Sweep.C - _rA;

            _length = _u.Length;

            float C = _length - MaxLength;
            if (C > 0.0f)
            {
                _state = LimitState.AtUpper;
            }
            else
            {
                _state = LimitState.Inactive;
            }

            if (_length > Settings.LinearSlop)
            {
                _u *= 1.0f / _length;
            }
            else
            {
                _u = Vector2.Zero;
                _mass = 0.0f;
                _impulse = 0.0f;
                return;
            }

            // Compute effective mass.
            float crA = MathUtils.Cross(_rA, _u);
            float crB = MathUtils.Cross(_rB, _u);
            float invMass = bA.InvMass + bA.InvI * crA * crA + bB.InvMass + bB.InvI * crB * crB;

            _mass = invMass != 0.0f ? 1.0f / invMass : 0.0f;

            if (Settings.EnableWarmstarting)
            {
                // Scale the impulse to support a variable time step.
                _impulse *= step.dtRatio;

                Vector2 P = _impulse * _u;
                bA.LinearVelocity -= bA.InvMass * P;
                bA.AngularVelocity -= bA.InvI * MathUtils.Cross(_rA, P);
                bB.LinearVelocity += bB.InvMass * P;
                bB.AngularVelocity += bB.InvI * MathUtils.Cross(_rB, P);
            }
            else
            {
                _impulse = 0.0f;
            }
        }

        internal override void SolveVelocityConstraints(ref TimeStep step)
        {
            Body bA = BodyA;
            Body bB = BodyB;

            // Cdot = dot(u, v + cross(w, r))
            Vector2 vA = bA.LinearVelocity + MathUtils.Cross(bA.AngularVelocity, _rA);
            Vector2 vB = bB.LinearVelocity + MathUtils.Cross(bB.AngularVelocity, _rB);
            float C = _length - MaxLength;
            float Cdot = Vector2.Dot(_u, vB - vA);

            // Predictive constraint.
            if (C < 0.0f)
            {
                Cdot += step.inv_dt * C;
            }

            float impulse = -_mass * Cdot;
            float oldImpulse = _impulse;
            _impulse = Math.Min(0.0f, _impulse + impulse);
            impulse = _impulse - oldImpulse;

            Vector2 P = impulse * _u;
            bA.LinearVelocity -= bA.InvMass * P;
            bA.AngularVelocity -= bA.InvI * MathUtils.Cross(_rA, P);
            bB.LinearVelocity += bB.InvMass * P;
            bB.AngularVelocity += bB.InvI * MathUtils.Cross(_rB, P);
        }

        internal override bool SolvePositionConstraints()
        {
            Body bA = BodyA;
            Body bB = BodyB;

            Transform xf1;
            bA.GetTransform(out xf1);

            Transform xf2;
            bB.GetTransform(out xf2);

            Vector2 rA = MathUtils.Multiply(ref xf1.R, LocalAnchorA - bA.LocalCenter);
            Vector2 rB = MathUtils.Multiply(ref xf2.R, LocalAnchorB - bB.LocalCenter);

            Vector2 u = bB.Sweep.C + rB - bA.Sweep.C - rA;


            float length = u.Length;
            u.Normalize();

            float C = length - MaxLength;

            C = MathUtils.Clamp(C, 0.0f, Settings.MaxLinearCorrection);

            float impulse = -_mass * C;
            Vector2 P = impulse * u;

            bA.Sweep.C -= bA.InvMass * P;
            bA.Sweep.A -= bA.InvI * MathUtils.Cross(rA, P);
            bB.Sweep.C += bB.InvMass * P;
            bB.Sweep.A += bB.InvI * MathUtils.Cross(rB, P);

            bA.SynchronizeTransform();
            bB.SynchronizeTransform();

            return length - MaxLength < Settings.LinearSlop;
        }
    }
}