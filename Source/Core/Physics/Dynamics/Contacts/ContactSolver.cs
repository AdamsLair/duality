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
using FarseerPhysics.Collision;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using Duality;

namespace FarseerPhysics.Dynamics.Contacts
{
    public sealed class ContactConstraintPoint
    {
        public Vector2 LocalPoint;
        public float NormalImpulse;
        public float NormalMass;
        public float TangentImpulse;
        public float TangentMass;
        public float VelocityBias;
        public Vector2 rA;
        public Vector2 rB;
    }

    public sealed class ContactConstraint
    {
        public Body BodyA;
        public Body BodyB;
        public float Friction;
        public Mat22 K;
        public Vector2 LocalNormal;
        public Vector2 LocalPoint;
        public Manifold Manifold;
        public Vector2 Normal;
        public Mat22 NormalMass;
        public int PointCount;
        public ContactConstraintPoint[] Points = new ContactConstraintPoint[Settings.MaxPolygonVertices];
        public float RadiusA;
        public float RadiusB;
        public float Restitution;
        public ManifoldType Type;

        public ContactConstraint()
        {
            for (int i = 0; i < Settings.MaxManifoldPoints; i++)
            {
                Points[i] = new ContactConstraintPoint();
            }
        }
    }

    public class ContactSolver
    {
        public ContactConstraint[] Constraints;
        private int _constraintCount; // collection can be bigger.
        private Contact[] _contacts;

        public void Reset(Contact[] contacts, int contactCount, float impulseRatio, bool warmstarting)
        {
            _contacts = contacts;

            _constraintCount = contactCount;

            // grow the array
            if (Constraints == null || Constraints.Length < _constraintCount)
            {
                Constraints = new ContactConstraint[_constraintCount * 2];

                for (int i = 0; i < Constraints.Length; i++)
                {
                    Constraints[i] = new ContactConstraint();
                }
            }

            // Initialize position independent portions of the constraints.
            for (int i = 0; i < _constraintCount; ++i)
            {
                Contact contact = contacts[i];

                Fixture fixtureA = contact.FixtureA;
                Fixture fixtureB = contact.FixtureB;
                Shape shapeA = fixtureA.Shape;
                Shape shapeB = fixtureB.Shape;
                float radiusA = shapeA.Radius;
                float radiusB = shapeB.Radius;
                Body bodyA = fixtureA.Body;
                Body bodyB = fixtureB.Body;
                Manifold manifold = contact.Manifold;

                Debug.Assert(manifold.PointCount > 0);

                ContactConstraint cc = Constraints[i];
                cc.Friction = Settings.MixFriction(fixtureA.Friction, fixtureB.Friction);
                cc.Restitution = Settings.MixRestitution(fixtureA.Restitution, fixtureB.Restitution);
                cc.BodyA = bodyA;
                cc.BodyB = bodyB;
                cc.Manifold = manifold;
                cc.Normal = Vector2.Zero;
                cc.PointCount = manifold.PointCount;

                cc.LocalNormal = manifold.LocalNormal;
                cc.LocalPoint = manifold.LocalPoint;
                cc.RadiusA = radiusA;
                cc.RadiusB = radiusB;
                cc.Type = manifold.Type;

                for (int j = 0; j < cc.PointCount; ++j)
                {
                    ManifoldPoint cp = manifold.Points[j];
                    ContactConstraintPoint ccp = cc.Points[j];

                    if (warmstarting)
                    {
                        ccp.NormalImpulse = impulseRatio * cp.NormalImpulse;
                        ccp.TangentImpulse = impulseRatio * cp.TangentImpulse;
                    }
                    else
                    {
                        ccp.NormalImpulse = 0.0f;
                        ccp.TangentImpulse = 0.0f;
                    }

                    ccp.LocalPoint = cp.LocalPoint;
                    ccp.rA = Vector2.Zero;
                    ccp.rB = Vector2.Zero;
                    ccp.NormalMass = 0.0f;
                    ccp.TangentMass = 0.0f;
                    ccp.VelocityBias = 0.0f;
                }

                cc.K.SetZero();
                cc.NormalMass.SetZero();
            }
        }

