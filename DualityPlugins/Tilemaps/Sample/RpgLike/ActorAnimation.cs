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
	/// Describes an animation that can be used by an <see cref="ActorAnimator"/> to animate an <see cref="ActorRenderer"/>.
	/// </summary>
	public class ActorAnimation
	{
		private string                 name              = "Empty Animation";
		private AnimDirMapping[]       startFrame        = new AnimDirMapping[0];
		private int                    frameCount        = 0;
		private float                  duration          = 2.0f;
		private ActorAnimator.LoopMode preferredLoopMode = ActorAnimator.LoopMode.Loop;

		/// <summary>
		/// [GET / SET] The name of this animation.
		/// </summary>
		public string Name
		{
			get { return this.name; }
			set { this.name = value; }
		}
		/// <summary>
		/// [GET / SET] For each animation direction, this array provides
		/// the sprite sheet index at which the animation starts.
		/// </summary>
		public AnimDirMapping[] DirectionMap
		{
			get { return this.startFrame; }
			set { this.startFrame = value ?? new AnimDirMapping[0];}
		}
		/// <summary>
		/// [GET / SET] The number of frames in this animation.
		/// </summary>
		public int FrameCount
		{
			get { return this.frameCount; }
			set { this.frameCount = value; }
		}
		/// <summary>
		/// [GET / SET] The duration of this animation, in seconds.
		/// </summary>
		public float Duration
		{
			get { return this.duration; }
			set { this.duration = value; }
		}
		/// <summary>
		/// [GET / SET] The loop mode that is preferred by this animation.
		/// </summary>
		public ActorAnimator.LoopMode PreferredLoopMode
		{
			get { return this.preferredLoopMode; }
			set { this.preferredLoopMode = value; }
		}

		public override string ToString()
		{
			return string.Format("{0} ({1} frames)", this.name, this.frameCount);
		}
	}
}
