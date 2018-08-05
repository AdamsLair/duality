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

			Coroutine coroutine = Coroutine.Start(scene, BasicRoutine());

			// All code until first yield is already executed
			Assert.AreEqual(10, this.x);
			Assert.False(coroutine.IsComplete);

			scene.Update();

			// All code until second yield is executed
			Assert.AreEqual(20, this.x);
			Assert.False(coroutine.IsComplete);

			scene.Update();

			// All remaining code has been executed
			Assert.AreEqual(30, this.x);
			Assert.True(coroutine.IsComplete);

			scene.Update();

			// No further changes
			Assert.AreEqual(30, this.x);
			Assert.True(coroutine.IsComplete);
		}
		[Test] public void WaitOne()
		{
			Scene scene = new Scene();
			scene.Activate();

			Coroutine coroutine = Coroutine.Start(scene, WaitOneRoutine());
			scene.Update();
			Assert.True(coroutine.IsComplete);
		}

		private int x = 0;
		private IEnumerable<CoroutineAction> BasicRoutine()
		{
			this.x = 10;
			yield return CoroutineAction.GetOne<WaitForFrames>().Setup(1);
			this.x = 20;
			yield return CoroutineAction.GetOne<WaitForFrames>().Setup(1);
			this.x = 30;
		}

		private IEnumerable<CoroutineAction> TaskTest(ManualResetEvent resetEvent)
		{
			/*
			yield return new LongRunningTask(new Task(() =>
			{
				Thread.Sleep(this.testSpan);
				resetEvent.Set();
			}));
			*/

			yield return CoroutineAction.GetOne<WaitForTime>().Setup(this.testSpan);
			resetEvent.Set();
		}
		private IEnumerable<CoroutineAction> WaitOneRoutine()
		{
			yield return CoroutineAction.GetOne<WaitOne>().SetupAsParams(
				CoroutineAction.GetOne<WaitForFrames>().Setup(1),
				CoroutineAction.GetOne<WaitForTime>().Setup(TimeSpan.FromSeconds(1)));
		}
	}
}
