using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NUnit.Framework;

namespace Duality.Editor.Tests
{
	[TestFixture]
	public class DataObjectExtensionsTest
	{
		private class TestComponent : Component
		{
		}

		private class TestResource : Resource
		{
		}

		[Test] public void GetGameObjects()
		{
			DataObject dataIn = new DataObject();
			GameObject gameObject = new GameObject();

			dataIn.SetGameObjects(new [] { gameObject });

			Assert.IsTrue(dataIn.ContainsGameObjects(DataFormat.Reference));
			Assert.IsTrue(dataIn.ContainsGameObjects(DataFormat.Value));

			Assert.AreSame(gameObject, dataIn.GetGameObjects(DataFormat.Reference)[0]);
			Assert.AreNotSame(gameObject, dataIn.GetGameObjects(DataFormat.Value)[0]);

			Clipboard.SetDataObject(dataIn);

			DataObject dataOut = (DataObject) Clipboard.GetDataObject();

			Assert.IsTrue(dataOut.ContainsGameObjects(DataFormat.Reference));
			Assert.IsTrue(dataOut.ContainsGameObjects(DataFormat.Value));

			Assert.AreSame(gameObject, dataOut.GetGameObjects(DataFormat.Reference)[0]);
			Assert.AreNotSame(gameObject, dataOut.GetGameObjects(DataFormat.Value)[0]);
		}

		[Test] public void GetComponents()
		{
			DataObject dataIn = new DataObject();
			TestComponent comp = new TestComponent();

			dataIn.SetComponents(new[] { comp });

			Assert.IsTrue(dataIn.ContainsComponents(DataFormat.Reference));
			Assert.IsTrue(dataIn.ContainsComponents(DataFormat.Value));

			Assert.AreSame(comp, dataIn.GetComponents(DataFormat.Reference)[0]);
			Assert.AreNotSame(comp, dataIn.GetComponents(DataFormat.Value)[0]);

			Clipboard.SetDataObject(dataIn);

			DataObject dataOut = (DataObject)Clipboard.GetDataObject();

			Assert.IsTrue(dataOut.ContainsComponents(DataFormat.Reference));
			Assert.IsTrue(dataOut.ContainsComponents(DataFormat.Value));

			Assert.AreSame(comp, dataOut.GetComponents(DataFormat.Reference)[0]);
			Assert.AreNotSame(comp, dataOut.GetComponents(DataFormat.Value)[0]);
		}

		[Test] public void GetComponentStronglyTyped()
		{
			DataObject dataIn = new DataObject();
			TestComponent comp = new TestComponent();

			dataIn.SetComponents(new[] { comp });

			Assert.IsTrue(dataIn.ContainsComponents<TestComponent>(DataFormat.Reference));
			Assert.IsTrue(dataIn.ContainsComponents<TestComponent>(DataFormat.Value));

			Assert.AreSame(comp, dataIn.GetComponents<TestComponent>(DataFormat.Reference)[0]);
			Assert.AreNotSame(comp, dataIn.GetComponents<TestComponent>(DataFormat.Value)[0]);

			Clipboard.SetDataObject(dataIn);

			DataObject dataOut = (DataObject)Clipboard.GetDataObject();

			Assert.IsTrue(dataOut.ContainsComponents<TestComponent>(DataFormat.Reference));
			Assert.IsTrue(dataOut.ContainsComponents<TestComponent>(DataFormat.Value));

			Assert.AreSame(comp, dataOut.GetComponents<TestComponent>(DataFormat.Reference)[0]);
			Assert.AreNotSame(comp, dataOut.GetComponents<TestComponent>(DataFormat.Value)[0]);
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
	}
}
