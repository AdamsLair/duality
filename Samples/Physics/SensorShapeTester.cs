using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Duality;
using Duality.Editor;
using Duality.Drawing;
using Duality.Components;
using Duality.Components.Renderers;
using Duality.Components.Physics;

namespace Duality.Samples.Physics
{
	[EditorHintCategory("PhysicsSample")]
    public class SensorShapeTester : Component, ICmpCollisionListener
	{
		private TextRenderer targetRenderer = null;
		private ColorRgba activeColor = ColorRgba.Red;

		[DontSerialize] private int triggerCounter = 0;
		[DontSerialize] private ColorRgba inactiveColor;


		public bool IsTriggered
		{
			get { return this.triggerCounter > 0; }
		}
		public TextRenderer TargetRenderer
		{
			get { return this.targetRenderer; }
			set { this.targetRenderer = value; }
		}
		public ColorRgba ActiveColor
		{
			get { return this.activeColor; }
			set { this.activeColor = value; }
		}


		private void OnTriggerActivated()
		{
			if (this.targetRenderer == null) return;
			this.inactiveColor = this.targetRenderer.ColorTint;
			this.targetRenderer.ColorTint = this.activeColor;
		}
		private void OnTriggerDeactivated()
		{
			if (this.targetRenderer == null) return;
			this.targetRenderer.ColorTint = this.inactiveColor;
		}

		void ICmpCollisionListener.OnCollisionBegin(Component sender, CollisionEventArgs args)
		{
			RigidBodyCollisionEventArgs bodyArgs = args as RigidBodyCollisionEventArgs;
			if (bodyArgs.MyShape.IsSensor)
			{
				this.triggerCounter++;
				if (this.triggerCounter == 1)
					this.OnTriggerActivated();
			}
		}
		void ICmpCollisionListener.OnCollisionEnd(Component sender, CollisionEventArgs args)
		{
			RigidBodyCollisionEventArgs bodyArgs = args as RigidBodyCollisionEventArgs;
			if (bodyArgs.MyShape.IsSensor)
			{
				this.triggerCounter--;
				if (this.triggerCounter == 0)
					this.OnTriggerDeactivated();
			}
		}
		void ICmpCollisionListener.OnCollisionSolve(Component sender, CollisionEventArgs args) { }
	}
}
