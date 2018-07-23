using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Duality.Tests.Utility
{
	
	[TestFixture]
	public class CoroutinesTest
	{
		private TimeSpan testSpan = TimeSpan.FromSeconds(5);
		private string testSignal = "#SIGNAL#";
		private int testValue = 0;

		[Test] public void Tests()
		{
			Coroutine c = Coroutine.Start(BasicTest());
			CoroutineManager.Update();
			Assert.AreEqual(10, this.x);
			Assert.False(c.IsComplete); // first yield
			CoroutineManager.Update();
			Assert.AreEqual(20, this.x);
			Assert.False(c.IsComplete); // second yield
			CoroutineManager.Update();
			Assert.True(c.IsComplete); // stopAction
			CoroutineManager.Update();

			ManualResetEvent resetEvent = new ManualResetEvent(false);
			DateTime endTime = DateTime.Now + this.testSpan;
			Coroutine t = Coroutine.Start(TaskTest(resetEvent));

			Coroutine.Start(SignalWaitTest());
			Coroutine.Start(SignalEmitTest());
			CoroutineManager.Update();

			while (!resetEvent.WaitOne((int)Time.MillisecondsPerFrame))
			{
				Time.FrameTick(true, true);
				CoroutineManager.Update();
			}

			CoroutineManager.Update();
			Assert.AreEqual(50, this.testValue);

			Assert.True(t.IsComplete);
			Assert.GreaterOrEqual(DateTime.Now, endTime);
		}

		private int x;
		private IEnumerable<ICoroutineAction> BasicTest()
		{
			this.x = 10;
			yield return new WaitForFrames(1);
			this.x = 20;
			yield return new WaitForFrames(1);
		}

		private IEnumerable<ICoroutineAction> TaskTest(ManualResetEvent resetEvent)
		{
			/*
			yield return new LongRunningTask(new Task(() =>
			{
				Thread.Sleep(this.testSpan);
				resetEvent.Set();
			}));
			*/

			yield return new WaitForTime(this.testSpan);
			resetEvent.Set();
		}

		private IEnumerable<ICoroutineAction> SignalWaitTest()
		{
			yield return new WaitForSignal(this.testSignal);
			this.testValue *= 10;
		}

		private IEnumerable<ICoroutineAction> SignalEmitTest()
		{
			this.testValue = 5;
			yield return new EmitSignal(this.testSignal);
		}
	}
}