        public void InitializeVelocityConstraints()
        {
            for (int i = 0; i < _constraintCount; ++i)
            {
                ContactConstraint cc = Constraints[i];

                float radiusA = cc.RadiusA;
                float radiusB = cc.RadiusB;
                Body bodyA = cc.BodyA;
                Body bodyB = cc.BodyB;
                Manifold manifold = cc.Manifold;

                Vector2 vA = bodyA.LinearVelocity;
                Vector2 vB = bodyB.LinearVelocity;
                float wA = bodyA.AngularVelocity;
                float wB = bodyB.AngularVelocity;

                Debug.Assert(manifold.PointCount > 0);
                FixedArray2<Vector2> points;

                Collision.Collision.GetWorldManifold(ref manifold, ref bodyA.Xf, radiusA, ref bodyB.Xf, radiusB,
                                                     out cc.Normal, out points);
                Vector2 tangent = new Vector2(cc.Normal.Y, -cc.Normal.X);

                for (int j = 0; j < cc.PointCount; ++j)
                {
                    ContactConstraintPoint ccp = cc.Points[j];

                    ccp.rA = points[j] - bodyA.Sweep.C;
                    ccp.rB = points[j] - bodyB.Sweep.C;

                    float rnA = ccp.rA.X * cc.Normal.Y - ccp.rA.Y * cc.Normal.X;
                    float rnB = ccp.rB.X * cc.Normal.Y - ccp.rB.Y * cc.Normal.X;
                    rnA *= rnA;
                    rnB *= rnB;

                    float kNormal = bodyA.InvMass + bodyB.InvMass + bodyA.InvI * rnA + bodyB.InvI * rnB;

                    Debug.Assert(kNormal > Settings.Epsilon);
                    ccp.NormalMass = 1.0f / kNormal;

                    float rtA = ccp.rA.X * tangent.Y - ccp.rA.Y * tangent.X;
                    float rtB = ccp.rB.X * tangent.Y - ccp.rB.Y * tangent.X;

                    rtA *= rtA;
                    rtB *= rtB;
                    float kTangent = bodyA.InvMass + bodyB.InvMass + bodyA.InvI * rtA + bodyB.InvI * rtB;

                    Debug.Assert(kTangent > Settings.Epsilon);
                    ccp.TangentMass = 1.0f / kTangent;

                    // Setup a velocity bias for restitution.
                    ccp.VelocityBias = 0.0f;
                    float vRel = cc.Normal.X * (vB.X + -wB * ccp.rB.Y - vA.X - -wA * ccp.rA.Y) +
                                 cc.Normal.Y * (vB.Y + wB * ccp.rB.X - vA.Y - wA * ccp.rA.X);
                    if (vRel < -Settings.VelocityThreshold)
                    {
                        ccp.VelocityBias = -cc.Restitution * vRel;
                    }
                }

                // If we have two points, then prepare the block solver.
                if (cc.PointCount == 2)
                {
                    ContactConstraintPoint ccp1 = cc.Points[0];
                    ContactConstraintPoint ccp2 = cc.Points[1];

                    float invMassA = bodyA.InvMass;
                    float invIA = bodyA.InvI;
                    float invMassB = bodyB.InvMass;
                    float invIB = bodyB.InvI;

                    float rn1A = ccp1.rA.X * cc.Normal.Y - ccp1.rA.Y * cc.Normal.X;
                    float rn1B = ccp1.rB.X * cc.Normal.Y - ccp1.rB.Y * cc.Normal.X;
                    float rn2A = ccp2.rA.X * cc.Normal.Y - ccp2.rA.Y * cc.Normal.X;
                    float rn2B = ccp2.rB.X * cc.Normal.Y - ccp2.rB.Y * cc.Normal.X;

                    float k11 = invMassA + invMassB + invIA * rn1A * rn1A + invIB * rn1B * rn1B;
                    float k22 = invMassA + invMassB + invIA * rn2A * rn2A + invIB * rn2B * rn2B;
                    float k12 = invMassA + invMassB + invIA * rn1A * rn2A + invIB * rn1B * rn2B;

                    // Ensure a reasonable condition number.
                    const float k_maxConditionNumber = 100.0f;
                    if (k11 * k11 < k_maxConditionNumber * (k11 * k22 - k12 * k12))
                    {
                        // K is safe to invert.
                        cc.K.Col1.X = k11;
                        cc.K.Col1.Y = k12;
                        cc.K.Col2.X = k12;
                        cc.K.Col2.Y = k22;

                        float a = cc.K.Col1.X, b = cc.K.Col2.X, c = cc.K.Col1.Y, d = cc.K.Col2.Y;
                        float det = a * d - b * c;
                        if (det != 0.0f)
                        {
                            det = 1.0f / det;
                        }

                        cc.NormalMass.Col1.X = det * d;
                        cc.NormalMass.Col1.Y = -det * c;
                        cc.NormalMass.Col2.X = -det * b;
                        cc.NormalMass.Col2.Y = det * a;
                    }
                    else
                    {
                        // The constraints are redundant, just use one.
                        // TODO_ERIN use deepest?
                        cc.PointCount = 1;
                    }
                }
            }
        }

