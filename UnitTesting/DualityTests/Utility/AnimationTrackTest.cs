using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

using Duality;
using Duality.Animation;
using Duality.Drawing;

using OpenTK;

using NUnit.Framework;

namespace Duality.Tests.Utility
{
	[TestFixture]
	public class AnimationTrackTest
	{
		[Test] public void Basics()
		{
			AnimationTrack<float> track;
			
			track = new AnimationTrack<float>(0.0f, 1.0f);
			for (int i = 0; i <= 100; i++)
			{
				float time = (float)i / 100.0f;
				Assert.AreEqual(time, track[time], 0.001f);
			}
			track.ScaleToDuration(10.0f);
			for (int i = 0; i <= 100; i++)
			{
				float time = (float)i / 100.0f;
				Assert.AreEqual(time, track[10.0f * time], 0.001f);
			}

			track = new AnimationTrack<float>
			{
				{ 0.5f, 10.0f },
				{ 1.0f, 20.0f },
				{ 1.5f, 30.0f }
			};
			Assert.AreEqual(10.0f, track[0.0f], 0.001f);
			Assert.AreEqual(30.0f, track[2.0f], 0.001f);

			track.IsLooping = true;
			Assert.AreEqual(10.0f, track[0.0f], 0.001f);
			Assert.AreEqual(10.0f, track[2.0f], 0.001f);
		}
		[Test] public void IntAnimation()
		{
			AnimationTrack<int> track;
			
			track = new AnimationTrack<int>(0, 100);
			for (int i = 0; i <= 100; i++)
			{
				float time = (float)i / 100.0f + 0.0001f;
				Assert.AreEqual(i, track[time]);
			}

			Assert.AreEqual(1, track[0.011f]);
			Assert.AreEqual(1, track[0.013f]);
			Assert.AreEqual(1, track[0.019f]);
			Assert.AreEqual(2, track[0.021f]);
		}
		[Test] public void Vector3Animation()
		{
			AnimationTrack<Vector3> track;
			
			track = new AnimationTrack<Vector3>(Vector3.Zero, Vector3.One);
			Assert.AreEqual(0.0f, track[0.0f].X, 0.0001f);
			Assert.AreEqual(0.0f, track[0.0f].Y, 0.0001f);
			Assert.AreEqual(0.0f, track[0.0f].Z, 0.0001f);
			Assert.AreEqual(0.5f, track[0.5f].X, 0.0001f);
			Assert.AreEqual(0.5f, track[0.5f].Y, 0.0001f);
			Assert.AreEqual(0.5f, track[0.5f].Z, 0.0001f);
			Assert.AreEqual(1.0f, track[1.0f].X, 0.0001f);
			Assert.AreEqual(1.0f, track[1.0f].Y, 0.0001f);
			Assert.AreEqual(1.0f, track[1.0f].Z, 0.0001f);
		}
		[Test] public void ColorRgbaAnimation()
		{
			AnimationTrack<ColorRgba> track;
			
			track = new AnimationTrack<ColorRgba>(ColorRgba.Red, ColorRgba.Green);
			Assert.AreEqual(ColorRgba.Red, track[0.0f]);
			Assert.AreEqual(ColorRgba.Lerp(ColorRgba.Red, ColorRgba.Green, 0.5f), track[0.5f]);
			Assert.AreEqual(ColorRgba.Green, track[1.0f]);
		}
	}
}
