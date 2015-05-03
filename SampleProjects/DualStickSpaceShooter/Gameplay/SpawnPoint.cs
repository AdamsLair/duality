using System;
using System.Collections.Generic;
using System.Linq;

using OpenTK;

using Duality;
using Duality.Components;
using Duality.Resources;
using Duality.Drawing;

namespace DualStickSpaceShooter
{
	[RequiredComponent(typeof(Transform))]
	public class SpawnPoint : Component, ICmpMessageListener, ICmpInitializable
	{
		private static int lastVisitedIndex = -1;
		public static int LastVisitedIndex
		{
			get { return lastVisitedIndex; }
			set { lastVisitedIndex = value; }
		}
		public static Vector3 SpawnPos
		{
			get
			{
				if (lastVisitedIndex == -1) return Vector3.Zero;

				SpawnPoint spawnPoint = Scene.Current.FindComponents<SpawnPoint>().FirstOrDefault(s => s.SpawnIndex == lastVisitedIndex);
				if (spawnPoint == null) return Vector3.Zero;

				Vector3 spawnPos = spawnPoint.GameObj.Transform.Pos;
				spawnPos = new Vector3(spawnPos.Xy);
				return spawnPos;
			}
		}

		private bool activated = false;
		private int index = 0;

		public int SpawnIndex
		{
			get { return this.index; }
			set { this.index = value; }
		}
		
		private void ChangeParticleEffect()
		{
			ParticleEffect effect = this.GameObj.GetComponent<ParticleEffect>();
			if (effect != null)
			{
				foreach (ParticleEmitter emitter in effect.Emitters)
				{
					emitter.MinColor = emitter.MinColor.WithValue(1.0f);
					emitter.MaxColor = emitter.MaxColor.WithValue(1.0f);
				}
			}
		}

		void ICmpMessageListener.OnMessage(GameMessage msg)
		{
			if (this.activated) return;

			TriggerEnteredMessage entered = msg as TriggerEnteredMessage;
			if (entered != null)
			{
				Ship ship = entered.GameObj.GetComponent<Ship>();
				if (ship != null && ship.Owner != null)
				{
					this.activated = true;
					lastVisitedIndex = this.index;

					this.ChangeParticleEffect();
				}
			}
		}
		void ICmpInitializable.OnInit(Component.InitContext context)
		{
			if (context == InitContext.Activate)
			{
				if (this.index == lastVisitedIndex || this.activated)
				{
					this.ChangeParticleEffect();
				}
			}
		}
		void ICmpInitializable.OnShutdown(Component.ShutdownContext context) {}
	}
}
