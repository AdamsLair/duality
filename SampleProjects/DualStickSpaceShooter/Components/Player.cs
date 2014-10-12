using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;
using OpenTK.Input;

using Duality;
using Duality.Components;
using Duality.Resources;
using Duality.Drawing;

namespace DualStickSpaceShooter
{
	[Serializable]
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


		private PlayerId		id			= PlayerId.Unknown;
		private	Ship			controlObj	= null;
		private	ColorRgba		color		= ColorRgba.White;
		private	float			respawnTime	= 0.0f;

		[NonSerialized]
		private	InputMapping	input		= null;


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
					gameAlreadyOver = true;
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
				if (this.controlObj.Active)
				{
					// Apply control inputs to the controlled object
					this.controlObj.TargetAngle = this.input.ControlLookAngle;
					this.controlObj.TargetAngleRatio = this.input.ControlLookSpeed;
					this.controlObj.TargetThrust = this.input.ControlMovement;
					if (this.input.ControlFireWeapon)
						this.controlObj.FireWeapon();
				}
				else if (hasInputMethod && Player.IsAnyPlayerAlive)
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
			if (gameOverScreen != null && gameOverScreen.IsGameOver)
			{
				if (this.input.ControlStart)
				{
					// Force the game to reload the current level by disposing it.
					ContentRef<Scene> currentLevel = Scene.Current;
					Scene.Current.Dispose();
					Scene.SwitchTo(currentLevel);
				}
			}
		}
	}
}
