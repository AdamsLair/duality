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
		private Random rnd;
		private TestObject[] results;
		private byte[] readData;
		private TestObject data;

		[GlobalSetup]
		public void Setup()
		{
			this.rnd = new Random(0);
			this.results = new TestObject[50];
			this.data = new TestObject(this.rnd, 5);

			using (MemoryStream stream = new MemoryStream())
			{
				for (int i = 0; i < this.results.Length; i++)
				{
					using (Serializer formatterWrite = Serializer.Create(stream, this.type))
					{
						formatterWrite.WriteObject(this.data);
					}
				}
				this.readData = stream.GetBuffer();
			}
		}

		[Benchmark]
		public void Write()
		{
			using (MemoryStream stream = new MemoryStream())
			{
				for (int i = 0; i < this.results.Length; i++)
				{
					using (Serializer formatterWrite = Serializer.Create(stream, this.type))
					{
						formatterWrite.WriteObject(this.data);
					}
				}
			}
		}

		[Benchmark]
		public void Read()
		{
			using (MemoryStream stream = new MemoryStream(this.readData))
			{
				for (int i = 0; i < this.results.Length; i++)
				{
					using (Serializer formatterRead = Serializer.Create(stream))
					{
						this.results[i] = formatterRead.ReadObject<TestObject>();
					}
				}
			}
		}
	}
}
