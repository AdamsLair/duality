using System;
using System.Collections.Generic;
using System.Linq;

using Duality;
using Duality.Resources;
using Duality.Components.Physics;

namespace DualStickSpaceShooter
{
	[Serializable]
	public class DoorControl : Component, ICmpMessageListener
	{
		private RigidBody	doorPanel	= null;
		private	float		openSpeed	= 0.0f;
		private	float		closeSpeed	= 0.0f;
		[NonSerialized] private	int triggerCount = 0;

		public float OpenSpeed
		{
			get { return this.openSpeed; }
			set { this.openSpeed = value; }
		}
		public float CloseSpeed
		{
			get { return this.closeSpeed; }
			set { this.closeSpeed = value; }
		}
		public RigidBody DoorPanel
		{
			get { return this.doorPanel; }
			set { this.doorPanel = value; }
		}
		private PrismaticJointInfo DoorJoint
		{
			get
			{
				if (this.doorPanel == null) return null;
				return this.doorPanel.Joints.OfType<PrismaticJointInfo>().FirstOrDefault();
			}
		}

		private void OpenDoor()
		{
			if (this.DoorJoint == null) return;
			this.DoorJoint.MotorSpeed = this.openSpeed;
		}
		private void CloseDoor()
		{
			if (this.DoorJoint == null) return;
			this.DoorJoint.MotorSpeed = this.closeSpeed;
		}

		private void AddSignal()
		{
			this.triggerCount++;
			if (this.triggerCount > 0) this.OpenDoor();
		}
		private void RemoveSignal()
		{
			this.triggerCount--;
			if (this.triggerCount == 0) this.CloseDoor();
		}
		void ICmpMessageListener.OnMessage(GameMessage msg)
		{
			if (msg is TriggerEnteredMessage)
			{
				this.AddSignal();
			}
			else if (msg is TriggerLeftMessage)
			{
				this.RemoveSignal();
			}
		}
	}
}
