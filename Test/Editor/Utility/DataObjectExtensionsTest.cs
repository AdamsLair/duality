using System.Windows.Forms;
using Duality.Drawing;
using NUnit.Framework;

namespace Duality.Editor.Tests
{
	[TestFixture]
	public class DataObjectExtensionsTest
	{
		private class TestClass
		{
		}

		private class TestComponent : Component
		{
		}

		private class TestResource : Resource
		{
		}

		[Test] public void WrappedDataReference()
		{
			DataObject dataIn = new DataObject();
			TestClass dataOne = new TestClass();
			object[] outParam;

			dataIn.SetWrappedData(new [] { dataOne }, DataFormat.Reference);

			Assert.IsTrue(dataIn.GetWrappedDataPresent(typeof(TestClass[]), DataFormat.Reference));
			Assert.IsTrue(dataIn.GetWrappedDataPresent(typeof(TestClass[]), DataFormat.Value));
			Assert.IsFalse(dataIn.GetWrappedDataPresent(typeof(TestClass[]), DataFormat.Value, false));
			Assert.AreSame(dataOne, dataIn.GetWrappedData(typeof(TestClass[]), DataFormat.Reference)[0]);
			Assert.AreNotSame(dataOne, dataIn.GetWrappedData(typeof(TestClass[]), DataFormat.Value)[0]);
			Assert.IsNull(dataIn.GetWrappedData(typeof(TestClass[]), DataFormat.Value, false));

			Assert.IsTrue(dataIn.TryGetWrappedData(typeof(TestClass[]), DataFormat.Reference, out outParam));
			Assert.IsNotNull(outParam);
			Assert.IsTrue(dataIn.TryGetWrappedData(typeof(TestClass[]), DataFormat.Value, out outParam));
			Assert.IsNotNull(outParam);
			Assert.IsFalse(dataIn.TryGetWrappedData(typeof(TestClass[]), DataFormat.Value, false, out outParam));
			Assert.IsNull(outParam);
			Assert.IsFalse(dataIn.TryGetWrappedData(typeof(object), DataFormat.Reference, out outParam));
			Assert.IsNull(outParam);
			Assert.IsFalse(dataIn.TryGetWrappedData(typeof(object), DataFormat.Value, out outParam));
			Assert.IsNull(outParam);

			Clipboard.SetDataObject(dataIn);
			DataObject dataOut = (DataObject)Clipboard.GetDataObject();

			Assert.IsTrue(dataOut.GetWrappedDataPresent(typeof(TestClass[]), DataFormat.Reference));
			Assert.IsTrue(dataOut.GetWrappedDataPresent(typeof(TestClass[]), DataFormat.Value));
			Assert.IsFalse(dataOut.GetWrappedDataPresent(typeof(TestClass[]), DataFormat.Value, false));
			Assert.AreSame(dataOne, dataOut.GetWrappedData(typeof(TestClass[]), DataFormat.Reference)[0]);
			Assert.AreNotSame(dataOne, dataOut.GetWrappedData(typeof(TestClass[]), DataFormat.Value)[0]);
			Assert.IsNull(dataIn.GetWrappedData(typeof(TestClass[]), DataFormat.Value, false));

			Assert.IsTrue(dataOut.TryGetWrappedData(typeof(TestClass[]), DataFormat.Reference, out outParam));
			Assert.IsNotNull(outParam);
			Assert.IsTrue(dataOut.TryGetWrappedData(typeof(TestClass[]), DataFormat.Value, out outParam));
			Assert.IsNotNull(outParam);
			Assert.IsFalse(dataOut.TryGetWrappedData(typeof(TestClass[]), DataFormat.Value, false, out outParam));
			Assert.IsNull(outParam);
			Assert.IsFalse(dataOut.TryGetWrappedData(typeof(object), DataFormat.Reference, out outParam));
			Assert.IsNull(outParam);
			Assert.IsFalse(dataOut.TryGetWrappedData(typeof(object), DataFormat.Value, out outParam));
			Assert.IsNull(outParam);
		}

