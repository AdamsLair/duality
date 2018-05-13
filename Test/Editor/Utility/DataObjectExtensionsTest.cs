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

		[Test] public void GetGameObjectAfterClipboard()
		{
			DataObject dataIn = new DataObject();
			GameObject gameObject = new GameObject("a");

			dataIn.SetGameObjects(new [] { gameObject });
			Clipboard.SetDataObject(dataIn);

			DataObject dataOut = (DataObject) Clipboard.GetDataObject();

			Assert.IsTrue(dataOut.ContainsGameObjects(DataFormat.Reference));
			Assert.IsTrue(dataOut.ContainsGameObjects(DataFormat.Value));

			Assert.AreSame(gameObject, dataOut.GetGameObjects(DataFormat.Reference)[0]);
			Assert.AreNotSame(gameObject, dataOut.GetGameObjects(DataFormat.Value)[0]);
		}

		[Test]
		public void GetComponentAfterClipboard()
		{
			DataObject dataIn = new DataObject();
			TestComponent comp = new TestComponent();

			dataIn.SetComponents(new[] { comp });
			Clipboard.SetDataObject(dataIn);

			DataObject dataOut = (DataObject)Clipboard.GetDataObject();

			Assert.IsTrue(dataOut.ContainsComponents(DataFormat.Reference));
			Assert.IsTrue(dataOut.ContainsComponents(DataFormat.Value));

			Assert.AreSame(comp, dataOut.GetComponents(DataFormat.Reference)[0]);
			Assert.AreNotSame(comp, dataOut.GetComponents(DataFormat.Value)[0]);
		}

		[Test]
		public void GetComponentAfterClipboardStronglyTyped()
		{
			DataObject dataIn = new DataObject();
			TestComponent comp = new TestComponent();

			dataIn.SetComponents(new[] { comp });
			Clipboard.SetDataObject(dataIn);

			DataObject dataOut = (DataObject)Clipboard.GetDataObject();

			Assert.IsTrue(dataOut.ContainsComponents<TestComponent>(DataFormat.Reference));
			Assert.IsTrue(dataOut.ContainsComponents<TestComponent>(DataFormat.Value));

			Assert.AreSame(comp, dataOut.GetComponents<TestComponent>(DataFormat.Reference)[0]);
			Assert.AreNotSame(comp, dataOut.GetComponents<TestComponent>(DataFormat.Value)[0]);
		}
	}
}
