using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Duality;
using Duality.Editor;
using Duality.Components;
using Duality.Components.Physics;
using Duality.Components.Renderers;
using Duality.Resources;
using Duality.Drawing;

namespace DualStickSpaceShooter
{
	[RequiredComponent(typeof(Ship))]
	public class EnemyClaymore : Component, ICmpUpdatable, ICmpInitializable, ICmpCollisionListener, ICmpMessageListener
	{
		[Flags]
		public enum BehaviorFlags
		{
			None,

			Chase
		}

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
			public bool Blinking;
			public int ContactCount;
		}

		private const float WakeupDist = 300.0f;
		private const float SpikeAttackMoveDist = 100.0f;
		private const float SleepTime = 3000.0f;

		private	MindState					state			= MindState.Asleep;
		private	BehaviorFlags				behavior		= BehaviorFlags.Chase;
		private	float						idleTimer		= 0.0f;
		private	float						blinkTimer		= 0.0f;
		private	float						eyeOpenValue	= 0.0f;
		private	float						eyeOpenTarget	= 0.0f;
		private	float						eyeSpeed		= 0.0f;
		private	bool						eyeBlinking		= false;
		private	bool						spikesActive	= false;
		private	SpikeState[]				spikeState		= new SpikeState[4];
		private	ContentRef<EnemyBlueprint>	blueprint		= null;

		[DontSerialize] private AnimSpriteRenderer	eye				= null;
		[DontSerialize] private SpriteRenderer[]	spikes			= null;
		[DontSerialize] private SoundInstance		moveSoundLoop	= null;
		[DontSerialize] private SoundInstance		dangerSoundLoop	= null;
		
		public BehaviorFlags Behavior
		{
			get { return this.behavior; }
			set { this.behavior = value; }
		}
		public ContentRef<EnemyBlueprint> Blueprint
		{
			get { return this.blueprint; }
			set { this.blueprint = value; }
		}

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
			this.eyeSpeed = Time.SPFMult / MathF.Rnd.NextFloat(1.0f, 1.5f);