		[Test] public void WrappedDataValue()
		{
			DataObject dataIn = new DataObject();
			TestClass dataOne = new TestClass();
			object[] outParam;

			dataIn.SetWrappedData(new[] { dataOne }, DataFormat.Value);

			Assert.IsFalse(dataIn.GetWrappedDataPresent(typeof(TestClass[]), DataFormat.Reference));
			Assert.IsTrue(dataIn.GetWrappedDataPresent(typeof(TestClass[]), DataFormat.Value));
			Assert.IsTrue(dataIn.GetWrappedDataPresent(typeof(TestClass[]), DataFormat.Value, false));
			Assert.IsNull(dataIn.GetWrappedData(typeof(TestClass[]), DataFormat.Reference));
			// Even though we are retrieving a value, serialization has not occured yet
			Assert.AreSame(dataOne, dataIn.GetWrappedData(typeof(TestClass[]), DataFormat.Value)[0]);
			Assert.AreSame(dataOne, dataIn.GetWrappedData(typeof(TestClass[]), DataFormat.Value, false)[0]);

			Assert.IsFalse(dataIn.TryGetWrappedData(typeof(TestClass[]), DataFormat.Reference, out outParam));
			Assert.IsNull(outParam);
			Assert.IsTrue(dataIn.TryGetWrappedData(typeof(TestClass[]), DataFormat.Value, out outParam));
			Assert.IsNotNull(outParam);
			Assert.IsTrue(dataIn.TryGetWrappedData(typeof(TestClass[]), DataFormat.Value, false, out outParam));
			Assert.IsNotNull(outParam);
			Assert.IsFalse(dataIn.TryGetWrappedData(typeof(object), DataFormat.Reference, out outParam));
			Assert.IsNull(outParam);
			Assert.IsFalse(dataIn.TryGetWrappedData(typeof(object), DataFormat.Value, out outParam));
			Assert.IsNull(outParam);

			Clipboard.SetDataObject(dataIn);
			DataObject dataOut = (DataObject)Clipboard.GetDataObject();

			Assert.IsFalse(dataOut.GetWrappedDataPresent(typeof(TestClass[]), DataFormat.Reference));
			Assert.IsTrue(dataOut.GetWrappedDataPresent(typeof(TestClass[]), DataFormat.Value));
			Assert.IsTrue(dataOut.GetWrappedDataPresent(typeof(TestClass[]), DataFormat.Value, false));
			Assert.IsNull(dataOut.GetWrappedData(typeof(TestClass[]), DataFormat.Reference));
			// DeepClone should have been done by now
			Assert.AreNotSame(dataOne, dataOut.GetWrappedData(typeof(TestClass[]), DataFormat.Value)[0]);
			Assert.AreNotSame(dataOne, dataOut.GetWrappedData(typeof(TestClass[]), DataFormat.Value, false)[0]);

			Assert.IsFalse(dataOut.TryGetWrappedData(typeof(TestClass[]), DataFormat.Reference, out outParam));
			Assert.IsNull(outParam);
			Assert.IsTrue(dataOut.TryGetWrappedData(typeof(TestClass[]), DataFormat.Value, out outParam));
			Assert.IsNotNull(outParam);
			Assert.IsTrue(dataOut.TryGetWrappedData(typeof(TestClass[]), DataFormat.Value, false, out outParam));
			Assert.IsNotNull(outParam);
			Assert.IsFalse(dataOut.TryGetWrappedData(typeof(object), DataFormat.Reference, out outParam));
			Assert.IsNull(outParam);
			Assert.IsFalse(dataOut.TryGetWrappedData(typeof(object), DataFormat.Value, out outParam));
			Assert.IsNull(outParam);
		}

