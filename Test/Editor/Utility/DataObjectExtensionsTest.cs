using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using Duality.Drawing;

using NUnit.Framework;

namespace Duality.Editor.Tests
{
	[TestFixture]
	public class DataObjectExtensionsTest
	{
		private class TestClass { }
		private class TestComponent : Component { }
		private class TestResource : Resource { }


		[Test] public void WrappedDataReference()
		{
			DataObject dataIn = new DataObject();
			TestClass dataOne = new TestClass();
			IEnumerable<object> outParam;

			dataIn.SetWrappedData(new [] { dataOne }, "CorrectFormat", DataObjectStorage.Reference);

			Assert.IsTrue(dataIn.GetWrappedDataPresent("CorrectFormat", DataObjectStorage.Reference));
			Assert.IsTrue(dataIn.GetWrappedDataPresent("CorrectFormat", DataObjectStorage.Value));
			Assert.AreSame(dataOne, dataIn.GetWrappedData("CorrectFormat", DataObjectStorage.Reference).First());
			Assert.AreNotSame(dataOne, dataIn.GetWrappedData("CorrectFormat", DataObjectStorage.Value).First());

			Assert.IsTrue(dataIn.TryGetWrappedData("CorrectFormat", DataObjectStorage.Reference, out outParam));
			Assert.IsNotNull(outParam);
			Assert.IsTrue(dataIn.TryGetWrappedData("CorrectFormat", DataObjectStorage.Value, out outParam));
			Assert.IsNotNull(outParam);
			Assert.IsFalse(dataIn.TryGetWrappedData("WrongFormat", DataObjectStorage.Reference, out outParam));
			Assert.IsNull(outParam);
			Assert.IsFalse(dataIn.TryGetWrappedData("WrongFormat", DataObjectStorage.Value, out outParam));
			Assert.IsNull(outParam);

			DataObject dataOut = this.SimulateClipboardSerialize(dataIn);

			Assert.IsTrue(dataOut.GetWrappedDataPresent("CorrectFormat", DataObjectStorage.Reference));
			Assert.IsTrue(dataOut.GetWrappedDataPresent("CorrectFormat", DataObjectStorage.Value));
			Assert.AreSame(dataOne, dataOut.GetWrappedData("CorrectFormat", DataObjectStorage.Reference).First());
			Assert.AreNotSame(dataOne, dataOut.GetWrappedData("CorrectFormat", DataObjectStorage.Value).First());

			Assert.IsTrue(dataOut.TryGetWrappedData("CorrectFormat", DataObjectStorage.Reference, out outParam));
			Assert.IsNotNull(outParam);
			Assert.IsTrue(dataOut.TryGetWrappedData("CorrectFormat", DataObjectStorage.Value, out outParam));
			Assert.IsNotNull(outParam);
			Assert.IsFalse(dataOut.TryGetWrappedData("WrongFormat", DataObjectStorage.Reference, out outParam));
			Assert.IsNull(outParam);
			Assert.IsFalse(dataOut.TryGetWrappedData("WrongFormat", DataObjectStorage.Value, out outParam));
			Assert.IsNull(outParam);
		}
		[Test] public void WrappedDataValue()
		{
			DataObject dataIn = new DataObject();
			TestClass dataOne = new TestClass();
			IEnumerable<object> outParam;

			dataIn.SetWrappedData(new[] { dataOne }, "CorrectFormat", DataObjectStorage.Value);

			Assert.IsFalse(dataIn.GetWrappedDataPresent("CorrectFormat", DataObjectStorage.Reference));
			Assert.IsTrue(dataIn.GetWrappedDataPresent("CorrectFormat", DataObjectStorage.Value));
			Assert.IsNull(dataIn.GetWrappedData("CorrectFormat", DataObjectStorage.Reference));
			Assert.AreNotSame(dataOne, dataIn.GetWrappedData("CorrectFormat", DataObjectStorage.Value).First());

			Assert.IsFalse(dataIn.TryGetWrappedData("CorrectFormat", DataObjectStorage.Reference, out outParam));
			Assert.IsNull(outParam);
			Assert.IsTrue(dataIn.TryGetWrappedData("CorrectFormat", DataObjectStorage.Value, out outParam));
			Assert.IsNotNull(outParam);
			Assert.IsFalse(dataIn.TryGetWrappedData("WrongFormat", DataObjectStorage.Reference, out outParam));
			Assert.IsNull(outParam);
			Assert.IsFalse(dataIn.TryGetWrappedData("WrongFormat", DataObjectStorage.Value, out outParam));
			Assert.IsNull(outParam);

			DataObject dataOut = this.SimulateClipboardSerialize(dataIn);

			Assert.IsFalse(dataOut.GetWrappedDataPresent("CorrectFormat", DataObjectStorage.Reference));
			Assert.IsTrue(dataOut.GetWrappedDataPresent("CorrectFormat", DataObjectStorage.Value));
			Assert.IsNull(dataOut.GetWrappedData("CorrectFormat", DataObjectStorage.Reference));
			Assert.AreNotSame(dataOne, dataOut.GetWrappedData("CorrectFormat", DataObjectStorage.Value).First());

			Assert.IsFalse(dataOut.TryGetWrappedData("CorrectFormat", DataObjectStorage.Reference, out outParam));
			Assert.IsNull(outParam);
			Assert.IsTrue(dataOut.TryGetWrappedData("CorrectFormat", DataObjectStorage.Value, out outParam));
			Assert.IsNotNull(outParam);
			Assert.IsFalse(dataOut.TryGetWrappedData("WrongFormat", DataObjectStorage.Reference, out outParam));
			Assert.IsNull(outParam);
			Assert.IsFalse(dataOut.TryGetWrappedData("WrongFormat", DataObjectStorage.Value, out outParam));
			Assert.IsNull(outParam);
		}

