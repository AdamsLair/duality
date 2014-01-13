using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;

using Duality;
using Duality.Serialization;
using Duality.Serialization.MetaFormat;

using OpenTK;
using NUnit.Framework;

namespace DualityTests.Serialization
{
	public abstract class FormatterTest
	{
		private struct TestData : IEquatable<TestData>
		{
			public int IntField;
			public float FloatField;

			public TestData(Random rnd)
			{
				this.IntField	= rnd.Next();
				this.FloatField	= rnd.NextFloat();
			}

			public static bool operator ==(TestData first, TestData second)
			{
				return first.Equals(second);
			}
			public static bool operator !=(TestData first, TestData second)
			{
				return !first.Equals(second);
			}
			public override bool Equals(object obj)
			{
				if (obj is TestData)
					return this.Equals((TestData)obj);
				else
					return base.Equals(obj);
			}
			public bool Equals(TestData other)
			{
				return 
					other.IntField == this.IntField &&
					other.FloatField == this.FloatField;
			}
		}
		private class TestObject
		{
			public string StringField;
			public TestData DataField;
			public List<int> ListField;
			public List<string> ListField2;
			public Dictionary<string,TestObject> DictField;
			
			public TestObject(Random rnd, int maxChildren = 5)
			{
				this.StringField	= rnd.Next().ToString();
				this.DataField		= new TestData(rnd);
				this.ListField		= Enumerable.Range(rnd.Next(-1000, 1000), rnd.Next(0, 50)).ToList();
				this.ListField2		= Enumerable.Range(rnd.Next(-1000, 1000), rnd.Next(0, 50)).Select(i => i.ToString()).ToList();
				this.DictField		= new Dictionary<string,TestObject>();

				for (int i = rnd.Next(0, maxChildren); i > 0; i--)
				{
					this.DictField.Add(rnd.Next().ToString(), new TestObject(rnd, maxChildren / 2));
				}
			}

			public static bool operator ==(TestObject first, TestObject second)
			{
				return first.Equals(second);
			}
			public static bool operator !=(TestObject first, TestObject second)
			{
				return !first.Equals(second);
			}
			public override bool Equals(object obj)
			{
				if (obj is TestObject)
					return this.Equals((TestObject)obj);
				else
					return base.Equals(obj);
			}
			public bool Equals(TestObject other)
			{
				return 
					other.StringField == this.StringField &&
					other.DataField == this.DataField &&
					other.ListField.SequenceEqual(this.ListField) &&
					other.ListField2.SequenceEqual(this.ListField2) &&
					other.DictField.SetEqual(this.DictField);
			}
		}
		
		[Test] public void SerializePlainOldData()
		{
			Random rnd = new Random();
			TestData data = new TestData(rnd);
			TestData dataResult;

			this.TestWriteRead(data, out dataResult, false);

			Assert.IsTrue(data.Equals(dataResult));
		}
		[Test] public void SerializeComplexObject()
		{
			Random rnd = new Random();
			TestObject data = new TestObject(rnd);
			TestObject dataResult;

			this.TestWriteRead(data, out dataResult, false);

			Assert.IsTrue(data.Equals(dataResult));
		}
		[Test] public void SerializeMetaPlainOldData()
		{
			Random rnd = new Random();
			TestData data = new TestData(rnd);
			TestData dataResult;

			this.TestWriteRead(data, out dataResult, true);

			Assert.IsTrue(data.Equals(dataResult));
		}
		[Test] public void SerializeMetaComplexObject()
		{
			Random rnd = new Random();
			TestObject data = new TestObject(rnd);
			TestObject dataResult;

			this.TestWriteRead(data, out dataResult, true);

			Assert.IsTrue(data.Equals(dataResult));
		}

		protected abstract Formatter CreateFormatter(Stream stream);
		protected abstract Formatter CreateMetaFormatter(Stream stream);
		private void TestWriteRead<T>(T writeObj, out T readObj, bool testMeta)
		{
			MemoryStream stream = new MemoryStream();
			{
				Formatter formatterWrite = this.CreateFormatter(stream);
				formatterWrite.WriteObject(writeObj);
				formatterWrite.Dispose();
			}
			stream.Seek(0, SeekOrigin.Begin);
			if (testMeta)
			{
				DataNode metaNode = null;
				{
					Formatter formatterRead = this.CreateMetaFormatter(stream);
					metaNode = (DataNode)formatterRead.ReadObject();
					formatterRead.Dispose();
				}
				stream.Seek(0, SeekOrigin.Begin);
				stream.SetLength(0);
				{
					Formatter formatterWrite = this.CreateMetaFormatter(stream);
					formatterWrite.WriteObject(metaNode);
					formatterWrite.Dispose();
				}
			}
			stream.Seek(0, SeekOrigin.Begin);
			{
				Formatter formatterRead = this.CreateFormatter(stream);
				readObj = (T)formatterRead.ReadObject();
				formatterRead.Dispose();
			}
			stream.Dispose();
			stream = null;
		}
	}
}
