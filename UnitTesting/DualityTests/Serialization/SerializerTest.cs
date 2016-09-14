using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Linq.Expressions;
using System.IO;
using System.Text;

using Duality;
using Duality.IO;
using Duality.Serialization;
using Duality.Tests.Properties;

using NUnit.Framework;

namespace Duality.Tests.Serialization
{
	[TestFixture(typeof(XmlSerializer))]
	[TestFixture(typeof(BinarySerializer))]
	public class SerializerTest
	{
		private Type format;

		private Type PrimaryFormat
		{
			get { return this.format; }
		}
		private IEnumerable<Type> OtherFormats
		{
			get { return Serializer.AvailableTypes.Where(m => m != this.PrimaryFormat); }
		}

		public SerializerTest(Type format)
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
		[Test] public void SerializeCharData()
		{
			for (int i = 0; i < 256; i++)
			{
				this.TestWriteRead((char)i,	this.PrimaryFormat);
			}
		}
		[Test] public void SerializePrimitiveArrays()
		{
			Random rnd = new Random();

			this.TestWriteRead(
				this.CreateArray<bool>(50, () => rnd.NextBool()), 
				this.PrimaryFormat,
				ArrayEquals);

			this.TestWriteRead(
				this.CreateArray<byte>(50, () => rnd.NextByte()), 
				this.PrimaryFormat,
				ArrayEquals);

			this.TestWriteRead(
				this.CreateArray<sbyte>(50, () => (sbyte)rnd.Next(sbyte.MinValue, sbyte.MaxValue)), 
				this.PrimaryFormat,
				ArrayEquals);

			this.TestWriteRead(
				this.CreateArray<short>(50, () => (short)rnd.Next(short.MinValue, short.MaxValue)), 
				this.PrimaryFormat,
				ArrayEquals);

			this.TestWriteRead(
				this.CreateArray<ushort>(50, () => (ushort)rnd.Next(ushort.MinValue, ushort.MaxValue)), 
				this.PrimaryFormat,
				ArrayEquals);

			this.TestWriteRead(
				this.CreateArray<int>(50, () => (int)rnd.Next(int.MinValue, int.MaxValue)), 
				this.PrimaryFormat,
				ArrayEquals);

			this.TestWriteRead(
				this.CreateArray<uint>(50, () => (uint)rnd.Next()), 
				this.PrimaryFormat,
				ArrayEquals);

			this.TestWriteRead(
				this.CreateArray<long>(50, () => (long)rnd.Next()), 
				this.PrimaryFormat,
				ArrayEquals);

			this.TestWriteRead(
				this.CreateArray<ulong>(50, () => (ulong)rnd.Next()), 
				this.PrimaryFormat,
				ArrayEquals);

			this.TestWriteRead(
				this.CreateArray<float>(50, () => rnd.NextFloat()), 
				this.PrimaryFormat,
				ArrayEquals);

			this.TestWriteRead(
				this.CreateArray<double>(50, () => rnd.NextDouble()), 
				this.PrimaryFormat,
				ArrayEquals);

			this.TestWriteRead(
				this.CreateArray<decimal>(50, () => (decimal)rnd.NextDouble()), 
				this.PrimaryFormat,
				ArrayEquals);
		}
		[Test] public void SerializeEmptyArrays()
		{
			Random rnd = new Random();

			// Primitive Types
			this.TestWriteRead(new bool[0],		this.PrimaryFormat, ArrayEquals);
			this.TestWriteRead(new byte[0],		this.PrimaryFormat, ArrayEquals);
			this.TestWriteRead(new sbyte[0],	this.PrimaryFormat, ArrayEquals);
			this.TestWriteRead(new short[0],	this.PrimaryFormat, ArrayEquals);
			this.TestWriteRead(new ushort[0],	this.PrimaryFormat, ArrayEquals);
			this.TestWriteRead(new int[0],		this.PrimaryFormat, ArrayEquals);
			this.TestWriteRead(new uint[0],		this.PrimaryFormat, ArrayEquals);
			this.TestWriteRead(new long[0],		this.PrimaryFormat, ArrayEquals);
			this.TestWriteRead(new ulong[0],	this.PrimaryFormat, ArrayEquals);
			this.TestWriteRead(new float[0],	this.PrimaryFormat, ArrayEquals);
			this.TestWriteRead(new double[0],	this.PrimaryFormat, ArrayEquals);
			this.TestWriteRead(new decimal[0],	this.PrimaryFormat, ArrayEquals);

			// Complex Types
			this.TestWriteRead(new object[0],		this.PrimaryFormat, ArrayEquals);
			this.TestWriteRead(new Type[0],			this.PrimaryFormat, ArrayEquals);
			this.TestWriteRead(new string[0],		this.PrimaryFormat, ArrayEquals);
			this.TestWriteRead(new GameObject[0],	this.PrimaryFormat, ArrayEquals);
			this.TestWriteRead(new Component[0],	this.PrimaryFormat, ArrayEquals);
		}
		[Test] public void SerializeStructArrays()
		{
			Random rnd = new Random();
			TestData[] testData = new TestData[10];
			for (int i = 0; i < testData.Length; i++)
			{
				testData[i] = new TestData(rnd);
			}

			// Can we properly serialize an array of structs?
			this.TestWriteRead(testData, this.PrimaryFormat, ArrayEquals);
		}
		[Test] public void SerializeNullDefault()
		{
			Random rnd = new Random();
			NullDefaultTestObject nullTestObj = new NullDefaultTestObject
			{
				TestField = rnd.Next(),
				ReferenceTypeField = null,
				ValueTypeField = default(TestData)
			};

			// Can we properly serialize null and default values?
			this.TestWriteRead(nullTestObj, this.PrimaryFormat);
		}
		[Test] public void SerializeEnumArrays()
		{
			Random rnd = new Random();
			SomeEnum[] testData = new SomeEnum[10];
			for (int i = 0; i < testData.Length; i++)
			{
				switch (rnd.Next(4))
				{
					default:
					case 0: testData[i] = SomeEnum.Zero;   break;
					case 1: testData[i] = SomeEnum.First;  break;
					case 2: testData[i] = SomeEnum.Second; break;
					case 3: testData[i] = SomeEnum.Third;  break;
				}
			}

			// Can we properly serialize an array of enums?
			this.TestWriteRead(testData, this.PrimaryFormat, ArrayEquals);
		}
		[Test] public void SerializeMemberInfo()
		{
			Type type = typeof(MemberInfoTestObject);
			TypeInfo typeInfo = type.GetTypeInfo();
			FieldInfo fieldInfo = type.GetRuntimeFields().FirstOrDefault();
			PropertyInfo propertyInfo = type.GetRuntimeProperties().FirstOrDefault();
			MethodInfo methodInfo = type.GetRuntimeMethods().FirstOrDefault();
			ConstructorInfo constructorInfo = typeInfo.DeclaredConstructors.FirstOrDefault();
			EventInfo eventInfo = type.GetRuntimeEvents().FirstOrDefault();

			this.TestWriteRead(type,			this.PrimaryFormat);
			this.TestWriteRead(fieldInfo,		this.PrimaryFormat);
			this.TestWriteRead(propertyInfo,	this.PrimaryFormat);
			this.TestWriteRead(methodInfo,		this.PrimaryFormat);
			this.TestWriteRead(constructorInfo,	this.PrimaryFormat);
			this.TestWriteRead(eventInfo,		this.PrimaryFormat);
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
			using (Serializer formatter = Serializer.Create(stream, this.PrimaryFormat))
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
		[Test] public void ConvertFormat([ValueSource("OtherFormats")] Type to)
		{
			Random rnd = new Random();
			TestObject data = new TestObject(rnd);
			TestObject dataResult;

			using (MemoryStream stream = new MemoryStream())
			{
				// Write old format
				using (Serializer formatterWrite = Serializer.Create(stream, this.PrimaryFormat))
				{
					formatterWrite.WriteObject(data);
				}

				// Read
				stream.Position = 0;
				using (Serializer formatterRead = Serializer.Create(stream))
				{
					formatterRead.ReadObject(out dataResult);
				}

				// Write new format
				using (Serializer formatterWrite = Serializer.Create(stream, to))
				{
					formatterWrite.WriteObject(data);
				}

				// Read
				stream.Position = 0;
				using (Serializer formatterRead = Serializer.Create(stream))
				{
					formatterRead.ReadObject(out dataResult);
				}
			}

			Assert.IsTrue(data.Equals(dataResult));
		}
		[Test] public void BackwardsCompatibility()
		{
			Random rnd = new Random(0);
			TestObject obj = TestObject.CreateBackwardsCompatible(rnd);
			this.TestDataEqual("Old", obj, this.PrimaryFormat);

			// Test Data last updated 2014-03-11
			// this.CreateReferenceFile("Old", obj, this.PrimaryFormat);
		}
		[Test] public void PerformanceTest()
		{
			var watch = new System.Diagnostics.Stopwatch();
			
			Random rnd = new Random(0);
			TestObject data = new TestObject(rnd, 5);
			TestObject[] results = new TestObject[50];
			
			watch.Start();
			long memUsage;
			using (MemoryStream stream = new MemoryStream())
			{
				// Write
				for (int i = 0; i < results.Length; i++)
				{
					using (Serializer formatterWrite = Serializer.Create(stream, format))
					{
						formatterWrite.WriteObject(data);
					}
				}

				memUsage = stream.Length / 1024;
				stream.Position = 0;

				// Read
				for (int i = 0; i < results.Length; i++)
				{
					using (Serializer formatterRead = Serializer.Create(stream))
					{
						results[i] = formatterRead.ReadObject<TestObject>();
					}
				}
			}
			watch.Stop();
			TestHelper.LogNumericTestResult(this, "ReadWritePerformance" + this.format.ToString(), watch.Elapsed.TotalMilliseconds, "ms");
			TestHelper.LogNumericTestResult(this, "MemoryUsage" + this.format.ToString(), memUsage, "Kb");

			Assert.Pass();
		}
		[Test] public void CleanupAfterDisposedStream()
		{
			Random rnd = new Random();
			
			// Prepare stream and serializer. Make sure it's a stream that won't
			// allow flushing after disposal, i.e. not a MemoryStream, which is fairly
			// robust with regard to behavior-after-disposal.
			string tempFile = Path.GetTempFileName();
			Stream stream = File.Open(tempFile, FileMode.Create);
			Serializer serializer = Serializer.Create(stream, this.PrimaryFormat);

			// Write something arbitrary
			serializer.WriteObject(rnd.NextVector2());

			// Dispose the stream first, and the serializer afterwards.
			// Make sure it doesn't fail on cleanup if the stream is already
			// gone.
			stream.Dispose();
			File.Delete(tempFile);
			serializer.Dispose();
		}

		
		private string GetReferenceResourceName(string name, Type format)
		{
			return string.Format("SerializerTest{0}{1}Data", name, format.Name);
		}
		private void CreateReferenceFile<T>(string name, T writeObj, Type format)
		{
			string filePath = TestHelper.GetEmbeddedResourcePath(GetReferenceResourceName(name, format), ".dat");
			using (FileStream stream = File.Open(filePath, FileMode.Create))
			using (Serializer formatter = Serializer.Create(stream, format))
			{
				formatter.WriteObject(writeObj);
			}
		}

		private T[] CreateArray<T>(int length, Func<T> initValue) where T : struct
		{
			T[] result = new T[length];
			for (int i = 0; i < length; i++)
			{
				result[i] = initValue();
			}
			return result;
		}
		private static bool ArrayEquals<T>(T[] a, T[] b)
		{
			if (a == b) return true;
			if (a == null) return false;
			if (b == null) return false;
			if (a.Length != b.Length) return false;

			for (int i = 0; i < a.Length; i++)
			{
				if (!object.Equals(a[i], b[i]))
					return false;
			}

			return true;
		}

		private void TestDataEqual<T>(string name, T writeObj, Type format, Func<T,T,bool> checkEqual = null)
		{
			if (checkEqual == null) checkEqual = (a, b) => object.Equals(a, b);

			T readObj;
			byte[] data = (byte[])TestRes.ResourceManager.GetObject(this.GetReferenceResourceName(name, format), System.Globalization.CultureInfo.InvariantCulture);
			using (MemoryStream stream = new MemoryStream(data))
			using (Serializer formatter = Serializer.Create(stream, format))
			{
				formatter.ReadObject(out readObj);
			}
			Assert.IsTrue(checkEqual(writeObj, readObj), "Failed data equality check of Type {0} with Value {1}", typeof(T), writeObj);
		}
		private void TestWriteRead<T>(T writeObj, Type format, Func<T,T,bool> checkEqual = null)
		{
			if (checkEqual == null) checkEqual = (a, b) => object.Equals(a, b);

			T readObj;
			using (MemoryStream stream = new MemoryStream())
			{
				// Write
				using (Serializer formatterWrite = Serializer.Create(stream, format))
				{
					formatterWrite.WriteObject(writeObj);
				}

				// Read
				stream.Position = 0;
				using (Serializer formatterRead = Serializer.Create(stream))
				{
					readObj = formatterRead.ReadObject<T>();
				}
			}
			Assert.IsTrue(checkEqual(writeObj, readObj), "Failed single WriteRead of Type {0} with Value {1}", typeof(T), writeObj);
		}
		private void TestSequential<T>(T writeObjA, T writeObjB, Type format, Func<T,T,bool> checkEqual = null)
		{
			if (checkEqual == null) checkEqual = (a, b) => object.Equals(a, b);

			T readObjA;
			T readObjB;

			using (MemoryStream stream = new MemoryStream())
			{
				long beginPos = stream.Position;
				// Write
				using (Serializer formatter = Serializer.Create(stream, format))
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
				using (Serializer formatter = Serializer.Create(stream))
				{
					readObjA = (T)formatter.ReadObject();
					readObjB = (T)formatter.ReadObject();
				}
			}

			Assert.IsTrue(checkEqual(writeObjA, readObjA), "Failed sequential WriteRead of Type {0} with Value {1}", typeof(T), writeObjA);
			Assert.IsTrue(checkEqual(writeObjB, readObjB), "Failed sequential WriteRead of Type {0} with Value {1}", typeof(T), writeObjB);
		}
		private void TestRandomAccess<T>(T writeObjA, T writeObjB, Type format, Func<T,T,bool> checkEqual = null)
		{
			if (checkEqual == null) checkEqual = (a, b) => object.Equals(a, b);

			T readObjA;
			T readObjB;

			using (MemoryStream stream = new MemoryStream())
			{
				long posB = 0;
				long posA = 0;
				// Write
				using (Serializer formatter = Serializer.Create(stream, format))
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
				using (Serializer formatter = Serializer.Create(stream, format))
				{
					stream.Position = posA;
					readObjA = (T)formatter.ReadObject();
					stream.Position = posB;
					readObjB = (T)formatter.ReadObject();
				}
			}

			Assert.IsTrue(checkEqual(writeObjA, readObjA), "Failed random access WriteRead of Type {0} with Value {1}", typeof(T), writeObjA);
			Assert.IsTrue(checkEqual(writeObjB, readObjB), "Failed random access WriteRead of Type {0} with Value {1}", typeof(T), writeObjB);
		}
	}
}
