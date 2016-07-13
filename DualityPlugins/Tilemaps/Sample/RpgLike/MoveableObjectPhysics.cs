using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

using Duality;
using Duality.Components;
using Duality.Components.Physics;
using Duality.Editor;
using Duality.Plugins.Tilemaps;
using Duality.Plugins.Tilemaps.Properties;
using Duality.Plugins.Tilemaps.Sample.RpgLike.Properties;

namespace Duality.Plugins.Tilemaps.Sample.RpgLike
{
	/// <summary>
	/// Applies friction physics to an otherwise passive, moveable object.
	/// </summary>
	[RequiredComponent(typeof(RigidBody))]
	[EditorHintCategory(SampleResNames.CategoryRpgLike)]
	public class MoveableObjectPhysics : Component, ICmpUpdatable, ICmpInitializable
	{
		private RigidBody baseObject;
		private float baseFriction = 10.0f;
		private float initialFriction = 25.0f;

		[DontSerialize] private FrictionJointInfo frictionJoint;
		[DontSerialize] private float currentDynamicFriction;


		/// <summary>
		/// [GET / SET] The object on which this object is standing on, i.e. relative
		/// to which friction will be simulated.
		/// </summary>
		public RigidBody BaseObject
		{
			get { return this.baseObject; }
			set { this.baseObject = value; }
		}
		public float BaseFriction
		{
			get { return this.baseFriction; }
			set { this.baseFriction = value; }
		}
		public float InitialFriction
		{
			get { return this.initialFriction; }
			set { this.initialFriction = value; }
		}

		void ICmpUpdatable.OnUpdate()
		{
			RigidBody body = this.GameObj.GetComponent<RigidBody>();

			// Create a friction joint connecting our body to the body it is standing on
			this.SetupFrictionJoint(body);

			// The friction we're applying has two components: A basic friction
			// that is inherent to the object and will always be applied, and a
			// dynamic friction that is applied while the object is standing still,
			// but decreases once it's in motion. This is what we call "dynamic"
			// friction.
			float targetDynamicFriction = this.initialFriction / (1.0f + body.LinearVelocity.Length);
			// The dynamic friction can only be reduced slowly, but will increase sharply
			// when the body slows down again.
			float dynamicFrictionChangeRate = (targetDynamicFriction < this.currentDynamicFriction) ? 0.1f : 0.5f;
			// Gradually change the dynamic friction we will simulate.
			this.currentDynamicFriction += (targetDynamicFriction - this.currentDynamicFriction) * dynamicFrictionChangeRate * Time.TimeMult;

			// Update the friction joint's parameters.
			this.frictionJoint.MaxForce = 
				this.baseFriction + 
				this.currentDynamicFriction;
		}
		void ICmpInitializable.OnInit(Component.InitContext context) { }
		void ICmpInitializable.OnShutdown(Component.ShutdownContext context)
		{
			if (context == ShutdownContext.Deactivate)
			{
				// Clean up the temporary friction joint we have created
				if (this.frictionJoint != null)
				{
					this.frictionJoint.ParentBody.RemoveJoint(this.frictionJoint);
					this.frictionJoint = null;
				}
			}
		}

		private void SetupFrictionJoint(RigidBody body)
		{
			// If we already have a friction joint, is it connecting the right bodies?
			if (this.frictionJoint != null)
			{
				if (this.frictionJoint.ParentBody != body ||
					this.frictionJoint.ParentBody != this.baseObject)
				{
					this.frictionJoint.ParentBody.RemoveJoint(this.frictionJoint);
					this.frictionJoint = null;
				}
			}

			// Create a friction joint connecting our body to the body it is standing on
			if (this.frictionJoint == null)
			{
				this.frictionJoint = new FrictionJointInfo();
				this.frictionJoint.CollideConnected = true;
				this.frictionJoint.MaxTorque = 0.0f;
				body.AddJoint(this.frictionJoint, this.baseObject);
			}
		}
	}
}
