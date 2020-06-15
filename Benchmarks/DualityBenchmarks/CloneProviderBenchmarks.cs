using System;
using BenchmarkDotNet.Attributes;
using Duality.Cloning;
using Duality.Tests.Serialization;

namespace DualityBenchmarks
{
	public class CloneProviderBenchmarks
	{
		[Benchmark]
		public void CloneTestObjectGraph()
		{
			Random rnd = new Random(0);
			TestObject data = new TestObject(rnd, 5);
			TestObject[] results = new TestObject[200];

			for (int i = 0; i < results.Length; i++)
			{
				results[i] = data.DeepClone();
			}
		}

		[Benchmark(Baseline = true)]
		public void CreateWithoutClone()
		{
			Random rnd = new Random(0);
			TestObject[] results = new TestObject[200];
			for (int i = 0; i < results.Length; i++)
			{
				results[i] = new TestObject(rnd, 5);
			}
		}
	}
}