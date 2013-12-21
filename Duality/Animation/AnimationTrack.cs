using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Duality.Animation
{
	/// <summary>
	/// Describes the animation of a value as a set of <see cref="AnimationKeyFrame{T}">keyframes</see>.
	/// </summary>
	/// <typeparam name="T">Type of the animated value.</typeparam>
	public class AnimationTrack<T> : IEnumerable<AnimationKeyFrame<T>>, IAnimationTrack
	{
        private RawList<AnimationKeyFrame<T>>	keyFrames	= new RawList<AnimationKeyFrame<T>>();
        private bool							loop		= false;


		/// <summary>
		/// [GET / SET] The set of keyframes that is used to describe the animation.
		/// </summary>
        public IEnumerable<AnimationKeyFrame<T>> KeyFrames
        {
            get { return this.keyFrames; }
            set
			{
				if (this.keyFrames != value)
				{
					this.keyFrames.Data = value.ToArray();
					this.keyFrames.Count = this.keyFrames.Data.Length;
					this.ValidateKeyFrames();
				}
			}
        }
		/// <summary>
		/// [GET] The total number of keyframes in this track.
		/// </summary>
		public int KeyFrameCount
		{
			get { return this.keyFrames.Count; }
		}
		/// <summary>
		/// [GET / SET] Describes whether or not this track is considered to be a loop.
		/// </summary>
        public bool IsLooping
        {
            get { return this.loop; }
            set { this.loop = value; }
        }
		/// <summary>
		/// [GET] Returns the total duration of this track in seconds, i.e. the last keyframes time value.
		/// </summary>
		public float Duration
		{
			get { return this.keyFrames.Data[this.keyFrames.Count - 1].Time; }
		}
		/// <summary>
		/// [GET / SET] The animated value at a certain time. Setting this value may
		/// introduce new keyframes or modify existing ones.
		/// </summary>
		/// <param name="time">Time in seconds.</param>
		/// <returns></returns>
		public T this[float time]
		{
			get { return this.GetValue(time); }
			set { this.Add(time, value); }
		}
		/// <summary>
		/// [GET / SET] The keyframe at a certain index.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public AnimationKeyFrame<T> this[int index]
		{
			get { return this.keyFrames[index]; }
			set { this.keyFrames[index] = value; }
		}


		public AnimationTrack() {}
        public AnimationTrack(IEnumerable<AnimationKeyFrame<T>> frames)
        {
			if (frames == null) throw new ArgumentNullException("frames");
			this.AddRange(frames);
        }
        public AnimationTrack(params T[] evenlyDistributedValues)
        {
			if (evenlyDistributedValues == null) throw new ArgumentNullException("evenlyDistributedValues");

			// Just one value? Add only this one.
			if (evenlyDistributedValues.Length == 1)
			{
				this.Add(0.0f, evenlyDistributedValues[0]);
				return;
			}

			// Otherwise, evenly distribute them along [0 ... 1]
			for (int i = 0; i < evenlyDistributedValues.Length; i++)
			{
				this.Add((float)i / (float)(evenlyDistributedValues.Length - 1), evenlyDistributedValues[i]);
			}
        }
        public AnimationTrack(IEnumerable<T> evenlyDistributedValues) : this(evenlyDistributedValues.ToArray()) {}

		/// <summary>
		/// Adds a range of keyframes to the track. They do not need to be sorted 
		/// or at the end of the track, as they will be sorted anyway.
		/// </summary>
		/// <param name="frames"></param>
		public void AddRange(IEnumerable<AnimationKeyFrame<T>> frames)
		{
			this.keyFrames.AddRange(frames);
			this.ValidateKeyFrames();
		}
		/// <summary>
		/// Adds a single keyframe to the track. You may use this method to add
		/// keyframes at arbitrary times.
		/// </summary>
		/// <param name="frame"></param>
		public void Add(AnimationKeyFrame<T> frame)
		{
			int insertIndex = this.SearchIndexBelow(frame.Time) + 1;

			if (insertIndex >= this.keyFrames.Count)
				this.keyFrames.Add(frame);
			else if (this.keyFrames.Data[insertIndex].Time == frame.Time)
				this.keyFrames.Data[insertIndex] = frame;
			else
				this.keyFrames.Insert(insertIndex, frame);

			if (insertIndex == 0 && frame.Time > 0.0f)
				this.keyFrames.Insert(0, new AnimationKeyFrame<T>(0.0f, frame.Value));
		}
		/// <summary>
		/// Adds a single keyframe to the track. You may use this method to add
		/// keyframes at arbitrary times.
		/// </summary>
		/// <param name="time">Time in seconds.</param>
		/// <param name="value"></param>
        public void Add(float time, T value)
        {
			this.Add(new AnimationKeyFrame<T>(time, value));
        }
		/// <summary>
		/// Removes a single keyframe from the track.
		/// </summary>
		/// <param name="time">Time in seconds.</param>
		public void Remove(float time)
		{
			this.keyFrames.RemoveAll(f => f.Time == time);
		}
		/// <summary>
		/// Clears the track from all keyframes.
		/// </summary>
		public void Clear()
		{
			this.keyFrames.Clear();
		}

		/// <summary>
		/// Scales the animation to use the specified amount of time for completing one cycle.
		/// </summary>
		/// <param name="duration"></param>
		public void ScaleToDuration(float duration)
		{
			float oldDuration = this.Duration;
			for (int i = 0; i < this.keyFrames.Count; i++)
			{
				this.keyFrames.Data[i].Time *= duration / oldDuration;
			}
		}

		/// <summary>
		/// Returns the animated value at the specified time.
		/// </summary>
		/// <param name="time">Time in seconds.</param>
		/// <returns></returns>
        public T GetValue(float time)
        {
            if (this.keyFrames.Count == 0) throw new InvalidOperationException("Can't interpolate on empty AnimationTrack.");

			AnimationKeyFrame<T>[] data = this.keyFrames.Data;
			int frameCount = this.KeyFrameCount;
			float duration = this.Duration;
            if (this.loop) time = MathF.NormalizeVar(time, 0.0f, duration);

			int baseIndex = this.SearchIndexBelow(time);
			if (baseIndex < 0) return data[0].Value;
			if (baseIndex == frameCount - 1 && !this.loop) return data[frameCount - 1].Value;

			int nextIndex = (baseIndex + 1) % frameCount;
			float nextTime = data[nextIndex].Time;
			if (nextTime < data[baseIndex].Time) nextTime += duration;

			float factor = (time - data[baseIndex].Time) / (nextTime - data[baseIndex].Time);
			return GenericOperator.Lerp(data[baseIndex].Value, data[nextIndex].Value, factor);
        }
		/// <summary>
		/// Assigns a value at the specified time. This may introduce new keyframes or modify existing ones.
		/// </summary>
		/// <param name="time">Time in seconds.</param>
		/// <param name="value"></param>
		public void SetValue(float time, T value)
		{
			this.Add(time, value);
		}
        public override string ToString()
        {
            return this.keyFrames.ToString(", ");
        }
		

		private int SearchIndexBelow(float time)
		{
			int left = 0;
			int right = this.keyFrames.Count - 1;
			AnimationKeyFrame<T>[] array = this.keyFrames.Data;
			while (right >= left)
			{
				int mid = (left + right) / 2;
				float midTime = array[mid].Time;
				if (midTime > time)
				{
					right = mid - 1;
				}
				else if (midTime <= time)
				{
					if (left != mid)
					{
						left = mid;
					}
					else
					{
						float rightTime = array[right].Time;
						if (rightTime <= time)
							return right;
						else
							return left;
					}
				}
				else if (left == right)
				{
					break;
				}
			}
			return -1;
		}
		private void ValidateKeyFrames()
		{
			for (int i = this.keyFrames.Count - 1; i >= 0; i--)
			{
				if (this.keyFrames.Data[i].Time < 0.0f)
					this.keyFrames.RemoveAt(i);
			}
			this.keyFrames.Sort();
			if (this.keyFrames.Data[0].Time > 0.0f)
			{
				this.keyFrames.Insert(0, new AnimationKeyFrame<T>(0.0f, this.keyFrames.Data[0].Value));
			}
		}


		IEnumerator<AnimationKeyFrame<T>> IEnumerable<AnimationKeyFrame<T>>.GetEnumerator()
		{
			return this.keyFrames.GetEnumerator();
		}
		IEnumerator<IAnimationKeyFrame> IEnumerable<IAnimationKeyFrame>.GetEnumerator()
		{
			return this.keyFrames.Cast<IAnimationKeyFrame>().GetEnumerator();
		}
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.keyFrames.GetEnumerator();
		}

		IEnumerable<IAnimationKeyFrame> IAnimationTrack.KeyFrames
		{
			get { return this.KeyFrames.Cast<IAnimationKeyFrame>(); }
			set { this.KeyFrames = value.Cast<AnimationKeyFrame<T>>(); }
		}
		object IAnimationTrack.this[float time]
		{
			get { return this.GetValue(time); }
			set { this.SetValue(time, (T)value); }
		}
		void IAnimationTrack.AddRange(IEnumerable<IAnimationKeyFrame> frames)
		{
			this.AddRange(frames.Cast<AnimationKeyFrame<T>>());
		}
		void IAnimationTrack.Add(IAnimationKeyFrame frame)
		{
			this.Add((AnimationKeyFrame<T>)frame);
		}
		void IAnimationTrack.Add<U>(float time, U value)
		{
			this.Add(time, GenericOperator.Convert<U,T>(value));
		}
		U IAnimationTrack.GetValue<U>(float time)
		{
			return GenericOperator.Convert<T,U>(this.GetValue(time));
		}
		void IAnimationTrack.SetValue<U>(float time, U value)
		{
			this.SetValue(time, GenericOperator.Convert<U,T>(value));
		}
	}
}