        public void WarmStart()
        {
            // Warm start.
            for (int i = 0; i < _constraintCount; ++i)
            {
                ContactConstraint c = Constraints[i];

                float tangentx = c.Normal.Y;
                float tangenty = -c.Normal.X;

                for (int j = 0; j < c.PointCount; ++j)
                {
                    ContactConstraintPoint ccp = c.Points[j];
                    float px = ccp.NormalImpulse * c.Normal.X + ccp.TangentImpulse * tangentx;
                    float py = ccp.NormalImpulse * c.Normal.Y + ccp.TangentImpulse * tangenty;
                    c.BodyA.AngularVelocityInternal -= c.BodyA.InvI * (ccp.rA.X * py - ccp.rA.Y * px);
                    c.BodyA.LinearVelocityInternal.X -= c.BodyA.InvMass * px;
                    c.BodyA.LinearVelocityInternal.Y -= c.BodyA.InvMass * py;
                    c.BodyB.AngularVelocityInternal += c.BodyB.InvI * (ccp.rB.X * py - ccp.rB.Y * px);
                    c.BodyB.LinearVelocityInternal.X += c.BodyB.InvMass * px;
                    c.BodyB.LinearVelocityInternal.Y += c.BodyB.InvMass * py;
                }
            }
        }

        public void SolveVelocityConstraints()
        {
            for (int i = 0; i < _constraintCount; ++i)
            {
                ContactConstraint c = Constraints[i];
                float wA = c.BodyA.AngularVelocityInternal;
                float wB = c.BodyB.AngularVelocityInternal;

                float tangentx = c.Normal.Y;
                float tangenty = -c.Normal.X;

                float friction = c.Friction;

                Debug.Assert(c.PointCount == 1 || c.PointCount == 2);

                // Solve tangent constraints
                for (int j = 0; j < c.PointCount; ++j)
                {
                    ContactConstraintPoint ccp = c.Points[j];
                    float lambda = ccp.TangentMass *
                                   -((c.BodyB.LinearVelocityInternal.X + (-wB * ccp.rB.Y) -
                                      c.BodyA.LinearVelocityInternal.X - (-wA * ccp.rA.Y)) * tangentx +
                                     (c.BodyB.LinearVelocityInternal.Y + (wB * ccp.rB.X) -
                                      c.BodyA.LinearVelocityInternal.Y - (wA * ccp.rA.X)) * tangenty);

                    // MathUtils.Clamp the accumulated force
                    float maxFriction = friction * ccp.NormalImpulse;
                    float newImpulse = Math.Max(-maxFriction, Math.Min(ccp.TangentImpulse + lambda, maxFriction));
                    lambda = newImpulse - ccp.TangentImpulse;

                    // Apply contact impulse
                    float px = lambda * tangentx;
                    float py = lambda * tangenty;

                    c.BodyA.LinearVelocityInternal.X -= c.BodyA.InvMass * px;
                    c.BodyA.LinearVelocityInternal.Y -= c.BodyA.InvMass * py;
                    wA -= c.BodyA.InvI * (ccp.rA.X * py - ccp.rA.Y * px);

                    c.BodyB.LinearVelocityInternal.X += c.BodyB.InvMass * px;
                    c.BodyB.LinearVelocityInternal.Y += c.BodyB.InvMass * py;
                    wB += c.BodyB.InvI * (ccp.rB.X * py - ccp.rB.Y * px);

                    ccp.TangentImpulse = newImpulse;
                }

                // Solve normal constraints
                if (c.PointCount == 1)
                {
                    ContactConstraintPoint ccp = c.Points[0];

                    // Relative velocity at contact
                    // Compute normal impulse
                    float lambda = -ccp.NormalMass *
                                   ((c.BodyB.LinearVelocityInternal.X + (-wB * ccp.rB.Y) -
                                     c.BodyA.LinearVelocityInternal.X - (-wA * ccp.rA.Y)) * c.Normal.X +
                                    (c.BodyB.LinearVelocityInternal.Y + (wB * ccp.rB.X) -
                                     c.BodyA.LinearVelocityInternal.Y -
                                     (wA * ccp.rA.X)) * c.Normal.Y - ccp.VelocityBias);

                    // Clamp the accumulated impulse
                    float newImpulse = Math.Max(ccp.NormalImpulse + lambda, 0.0f);
                    lambda = newImpulse - ccp.NormalImpulse;

                    // Apply contact impulse
                    float px = lambda * c.Normal.X;
                    float py = lambda * c.Normal.Y;

                    c.BodyA.LinearVelocityInternal.X -= c.BodyA.InvMass * px;
                    c.BodyA.LinearVelocityInternal.Y -= c.BodyA.InvMass * py;
                    wA -= c.BodyA.InvI * (ccp.rA.X * py - ccp.rA.Y * px);

                    c.BodyB.LinearVelocityInternal.X += c.BodyB.InvMass * px;
                    c.BodyB.LinearVelocityInternal.Y += c.BodyB.InvMass * py;
                    wB += c.BodyB.InvI * (ccp.rB.X * py - ccp.rB.Y * px);

                    ccp.NormalImpulse = newImpulse;
                }
                else
                {
                    // Block solver developed in collaboration with Dirk Gregorius (back in 01/07 on Box2D_Lite).
                    // Build the mini LCP for this contact patch
                    //
                    // vn = A * x + b, vn >= 0, , vn >= 0, x >= 0 and vn_i * x_i = 0 with i = 1..2
                    //
                    // A = J * W * JT and J = ( -n, -r1 x n, n, r2 x n )
                    // b = vn_0 - velocityBias
                    //
                    // The system is solved using the "Total enumeration method" (s. Murty). The complementary constraint vn_i * x_i
                    // implies that we must have in any solution either vn_i = 0 or x_i = 0. So for the 2D contact problem the cases
                    // vn1 = 0 and vn2 = 0, x1 = 0 and x2 = 0, x1 = 0 and vn2 = 0, x2 = 0 and vn1 = 0 need to be tested. The first valid
                    // solution that satisfies the problem is chosen.
                    // 
                    // In order to account of the accumulated impulse 'a' (because of the iterative nature of the solver which only requires
                    // that the accumulated impulse is clamped and not the incremental impulse) we change the impulse variable (x_i).
                    //
                    // Substitute:
                    // 
                    // x = x' - a
                    // 
                    // Plug into above equation:
                    //
                    // vn = A * x + b
                    //    = A * (x' - a) + b
                    //    = A * x' + b - A * a
                    //    = A * x' + b'
                    // b' = b - A * a;

                    ContactConstraintPoint cp1 = c.Points[0];
                    ContactConstraintPoint cp2 = c.Points[1];

                    float ax = cp1.NormalImpulse;
                    float ay = cp2.NormalImpulse;
                    Debug.Assert(ax >= 0.0f && ay >= 0.0f);

                    // Relative velocity at contact
                    // Compute normal velocity
                    float vn1 = (c.BodyB.LinearVelocityInternal.X + (-wB * cp1.rB.Y) - c.BodyA.LinearVelocityInternal.X -
                                 (-wA * cp1.rA.Y)) * c.Normal.X +
                                (c.BodyB.LinearVelocityInternal.Y + (wB * cp1.rB.X) - c.BodyA.LinearVelocityInternal.Y -
                                 (wA * cp1.rA.X)) * c.Normal.Y;
                    float vn2 = (c.BodyB.LinearVelocityInternal.X + (-wB * cp2.rB.Y) - c.BodyA.LinearVelocityInternal.X -
                                 (-wA * cp2.rA.Y)) * c.Normal.X +
                                (c.BodyB.LinearVelocityInternal.Y + (wB * cp2.rB.X) - c.BodyA.LinearVelocityInternal.Y -
                                 (wA * cp2.rA.X)) * c.Normal.Y;

                    float bx = vn1 - cp1.VelocityBias - (c.K.Col1.X * ax + c.K.Col2.X * ay);
                    float by = vn2 - cp2.VelocityBias - (c.K.Col1.Y * ax + c.K.Col2.Y * ay);

                    float xx = -(c.NormalMass.Col1.X * bx + c.NormalMass.Col2.X * by);
                    float xy = -(c.NormalMass.Col1.Y * bx + c.NormalMass.Col2.Y * by);

                    while (true)
                    {
                        //
                        // Case 1: vn = 0
                        //
                        // 0 = A * x' + b'
                        //
                        // Solve for x':
                        //
                        // x' = - inv(A) * b'
                        //
                        if (xx >= 0.0f && xy >= 0.0f)
                        {
                            // Resubstitute for the incremental impulse
                            float dx = xx - ax;
                            float dy = xy - ay;

                            // Apply incremental impulse
                            float p1x = dx * c.Normal.X;
                            float p1y = dx * c.Normal.Y;

                            float p2x = dy * c.Normal.X;
                            float p2y = dy * c.Normal.Y;

                            float p12x = p1x + p2x;
                            float p12y = p1y + p2y;

                            c.BodyA.LinearVelocityInternal.X -= c.BodyA.InvMass * p12x;
                            c.BodyA.LinearVelocityInternal.Y -= c.BodyA.InvMass * p12y;
                            wA -= c.BodyA.InvI * ((cp1.rA.X * p1y - cp1.rA.Y * p1x) + (cp2.rA.X * p2y - cp2.rA.Y * p2x));

                            c.BodyB.LinearVelocityInternal.X += c.BodyB.InvMass * p12x;
                            c.BodyB.LinearVelocityInternal.Y += c.BodyB.InvMass * p12y;
                            wB += c.BodyB.InvI * ((cp1.rB.X * p1y - cp1.rB.Y * p1x) + (cp2.rB.X * p2y - cp2.rB.Y * p2x));

                            // Accumulate
                            cp1.NormalImpulse = xx;
                            cp2.NormalImpulse = xy;

                            break;
                        }

                        //
                        // Case 2: vn1 = 0 and x2 = 0
                        //
                        //   0 = a11 * x1' + a12 * 0 + b1' 
                        // vn2 = a21 * x1' + a22 * 0 + b2'
                        //
                        xx = -cp1.NormalMass * bx;
                        xy = 0.0f;
                        vn1 = 0.0f;
                        vn2 = c.K.Col1.Y * xx + by;

                        if (xx >= 0.0f && vn2 >= 0.0f)
                        {
                            // Resubstitute for the incremental impulse
                            float dx = xx - ax;
                            float dy = xy - ay;

                            // Apply incremental impulse
                            float p1x = dx * c.Normal.X;
                            float p1y = dx * c.Normal.Y;

                            float p2x = dy * c.Normal.X;
                            float p2y = dy * c.Normal.Y;

                            float p12x = p1x + p2x;
                            float p12y = p1y + p2y;

                            c.BodyA.LinearVelocityInternal.X -= c.BodyA.InvMass * p12x;
                            c.BodyA.LinearVelocityInternal.Y -= c.BodyA.InvMass * p12y;
                            wA -= c.BodyA.InvI * ((cp1.rA.X * p1y - cp1.rA.Y * p1x) + (cp2.rA.X * p2y - cp2.rA.Y * p2x));

                            c.BodyB.LinearVelocityInternal.X += c.BodyB.InvMass * p12x;
                            c.BodyB.LinearVelocityInternal.Y += c.BodyB.InvMass * p12y;
                            wB += c.BodyB.InvI * ((cp1.rB.X * p1y - cp1.rB.Y * p1x) + (cp2.rB.X * p2y - cp2.rB.Y * p2x));

                            // Accumulate
                            cp1.NormalImpulse = xx;
                            cp2.NormalImpulse = xy;

                            break;
                        }


                        //
                        // Case 3: vn2 = 0 and x1 = 0
                        //
                        // vn1 = a11 * 0 + a12 * x2' + b1' 
                        //   0 = a21 * 0 + a22 * x2' + b2'
                        //
                        xx = 0.0f;
                        xy = -cp2.NormalMass * by;
                        vn1 = c.K.Col2.X * xy + bx;
                        vn2 = 0.0f;

                        if (xy >= 0.0f && vn1 >= 0.0f)
                        {
                            // Resubstitute for the incremental impulse
                            float dx = xx - ax;
                            float dy = xy - ay;

                            // Apply incremental impulse
                            float p1x = dx * c.Normal.X;
                            float p1y = dx * c.Normal.Y;

                            float p2x = dy * c.Normal.X;
                            float p2y = dy * c.Normal.Y;

                            float p12x = p1x + p2x;
                            float p12y = p1y + p2y;

                            c.BodyA.LinearVelocityInternal.X -= c.BodyA.InvMass * p12x;
                            c.BodyA.LinearVelocityInternal.Y -= c.BodyA.InvMass * p12y;
                            wA -= c.BodyA.InvI * ((cp1.rA.X * p1y - cp1.rA.Y * p1x) + (cp2.rA.X * p2y - cp2.rA.Y * p2x));

                            c.BodyB.LinearVelocityInternal.X += c.BodyB.InvMass * p12x;
                            c.BodyB.LinearVelocityInternal.Y += c.BodyB.InvMass * p12y;
                            wB += c.BodyB.InvI * ((cp1.rB.X * p1y - cp1.rB.Y * p1x) + (cp2.rB.X * p2y - cp2.rB.Y * p2x));

                            // Accumulate
                            cp1.NormalImpulse = xx;
                            cp2.NormalImpulse = xy;

                            break;
                        }

                        //
                        // Case 4: x1 = 0 and x2 = 0
                        // 
                        // vn1 = b1
                        // vn2 = b2;
                        xx = 0.0f;
                        xy = 0.0f;
                        vn1 = bx;
                        vn2 = by;

                        if (vn1 >= 0.0f && vn2 >= 0.0f)
                        {
                            // Resubstitute for the incremental impulse
                            float dx = xx - ax;
                            float dy = xy - ay;

                            // Apply incremental impulse
                            float p1x = dx * c.Normal.X;
                            float p1y = dx * c.Normal.Y;

                            float p2x = dy * c.Normal.X;
                            float p2y = dy * c.Normal.Y;

                            float p12x = p1x + p2x;
                            float p12y = p1y + p2y;

                            c.BodyA.LinearVelocityInternal.X -= c.BodyA.InvMass * p12x;
                            c.BodyA.LinearVelocityInternal.Y -= c.BodyA.InvMass * p12y;
                            wA -= c.BodyA.InvI * ((cp1.rA.X * p1y - cp1.rA.Y * p1x) + (cp2.rA.X * p2y - cp2.rA.Y * p2x));

                            c.BodyB.LinearVelocityInternal.X += c.BodyB.InvMass * p12x;
                            c.BodyB.LinearVelocityInternal.Y += c.BodyB.InvMass * p12y;
                            wB += c.BodyB.InvI * ((cp1.rB.X * p1y - cp1.rB.Y * p1x) + (cp2.rB.X * p2y - cp2.rB.Y * p2x));

                            // Accumulate
                            cp1.NormalImpulse = xx;
                            cp2.NormalImpulse = xy;

                            break;
                        }

                        // No solution, give up. This is hit sometimes, but it doesn't seem to matter.
                        break;
                    }
                }

                c.BodyA.AngularVelocityInternal = wA;
                c.BodyB.AngularVelocityInternal = wB;
            }
        }

