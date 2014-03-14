using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.IO;
using System.Text;

using Duality;
using Duality.Serialization;
using Duality.Tests.Properties;

using OpenTK;
using NUnit.Framework;

namespace Duality.Tests.Serialization
{
	[TestFixture(FormattingMethod.Xml)]
	[TestFixture(FormattingMethod.Binary)]
	public class FormatterTest
	{
		private FormattingMethod format;

		private FormattingMethod PrimaryFormat
		{
			get { return this.format; }
		}
		private IEnumerable<FormattingMethod> OtherFormats
		{
			get { return Enum.GetValues(typeof(FormattingMethod)).Cast<FormattingMethod>().Where(m => m != FormattingMethod.Unknown && m != this.PrimaryFormat); }
		}

		public FormatterTest(FormattingMethod format)
		{
			this.format = format;
		}


		[Test] public void SerializePlainOldData()
		{
			Random rnd = new Random();

			this.TestWriteRead(rnd.NextBool(),			this.PrimaryFormat);
			this.TestWriteRead(rnd.NextByte(),			this.PrimaryFormat);
			this.TestWriteRead(rnd.Next(),				this.PrimaryFormat);
			this.TestWriteRead(rnd.NextFloat(),			this.PrimaryFormat);
			this.TestWriteRead(rnd.NextDouble(),		this.PrimaryFormat);
			this.TestWriteRead(rnd.Next().ToString(),	this.PrimaryFormat);
			this.TestWriteRead((SomeEnum)rnd.Next(10),	this.PrimaryFormat);
		}
		[Test] public void SerializeFlatStruct()
		{
			Random rnd = new Random();
			this.TestWriteRead(new TestData(rnd), this.PrimaryFormat);
		}
		[Test] public void SerializeObjectTree()
		{
			Random rnd = new Random();
			this.TestWriteRead(new TestObject(rnd), this.PrimaryFormat);
		}
		[Test] public void SequentialAccess()
		{
			Random rnd = new Random();
			TestObject dataA = new TestObject(rnd);
			TestObject dataB = new TestObject(rnd);

			this.TestSequential(dataA, dataB, this.PrimaryFormat);
		}
		[Test] public void RandomAccess()
		{
			Random rnd = new Random();
			TestObject dataA = new TestObject(rnd);
			TestObject dataB = new TestObject(rnd);

			this.TestRandomAccess(dataA, dataB, this.PrimaryFormat);
		}
		[Test] public void BlendInOtherData()
		{
			Random rnd = new Random();

			string		rawDataA	= "Hello World";
			long		rawDataB	= 17;
			TestObject	data		= new TestObject(rnd);

			string		rawDataResultA;
			long		rawDataResultB;
			TestObject	dataResult;

			using (MemoryStream stream = new MemoryStream())
			using (Formatter formatter = Formatter.Create(stream, this.PrimaryFormat))
			{
				using (BinaryWriter binWriter = new BinaryWriter(stream.NonClosing()))
				{
					binWriter.Write(rawDataA);
					formatter.WriteObject(data);
					binWriter.Write(rawDataB);
				}

				stream.Position = 0;
				using (BinaryReader binReader = new BinaryReader(stream.NonClosing()))
				{
					rawDataResultA = binReader.ReadString();
					formatter.ReadObject(out dataResult);
					rawDataResultB = binReader.ReadInt64();
				}
			}

			Assert.IsTrue(rawDataA.Equals(rawDataResultA));
			Assert.IsTrue(rawDataB.Equals(rawDataResultB));
			Assert.IsTrue(data.Equals(dataResult));
		}
		[Test] public void ConvertFormat([ValueSource("OtherFormats")] FormattingMethod to)
		{
			Random rnd = new Random();
			TestObject data = new TestObject(rnd);
			TestObject dataResult;

			using (MemoryStream stream = new MemoryStream())
			{
				// Write old format
				using (Formatter formatterWrite = Formatter.Create(stream, this.PrimaryFormat))
				{
					formatterWrite.WriteObject(data);
				}

				// Read
				stream.Position = 0;
				using (Formatter formatterRead = Formatter.Create(stream))
				{
					formatterRead.ReadObject(out dataResult);
				}

				// Write new format
				using (Formatter formatterWrite = Formatter.Create(stream, to))
				{
					formatterWrite.WriteObject(data);
				}

				// Read
				stream.Position = 0;
				using (Formatter formatterRead = Formatter.Create(stream))
				{
					formatterRead.ReadObject(out dataResult);
				}
			}

			Assert.IsTrue(data.Equals(dataResult));
		}
		[Test] public void BackwardsCompatibility()
		{
			Random rnd = new Random(0);
			TestObject obj = new TestObject(rnd);
			this.TestDataEqual("Old", obj, this.PrimaryFormat);

			// Test Data last updated 2014-03-11
			// this.CreateReferenceFile("Old", obj, this.PrimaryFormat);
		}

		
		private string GetReferenceResourceName(string name, FormattingMethod format)
		{
			return string.Format("FormatterTest{0}{1}Data", name, format);
		}
		private void CreateReferenceFile<T>(string name, T writeObj, FormattingMethod format)
		{
			string filePath = TestHelper.GetEmbeddedResourcePath(GetReferenceResourceName(name, format), ".dat");
			using (FileStream stream = File.Open(filePath, FileMode.Create))
			using (Formatter formatter = Formatter.Create(stream, format))
			{
				formatter.WriteObject(writeObj);
			}
		}

