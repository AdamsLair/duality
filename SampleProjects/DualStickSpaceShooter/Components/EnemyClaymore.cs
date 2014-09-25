using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;

using Duality;
using Duality.Components;
using Duality.Components.Renderers;
using Duality.Resources;
using Duality.Drawing;

namespace DualStickSpaceShooter
{
	[Serializable]
	[RequiredComponent(typeof(Ship))]
	public class EnemyClaymore : Component, ICmpUpdatable, ICmpInitializable
	{
		private enum MindState
		{
			Asleep,
			Awaking,
			FallingAsleep,
			Idle
		}

		private	MindState	state			= MindState.Asleep;
		private	float		blinkTimer		= 0.0f;
		private	float		eyeOpenValue	= 0.0f;
		private	float		eyeOpenTarget	= 0.0f;
		private	float		eyeSpeed		= 0.0f;
		private	bool		eyeBlinking		= false;

		[NonSerialized] private AnimSpriteRenderer	eye	= null;
		
		private void Sleep()
		{
			if (this.state == MindState.Asleep) return;
			if (this.state == MindState.FallingAsleep) return;

			this.state = MindState.FallingAsleep;
			this.eyeOpenTarget = 0.0f;
			this.eyeBlinking = false;
			this.eyeSpeed = Time.SPFMult / MathF.Rnd.NextFloat(2.5f, 3.5f);
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

		void ICmpUpdatable.OnUpdate()
		{
			Ship ship = this.GameObj.GetComponent<Ship>();

			// Udpate the eyes visual appearance
			this.eyeOpenValue = MathF.Clamp(this.eyeOpenValue + MathF.Sign(this.eyeOpenTarget - this.eyeOpenValue) * this.eyeSpeed * Time.TimeMult, 0.0f, 1.0f);
			if (this.eyeBlinking && this.eyeOpenValue == 0.0f) this.eyeOpenTarget = 1.0f;
			if (this.eye != null)
			{
				this.eye.AnimTime = this.eyeOpenValue;
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
						this.state = MindState.Idle;
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
			}
		}

		void ICmpInitializable.OnShutdown(Component.ShutdownContext context) {}
	}
}
