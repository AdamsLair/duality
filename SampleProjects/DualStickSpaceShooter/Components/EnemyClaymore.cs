using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;

using Duality;
using Duality.Components;
using Duality.Components.Physics;
using Duality.Components.Renderers;
using Duality.Resources;
using Duality.Drawing;

namespace DualStickSpaceShooter
{
	[Serializable]
	[RequiredComponent(typeof(Ship))]
	public class EnemyClaymore : Component, ICmpUpdatable, ICmpInitializable, ICmpCollisionListener
	{
		private enum MindState
		{
			Asleep,
			Awaking,
			FallingAsleep,
			Idle
		}

		private struct SpikeState
		{
			public float OpenValue;
			public float OpenTarget;
			public float Speed;
		}

		private	MindState		state			= MindState.Asleep;
		private	float			blinkTimer		= 0.0f;
		private	float			eyeOpenValue	= 0.0f;
		private	float			eyeOpenTarget	= 0.0f;
		private	float			eyeSpeed		= 0.0f;
		private	bool			eyeBlinking		= false;
		private	SpikeState[]	spikeState		= new SpikeState[4];

		[NonSerialized] private AnimSpriteRenderer	eye		= null;
		[NonSerialized] private SpriteRenderer[]	spikes	= null;
		
		private void Sleep()
		{
			if (this.state == MindState.Asleep) return;
			if (this.state == MindState.FallingAsleep) return;

			this.state = MindState.FallingAsleep;
			this.eyeOpenTarget = 0.0f;
			this.eyeBlinking = false;
			this.eyeSpeed = Time.SPFMult / MathF.Rnd.NextFloat(1.5f, 3.5f);
			this.DeactivateSpikes();
		}
		private void Awake()
		{
			if (this.state == MindState.Idle) return;
			if (this.state == MindState.Awaking) return;

			this.state = MindState.Awaking;
			this.eyeOpenTarget = 1.0f;
			this.eyeBlinking = false;
			this.eyeSpeed = Time.SPFMult / MathF.Rnd.NextFloat(1.0f, 2.0f);

			this.RandomizeBlinkTimer();
		}
		private void RandomizeBlinkTimer()
		{
			this.blinkTimer += MathF.Rnd.Next(100, 10000);
		}
		
		private void BlinkEye()
		{
			this.eyeBlinking = true;
			this.eyeOpenTarget = 0.0f;
			this.eyeSpeed = Time.SPFMult / MathF.Rnd.NextFloat(0.05f, 0.25f);
		}
		private void DeactivateSpikes()
		{
			for (int i = 0; i < this.spikeState.Length; i++)
			{
				this.spikeState[i].OpenTarget = 0.0f;
				this.spikeState[i].Speed = Time.SPFMult / MathF.Rnd.NextFloat(1.0f, 2.0f);
			}
		}
		private void ActivateSpikes()
		{
			for (int i = 0; i < this.spikeState.Length; i++)
			{
				this.spikeState[i].OpenTarget = 1.0f;
				this.spikeState[i].Speed = Time.SPFMult / MathF.Rnd.NextFloat(0.25f, 1.0f);
			}
		}

