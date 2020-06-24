using System;
using System.IO;
using Duality.Tests.Serialization;
using Duality.Serialization;
using BenchmarkDotNet.Attributes;

namespace DualityBenchmarks
{
	[MemoryDiagnoser]
	public class SerializerBenchmarks
	{
		[Params(typeof(XmlSerializer), typeof(BinarySerializer))]
		public Type type;
		private TestObject[] results;
		private byte[] readData;
		private TestObject data;

		[GlobalSetup]
		public void Setup()
		{
			this.results = new TestObject[50];
			this.data = new TestObject(new Random(0), 5);

			this.readData = this.Write().GetBuffer();
		}

		[Benchmark]
		public MemoryStream Write()
		{
			MemoryStream stream = new MemoryStream();
			for (int i = 0; i < this.results.Length; i++)
			{
				using (Serializer formatter = Serializer.Create(stream, this.type))
				{
					formatter.WriteObject(this.data);
				}
			}
			return stream;
		}

		[Benchmark]
		public void Read()
		{
			MemoryStream stream = new MemoryStream(this.readData);
			for (int i = 0; i < this.results.Length; i++)
			{
				using (Serializer formatter = Serializer.Create(stream))
				{
					this.results[i] = formatter.ReadObject<TestObject>();
				}
			}
		}
	}
}
