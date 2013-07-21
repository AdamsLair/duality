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
using System.Collections.Generic;
using System.Diagnostics;
using FarseerPhysics.Collision;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using FarseerPhysics.Common.PhysicsLogic;
using FarseerPhysics.Controllers;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Dynamics.Joints;
using OpenTK;

namespace FarseerPhysics.Dynamics
{
    /// <summary>
    /// The body type.
    /// </summary>
    public enum BodyType
    {
        /// <summary>
        /// Zero velocity, may be manually moved. Note: even static bodies have mass.
        /// </summary>
        Static,
        /// <summary>
        /// Zero mass, non-zero velocity set by user, moved by solver
        /// </summary>
        Kinematic,
        /// <summary>
        /// Positive mass, non-zero velocity determined by forces, moved by solver
        /// </summary>
        Dynamic,
    }

    [Flags]
    public enum BodyFlags
    {
        None = 0,
        Island = (1 << 0),
        Awake = (1 << 1),
        AutoSleep = (1 << 2),
        Bullet = (1 << 3),
        FixedRotation = (1 << 4),
        Enabled = (1 << 5),
        IgnoreGravity = (1 << 6),
        IgnoreCCD = (1 << 7),
    }

    public class Body : IDisposable
    {
        private static int _bodyIdCounter;
        internal float AngularVelocityInternal;
        public int BodyId;
        public ControllerFilter ControllerFilter;
        internal BodyFlags Flags;
        internal Vector2 Force;
        internal float InvI;
        internal float InvMass;
        internal Vector2 LinearVelocityInternal;
        public PhysicsLogicFilter PhysicsLogicFilter;
        internal float SleepTime;
        internal Sweep Sweep; // the swept motion for CCD
        internal float Torque;
        internal World World;
        internal Transform Xf; // the body origin transform
        private float _angularDamping;
        private BodyType _bodyType;
        private float _inertia;
        private float _linearDamping;
        private float _mass;

        internal Body()
        {
            FixtureList = new List<Fixture>(32);
        }

        public Body(World world)
            : this(world, null)
        {
        }

        public Body(World world, object userData)
        {
            FixtureList = new List<Fixture>(32);
            BodyId = _bodyIdCounter++;

            World = world;
            UserData = userData;

            FixedRotation = false;
            IsBullet = false;
            SleepingAllowed = true;
            Awake = true;
            BodyType = BodyType.Static;
            Enabled = true;

            Xf.R.Set(0);

            world.AddBody(this);
        }

        /// <summary>
        /// Gets the total number revolutions the body has made.
        /// </summary>
        /// <value>The revolutions.</value>
        public float Revolutions
        {
            get { return Rotation / (float)Math.PI; }
        }

        /// <summary>
        /// Gets or sets the body type.
        /// </summary>
        /// <value>The type of body.</value>
        public BodyType BodyType
        {
            get { return _bodyType; }
            set
            {
                if (_bodyType == value)
                {
                    return;
                }

                _bodyType = value;

                ResetMassData();

                if (_bodyType == BodyType.Static)
                {
                    LinearVelocityInternal = Vector2.Zero;
                    AngularVelocityInternal = 0.0f;
                }

                Awake = true;

                Force = Vector2.Zero;
                Torque = 0.0f;

                // Since the body type changed, we need to flag contacts for filtering.
                for (int i = 0; i < FixtureList.Count; i++)
                {
                    Fixture f = FixtureList[i];
                    f.Refilter();
                }
            }
        }

        /// <summary>
        /// Get or sets the linear velocity of the center of mass.
        /// </summary>
        /// <value>The linear velocity.</value>
        public Vector2 LinearVelocity
        {
            set
            {
                Debug.Assert(!float.IsNaN(value.X) && !float.IsNaN(value.Y));

                if (_bodyType == BodyType.Static)
                    return;

                if (Vector2.Dot(value, value) > 0.0f)
                    Awake = true;

                LinearVelocityInternal = value;
            }
            get { return LinearVelocityInternal; }
        }

        /// <summary>
        /// Gets or sets the angular velocity. Radians/second.
        /// </summary>
        /// <value>The angular velocity.</value>
        public float AngularVelocity
        {
            set
            {
                Debug.Assert(!float.IsNaN(value));

                if (_bodyType == BodyType.Static)
                    return;

                if (value * value > 0.0f)
                    Awake = true;

                AngularVelocityInternal = value;
            }
            get { return AngularVelocityInternal; }
        }

        /// <summary>
        /// Gets or sets the linear damping.
        /// </summary>
        /// <value>The linear damping.</value>
        public float LinearDamping
        {
            get { return _linearDamping; }
            set
            {
                Debug.Assert(!float.IsNaN(value));

                _linearDamping = value;
            }
        }

