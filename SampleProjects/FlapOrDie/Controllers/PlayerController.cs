using System;
using System.Collections.Generic;
using System.Linq;

using Duality;
using Duality.Input;
using Duality.Components.Physics;
using Duality.Resources;
using Duality.Components.Renderers;

namespace FlapOrDie.Controllers
{
    [RequiredComponent(typeof(RigidBody))]
    public class PlayerController : Component, ICmpUpdatable, ICmpCollisionListener
    {
        [DontSerialize]
        private RigidBody rigidBody;

        private AnimSpriteRenderer bodyRenderer;

        private AnimSpriteRenderer frontWingRenderer;

        private AnimSpriteRenderer backWingRenderer;

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
        private float flapTime;

        [DontSerialize]
        private bool isDead;

        public bool IsDead
        {
            get { return this.isDead; }
        }

        public AnimSpriteRenderer Body
        {
            get { return this.bodyRenderer; }
            set { this.bodyRenderer = value; }
        }

        public AnimSpriteRenderer FrontWing
        {
            get { return this.frontWingRenderer; }
            set { this.frontWingRenderer = value; }
        }

        public AnimSpriteRenderer BackWing
        {
            get { return this.backWingRenderer; }
            set { this.backWingRenderer = value; }
        }

        public void Reset()
        {
            this.points = 0;
            this.isDead = false;
            this.GameObj.Transform.Pos = Vector3.Zero;
			this.GameObj.Transform.Angle = 0;
			this.GameObj.GetComponent<RigidBody>().LinearVelocity = Vector2.Zero;
			this.GameObj.GetComponent<RigidBody>().AngularVelocity = 0;
            
            Body.AnimFirstFrame = 1;
        }

        void ICmpUpdatable.OnUpdate()
        {
            float lastDelta = Time.MsPFMult * Time.TimeMult / 1000;
            if (this.rigidBody == null) this.rigidBody = this.GameObj.GetComponent<RigidBody>();

            if (!this.isDead)
            {
                if (DualityApp.Keyboard.KeyHit(Key.Space))
                {
                    this.rigidBody.ApplyLocalImpulse(-Vector2.UnitY * this.impulseStrength);
                    flapTime = (Time.MsPFMult / 1000 * 3);
                }
            }

            if(flapTime > 0)
            {
                FrontWing.GameObj.Transform.Angle = -MathF.PiOver4;
                BackWing.GameObj.Transform.Angle = MathF.PiOver4;
                flapTime -= lastDelta;
            }

            if(flapTime < 0)
            {
                FrontWing.GameObj.Transform.Angle = 0;
                BackWing.GameObj.Transform.Angle = 0;
                flapTime = 0;
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
                Body.AnimFirstFrame = 2;
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
