using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Duality.Animation
{
	public interface IAnimationTrack : IEnumerable<IAnimationKeyFrame>
	{
        IEnumerable<IAnimationKeyFrame> KeyFrames { get; set; }
		int KeyFrameCount { get; }
        bool IsLooping { get; set; }
		float Duration { get; }
		object this[float time] { get; set; }

		void AddRange(IEnumerable<IAnimationKeyFrame> frames);
		void Add(IAnimationKeyFrame frame);
        void Add<T>(float time, T value);
		void Remove(float time);
		void Clear();

        T GetValue<T>(float time);
		void SetValue<T>(float time, T value);
	}
}