		private void TestDataEqual<T>(string name, T writeObj, FormattingMethod format)
		{
			T readObj;
			byte[] data = (byte[])TestRes.ResourceManager.GetObject(this.GetReferenceResourceName(name, format), System.Globalization.CultureInfo.InvariantCulture);
			using (MemoryStream stream = new MemoryStream(data))
			using (Formatter formatter = Formatter.Create(stream, format))
			{
				formatter.ReadObject(out readObj);
			}
			Assert.IsTrue(writeObj.Equals(readObj), "Failed data equality check of Type {0} with Value {1}", typeof(T), writeObj);
		}
		private void TestWriteRead<T>(T writeObj, FormattingMethod format)
		{
			T readObj;
			using (MemoryStream stream = new MemoryStream())
			{
				// Write
				using (Formatter formatterWrite = Formatter.Create(stream, format))
				{
					formatterWrite.WriteObject(writeObj);
				}

				// Read
				stream.Position = 0;
				using (Formatter formatterRead = Formatter.Create(stream))
				{
					readObj = formatterRead.ReadObject<T>();
				}
			}
			Assert.IsTrue(writeObj.Equals(readObj), "Failed single WriteRead of Type {0} with Value {1}", typeof(T), writeObj);
		}
		private void TestSequential<T>(T writeObjA, T writeObjB, FormattingMethod format)
		{
			T readObjA;
			T readObjB;

			using (MemoryStream stream = new MemoryStream())
			{
				long beginPos = stream.Position;
				// Write
				using (Formatter formatter = Formatter.Create(stream, format))
				{
					stream.Position = beginPos;
					formatter.WriteObject(writeObjA);
					formatter.WriteObject(writeObjB);

					stream.Position = beginPos;
					readObjA = (T)formatter.ReadObject();
					readObjB = (T)formatter.ReadObject();

					stream.Position = beginPos;
					formatter.WriteObject(writeObjA);
					formatter.WriteObject(writeObjB);
				}

				// Read
				stream.Position = beginPos;
				using (Formatter formatter = Formatter.Create(stream))
				{
					readObjA = (T)formatter.ReadObject();
					readObjB = (T)formatter.ReadObject();
				}
			}

			Assert.IsTrue(writeObjA.Equals(readObjA), "Failed sequential WriteRead of Type {0} with Value {1}", typeof(T), writeObjA);
			Assert.IsTrue(writeObjB.Equals(readObjB), "Failed sequential WriteRead of Type {0} with Value {1}", typeof(T), writeObjB);
		}
		private void TestRandomAccess<T>(T writeObjA, T writeObjB, FormattingMethod format)
		{
			T readObjA;
			T readObjB;

			using (MemoryStream stream = new MemoryStream())
			{
				long posB = 0;
				long posA = 0;
				// Write
				using (Formatter formatter = Formatter.Create(stream, format))
				{
					posB = stream.Position;
					formatter.WriteObject(writeObjB);
					posA = stream.Position;
					formatter.WriteObject(writeObjA);
					stream.Position = posB;
					formatter.WriteObject(writeObjB);

					stream.Position = posA;
					readObjA = (T)formatter.ReadObject();
					stream.Position = posB;
					readObjB = (T)formatter.ReadObject();

					stream.Position = posA;
					formatter.WriteObject(writeObjA);
					stream.Position = posB;
					formatter.WriteObject(writeObjB);
				}

				// Read
				using (Formatter formatter = Formatter.Create(stream, format))
				{
					stream.Position = posA;
					readObjA = (T)formatter.ReadObject();
					stream.Position = posB;
					readObjB = (T)formatter.ReadObject();
				}
			}

			Assert.IsTrue(writeObjA.Equals(readObjA), "Failed random access WriteRead of Type {0} with Value {1}", typeof(T), writeObjA);
			Assert.IsTrue(writeObjB.Equals(readObjB), "Failed random access WriteRead of Type {0} with Value {1}", typeof(T), writeObjB);
		}
	}
}
