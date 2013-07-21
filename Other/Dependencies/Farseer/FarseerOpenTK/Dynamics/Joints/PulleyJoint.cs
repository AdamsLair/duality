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
using OpenTK;

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
            JointType = JointType.Pulley;
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
            JointType = JointType.Pulley;

            GroundAnchorA = groundAnchorA;
            GroundAnchorB = groundAnchorB;
            LocalAnchorA = localAnchorA;
            LocalAnchorB = localAnchorB;

            Vector2 d1 = BodyA.GetWorldPoint(localAnchorA) - groundAnchorA;
            _lengthA = d1.Length;

            Vector2 d2 = BodyB.GetWorldPoint(localAnchorB) - groundAnchorB;
            _lengthB = d2.Length;

            Debug.Assert(ratio != 0.0f);
            Debug.Assert(ratio > Settings.Epsilon);
            Ratio = ratio;

            float C = _lengthA + Ratio * _lengthB;

            MaxLengthA = C - Ratio * MinPulleyLength;
            MaxLengthB = (C - MinPulleyLength) / Ratio;

            _ant = _lengthA + Ratio * _lengthB;

            MaxLengthA = Math.Min(MaxLengthA, _ant - Ratio * MinPulleyLength);
            MaxLengthB = Math.Min(MaxLengthB, (_ant - MinPulleyLength) / Ratio);

            _impulse = 0.0f;
            _limitImpulse1 = 0.0f;
            _limitImpulse2 = 0.0f;
        }

        public override Vector2 WorldAnchorA
        {
            get { return BodyA.GetWorldPoint(LocalAnchorA); }
        }

        public override Vector2 WorldAnchorB
        {
            get { return BodyB.GetWorldPoint(LocalAnchorB); }
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
                Vector2 d = BodyA.GetWorldPoint(LocalAnchorA) - GroundAnchorA;
                return d.Length;
            }
            set { _lengthA = value; }
        }

        /// <summary>
        /// Get the current length of the segment attached to body2.
        /// </summary>
        /// <value></value>
        public float LengthB
        {
            get
            {
                Vector2 d = BodyB.GetWorldPoint(LocalAnchorB) - GroundAnchorB;
                return d.Length;
            }
            set { _lengthB = value; }
        }

        /// <summary>
        /// Get the pulley ratio.
        /// </summary>
        /// <value></value>
        public float Ratio { get; set; }
		
        public float TotalLength
        {
            get { return _ant; }
            set { _ant = value; }
        }
        public float MaxLengthA
        {
            get { return _maxLengthA; }
            set { _maxLengthA = value; }
        }

        public float MaxLengthB
        {
            get { return _maxLengthB; }
            set { _maxLengthB = value; }
        }

        public override Vector2 GetReactionForce(float inv_dt)
        {
            Vector2 P = _impulse * _u2;
            return inv_dt * P;
        }

        public override float GetReactionTorque(float inv_dt)
        {
            return 0.0f;
        }

        internal override void InitVelocityConstraints(ref TimeStep step)
        {
            Body b1 = BodyA;
            Body b2 = BodyB;

            Transform xf1, xf2;
            b1.GetTransform(out xf1);
            b2.GetTransform(out xf2);

            Vector2 r1 = MathUtils.Multiply(ref xf1.R, LocalAnchorA - b1.LocalCenter);
            Vector2 r2 = MathUtils.Multiply(ref xf2.R, LocalAnchorB - b2.LocalCenter);

            Vector2 p1 = b1.Sweep.C + r1;
            Vector2 p2 = b2.Sweep.C + r2;

            Vector2 s1 = GroundAnchorA;
            Vector2 s2 = GroundAnchorB;

            // Get the pulley axes.
            _u1 = p1 - s1;
            _u2 = p2 - s2;

            float length1 = _u1.Length;
            float length2 = _u2.Length;

            if (length1 > Settings.LinearSlop)
            {
                _u1 *= 1.0f / length1;
            }
            else
            {
                _u1 = Vector2.Zero;
            }

            if (length2 > Settings.LinearSlop)
            {
                _u2 *= 1.0f / length2;
            }
            else
            {
                _u2 = Vector2.Zero;
            }

            float C = _ant - length1 - Ratio * length2;
            if (C > 0.0f)
            {
                _state = LimitState.Inactive;
                _impulse = 0.0f;
            }
            else
            {
                _state = LimitState.AtUpper;
            }

            if (length1 < MaxLengthA)
            {
                _limitState1 = LimitState.Inactive;
                _limitImpulse1 = 0.0f;
            }
            else
            {
                _limitState1 = LimitState.AtUpper;
            }

            if (length2 < MaxLengthB)
            {
                _limitState2 = LimitState.Inactive;
                _limitImpulse2 = 0.0f;
            }
            else
            {
                _limitState2 = LimitState.AtUpper;
            }

            // Compute effective mass.
            float cr1u1 = MathUtils.Cross(r1, _u1);
            float cr2u2 = MathUtils.Cross(r2, _u2);

            _limitMass1 = b1.InvMass + b1.InvI * cr1u1 * cr1u1;
            _limitMass2 = b2.InvMass + b2.InvI * cr2u2 * cr2u2;
            _pulleyMass = _limitMass1 + Ratio * Ratio * _limitMass2;
            Debug.Assert(_limitMass1 > Settings.Epsilon);
            Debug.Assert(_limitMass2 > Settings.Epsilon);
            Debug.Assert(_pulleyMass > Settings.Epsilon);
            _limitMass1 = 1.0f / _limitMass1;
            _limitMass2 = 1.0f / _limitMass2;
            _pulleyMass = 1.0f / _pulleyMass;

            if (Settings.EnableWarmstarting)
            {
                // Scale impulses to support variable time steps.
                _impulse *= step.dtRatio;
                _limitImpulse1 *= step.dtRatio;
                _limitImpulse2 *= step.dtRatio;

                // Warm starting.
                Vector2 P1 = -(_impulse + _limitImpulse1) * _u1;
                Vector2 P2 = (-Ratio * _impulse - _limitImpulse2) * _u2;
                b1.LinearVelocityInternal += b1.InvMass * P1;
                b1.AngularVelocityInternal += b1.InvI * MathUtils.Cross(r1, P1);
                b2.LinearVelocityInternal += b2.InvMass * P2;
                b2.AngularVelocityInternal += b2.InvI * MathUtils.Cross(r2, P2);
            }
            else
            {
                _impulse = 0.0f;
                _limitImpulse1 = 0.0f;
                _limitImpulse2 = 0.0f;
            }
        }

        internal override void SolveVelocityConstraints(ref TimeStep step)
        {
            Body b1 = BodyA;
            Body b2 = BodyB;

            Transform xf1, xf2;
            b1.GetTransform(out xf1);
            b2.GetTransform(out xf2);

            Vector2 r1 = MathUtils.Multiply(ref xf1.R, LocalAnchorA - b1.LocalCenter);
            Vector2 r2 = MathUtils.Multiply(ref xf2.R, LocalAnchorB - b2.LocalCenter);

            if (_state == LimitState.AtUpper)
            {
                Vector2 v1 = b1.LinearVelocityInternal + MathUtils.Cross(b1.AngularVelocityInternal, r1);
                Vector2 v2 = b2.LinearVelocityInternal + MathUtils.Cross(b2.AngularVelocityInternal, r2);

                float Cdot = -Vector2.Dot(_u1, v1) - Ratio * Vector2.Dot(_u2, v2);
                float impulse = _pulleyMass * (-Cdot);
                float oldImpulse = _impulse;
                _impulse = Math.Max(0.0f, _impulse + impulse);
                impulse = _impulse - oldImpulse;

                Vector2 P1 = -impulse * _u1;
                Vector2 P2 = -Ratio * impulse * _u2;
                b1.LinearVelocityInternal += b1.InvMass * P1;
                b1.AngularVelocityInternal += b1.InvI * MathUtils.Cross(r1, P1);
                b2.LinearVelocityInternal += b2.InvMass * P2;
                b2.AngularVelocityInternal += b2.InvI * MathUtils.Cross(r2, P2);
            }

            if (_limitState1 == LimitState.AtUpper)
            {
                Vector2 v1 = b1.LinearVelocityInternal + MathUtils.Cross(b1.AngularVelocityInternal, r1);

                float Cdot = -Vector2.Dot(_u1, v1);
                float impulse = -_limitMass1 * Cdot;
                float oldImpulse = _limitImpulse1;
                _limitImpulse1 = Math.Max(0.0f, _limitImpulse1 + impulse);
                impulse = _limitImpulse1 - oldImpulse;

                Vector2 P1 = -impulse * _u1;
                b1.LinearVelocityInternal += b1.InvMass * P1;
                b1.AngularVelocityInternal += b1.InvI * MathUtils.Cross(r1, P1);
            }

            if (_limitState2 == LimitState.AtUpper)
            {
                Vector2 v2 = b2.LinearVelocityInternal + MathUtils.Cross(b2.AngularVelocityInternal, r2);

                float Cdot = -Vector2.Dot(_u2, v2);
                float impulse = -_limitMass2 * Cdot;
                float oldImpulse = _limitImpulse2;
                _limitImpulse2 = Math.Max(0.0f, _limitImpulse2 + impulse);
                impulse = _limitImpulse2 - oldImpulse;

                Vector2 P2 = -impulse * _u2;
                b2.LinearVelocityInternal += b2.InvMass * P2;
                b2.AngularVelocityInternal += b2.InvI * MathUtils.Cross(r2, P2);
            }
        }

        internal override bool SolvePositionConstraints()
        {
            Body b1 = BodyA;
            Body b2 = BodyB;

            Vector2 s1 = GroundAnchorA;
            Vector2 s2 = GroundAnchorB;

            float linearError = 0.0f;

            if (_state == LimitState.AtUpper)
            {
                Transform xf1, xf2;
                b1.GetTransform(out xf1);
                b2.GetTransform(out xf2);

                Vector2 r1 = MathUtils.Multiply(ref xf1.R, LocalAnchorA - b1.LocalCenter);
                Vector2 r2 = MathUtils.Multiply(ref xf2.R, LocalAnchorB - b2.LocalCenter);

                Vector2 p1 = b1.Sweep.C + r1;
                Vector2 p2 = b2.Sweep.C + r2;

                // Get the pulley axes.
                _u1 = p1 - s1;
                _u2 = p2 - s2;

                float length1 = _u1.Length;
                float length2 = _u2.Length;

                if (length1 > Settings.LinearSlop)
                {
                    _u1 *= 1.0f / length1;
                }
                else
                {
                    _u1 = Vector2.Zero;
                }

                if (length2 > Settings.LinearSlop)
                {
                    _u2 *= 1.0f / length2;
                }
                else
                {
                    _u2 = Vector2.Zero;
                }

                float C = _ant - length1 - Ratio * length2;
                linearError = Math.Max(linearError, -C);

                C = MathUtils.Clamp(C + Settings.LinearSlop, -Settings.MaxLinearCorrection, 0.0f);
                float impulse = -_pulleyMass * C;

                Vector2 P1 = -impulse * _u1;
                Vector2 P2 = -Ratio * impulse * _u2;

                b1.Sweep.C += b1.InvMass * P1;
                b1.Sweep.A += b1.InvI * MathUtils.Cross(r1, P1);
                b2.Sweep.C += b2.InvMass * P2;
                b2.Sweep.A += b2.InvI * MathUtils.Cross(r2, P2);

                b1.SynchronizeTransform();
                b2.SynchronizeTransform();
            }

            if (_limitState1 == LimitState.AtUpper)
            {
                Transform xf1;
                b1.GetTransform(out xf1);

                Vector2 r1 = MathUtils.Multiply(ref xf1.R, LocalAnchorA - b1.LocalCenter);
                Vector2 p1 = b1.Sweep.C + r1;

                _u1 = p1 - s1;
                float length1 = _u1.Length;

                if (length1 > Settings.LinearSlop)
                {
                    _u1 *= 1.0f / length1;
                }
                else
                {
                    _u1 = Vector2.Zero;
                }

                float C = MaxLengthA - length1;
                linearError = Math.Max(linearError, -C);
                C = MathUtils.Clamp(C + Settings.LinearSlop, -Settings.MaxLinearCorrection, 0.0f);
                float impulse = -_limitMass1 * C;

                Vector2 P1 = -impulse * _u1;
                b1.Sweep.C += b1.InvMass * P1;
                b1.Sweep.A += b1.InvI * MathUtils.Cross(r1, P1);

                b1.SynchronizeTransform();
            }

            if (_limitState2 == LimitState.AtUpper)
            {
                Transform xf2;
                b2.GetTransform(out xf2);

                Vector2 r2 = MathUtils.Multiply(ref xf2.R, LocalAnchorB - b2.LocalCenter);
                Vector2 p2 = b2.Sweep.C + r2;

                _u2 = p2 - s2;
                float length2 = _u2.Length;

                if (length2 > Settings.LinearSlop)
                {
                    _u2 *= 1.0f / length2;
                }
                else
                {
                    _u2 = Vector2.Zero;
                }

                float C = MaxLengthB - length2;
                linearError = Math.Max(linearError, -C);
                C = MathUtils.Clamp(C + Settings.LinearSlop, -Settings.MaxLinearCorrection, 0.0f);
                float impulse = -_limitMass2 * C;

                Vector2 P2 = -impulse * _u2;
                b2.Sweep.C += b2.InvMass * P2;
                b2.Sweep.A += b2.InvI * MathUtils.Cross(r2, P2);

                b2.SynchronizeTransform();
            }

            return linearError < Settings.LinearSlop;
        }
    }
}