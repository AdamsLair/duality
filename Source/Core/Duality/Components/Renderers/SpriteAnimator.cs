using System;
using System.Collections.Generic;
using System.Linq;

using Duality.Editor;
using Duality.Drawing;
using Duality.Resources;
using Duality.Cloning;
using Duality.Properties;

namespace Duality.Components.Renderers
{
	/// <summary>
	/// Animates a <see cref="ICmpSpriteRenderer"/> on the same <see cref="GameObject"/>.
	/// </summary>
	[ManuallyCloned]
	[EditorHintCategory(CoreResNames.CategoryGraphics)]
	[EditorHintImage(CoreResNames.ImageAnimSpriteAnimator)]
	public class SpriteAnimator : Component, ICmpUpdatable, ICmpInitializable
	{
		/// <summary>
		/// Describes the sprite animations loop behaviour.
		/// </summary>
		public enum LoopMode
		{
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
			RandomSingle,
			/// <summary>
			/// A fixed, single frame is displayed. Which one depends on the one you set in the editor or
			/// in source code.
			/// </summary>
			FixedSingle,
			/// <summary>
			/// The <see cref="CustomFrameSequence"/> is interpreted and processed as a queue where <see cref="AnimDuration"/>
			/// is the time a single frame takes.
			/// </summary>
			Queue
		}

		private int       animFirstFrame      = 0;
		private int       animFrameCount      = 0;
		private float     animDuration        = 5.0f;
		private LoopMode  animLoopMode        = LoopMode.Loop;
		private float     animTime            = 0.0f;
		private bool      animPaused          = false;
		private List<int> customFrameSequence = null;

		[DontSerialize] private ICmpSpriteRenderer sprite = null;
		[DontSerialize] private SpriteIndexBlend spriteIndex = new SpriteIndexBlend(0);


		/// <summary>
		/// [GET / SET] The index of the first frame to display. Ignored if <see cref="CustomFrameSequence"/> is set.
		/// </summary>
		/// <remarks>
		/// Animation indices are looked up in the <see cref="Duality.Resources.Pixmap.Atlas"/> map
		/// of the <see cref="Duality.Resources.Texture"/> that is used.
		/// </remarks>
		[EditorHintRange(0, int.MaxValue)]
		public int AnimFirstFrame
		{
			get { return this.animFirstFrame; }
			set
			{
				value = MathF.Max(0, value);
				if (this.animFirstFrame != value)
				{
					this.animFirstFrame = value;
					this.UpdateVisibleFrames();
				}
			}
		}
		/// <summary>
		/// [GET / SET] The number of continous frames to use for the animation. Ignored if <see cref="CustomFrameSequence"/> is set.
		/// </summary>
		/// <remarks>
		/// Animation indices are looked up in the <see cref="Duality.Resources.Pixmap.Atlas"/> map
		/// of the <see cref="Duality.Resources.Texture"/> that is used.
		/// </remarks>
		[EditorHintRange(0, int.MaxValue)]
		public int AnimFrameCount
		{
			get { return this.animFrameCount; }
			set
			{
				value = MathF.Max(0, value);
				if (this.animFrameCount != value)
				{
					this.animFrameCount = value;
					this.UpdateVisibleFrames();
				}
			}
		}
		/// <summary>
		/// [GET / SET] The time a single animation cycle needs to complete, in seconds.
		/// </summary>
		[EditorHintRange(0.0f, float.MaxValue)]
		public float AnimDuration
		{
			get { return this.animDuration; }
			set
			{
				value = MathF.Max(0.0f, value);
				if (this.animDuration != value)
				{
					float lastDuration = this.animDuration;
					this.animDuration = MathF.Max(0.0f, value);
					if (lastDuration != 0.0f && this.animDuration != 0.0f)
						this.animTime *= this.animDuration / lastDuration;
				}
			}
		}
		/// <summary>
		/// [GET / SET] The animations current play time, i.e. the current state of the animation.
		/// </summary>
		[EditorHintRange(0.0f, float.MaxValue)]
		public float AnimTime
		{
			get { return this.animTime; }
			set
			{
				value = MathF.Max(0.0f, value);
				if (this.animTime != value)
				{
					this.animTime = value;
					this.UpdateVisibleFrames();
				}
			}
		}
		/// <summary>
		/// [GET / SET] If true, the animation is paused and won't advance over time. <see cref="AnimTime"/> will stay constant until resumed.
		/// </summary>
		public bool AnimPaused
		{
			get { return this.animPaused; }
			set { this.animPaused = value; }
		}
		/// <summary>
		/// [GET / SET] The animations loop behaviour.
		/// </summary>
		public LoopMode AnimLoopMode
		{
			get { return this.animLoopMode; }
			set { this.animLoopMode = value; }
		}
		/// <summary>
		/// [GET / SET] A custom sequence of frame indices that will be used instead of the default range
		/// specified by <see cref="AnimFirstFrame"/> and <see cref="AnimFrameCount"/>. Unused if set to null.
		/// </summary>
		/// <remarks>
		/// Animation indices are looked up in the <see cref="Duality.Resources.Pixmap.Atlas"/> map
		/// of the <see cref="Duality.Resources.Texture"/> that is used.
		/// </remarks>
		[EditorHintFlags(MemberFlags.ForceWriteback)]
		public List<int> CustomFrameSequence
		{
			get { return this.customFrameSequence; }
			set
			{
				this.customFrameSequence = value;
				this.UpdateVisibleFrames();
			}
		}
		/// <summary>
		/// [GET] Whether the animation is currently running, i.e. if there is anything animated right now.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public bool IsAnimationRunning
		{
			get
			{
				switch (this.animLoopMode)
				{
					case LoopMode.FixedSingle:
					case LoopMode.RandomSingle:
						return false;
					case LoopMode.Loop:
					case LoopMode.PingPong:
						return !this.animPaused;
					case LoopMode.Once:
						return !this.animPaused && this.animTime < this.animDuration;
					case LoopMode.Queue:
						return !this.animPaused && this.customFrameSequence != null && this.customFrameSequence.Count > 1;
					default:
						return false;
				}
			}
		}
		/// <summary>
		/// [GET] The sprite index (blend) that should be displayed by the target sprite.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public SpriteIndexBlend SpriteIndex
		{
			get { return this.spriteIndex; }
		}


