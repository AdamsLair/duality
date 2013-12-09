using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Duality
{
	public class AnimationTrack<T> : IEnumerable<AnimationTrack<T>.KeyFrame> where T : struct
	{
        private List<KeyFrame>	keyFrames	= new List<KeyFrame>();
        private bool			loop		= false;


        public IEnumerable<KeyFrame> KeyFrames
        {
            get { return this.keyFrames; }
            set
			{
				if (this.keyFrames != value)
				{
					this.keyFrames = (value != null) ? value.ToList() : new List<KeyFrame>();
					this.ValidateKeyFrames();
				}
			}
        }
		public int KeyFrameCount
		{
			get { return this.keyFrames.Count; }
		}
        public bool IsLooping
        {
            get { return this.loop; }
            set { this.loop = value; }
        }
		public float Duration
		{
			get { return this.keyFrames[this.keyFrames.Count - 1].Time; }
		}
		public T this[float time]
		{
			get { return this.GetValueAt(time); }
		}


		public AnimationTrack() {}
        public AnimationTrack(IEnumerable<KeyFrame> frames)
        {
			this.AddRange(frames);
        }

		public void AddRange(IEnumerable<KeyFrame> frames)
		{
			this.keyFrames.AddRange(frames);
			this.ValidateKeyFrames();
		}
		public void Add(KeyFrame frame)
		{
			int insertIndex = this.BinarySearchIndexBelow(frame.Time) + 1;
			if (insertIndex >= this.keyFrames.Count)
				this.keyFrames.Add(frame);
			else
				this.keyFrames.Insert(insertIndex, frame);

			if (insertIndex == 0 && frame.Time > 0.0f)
				this.keyFrames.Insert(0, new KeyFrame(0.0f, frame.Value));
		}
        public void Add(float time, T value)
        {
			this.Add(new KeyFrame(time, value));
        }
		public void RemoveAt(float time)
		{
			this.keyFrames.RemoveAll(f => f.Time == time);
		}
		public void Clear()
		{
			this.keyFrames.Clear();
		}

        public T GetValueAt(float time)
        {
            if (this.keyFrames.Count == 0) throw new InvalidOperationException("Can't interpolate on empty AnimationTrack.");

			int frameCount = this.KeyFrameCount;
			float duration = this.Duration;
            if (this.loop) time = MathF.NormalizeVar(time, 0.0f, duration);

			int baseIndex = this.BinarySearchIndexBelow(time);
			if (baseIndex < 0) return this.keyFrames[0].Value;
			if (baseIndex == frameCount - 1 && !this.loop) return this.keyFrames[frameCount - 1].Value;

			KeyFrame keyBefore = this.keyFrames[baseIndex];
			KeyFrame keyAfter = this.keyFrames[(baseIndex + 1) % frameCount];
			if (keyAfter.Time < keyBefore.Time) keyAfter.Time += duration;

			float factor = (time - keyBefore.Time) / (keyAfter.Time - keyBefore.Time);
			return GenericOperator.Lerp(keyBefore.Value, keyAfter.Value, factor);
        }
        public override string ToString()
        {
            return this.keyFrames.ToString(", ");
        }
		
		private int BinarySearchIndexBelow(float time)
		{
			int left = 0;
			int right = this.keyFrames.Count - 1;
			while (right >= left)
			{
				int mid = (left + right) / 2;
				float midTime = this.keyFrames[mid].Time;
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
						float rightTime = this.keyFrames[right].Time;
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
				if (this.keyFrames[i].Time < 0.0f)
					this.keyFrames.RemoveAt(i);
			}
			this.keyFrames.Sort();
			if (this.keyFrames[0].Time > 0.0f)
			{
				this.keyFrames.Insert(0, new KeyFrame(0.0f, this.keyFrames[0].Value));
			}
		}

		IEnumerator<AnimationTrack<T>.KeyFrame> IEnumerable<AnimationTrack<T>.KeyFrame>.GetEnumerator()
		{
			return this.keyFrames.GetEnumerator();
		}
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.keyFrames.GetEnumerator();
		}

		public struct KeyFrame : IComparable<KeyFrame>
		{
			public float Time;
			public T Value;

			public KeyFrame(float time, T value)
			{
				this.Time = time;
				this.Value = value;
			}

			int IComparable<KeyFrame>.CompareTo(KeyFrame other)
			{
				return this.Time.CompareTo(other.Time);
			}

			public override string ToString()
			{
				return string.Format("[{0:F}: {1:F}]", this.Time, this.Value);
			}
		}
	}
}
