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
using OpenTK;

namespace FarseerPhysics.Dynamics.Joints
{
    /// <summary>
    /// A mouse joint is used to make a point on a body track a
    /// specified world point. This a soft constraint with a maximum
    /// force. This allows the constraint to stretch and without
    /// applying huge forces.
    /// NOTE: this joint is not documented in the manual because it was
    /// developed to be used in the testbed. If you want to learn how to
    /// use the mouse joint, look at the testbed.
    /// </summary>
    public class FixedMouseJoint : Joint
    {
        public Vector2 LocalAnchorA;
        private Vector2 _C; // position error
        private float _beta;
        private float _gamma;
        private Vector2 _impulse;
        private Mat22 _mass; // effective mass for point-to-point constraint.

        private Vector2 _worldAnchor;

        /// <summary>
        /// This requires a world target point,
        /// tuning parameters, and the time step.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <param name="worldAnchor">The target.</param>
        public FixedMouseJoint(Body body, Vector2 worldAnchor)
            : base(body)
        {
            JointType = JointType.FixedMouse;
            Frequency = 5.0f;
            DampingRatio = 0.7f;

            Debug.Assert(worldAnchor.IsValid());

            Transform xf1;
            BodyA.GetTransform(out xf1);

            _worldAnchor = worldAnchor;
            LocalAnchorA = BodyA.GetLocalPoint(worldAnchor);
        }

        public override Vector2 WorldAnchorA
        {
            get { return BodyA.GetWorldPoint(LocalAnchorA); }
        }

        public override Vector2 WorldAnchorB
        {
            get { return _worldAnchor; }
            set
            {
                BodyA.Awake = true;
                _worldAnchor = value;
            }
        }

        /// <summary>
        /// The maximum constraint force that can be exerted
        /// to move the candidate body. Usually you will express
        /// as some multiple of the weight (multiplier * mass * gravity).
        /// </summary>
        public float MaxForce { get; set; }

        /// <summary>
        /// The response speed.
        /// </summary>
        public float Frequency { get; set; }

        /// <summary>
        /// The damping ratio. 0 = no damping, 1 = critical damping.
        /// </summary>
        public float DampingRatio { get; set; }

        public override Vector2 GetReactionForce(float inv_dt)
        {
            return inv_dt * _impulse;
        }

        public override float GetReactionTorque(float inv_dt)
        {
            return inv_dt * 0.0f;
        }

        internal override void InitVelocityConstraints(ref TimeStep step)
        {
            Body b = BodyA;

            float mass = b.Mass;

            // Frequency
            float omega = 2.0f * Settings.Pi * Frequency;

            // Damping coefficient
            float d = 2.0f * mass * DampingRatio * omega;

            // Spring stiffness
            float k = mass * (omega * omega);

            // magic formulas
            // gamma has units of inverse mass.
            // beta has units of inverse time.
            Debug.Assert(d + step.dt * k > Settings.Epsilon);

            _gamma = step.dt * (d + step.dt * k);
            if (_gamma != 0.0f)
            {
                _gamma = 1.0f / _gamma;
            }

            _beta = step.dt * k * _gamma;

            // Compute the effective mass matrix.
            Transform xf1;
            b.GetTransform(out xf1);
            Vector2 r = MathUtils.Multiply(ref xf1.R, LocalAnchorA - b.LocalCenter);

            // K    = [(1/m1 + 1/m2) * eye(2) - skew(r1) * invI1 * skew(r1) - skew(r2) * invI2 * skew(r2)]
            //      = [1/m1+1/m2     0    ] + invI1 * [r1.Y*r1.Y -r1.X*r1.Y] + invI2 * [r1.Y*r1.Y -r1.X*r1.Y]
            //        [    0     1/m1+1/m2]           [-r1.X*r1.Y r1.X*r1.X]           [-r1.X*r1.Y r1.X*r1.X]
            float invMass = b.InvMass;
            float invI = b.InvI;

            Mat22 K1 = new Mat22(new Vector2(invMass, 0.0f), new Vector2(0.0f, invMass));
            Mat22 K2 = new Mat22(new Vector2(invI * r.Y * r.Y, -invI * r.X * r.Y),
                                 new Vector2(-invI * r.X * r.Y, invI * r.X * r.X));

            Mat22 K;
            Mat22.Add(ref K1, ref K2, out K);

            K.Col1.X += _gamma;
            K.Col2.Y += _gamma;

            _mass = K.Inverse;

            _C = b.Sweep.C + r - _worldAnchor;

            // Cheat with some damping
            b.AngularVelocityInternal *= 0.98f;

            // Warm starting.
            _impulse *= step.dtRatio;
            b.LinearVelocityInternal += invMass * _impulse;
            b.AngularVelocityInternal += invI * MathUtils.Cross(r, _impulse);
        }

        internal override void SolveVelocityConstraints(ref TimeStep step)
        {
            Body b = BodyA;

            Transform xf1;
            b.GetTransform(out xf1);

            Vector2 r = MathUtils.Multiply(ref xf1.R, LocalAnchorA - b.LocalCenter);

            // Cdot = v + cross(w, r)
            Vector2 Cdot = b.LinearVelocityInternal + MathUtils.Cross(b.AngularVelocityInternal, r);
            Vector2 impulse = MathUtils.Multiply(ref _mass, -(Cdot + _beta * _C + _gamma * _impulse));

            Vector2 oldImpulse = _impulse;
            _impulse += impulse;
            float maxImpulse = step.dt * MaxForce;
            if (_impulse.LengthSquared > maxImpulse * maxImpulse)
            {
                _impulse *= maxImpulse / _impulse.Length;
            }
            impulse = _impulse - oldImpulse;

            b.LinearVelocityInternal += b.InvMass * impulse;
            b.AngularVelocityInternal += b.InvI * MathUtils.Cross(r, impulse);
        }

        internal override bool SolvePositionConstraints()
        {
            return true;
        }
    }
}