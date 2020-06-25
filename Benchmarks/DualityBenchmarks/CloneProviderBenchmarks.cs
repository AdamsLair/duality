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
		private TestObject data;

		[GlobalSetup]
		public void Setup()
		{
			this.rnd = new Random(0);
			this.data = new TestObject(this.rnd, 5);
		}

		[Benchmark]
		public TestObject CloneTestObjectGraph()
		{
			return this.data.DeepClone();
		}

		[Benchmark(Baseline = true)]
		public TestObject CreateWithoutClone()
		{
			return new TestObject(this.rnd, 5);
		}
	}
}