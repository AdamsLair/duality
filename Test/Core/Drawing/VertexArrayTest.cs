using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

using Duality;
using Duality.Drawing;
using Duality.Tests.Properties;

using NUnit.Framework;

namespace Duality.Tests.Drawing
{
	public class VertexArrayTest
	{
		[Test] public void EmptyArray()
		{
			VertexArray<VertexC1P3> typedArray = new VertexArray<VertexC1P3>();
			IVertexArray abstractArray = typedArray;

			Assert.AreEqual(0, typedArray.Count);
			Assert.AreEqual(VertexDeclaration.Get<VertexC1P3>(), typedArray.Declaration);
			Assert.IsNotNull(typedArray.Vertices);
			Assert.AreEqual(0, typedArray.Vertices.Count);

			Assert.AreEqual(0, abstractArray.Count);
			Assert.AreEqual(VertexDeclaration.Get<VertexC1P3>(), abstractArray.Declaration);
			Assert.IsNotNull(abstractArray.GetTypedData<VertexC1P3>());
			Assert.AreEqual(0, abstractArray.GetTypedData<VertexC1P3>().Count);
		}
		[Test] public void VertexAccess()
		{
			VertexArray<VertexC1P3> typedArray = new VertexArray<VertexC1P3>();
			IVertexArray abstractArray = typedArray;

			typedArray.Vertices.Add(new VertexC1P3 { Color = new ColorRgba(0) });
			typedArray.Vertices.Add(new VertexC1P3 { Color = new ColorRgba(1) });
			typedArray.Vertices.Add(new VertexC1P3 { Color = new ColorRgba(2) });
			typedArray.Vertices.Add(new VertexC1P3 { Color = new ColorRgba(3) });

			Assert.AreEqual(4, typedArray.Count);
			Assert.AreEqual(4, typedArray.Vertices.Count);
			Assert.AreEqual(new ColorRgba(0), typedArray.Vertices[0].Color);
			Assert.AreEqual(new ColorRgba(1), typedArray.Vertices[1].Color);
			Assert.AreEqual(new ColorRgba(2), typedArray.Vertices[2].Color);
			Assert.AreEqual(new ColorRgba(3), typedArray.Vertices[3].Color);

			Assert.AreEqual(4, abstractArray.Count);
			Assert.AreEqual(4, abstractArray.GetTypedData<VertexC1P3>().Count);
			Assert.AreEqual(new ColorRgba(0), abstractArray.GetTypedData<VertexC1P3>()[0].Color);
			Assert.AreEqual(new ColorRgba(1), abstractArray.GetTypedData<VertexC1P3>()[1].Color);
			Assert.AreEqual(new ColorRgba(2), abstractArray.GetTypedData<VertexC1P3>()[2].Color);
			Assert.AreEqual(new ColorRgba(3), abstractArray.GetTypedData<VertexC1P3>()[3].Color);

			typedArray.Clear();

			Assert.AreEqual(0, typedArray.Count);
			Assert.AreEqual(0, typedArray.Vertices.Count);
			Assert.AreEqual(0, abstractArray.Count);
			Assert.AreEqual(0, abstractArray.GetTypedData<VertexC1P3>().Count);
		}
		[Test] public void Locking()
		{
			VertexArray<VertexC1P3> typedArray = new VertexArray<VertexC1P3>();
			IVertexArray abstractArray = typedArray;

			typedArray.Vertices.Add(new VertexC1P3 { Color = new ColorRgba(0) });
			typedArray.Vertices.Add(new VertexC1P3 { Color = new ColorRgba(1) });
			typedArray.Vertices.Add(new VertexC1P3 { Color = new ColorRgba(2) });
			typedArray.Vertices.Add(new VertexC1P3 { Color = new ColorRgba(3) });

			// Assert that we can retrieve all data via unmanaged pointer access
			VertexDeclaration layout = typedArray.Declaration;
			int vertexSize = layout.Size;
			int colorElementIndex = layout.Elements.IndexOfFirst(item => item.Role == VertexElementRole.Color);
			int colorOffset = (int)layout.Elements[colorElementIndex].Offset;
			using (PinnedArrayHandle locked = typedArray.Lock())
			{
				Assert.AreEqual(new ColorRgba(0), ReadColor(locked.Address, vertexSize * 0 + colorOffset));
				Assert.AreEqual(new ColorRgba(1), ReadColor(locked.Address, vertexSize * 1 + colorOffset));
				Assert.AreEqual(new ColorRgba(2), ReadColor(locked.Address, vertexSize * 2 + colorOffset));
				Assert.AreEqual(new ColorRgba(3), ReadColor(locked.Address, vertexSize * 3 + colorOffset));
			}
			using (PinnedArrayHandle locked = abstractArray.Lock())
			{
				Assert.AreEqual(new ColorRgba(0), ReadColor(locked.Address, vertexSize * 0 + colorOffset));
				Assert.AreEqual(new ColorRgba(1), ReadColor(locked.Address, vertexSize * 1 + colorOffset));
				Assert.AreEqual(new ColorRgba(2), ReadColor(locked.Address, vertexSize * 2 + colorOffset));
				Assert.AreEqual(new ColorRgba(3), ReadColor(locked.Address, vertexSize * 3 + colorOffset));
			}

			// Make sure that our locks released properly, i.e. allowing the array to be garbage collected
			WeakReference weakRefToLockedData = new WeakReference(typedArray.Vertices.Data);
			Assert.IsTrue(weakRefToLockedData.IsAlive);
			typedArray = null;
			abstractArray = null;
			GC.Collect();
			Assert.IsFalse(weakRefToLockedData.IsAlive);
		}

		private static ColorRgba ReadColor(IntPtr address, int offset)
		{
			return (ColorRgba)Marshal.PtrToStructure(
				IntPtr.Add(address, offset), 
				typeof(ColorRgba));
		}
	}
}
