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
    public class LineJoint : Joint
    {
        private Vector2 _ax, _ay;
        private float _bias;
        private bool _enableMotor;
        private float _gamma;
        private float _impulse;
        private Vector2 _localXAxis;
        private Vector2 _localYAxisA;
        private float _mass;
        private float _maxMotorTorque;
        private float _motorImpulse;
        private float _motorMass;
        private float _motorSpeed;

        private float _sAx;
        private float _sAy;
        private float _sBx;
        private float _sBy;

        private float _springImpulse;
        private float _springMass;

        // Linear constraint (point-to-line)
        // d = pB - pA = xB + rB - xA - rA
        // C = dot(ay, d)
        // Cdot = dot(d, cross(wA, ay)) + dot(ay, vB + cross(wB, rB) - vA - cross(wA, rA))
        //      = -dot(ay, vA) - dot(cross(d + rA, ay), wA) + dot(ay, vB) + dot(cross(rB, ay), vB)
        // J = [-ay, -cross(d + rA, ay), ay, cross(rB, ay)]

        // Spring linear constraint
        // C = dot(ax, d)
        // Cdot = = -dot(ax, vA) - dot(cross(d + rA, ax), wA) + dot(ax, vB) + dot(cross(rB, ax), vB)
        // J = [-ax -cross(d+rA, ax) ax cross(rB, ax)]

        // Motor rotational constraint
        // Cdot = wB - wA
        // J = [0 0 -1 0 0 1]

        internal LineJoint()
        {
            JointType = JointType.Line;
        }

        public LineJoint(Body bA, Body bB, Vector2 anchor, Vector2 axis)
            : base(bA, bB)
        {
            JointType = JointType.Line;

            LocalAnchorA = bA.GetLocalPoint(anchor);
            LocalAnchorB = bB.GetLocalPoint(anchor);
            LocalXAxis = bA.GetLocalVector(axis);
        }

        public Vector2 LocalAnchorA { get; set; }

        public Vector2 LocalAnchorB { get; set; }

        public override Vector2 WorldAnchorA
        {
            get { return BodyA.GetWorldPoint(LocalAnchorA); }
        }

        public override Vector2 WorldAnchorB
        {
            get { return BodyB.GetWorldPoint(LocalAnchorB); }
            set { Debug.Assert(false, "You can't set the world anchor on this joint type."); }
        }

        public float JointTranslation
        {
            get
            {
                Body bA = BodyA;
                Body bB = BodyB;

                Vector2 pA = bA.GetWorldPoint(LocalAnchorA);
                Vector2 pB = bB.GetWorldPoint(LocalAnchorB);
                Vector2 d = pB - pA;
                Vector2 axis = bA.GetWorldVector(LocalXAxis);

                float translation = Vector2.Dot(d, axis);
                return translation;
            }
        }

        public float JointSpeed
        {
            get
            {
                float wA = BodyA.AngularVelocityInternal;
                float wB = BodyB.AngularVelocityInternal;
                return wB - wA;
            }
        }

        public bool MotorEnabled
        {
            get { return _enableMotor; }
            set
            {
                BodyA.Awake = true;
                BodyB.Awake = true;
                _enableMotor = value;
            }
        }

        public float MotorSpeed
        {
            set
            {
                BodyA.Awake = true;
                BodyB.Awake = true;
                _motorSpeed = value;
            }
            get { return _motorSpeed; }
        }

        public float MaxMotorTorque
        {
            set
            {
                BodyA.Awake = true;
                BodyB.Awake = true;
                _maxMotorTorque = value;
            }
            get { return _maxMotorTorque; }
        }

        public float Frequency { get; set; }

        public float DampingRatio { get; set; }

        public Vector2 LocalXAxis
        {
            get { return _localXAxis; }
            set
            {
                _localXAxis = value;
                _localYAxisA = MathUtils.Cross(1.0f, _localXAxis);
            }
        }

        public override Vector2 GetReactionForce(float invDt)
        {
            return invDt * (_impulse * _ay + _springImpulse * _ax);
        }

        public override float GetReactionTorque(float invDt)
        {
            return invDt * _motorImpulse;
        }

        internal override void InitVelocityConstraints(ref TimeStep step)
        {
            Body bA = BodyA;
            Body bB = BodyB;

            LocalCenterA = bA.LocalCenter;
            LocalCenterB = bB.LocalCenter;

            Transform xfA;
            bA.GetTransform(out xfA);
            Transform xfB;
            bB.GetTransform(out xfB);

            // Compute the effective masses.
            Vector2 rA = MathUtils.Multiply(ref xfA.R, LocalAnchorA - LocalCenterA);
            Vector2 rB = MathUtils.Multiply(ref xfB.R, LocalAnchorB - LocalCenterB);
            Vector2 d = bB.Sweep.C + rB - bA.Sweep.C - rA;

            InvMassA = bA.InvMass;
            InvIA = bA.InvI;
            InvMassB = bB.InvMass;
            InvIB = bB.InvI;

            // Point to line constraint
            {
                _ay = MathUtils.Multiply(ref xfA.R, _localYAxisA);
                _sAy = MathUtils.Cross(d + rA, _ay);
                _sBy = MathUtils.Cross(rB, _ay);

                _mass = InvMassA + InvMassB + InvIA * _sAy * _sAy + InvIB * _sBy * _sBy;

                if (_mass > 0.0f)
                {
                    _mass = 1.0f / _mass;
                }
            }

            // Spring constraint
            _springMass = 0.0f;
            if (Frequency > 0.0f)
            {
                _ax = MathUtils.Multiply(ref xfA.R, LocalXAxis);
                _sAx = MathUtils.Cross(d + rA, _ax);
                _sBx = MathUtils.Cross(rB, _ax);

                float invMass = InvMassA + InvMassB + InvIA * _sAx * _sAx + InvIB * _sBx * _sBx;

                if (invMass > 0.0f)
                {
                    _springMass = 1.0f / invMass;

                    float C = Vector2.Dot(d, _ax);

                    // Frequency
                    float omega = 2.0f * Settings.Pi * Frequency;

                    // Damping coefficient
                    float da = 2.0f * _springMass * DampingRatio * omega;

                    // Spring stiffness
                    float k = _springMass * omega * omega;

                    // magic formulas
                    _gamma = step.dt * (da + step.dt * k);
                    if (_gamma > 0.0f)
                    {
                        _gamma = 1.0f / _gamma;
                    }

                    _bias = C * step.dt * k * _gamma;

                    _springMass = invMass + _gamma;
                    if (_springMass > 0.0f)
                    {
                        _springMass = 1.0f / _springMass;
                    }
                }
            }
            else
            {
                _springImpulse = 0.0f;
                _springMass = 0.0f;
            }

            // Rotational motor
            if (_enableMotor)
            {
                _motorMass = InvIA + InvIB;
                if (_motorMass > 0.0f)
                {
                    _motorMass = 1.0f / _motorMass;
                }
            }
            else
            {
                _motorMass = 0.0f;
                _motorImpulse = 0.0f;
            }

            if (Settings.EnableWarmstarting)
            {
                // Account for variable time step.
                _impulse *= step.dtRatio;
                _springImpulse *= step.dtRatio;
                _motorImpulse *= step.dtRatio;

                Vector2 P = _impulse * _ay + _springImpulse * _ax;
                float LA = _impulse * _sAy + _springImpulse * _sAx + _motorImpulse;
                float LB = _impulse * _sBy + _springImpulse * _sBx + _motorImpulse;

                bA.LinearVelocityInternal -= InvMassA * P;
                bA.AngularVelocityInternal -= InvIA * LA;

                bB.LinearVelocityInternal += InvMassB * P;
                bB.AngularVelocityInternal += InvIB * LB;
            }
            else
            {
                _impulse = 0.0f;
                _springImpulse = 0.0f;
                _motorImpulse = 0.0f;
            }
        }

        internal override void SolveVelocityConstraints(ref TimeStep step)
        {
            Body bA = BodyA;
            Body bB = BodyB;

            Vector2 vA = bA.LinearVelocity;
            float wA = bA.AngularVelocityInternal;
            Vector2 vB = bB.LinearVelocityInternal;
            float wB = bB.AngularVelocityInternal;

            // Solve spring constraint
            {
                float Cdot = Vector2.Dot(_ax, vB - vA) + _sBx * wB - _sAx * wA;
                float impulse = -_springMass * (Cdot + _bias + _gamma * _springImpulse);
                _springImpulse += impulse;

                Vector2 P = impulse * _ax;
                float LA = impulse * _sAx;
                float LB = impulse * _sBx;

                vA -= InvMassA * P;
                wA -= InvIA * LA;

                vB += InvMassB * P;
                wB += InvIB * LB;
            }

            // Solve rotational motor constraint
            {
                float Cdot = wB - wA - _motorSpeed;
                float impulse = -_motorMass * Cdot;

                float oldImpulse = _motorImpulse;
                float maxImpulse = step.dt * _maxMotorTorque;
                _motorImpulse = MathUtils.Clamp(_motorImpulse + impulse, -maxImpulse, maxImpulse);
                impulse = _motorImpulse - oldImpulse;

                wA -= InvIA * impulse;
                wB += InvIB * impulse;
            }

            // Solve point to line constraint
            {
                float Cdot = Vector2.Dot(_ay, vB - vA) + _sBy * wB - _sAy * wA;
                float impulse = _mass * (-Cdot);
                _impulse += impulse;

                Vector2 P = impulse * _ay;
                float LA = impulse * _sAy;
                float LB = impulse * _sBy;

                vA -= InvMassA * P;
                wA -= InvIA * LA;

                vB += InvMassB * P;
                wB += InvIB * LB;
            }

            bA.LinearVelocityInternal = vA;
            bA.AngularVelocityInternal = wA;
            bB.LinearVelocityInternal = vB;
            bB.AngularVelocityInternal = wB;
        }

        internal override bool SolvePositionConstraints()
        {
            Body bA = BodyA;
            Body bB = BodyB;

            Vector2 xA = bA.Sweep.C;
            float angleA = bA.Sweep.A;

            Vector2 xB = bB.Sweep.C;
            float angleB = bB.Sweep.A;

            Mat22 RA = new Mat22(angleA);
            Mat22 RB = new Mat22(angleB);

            Vector2 rA = MathUtils.Multiply(ref RA, LocalAnchorA - LocalCenterA);
            Vector2 rB = MathUtils.Multiply(ref RB, LocalAnchorB - LocalCenterB);
            Vector2 d = xB + rB - xA - rA;

            Vector2 ay = MathUtils.Multiply(ref RA, _localYAxisA);

            float sAy = MathUtils.Cross(d + rA, ay);
            float sBy = MathUtils.Cross(rB, ay);

            float C = Vector2.Dot(d, ay);

            float k = InvMassA + InvMassB + InvIA * _sAy * _sAy + InvIB * _sBy * _sBy;

            float impulse;
            if (k != 0.0f)
            {
                impulse = -C / k;
            }
            else
            {
                impulse = 0.0f;
            }

            Vector2 P = impulse * ay;
            float LA = impulse * sAy;
            float LB = impulse * sBy;

            xA -= InvMassA * P;
            angleA -= InvIA * LA;
            xB += InvMassB * P;
            angleB += InvIB * LB;

            // TODO_ERIN remove need for this.
            bA.Sweep.C = xA;
            bA.Sweep.A = angleA;
            bB.Sweep.C = xB;
            bB.Sweep.A = angleB;
            bA.SynchronizeTransform();
            bB.SynchronizeTransform();

            return Math.Abs(C) <= Settings.LinearSlop;
        }

        public float GetMotorTorque(float invDt)
        {
            return invDt * _motorImpulse;
        }
    }
}