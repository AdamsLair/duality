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
    public class FixedDistanceJoint : Joint
    {
        /// <summary>
        /// The local anchor point relative to bodyA's origin.
        /// </summary>
        public Vector2 LocalAnchorA;

        private float _bias;
        private float _gamma;
        private float _impulse;
        private float _mass;
        private Vector2 _u;
        private Vector2 _worldAnchorB;

        /// <summary>
        /// This requires defining an
        /// anchor point on both bodies and the non-zero length of the
        /// distance joint. If you don't supply a length, the local anchor points
        /// is used so that the initial configuration can violate the constraint
        /// slightly. This helps when saving and loading a game.
        /// @warning Do not use a zero or short length.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <param name="bodyAnchor">The body anchor.</param>
        /// <param name="worldAnchor">The world anchor.</param>
        public FixedDistanceJoint(Body body, Vector2 bodyAnchor, Vector2 worldAnchor)
            : base(body)
        {
            JointType = JointType.FixedDistance;

            LocalAnchorA = bodyAnchor;
            _worldAnchorB = worldAnchor;

            //Calculate the length
            Vector2 d = WorldAnchorB - WorldAnchorA;
            Length = d.Length;
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
            get { return BodyA.GetWorldPoint(LocalAnchorA); }
        }

        public override sealed Vector2 WorldAnchorB
        {
            get { return _worldAnchorB; }
            set { _worldAnchorB = value; }
        }

        public override Vector2 GetReactionForce(float invDt)
        {
            return (invDt * _impulse) * _u;
        }

        public override float GetReactionTorque(float invDt)
        {
            return 0.0f;
        }

        internal override void InitVelocityConstraints(ref TimeStep step)
        {
            Body b1 = BodyA;

            Transform xf1;
            b1.GetTransform(out xf1);

            // Compute the effective mass matrix.
            Vector2 r1 = MathUtils.Multiply(ref xf1.R, LocalAnchorA - b1.LocalCenter);
            Vector2 r2 = _worldAnchorB;
            _u = r2 - b1.Sweep.C - r1;

            // Handle singularity.
            float length = _u.Length;
            if (length > Settings.LinearSlop)
            {
                _u *= 1.0f / length;
            }
            else
            {
                _u = Vector2.Zero;
            }

            float cr1u = MathUtils.Cross(r1, _u);
            float cr2u = MathUtils.Cross(r2, _u);
            float invMass = b1.InvMass + b1.InvI * cr1u * cr1u + 0 * cr2u * cr2u;
            Debug.Assert(invMass > Settings.Epsilon);
            _mass = invMass != 0.0f ? 1.0f / invMass : 0.0f;

            if (Frequency > 0.0f)
            {
                float C = length - Length;

                // Frequency
                float omega = 2.0f * Settings.Pi * Frequency;

                // Damping coefficient
                float d = 2.0f * _mass * DampingRatio * omega;

                // Spring stiffness
                float k = _mass * omega * omega;

                // magic formulas
                _gamma = step.dt * (d + step.dt * k);
                _gamma = _gamma != 0.0f ? 1.0f / _gamma : 0.0f;
                _bias = C * step.dt * k * _gamma;

                _mass = invMass + _gamma;
                _mass = _mass != 0.0f ? 1.0f / _mass : 0.0f;
            }

            if (Settings.EnableWarmstarting)
            {
                // Scale the impulse to support a variable time step.
                _impulse *= step.dtRatio;

                Vector2 P = _impulse * _u;
                b1.LinearVelocityInternal -= b1.InvMass * P;
                b1.AngularVelocityInternal -= b1.InvI * MathUtils.Cross(r1, P);
            }
            else
            {
                _impulse = 0.0f;
            }
        }

        internal override void SolveVelocityConstraints(ref TimeStep step)
        {
            Body b1 = BodyA;

            Transform xf1;
            b1.GetTransform(out xf1);

            Vector2 r1 = MathUtils.Multiply(ref xf1.R, LocalAnchorA - b1.LocalCenter);

            // Cdot = dot(u, v + cross(w, r))
            Vector2 v1 = b1.LinearVelocityInternal + MathUtils.Cross(b1.AngularVelocityInternal, r1);
            Vector2 v2 = Vector2.Zero;
            float Cdot = Vector2.Dot(_u, v2 - v1);

            float impulse = -_mass * (Cdot + _bias + _gamma * _impulse);
            _impulse += impulse;

            Vector2 P = impulse * _u;
            b1.LinearVelocityInternal -= b1.InvMass * P;
            b1.AngularVelocityInternal -= b1.InvI * MathUtils.Cross(r1, P);
        }

        internal override bool SolvePositionConstraints()
        {
            if (Frequency > 0.0f)
            {
                // There is no position correction for soft distance constraints.
                return true;
            }

            Body b1 = BodyA;

            Transform xf1;
            b1.GetTransform(out xf1);

            Vector2 r1 = MathUtils.Multiply(ref xf1.R, LocalAnchorA - b1.LocalCenter);
            Vector2 r2 = _worldAnchorB;

            Vector2 d = r2 - b1.Sweep.C - r1;

            float length = d.Length;

            if (length == 0.0f)
                return true;

            d /= length;
            float C = length - Length;
            C = MathUtils.Clamp(C, -Settings.MaxLinearCorrection, Settings.MaxLinearCorrection);

            float impulse = -_mass * C;
            _u = d;
            Vector2 P = impulse * _u;

            b1.Sweep.C -= b1.InvMass * P;
            b1.Sweep.A -= b1.InvI * MathUtils.Cross(r1, P);

            b1.SynchronizeTransform();

            return Math.Abs(C) < Settings.LinearSlop;
        }
    }
}