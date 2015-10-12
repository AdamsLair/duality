using System;
using System.Collections.Generic;
using System.Linq;

using Duality;
using Duality.Input;
using Duality.Components.Physics;
using Duality.Resources;

namespace FlapOrDie.Controllers
{
    [RequiredComponent(typeof(RigidBody))]
    public class PlayerController : Component, ICmpUpdatable, ICmpCollisionListener
    {
        [DontSerialize]
        private RigidBody rigidBody;

        private float impulseStrength;

        public float ImpulseStrength
        {
            get { return this.impulseStrength; }
            set { this.impulseStrength = value; }
        }
        [DontSerialize]
        private ushort points;

        public ushort Points
        {
            get { return this.points; }
        }

        [DontSerialize]
        private bool isDead;

        public bool IsDead
        {
            get { return this.isDead; }
        }

        public void Reset()
        {
            this.points = 0;
            this.isDead = false;
            this.GameObj.Transform.Pos = Vector3.Zero;
        }

        void ICmpUpdatable.OnUpdate()
        {
            if (this.rigidBody == null)
            {
                this.rigidBody = this.GameObj.GetComponent<RigidBody>();
            }

            if (!this.isDead && DualityApp.Keyboard.KeyHit(Key.Space))
            {
                this.rigidBody.ApplyLocalImpulse(-Vector2.UnitY * this.impulseStrength);
            }

            if (DualityApp.Keyboard.KeyHit(Key.Escape))
            {
                Scene.SwitchTo(ContentProvider.RequestContent<Scene>(@"Data\MainMenu.Scene.res"));
            }
        }

        void ICmpCollisionListener.OnCollisionBegin(Component sender, CollisionEventArgs args)
        {
            // did the player hit a gate sensor? +1 point)
            if (!this.isDead && args.CollideWith.GetComponent<RigidBody>().CollisionCategory == CollisionCategory.Cat2)
            {
                // remove the sensor..
                args.CollideWith.RemoveComponent<RigidBody>();
                this.points++;
            }
            else
            {
                // otherwise, you're dead!
                this.isDead = true;
            }
        }

        void ICmpCollisionListener.OnCollisionEnd(Component sender, CollisionEventArgs args)
        {
            // nothing to do here
        }

        void ICmpCollisionListener.OnCollisionSolve(Component sender, CollisionEventArgs args)
        {
            // nothing to do here
        }
    }
}
