using System;
using System.Collections.Generic;
using System.Linq;

using Duality;
using Duality.Components;
using Duality.Editor;
using Duality.Plugins.Tilemaps;
using Duality.Plugins.Tilemaps.Properties;
using Duality.Plugins.Tilemaps.Sample.RpgLike.Properties;

namespace Duality.Plugins.Tilemaps.Sample.RpgLike
{
	/// <summary>
	/// Animates an <see cref="ActorRenderer"/> on the same <see cref="GameObject"/>.
	/// </summary>
	[RequiredComponent(typeof(ActorRenderer))]
	[EditorHintCategory(SampleResNames.CategoryRpgLike)]
	[EditorHintImage(TilemapsResNames.ImageActorRenderer)]
	public class ActorAnimator : Component, ICmpUpdatable
	{
		/// <summary>
		/// Describes the sprite animations loop behaviour.
		/// </summary>
		public enum LoopMode
		{
			/// <summary>
			/// Represents the intention to let the animation decide the loop mode.
			/// </summary>
			Default,

			/// <summary>
			/// The animation is played once an then remains in its last frame.
			/// </summary>
			Once,
			/// <summary>
			/// The animation is looped: When reaching the last frame, it begins again at the first one.
			/// </summary>
			Loop,
			/// <summary>
			/// The animation plays forward until reaching the end, then reverses and plays backward until 
			/// reaching the start again. This "pingpong" behaviour is looped.
			/// </summary>
			PingPong,
			/// <summary>
			/// A single frame is selected randomly each time the object is initialized and remains static
			/// for its whole lifetime.
			/// </summary>
			RandomSingle
		}

		private List<ActorAnimation> animations     = new List<ActorAnimation>();
		private ActorAnimation       activeAnim     = null;
		private LoopMode             activeLoopMode = LoopMode.Loop;
		private float                animTime       = 0.0f;
		private float                animDirection  = 0.0f;
		private float                animSpeed      = 1.0f;

		/// <summary>
		/// [GET / SET] A list of animations that is available for the animated actor.
		/// </summary>
		public List<ActorAnimation> Animations
		{
			get { return this.animations; }
			set
			{
				if (value != null)
					this.animations = value;
				else
					this.animations.Clear();
			}
		}
		/// <summary>
		/// [GET / SET] The currently active animation.
		/// </summary>
		public ActorAnimation ActiveAnimation
		{
			get { return this.activeAnim; }
			set { this.activeAnim = value; }
		}
		/// <summary>
		/// [GET / SET] The currently active animation loop mode.
		/// </summary>
		public LoopMode ActiveLoopMode
		{
			get { return this.activeLoopMode; }
			set { this.activeLoopMode = value; }
		}
		/// <summary>
		/// [GET / SET] The current time of the active animation.
		/// </summary>
		public float AnimationTime
		{
			get { return this.animTime; }
			set { this.animTime = value; }
		}
		/// <summary>
		/// [GET / SET] The direction (in radians) that is used for selecting animation frames.
		/// </summary>
		public float AnimationDirection
		{
			get { return this.animDirection; }
			set { this.animDirection = value; }
		}
		/// <summary>
		/// [GET / SET] A speed multiplier that determines how fast the animation is played,
		/// relative to its regular animation speed.
		/// </summary>
		public float AnimationSpeed
		{
			get { return this.animSpeed; }
			set { this.animSpeed = value; }
		}

		/// <summary>
		/// Retrieves one of the available animations that matches the specified name.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public ActorAnimation GetAnimation(string name)
		{
			foreach (ActorAnimation anim in this.animations)
			{
				if (anim == null) continue;
				if (anim.Name == name)
					return anim;
			}
			return null;
		}
		/// <summary>
		/// Plays one of the available animations that matches the specified name.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="resetAnim"></param>
		/// <param name="loopMode"></param>
		public void PlayAnimation(string name, bool resetAnim = false, LoopMode loopMode = LoopMode.Default)
		{
			ActorAnimation anim = this.GetAnimation(name);

			// When specifying default, let the animation decide which loop mode to use.
			if (anim != null && loopMode == LoopMode.Default)
				loopMode = anim.PreferredLoopMode;

			// Still set to default? Use a regular loop then.
			if (loopMode == LoopMode.Default)
				loopMode = LoopMode.Loop;

			// If we're already playing that animation, just continue doing it.
			if (this.activeAnim == anim && !resetAnim)
				return;

			this.activeAnim = anim;
			this.activeLoopMode = loopMode;

			if (this.activeAnim != null && loopMode == LoopMode.RandomSingle)
				this.animTime = MathF.Rnd.NextFloat(0.0f, this.activeAnim.Duration);
			else
				this.animTime = 0.0f;
		}

		void ICmpUpdatable.OnUpdate()
		{
			// Don't have a valid, active animation? Early-out.
			if (this.activeAnim == null) return;
			if (this.activeAnim.FrameCount <= 0) return;
			if (this.activeAnim.DirectionMap.Length == 0) return;

			// Retrieve the actor renderer we're going to animate
			ActorRenderer actor = this.GameObj.GetComponent<ActorRenderer>();

			// Determine the active direction
			int startFrame = 0;
			float minAngleDiff = float.MaxValue;
			for (int i = 0; i < this.activeAnim.DirectionMap.Length; i++)
			{
				float angleDiff = MathF.CircularDist(
					this.animDirection, 
					MathF.DegToRad(this.activeAnim.DirectionMap[i].Angle));
				if (angleDiff < minAngleDiff)
				{
					minAngleDiff = angleDiff;
					startFrame = this.activeAnim.DirectionMap[i].SpriteSheetIndex;
				}
			}

			// Determine the currently displayed frame
			float animProgress = (this.animTime / this.activeAnim.Duration) % 1.0f;
			int animCycleCount = (int)(this.animTime / this.activeAnim.Duration);
			switch (this.activeLoopMode)
			{
				// In single-shot animations, complete the animation only once
				case LoopMode.Once:
					if (animCycleCount > 1)
						animProgress = 0.0f;
					goto case LoopMode.Loop;

				// Regular looped animation
				case LoopMode.Loop:
					actor.SpriteIndex = startFrame + MathF.Clamp(
						(int)(this.activeAnim.FrameCount * animProgress), 
						0, 
						this.activeAnim.FrameCount);
					break;

				// Alternating regular and reverse animation
				case LoopMode.PingPong:
					bool reverse = (animCycleCount % 2 == 0);
					float pingPongAnimProgress = 
						reverse ? 
						(1.0f - animProgress) : 
						animProgress;

					actor.SpriteIndex = startFrame + MathF.Clamp(
						(int)(0.5f + (this.activeAnim.FrameCount - 1) * pingPongAnimProgress), 
						0, 
						this.activeAnim.FrameCount);
					break;
			}

			// Advance animation time, unless we're displaying a fixed single frame
			if (this.activeLoopMode != LoopMode.RandomSingle)
			{
				this.animTime += this.animSpeed * Time.TimeMult * Time.SPFMult;
			}
		}
	}
}
