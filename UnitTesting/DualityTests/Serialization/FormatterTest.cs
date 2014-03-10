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

namespace Duality.Tests.Serialization
{
	public abstract class FormatterTest
	{
		protected abstract FormattingMethod PrimaryFormat { get; }
		protected IEnumerable<FormattingMethod> OtherFormats
		{
			get { return Enum.GetValues(typeof(FormattingMethod)).Cast<FormattingMethod>().Where(m => m != FormattingMethod.Unknown && m != this.PrimaryFormat); }
		}


		[Test] public void SerializePlainOldData([Values(true, false)] bool meta)
		{
			Random rnd = new Random();

			this.TestWriteRead(rnd.NextBool(),			this.PrimaryFormat, meta);
			this.TestWriteRead(rnd.NextByte(),			this.PrimaryFormat, meta);
			this.TestWriteRead(rnd.Next(),				this.PrimaryFormat, meta);
			this.TestWriteRead(rnd.NextFloat(),			this.PrimaryFormat, meta);
			this.TestWriteRead(rnd.NextDouble(),		this.PrimaryFormat, meta);
			this.TestWriteRead(rnd.Next().ToString(),	this.PrimaryFormat, meta);
			this.TestWriteRead((SomeEnum)rnd.Next(10),	this.PrimaryFormat, meta);
		}
		[Test] public void SerializeFlatStruct([Values(true, false)] bool meta)
		{
			Random rnd = new Random();
			this.TestWriteRead(new TestData(rnd), this.PrimaryFormat, meta);
		}
		[Test] public void SerializeObjectTree([Values(true, false)] bool meta)
		{
			Random rnd = new Random();
			this.TestWriteRead(new TestObject(rnd), this.PrimaryFormat, meta);
		}
		[Test] public void SequentialAccess([Values(true, false)] bool meta)
		{
			Random rnd = new Random();
			TestObject dataA = new TestObject(rnd);
			TestObject dataB = new TestObject(rnd);

			this.TestSequential(dataA, dataB, this.PrimaryFormat, meta);
		}
		[Test] public void RandomAccess([Values(true, false)] bool meta)
		{
			Random rnd = new Random();
			TestObject dataA = new TestObject(rnd);
			TestObject dataB = new TestObject(rnd);

			this.TestRandomAccess(dataA, dataB, this.PrimaryFormat, meta);
		}
		[Test] public void BlendInOtherData([Values(true, false)] bool meta)
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

				if (meta)
				{
					using (Formatter metaFormatter = Formatter.CreateMeta(stream, this.PrimaryFormat))
					{
						DataNode metaNode;

						stream.Position = 0;
						using (BinaryReader binReader = new BinaryReader(stream.NonClosing()))
						{
							rawDataResultA = binReader.ReadString();
							metaFormatter.ReadObject(out metaNode);
							rawDataResultB = binReader.ReadInt64();
						}

						stream.Position = 0;
						stream.SetLength(0);
						using (BinaryWriter binWriter = new BinaryWriter(stream.NonClosing()))
						{
							binWriter.Write(rawDataA);
							metaFormatter.WriteObject(metaNode);
							binWriter.Write(rawDataB);
						}
					}
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
				// Write
				using (Formatter formatterWrite = Formatter.Create(stream, this.PrimaryFormat))
				{
					formatterWrite.WriteObject(data);
				}

				// Read-Write using MetaFormatter
				stream.Position = 0;
				DataNode metaNode = null;
				using (Formatter formatterRead = Formatter.CreateMeta(stream))
				{
					formatterRead.ReadObject(out metaNode);
				}
				stream.Position = 0;
				stream.SetLength(0);
				using (Formatter formatterWrite = Formatter.CreateMeta(stream, to))
				{
					formatterWrite.WriteObject(metaNode);
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


		protected void TestWriteRead<T>(T writeObj, FormattingMethod format, bool testMeta)
		{
			T readObj;
			using (MemoryStream stream = new MemoryStream())
			{
				// Write
				using (Formatter formatterWrite = Formatter.Create(stream, format))
				{
					formatterWrite.WriteObject(writeObj);
				}

				// Read-Write using MetaFormatter
				if (testMeta)
				{
					DataNode metaNode = null;
					stream.Position = 0;
					using (Formatter formatterRead = Formatter.CreateMeta(stream))
					{
						metaNode = formatterRead.ReadObject<DataNode>();
					}
					stream.Position = 0;
					stream.SetLength(0);
					using (Formatter formatterWrite = Formatter.CreateMeta(stream))
					{
						formatterWrite.WriteObject(metaNode);
					}
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
		protected void TestSequential<T>(T writeObjA, T writeObjB, FormattingMethod format, bool testMeta)
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

				// Read-Write using MetaFormatter
				if (testMeta)
				{
					DataNode metaNodeA = null;
					DataNode metaNodeB = null;
					using (Formatter formatter = Formatter.CreateMeta(stream, format))
					{
						stream.Position = beginPos;
						metaNodeA = (DataNode)formatter.ReadObject();
						metaNodeB = (DataNode)formatter.ReadObject();
					
						stream.Position = beginPos;
						formatter.WriteObject(metaNodeA);
						formatter.WriteObject(metaNodeB);
					}
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
		protected void TestRandomAccess<T>(T writeObjA, T writeObjB, FormattingMethod format, bool testMeta)
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

				// Read-Write using MetaFormatter
				if (testMeta)
				{
					DataNode metaNodeA = null;
					DataNode metaNodeB = null;
					using (Formatter formatter = Formatter.CreateMeta(stream, format))
					{
						stream.Position = posA;
						metaNodeA = (DataNode)formatter.ReadObject();
						stream.Position = posB;
						metaNodeB = (DataNode)formatter.ReadObject();

						stream.Position = posB;
						formatter.WriteObject(metaNodeB);
						formatter.WriteObject(metaNodeA);
						formatter.WriteObject(metaNodeB);
						stream.Position = posA;
						formatter.WriteObject(metaNodeA);
						stream.Position = posB;
						formatter.WriteObject(metaNodeB);
					}
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
