using System;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using Duality;
using Duality.Backend;

namespace DualityBenchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
			// Initialize Duality
			DualityApp.Init(
				DualityApp.ExecutionEnvironment.Launcher,
				DualityApp.ExecutionContext.Game,
				new DefaultAssemblyLoader(),
				null);

			BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args, new DebugInProcessConfig());
		}
    }
}
