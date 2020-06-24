using BenchmarkDotNet.Attributes;
using Duality;
using Duality.Components;
using Duality.Components.Renderers;
using Duality.Cloning;
using Duality.Components.Physics;

namespace DualityBenchmarks
{
	[MemoryDiagnoser]
	public class SpecificCloningBenchmarks
	{
		private GameObject data;

		[GlobalSetup]
		public void Setup()
		{
			this.data = new GameObject("CloneRoot");
			for (int i = 0; i < 1000; i++)
			{
				GameObject child = new GameObject("Child", this.data);
				child.AddComponent<Transform>();
				if (i % 3 != 0) child.AddComponent<SpriteRenderer>();
				if (i % 3 == 0) child.AddComponent<RigidBody>();
				if (i % 7 == 0) child.AddComponent<TextRenderer>();
			}
		}

		[Benchmark]
		public void CreateWithoutClone()
		{
			var data = new GameObject("CloneRoot");
			for (int i = 0; i < 1000; i++)
			{
				GameObject child = new GameObject("Child", data);
				child.AddComponent<Transform>();
				if (i % 3 != 0) child.AddComponent<SpriteRenderer>();
				if (i % 3 == 0) child.AddComponent<RigidBody>();
				if (i % 7 == 0) child.AddComponent<TextRenderer>();
			}
		}

		[Benchmark]
		public GameObject CloneGameObjectGraph()
		{
			return this.data.DeepClone();
		}
	}
}
