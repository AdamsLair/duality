using System.Diagnostics;
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

			if (Debugger.IsAttached)
			{
				BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args, new DebugInProcessConfig());
			}
			else {
				BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
			}			
		}
    }
}
