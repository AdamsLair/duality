using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Duality;

using NUnit.Framework;

namespace Duality.Tests.Utility
{
	[TestFixture]
	public class MathFTest
	{
		private const float Epsilon = 0.0001f;

		[Test] public void Clamp01Checks()
		{
			Assert.AreEqual(MathF.Clamp01(-0.0f), 0.0f, Epsilon);
			Assert.AreEqual(MathF.Clamp01(0.0f), 0.0f, Epsilon);
			Assert.AreEqual(MathF.Clamp01(0.001f), 0.001f, Epsilon);
			Assert.AreEqual(MathF.Clamp01(1.0001f), 1.0f, Epsilon);
			Assert.AreEqual(MathF.Clamp01(1.0f), 1.0f, Epsilon);
			Assert.AreEqual(MathF.Clamp01(0.999f), 0.999f, Epsilon);
			Assert.AreEqual(MathF.Clamp01(0.567f), 0.567f, Epsilon);

		}

		[Test] public void InvLerpChecks()
		{
			Assert.AreEqual(MathF.InvLerp(0.0f, 1.0f, 0.25f), 0.25f, Epsilon);
			Assert.AreEqual(MathF.InvLerp(0.0f, 1.0f, 0.0f), 0.0f, Epsilon);
			Assert.AreEqual(MathF.InvLerp(0.0f, 1.0f, 1.0f), 1.0f, Epsilon);
			Assert.AreEqual(MathF.InvLerp(0.0f, 1.0f, 1.5f), 1.5f, Epsilon);
			Assert.AreEqual(MathF.InvLerp(0.0f, 1.0f, -0.5f), -.5f, Epsilon);

			Assert.AreEqual(MathF.InvLerp(0, 100, 25), 0.25f, Epsilon);
			Assert.AreEqual(MathF.InvLerp(0, 100, 0), 0.0f, Epsilon);
			Assert.AreEqual(MathF.InvLerp(0, 100, 100), 1.0f, Epsilon);
			Assert.AreEqual(MathF.InvLerp(0, 100, 150), 1.5f, Epsilon);
			Assert.AreEqual(MathF.InvLerp(0, 100, -50), -0.5f, Epsilon);
		}

		[Test] public void SmoothStepChecks()
		{
			Assert.AreEqual(MathF.SmoothStep(0.25f), 0.15625f, Epsilon);
			Assert.AreEqual(MathF.SmoothStep(0.0f), 0.0f, Epsilon);
			Assert.AreEqual(MathF.SmoothStep(1.0f), 1.0f, Epsilon);
			Assert.AreEqual(MathF.SmoothStep(-0.0001f), 0.0f, Epsilon);
			Assert.AreEqual(MathF.SmoothStep(1.0001f), 1.0f, Epsilon);

			Assert.AreEqual(MathF.SmoothStep(10.0f, 20.0f, 15.0f), 0.5f, Epsilon);
			Assert.AreEqual(MathF.SmoothStep(10.0f, 20.0f, 10.0f), 0.0f, Epsilon);
			Assert.AreEqual(MathF.SmoothStep(10.0f, 20.0f, 20.0f), 1.0f, Epsilon);
			Assert.AreEqual(MathF.SmoothStep(10.0f, 20.0f, 9.999f), 0.0f, Epsilon);
			Assert.AreEqual(MathF.SmoothStep(10.0f, 20.0f, 20.001f), 1.0f, Epsilon);
		}
	}
}
