using System;
using System.Collections.Generic;
using System.Linq;

using Duality;
using Duality.Resources;
using Duality.Components.Physics;

namespace DualStickSpaceShooter
{
	[Serializable]
	public class DoorControl : Component, ICmpMessageListener, ICmpUpdatable
	{
		private RigidBody	doorPanel		= null;
		private	float		openSpeed		= 0.0f;
		private	float		closeSpeed		= 0.0f;
		private	int			minTriggerCount	= 1;
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
		public int MinTriggerCount
		{
			get { return this.minTriggerCount; }
			set { this.minTriggerCount = value; }
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
			PrismaticJointInfo joint = this.DoorJoint;
			if (joint == null) return;

			this.doorPanel.BodyType = BodyType.Dynamic;
			joint.MotorSpeed = this.openSpeed;
			joint.MotorEnabled = true;
		}
		private void CloseDoor()
		{
			PrismaticJointInfo joint = this.DoorJoint;
			if (joint == null) return;

			this.doorPanel.BodyType = BodyType.Dynamic;
			joint.MotorSpeed = this.closeSpeed;
			joint.MotorEnabled = true;
		}

		private void AddSignal()
		{
			this.triggerCount++;
			if (this.triggerCount >= this.minTriggerCount) this.OpenDoor();
		}
		private void RemoveSignal()
		{
			this.triggerCount--;
			if (this.triggerCount < this.minTriggerCount) this.CloseDoor();
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
		void ICmpUpdatable.OnUpdate()
		{
			PrismaticJointInfo joint = this.DoorJoint;
			if (joint == null) return;

			if (joint.MotorEnabled)
			{
				bool isPanelInTargetArea = false;
				if (joint.MotorSpeed > 0.0f)
					isPanelInTargetArea = -joint.JointTranslation >= joint.UpperLimit;
				else
					isPanelInTargetArea = -joint.JointTranslation <= joint.LowerLimit;

				if (isPanelInTargetArea && joint.JointSpeed <= 0.1f)
				{
					joint.MotorEnabled = false;
					joint.MotorSpeed = 0.0f;
					this.doorPanel.BodyType = BodyType.Static;
				}
			}
		}
	}
}
