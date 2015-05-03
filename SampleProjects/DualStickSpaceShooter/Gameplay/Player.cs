using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;
using OpenTK.Input;

using Duality;
using Duality.Components;
using Duality.Components.Renderers;
using Duality.Components.Physics;
using Duality.Resources;
using Duality.Drawing;

namespace DualStickSpaceShooter
{
	public class Player : Component, ICmpUpdatable
	{
		public const float RespawnDelay = 10000.0f;

		public static bool IsAnyPlayerAlive
		{
			get { return Scene.Current.FindComponents<Player>().Any(p => p.Active && p.ControlObject != null && p.ControlObject.Active); }
		}
		public static IEnumerable<Player> AllPlayers
		{
			get { return Scene.Current.FindComponents<Player>(); }
		}
		public static IEnumerable<Player> AlivePlayers
		{
			get { return Scene.Current.FindComponents<Player>().Where(p => p.Active && p.ControlObject != null && p.ControlObject.Active); }
		}


		private PlayerId			id					= PlayerId.Unknown;
		private	Ship				controlObj			= null;
		private	ColorRgba			color				= ColorRgba.White;
		private	float				respawnTime			= 0.0f;
		private	bool				hasReachedGoal		= false;
		private	float				goalReachTime		= 0.0f;
		private	ContentRef<Prefab>	goalEffect			= null;
		private	ContentRef<Sound>	weaponSound			= null;
		private	ContentRef<Sound>	flightLoop			= null;

		[DontSerialize]
		private	InputMapping	input				= null;
		[DontSerialize]
		private	GameObject		goalEffectInstance	= null;


		public PlayerId Id
		{
			get { return this.id; }
			set { this.id = value; }
		}
		public InputMethod InputMethod
		{
			get { return this.input != null ? this.input.Method : InputMethod.Unknown; }
			set
			{
				if (this.input == null) this.input = new InputMapping();
				this.input.Method = value;
			}
		}
		public bool IsPlaying
		{
			get { return this.InputMethod != InputMethod.Unknown; }
		}
		public Ship ControlObject
		{
			get { return this.controlObj; }
			set
			{
				this.controlObj = value;
				if (this.controlObj != null)
					this.controlObj.UpdatePlayerColor();
			}
		}
		public ColorRgba Color
		{
			get { return this.color; }
			set
			{
				this.color = value;
				if (this.controlObj != null)
					this.controlObj.UpdatePlayerColor();
			}
		}
		public float RespawnTime
		{
			get { return this.respawnTime; }
		}
		public bool HasReachedGoal
		{
			get { return this.hasReachedGoal; }
		}
		public ContentRef<Prefab> GoalEffect
		{
			get { return this.goalEffect; }
			set { this.goalEffect = value; }
		}
		public ContentRef<Sound> WeaponSound
		{
			get { return this.weaponSound; }
			set { this.weaponSound = value; }
		}
		public ContentRef<Sound> FlightLoop
		{
			get { return this.flightLoop; }
			set { this.flightLoop = value; }
		}


