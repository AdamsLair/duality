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
    /// A distance joint contrains two points on two bodies
    /// to remain at a fixed distance from each other. You can view
    /// this as a massless, rigid rod.
    /// </summary>
    public class SliderJoint : Joint
    {
        // 1-D constrained system
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

        public Vector2 LocalAnchorA;

        public Vector2 LocalAnchorB;
        private float _bias;
        private float _gamma;
        private float _impulse;
        private float _mass;
        private Vector2 _u;

        internal SliderJoint()
        {
            JointType = JointType.Slider;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SliderJoint"/> class.
        /// Warning: Do not use a zero or short length.
        /// </summary>
        /// <param name="bodyA">The first body.</param>
        /// <param name="bodyB">The second body.</param>
        /// <param name="localAnchorA">The first body anchor.</param>
        /// <param name="localAnchorB">The second body anchor.</param>
        /// <param name="minLength">The minimum length between anchorpoints</param>
        /// <param name="maxlength">The maximum length between anchorpoints.</param>
        public SliderJoint(Body bodyA, Body bodyB, Vector2 localAnchorA, Vector2 localAnchorB, float minLength,
                           float maxlength)
            : base(bodyA, bodyB)
        {
            JointType = JointType.Slider;

            LocalAnchorA = localAnchorA;
            LocalAnchorB = localAnchorB;
            MaxLength = maxlength;
            MinLength = minLength;
        }

        /// <summary>
        /// The maximum length between the anchor points.
        /// </summary>
        /// <value>The length.</value>
        public float MaxLength { get; set; }

        /// <summary>
        /// The minimal length between the anchor points.
        /// </summary>
        /// <value>The length.</value>
        public float MinLength { get; set; }

        /// <summary>
        /// The mass-spring-damper frequency in Hertz.
        /// </summary>
        /// <value>The frequency.</value>
        public float Frequency { get; set; }

        /// <summary>
        /// The damping ratio. 0 = no damping, 1 = critical damping.
        /// </summary>
        /// <value>The damping ratio.</value>
        public float DampingRatio { get; set; }

        public override Vector2 WorldAnchorA
        {
            get { return BodyA.GetWorldPoint(LocalAnchorA); }
        }

        public override Vector2 WorldAnchorB
        {
            get { return BodyB.GetWorldPoint(LocalAnchorB); }
            set { Debug.Assert(false, "You can't set the world anchor on this joint type."); }
        }

        public override Vector2 GetReactionForce(float inv_dt)
        {
            Vector2 F = (inv_dt * _impulse) * _u;
            return F;
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

            // Compute the effective mass matrix.
            Vector2 r1 = MathUtils.Multiply(ref xf1.R, LocalAnchorA - b1.LocalCenter);
            Vector2 r2 = MathUtils.Multiply(ref xf2.R, LocalAnchorB - b2.LocalCenter);
            _u = b2.Sweep.C + r2 - b1.Sweep.C - r1;

            // Handle singularity.
            float length = _u.Length;

            if (length < MaxLength && length > MinLength)
            {
                return;
            }

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
            float invMass = b1.InvMass + b1.InvI * cr1u * cr1u + b2.InvMass + b2.InvI * cr2u * cr2u;
            Debug.Assert(invMass > Settings.Epsilon);
            _mass = invMass != 0.0f ? 1.0f / invMass : 0.0f;

            if (Frequency > 0.0f)
            {
                float C = length - MaxLength;

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
                b2.LinearVelocityInternal += b2.InvMass * P;
                b2.AngularVelocityInternal += b2.InvI * MathUtils.Cross(r2, P);
            }
            else
            {
                _impulse = 0.0f;
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

            Vector2 d = b2.Sweep.C + r2 - b1.Sweep.C - r1;

            float length = d.Length;

            if (length < MaxLength && length > MinLength)
            {
                return;
            }

            // Cdot = dot(u, v + cross(w, r))
            Vector2 v1 = b1.LinearVelocityInternal + MathUtils.Cross(b1.AngularVelocityInternal, r1);
            Vector2 v2 = b2.LinearVelocityInternal + MathUtils.Cross(b2.AngularVelocityInternal, r2);
            float Cdot = Vector2.Dot(_u, v2 - v1);

            float impulse = -_mass * (Cdot + _bias + _gamma * _impulse);
            _impulse += impulse;

            Vector2 P = impulse * _u;
            b1.LinearVelocityInternal -= b1.InvMass * P;
            b1.AngularVelocityInternal -= b1.InvI * MathUtils.Cross(r1, P);
            b2.LinearVelocityInternal += b2.InvMass * P;
            b2.AngularVelocityInternal += b2.InvI * MathUtils.Cross(r2, P);
        }

        internal override bool SolvePositionConstraints()
        {
            if (Frequency > 0.0f)
            {
                // There is no position correction for soft distance constraints.
                return true;
            }

            Body b1 = BodyA;
            Body b2 = BodyB;

            Transform xf1, xf2;
            b1.GetTransform(out xf1);
            b2.GetTransform(out xf2);

            Vector2 r1 = MathUtils.Multiply(ref xf1.R, LocalAnchorA - b1.LocalCenter);
            Vector2 r2 = MathUtils.Multiply(ref xf2.R, LocalAnchorB - b2.LocalCenter);

            Vector2 d = b2.Sweep.C + r2 - b1.Sweep.C - r1;

            float length = d.Length;

            if (length < MaxLength && length > MinLength)
            {
                return true;
            }

            if (length == 0.0f)
                return true;

            d /= length;
            float C = length - MaxLength;
            C = MathUtils.Clamp(C, -Settings.MaxLinearCorrection, Settings.MaxLinearCorrection);

            float impulse = -_mass * C;
            _u = d;
            Vector2 P = impulse * _u;

            b1.Sweep.C -= b1.InvMass * P;
            b1.Sweep.A -= b1.InvI * MathUtils.Cross(r1, P);
            b2.Sweep.C += b2.InvMass * P;
            b2.Sweep.A += b2.InvI * MathUtils.Cross(r2, P);

            b1.SynchronizeTransform();
            b2.SynchronizeTransform();

            return Math.Abs(C) < Settings.LinearSlop;
        }
    }
}