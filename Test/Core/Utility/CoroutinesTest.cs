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
			Scene scene = new Scene();
			scene.Activate();

			CoroutineObject obj = new CoroutineObject();
			Coroutine coroutine = scene.StartCoroutine(this.BasicRoutine(obj), "basic");

			// All code until first yield is already executed
			Assert.AreEqual(10, obj.Value);
			Assert.True(coroutine.Status == CoroutineStatus.Running);

			scene.Update();

			// All code until second yield is executed
			Assert.AreEqual(20, obj.Value);
			Assert.True(coroutine.Status == CoroutineStatus.Running);

			scene.Update();

			// Yelded null, value didn't change, need to wait one more update
			Assert.AreEqual(20, obj.Value);
			Assert.True(coroutine.Status == CoroutineStatus.Running);

			scene.Update();
			// Yielded condition is waiting for two frames now..
			Assert.AreEqual(20, obj.Value);
			Assert.True(coroutine.Status == CoroutineStatus.Running);

			scene.Update();
			// All remaining code has been executed
			Assert.AreEqual(30, obj.Value);
			Assert.True(coroutine.Status == CoroutineStatus.Complete);

			scene.Update();

			// No further changes
			Assert.AreEqual(30, obj.Value);
			Assert.True(coroutine.Status == CoroutineStatus.Complete);
		}

		[Test] public void Disposing()
		{
			Scene scene = new Scene();
			scene.Activate();

			CoroutineObject obj = new CoroutineObject();
			Coroutine coroutine = scene.StartCoroutine(this.BasicRoutine(obj), "disposable");

			// All code until first yield is already executed
			Assert.AreEqual(10, obj.Value);
			Assert.True(coroutine.Status == CoroutineStatus.Running);

			scene.Update();

			// All code until second yield is executed
			Assert.AreEqual(20, obj.Value);
			Assert.True(coroutine.Status == CoroutineStatus.Running);

			coroutine.Dispose();
			scene.Update();

			// Coroutine disposed
			Assert.AreEqual(20, obj.Value);
			Assert.True(coroutine.Status == CoroutineStatus.Disposed);

			scene.Update();

			// No further changes
			Assert.AreEqual(20, obj.Value);
			Assert.True(coroutine.Status == CoroutineStatus.Disposed);
		}

		[Test] public void Resuming()
		{
			Scene scene = new Scene();
			scene.Activate();

			CoroutineObject obj = new CoroutineObject();
			Coroutine coroutine = scene.StartCoroutine(this.BasicRoutine(obj), "resuming");

			// All code until first yield is already executed
			Assert.AreEqual(10, obj.Value);
			Assert.True(coroutine.Status == CoroutineStatus.Running);

			coroutine.Pause();
			scene.Update();

			// No changes, since the coroutine is now paused
			Assert.AreEqual(10, obj.Value);
			Assert.True(coroutine.Status == CoroutineStatus.Paused);

			scene.Update();
			scene.Update();
			scene.Update();

			// No matter how many updates
			Assert.AreEqual(10, obj.Value);
			Assert.True(coroutine.Status == CoroutineStatus.Paused);

			coroutine.Resume();
			scene.Update();

			// All code until second yield is executed
			Assert.AreEqual(20, obj.Value);
			Assert.True(coroutine.Status == CoroutineStatus.Running);
		}

		[Test] public void WaitTwoSeconds()
		{
			Scene scene = new Scene();
			scene.Activate();

			int secondsToWait = 2;
			CoroutineObject obj = new CoroutineObject();
			Coroutine coroutine = scene.StartCoroutine(this.WaitSeconds(obj, secondsToWait), "waiting");

			// All code until first yield is already executed
			Assert.AreEqual(10, obj.Value);
			TimeSpan startTime = Time.GameTimer;

			// Waiting...
			while (obj.Value == 10)
			{
				scene.Update();
				Time.FrameTick(true, true);
			}

			double secondsElapsed = (Time.GameTimer - startTime).TotalSeconds;

			Assert.AreEqual(20, obj.Value);

			// Not being overly precise about the 2 seconds wait..
			Assert.GreaterOrEqual(secondsElapsed, secondsToWait);
			Assert.LessOrEqual(secondsElapsed, secondsToWait + Time.SecondsPerFrame * 10);

			Assert.True(coroutine.Status == CoroutineStatus.Complete);
		}

		[Test] public void Exception()
		{
			Scene scene = new Scene();
			scene.Activate();

			Coroutine coroutine = scene.StartCoroutine(this.ExceptionRoutine(), "exception");

			// All code until first yield is already executed
			Assert.True(coroutine.Status == CoroutineStatus.Running);

			scene.Update();
			// All code until second yield is executed
			Assert.True(coroutine.Status == CoroutineStatus.Running);

			// Exception thrown, a log should appear
			scene.Update();
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
			yield return WaitUntil.Seconds(2);
			obj.Value = 20;
		}
	}
}
