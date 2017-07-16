using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Duality;
using Duality.Editor;
using Duality.Input;
using Duality.Resources;
using Duality.Components;

namespace Duality.Samples.Benchmarks
{
	[EditorHintCategory("Benchmarks")]
    public class BenchmarkController : Component, ICmpInitializable, ICmpUpdatable
	{
		[DontSerialize] private List<ContentRef<Scene>> benchmarkScenes;

		
		private void SwitchToSample(int index)
		{
			// Force reload of current sample later on by disposing it.
			this.GameObj.ParentScene.DisposeLater();
			// Switch to new sample
			Scene.SwitchTo(this.benchmarkScenes[index]);
		}
		private void AdvanceSampleBy(int indexOffset)
		{
			// Determine the current samples' index and advance it
			int currentIndex = this.benchmarkScenes.IndexOf(this.GameObj.ParentScene);
			int newIndex = (currentIndex + indexOffset + this.benchmarkScenes.Count) % this.benchmarkScenes.Count;
			this.SwitchToSample(newIndex);
		}


		void ICmpUpdatable.OnUpdate()
		{
			// Pressing an arrow key: Switch benchmark scenes
			if (DualityApp.Keyboard.KeyHit(Key.Left) || DualityApp.Keyboard.KeyHit(Key.Up))
			{
				this.AdvanceSampleBy(-1);
			}
			else if (DualityApp.Keyboard.KeyHit(Key.Right) || DualityApp.Keyboard.KeyHit(Key.Down))
			{
				this.AdvanceSampleBy(1);
			}
		}

		void ICmpInitializable.OnInit(Component.InitContext context)
		{
			if (context == InitContext.Activate && DualityApp.ExecContext == DualityApp.ExecutionContext.Game)
			{
				// Retrieve a list of all available scenes to cycle through.
				this.benchmarkScenes = ContentProvider.GetAvailableContent<Scene>();
			}
		}
		void ICmpInitializable.OnShutdown(Component.ShutdownContext context) { }
	}
}
