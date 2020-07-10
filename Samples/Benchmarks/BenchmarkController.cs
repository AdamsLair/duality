using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Duality;
using Duality.IO;
using Duality.Editor;
using Duality.Input;
using Duality.Resources;
using Duality.Components;

namespace Duality.Samples.Benchmarks
{
	public class BenchmarkController
	{
		private static BenchmarkController instance = null;
		public static BenchmarkController Instance
		{
			get
			{
				if (instance == null)
					instance = new BenchmarkController();
				return instance;
			}
		}


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

		private bool testRunActive = false;
		private int testRunSampleIndex = 0;
		private float testRunSampleTimer = 0.0f;
		private StringBuilder testRunDataCollector = null;

		
		private void SwitchToSample(int index)
		{
			// Determine which scene we're going to switch to, and
			// don't do it if we're already there.
			ContentRef<Scene> targetScene = this.benchmarkScenes[index];
			if (Scene.Current == targetScene) return;

			// Force reload of current sample later on by disposing it.
			Scene.Current.DisposeLater();
			// Switch to new sample
			Scene.SwitchTo(targetScene);
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

		private void StartStandardTestRun()
		{
			Logs.Game.Write("Starting benchmark test run.");
			this.testRunActive = true;
			this.testRunSampleIndex = 0;
			this.testRunSampleTimer = 0.0f;
			this.testRunDataCollector = new StringBuilder();
			this.StartTestRunSample();
		}
		private void StartTestRunSample()
		{
			Logs.Game.Write("Sample {0}, index {1}...", 
				this.benchmarkScenes[this.testRunSampleIndex].FullName, 
				this.testRunSampleIndex);
			this.SwitchToSample(this.testRunSampleIndex);
		}
		private void CompleteTestRun()
		{
			Logs.Game.Write("Benchmark test run complete.");

			string reportFileName = "BenchmarkTestReport.txt";
			string report = this.testRunDataCollector.ToString();

			Logs.Game.Write("Saving report to '{0}'...", reportFileName);
			using (Stream stream = FileOp.Create(reportFileName))
			using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8))
			{
				writer.Write(report);
			}

			this.testRunActive = false;
			this.testRunSampleIndex = 0;
			this.testRunSampleTimer = 0.0f;
			this.SwitchToSample(0);
		}
		private void RetrieveTestRunData()
		{
			Logs.Game.Write("Retrieving test run data.");

			this.testRunDataCollector.AppendFormat("### Sample {0}, index {1} ###", this.benchmarkScenes[this.testRunSampleIndex].Name, this.testRunSampleIndex);
			this.testRunDataCollector.AppendLine();
			this.testRunDataCollector.AppendLine();

			// Find the renderer that displays the perf stats and ask it to
			// append its text data to our collector.
			PerfStatsRenderer perfStats = Scene.Current.FindComponent<PerfStatsRenderer>();
			perfStats.ExportText(this.testRunDataCollector);

			this.testRunDataCollector.AppendLine();
			this.testRunDataCollector.AppendLine();
		}

		private void UpdateTestRun()
		{
			this.testRunSampleTimer += Time.DeltaTime;
			if (this.testRunSampleTimer > 15.0f)
			{
				this.testRunSampleTimer = 0.0f;
				this.RetrieveTestRunData();

				this.testRunSampleIndex++;
				if (this.testRunSampleIndex < this.benchmarkScenes.Count)
					this.StartTestRunSample();
				else
					this.CompleteTestRun();
			}
		}
		private void UpdateManual()
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

			// Pressing the T key: Start the standard test run
			if (DualityApp.Keyboard.KeyHit(Key.T))
				this.StartStandardTestRun();
		}

		public void EnterBenchmarkMode()
		{
			// Retrieve a list of all available scenes to cycle through.
			this.benchmarkScenes = ContentProvider.GetAvailableContent<Scene>();
			this.renderSetup = ContentProvider.GetAvailableContent<BenchmarkRenderSetup>().FirstOrDefault();

			// Make sure the benchmark setup is used globally
			DualityApp.AppData.Instance.RenderingSetup = this.renderSetup.As<RenderSetup>();
		}
		public void LeaveBenchmarkMode()
		{
			// Uninstall the benchmark setup we set globally
			DualityApp.AppData.Instance.RenderingSetup = null;

			// Discard local references to content, since we know the controller 
			// itself won't be discarded due to being static
			this.benchmarkScenes.Clear();
			this.renderSetup = null;
		}
		public void Update()
		{
			this.renderSetup.Res.DisplayTestRunActive = this.testRunActive;
			if (this.testRunActive)
				this.UpdateTestRun();
			else
				this.UpdateManual();
		}
	}
}
