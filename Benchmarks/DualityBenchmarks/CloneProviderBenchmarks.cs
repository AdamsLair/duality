using System;
using BenchmarkDotNet.Attributes;
using Duality.Cloning;
using Duality.Tests.Serialization;

namespace DualityBenchmarks
{
	[MemoryDiagnoser]
	public class CloneProviderBenchmarks
	{
		private Random rnd;
		private TestObject[] results;
		private TestObject data;

		[GlobalSetup]
		public void Setup()
		{
			this.rnd = new Random(0);
			this.results = new TestObject[200];
			this.data = new TestObject(this.rnd, 5);
		}

		[Benchmark]
		public void CloneTestObjectGraph()
		{
			for (int i = 0; i < this.results.Length; i++)
			{
				this.results[i] = this.data.DeepClone();
			}
		}

		[Benchmark(Baseline = true)]
		public void CreateWithoutClone()
		{
			for (int i = 0; i < this.results.Length; i++)
			{
				this.results[i] = new TestObject(this.rnd, 5);
			}
		}
	}
}