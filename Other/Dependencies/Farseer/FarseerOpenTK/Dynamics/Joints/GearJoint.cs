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
    /// A gear joint is used to connect two joints together. Either joint
    /// can be a revolute or prismatic joint. You specify a gear ratio
    /// to bind the motions together:
    /// coordinate1 + ratio * coordinate2 = ant
    /// The ratio can be negative or positive. If one joint is a revolute joint
    /// and the other joint is a prismatic joint, then the ratio will have units
    /// of length or units of 1/length.
    /// @warning The revolute and prismatic joints must be attached to
    /// fixed bodies (which must be body1 on those joints).
    /// </summary>
    public class GearJoint : Joint
    {
        private Jacobian _J;

        private float _ant;
        private FixedPrismaticJoint _fixedPrismatic1;
        private FixedPrismaticJoint _fixedPrismatic2;
        private FixedRevoluteJoint _fixedRevolute1;
        private FixedRevoluteJoint _fixedRevolute2;
        private float _impulse;
        private float _mass;
        private PrismaticJoint _prismatic1;
        private PrismaticJoint _prismatic2;
        private RevoluteJoint _revolute1;
        private RevoluteJoint _revolute2;

        /// <summary>
        /// Requires two existing revolute or prismatic joints (any combination will work).
        /// The provided joints must attach a dynamic body to a static body.
        /// </summary>
        /// <param name="jointA">The first joint.</param>
        /// <param name="jointB">The second joint.</param>
        /// <param name="ratio">The ratio.</param>
        public GearJoint(Joint jointA, Joint jointB, float ratio)
            : base(jointA.BodyA, jointA.BodyB)
        {
            JointType = JointType.Gear;
            JointA = jointA;
            JointB = jointB;
            Ratio = ratio;

            JointType type1 = jointA.JointType;
            JointType type2 = jointB.JointType;

            // Make sure its the right kind of joint
            Debug.Assert(type1 == JointType.Revolute ||
                         type1 == JointType.Prismatic ||
                         type1 == JointType.FixedRevolute ||
                         type1 == JointType.FixedPrismatic);
            Debug.Assert(type2 == JointType.Revolute ||
                         type2 == JointType.Prismatic ||
                         type2 == JointType.FixedRevolute ||
                         type2 == JointType.FixedPrismatic);

            // In the case of a prismatic and revolute joint, the first body must be static.
            if (type1 == JointType.Revolute || type1 == JointType.Prismatic)
                Debug.Assert(jointA.BodyA.BodyType == BodyType.Static);
            if (type2 == JointType.Revolute || type2 == JointType.Prismatic)
                Debug.Assert(jointB.BodyA.BodyType == BodyType.Static);

            float coordinate1 = 0.0f, coordinate2 = 0.0f;

            switch (type1)
            {
                case JointType.Revolute:
                    BodyA = jointA.BodyB;
                    _revolute1 = (RevoluteJoint)jointA;
                    LocalAnchor1 = _revolute1.LocalAnchorB;
                    coordinate1 = _revolute1.JointAngle;
                    break;
                case JointType.Prismatic:
                    BodyA = jointA.BodyB;
                    _prismatic1 = (PrismaticJoint)jointA;
                    LocalAnchor1 = _prismatic1.LocalAnchorB;
                    coordinate1 = _prismatic1.JointTranslation;
                    break;
                case JointType.FixedRevolute:
                    BodyA = jointA.BodyA;
                    _fixedRevolute1 = (FixedRevoluteJoint)jointA;
                    LocalAnchor1 = _fixedRevolute1.LocalAnchorA;
                    coordinate1 = _fixedRevolute1.JointAngle;
                    break;
                case JointType.FixedPrismatic:
                    BodyA = jointA.BodyA;
                    _fixedPrismatic1 = (FixedPrismaticJoint)jointA;
                    LocalAnchor1 = _fixedPrismatic1.LocalAnchorA;
                    coordinate1 = _fixedPrismatic1.JointTranslation;
                    break;
            }

            switch (type2)
            {
                case JointType.Revolute:
                    BodyB = jointB.BodyB;
                    _revolute2 = (RevoluteJoint)jointB;
                    LocalAnchor2 = _revolute2.LocalAnchorB;
                    coordinate2 = _revolute2.JointAngle;
                    break;
                case JointType.Prismatic:
                    BodyB = jointB.BodyB;
                    _prismatic2 = (PrismaticJoint)jointB;
                    LocalAnchor2 = _prismatic2.LocalAnchorB;
                    coordinate2 = _prismatic2.JointTranslation;
                    break;
                case JointType.FixedRevolute:
                    BodyB = jointB.BodyA;
                    _fixedRevolute2 = (FixedRevoluteJoint)jointB;
                    LocalAnchor2 = _fixedRevolute2.LocalAnchorA;
                    coordinate2 = _fixedRevolute2.JointAngle;
                    break;
                case JointType.FixedPrismatic:
                    BodyB = jointB.BodyA;
                    _fixedPrismatic2 = (FixedPrismaticJoint)jointB;
                    LocalAnchor2 = _fixedPrismatic2.LocalAnchorA;
                    coordinate2 = _fixedPrismatic2.JointTranslation;
                    break;
            }

            _ant = coordinate1 + Ratio * coordinate2;
        }

        public override Vector2 WorldAnchorA
        {
            get { return BodyA.GetWorldPoint(LocalAnchor1); }
        }

        public override Vector2 WorldAnchorB
        {
            get { return BodyB.GetWorldPoint(LocalAnchor2); }
            set { Debug.Assert(false, "You can't set the world anchor on this joint type."); }
        }

        /// <summary>
        /// The gear ratio.
        /// </summary>
        public float Ratio { get; set; }

        /// <summary>
        /// The first revolute/prismatic joint attached to the gear joint.
        /// </summary>
        public Joint JointA { get; set; }

        /// <summary>
        /// The second revolute/prismatic joint attached to the gear joint.
        /// </summary>
        public Joint JointB { get; set; }

        public Vector2 LocalAnchor1 { get; private set; }
        public Vector2 LocalAnchor2 { get; private set; }

        public override Vector2 GetReactionForce(float inv_dt)
        {
            Vector2 P = _impulse * _J.LinearB;
            return inv_dt * P;
        }

        public override float GetReactionTorque(float inv_dt)
        {
            Transform xf1;
            BodyB.GetTransform(out xf1);

            Vector2 r = MathUtils.Multiply(ref xf1.R, LocalAnchor2 - BodyB.LocalCenter);
            Vector2 P = _impulse * _J.LinearB;
            float L = _impulse * _J.AngularB - MathUtils.Cross(r, P);
            return inv_dt * L;
        }

        internal override void InitVelocityConstraints(ref TimeStep step)
        {
            Body b1 = BodyA;
            Body b2 = BodyB;

            float K = 0.0f;
            _J.SetZero();

            if (_revolute1 != null || _fixedRevolute1 != null)
            {
                _J.AngularA = -1.0f;
                K += b1.InvI;
            }
            else
            {
                Vector2 ug;
                if (_prismatic1 != null)
                    ug = _prismatic1.LocalXAxis1; // MathUtils.Multiply(ref xfg1.R, _prismatic1.LocalXAxis1);
                else
                    ug = _fixedPrismatic1.LocalXAxis1; // MathUtils.Multiply(ref xfg1.R, _prismatic1.LocalXAxis1);

                Transform xf1 /*, xfg1*/;
                b1.GetTransform(out xf1);
                //g1.GetTransform(out xfg1);


                Vector2 r = MathUtils.Multiply(ref xf1.R, LocalAnchor1 - b1.LocalCenter);
                float crug = MathUtils.Cross(r, ug);
                _J.LinearA = -ug;
                _J.AngularA = -crug;
                K += b1.InvMass + b1.InvI * crug * crug;
            }

            if (_revolute2 != null || _fixedRevolute2 != null)
            {
                _J.AngularB = -Ratio;
                K += Ratio * Ratio * b2.InvI;
            }
            else
            {
                Vector2 ug;
                if (_prismatic2 != null)
                    ug = _prismatic2.LocalXAxis1; // MathUtils.Multiply(ref xfg1.R, _prismatic1.LocalXAxis1);
                else
                    ug = _fixedPrismatic2.LocalXAxis1; // MathUtils.Multiply(ref xfg1.R, _prismatic1.LocalXAxis1);

                Transform /*xfg1,*/ xf2;
                //g1.GetTransform(out xfg1);
                b2.GetTransform(out xf2);

                Vector2 r = MathUtils.Multiply(ref xf2.R, LocalAnchor2 - b2.LocalCenter);
                float crug = MathUtils.Cross(r, ug);
                _J.LinearB = -Ratio * ug;
                _J.AngularB = -Ratio * crug;
                K += Ratio * Ratio * (b2.InvMass + b2.InvI * crug * crug);
            }

            // Compute effective mass.
            Debug.Assert(K > 0.0f);
            _mass = K > 0.0f ? 1.0f / K : 0.0f;

            if (Settings.EnableWarmstarting)
            {
                // Warm starting.
                b1.LinearVelocityInternal += b1.InvMass * _impulse * _J.LinearA;
                b1.AngularVelocityInternal += b1.InvI * _impulse * _J.AngularA;
                b2.LinearVelocityInternal += b2.InvMass * _impulse * _J.LinearB;
                b2.AngularVelocityInternal += b2.InvI * _impulse * _J.AngularB;
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

            float Cdot = _J.Compute(b1.LinearVelocityInternal, b1.AngularVelocityInternal,
                                    b2.LinearVelocityInternal, b2.AngularVelocityInternal);

            float impulse = _mass * (-Cdot);
            _impulse += impulse;

            b1.LinearVelocityInternal += b1.InvMass * impulse * _J.LinearA;
            b1.AngularVelocityInternal += b1.InvI * impulse * _J.AngularA;
            b2.LinearVelocityInternal += b2.InvMass * impulse * _J.LinearB;
            b2.AngularVelocityInternal += b2.InvI * impulse * _J.AngularB;
        }

        internal override bool SolvePositionConstraints()
        {
            const float linearError = 0.0f;

            Body b1 = BodyA;
            Body b2 = BodyB;

            float coordinate1 = 0.0f, coordinate2 = 0.0f;
            if (_revolute1 != null)
            {
                coordinate1 = _revolute1.JointAngle;
            }
            else if (_fixedRevolute1 != null)
            {
                coordinate1 = _fixedRevolute1.JointAngle;
            }
            else if (_prismatic1 != null)
            {
                coordinate1 = _prismatic1.JointTranslation;
            }
            else if (_fixedPrismatic1 != null)
            {
                coordinate1 = _fixedPrismatic1.JointTranslation;
            }

            if (_revolute2 != null)
            {
                coordinate2 = _revolute2.JointAngle;
            }
            else if (_fixedRevolute2 != null)
            {
                coordinate2 = _fixedRevolute2.JointAngle;
            }
            else if (_prismatic2 != null)
            {
                coordinate2 = _prismatic2.JointTranslation;
            }
            else if (_fixedPrismatic2 != null)
            {
                coordinate2 = _fixedPrismatic2.JointTranslation;
            }

            float C = _ant - (coordinate1 + Ratio * coordinate2);

            float impulse = _mass * (-C);

            b1.Sweep.C += b1.InvMass * impulse * _J.LinearA;
            b1.Sweep.A += b1.InvI * impulse * _J.AngularA;
            b2.Sweep.C += b2.InvMass * impulse * _J.LinearB;
            b2.Sweep.A += b2.InvI * impulse * _J.AngularB;

            b1.SynchronizeTransform();
            b2.SynchronizeTransform();

            // TODO_ERIN not implemented
            return linearError < Settings.LinearSlop;
        }
    }
}