        public void StoreImpulses()
        {
            for (int i = 0; i < _constraintCount; ++i)
            {
                ContactConstraint c = Constraints[i];
                Manifold m = c.Manifold;

                for (int j = 0; j < c.PointCount; ++j)
                {
                    ManifoldPoint pj = m.Points[j];
                    ContactConstraintPoint cp = c.Points[j];

                    pj.NormalImpulse = cp.NormalImpulse;
                    pj.TangentImpulse = cp.TangentImpulse;

                    m.Points[j] = pj;
                }

                c.Manifold = m;
                _contacts[i].Manifold = m;
            }
        }

        public bool SolvePositionConstraints(float baumgarte)
        {
            float minSeparation = 0.0f;

            for (int i = 0; i < _constraintCount; ++i)
            {
                ContactConstraint c = Constraints[i];

                Body bodyA = c.BodyA;
                Body bodyB = c.BodyB;

                float invMassA = bodyA.Mass * bodyA.InvMass;
                float invIA = bodyA.Mass * bodyA.InvI;
                float invMassB = bodyB.Mass * bodyB.InvMass;
                float invIB = bodyB.Mass * bodyB.InvI;

                // Solve normal constraints
                for (int j = 0; j < c.PointCount; ++j)
                {
                    Vector2 normal;
                    Vector2 point;
                    float separation;

                    Solve(c, j, out normal, out point, out separation);

                    float rax = point.X - bodyA.Sweep.C.X;
                    float ray = point.Y - bodyA.Sweep.C.Y;

                    float rbx = point.X - bodyB.Sweep.C.X;
                    float rby = point.Y - bodyB.Sweep.C.Y;

                    // Track max constraint error.
                    minSeparation = Math.Min(minSeparation, separation);

                    // Prevent large corrections and allow slop.
                    float C = Math.Max(-Settings.MaxLinearCorrection,
                                       Math.Min(baumgarte * (separation + Settings.LinearSlop), 0.0f));

                    // Compute the effective mass.
                    float rnA = rax * normal.Y - ray * normal.X;
                    float rnB = rbx * normal.Y - rby * normal.X;
                    float K = invMassA + invMassB + invIA * rnA * rnA + invIB * rnB * rnB;

                    // Compute normal impulse
                    float impulse = K > 0.0f ? -C / K : 0.0f;

                    float px = impulse * normal.X;
                    float py = impulse * normal.Y;

                    bodyA.Sweep.C.X -= invMassA * px;
                    bodyA.Sweep.C.Y -= invMassA * py;
                    bodyA.Sweep.A -= invIA * (rax * py - ray * px);

                    bodyB.Sweep.C.X += invMassB * px;
                    bodyB.Sweep.C.Y += invMassB * py;
                    bodyB.Sweep.A += invIB * (rbx * py - rby * px);

                    bodyA.SynchronizeTransform();
                    bodyB.SynchronizeTransform();
                }
            }

            // We can't expect minSpeparation >= -Settings.b2_linearSlop because we don't
            // push the separation above -Settings.b2_linearSlop.
            return minSeparation >= -1.5f * Settings.LinearSlop;
        }

