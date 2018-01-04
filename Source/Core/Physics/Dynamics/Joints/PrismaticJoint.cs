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
    // Linear constraint (point-to-line)
    // d = p2 - p1 = x2 + r2 - x1 - r1
    // C = dot(perp, d)
    // Cdot = dot(d, cross(w1, perp)) + dot(perp, v2 + cross(w2, r2) - v1 - cross(w1, r1))
    //      = -dot(perp, v1) - dot(cross(d + r1, perp), w1) + dot(perp, v2) + dot(cross(r2, perp), v2)
    // J = [-perp, -cross(d + r1, perp), perp, cross(r2,perp)]
    //
    // Angular constraint
    // C = a2 - a1 + a_initial
    // Cdot = w2 - w1
    // J = [0 0 -1 0 0 1]
    //
    // K = J * invM * JT
    //
    // J = [-a -s1 a s2]
    //     [0  -1  0  1]
    // a = perp
    // s1 = cross(d + r1, a) = cross(p2 - x1, a)
    // s2 = cross(r2, a) = cross(p2 - x2, a)
    // Motor/Limit linear constraint
    // C = dot(ax1, d)
    // Cdot = = -dot(ax1, v1) - dot(cross(d + r1, ax1), w1) + dot(ax1, v2) + dot(cross(r2, ax1), v2)
    // J = [-ax1 -cross(d+r1,ax1) ax1 cross(r2,ax1)]
    // Block Solver
    // We develop a block solver that includes the joint limit. This makes the limit stiff (inelastic) even
    // when the mass has poor distribution (leading to large torques about the joint anchor points).
    //
    // The Jacobian has 3 rows:
    // J = [-uT -s1 uT s2] // linear
    //     [0   -1   0  1] // angular
    //     [-vT -a1 vT a2] // limit
    //
    // u = perp
    // v = axis
    // s1 = cross(d + r1, u), s2 = cross(r2, u)
    // a1 = cross(d + r1, v), a2 = cross(r2, v)
    // M * (v2 - v1) = JT * df
    // J * v2 = bias
    //
    // v2 = v1 + invM * JT * df
    // J * (v1 + invM * JT * df) = bias
    // K * df = bias - J * v1 = -Cdot
    // K = J * invM * JT
    // Cdot = J * v1 - bias
    //
    // Now solve for f2.
    // df = f2 - f1
    // K * (f2 - f1) = -Cdot
    // f2 = invK * (-Cdot) + f1
    //
    // Clamp accumulated limit impulse.
    // lower: f2(3) = max(f2(3), 0)
    // upper: f2(3) = min(f2(3), 0)
    //
    // Solve for correct f2(1:2)
    // K(1:2, 1:2) * f2(1:2) = -Cdot(1:2) - K(1:2,3) * f2(3) + K(1:2,1:3) * f1
    //                       = -Cdot(1:2) - K(1:2,3) * f2(3) + K(1:2,1:2) * f1(1:2) + K(1:2,3) * f1(3)
    // K(1:2, 1:2) * f2(1:2) = -Cdot(1:2) - K(1:2,3) * (f2(3) - f1(3)) + K(1:2,1:2) * f1(1:2)
    // f2(1:2) = invK(1:2,1:2) * (-Cdot(1:2) - K(1:2,3) * (f2(3) - f1(3))) + f1(1:2)
    //
    // Now compute impulse to be applied:
    // df = f2 - f1

    /// <summary>
    /// A prismatic joint. This joint provides one degree of freedom: translation
    /// along an axis fixed in body1. Relative rotation is prevented. You can
    /// use a joint limit to restrict the range of motion and a joint motor to
    /// drive the motion or to model joint friction.
    /// </summary>
    public class PrismaticJoint : Joint
    {
        public Vector2 LocalAnchorA;

        public Vector2 LocalAnchorB;
        private Mat33 _K;
        private float _a1, _a2;
        private Vector2 _axis;
        private bool _enableLimit;
        private bool _enableMotor;
        private Vector3 _impulse;
        private LimitState _limitState;
        private Vector2 _localXAxis1;
        private Vector2 _localYAxis1;
        private float _lowerTranslation;
        private float _maxMotorForce;
        private float _motorImpulse;
        private float _motorMass; // effective mass for motor/limit translational constraint.
        private float _motorSpeed;
        private Vector2 _perp;
        private float _refAngle;
        private float _s1, _s2;
        private float _upperTranslation;

        internal PrismaticJoint()
        {
            JointType = JointType.Prismatic;
        }

        /// <summary>
        /// This requires defining a line of
        /// motion using an axis and an anchor point. The definition uses local
        /// anchor points and a local axis so that the initial configuration
        /// can violate the constraint slightly. The joint translation is zero
        /// when the local anchor points coincide in world space. Using local
        /// anchors and a local axis helps when saving and loading a game.
        /// </summary>
        /// <param name="bodyA">The first body.</param>
        /// <param name="bodyB">The second body.</param>
        /// <param name="localAnchorA">The first body anchor.</param>
        /// <param name="localAnchorB">The second body anchor.</param>
        /// <param name="axis">The axis.</param>
        public PrismaticJoint(Body bodyA, Body bodyB, Vector2 localAnchorA, Vector2 localAnchorB, Vector2 axis)
            : base(bodyA, bodyB)
        {
            JointType = JointType.Prismatic;

            LocalAnchorA = localAnchorA;
            LocalAnchorB = localAnchorB;

            _localXAxis1 = BodyA.GetLocalVector(axis);
            _localYAxis1 = MathUtils.Cross(1.0f, _localXAxis1);
            _refAngle = BodyB.Rotation - BodyA.Rotation;

            _limitState = LimitState.Inactive;
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
        /// Get the current joint translation, usually in meters.
        /// </summary>
        /// <value></value>
        public float JointTranslation
        {
            get
            {
                Vector2 d = BodyB.GetWorldPoint(LocalAnchorB) - BodyA.GetWorldPoint(LocalAnchorA);
                Vector2 axis = BodyA.GetWorldVector(ref _localXAxis1);

                return Vector2.Dot(d, axis);
            }
        }

        /// <summary>
        /// Get the current joint translation speed, usually in meters per second.
        /// </summary>
        /// <value></value>
        public float JointSpeed
        {
            get
            {
                Transform xf1, xf2;
                BodyA.GetTransform(out xf1);
                BodyB.GetTransform(out xf2);

                Vector2 r1 = MathUtils.Multiply(ref xf1.R, LocalAnchorA - BodyA.LocalCenter);
                Vector2 r2 = MathUtils.Multiply(ref xf2.R, LocalAnchorB - BodyB.LocalCenter);
                Vector2 p1 = BodyA.Sweep.C + r1;
                Vector2 p2 = BodyB.Sweep.C + r2;
                Vector2 d = p2 - p1;
                Vector2 axis = BodyA.GetWorldVector(ref _localXAxis1);

                Vector2 v1 = BodyA.LinearVelocityInternal;
                Vector2 v2 = BodyB.LinearVelocityInternal;
                float w1 = BodyA.AngularVelocityInternal;
                float w2 = BodyB.AngularVelocityInternal;

                float speed = Vector2.Dot(d, MathUtils.Cross(w1, axis)) +
                              Vector2.Dot(axis, v2 + MathUtils.Cross(w2, r2) - v1 - MathUtils.Cross(w1, r1));
                return speed;
            }
        }

        /// <summary>
        /// Is the joint limit enabled?
        /// </summary>
        /// <value><c>true</c> if [limit enabled]; otherwise, <c>false</c>.</value>
        public bool LimitEnabled
        {
            get { return _enableLimit; }
            set
            {
                Debug.Assert(BodyA.FixedRotation == false || BodyB.FixedRotation == false,
                             "Warning: limits does currently not work with fixed rotation");

                WakeBodies();
                _enableLimit = value;
            }
        }

        /// <summary>
        /// Get the lower joint limit, usually in meters.
        /// </summary>
        /// <value></value>
        public float LowerLimit
        {
            get { return _lowerTranslation; }
            set
            {
                WakeBodies();
                _lowerTranslation = value;
            }
        }

        /// <summary>
        /// Get the upper joint limit, usually in meters.
        /// </summary>
        /// <value></value>
        public float UpperLimit
        {
            get { return _upperTranslation; }
            set
            {
                WakeBodies();
                _upperTranslation = value;
            }
        }

        /// <summary>
        /// Is the joint motor enabled?
        /// </summary>
        /// <value><c>true</c> if [motor enabled]; otherwise, <c>false</c>.</value>
        public bool MotorEnabled
        {
            get { return _enableMotor; }
            set
            {
                WakeBodies();
                _enableMotor = value;
            }
        }

        /// <summary>
        /// Set the motor speed, usually in meters per second.
        /// </summary>
        /// <value>The speed.</value>
        public float MotorSpeed
        {
            set
            {
                WakeBodies();
                _motorSpeed = value;
            }
            get { return _motorSpeed; }
        }

        /// <summary>
        /// Set the maximum motor force, usually in N.
        /// </summary>
        /// <value>The force.</value>
        public float MaxMotorForce
        {
            get { return _maxMotorForce; }
            set
            {
                WakeBodies();
                _maxMotorForce = value;
            }
        }

        /// <summary>
        /// Get the current motor force, usually in N.
        /// </summary>
        /// <value></value>
        public float MotorForce
        {
            get { return _motorImpulse; }
            set { _motorImpulse = value; }
        }

        public Vector2 LocalXAxis1
        {
            get { return _localXAxis1; }
            set
            {
                _localXAxis1 = BodyA.GetLocalVector(value);
                _localYAxis1 = MathUtils.Cross(1.0f, _localXAxis1);
            }
        }

        public float ReferenceAngle
        {
            get { return _refAngle; }
            set { WakeBodies(); _refAngle = value; }
        }

        public override Vector2 GetReactionForce(float inv_dt)
        {
            return inv_dt * (_impulse.X * _perp + (_motorImpulse + _impulse.Z) * _axis);
        }

        public override float GetReactionTorque(float inv_dt)
        {
            return inv_dt * _impulse.Y;
        }

        internal override void InitVelocityConstraints(ref TimeStep step)
        {
            Body b1 = BodyA;
            Body b2 = BodyB;

            LocalCenterA = b1.LocalCenter;
            LocalCenterB = b2.LocalCenter;

            Transform xf1, xf2;
            b1.GetTransform(out xf1);
            b2.GetTransform(out xf2);

            // Compute the effective masses.
            Vector2 r1 = MathUtils.Multiply(ref xf1.R, LocalAnchorA - LocalCenterA);
            Vector2 r2 = MathUtils.Multiply(ref xf2.R, LocalAnchorB - LocalCenterB);
            Vector2 d = b2.Sweep.C + r2 - b1.Sweep.C - r1;

            InvMassA = b1.InvMass;
            InvIA = b1.InvI;
            InvMassB = b2.InvMass;
            InvIB = b2.InvI;

            // Compute motor Jacobian and effective mass.
            {
                _axis = MathUtils.Multiply(ref xf1.R, _localXAxis1);
                _a1 = MathUtils.Cross(d + r1, _axis);
                _a2 = MathUtils.Cross(r2, _axis);

                _motorMass = InvMassA + InvMassB + InvIA * _a1 * _a1 + InvIB * _a2 * _a2;

                if (_motorMass > Settings.Epsilon)
                {
                    _motorMass = 1.0f / _motorMass;
                }
            }

            // Prismatic constraint.
            {
                _perp = MathUtils.Multiply(ref xf1.R, _localYAxis1);

                _s1 = MathUtils.Cross(d + r1, _perp);
                _s2 = MathUtils.Cross(r2, _perp);

                float m1 = InvMassA, m2 = InvMassB;
                float i1 = InvIA, i2 = InvIB;

                float k11 = m1 + m2 + i1 * _s1 * _s1 + i2 * _s2 * _s2;
                float k12 = i1 * _s1 + i2 * _s2;
                float k13 = i1 * _s1 * _a1 + i2 * _s2 * _a2;
                float k22 = i1 + i2;
                float k23 = i1 * _a1 + i2 * _a2;
                float k33 = m1 + m2 + i1 * _a1 * _a1 + i2 * _a2 * _a2;

                _K.Col1 = new Vector3(k11, k12, k13);
                _K.Col2 = new Vector3(k12, k22, k23);
                _K.Col3 = new Vector3(k13, k23, k33);
            }

            // Compute motor and limit terms.
            if (_enableLimit)
            {
                float jointTranslation = Vector2.Dot(_axis, d);
                if (Math.Abs(_upperTranslation - _lowerTranslation) < 2.0f * Settings.LinearSlop)
                {
                    _limitState = LimitState.Equal;
                }
                else if (jointTranslation <= _lowerTranslation)
                {
                    if (_limitState != LimitState.AtLower)
                    {
                        _limitState = LimitState.AtLower;
                        _impulse.Z = 0.0f;
                    }
                }
                else if (jointTranslation >= _upperTranslation)
                {
                    if (_limitState != LimitState.AtUpper)
                    {
                        _limitState = LimitState.AtUpper;
                        _impulse.Z = 0.0f;
                    }
                }
                else
                {
                    _limitState = LimitState.Inactive;
                    _impulse.Z = 0.0f;
                }
            }
            else
            {
                _limitState = LimitState.Inactive;
            }

            if (_enableMotor == false)
            {
                _motorImpulse = 0.0f;
            }

            if (Settings.EnableWarmstarting)
            {
                // Account for variable time step.
                _impulse *= step.dtRatio;
                _motorImpulse *= step.dtRatio;

                Vector2 P = _impulse.X * _perp + (_motorImpulse + _impulse.Z) * _axis;
                float L1 = _impulse.X * _s1 + _impulse.Y + (_motorImpulse + _impulse.Z) * _a1;
                float L2 = _impulse.X * _s2 + _impulse.Y + (_motorImpulse + _impulse.Z) * _a2;

                b1.LinearVelocityInternal -= InvMassA * P;
                b1.AngularVelocityInternal -= InvIA * L1;

                b2.LinearVelocityInternal += InvMassB * P;
                b2.AngularVelocityInternal += InvIB * L2;
            }
            else
            {
                _impulse = Vector3.Zero;
                _motorImpulse = 0.0f;
            }
        }

        internal override void SolveVelocityConstraints(ref TimeStep step)
        {
            Body b1 = BodyA;
            Body b2 = BodyB;

            Vector2 v1 = b1.LinearVelocityInternal;
            float w1 = b1.AngularVelocityInternal;
            Vector2 v2 = b2.LinearVelocityInternal;
            float w2 = b2.AngularVelocityInternal;

            // Solve linear motor constraint.
            if (_enableMotor && _limitState != LimitState.Equal)
            {
                float Cdot = Vector2.Dot(_axis, v2 - v1) + _a2 * w2 - _a1 * w1;
                float impulse = _motorMass * (_motorSpeed - Cdot);
                float oldImpulse = _motorImpulse;
                float maxImpulse = step.dt * _maxMotorForce;
                _motorImpulse = MathUtils.Clamp(_motorImpulse + impulse, -maxImpulse, maxImpulse);
                impulse = _motorImpulse - oldImpulse;

                Vector2 P = impulse * _axis;
                float L1 = impulse * _a1;
                float L2 = impulse * _a2;

                v1 -= InvMassA * P;
                w1 -= InvIA * L1;

                v2 += InvMassB * P;
                w2 += InvIB * L2;
            }

            Vector2 Cdot1 = new Vector2(Vector2.Dot(_perp, v2 - v1) + _s2 * w2 - _s1 * w1, w2 - w1);

            if (_enableLimit && _limitState != LimitState.Inactive)
            {
                // Solve prismatic and limit constraint in block form.
                float Cdot2 = Vector2.Dot(_axis, v2 - v1) + _a2 * w2 - _a1 * w1;
                Vector3 Cdot = new Vector3(Cdot1.X, Cdot1.Y, Cdot2);

                Vector3 f1 = _impulse;
                Vector3 df = _K.Solve33(-Cdot);
                _impulse += df;

                if (_limitState == LimitState.AtLower)
                {
                    _impulse.Z = Math.Max(_impulse.Z, 0.0f);
                }
                else if (_limitState == LimitState.AtUpper)
                {
                    _impulse.Z = Math.Min(_impulse.Z, 0.0f);
                }

                // f2(1:2) = invK(1:2,1:2) * (-Cdot(1:2) - K(1:2,3) * (f2(3) - f1(3))) + f1(1:2)
                Vector2 b = -Cdot1 - (_impulse.Z - f1.Z) * new Vector2(_K.Col3.X, _K.Col3.Y);
                Vector2 f2r = _K.Solve22(b) + new Vector2(f1.X, f1.Y);
                _impulse.X = f2r.X;
                _impulse.Y = f2r.Y;

                df = _impulse - f1;

                Vector2 P = df.X * _perp + df.Z * _axis;
                float L1 = df.X * _s1 + df.Y + df.Z * _a1;
                float L2 = df.X * _s2 + df.Y + df.Z * _a2;

                v1 -= InvMassA * P;
                w1 -= InvIA * L1;

                v2 += InvMassB * P;
                w2 += InvIB * L2;
            }
            else
            {
                // Limit is inactive, just solve the prismatic constraint in block form.
                Vector2 df = _K.Solve22(-Cdot1);
                _impulse.X += df.X;
                _impulse.Y += df.Y;

                Vector2 P = df.X * _perp;
                float L1 = df.X * _s1 + df.Y;
                float L2 = df.X * _s2 + df.Y;

                v1 -= InvMassA * P;
                w1 -= InvIA * L1;

                v2 += InvMassB * P;
                w2 += InvIB * L2;
            }

            b1.LinearVelocityInternal = v1;
            b1.AngularVelocityInternal = w1;
            b2.LinearVelocityInternal = v2;
            b2.AngularVelocityInternal = w2;
        }

        internal override bool SolvePositionConstraints()
        {
            Body b1 = BodyA;
            Body b2 = BodyB;

            Vector2 c1 = b1.Sweep.C;
            float a1 = b1.Sweep.A;

            Vector2 c2 = b2.Sweep.C;
            float a2 = b2.Sweep.A;

            // Solve linear limit constraint.
            float linearError = 0.0f;
            bool active = false;
            float C2 = 0.0f;

            Mat22 R1 = new Mat22(a1);
            Mat22 R2 = new Mat22(a2);

            Vector2 r1 = MathUtils.Multiply(ref R1, LocalAnchorA - LocalCenterA);
            Vector2 r2 = MathUtils.Multiply(ref R2, LocalAnchorB - LocalCenterB);
            Vector2 d = c2 + r2 - c1 - r1;

            if (_enableLimit)
            {
                _axis = MathUtils.Multiply(ref R1, _localXAxis1);

                _a1 = MathUtils.Cross(d + r1, _axis);
                _a2 = MathUtils.Cross(r2, _axis);

                float translation = Vector2.Dot(_axis, d);
                if (Math.Abs(_upperTranslation - _lowerTranslation) < 2.0f * Settings.LinearSlop)
                {
                    // Prevent large angular corrections
                    C2 = MathUtils.Clamp(translation, -Settings.MaxLinearCorrection, Settings.MaxLinearCorrection);
                    linearError = Math.Abs(translation);
                    active = true;
                }
                else if (translation <= _lowerTranslation)
                {
                    // Prevent large linear corrections and allow some slop.
                    C2 = MathUtils.Clamp(translation - _lowerTranslation + Settings.LinearSlop,
                                         -Settings.MaxLinearCorrection, 0.0f);
                    linearError = _lowerTranslation - translation;
                    active = true;
                }
                else if (translation >= _upperTranslation)
                {
                    // Prevent large linear corrections and allow some slop.
                    C2 = MathUtils.Clamp(translation - _upperTranslation - Settings.LinearSlop, 0.0f,
                                         Settings.MaxLinearCorrection);
                    linearError = translation - _upperTranslation;
                    active = true;
                }
            }

            _perp = MathUtils.Multiply(ref R1, _localYAxis1);

            _s1 = MathUtils.Cross(d + r1, _perp);
            _s2 = MathUtils.Cross(r2, _perp);

            Vector3 impulse;
            Vector2 C1 = new Vector2(Vector2.Dot(_perp, d), a2 - a1 - ReferenceAngle);

            linearError = Math.Max(linearError, Math.Abs(C1.X));
            float angularError = Math.Abs(C1.Y);

            if (active)
            {
                float m1 = InvMassA, m2 = InvMassB;
                float i1 = InvIA, i2 = InvIB;

                float k11 = m1 + m2 + i1 * _s1 * _s1 + i2 * _s2 * _s2;
                float k12 = i1 * _s1 + i2 * _s2;
                float k13 = i1 * _s1 * _a1 + i2 * _s2 * _a2;
                float k22 = i1 + i2;
                float k23 = i1 * _a1 + i2 * _a2;
                float k33 = m1 + m2 + i1 * _a1 * _a1 + i2 * _a2 * _a2;

                _K.Col1 = new Vector3(k11, k12, k13);
                _K.Col2 = new Vector3(k12, k22, k23);
                _K.Col3 = new Vector3(k13, k23, k33);

                Vector3 C = new Vector3(-C1.X, -C1.Y, -C2);
                impulse = _K.Solve33(C); // negated above
            }
            else
            {
                float m1 = InvMassA, m2 = InvMassB;
                float i1 = InvIA, i2 = InvIB;

                float k11 = m1 + m2 + i1 * _s1 * _s1 + i2 * _s2 * _s2;
                float k12 = i1 * _s1 + i2 * _s2;
                float k22 = i1 + i2;

                _K.Col1 = new Vector3(k11, k12, 0.0f);
                _K.Col2 = new Vector3(k12, k22, 0.0f);

                Vector2 impulse1 = _K.Solve22(-C1);
                impulse.X = impulse1.X;
                impulse.Y = impulse1.Y;
                impulse.Z = 0.0f;
            }

            Vector2 P = impulse.X * _perp + impulse.Z * _axis;
            float L1 = impulse.X * _s1 + impulse.Y + impulse.Z * _a1;
            float L2 = impulse.X * _s2 + impulse.Y + impulse.Z * _a2;

            c1 -= InvMassA * P;
            a1 -= InvIA * L1;
            c2 += InvMassB * P;
            a2 += InvIB * L2;

            // TODO_ERIN remove need for this.
            b1.Sweep.C = c1;
            b1.Sweep.A = a1;
            b2.Sweep.C = c2;
            b2.Sweep.A = a2;
            b1.SynchronizeTransform();
            b2.SynchronizeTransform();

            return linearError <= Settings.LinearSlop && angularError <= Settings.AngularSlop;
        }
    }
}