		public void NotifyGoalReached()
		{
			if (this.hasReachedGoal) return;
			this.hasReachedGoal = true;
			this.goalReachTime = (float)Time.GameTimer.TotalMilliseconds;

			// Become a ghost
			RigidBody body = this.controlObj.GameObj.RigidBody;
			body.CollidesWith = CollisionCategory.None;

			// Become invincible
			this.controlObj.Hitpoints = 10000.0f;
		}
		void ICmpUpdatable.OnUpdate()
		{

			// If the object we're controlling has been destroyed, forget about it
			if (this.controlObj != null && this.controlObj.Disposed)
				this.controlObj = null;
			
			// See what player inputs there are to handle
			if (this.input == null) this.input = new InputMapping();
			bool hasInputMethod = this.input.Method != InputMethod.Unknown;
			if (this.controlObj != null)
			{
				this.input.Update(this.controlObj.GameObj.Transform);
			}
			else
			{
				this.input.Update(null);
			}

			// Spawn the player object for the first time when input is detected
			if (this.controlObj != null && this.input.Method != InputMethod.Unknown && !hasInputMethod)
			{
				// Find an alive player or determine that no one has started playing yet
				bool gameAlreadyOver = false;
				Player alivePlayer = Player.AlivePlayers.FirstOrDefault();
				if (alivePlayer != null)
				{
					// Move near already alive player
					Vector3 alivePlayerPos = alivePlayer.controlObj.GameObj.Transform.Pos;
					this.controlObj.GameObj.Transform.Pos = alivePlayerPos;
				}
				else if (Player.AllPlayers.Any(p => p != this && p.IsPlaying))
				{
					// If we don't have an alive player, but do have playing players, the game must be already over.
					gameAlreadyOver = true;
				}
				else
				{
					// Move near initial spawn point
					this.controlObj.GameObj.Transform.Pos = SpawnPoint.SpawnPos;
				}

				// Spawn for the first time / enter the game
				if (!gameAlreadyOver)
				{
					this.controlObj.GameObj.Active = true;
				}
			}
			
			// Manage the object this player is supposed to control
			if (this.controlObj != null)
			{
				if (this.hasReachedGoal)
				{
					RigidBody body = this.controlObj.GameObj.RigidBody;
					SpriteRenderer sprite = this.controlObj.GameObj.GetComponent<SpriteRenderer>();

					// If we've reached the goal, update the final animation and do nothing else
					float goalAnim = MathF.Clamp(((float)Time.GameTimer.TotalMilliseconds - this.goalReachTime) / 2000.0f, 0.0f, 1.0f);

					// Don't move
					body.LinearVelocity *= MathF.Pow(0.9f, Time.TimeMult);
					this.controlObj.TargetAngleRatio = 0.1f;
					this.controlObj.TargetThrust = Vector2.Zero;

					// Fade out
					if (sprite.CustomMaterial == null) sprite.CustomMaterial = new BatchInfo(sprite.SharedMaterial.Res);
					sprite.CustomMaterial.Technique = DrawTechnique.SharpAlpha;
					sprite.ColorTint = sprite.ColorTint.WithAlpha(1.0f - goalAnim);
					
					// Spawn a goal reached effect
					if (goalAnim > 0.2f && this.goalEffect != null && this.goalEffectInstance == null)
					{
						this.goalEffectInstance = this.goalEffect.Res.Instantiate(this.controlObj.GameObj.Transform.Pos);
						Scene.Current.AddObject(this.goalEffectInstance);
					}

					// Let the ship disappear
					if (goalAnim >= 1.0f)
					{
						this.controlObj.GameObj.Active = false;
					}
				}
				else if (this.controlObj.Active)
				{
					// Apply control inputs to the controlled object
					this.controlObj.TargetAngle = this.input.ControlLookAngle;
					this.controlObj.TargetAngleRatio = this.input.ControlLookSpeed;
					this.controlObj.TargetThrust = this.input.ControlMovement;
					if (this.input.ControlFireWeapon)
						this.controlObj.FireWeapon();
				}
				else if (hasInputMethod && Player.IsAnyPlayerAlive && !this.hasReachedGoal)
				{
					// Respawn when possible
					this.respawnTime += Time.MsPFMult * Time.TimeMult;
					if (this.respawnTime > RespawnDelay)
					{
						// Move near alive player
						Player alivePlayer = Player.AlivePlayers.FirstOrDefault(); 
						Vector3 alivePlayerPos = alivePlayer.controlObj.GameObj.Transform.Pos;
						this.controlObj.GameObj.Transform.Pos = alivePlayerPos;

						// Respawn
						this.respawnTime = 0.0f;
						this.controlObj.Revive();
					}
				}
			}

			// Quit the game when requested
			if (this.input.ControlQuit)
				DualityApp.Terminate();

			// If it's game over, allow to restart the game
			GameOverScreen gameOverScreen = Scene.Current.FindComponent<GameOverScreen>();
			if (gameOverScreen != null && gameOverScreen.HasGameEnded)
			{
				if (this.input.ControlStart)
					Scene.Reload();
			}
		}
	}
}