		/// <summary>
		/// Updates the <see cref="SpriteIndex"/> property based on the animators current state, and applies the new
		/// <see cref="SpriteIndex"/> to the target sprite.
		/// </summary>
		private void UpdateVisibleFrames()
		{
			int actualFrameBegin = this.customFrameSequence != null ? 0 : this.animFirstFrame;
			int actualFrameCount = this.customFrameSequence != null ? this.customFrameSequence.Count : this.animFrameCount;
			float frameTemp = actualFrameCount * this.animTime / this.animDuration;

			// Calculate visible frames
			this.spriteIndex.Current = 0;
			this.spriteIndex.Next = 0;
			this.spriteIndex.Blend = 0.0f;
			if (actualFrameCount > 0 && this.animDuration > 0)
			{
				// Queued behavior
				if (this.animLoopMode == LoopMode.Queue)
				{
					this.spriteIndex.Current = 0;
					this.spriteIndex.Next = 1;
					this.spriteIndex.Blend = MathF.Clamp(this.animTime / this.animDuration, 0.0f, 1.0f);
				}
				// Non-queued behavior
				else
				{
					// Calculate currently visible frame
					this.spriteIndex.Current = (int)frameTemp;

					// Handle extended frame range for ping pong mode
					if (this.animLoopMode == LoopMode.PingPong)
					{
						if (this.spriteIndex.Current >= actualFrameCount)
							this.spriteIndex.Current = (actualFrameCount - 1) * 2 - this.spriteIndex.Current;
					}

					// Normalize current frame when exceeding anim duration
					if (this.animLoopMode == LoopMode.Once || this.animLoopMode == LoopMode.FixedSingle || this.animLoopMode == LoopMode.RandomSingle)
						this.spriteIndex.Current = MathF.Clamp(this.spriteIndex.Current, 0, actualFrameCount - 1);
					else
						this.spriteIndex.Current = MathF.NormalizeVar(this.spriteIndex.Current, 0, actualFrameCount);

					// Calculate second frame and fade value
					this.spriteIndex.Blend = frameTemp - (int)frameTemp;
					if (this.animLoopMode == LoopMode.Loop)
					{
						this.spriteIndex.Next = MathF.NormalizeVar(this.spriteIndex.Current + 1, 0, actualFrameCount);
					}
					else if (this.animLoopMode == LoopMode.PingPong)
					{
						if ((int)frameTemp < actualFrameCount)
						{
							this.spriteIndex.Next = this.spriteIndex.Current + 1;
							if (this.spriteIndex.Next >= actualFrameCount)
								this.spriteIndex.Next = (actualFrameCount - 1) * 2 - this.spriteIndex.Next;
						}
						else
						{
							this.spriteIndex.Next = this.spriteIndex.Current - 1;
							if (this.spriteIndex.Next < 0)
								this.spriteIndex.Next = -this.spriteIndex.Next;
						}
					}
					else
					{
						this.spriteIndex.Next = this.spriteIndex.Current + 1;
					}
				}
			}
			this.spriteIndex.Current = actualFrameBegin + MathF.Clamp(this.spriteIndex.Current, 0, actualFrameCount - 1);
			this.spriteIndex.Next = actualFrameBegin + MathF.Clamp(this.spriteIndex.Next, 0, actualFrameCount - 1);

			// Map to custom sequence
			if (this.customFrameSequence != null)
			{
				if (this.customFrameSequence.Count > 0)
				{
					this.spriteIndex.Current = this.customFrameSequence[this.spriteIndex.Current];
					this.spriteIndex.Next = this.customFrameSequence[this.spriteIndex.Next];
				}
				else
				{
					this.spriteIndex.Current = 0;
					this.spriteIndex.Next = 0;
				}
			}

			this.ApplySpriteIndex();
		}
		private void ApplySpriteIndex()
		{
			// Retrieve the target sprite if it's unavailable or no longer up-to-date
			if ((this.sprite == null || (this.sprite as Component).GameObj != this.gameobj) && this.gameobj != null)
				this.sprite = this.gameobj.GetComponent<ICmpSpriteRenderer>();

			// Apply the current animation state to the target sprite
			if (this.sprite != null)
				this.sprite.SpriteIndex = this.spriteIndex;
		}

