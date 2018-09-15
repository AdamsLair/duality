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
	public class VertexBatchTest
	{
		[Test] public void EmptyArray()
		{
			VertexBatch<VertexC1P3> typedBatch = new VertexBatch<VertexC1P3>();
			IVertexBatch abstractBatch = typedBatch;

			Assert.AreEqual(0, typedBatch.Count);
			Assert.AreEqual(VertexDeclaration.Get<VertexC1P3>(), typedBatch.Declaration);
			Assert.IsNotNull(typedBatch.Vertices);
			Assert.AreEqual(0, typedBatch.Vertices.Count);

			Assert.AreEqual(0, abstractBatch.Count);
			Assert.AreEqual(VertexDeclaration.Get<VertexC1P3>(), abstractBatch.Declaration);
		}
		[Test] public void VertexAccess()
		{
			VertexBatch<VertexC1P3> typedBatch = new VertexBatch<VertexC1P3>();
			IVertexBatch abstractBatch = typedBatch;

			typedBatch.Vertices.Add(new VertexC1P3 { Color = new ColorRgba(0) });
			typedBatch.Vertices.Add(new VertexC1P3 { Color = new ColorRgba(1) });
			typedBatch.Vertices.Add(new VertexC1P3 { Color = new ColorRgba(2) });
			typedBatch.Vertices.Add(new VertexC1P3 { Color = new ColorRgba(3) });

			Assert.AreEqual(4, typedBatch.Count);
			Assert.AreEqual(4, typedBatch.Vertices.Count);
			Assert.AreEqual(new ColorRgba(0), typedBatch.Vertices[0].Color);
			Assert.AreEqual(new ColorRgba(1), typedBatch.Vertices[1].Color);
			Assert.AreEqual(new ColorRgba(2), typedBatch.Vertices[2].Color);
			Assert.AreEqual(new ColorRgba(3), typedBatch.Vertices[3].Color);

			Assert.AreEqual(4, abstractBatch.Count);

			typedBatch.Clear();

			Assert.AreEqual(0, typedBatch.Count);
			Assert.AreEqual(0, typedBatch.Vertices.Count);
			Assert.AreEqual(0, abstractBatch.Count);
		}
		[Test] public void Locking()
		{
			VertexBatch<VertexC1P3> typedBatch = new VertexBatch<VertexC1P3>();
			IVertexBatch abstractBatch = typedBatch;

			typedBatch.Vertices.Add(new VertexC1P3 { Color = new ColorRgba(0) });
			typedBatch.Vertices.Add(new VertexC1P3 { Color = new ColorRgba(1) });
			typedBatch.Vertices.Add(new VertexC1P3 { Color = new ColorRgba(2) });
			typedBatch.Vertices.Add(new VertexC1P3 { Color = new ColorRgba(3) });

			// Assert that we can retrieve all data via unmanaged pointer access
			VertexDeclaration layout = typedBatch.Declaration;
			int vertexSize = layout.Size;
			int colorElementIndex = layout.Elements.IndexOfFirst(item => item.FieldName == VertexDeclaration.ShaderFieldPrefix + "Color");
			int colorOffset = (int)layout.Elements[colorElementIndex].Offset;
			using (PinnedArrayHandle locked = typedBatch.Lock())
			{
				Assert.AreEqual(new ColorRgba(0), ReadColor(locked.Address, vertexSize * 0 + colorOffset));
				Assert.AreEqual(new ColorRgba(1), ReadColor(locked.Address, vertexSize * 1 + colorOffset));
				Assert.AreEqual(new ColorRgba(2), ReadColor(locked.Address, vertexSize * 2 + colorOffset));
				Assert.AreEqual(new ColorRgba(3), ReadColor(locked.Address, vertexSize * 3 + colorOffset));
			}
			using (PinnedArrayHandle locked = abstractBatch.Lock())
			{
				Assert.AreEqual(new ColorRgba(0), ReadColor(locked.Address, vertexSize * 0 + colorOffset));
				Assert.AreEqual(new ColorRgba(1), ReadColor(locked.Address, vertexSize * 1 + colorOffset));
				Assert.AreEqual(new ColorRgba(2), ReadColor(locked.Address, vertexSize * 2 + colorOffset));
				Assert.AreEqual(new ColorRgba(3), ReadColor(locked.Address, vertexSize * 3 + colorOffset));
			}

			// Make sure that our locks released properly, i.e. allowing the array to be garbage collected
			WeakReference weakRefToLockedData = new WeakReference(typedBatch.Vertices.Data);
			Assert.IsTrue(weakRefToLockedData.IsAlive);
			typedBatch = null;
			abstractBatch = null;
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
