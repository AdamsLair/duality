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
	public class BenchmarkController
	{
		private List<Point2> renderSizes = new List<Point2>
		{
			new Point2(320, 300), 
			new Point2(800, 600), 
			new Point2(1024, 768), 
			new Point2(1920, 1080)
		};
		private List<float> renderResolutionScales = new List<float>
		{
			0.0f, 
			0.5f, 
			1.0f, 
			2.0f, 
			4.0f
		};
		private List<AAQuality> renderAAQualityLevels = new List<AAQuality>
		{
			AAQuality.Off,
			AAQuality.Low,
			AAQuality.Medium,
			AAQuality.High
		};

		[DontSerialize] private ContentRef<BenchmarkRenderSetup> renderSetup;
		[DontSerialize] private List<ContentRef<Scene>> benchmarkScenes;

		
		private void SwitchToSample(int index)
		{
			// Force reload of current sample later on by disposing it.
			Scene.Current.DisposeLater();
			// Switch to new sample
			Scene.SwitchTo(this.benchmarkScenes[index]);
		}
		private void AdvanceSampleBy(int indexOffset)
		{
			// Determine the current samples' index and advance it
			int currentIndex = this.benchmarkScenes.IndexOf(Scene.Current);
			int newIndex = (currentIndex + indexOffset + this.benchmarkScenes.Count) % this.benchmarkScenes.Count;
			this.SwitchToSample(newIndex);
		}
		private void CycleRenderSize()
		{
			BenchmarkRenderSetup setup = this.renderSetup.Res;
			int currentIndex = this.renderSizes.IndexOf(setup.RenderingSize);
			int newIndex = (currentIndex + 1 + this.renderSizes.Count) % this.renderSizes.Count;
			setup.RenderingSize = this.renderSizes[newIndex];
		}
		private void CycleResolutionScale()
		{
			BenchmarkRenderSetup setup = this.renderSetup.Res;
			int currentIndex = this.renderResolutionScales.IndexOf(setup.ResolutionScale);
			int newIndex = (currentIndex + 1 + this.renderResolutionScales.Count) % this.renderResolutionScales.Count;
			setup.ResolutionScale = this.renderResolutionScales[newIndex];
		}
		private void CycleAntialiasingQuality()
		{
			BenchmarkRenderSetup setup = this.renderSetup.Res;
			int currentIndex = this.renderAAQualityLevels.IndexOf(setup.AntialiasingQuality);
			int newIndex = (currentIndex + 1 + this.renderAAQualityLevels.Count) % this.renderAAQualityLevels.Count;
			setup.AntialiasingQuality = this.renderAAQualityLevels[newIndex];
		}


		public void PrepareBenchmarks()
		{
			// Retrieve a list of all available scenes to cycle through.
			this.benchmarkScenes = ContentProvider.GetAvailableContent<Scene>();
			this.renderSetup = ContentProvider.GetAvailableContent<BenchmarkRenderSetup>().FirstOrDefault();

			// Make sure the benchmark setup is used globally
			DualityApp.AppData.RenderingSetup = this.renderSetup.As<RenderSetup>();
		}
		public void Update()
		{
			// Pressing an arrow key: Switch benchmark scenes
			if (DualityApp.Keyboard.KeyHit(Key.Left) || DualityApp.Keyboard.KeyHit(Key.Up))
				this.AdvanceSampleBy(-1);
			else if (DualityApp.Keyboard.KeyHit(Key.Right) || DualityApp.Keyboard.KeyHit(Key.Down))
				this.AdvanceSampleBy(1);

			// Pressing the S key: Cycle resolution scaling
			if (DualityApp.Keyboard.KeyHit(Key.S))
				this.CycleResolutionScale();

			// Pressing the W key: Cycle rendering size
			if (DualityApp.Keyboard.KeyHit(Key.W))
				this.CycleRenderSize();

			// Pressing the A key: Cycle AA quality
			if (DualityApp.Keyboard.KeyHit(Key.A))
				this.CycleAntialiasingQuality();
		}
	}
}