		void ICmpUpdatable.OnUpdate()
		{
			Transform transform = this.GameObj.Transform;
			RigidBody body = this.GameObj.RigidBody;
			Ship ship = this.GameObj.GetComponent<Ship>();

			// Udpate the eyes visual appearance
			this.eyeOpenValue = MathF.Clamp(this.eyeOpenValue + MathF.Sign(this.eyeOpenTarget - this.eyeOpenValue) * this.eyeSpeed * Time.TimeMult, 0.0f, 1.0f);
			if (this.eyeBlinking && this.eyeOpenValue == 0.0f) this.eyeOpenTarget = 1.0f;
			if (this.eye != null)
			{
				this.eye.AnimTime = this.eyeOpenValue;
			}

			// Update the spikes visual appearance
			for (int i = 0; i < this.spikeState.Length; i++)
			{
				float actualTarget = this.spikeState[i].OpenTarget;
				if (actualTarget > this.spikeState[i].OpenValue)
				{
					Vector2 spikeDir;
					switch (i)
					{
						default:
						case 0: spikeDir = new Vector2(1, -1); break;
						case 1: spikeDir = new Vector2(1, 1); break;
						case 2: spikeDir = new Vector2(-1, 1); break;
						case 3: spikeDir = new Vector2(-1, -1); break;
					}

					Vector2 spikeBeginWorld = transform.GetWorldPoint(spikeDir * 4);
					Vector2 spikeEndWorld = transform.GetWorldPoint(spikeDir * 11);
					List<RayCastData> raycast = RigidBody.RayCast(spikeBeginWorld, spikeEndWorld, data => 
					{
						if (data.Body == body) return -1;
						return data.Fraction;
					});

					if (raycast.Count > 0 && raycast[0].Fraction < 1.0f)
					{
						actualTarget = 0.0f;
					}
				}

				float spikeMoveDir = MathF.Sign(actualTarget - this.spikeState[i].OpenValue);
				this.spikeState[i].OpenValue = MathF.Clamp(this.spikeState[i].OpenValue + spikeMoveDir * this.spikeState[i].Speed * Time.TimeMult, 0.0f, 1.0f);
			}
			if (this.spikes != null)
			{
				for (int i = 0; i < this.spikes.Length; i++)
				{
					if (this.spikes[i] == null) continue;
					Rect spikeRect = this.spikes[i].Rect;

					spikeRect.Y = MathF.Lerp(3.5f, -4.5f, this.spikeState[i].OpenValue);

					this.spikes[i].Rect = spikeRect;
				}
			}

			// Do AI state handling stuff
			switch (this.state)
			{
				case MindState.Asleep:
				{
					this.Awake();
					break;
				}
				case MindState.FallingAsleep:
				{
					if (this.eyeOpenValue == 0.0f)
						this.state = MindState.Asleep;
					break;
				}
				case MindState.Awaking:
				{
					if (this.eyeOpenValue == 1.0f)
					{
						this.state = MindState.Idle;
						this.ActivateSpikes();
					}
					break;
				}
				case MindState.Idle:
				{
					// Blink occasionally
					this.blinkTimer -= Time.MsPFMult * Time.TimeMult;
					if (this.blinkTimer <= 0.0f)
					{
						this.RandomizeBlinkTimer();
						this.BlinkEye();
					}
					break;
				}
			}
		}

		void ICmpInitializable.OnInit(Component.InitContext context)
		{
			if (context == InitContext.Activate)
			{
				// Retrieve eye object references and initialize it
				GameObject eyeObject = this.GameObj.ChildByName("Eye");
				this.eye = eyeObject != null ? eyeObject.GetComponent<AnimSpriteRenderer>() : null;
				if (this.eye != null)
				{
					this.eye.AnimLoopMode = AnimSpriteRenderer.LoopMode.FixedSingle;
					this.eye.AnimDuration = 1.0f;
					this.eye.AnimTime = this.eyeOpenValue;
				}

				// Retrieve spike references
				GameObject[] spikeObj = new GameObject[4];
				spikeObj[0] = this.GameObj.ChildByName("SpikeTopRight");
				spikeObj[1] = this.GameObj.ChildByName("SpikeBottomRight");
				spikeObj[2] = this.GameObj.ChildByName("SpikeBottomLeft");
				spikeObj[3] = this.GameObj.ChildByName("SpikeTopLeft");
				this.spikes = new SpriteRenderer[spikeObj.Length];
				for (int i = 0; i < spikeObj.Length; i++)
				{
					this.spikes[i] = spikeObj[i] != null ? spikeObj[i].GetComponent<SpriteRenderer>() : null;
				}
			}
		}
		void ICmpInitializable.OnShutdown(Component.ShutdownContext context) {}

		void ICmpCollisionListener.OnCollisionBegin(Component sender, CollisionEventArgs args)
		{
			RigidBodyCollisionEventArgs bodyArgs = args as RigidBodyCollisionEventArgs;
			if (bodyArgs == null) return;

			RigidBody body = bodyArgs.MyShape.Parent;
			int spikeIndex = -1;
			{
				int i = 0;
				foreach (ShapeInfo shape in body.Shapes.Skip(1))
				{
					if (bodyArgs.MyShape == shape)
					{
						spikeIndex = i;
						break;
					}
					i++;
				}
			}

			if (spikeIndex != -1)
			{
				if (this.spikeState[spikeIndex].OpenValue > 0.75f)
				{
					this.GameObj.DisposeLater();
				}
			}
			else
			{
			}
		}
		void ICmpCollisionListener.OnCollisionEnd(Component sender, CollisionEventArgs args)
		{
		}
		void ICmpCollisionListener.OnCollisionSolve(Component sender, CollisionEventArgs args)
		{
		}
	}
}