        private static void Solve(ContactConstraint cc, int index, out Vector2 normal, out Vector2 point,
                                  out float separation)
        {
            Debug.Assert(cc.PointCount > 0);

            normal = Vector2.Zero;

            switch (cc.Type)
            {
                case ManifoldType.Circles:
                    {
                        Vector2 pointA = cc.BodyA.GetWorldPoint(ref cc.LocalPoint);
                        Vector2 pointB = cc.BodyB.GetWorldPoint(ref cc.Points[0].LocalPoint);
                        float a = (pointA.X - pointB.X) * (pointA.X - pointB.X) +
                                  (pointA.Y - pointB.Y) * (pointA.Y - pointB.Y);
                        if (a > Settings.Epsilon * Settings.Epsilon)
                        {
                            Vector2 normalTmp = pointB - pointA;
                            float factor = 1f / (float)Math.Sqrt(normalTmp.X * normalTmp.X + normalTmp.Y * normalTmp.Y);
                            normal.X = normalTmp.X * factor;
                            normal.Y = normalTmp.Y * factor;
                        }
                        else
                        {
                            normal.X = 1;
                            normal.Y = 0;
                        }

                        point = 0.5f * (pointA + pointB);
                        separation = (pointB.X - pointA.X) * normal.X + (pointB.Y - pointA.Y) * normal.Y - cc.RadiusA -
                                     cc.RadiusB;
                    }
                    break;

                case ManifoldType.FaceA:
                    {
                        normal = cc.BodyA.GetWorldVector(ref cc.LocalNormal);
                        Vector2 planePoint = cc.BodyA.GetWorldPoint(ref cc.LocalPoint);
                        Vector2 clipPoint = cc.BodyB.GetWorldPoint(ref cc.Points[index].LocalPoint);
                        separation = (clipPoint.X - planePoint.X) * normal.X + (clipPoint.Y - planePoint.Y) * normal.Y -
                                     cc.RadiusA - cc.RadiusB;
                        point = clipPoint;
                    }
                    break;

                case ManifoldType.FaceB:
                    {
                        normal = cc.BodyB.GetWorldVector(ref cc.LocalNormal);
                        Vector2 planePoint = cc.BodyB.GetWorldPoint(ref cc.LocalPoint);

                        Vector2 clipPoint = cc.BodyA.GetWorldPoint(ref cc.Points[index].LocalPoint);
                        separation = (clipPoint.X - planePoint.X) * normal.X + (clipPoint.Y - planePoint.Y) * normal.Y -
                                     cc.RadiusA - cc.RadiusB;
                        point = clipPoint;

                        // Ensure normal points from A to B
                        normal = -normal;
                    }
                    break;
                default:
                    point = Vector2.Zero;
                    separation = 0.0f;
                    break;
            }
        }
    }
}