using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Duality.Resources;
using Duality.Utility.Coroutines;
using NUnit.Framework;

namespace Duality.Tests.Utility
{
	class CoroutineObject
	{
		public int Value { get; set; }
	}

	[TestFixture]
	public class CoroutinesTest
	{
		[Test] public void Basics()
		{
			CoroutineManager cm = new CoroutineManager();

			CoroutineObject obj = new CoroutineObject();
			Coroutine coroutine = cm.StartNew(this.BasicRoutine(obj));

			// All code until first yield is already executed
			Assert.AreEqual(10, obj.Value);
			Assert.True(coroutine.Status == CoroutineStatus.Running);

			cm.Update();

			// All code until second yield is executed
			Assert.AreEqual(20, obj.Value);
			Assert.True(coroutine.Status == CoroutineStatus.Running);

			cm.Update();

			// Yelded null, value didn't change, need to wait one more update
			Assert.AreEqual(20, obj.Value);
			Assert.True(coroutine.Status == CoroutineStatus.Running);

			cm.Update();
			// Yielded condition is waiting for two frames now..
			Assert.AreEqual(20, obj.Value);
			Assert.True(coroutine.Status == CoroutineStatus.Running);

			cm.Update();
			// All remaining code has been executed
			Assert.AreEqual(30, obj.Value);
			Assert.True(coroutine.Status == CoroutineStatus.Complete);

			cm.Update();

			// No further changes
			Assert.AreEqual(30, obj.Value);
			Assert.True(coroutine.Status == CoroutineStatus.Complete);
		}

		[Test] public void Cancelling()
		{
			CoroutineManager cm = new CoroutineManager();

			CoroutineObject obj = new CoroutineObject();
			Coroutine coroutine = cm.StartNew(this.BasicRoutine(obj));

			// All code until first yield is already executed
			Assert.AreEqual(10, obj.Value);
			Assert.True(coroutine.Status == CoroutineStatus.Running);

			cm.Update();

			// All code until second yield is executed
			Assert.AreEqual(20, obj.Value);
			Assert.True(coroutine.Status == CoroutineStatus.Running);

			coroutine.Cancel();
			cm.Update();

			// Coroutine disposed
			Assert.AreEqual(20, obj.Value);
			Assert.True(coroutine.Status == CoroutineStatus.Cancelled);

			cm.Update();

			// No further changes
			Assert.AreEqual(20, obj.Value);
			Assert.True(coroutine.Status == CoroutineStatus.Cancelled);
		}

		[Test] public void Resuming()
		{
			CoroutineManager cm = new CoroutineManager();

			CoroutineObject obj = new CoroutineObject();
			Coroutine coroutine = cm.StartNew(this.BasicRoutine(obj));

			// All code until first yield is already executed
			Assert.AreEqual(10, obj.Value);
			Assert.True(coroutine.Status == CoroutineStatus.Running);

			coroutine.Pause();
			cm.Update();

			// No changes, since the coroutine is now paused
			Assert.AreEqual(10, obj.Value);
			Assert.True(coroutine.Status == CoroutineStatus.Paused);

			int rnd = MathF.Rnd.Next(ushort.MaxValue);
			for(int i = 0; i < rnd; i++)
				cm.Update();

			// No matter how many updates
			Assert.AreEqual(10, obj.Value);
			Assert.True(coroutine.Status == CoroutineStatus.Paused);

			coroutine.Resume();
			cm.Update();

			// All code until second yield is executed
			Assert.AreEqual(20, obj.Value);
			Assert.True(coroutine.Status == CoroutineStatus.Running);
		}

		[Test] public void WaitTwoSeconds()
		{
			CoroutineManager cm = new CoroutineManager();

			int secondsToWait = 2;
			CoroutineObject obj = new CoroutineObject();
			Coroutine coroutine = cm.StartNew(this.WaitSeconds(obj, secondsToWait));

			// All code until first yield is already executed
			Assert.AreEqual(10, obj.Value);
			TimeSpan startTime = Time.GameTimer;

			// Waiting...
			while (obj.Value == 10)
			{
				cm.Update();
				Time.FrameTick(true, true);
			}

			double secondsElapsed = (Time.GameTimer - startTime).TotalSeconds;

			Assert.AreEqual(20, obj.Value);

			// Not being overly precise about the 2 seconds wait..
			Assert.GreaterOrEqual(secondsElapsed, secondsToWait);
			Assert.LessOrEqual(secondsElapsed, secondsToWait + Time.SecondsPerFrame * 10);

			Assert.True(coroutine.Status == CoroutineStatus.Complete);
		}

		[Test] public void Subroutines()
		{
			CoroutineManager cm = new CoroutineManager();
			CoroutineObject obj = new CoroutineObject();

			cm.StartNew(this.CoroutineMaster(obj));

			Assert.AreEqual(0, obj.Value);
			cm.Update();

			// at this point obj.Value should be 1, as it has been updated by the slave coroutine
			Assert.AreEqual(1, obj.Value);

			for (int i = 2; i <= 100; i++)
			{
				cm.Update();
				Assert.AreEqual(i, obj.Value);
			}

			// the slave coroutine ended, the master coroutine is stopped at [1], and the object's value should be 100
			Assert.AreEqual(100, obj.Value);

			// one last update, and it's reset to 0
			cm.Update();
			Assert.AreEqual(0, obj.Value);
		}

		[Test] public void Exception()
		{
			CoroutineManager cm = new CoroutineManager();
			Coroutine coroutine = cm.StartNew(this.ExceptionRoutine());

			// All code until first yield is already executed
			Assert.True(coroutine.Status == CoroutineStatus.Running);

			cm.Update();
			// All code until second yield is executed
			Assert.True(coroutine.Status == CoroutineStatus.Running);

			// Exception thrown, a log should appear
			cm.Update();
			Assert.True(coroutine.Status == CoroutineStatus.Error);
		}

		private IEnumerable<WaitUntil> BasicRoutine(CoroutineObject obj)
		{
			obj.Value = 10;
			yield return WaitUntil.NextFrame;
			obj.Value = 20;
			yield return WaitUntil.NextFrame;
			yield return WaitUntil.Frames(2);
			obj.Value = 30;

		}

		private IEnumerable<WaitUntil> ExceptionRoutine()
		{
			yield return WaitUntil.Frames(2);
			throw new Exception("Test exception");
		}

		private IEnumerable<WaitUntil> WaitSeconds(CoroutineObject obj, int seconds)
		{
			obj.Value = 10;
			yield return WaitUntil.Seconds(seconds);
			obj.Value = 20;
		}

		private IEnumerable<WaitUntil> CoroutineMaster(CoroutineObject obj)
		{
			obj.Value = 0;
			yield return WaitUntil.NextFrame;
			yield return WaitUntil.CoroutineEnds(this.CoroutineSlave(obj)); // [1]
			obj.Value = 0;
		}

		private IEnumerable<WaitUntil> CoroutineSlave(CoroutineObject obj)
		{
			while (obj.Value < 100)
			{
				obj.Value++;
				yield return WaitUntil.NextFrame;
			}
		}
	}
}
