using BenchmarkDotNet.Attributes;
using Duality.Resources;
using Duality;
using Duality.Components;
using Duality.Components.Renderers;

namespace DualityBenchmarks
{
	[MemoryDiagnoser]
	public class PrefabBenchmarks
	{
		private Prefab prefab;

		[GlobalSetup]
		public void Setup()
		{
			this.prefab = new Prefab(this.CreateSimpleGameObject());
		}

		[Benchmark(Baseline = true)]
		public GameObject CreateWithoutPrefabs()
		{
			return this.CreateSimpleGameObject();
		}

		[Benchmark]
		public GameObject Instantiate()
		{
			return this.prefab.Instantiate();
		}

		private GameObject CreateSimpleGameObject(GameObject parent = null)
		{
			GameObject obj = new GameObject("SimpleObject", parent);
			obj.AddComponent<Transform>();
			obj.AddComponent<SpriteRenderer>();
			return obj;
		}
	}
}