		[Test] public void GetGameObjects()
		{
			DataObject nullData = new DataObject();
			DataObject dataIn = new DataObject();
			GameObject gameObject = new GameObject();
			GameObject[] outParam;

			dataIn.SetGameObjects(new [] { gameObject });

			Assert.IsTrue(dataIn.ContainsGameObjects(DataFormat.Reference));
			Assert.IsTrue(dataIn.ContainsGameObjects(DataFormat.Value));
			Assert.AreSame(gameObject, dataIn.GetGameObjects(DataFormat.Reference)[0]);
			Assert.AreNotSame(gameObject, dataIn.GetGameObjects(DataFormat.Value)[0]);
			Assert.IsFalse(nullData.ContainsGameObjects(DataFormat.Reference));
			Assert.IsFalse(nullData.ContainsGameObjects(DataFormat.Value));

			Assert.IsTrue(dataIn.TryGetGameObjects(out outParam));
			Assert.IsNotNull(outParam);
			Assert.IsTrue(dataIn.TryGetGameObjects(DataFormat.Reference, out outParam));
			Assert.IsNotNull(outParam);
			Assert.IsTrue(dataIn.TryGetGameObjects(DataFormat.Value, out outParam));
			Assert.IsNotNull(outParam);
			Assert.IsFalse(nullData.TryGetGameObjects(out outParam));
			Assert.IsNull(outParam);
			Assert.IsFalse(nullData.TryGetGameObjects(DataFormat.Reference, out outParam));
			Assert.IsNull(outParam);
			Assert.IsFalse(nullData.TryGetGameObjects(DataFormat.Value, out outParam));
			Assert.IsNull(outParam);

			Clipboard.SetDataObject(dataIn);

			DataObject dataOut = (DataObject) Clipboard.GetDataObject();

			Assert.IsTrue(dataOut.ContainsGameObjects(DataFormat.Reference));
			Assert.IsTrue(dataOut.ContainsGameObjects(DataFormat.Value));
			Assert.AreSame(gameObject, dataOut.GetGameObjects(DataFormat.Reference)[0]);
			Assert.AreNotSame(gameObject, dataOut.GetGameObjects(DataFormat.Value)[0]);

			Assert.IsTrue(dataOut.TryGetGameObjects(out outParam));
			Assert.IsNotNull(outParam);
			Assert.IsTrue(dataOut.TryGetGameObjects(DataFormat.Reference, out outParam));
			Assert.IsNotNull(outParam);
			Assert.IsTrue(dataOut.TryGetGameObjects(DataFormat.Value, out outParam));
			Assert.IsNotNull(outParam);
		}

		[Test] public void GetComponents()
		{
			DataObject dataIn = new DataObject();
			TestComponent comp = new TestComponent();
			Component[] outParam;

			dataIn.SetComponents(new[] { comp });

			Assert.IsTrue(dataIn.ContainsComponents(DataFormat.Reference));
			Assert.IsTrue(dataIn.ContainsComponents(DataFormat.Value));
			Assert.AreSame(comp, dataIn.GetComponents(DataFormat.Reference)[0]);
			Assert.AreNotSame(comp, dataIn.GetComponents(DataFormat.Value)[0]);

			Assert.IsTrue(dataIn.TryGetComponents(out outParam));
			Assert.IsNotNull(outParam);
			Assert.IsTrue(dataIn.TryGetComponents(typeof(TestComponent), out outParam));
			Assert.IsNotNull(outParam);
			Assert.IsTrue(dataIn.TryGetComponents(DataFormat.Reference, out outParam));
			Assert.IsNotNull(outParam);
			Assert.IsTrue(dataIn.TryGetComponents(DataFormat.Value, out outParam));
			Assert.IsNotNull(outParam);
			Assert.IsTrue(dataIn.TryGetComponents(typeof(TestComponent), DataFormat.Reference, out outParam));
			Assert.IsNotNull(outParam);
			Assert.IsTrue(dataIn.TryGetComponents(typeof(TestComponent), DataFormat.Value, out outParam));
			Assert.IsNotNull(outParam);

			Clipboard.SetDataObject(dataIn);

			DataObject dataOut = (DataObject)Clipboard.GetDataObject();

			Assert.IsTrue(dataOut.ContainsComponents(DataFormat.Reference));
			Assert.IsTrue(dataOut.ContainsComponents(DataFormat.Value));
			Assert.AreSame(comp, dataOut.GetComponents(DataFormat.Reference)[0]);
			Assert.AreNotSame(comp, dataOut.GetComponents(DataFormat.Value)[0]);

			Assert.IsTrue(dataOut.TryGetComponents(out outParam));
			Assert.IsNotNull(outParam);
			Assert.IsTrue(dataOut.TryGetComponents(typeof(TestComponent), out outParam));
			Assert.IsNotNull(outParam);
			Assert.IsTrue(dataOut.TryGetComponents(DataFormat.Reference, out outParam));
			Assert.IsNotNull(outParam);
			Assert.IsTrue(dataOut.TryGetComponents(DataFormat.Value, out outParam));
			Assert.IsNotNull(outParam);
			Assert.IsTrue(dataOut.TryGetComponents(typeof(TestComponent), DataFormat.Reference, out outParam));
			Assert.IsNotNull(outParam);
			Assert.IsTrue(dataOut.TryGetComponents(typeof(TestComponent), DataFormat.Value, out outParam));
			Assert.IsNotNull(outParam);
		}