			this.RandomizeBlinkTimer();
		}
		private void RandomizeBlinkTimer()
		{
			this.blinkTimer += MathF.Rnd.Next(100, 10000);
		}
		
		private void BlinkEye(float blinkStrength = 1.0f)
		{
			this.eyeBlinking = true;
			this.eyeOpenTarget = MathF.Min(1.0f - blinkStrength, this.eyeOpenTarget);
			this.eyeSpeed = Time.SPFMult / MathF.Rnd.NextFloat(0.05f, 0.25f);
		}
		private void BlinkSpikes(float blinkStrength = 1.0f)
		{
			if (!this.spikesActive) return;
			for (int i = 0; i < this.spikeState.Length; i++)
			{
				this.spikeState[i].Blinking = true;
				this.spikeState[i].OpenTarget = MathF.Min(1.0f - blinkStrength, this.eyeOpenTarget);
				this.spikeState[i].Speed = Time.SPFMult / MathF.Rnd.NextFloat(0.1f, 0.2f);
			}
		}
		private void DeactivateSpikes()
		{
			if (!this.spikesActive) return;
			this.spikesActive = false;
			for (int i = 0; i < this.spikeState.Length; i++)
			{
				this.spikeState[i].Blinking = false;
				this.spikeState[i].OpenTarget = 0.0f;
				this.spikeState[i].Speed = Time.SPFMult / MathF.Rnd.NextFloat(0.25f, 1.0f);
			}
		}
		private void ActivateSpikes()
		{
			if (this.spikesActive) return;
			this.spikesActive = true;
			for (int i = 0; i < this.spikeState.Length; i++)
			{
				this.spikeState[i].Blinking = false;
				this.spikeState[i].OpenTarget = 1.0f;
				this.spikeState[i].Speed = Time.SPFMult / MathF.Rnd.NextFloat(0.25f, 1.0f);
			}
		}

		private void FireExplosives()
		{
			// Set up working data
			EnemyBlueprint blueprint = this.blueprint.Res;
			Ship ship = this.GameObj.GetComponent<Ship>();
			Vector2 pos = this.GameObj.Transform.Pos.Xy;

			// Push other objects away
			GameplayHelper.Shockwave(
				pos, 
				blueprint.ExplosionRadius, 
				blueprint.ExplosionForce,
				blueprint.ExplosionMaxVelocity,
				obj => obj.GameObj != this.GameObj);

			// Damage other objects
			GameplayHelper.ExplosionDamage(
				pos, 
				blueprint.ExplosionRadius,
				blueprint.ExplosionDamage,
				body => body.GameObj.GetComponent<Ship>() != null && body.GameObj != ship.GameObj);

			// Die instantly
			ship.Die();

			// Spawn explosion effects
			if (blueprint.ExplosionEffects != null)
			{
				Transform transform = this.GameObj.Transform;
				for (int i = 0; i < blueprint.ExplosionEffects.Length; i++)
				{
					GameObject effectObj = blueprint.ExplosionEffects[i].Res.Instantiate(transform.Pos);
					Scene.Current.AddObject(effectObj);
				}
			}

			// Play explosion sound
			if (blueprint.ExplosionSound != null)
			{
				SoundInstance inst = DualityApp.Sound.PlaySound3D(blueprint.ExplosionSound, new Vector3(pos));
				inst.Pitch = MathF.Rnd.NextFloat(0.8f, 1.25f);
			}
		}

		private bool HasLineOfSight(GameObject obj, bool passThroughShips)
		{
			Transform otherTransform = obj.Transform;
			Transform transform = this.GameObj.Transform;

			RayCastData firstHit;
			bool hitAnything = RigidBody.RayCast(transform.Pos.Xy, otherTransform.Pos.Xy, data => 
			{
				if (data.GameObj == this.GameObj) return -1;
				if (data.Shape.IsSensor) return -1;

				if (passThroughShips)
				{
					Ship otherShip = data.GameObj.GetComponent<Ship>();
					if (otherShip != null && otherShip.Owner == null) return -1;
				}

				return data.Fraction;
			}, out firstHit);

			return hitAnything && firstHit.GameObj == obj;
		}
		private GameObject GetNearestPlayerObj(out float nearestDist)
		{
			nearestDist = float.MaxValue;
			GameObject nearestObj = null;

			Transform transform = this.GameObj.Transform;
			foreach (Player player in Scene.Current.FindComponents<Player>())
			{
				if (player.ControlObject == null) continue;
				if (!player.ControlObject.Active) continue;
				Transform shipTransform = player.ControlObject.GameObj.Transform;

				float dist = (shipTransform.Pos - transform.Pos).Length;
				if (dist < nearestDist)
				{
					nearestDist = dist;
					nearestObj = player.ControlObject.GameObj;
				}
			}

			return nearestObj;
		}
		private int GetSpikeIndex(ShapeInfo spikeShape)
		{
			RigidBody body = this.GameObj.RigidBody;
			if (body == null) return -1;

			int i = 0;
			foreach (ShapeInfo shape in body.Shapes.Skip(1))
			{
				if (spikeShape == shape) return i;
				i++;
			}

			return -1;
		}

		void ICmpUpdatable.OnUpdate()
		{
			EnemyBlueprint blueprint = this.blueprint.Res;
			Transform transform = this.GameObj.Transform;
			RigidBody body = this.GameObj.RigidBody;
			Ship ship = this.GameObj.GetComponent<Ship>();

			// Calculate distress caused by going in a different direction than desired
			float moveDistress = 0.0f;
			if (body.LinearVelocity.Length > 1.0f)
			{
				Vector2 actualVelocityDir = body.LinearVelocity.Normalized;
				Vector2 desiredVelocityDir = ship.TargetThrust;
				float desiredDirectionFactor = Vector2.Dot(actualVelocityDir, desiredVelocityDir);
				moveDistress = MathF.Clamp(1.0f - desiredDirectionFactor, 0.0f, 1.0f) * MathF.Clamp(body.LinearVelocity.Length - 0.5f, 0.0f, 1.0f);
			}

			// Do AI state handling stuff
			float moveTowardsEnemyRatio = 0.0f;
			switch (this.state)
			{
				case MindState.Asleep:
				{
					// Wake up, if there is a player near
					float nearestDist;
					GameObject nearestObj = this.GetNearestPlayerObj(out nearestDist);
					if (nearestObj != null && nearestDist <= WakeupDist && this.HasLineOfSight(nearestObj, true))
					{
						this.Awake();
					}

					// Don't move actively
					ship.TargetThrust = Vector2.Zero;
					ship.TargetAngle = MathF.Rnd.NextFloat(-MathF.RadAngle30, MathF.RadAngle30);
					ship.TargetAngleRatio = 0.0f;
					break;
				}
				case MindState.FallingAsleep:
				{
					if (this.eyeOpenValue <= 0.0001f)
						this.state = MindState.Asleep;
					break;
				}
				case MindState.Awaking:
				{
					if (this.eyeOpenValue >= 0.9999f)
						this.state = MindState.Idle;
					break;
				}
				case MindState.Idle:
				{
					// Follow, if there is a player near
					float nearestDist;
					GameObject nearestObj = this.GetNearestPlayerObj(out nearestDist);
					if (nearestObj != null && this.HasLineOfSight(nearestObj, false))
					{
						if (behavior.HasFlag(BehaviorFlags.Chase))
						{
							Transform nearestObjTransform = nearestObj.Transform;
							Vector2 targetDiff = nearestObjTransform.Pos.Xy - transform.Pos.Xy;
							ship.TargetThrust = targetDiff / MathF.Max(targetDiff.Length, 25.0f);
							moveTowardsEnemyRatio = ship.TargetThrust.Length;
						}
						else
						{
							ship.TargetThrust = Vector2.Zero;
						}
						ship.TargetAngle += 0.001f * Time.TimeMult;
						ship.TargetAngleRatio = 0.1f;

						this.idleTimer = MathF.Rnd.NextFloat(0.0f, SleepTime * 0.25f);
						
						if (nearestDist <= SpikeAttackMoveDist)
						{
							moveDistress = 0.0f;
							if (!this.spikesActive)
								this.ActivateSpikes();
						}
						else if (ship.TargetThrust.Length > 0.1f)
						{
							if (this.spikesActive)
								this.DeactivateSpikes();
						}
					}
					// Try to stay in place otherwise
					else
					{
						ship.TargetThrust = -body.LinearVelocity / MathF.Max(body.LinearVelocity.Length, ship.Blueprint.Res.MaxSpeed);
						ship.TargetAngleRatio = 0.1f;
						this.idleTimer += Time.MsPFMult * Time.TimeMult;
						
						if (this.spikesActive)
							this.DeactivateSpikes();
					}

					// Blink occasionally
					this.blinkTimer -= Time.MsPFMult * Time.TimeMult;
					if (this.blinkTimer <= 0.0f)
					{
						this.RandomizeBlinkTimer();
						this.BlinkEye();
					}

					// Go to sleep if nothing happens.
					if (this.idleTimer > SleepTime)
					{
						this.Sleep();
					}
					break;
				}
			}

			// Udpate the eyes state and visual appearance
			{
				float actualTarget = MathF.Clamp(this.eyeOpenTarget - moveDistress * 0.35f, 0.0f, 1.0f);
				float eyeDiff = MathF.Abs(actualTarget - this.eyeOpenValue);
				float eyeChange = MathF.Sign(actualTarget - this.eyeOpenValue) * MathF.Min(this.eyeSpeed, eyeDiff);

				this.eyeOpenValue = MathF.Clamp(this.eyeOpenValue + eyeChange * Time.TimeMult, 0.0f, 1.0f);
				if (this.eyeBlinking && this.eyeOpenValue <= this.eyeOpenTarget + 0.0001f) this.eyeOpenTarget = 1.0f;

				if (this.eye != null)
				{
					this.eye.AnimTime = this.eyeOpenValue;
				}
			}

			// Update the spikes state and visual appearance
			for (int i = 0; i < this.spikeState.Length; i++)
			{
				float actualTarget = MathF.Clamp(this.spikeState[i].OpenTarget - moveDistress, 0.0f, 1.0f);
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
					bool hitAnything = false;
					RigidBody.RayCast(spikeBeginWorld, spikeEndWorld, data => 
					{
						if (data.Shape.IsSensor) return -1;
						if (data.Body == body) return -1;

						Ship otherShip = data.GameObj.GetComponent<Ship>();
						if (otherShip != null && otherShip.Owner != null) return -1;

						hitAnything = true;
						return 0;
					});

					if (hitAnything)
					{
						actualTarget = 0.0f;
					}
				}

				float spikeMoveDir = MathF.Sign(actualTarget - this.spikeState[i].OpenValue);
				this.spikeState[i].OpenValue = MathF.Clamp(this.spikeState[i].OpenValue + spikeMoveDir * this.spikeState[i].Speed * Time.TimeMult, 0.0f, 1.0f);
				if (this.spikeState[i].Blinking && this.spikeState[i].OpenValue <= this.spikeState[i].OpenTarget + 0.0001f)
				{
					this.spikeState[i].OpenTarget = 1.0f;
					this.spikeState[i].Speed = Time.SPFMult / MathF.Rnd.NextFloat(0.25f, 1.0f);
				}

				// If we're extending a spike where the sensor has already registered a contact, explode
				if (this.spikeState[i].OpenValue > 0.75f && this.spikeState[i].ContactCount > 0)
					this.FireExplosives();
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

			// Make a sound while moving
			if (blueprint.MoveSound != null)
			{
				// Determine the target volume
				float targetVolume = MathF.Clamp(moveTowardsEnemyRatio, 0.0f, 1.0f);

				// Clean up disposed loop
				if (this.moveSoundLoop != null && this.moveSoundLoop.Disposed)
					this.moveSoundLoop = null;

				// Start the loop when requested
				if (targetVolume > 0.0f && this.moveSoundLoop == null)
				{
					this.moveSoundLoop = DualityApp.Sound.PlaySound3D(blueprint.MoveSound, this.GameObj);
					this.moveSoundLoop.Looped = true;
				}

				// Configure existing loop and dispose it when no longer needed
				if (this.moveSoundLoop != null)
				{
					this.moveSoundLoop.Volume += (targetVolume - this.moveSoundLoop.Volume) * 0.05f * Time.TimeMult;
					if (this.moveSoundLoop.Volume <= 0.05f)
					{
						this.moveSoundLoop.FadeOut(0.1f);
						this.moveSoundLoop = null;
					}
				}
			}

			// Make a danger sound while moving with spikes out
			if (blueprint.AttackSound != null)
			{
				// Determine the target volume
				float targetVolume = this.spikesActive ? MathF.Clamp(moveTowardsEnemyRatio, 0.25f, 1.0f) : 0.0f;

				// Clean up disposed loop
				if (this.dangerSoundLoop != null && this.dangerSoundLoop.Disposed)
					this.dangerSoundLoop = null;

				// Start the loop when requested
				if (targetVolume > 0.0f && this.dangerSoundLoop == null)
				{
					this.dangerSoundLoop = DualityApp.Sound.PlaySound3D(blueprint.AttackSound, this.GameObj);
					this.dangerSoundLoop.Looped = true;
				}

				// Configure existing loop and dispose it when no longer needed
				if (this.dangerSoundLoop != null)
				{
					this.dangerSoundLoop.Volume += (targetVolume - this.dangerSoundLoop.Volume) * 0.1f * Time.TimeMult;
					if (this.dangerSoundLoop.Volume <= 0.05f)
					{
						this.dangerSoundLoop.FadeOut(0.1f);
						this.dangerSoundLoop = null;
					}
				}
			}
		}

		void ICmpMessageListener.OnMessage(GameMessage msg)
		{
			// We're dead? Damnit! Blow up at least.
			if (msg is ShipDeathMessage)
			{
				this.FireExplosives();
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
		void ICmpInitializable.OnShutdown(Component.ShutdownContext context)
		{
			if (context == ShutdownContext.Deactivate)
			{
				// Fade out playing loop sounds, if there are any. Clean up!
				if (this.moveSoundLoop != null)
				{
					this.moveSoundLoop.FadeOut(0.5f);
					this.moveSoundLoop = null;
				}
				if (this.dangerSoundLoop != null)
				{
					this.dangerSoundLoop.FadeOut(0.5f);
					this.dangerSoundLoop = null;
				}
			}
		}

		void ICmpCollisionListener.OnCollisionBegin(Component sender, CollisionEventArgs args)
		{
			RigidBodyCollisionEventArgs bodyArgs = args as RigidBodyCollisionEventArgs;
			if (bodyArgs == null) return;
			if (bodyArgs.OtherShape.IsSensor) return;

			Bullet otherBullet = bodyArgs.CollideWith.GetComponent<Bullet>();
			int spikeIndex = this.GetSpikeIndex(bodyArgs.MyShape);
			if (spikeIndex != -1 && otherBullet == null)
			{
				this.spikeState[spikeIndex].ContactCount++;
			}

			if (this.state != MindState.Asleep)
			{
				if (spikeIndex != -1)
				{
					if (this.spikeState[spikeIndex].OpenValue > 0.75f && otherBullet == null)
						this.FireExplosives();
				}
				else
				{
					if (otherBullet != null)
					{
						this.BlinkSpikes(MathF.Rnd.NextFloat(0.5f, 1.0f));
						this.BlinkEye(MathF.Rnd.NextFloat(0.35f, 0.6f));
					}
				}
			}
		}
		void ICmpCollisionListener.OnCollisionEnd(Component sender, CollisionEventArgs args)
		{
			RigidBodyCollisionEventArgs bodyArgs = args as RigidBodyCollisionEventArgs;
			if (bodyArgs == null) return;
			if (bodyArgs.OtherShape.IsSensor) return;
			
			Bullet otherBullet = bodyArgs.CollideWith.GetComponent<Bullet>();
			int spikeIndex = this.GetSpikeIndex(bodyArgs.MyShape);
			if (spikeIndex != -1 && otherBullet == null)
			{
				this.spikeState[spikeIndex].ContactCount--;
			}
		}
		void ICmpCollisionListener.OnCollisionSolve(Component sender, CollisionEventArgs args)
		{
		}
	}
}
