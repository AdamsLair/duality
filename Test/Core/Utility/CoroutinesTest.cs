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
		private TimeSpan testSpan = TimeSpan.FromSeconds(5);
		private string testSignal = "#SIGNAL#";
		private int testValue = 0;

		[Test] public void Coroutines()
		{
			Scene scene = new Scene();
			scene.Activate();

			Coroutine c = Coroutine.Start(scene, BasicTest());
			Assert.AreEqual(10, this.x);
			Assert.False(c.IsComplete); // first yield
			scene.Update();
			Assert.AreEqual(20, this.x);
			Assert.False(c.IsComplete); // second yield
			scene.Update();
			Assert.True(c.IsComplete); // stopAction
			scene.Update();

			ManualResetEvent resetEvent = new ManualResetEvent(false);
			DateTime endTime = DateTime.Now + this.testSpan;
			Coroutine t = Coroutine.Start(scene, TaskTest(resetEvent));

			Coroutine.Start(scene, SignalWaitTest());
			Coroutine.Start(scene, SignalEmitTest());
			scene.Update();

			while (!resetEvent.WaitOne((int)MathF.Ceiling(Time.MillisecondsPerFrame)))
			{
				Time.FrameTick(true, true);
				scene.Update();
			}

			scene.Update();
			Assert.AreEqual(50, this.testValue);

			Assert.True(t.IsComplete);
			Assert.GreaterOrEqual(DateTime.Now, endTime);

			Coroutine one = Coroutine.Start(scene, WaitMultipleTest());
			scene.Update();
			Assert.True(one.IsComplete);
		}

		private int x;
		private IEnumerable<CoroutineAction> BasicTest()
		{
			this.x = 10;
			yield return CoroutineAction.GetOne<WaitForFrames>().Setup(1);
			this.x = 20;
			yield return CoroutineAction.GetOne<WaitForFrames>().Setup(1);
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

		private IEnumerable<CoroutineAction> SignalWaitTest()
		{
			yield return CoroutineAction.GetOne<WaitForSignal>().Setup(this.testSignal);
			this.testValue *= 10;
		}

		private IEnumerable<CoroutineAction> SignalEmitTest()
		{
			this.testValue = 5;
			yield return CoroutineAction.GetOne<EmitSignal>().Setup(this.testSignal);
		}

		private IEnumerable<CoroutineAction> WaitMultipleTest()
		{
			yield return CoroutineAction.GetOne<WaitOne>().SetupAsParams(
				CoroutineAction.GetOne<WaitForFrames>().Setup(1),
				CoroutineAction.GetOne<WaitForTime>().Setup(TimeSpan.FromSeconds(1)));
		}
	}
}