		void ICmpUpdatable.OnUpdate()
		{
			if (!this.IsAnimationRunning) return;
			if (this.animPaused) return;

			int actualFrameBegin = this.customFrameSequence != null ? 0 : this.animFirstFrame;
			int actualFrameCount = this.customFrameSequence != null ? this.customFrameSequence.Count : this.animFrameCount;

			// Advance animation timer
			if (this.animLoopMode == LoopMode.Loop)
			{
				this.animTime += Time.TimeMult * Time.SecondsPerFrame;
				if (this.animTime > this.animDuration)
				{
					int n = (int)(this.animTime / this.animDuration);
					this.animTime -= this.animDuration * n;
				}
			}
			else if (this.animLoopMode == LoopMode.Once)
			{
				this.animTime = MathF.Min(this.animTime + Time.TimeMult * Time.SecondsPerFrame, this.animDuration);
			}
			else if (this.animLoopMode == LoopMode.PingPong)
			{
				float frameTime = this.animDuration / actualFrameCount;
				float pingpongDuration = (this.animDuration - frameTime) * 2.0f;

				this.animTime += Time.TimeMult * Time.SecondsPerFrame;
				if (this.animTime > pingpongDuration)
				{
					int n = (int)(this.animTime / pingpongDuration);
					this.animTime -= pingpongDuration * n;
				}
			}
			else if (this.animLoopMode == LoopMode.Queue)
			{
				this.animTime += Time.TimeMult * Time.SecondsPerFrame;
				if (this.animTime > this.animDuration)
				{
					int n = (int)(this.animTime / this.animDuration);
					this.animTime -= this.animDuration * n;

					if (this.customFrameSequence != null)
					{
						while (n > 0 && this.customFrameSequence.Count > 1)
						{
							this.customFrameSequence.RemoveAt(0);
							n--;
						}
					}
				}
			}

			this.UpdateVisibleFrames();
		}
		void ICmpInitializable.OnInit(Component.InitContext context)
		{
			if (context == InitContext.Loaded)
			{
				if (this.animLoopMode == LoopMode.RandomSingle)
					this.animTime = MathF.Rnd.NextFloat(this.animDuration);
			}
			else if (context == InitContext.Activate)
			{
				this.UpdateVisibleFrames();
			}
		}
		void ICmpInitializable.OnShutdown(Component.ShutdownContext context) {}
		
		protected override void OnSetupCloneTargets(object targetObj, ICloneTargetSetup setup)
		{
			base.OnSetupCloneTargets(targetObj, setup);
			SpriteAnimator target = targetObj as SpriteAnimator;

			setup.HandleObject(this.customFrameSequence, target.customFrameSequence);
		}
		protected override void OnCopyDataTo(object targetObj, ICloneOperation operation)
		{
			base.OnCopyDataTo(targetObj, operation);
			SpriteAnimator target = targetObj as SpriteAnimator;

			target.animFirstFrame		= this.animFirstFrame;
			target.animFrameCount		= this.animFrameCount;
			target.animDuration			= this.animDuration;
			target.animLoopMode			= this.animLoopMode;
			target.animTime				= this.animTime;
			target.animPaused			= this.animPaused;

			operation.HandleObject(this.customFrameSequence, ref target.customFrameSequence);
		}
	}
}
