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
		private GameObject[] results;

		[GlobalSetup]
		public void Setup()
		{
			this.prefab = new Prefab(this.CreateSimpleGameObject());
			this.results = new GameObject[1000];
		}

		[Benchmark(Baseline = true)]
		public void CreateWithoutPrefabs()
		{
			for (int i = 0; i < this.results.Length; i++)
			{
				this.CreateSimpleGameObject();
			}
		}

		[Benchmark]
		public void Instantiate()
		{
			for (int i = 0; i < this.results.Length; i++)
			{
				this.results[i] = this.prefab.Instantiate();
			}
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
