using System;
using System.Collections.Generic;
using System.Linq;

using Duality;
using Duality.Audio;
using Duality.Resources;
using Duality.Components.Physics;

namespace DualStickSpaceShooter
{
	public class DoorControl : Component, ICmpMessageListener, ICmpUpdatable, ICmpInitializable
	{
		private RigidBody			doorPanel		= null;
		private	float				openSpeed		= 0.0f;
		private	float				closeSpeed		= 0.0f;
		private	int					minTriggerCount	= 1;
		private	ContentRef<Sound>	moveSound		= null;
		[DontSerialize] private SoundInstance moveSoundInst	= null;
		[DontSerialize] private	int triggerCount = 0;

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
		public ContentRef<Sound> MoveSound
		{
			get { return this.moveSound; }
			set { this.moveSound = value; }
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
				if (this.moveSoundInst != null && this.moveSoundInst.Disposed) this.moveSoundInst = null;

				bool isPanelInTargetArea = false;
				if (joint.MotorSpeed > 0.0f)
					isPanelInTargetArea = -joint.JointTranslation >= joint.UpperLimit;
				else
					isPanelInTargetArea = -joint.JointTranslation <= joint.LowerLimit;

				if (joint.JointSpeed <= 0.1f)
				{
					if (isPanelInTargetArea)
					{
						joint.MotorEnabled = false;
						joint.MotorSpeed = 0.0f;
						this.doorPanel.BodyType = BodyType.Static;
						if (this.moveSoundInst != null)
						{
							this.moveSoundInst.FadeOut(0.5f);
							this.moveSoundInst = null;
						}
					}
				}
				else
				{
					if (this.moveSoundInst == null)
					{
						this.moveSoundInst = DualityApp.Sound.PlaySound3D(this.moveSound, this.doorPanel.GameObj, false);
						this.moveSoundInst.FadeIn(0.5f);
						this.moveSoundInst.Looped = true;
					}
					this.moveSoundInst.Volume = MathF.Abs(joint.JointSpeed) / MathF.Max(MathF.Abs(this.openSpeed), MathF.Abs(this.closeSpeed));
				}
			}
		}
		void ICmpInitializable.OnInit(Component.InitContext context) {}
		void ICmpInitializable.OnShutdown(Component.ShutdownContext context)
		{
			if (context == ShutdownContext.Deactivate)
			{
				if (this.moveSoundInst != null)
				{
					this.moveSoundInst.Dispose();
					this.moveSoundInst = null;
				}
			}
		}
	}
}