		[Test] public void GetGameObjects()
		{
			DataObject nullData = new DataObject();
			DataObject dataIn = new DataObject();
			GameObject gameObject = new GameObject();
			GameObject[] outParam;

			dataIn.SetGameObjects(new [] { gameObject });

			Assert.IsTrue(dataIn.ContainsGameObjects(DataObjectStorage.Reference));
			Assert.IsTrue(dataIn.ContainsGameObjects(DataObjectStorage.Value));
			Assert.AreSame(gameObject, dataIn.GetGameObjects(DataObjectStorage.Reference)[0]);
			Assert.AreNotSame(gameObject, dataIn.GetGameObjects(DataObjectStorage.Value)[0]);
			Assert.IsFalse(nullData.ContainsGameObjects(DataObjectStorage.Reference));
			Assert.IsFalse(nullData.ContainsGameObjects(DataObjectStorage.Value));

			Assert.IsTrue(dataIn.TryGetGameObjects(DataObjectStorage.Reference, out outParam));
			Assert.IsNotNull(outParam);
			Assert.IsTrue(dataIn.TryGetGameObjects(DataObjectStorage.Value, out outParam));
			Assert.IsNotNull(outParam);
			Assert.IsFalse(nullData.TryGetGameObjects(DataObjectStorage.Reference, out outParam));
			Assert.IsNull(outParam);
			Assert.IsFalse(nullData.TryGetGameObjects(DataObjectStorage.Value, out outParam));
			Assert.IsNull(outParam);

			DataObject dataOut = this.SimulateClipboardSerialize(dataIn);

			Assert.IsTrue(dataOut.ContainsGameObjects(DataObjectStorage.Reference));
			Assert.IsTrue(dataOut.ContainsGameObjects(DataObjectStorage.Value));
			Assert.AreSame(gameObject, dataOut.GetGameObjects(DataObjectStorage.Reference)[0]);
			Assert.AreNotSame(gameObject, dataOut.GetGameObjects(DataObjectStorage.Value)[0]);

			Assert.IsTrue(dataOut.TryGetGameObjects(DataObjectStorage.Reference, out outParam));
			Assert.IsNotNull(outParam);
			Assert.IsTrue(dataOut.TryGetGameObjects(DataObjectStorage.Value, out outParam));
			Assert.IsNotNull(outParam);
		}
		[Test] public void GetComponents()
		{
			DataObject dataIn = new DataObject();
			TestComponent comp = new TestComponent();
			Component[] outParam;

			dataIn.SetComponents(new[] { comp });

			Assert.IsTrue(dataIn.ContainsComponents(typeof(TestComponent), DataObjectStorage.Reference));
			Assert.IsTrue(dataIn.ContainsComponents(typeof(TestComponent), DataObjectStorage.Value));
			Assert.AreSame(comp, dataIn.GetComponents(typeof(TestComponent), DataObjectStorage.Reference)[0]);
			Assert.AreNotSame(comp, dataIn.GetComponents(typeof(TestComponent), DataObjectStorage.Value)[0]);

			Assert.IsTrue(dataIn.TryGetComponents(typeof(TestComponent), DataObjectStorage.Reference, out outParam));
			Assert.IsNotNull(outParam);
			Assert.IsTrue(dataIn.TryGetComponents(typeof(TestComponent), DataObjectStorage.Value, out outParam));
			Assert.IsNotNull(outParam);

			DataObject dataOut = this.SimulateClipboardSerialize(dataIn);

			Assert.IsTrue(dataOut.ContainsComponents(typeof(TestComponent), DataObjectStorage.Reference));
			Assert.IsTrue(dataOut.ContainsComponents(typeof(TestComponent), DataObjectStorage.Value));
			Assert.AreSame(comp, dataOut.GetComponents(typeof(TestComponent), DataObjectStorage.Reference)[0]);
			Assert.AreNotSame(comp, dataOut.GetComponents(typeof(TestComponent), DataObjectStorage.Value)[0]);

			Assert.IsTrue(dataOut.TryGetComponents(typeof(TestComponent), DataObjectStorage.Reference, out outParam));
			Assert.IsNotNull(outParam);
			Assert.IsTrue(dataOut.TryGetComponents(typeof(TestComponent), DataObjectStorage.Value, out outParam));
			Assert.IsNotNull(outParam);
		}
		[Test] public void GetContentRefs()
		{
			DataObject dataIn = new DataObject();
			TestResource res = new TestResource();
			ContentRef<TestResource> contentRef = new ContentRef<TestResource>(res);

			dataIn.SetContentRefs(new IContentRef[] { contentRef });

			Assert.IsTrue(dataIn.ContainsContentRefs());
			Assert.IsTrue(dataIn.ContainsContentRefs(typeof(TestResource)));
			Assert.AreEqual(contentRef, dataIn.GetContentRefs()[0]);
			Assert.AreEqual(contentRef, dataIn.GetContentRefs(typeof(TestResource))[0]);

			DataObject dataOut = this.SimulateClipboardSerialize(dataIn);

			Assert.IsTrue(dataOut.ContainsContentRefs());

			// This is false due to the fact that the reference to the locally created
			// TestResource will not survive serialization.
			Assert.IsFalse(dataOut.ContainsContentRefs(typeof(TestResource)));
		}
		[Test] public void GetBatchInfo()
		{
			DataObject dataIn = new DataObject();
			BatchInfo batch = new BatchInfo();

			dataIn.SetBatchInfos(new[] { batch });

			Assert.IsTrue(dataIn.ContainsBatchInfos());
			Assert.AreNotSame(batch, dataIn.GetBatchInfos()[0]);

			DataObject dataOut = this.SimulateClipboardSerialize(dataIn);

			Assert.IsTrue(dataOut.ContainsBatchInfos());
			Assert.AreNotSame(batch, dataOut.GetBatchInfos()[0]);
		}
		[Test] public void GetIColorData()
		{
			DataObject dataIn = new DataObject();
			ColorRgba color = new ColorRgba(10, 10, 10, 10);

			dataIn.SetIColorData(new[] { (IColorData)color });

			Assert.IsTrue(dataIn.ContainsIColorData());
			Assert.AreEqual(color, dataIn.GetIColorData<IColorData>()[0]);
			Assert.AreEqual(color, dataIn.GetIColorData<ColorRgba>()[0]);
			Assert.AreEqual(color.ConvertTo<ColorHsva>(), dataIn.GetIColorData<ColorHsva>()[0]);

			DataObject dataOut = this.SimulateClipboardSerialize(dataIn);

			Assert.IsTrue(dataOut.ContainsIColorData());
			Assert.AreEqual(color, dataOut.GetIColorData<IColorData>()[0]);
			Assert.AreEqual(color, dataOut.GetIColorData<ColorRgba>()[0]);
			Assert.AreEqual(color.ConvertTo<ColorHsva>(), dataOut.GetIColorData<ColorHsva>()[0]);
		}
		[Test] public void GetIColorDataString()
		{
			DataObject dataIn = new DataObject();
			ColorRgba color = new ColorRgba(10, 10, 10, 10);

			dataIn.SetString("10,10,10,10");

			Assert.IsTrue(dataIn.ContainsIColorData());
			Assert.AreEqual(color, dataIn.GetIColorData<ColorRgba>()[0]);

			DataObject dataOut = this.SimulateClipboardSerialize(dataIn);

			Assert.IsTrue(dataOut.ContainsIColorData());
			Assert.AreEqual(color, dataOut.GetIColorData<ColorRgba>()[0]);

			DataObject dataTooLong = new DataObject();
			dataTooLong.SetString("10,10,10,10,10");
			Assert.IsFalse(dataTooLong.ContainsIColorData());
			Assert.IsNull(dataTooLong.GetIColorData<ColorRgba>());

			DataObject dataWrongForm = new DataObject();
			dataWrongForm.SetString("error,a,b");
			Assert.IsFalse(dataWrongForm.ContainsIColorData());
			Assert.IsNull(dataWrongForm.GetIColorData<ColorRgba>());
		}
		[Test] public void GetString()
		{
			DataObject nullData = new DataObject();
			DataObject textData = new DataObject("Text", "test");
			DataObject unicodeData = new DataObject("UnicodeText", "test");
			DataObject typedData = new DataObject();
			typedData.SetData(typeof(string), "test");
			DataObject setData = new DataObject();
			setData.SetString("test");

			Assert.IsFalse(nullData.ContainsString());
			Assert.IsTrue(textData.ContainsString());
			Assert.IsTrue(unicodeData.ContainsString());
			Assert.IsTrue(typedData.ContainsString());
			Assert.IsTrue(setData.ContainsString());

			Assert.AreEqual(null, nullData.GetString());
			Assert.AreEqual("test", textData.GetString());
			Assert.AreEqual("test", unicodeData.GetString());
			Assert.AreEqual("test", typedData.GetString());
			Assert.AreEqual("test", setData.GetString());
			Assert.AreEqual("test", setData.GetData("Text") as string);
			Assert.AreEqual("test", setData.GetData("UnicodeText") as string);
		}


		private DataObject SimulateClipboardSerialize(DataObject dataIn)
		{
			BinaryFormatter formatter = new BinaryFormatter();
			DataObject dataOut = new DataObject();
			using (MemoryStream stream = new MemoryStream())
			{
				string[] formats = dataIn.GetFormats();
				foreach (string format in formats)
				{
					object objIn = dataIn.GetData(format, false);
					try
					{
						formatter.Serialize(stream, objIn);
						stream.Position = 0;
						object objOut = formatter.Deserialize(stream);
						stream.Position = 0;
						stream.SetLength(0);
						dataOut.SetData(format, objOut);
					}
					catch (Exception) { }
				}
			}
			return dataOut;
		}
	}
}
