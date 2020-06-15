using System;
using BenchmarkDotNet.Running;

namespace DualityBenchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
			BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
		}
    }
}
