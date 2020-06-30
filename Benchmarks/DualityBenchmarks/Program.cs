using System;
using System.Diagnostics;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;

namespace DualityBenchmarks
{
	class Program
    {
        static void Main(string[] args)
        {
			if (Debugger.IsAttached)
			{
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine($"[WARNING] Running with debugger attached, using {nameof(DebugInProcessConfig)} this might affect benchmark results!");
				Console.ResetColor();
				BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args, new DebugInProcessConfig());
			}
			else {
				BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
			}			
		}
    }
}
