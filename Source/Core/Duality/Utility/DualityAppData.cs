﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Duality.Resources;

namespace Duality
{
	/// <summary>
	/// Provides general information about this Duality application / game.
	/// </summary>
	public class DualityAppData
	{
		private string            appName               = "Duality Application";
		private string            authorName            = "Unknown";
		private string            websiteUrl            = "http://www.adamslair.net";
		private uint              version               = 0;
		private ContentRef<Scene> startScene            = null;
		private float             speedOfSound          = 360.0f;
		private float             soundDopplerFactor    = 1.0f;
		private float             physicsVelThreshold   = 0.5f * PhysicsUnit.VelocityToDuality;
		private bool              physicsFixedTime      = false;
		private bool              localUserData         = true;
		private bool              multisampleBackBuffer = true;
		private string[]          skipBackends          = null;
		private object            customData            = null;

		/// <summary>
		/// [GET / SET] The name of your application / game. It will also be used as a window title by the launcher app.
		/// </summary>
		public string AppName
		{
			get { return this.appName; }
			set { this.appName = value; }
		}
		/// <summary>
		/// [GET / SET] The author name of your application. Might be your or your team's name or -nickname.
		/// </summary>
		public string AuthorName
		{
			get { return this.authorName; }
			set { this.authorName = value; }
		}
		/// <summary>
		/// [GET / SET] The address of this game's official website or similar.
		/// </summary>
		public string WebsiteUrl
		{
			get { return this.websiteUrl; }
			set { this.websiteUrl = value; }
		}
		/// <summary>
		/// [GET / SET] The current application / game version.
		/// </summary>
		public uint Version
		{
			get { return this.version; }
			set { this.version = value; }
		}
		/// <summary>
		/// [GET / SET] A reference to the start <see cref="Duality.Resources.Scene"/>. It is used by the launcher app to
		/// determine which Scene to load initially.
		/// </summary>
		public ContentRef<Scene> StartScene
		{
			get { return this.startScene; }
			set { this.startScene = value; }
		}
		/// <summary>
		/// [GET / SET] The speed of sound. While this is technically a unitless value, you might assume something like "meters per second".
		/// It is used to calculate the doppler effect of <see cref="Duality.Audio.SoundInstance">SoundInstances</see> that are moving relative to the
		/// <see cref="Duality.Components.SoundListener"/>.
		/// </summary>
		public float SpeedOfSound
		{
			get { return this.speedOfSound; }
			set { this.speedOfSound = value; }
		}
		/// <summary>
		/// [GET / SET] A factor by which the strength of the doppler effect is multiplied.
		/// </summary>
		public float SoundDopplerFactor
		{
			get { return this.soundDopplerFactor; }
			set { this.soundDopplerFactor = value; }
		}
		/// <summary>
		/// [GET / SET] Any velocity below this value will be resolved using inelastic equations i.e. won't lead to "bouncing".
		/// </summary>
		public float PhysicsVelocityThreshold
		{
			get { return this.physicsVelThreshold; }
			set { this.physicsVelThreshold = value; }
		}
		/// <summary>
		/// [GET / SET] Does the physics simulation use fixed time steps? However, this setting may be overwritten dynamically due
		/// to frame timing restrictions. To check whether fixed-timestep physics is currently active, use <see cref="Duality.Resources.Scene.PhysicsFixedTime"/>
		/// </summary>
		public bool PhysicsFixedTime
		{
			get { return this.physicsFixedTime; }
			set { this.physicsFixedTime = value; }
		}
		/// <summary>
		/// [GET / SET] If true, user data is saved locally in the game folder instead of the current user account.
		/// </summary>
		public bool LocalUserData
		{
			get { return this.localUserData; }
			set { this.localUserData = value; }
		}
		/// <summary>
		/// [GET / SET] Determines whether or not the backbuffer uses multisampling based on <see cref="DualityUserData.AntialiasingQuality"/>.
		/// Set this to false if you don't render to the backbuffer directly, but use a custom <see cref="RenderTarget"/> setup. You'll have to
		/// apply possible existing user quality settings yourself.
		/// </summary>
		public bool MultisampleBackBuffer
		{
			get { return this.multisampleBackBuffer; }
			set { this.multisampleBackBuffer = value; }
		}
		/// <summary>
		/// [GET / SET] An optional list of backend <see cref="Duality.Backend.IDualityBackend.Id"/> values to skip when loading.
		/// </summary>
		public string[] SkipBackends
		{
			get { return this.skipBackends; }
			set { this.skipBackends = value; }
		}
		/// <summary>
		/// [GET / SET] Use this property to store custom application data.
		/// </summary>
		public object CustomData
		{
			get { return this.customData; }
			set { this.customData = value; }
		}
	}
}