        /// <summary>
        /// Gets or sets the angular damping.
        /// </summary>
        /// <value>The angular damping.</value>
        public float AngularDamping
        {
            get { return _angularDamping; }
            set
            {
                Debug.Assert(!float.IsNaN(value));

                _angularDamping = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this body should be included in the CCD solver.
        /// </summary>
        /// <value><c>true</c> if this instance is included in CCD; otherwise, <c>false</c>.</value>
        public bool IsBullet
        {
            set
            {
                if (value)
                {
                    Flags |= BodyFlags.Bullet;
                }
                else
                {
                    Flags &= ~BodyFlags.Bullet;
                }
            }
            get { return (Flags & BodyFlags.Bullet) == BodyFlags.Bullet; }
        }

        /// <summary>
        /// You can disable sleeping on this body. If you disable sleeping, the
        /// body will be woken.
        /// </summary>
        /// <value><c>true</c> if sleeping is allowed; otherwise, <c>false</c>.</value>
        public bool SleepingAllowed
        {
            set
            {
                if (value)
                {
                    Flags |= BodyFlags.AutoSleep;
                }
                else
                {
                    Flags &= ~BodyFlags.AutoSleep;
                    Awake = true;
                }
            }
            get { return (Flags & BodyFlags.AutoSleep) == BodyFlags.AutoSleep; }
        }

        /// <summary>
        /// Set the sleep state of the body. A sleeping body has very
        /// low CPU cost.
        /// </summary>
        /// <value><c>true</c> if awake; otherwise, <c>false</c>.</value>
        public bool Awake
        {
            set
            {
                if (value)
                {
                    if ((Flags & BodyFlags.Awake) == 0)
                    {
                        Flags |= BodyFlags.Awake;
                        SleepTime = 0.0f;
                    }
                }
                else
                {
                    Flags &= ~BodyFlags.Awake;
                    SleepTime = 0.0f;
                    LinearVelocityInternal = Vector2.Zero;
                    AngularVelocityInternal = 0.0f;
                    Force = Vector2.Zero;
                    Torque = 0.0f;
                }
            }
            get { return (Flags & BodyFlags.Awake) == BodyFlags.Awake; }
        }

        /// <summary>
        /// Set the active state of the body. An inactive body is not
        /// simulated and cannot be collided with or woken up.
        /// If you pass a flag of true, all fixtures will be added to the
        /// broad-phase.
        /// If you pass a flag of false, all fixtures will be removed from
        /// the broad-phase and all contacts will be destroyed.
        /// Fixtures and joints are otherwise unaffected. You may continue
        /// to create/destroy fixtures and joints on inactive bodies.
        /// Fixtures on an inactive body are implicitly inactive and will
        /// not participate in collisions, ray-casts, or queries.
        /// Joints connected to an inactive body are implicitly inactive.
        /// An inactive body is still owned by a b2World object and remains
        /// in the body list.
        /// </summary>
        /// <value><c>true</c> if active; otherwise, <c>false</c>.</value>
        public bool Enabled
        {
            set
            {
                if (value == Enabled)
                {
                    return;
                }

                if (value)
                {
                    Flags |= BodyFlags.Enabled;

                    // Create all proxies.
                    IBroadPhase broadPhase = World.ContactManager.BroadPhase;
                    for (int i = 0; i < FixtureList.Count; i++)
                    {
                        FixtureList[i].CreateProxies(broadPhase, ref Xf);
                    }

                    // Contacts are created the next time step.
                }
                else
                {
                    Flags &= ~BodyFlags.Enabled;

                    // Destroy all proxies.
                    IBroadPhase broadPhase = World.ContactManager.BroadPhase;

                    for (int i = 0; i < FixtureList.Count; i++)
                    {
                        FixtureList[i].DestroyProxies(broadPhase);
                    }

                    // Destroy the attached contacts.
                    ContactEdge ce = ContactList;
                    while (ce != null)
                    {
                        ContactEdge ce0 = ce;
                        ce = ce.Next;
                        World.ContactManager.Destroy(ce0.Contact);
                    }
                    ContactList = null;
                }
            }
            get { return (Flags & BodyFlags.Enabled) == BodyFlags.Enabled; }
        }

        /// <summary>
        /// Set this body to have fixed rotation. This causes the mass
        /// to be reset.
        /// </summary>
        /// <value><c>true</c> if it has fixed rotation; otherwise, <c>false</c>.</value>
        public bool FixedRotation
        {
            set
            {
                if (value)
                {
                    Flags |= BodyFlags.FixedRotation;
                }
                else
                {
                    Flags &= ~BodyFlags.FixedRotation;
                }

                ResetMassData();
            }
            get { return (Flags & BodyFlags.FixedRotation) == BodyFlags.FixedRotation; }
        }

        /// <summary>
        /// Gets all the fixtures attached to this body.
        /// </summary>
        /// <value>The fixture list.</value>
        public List<Fixture> FixtureList { get; internal set; }

        /// <summary>
        /// Get the list of all joints attached to this body.
        /// </summary>
        /// <value>The joint list.</value>
        public JointEdge JointList { get; internal set; }

        /// <summary>
        /// Get the list of all contacts attached to this body.
        /// Warning: this list changes during the time step and you may
        /// miss some collisions if you don't use ContactListener.
        /// </summary>
        /// <value>The contact list.</value>
        public ContactEdge ContactList { get; internal set; }

        /// <summary>
        /// Set the user data. Use this to store your application specific data.
        /// </summary>
        /// <value>The user data.</value>
        public object UserData { get; set; }

        /// <summary>
        /// Get the world body origin position.
        /// </summary>
        /// <returns>Return the world position of the body's origin.</returns>
        public Vector2 Position
        {
            get { return Xf.Position; }
            set
            {
                Debug.Assert(!float.IsNaN(value.X) && !float.IsNaN(value.Y));

                SetTransform(ref value, Rotation);
            }
        }

        /// <summary>
        /// Get the angle in radians.
        /// </summary>
        /// <returns>Return the current world rotation angle in radians.</returns>
        public float Rotation
        {
            get { return Sweep.A; }
            set
            {
                Debug.Assert(!float.IsNaN(value));

                SetTransform(ref Xf.Position, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this body is static.
        /// </summary>
        /// <value><c>true</c> if this instance is static; otherwise, <c>false</c>.</value>
        public bool IsStatic
        {
            get { return _bodyType == BodyType.Static; }
            set
            {
                if (value)
                    BodyType = BodyType.Static;
                else
                    BodyType = BodyType.Dynamic;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this body ignores gravity.
        /// </summary>
        /// <value><c>true</c> if  it ignores gravity; otherwise, <c>false</c>.</value>
        public bool IgnoreGravity
        {
            get { return (Flags & BodyFlags.IgnoreGravity) == BodyFlags.IgnoreGravity; }
            set
            {
                if (value)
                    Flags |= BodyFlags.IgnoreGravity;
                else
                    Flags &= ~BodyFlags.IgnoreGravity;
            }
        }

        /// <summary>
        /// Get the world position of the center of mass.
        /// </summary>
        /// <value>The world position.</value>
        public Vector2 WorldCenter
        {
            get { return Sweep.C; }
        }

        /// <summary>
        /// Get the local position of the center of mass.
        /// </summary>
        /// <value>The local position.</value>
        public Vector2 LocalCenter
        {
            get { return Sweep.LocalCenter; }
            set
            {
                if (_bodyType != BodyType.Dynamic)
                    return;

                // Move center of mass.
                Vector2 oldCenter = Sweep.C;
                Sweep.LocalCenter = value;
                Sweep.C0 = Sweep.C = MathUtils.Multiply(ref Xf, ref Sweep.LocalCenter);

                // Update center of mass velocity.
                Vector2 a = Sweep.C - oldCenter;
                LinearVelocityInternal += new Vector2(-AngularVelocityInternal * a.Y, AngularVelocityInternal * a.X);
            }
        }

        /// <summary>
        /// Gets or sets the mass. Usually in kilograms (kg).
        /// </summary>
        /// <value>The mass.</value>
        public float Mass
        {
            get { return _mass; }
            set
            {
                Debug.Assert(!float.IsNaN(value));

                if (_bodyType != BodyType.Dynamic)
                    return;

                _mass = value;

                if (_mass <= 0.0f)
                    _mass = 1.0f;

                InvMass = 1.0f / _mass;
            }
        }

        /// <summary>
        /// Get or set the rotational inertia of the body about the local origin. usually in kg-m^2.
        /// </summary>
        /// <value>The inertia.</value>
        public float Inertia
        {
            get { return _inertia + Mass * Vector2.Dot(Sweep.LocalCenter, Sweep.LocalCenter); }
            set
            {
                Debug.Assert(!float.IsNaN(value));

                if (_bodyType != BodyType.Dynamic)
                    return;

                if (value > 0.0f && (Flags & BodyFlags.FixedRotation) == 0)
                {
                    _inertia = value - Mass * Vector2.Dot(LocalCenter, LocalCenter);
                    Debug.Assert(_inertia > 0.0f);
                    InvI = 1.0f / _inertia;
                }
            }
        }

        public float Restitution
        {
            get
            {
                float res = 0;

                for (int i = 0; i < FixtureList.Count; i++)
                {
                    Fixture f = FixtureList[i];
                    res += f.Restitution;
                }

                return res / FixtureList.Count;
            }
            set
            {
                for (int i = 0; i < FixtureList.Count; i++)
                {
                    Fixture f = FixtureList[i];
                    f.Restitution = value;
                }
            }
        }

        public float Friction
        {
            get
            {
                float res = 0;

                for (int i = 0; i < FixtureList.Count; i++)
                {
                    Fixture f = FixtureList[i];
                    res += f.Friction;
                }

                return res / FixtureList.Count;
            }
            set
            {
                for (int i = 0; i < FixtureList.Count; i++)
                {
                    Fixture f = FixtureList[i];
                    f.Friction = value;
                }
            }
        }

        public Category CollisionCategories
        {
            set
            {
                for (int i = 0; i < FixtureList.Count; i++)
                {
                    Fixture f = FixtureList[i];
                    f.CollisionCategories = value;
                }
            }
        }

        public Category CollidesWith
        {
            set
            {
                for (int i = 0; i < FixtureList.Count; i++)
                {
                    Fixture f = FixtureList[i];
                    f.CollidesWith = value;
                }
            }
        }

        public short CollisionGroup
        {
            set
            {
                for (int i = 0; i < FixtureList.Count; i++)
                {
                    Fixture f = FixtureList[i];
                    f.CollisionGroup = value;
                }
            }
        }

        public bool IsSensor
        {
            set
            {
                for (int i = 0; i < FixtureList.Count; i++)
                {
                    Fixture f = FixtureList[i];
                    f.IsSensor = value;
                }
            }
        }

        public bool IgnoreCCD
        {
            get { return (Flags & BodyFlags.IgnoreCCD) == BodyFlags.IgnoreCCD; }
            set
            {
                if (value)
                    Flags |= BodyFlags.IgnoreCCD;
                else
                    Flags &= ~BodyFlags.IgnoreCCD;
            }
        }

        #region IDisposable Members

        public bool IsDisposed { get; set; }

        public void Dispose()
        {
            if (!IsDisposed)
            {

                World.RemoveBody(this);
                IsDisposed = true;
                GC.SuppressFinalize(this);
            }
        }

        #endregion

        /// <summary>
        /// Resets the dynamics of this body.
        /// Sets torque, force and linear/angular velocity to 0
        /// </summary>
        public void ResetDynamics()
        {
            Torque = 0;
            AngularVelocityInternal = 0;
            Force = Vector2.Zero;
            LinearVelocityInternal = Vector2.Zero;
        }

        /// <summary>
        /// Creates a fixture and attach it to this body.
        /// If the density is non-zero, this function automatically updates the mass of the body.
        /// Contacts are not created until the next time step.
        /// Warning: This function is locked during callbacks.
        /// </summary>
        /// <param name="shape">The shape.</param>
        /// <returns></returns>
        public Fixture CreateFixture(Shape shape)
        {
            return new Fixture(this, shape);
        }
        /// <summary>
        /// Creates a fixture and attach it to this body.
        /// If the density is non-zero, this function automatically updates the mass of the body.
        /// Contacts are not created until the next time step.
        /// Warning: This function is locked during callbacks.
        /// </summary>
        /// <param name="shape">The shape.</param>
        /// <param name="userData">Application specific data</param>
        /// <returns></returns>
        public Fixture CreateFixture(Shape shape, object userData)
        {
            return new Fixture(this, shape, userData);
        }
        /// <summary>
        /// Destroy a fixture. This removes the fixture from the broad-phase and
        /// destroys all contacts associated with this fixture. This will
        /// automatically adjust the mass of the body if the body is dynamic and the
        /// fixture has positive density.
        /// All fixtures attached to a body are implicitly destroyed when the body is destroyed.
        /// Warning: This function is locked during callbacks.
        /// </summary>
        /// <param name="fixture">The fixture to be removed.</param>
        public void DestroyFixture(Fixture fixture)
        {
            Debug.Assert(fixture.Body == this);

            // Remove the fixture from this body's singly linked list.
            Debug.Assert(FixtureList.Count > 0);

            // You tried to remove a fixture that not present in the fixturelist.
            Debug.Assert(FixtureList.Contains(fixture));

            // Destroy any contacts associated with the fixture.
            ContactEdge edge = ContactList;
            while (edge != null)
            {
                Contact c = edge.Contact;
                edge = edge.Next;

                Fixture fixtureA = c.FixtureA;
                Fixture fixtureB = c.FixtureB;

                if (fixture == fixtureA || fixture == fixtureB)
                {
                    // This destroys the contact and removes it from
                    // this body's contact list.
                    World.ContactManager.Destroy(c);
                }
            }

            if ((Flags & BodyFlags.Enabled) == BodyFlags.Enabled)
            {
                IBroadPhase broadPhase = World.ContactManager.BroadPhase;
                fixture.DestroyProxies(broadPhase);
            }

            FixtureList.Remove(fixture);
            fixture.Destroy();
            fixture.Body = null;

            ResetMassData();
        }

        /// <summary>
        /// Set the position of the body's origin and rotation.
        /// This breaks any contacts and wakes the other bodies.
        /// Manipulating a body's transform may cause non-physical behavior.
        /// </summary>
        /// <param name="position">The world position of the body's local origin.</param>
        /// <param name="rotation">The world rotation in radians.</param>
        public void SetTransform(ref Vector2 position, float rotation)
        {
            SetTransformIgnoreContacts(ref position, rotation);

            World.ContactManager.FindNewContacts();
        }
        /// <summary>
        /// Set the position of the body's origin and rotation.
        /// This breaks any contacts and wakes the other bodies.
        /// Manipulating a body's transform may cause non-physical behavior.
        /// </summary>
        /// <param name="position">The world position of the body's local origin.</param>
        /// <param name="rotation">The world rotation in radians.</param>
        public void SetTransform(Vector2 position, float rotation)
        {
            SetTransform(ref position, rotation);
        }
        /// <summary>
        /// For teleporting a body without considering new contacts immediately.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="angle">The angle.</param>
        public void SetTransformIgnoreContacts(ref Vector2 position, float angle)
        {
            Xf.R.Set(angle);
            Xf.Position = position;

            Sweep.C0 =
                Sweep.C =
                new Vector2(Xf.Position.X + Xf.R.Col1.X * Sweep.LocalCenter.X + Xf.R.Col2.X * Sweep.LocalCenter.Y,
                            Xf.Position.Y + Xf.R.Col1.Y * Sweep.LocalCenter.X + Xf.R.Col2.Y * Sweep.LocalCenter.Y);
            Sweep.A0 = Sweep.A = angle;

            IBroadPhase broadPhase = World.ContactManager.BroadPhase;
            for (int i = 0; i < FixtureList.Count; i++)
            {
                FixtureList[i].Synchronize(broadPhase, ref Xf, ref Xf);
            }
        }

        /// <summary>
        /// Get the body transform for the body's origin.
        /// </summary>
        /// <param name="transform">The transform of the body's origin.</param>
        public void GetTransform(out Transform transform)
        {
            transform = Xf;
        }

        /// <summary>
        /// Apply a force at a world point. If the force is not
        /// applied at the center of mass, it will generate a torque and
        /// affect the angular velocity. This wakes up the body.
        /// </summary>
        /// <param name="force">The world force vector, usually in Newtons (N).</param>
        /// <param name="point">The world position of the point of application.</param>
        public void ApplyForce(Vector2 force, Vector2 point)
        {
            ApplyForce(ref force, ref point);
        }
        /// <summary>
        /// Applies a force at the center of mass.
        /// </summary>
        /// <param name="force">The force.</param>
        public void ApplyForce(ref Vector2 force)
        {
            ApplyForce(ref force, ref Xf.Position);
        }
        /// <summary>
        /// Applies a force at the center of mass.
        /// </summary>
        /// <param name="force">The force.</param>
        public void ApplyForce(Vector2 force)
        {
            ApplyForce(ref force, ref Xf.Position);
        }
        /// <summary>
        /// Apply a force at a world point. If the force is not
        /// applied at the center of mass, it will generate a torque and
        /// affect the angular velocity. This wakes up the body.
        /// </summary>
        /// <param name="force">The world force vector, usually in Newtons (N).</param>
        /// <param name="point">The world position of the point of application.</param>
        public void ApplyForce(ref Vector2 force, ref Vector2 point)
        {
            Debug.Assert(!float.IsNaN(force.X));
            Debug.Assert(!float.IsNaN(force.Y));
            Debug.Assert(!float.IsNaN(point.X));
            Debug.Assert(!float.IsNaN(point.Y));

            if (_bodyType == BodyType.Dynamic)
            {
                if (Awake == false)
                {
                    Awake = true;
                }

                Force += force;
                Torque += (point.X - Sweep.C.X) * force.Y - (point.Y - Sweep.C.Y) * force.X;
            }
        }
        /// <summary>
        /// Apply a torque. This affects the angular velocity
        /// without affecting the linear velocity of the center of mass.
        /// This wakes up the body.
        /// </summary>
        /// <param name="torque">The torque about the z-axis (out of the screen), usually in N-m.</param>
        public void ApplyTorque(float torque)
        {
            Debug.Assert(!float.IsNaN(torque));

            if (_bodyType == BodyType.Dynamic)
            {
                if (Awake == false)
                {
                    Awake = true;
                }

                Torque += torque;
            }
        }

        /// <summary>
        /// Apply an impulse at a point. This immediately modifies the velocity.
        /// This wakes up the body.
        /// </summary>
        /// <param name="impulse">The world impulse vector, usually in N-seconds or kg-m/s.</param>
        public void ApplyLinearImpulse(Vector2 impulse)
        {
            ApplyLinearImpulse(ref impulse);
        }
        /// <summary>
        /// Apply an impulse at a point. This immediately modifies the velocity.
        /// It also modifies the angular velocity if the point of application
        /// is not at the center of mass.
        /// This wakes up the body.
        /// </summary>
        /// <param name="impulse">The world impulse vector, usually in N-seconds or kg-m/s.</param>
        /// <param name="point">The world position of the point of application.</param>
        public void ApplyLinearImpulse(Vector2 impulse, Vector2 point)
        {
            ApplyLinearImpulse(ref impulse, ref point);
        }
        /// <summary>
        /// Apply an impulse at a point. This immediately modifies the velocity.
        /// This wakes up the body.
        /// </summary>
        /// <param name="impulse">The world impulse vector, usually in N-seconds or kg-m/s.</param>
        public void ApplyLinearImpulse(ref Vector2 impulse)
        {
            if (_bodyType != BodyType.Dynamic)
            {
                return;
            }
            if (Awake == false)
            {
                Awake = true;
            }
            LinearVelocityInternal += InvMass * impulse;
        }
        /// <summary>
        /// Apply an impulse at a point. This immediately modifies the velocity.
        /// It also modifies the angular velocity if the point of application
        /// is not at the center of mass.
        /// This wakes up the body.
        /// </summary>
        /// <param name="impulse">The world impulse vector, usually in N-seconds or kg-m/s.</param>
        /// <param name="point">The world position of the point of application.</param>
        public void ApplyLinearImpulse(ref Vector2 impulse, ref Vector2 point)
        {
            if (_bodyType != BodyType.Dynamic)
                return;

            if (Awake == false)
                Awake = true;

            LinearVelocityInternal += InvMass * impulse;
            AngularVelocityInternal += InvI * ((point.X - Sweep.C.X) * impulse.Y - (point.Y - Sweep.C.Y) * impulse.X);
        }
        /// <summary>
        /// Apply an angular impulse.
        /// </summary>
        /// <param name="impulse">The angular impulse in units of kg*m*m/s.</param>
        public void ApplyAngularImpulse(float impulse)
        {
            if (_bodyType != BodyType.Dynamic)
            {
                return;
            }

            if (Awake == false)
            {
                Awake = true;
            }

            AngularVelocityInternal += InvI * impulse;
        }

        /// <summary>
        /// This resets the mass properties to the sum of the mass properties of the fixtures.
        /// This normally does not need to be called unless you called SetMassData to override
        /// the mass and you later want to reset the mass.
        /// </summary>
        public void ResetMassData()
        {
            // Compute mass data from shapes. Each shape has its own density.
            _mass = 0.0f;
            InvMass = 0.0f;
            _inertia = 0.0f;
            InvI = 0.0f;
            Sweep.LocalCenter = Vector2.Zero;

            // Kinematic bodies have zero mass.
            if (BodyType == BodyType.Kinematic)
            {
                Sweep.C0 = Sweep.C = Xf.Position;
                return;
            }

            Debug.Assert(BodyType == BodyType.Dynamic || BodyType == BodyType.Static);

            // Accumulate mass over all fixtures.
            Vector2 center = Vector2.Zero;
            foreach (Fixture f in FixtureList)
            {
                if (f.Shape._density == 0)
                {
                    continue;
                }

                MassData massData = f.Shape.MassData;
                _mass += massData.Mass;
                center += massData.Mass * massData.Centroid;
                _inertia += massData.Inertia;
            }

            //Static bodies only have mass, they don't have other properties. A little hacky tho...
            if (BodyType == BodyType.Static)
            {
                Sweep.C0 = Sweep.C = Xf.Position;
                return;
            }

            // Compute center of mass.
            if (_mass > 0.0f)
            {
                InvMass = 1.0f / _mass;
                center *= InvMass;
            }
            else
            {
                // Force all dynamic bodies to have a positive mass.
                _mass = 1.0f;
                InvMass = 1.0f;
            }

            if (_inertia > 0.0f && (Flags & BodyFlags.FixedRotation) == 0)
            {
                // Center the inertia about the center of mass.
                _inertia -= _mass * Vector2.Dot(center, center);

                Debug.Assert(_inertia > 0.0f);
                InvI = 1.0f / _inertia;
            }
            else
            {
                _inertia = 0.0f;
                InvI = 0.0f;
            }

            // Move center of mass.
            Vector2 oldCenter = Sweep.C;
            Sweep.LocalCenter = center;
            Sweep.C0 = Sweep.C = MathUtils.Multiply(ref Xf, ref Sweep.LocalCenter);

            // Update center of mass velocity.
            Vector2 a = Sweep.C - oldCenter;
            LinearVelocityInternal += new Vector2(-AngularVelocityInternal * a.Y, AngularVelocityInternal * a.X);
        }

        /// <summary>
        /// Get the world coordinates of a point given the local coordinates.
        /// </summary>
        /// <param name="localPoint">A point on the body measured relative the the body's origin.</param>
        /// <returns>The same point expressed in world coordinates.</returns>
        public Vector2 GetWorldPoint(ref Vector2 localPoint)
        {
            return new Vector2(Xf.Position.X + Xf.R.Col1.X * localPoint.X + Xf.R.Col2.X * localPoint.Y,
                               Xf.Position.Y + Xf.R.Col1.Y * localPoint.X + Xf.R.Col2.Y * localPoint.Y);
        }
        /// <summary>
        /// Get the world coordinates of a point given the local coordinates.
        /// </summary>
        /// <param name="localPoint">A point on the body measured relative the the body's origin.</param>
        /// <returns>The same point expressed in world coordinates.</returns>
        public Vector2 GetWorldPoint(Vector2 localPoint)
        {
            return GetWorldPoint(ref localPoint);
        }

        /// <summary>
        /// Get the world coordinates of a vector given the local coordinates.
        /// Note that the vector only takes the rotation into account, not the position.
        /// </summary>
        /// <param name="localVector">A vector fixed in the body.</param>
        /// <returns>The same vector expressed in world coordinates.</returns>
        public Vector2 GetWorldVector(ref Vector2 localVector)
        {
            return new Vector2(Xf.R.Col1.X * localVector.X + Xf.R.Col2.X * localVector.Y,
                               Xf.R.Col1.Y * localVector.X + Xf.R.Col2.Y * localVector.Y);
        }
        /// <summary>
        /// Get the world coordinates of a vector given the local coordinates.
        /// </summary>
        /// <param name="localVector">A vector fixed in the body.</param>
        /// <returns>The same vector expressed in world coordinates.</returns>
        public Vector2 GetWorldVector(Vector2 localVector)
        {
            return GetWorldVector(ref localVector);
        }

        /// <summary>
        /// Gets a local point relative to the body's origin given a world point.
        /// Note that the vector only takes the rotation into account, not the position.
        /// </summary>
        /// <param name="worldPoint">A point in world coordinates.</param>
        /// <returns>The corresponding local point relative to the body's origin.</returns>
        public Vector2 GetLocalPoint(ref Vector2 worldPoint)
        {
            return
                new Vector2((worldPoint.X - Xf.Position.X) * Xf.R.Col1.X + (worldPoint.Y - Xf.Position.Y) * Xf.R.Col1.Y,
                            (worldPoint.X - Xf.Position.X) * Xf.R.Col2.X + (worldPoint.Y - Xf.Position.Y) * Xf.R.Col2.Y);
        }
        /// <summary>
        /// Gets a local point relative to the body's origin given a world point.
        /// </summary>
        /// <param name="worldPoint">A point in world coordinates.</param>
        /// <returns>The corresponding local point relative to the body's origin.</returns>
        public Vector2 GetLocalPoint(Vector2 worldPoint)
        {
            return GetLocalPoint(ref worldPoint);
        }
        /// <summary>
        /// Gets a local vector given a world vector.
        /// Note that the vector only takes the rotation into account, not the position.
        /// </summary>
        /// <param name="worldVector">A vector in world coordinates.</param>
        /// <returns>The corresponding local vector.</returns>
        public Vector2 GetLocalVector(ref Vector2 worldVector)
        {
            return new Vector2(worldVector.X * Xf.R.Col1.X + worldVector.Y * Xf.R.Col1.Y,
                               worldVector.X * Xf.R.Col2.X + worldVector.Y * Xf.R.Col2.Y);
        }
        /// <summary>
        /// Gets a local vector given a world vector.
        /// Note that the vector only takes the rotation into account, not the position.
        /// </summary>
        /// <param name="worldVector">A vector in world coordinates.</param>
        /// <returns>The corresponding local vector.</returns>
        public Vector2 GetLocalVector(Vector2 worldVector)
        {
            return GetLocalVector(ref worldVector);
        }

        /// <summary>
        /// Get the world linear velocity of a world point attached to this body.
        /// </summary>
        /// <param name="worldPoint">A point in world coordinates.</param>
        /// <returns>The world velocity of a point.</returns>
        public Vector2 GetLinearVelocityFromWorldPoint(Vector2 worldPoint)
        {
            return GetLinearVelocityFromWorldPoint(ref worldPoint);
        }
        /// <summary>
        /// Get the world linear velocity of a world point attached to this body.
        /// </summary>
        /// <param name="worldPoint">A point in world coordinates.</param>
        /// <returns>The world velocity of a point.</returns>
        public Vector2 GetLinearVelocityFromWorldPoint(ref Vector2 worldPoint)
        {
            return LinearVelocityInternal +
                   new Vector2(-AngularVelocityInternal * (worldPoint.Y - Sweep.C.Y),
                               AngularVelocityInternal * (worldPoint.X - Sweep.C.X));
        }
        /// <summary>
        /// Get the world velocity of a local point.
        /// </summary>
        /// <param name="localPoint">A point in local coordinates.</param>
        /// <returns>The world velocity of a point.</returns>
        public Vector2 GetLinearVelocityFromLocalPoint(Vector2 localPoint)
        {
            return GetLinearVelocityFromLocalPoint(ref localPoint);
        }
        /// <summary>
        /// Get the world velocity of a local point.
        /// </summary>
        /// <param name="localPoint">A point in local coordinates.</param>
        /// <returns>The world velocity of a point.</returns>
        public Vector2 GetLinearVelocityFromLocalPoint(ref Vector2 localPoint)
        {
            return GetLinearVelocityFromWorldPoint(GetWorldPoint(ref localPoint));
        }

        public Body DeepClone()
        {
            Body body = Clone();

            for (int i = 0; i < FixtureList.Count; i++)
            {
                FixtureList[i].Clone(body);
            }

            return body;
        }
        public Body Clone()
        {
            Body body = new Body();
            body.World = World;
            body.UserData = UserData;
            body.LinearDamping = LinearDamping;
            body.LinearVelocityInternal = LinearVelocityInternal;
            body.AngularDamping = AngularDamping;
            body.AngularVelocityInternal = AngularVelocityInternal;
            body.Position = Position;
            body.Rotation = Rotation;
            body._bodyType = _bodyType;
            body.Flags = Flags;

            World.AddBody(body);

            return body;
        }

        internal void SynchronizeFixtures()
        {
            Transform xf1 = new Transform();
            float c = (float)Math.Cos(Sweep.A0), s = (float)Math.Sin(Sweep.A0);
            xf1.R.Col1.X = c;
            xf1.R.Col2.X = -s;
            xf1.R.Col1.Y = s;
            xf1.R.Col2.Y = c;

            xf1.Position.X = Sweep.C0.X - (xf1.R.Col1.X * Sweep.LocalCenter.X + xf1.R.Col2.X * Sweep.LocalCenter.Y);
            xf1.Position.Y = Sweep.C0.Y - (xf1.R.Col1.Y * Sweep.LocalCenter.X + xf1.R.Col2.Y * Sweep.LocalCenter.Y);

            IBroadPhase broadPhase = World.ContactManager.BroadPhase;
            for (int i = 0; i < FixtureList.Count; i++)
            {
                FixtureList[i].Synchronize(broadPhase, ref xf1, ref Xf);
            }
        }
        internal void SynchronizeTransform()
        {
            Xf.R.Set(Sweep.A);

            float vx = Xf.R.Col1.X * Sweep.LocalCenter.X + Xf.R.Col2.X * Sweep.LocalCenter.Y;
            float vy = Xf.R.Col1.Y * Sweep.LocalCenter.X + Xf.R.Col2.Y * Sweep.LocalCenter.Y;

            Xf.Position.X = Sweep.C.X - vx;
            Xf.Position.Y = Sweep.C.Y - vy;
        }

        /// <summary>
        /// This is used to prevent connected bodies from colliding.
        /// It may lie, depending on the collideConnected flag.
        /// </summary>
        /// <param name="other">The other body.</param>
        /// <returns></returns>
        internal bool ShouldCollide(Body other)
        {
            // At least one body should be dynamic.
            if (_bodyType != BodyType.Dynamic && other._bodyType != BodyType.Dynamic)
            {
                return false;
            }

            // Does a joint prevent collision?
            for (JointEdge jn = JointList; jn != null; jn = jn.Next)
            {
                if (jn.Other == other)
                {
                    if (jn.Joint.CollideConnected == false)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
        internal void Advance(float alpha)
        {
            // Advance to the new safe time.
            Sweep.Advance(alpha);
            Sweep.C = Sweep.C0;
            Sweep.A = Sweep.A0;
            SynchronizeTransform();
        }

		internal bool OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
		{
			if (this.Collision != null)
				return this.Collision(fixtureA, fixtureB, contact);
			else
				return true;
		}
		internal void OnSeparation(Fixture fixtureA, Fixture fixtureB)
		{
			if (this.Separation != null)
				this.Separation(fixtureA, fixtureB);
		}
		internal void OnAfterCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
		{
			if (this.AfterCollision != null)
				this.AfterCollision(fixtureA, fixtureB, contact);
		}
		internal bool OnBeforeCollision(Fixture fixtureA, Fixture fixtureB)
		{
			if (this.BeforeCollision != null)
				return this.BeforeCollision(fixtureA, fixtureB);
			else 
				return true;
		}
		internal void OnPostSolve(Contact contact, ContactConstraint impulse)
		{
			if (this.PostSolve != null)
				this.PostSolve(contact, impulse);
		}

        public event OnCollisionEventHandler Collision;
        public event OnSeparationEventHandler Separation;
        public event AfterCollisionEventHandler AfterCollision;
        public event BeforeCollisionEventHandler BeforeCollision;
		public event PostSolveDelegate PostSolve;

        public void IgnoreCollisionWith(Body other)
        {
            for (int i = 0; i < FixtureList.Count; i++)
            {
                Fixture f = FixtureList[i];
                for (int j = 0; j < other.FixtureList.Count; j++)
                {
                    Fixture f2 = other.FixtureList[j];

                    f.IgnoreCollisionWith(f2);
                }
            }
        }
        public void RestoreCollisionWith(Body other)
        {
            for (int i = 0; i < FixtureList.Count; i++)
            {
                Fixture f = FixtureList[i];
                for (int j = 0; j < other.FixtureList.Count; j++)
                {
                    Fixture f2 = other.FixtureList[j];

                    f.RestoreCollisionWith(f2);
                }
            }
        }
    }
}