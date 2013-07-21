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
    public class FixedFrictionJoint : Joint
    {
        public Vector2 LocalAnchorA;

        /// <summary>
        /// The maximum friction force in N.
        /// </summary>
        public float MaxForce;

        /// <summary>
        /// The maximum friction torque in N-m.
        /// </summary>
        public float MaxTorque;

        private float _angularImpulse;
        private float _angularMass;
        private Vector2 _linearImpulse;
        private Mat22 _linearMass;

        public FixedFrictionJoint(Body body, Vector2 localAnchorA)
            : base(body)
        {
            JointType = JointType.FixedFriction;
            LocalAnchorA = localAnchorA;

            //Setting default max force and max torque
            const float gravity = 10.0f;

            // For a circle: I = 0.5 * m * r * r ==> r = sqrt(2 * I / m)
            float radius = (float)Math.Sqrt(2.0 * (body.Inertia / body.Mass));

            MaxForce = body.Mass * gravity;
            MaxTorque = body.Mass * radius * gravity;
        }

        public override Vector2 WorldAnchorA
        {
            get { return BodyA.GetWorldPoint(LocalAnchorA); }
        }

        public override Vector2 WorldAnchorB
        {
            get { return Vector2.Zero; }
            set { Debug.Assert(false, "You can't set the world anchor on this joint type."); }
        }

        public override Vector2 GetReactionForce(float invDT)
        {
            return invDT * _linearImpulse;
        }

        public override float GetReactionTorque(float invDT)
        {
            return invDT * _angularImpulse;
        }

        internal override void InitVelocityConstraints(ref TimeStep step)
        {
            Body bA = BodyA;

            Transform xfA;
            bA.GetTransform(out xfA);

            // Compute the effective mass matrix.
            Vector2 rA = MathUtils.Multiply(ref xfA.R, LocalAnchorA - bA.LocalCenter);

            // J = [-I -r1_skew I r2_skew]
            //     [ 0       -1 0       1]
            // r_skew = [-ry; rx]

            // Matlab
            // K = [ mA+r1y^2*iA+mB+r2y^2*iB,  -r1y*iA*r1x-r2y*iB*r2x,          -r1y*iA-r2y*iB]
            //     [  -r1y*iA*r1x-r2y*iB*r2x, mA+r1x^2*iA+mB+r2x^2*iB,           r1x*iA+r2x*iB]
            //     [          -r1y*iA-r2y*iB,           r1x*iA+r2x*iB,                   iA+iB]

            float mA = bA.InvMass;
            float iA = bA.InvI;

            Mat22 K1 = new Mat22();
            K1.Col1.X = mA;
            K1.Col2.X = 0.0f;
            K1.Col1.Y = 0.0f;
            K1.Col2.Y = mA;

            Mat22 K2 = new Mat22();
            K2.Col1.X = iA * rA.Y * rA.Y;
            K2.Col2.X = -iA * rA.X * rA.Y;
            K2.Col1.Y = -iA * rA.X * rA.Y;
            K2.Col2.Y = iA * rA.X * rA.X;

            Mat22 K12;
            Mat22.Add(ref K1, ref K2, out K12);

            _linearMass = K12.Inverse;

            _angularMass = iA;
            if (_angularMass > 0.0f)
            {
                _angularMass = 1.0f / _angularMass;
            }

            if (Settings.EnableWarmstarting)
            {
                // Scale impulses to support a variable time step.
                _linearImpulse *= step.dtRatio;
                _angularImpulse *= step.dtRatio;

                Vector2 P = new Vector2(_linearImpulse.X, _linearImpulse.Y);

                bA.LinearVelocityInternal -= mA * P;
                bA.AngularVelocityInternal -= iA * (MathUtils.Cross(rA, P) + _angularImpulse);
            }
            else
            {
                _linearImpulse = Vector2.Zero;
                _angularImpulse = 0.0f;
            }
        }

        internal override void SolveVelocityConstraints(ref TimeStep step)
        {
            Body bA = BodyA;

            Vector2 vA = bA.LinearVelocityInternal;
            float wA = bA.AngularVelocityInternal;

            float mA = bA.InvMass;
            float iA = bA.InvI;

            Transform xfA;
            bA.GetTransform(out xfA);

            Vector2 rA = MathUtils.Multiply(ref xfA.R, LocalAnchorA - bA.LocalCenter);

            // Solve angular friction
            {
                float Cdot = -wA;
                float impulse = -_angularMass * Cdot;

                float oldImpulse = _angularImpulse;
                float maxImpulse = step.dt * MaxTorque;
                _angularImpulse = MathUtils.Clamp(_angularImpulse + impulse, -maxImpulse, maxImpulse);
                impulse = _angularImpulse - oldImpulse;

                wA -= iA * impulse;
            }

            // Solve linear friction
            {
                Vector2 Cdot = -vA - MathUtils.Cross(wA, rA);

                Vector2 impulse = -MathUtils.Multiply(ref _linearMass, Cdot);
                Vector2 oldImpulse = _linearImpulse;
                _linearImpulse += impulse;

                float maxImpulse = step.dt * MaxForce;

                if (_linearImpulse.LengthSquared > maxImpulse * maxImpulse)
                {
                    _linearImpulse.Normalize();
                    _linearImpulse *= maxImpulse;
                }

                impulse = _linearImpulse - oldImpulse;

                vA -= mA * impulse;
                wA -= iA * MathUtils.Cross(rA, impulse);
            }

            bA.LinearVelocityInternal = vA;
            bA.AngularVelocityInternal = wA;
        }

        internal override bool SolvePositionConstraints()
        {
            return true;
        }
    }
}