		[Test] public void GetComponentStronglyTyped()
		{
			DataObject dataIn = new DataObject();
			TestComponent comp = new TestComponent();
			TestComponent[] outParam;

			dataIn.SetComponents(new[] { comp });

			Assert.IsTrue(dataIn.ContainsComponents<TestComponent>(DataFormat.Reference));
			Assert.IsTrue(dataIn.ContainsComponents<TestComponent>(DataFormat.Value));
			Assert.AreSame(comp, dataIn.GetComponents<TestComponent>(DataFormat.Reference)[0]);
			Assert.AreNotSame(comp, dataIn.GetComponents<TestComponent>(DataFormat.Value)[0]);

			Assert.IsTrue(dataIn.TryGetComponents<TestComponent>(out outParam));
			Assert.IsNotNull(outParam);
			Assert.IsTrue(dataIn.TryGetComponents<TestComponent>(DataFormat.Reference, out outParam));
			Assert.IsNotNull(outParam);
			Assert.IsTrue(dataIn.TryGetComponents<TestComponent>(DataFormat.Value, out outParam));
			Assert.IsNotNull(outParam);

			Clipboard.SetDataObject(dataIn);

			DataObject dataOut = (DataObject)Clipboard.GetDataObject();

			Assert.IsTrue(dataOut.ContainsComponents<TestComponent>(DataFormat.Reference));
			Assert.IsTrue(dataOut.ContainsComponents<TestComponent>(DataFormat.Value));
			Assert.AreSame(comp, dataOut.GetComponents<TestComponent>(DataFormat.Reference)[0]);
			Assert.AreNotSame(comp, dataOut.GetComponents<TestComponent>(DataFormat.Value)[0]);

			Assert.IsTrue(dataIn.TryGetComponents<TestComponent>(out outParam));
			Assert.IsNotNull(outParam);
			Assert.IsTrue(dataIn.TryGetComponents<TestComponent>(DataFormat.Reference, out outParam));
			Assert.IsNotNull(outParam);
			Assert.IsTrue(dataIn.TryGetComponents<TestComponent>(DataFormat.Value, out outParam));
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

			Clipboard.SetDataObject(dataIn);

			DataObject dataOut = (DataObject)Clipboard.GetDataObject();

			Assert.IsTrue(dataOut.ContainsContentRefs());

			// This is false due to the fact that the reference to the locally created
			// TestResource will not survive serialization.
			Assert.IsFalse(dataOut.ContainsContentRefs(typeof(TestResource)));
		}

		[Test] public void GetContentRefsStronglyTyped()
		{
			DataObject dataIn = new DataObject();
			TestResource res = new TestResource();
			ContentRef<TestResource> contentRef = new ContentRef<TestResource>(res);

			dataIn.SetContentRefs(new IContentRef[] { contentRef });

			Assert.IsTrue(dataIn.ContainsContentRefs<TestResource>());

			Assert.AreEqual(contentRef, dataIn.GetContentRefs<TestResource>()[0]);

			Clipboard.SetDataObject(dataIn);

			DataObject dataOut = (DataObject)Clipboard.GetDataObject();

			// This is false due to the fact that the reference to the locally created
			// TestResource will not survive serialization.
			Assert.IsFalse(dataOut.ContainsContentRefs<TestResource>());
		}

		[Test] public void GetBatchInfo()
		{
			DataObject dataIn = new DataObject();
			BatchInfo batch = new BatchInfo();

			dataIn.SetBatchInfos(new[] { batch });

			Assert.IsTrue(dataIn.ContainsBatchInfos());
			Assert.AreNotSame(batch, dataIn.GetBatchInfos()[0]);

			Clipboard.SetDataObject(dataIn);

			DataObject dataOut = (DataObject)Clipboard.GetDataObject();

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

			Clipboard.SetDataObject(dataIn);

			DataObject dataOut = (DataObject)Clipboard.GetDataObject();

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

			Clipboard.SetDataObject(dataIn);

			DataObject dataOut = (DataObject)Clipboard.GetDataObject();

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
	}
}
