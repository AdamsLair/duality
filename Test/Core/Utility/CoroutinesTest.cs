using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Duality.Resources;
using NUnit.Framework;

namespace Duality.Tests.Utility
{
	[TestFixture]
	public class CoroutinesTest
	{
		[Test] public void Basics()
		{
			Scene scene = new Scene();
			scene.Activate();

			Coroutine coroutine = Coroutine.Start(scene, BasicRoutine(), "basic");

			// All code until first yield is already executed
			Assert.AreEqual(10, this.x);
			Assert.True(coroutine.Status == CoroutineStatus.Running);

			scene.Update();

			// All code until second yield is executed
			Assert.AreEqual(20, this.x);
			Assert.True(coroutine.Status == CoroutineStatus.Running);

			scene.Update();

			// Yelded null, value didn't change, need to wait one more update
			Assert.AreEqual(20, this.x);
			Assert.True(coroutine.Status == CoroutineStatus.Running);

			scene.Update();
			// Yielded condition is waiting for two frames now..
			Assert.AreEqual(20, this.x);
			Assert.True(coroutine.Status == CoroutineStatus.Running);

			scene.Update();
			// All remaining code has been executed
			Assert.AreEqual(30, this.x);
			Assert.True(coroutine.Status == CoroutineStatus.Complete);

			scene.Update();

			// No further changes
			Assert.AreEqual(30, this.x);
			Assert.True(coroutine.Status == CoroutineStatus.Complete);
		}

		[Test] public void Disposing()
		{
			Scene scene = new Scene();
			scene.Activate();

			Coroutine coroutine = Coroutine.Start(scene, BasicRoutine(), "disposable");

			// All code until first yield is already executed
			Assert.AreEqual(10, this.x);
			Assert.True(coroutine.Status == CoroutineStatus.Running);

			scene.Update();

			// All code until second yield is executed
			Assert.AreEqual(20, this.x);
			Assert.True(coroutine.Status == CoroutineStatus.Running);

			coroutine.Dispose();
			scene.Update();

			// Coroutine disposed
			Assert.AreEqual(20, this.x);
			Assert.True(coroutine.Status == CoroutineStatus.Disposed);

			scene.Update();

			// No further changes
			Assert.AreEqual(20, this.x);
			Assert.True(coroutine.Status == CoroutineStatus.Disposed);
		}

		[Test] public void Resuming()
		{
			Scene scene = new Scene();
			scene.Activate();

			Coroutine coroutine = Coroutine.Start(scene, BasicRoutine(), "resuming");

			// All code until first yield is already executed
			Assert.AreEqual(10, this.x);
			Assert.True(coroutine.Status == CoroutineStatus.Running);

			coroutine.Pause();
			scene.Update();

			// No changes, since the coroutine is now paused
			Assert.AreEqual(10, this.x);
			Assert.True(coroutine.Status == CoroutineStatus.Paused);

			scene.Update();
			scene.Update();
			scene.Update();

			// No matter how many updates
			Assert.AreEqual(10, this.x);
			Assert.True(coroutine.Status == CoroutineStatus.Paused);

			coroutine.Resume();
			scene.Update();

			// All code until second yield is executed
			Assert.AreEqual(20, this.x);
			Assert.True(coroutine.Status == CoroutineStatus.Running);
		}

		[Test] public void WaitTwoSeconds()
		{
			Scene scene = new Scene();
			scene.Activate();

			int secondsToWait = 2;
			Coroutine coroutine = Coroutine.Start(scene, WaitSeconds(secondsToWait), "waiting");

			// All code until first yield is already executed
			Assert.AreEqual(10, this.x);
			TimeSpan startTime = Time.GameTimer;

			// Waiting...
			while (this.x == 10)
			{
				scene.Update();
				Time.FrameTick(true, true);
			}

			double secondsElapsed = (Time.GameTimer - startTime).TotalSeconds;

			Assert.AreEqual(20, this.x);

			// Not being overly precise about the 2 seconds wait..
			Assert.GreaterOrEqual(secondsElapsed, secondsToWait);
			Assert.LessOrEqual(secondsElapsed, secondsToWait + Time.SecondsPerFrame * 10);

			Assert.True(coroutine.Status == CoroutineStatus.Complete);
		}

		[Test] public void Exception()
		{
			Scene scene = new Scene();
			scene.Activate();

			Coroutine coroutine = Coroutine.Start(scene, ExceptionRoutine(), "exception");

			// All code until first yield is already executed
			Assert.True(coroutine.Status == CoroutineStatus.Running);

			scene.Update();
			// All code until second yield is executed
			Assert.True(coroutine.Status == CoroutineStatus.Running);

			// Exception thrown, a log should appear
			scene.Update();
			Assert.True(coroutine.Status == CoroutineStatus.Error);
		}

		private int x = 0;
		private IEnumerable<IWaitCondition> BasicRoutine()
		{
			this.x = 10;
			yield return new WaitFrames(1);
			this.x = 20;
			yield return null;
			yield return new WaitFrames(2);
			this.x = 30;

		}

		private IEnumerable<IWaitCondition> ExceptionRoutine()
		{
			yield return null;
			yield return null;
			throw new Exception("Test exception");
		}

		private IEnumerable<IWaitCondition> WaitSeconds(int seconds)
		{
			this.x = 10;
			yield return new WaitTime(TimeSpan.FromSeconds(seconds));
			this.x = 20;
		}
	}
}
