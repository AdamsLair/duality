using System;
using System.Collections.Generic;
using System.Linq;

using Duality;
using Duality.Drawing;

using NUnit.Framework;

namespace Duality.Tests.Drawing
{
	public class VertexBatchStoreTest
	{
		[Test] public void RentAndClear()
		{
			VertexBatchStore memory = new VertexBatchStore();

			// Repeatedly rent slices of varying types from memory
			{
				VertexSlice<VertexC1P3> slice = memory.Rent<VertexC1P3>(4);
				slice[0] = new VertexC1P3 { Color = new ColorRgba(0) };
				slice[1] = new VertexC1P3 { Color = new ColorRgba(1) };
				slice[2] = new VertexC1P3 { Color = new ColorRgba(2) };
				slice[3] = new VertexC1P3 { Color = new ColorRgba(3) };
			}
			{
				VertexSlice<VertexC1P3T2> slice = memory.Rent<VertexC1P3T2>(3);
				slice[0] = new VertexC1P3T2 { Color = new ColorRgba(4) };
				slice[1] = new VertexC1P3T2 { Color = new ColorRgba(5) };
				slice[2] = new VertexC1P3T2 { Color = new ColorRgba(6) };
			}
			{
				VertexSlice<VertexC1P3> slice = memory.Rent<VertexC1P3>(2);
				slice[0] = new VertexC1P3 { Color = new ColorRgba(7) };
				slice[1] = new VertexC1P3 { Color = new ColorRgba(8) };
			}
			{
				VertexSlice<VertexC1P3> slice = memory.Rent<VertexC1P3>(1);
				slice[0] = new VertexC1P3 { Color = new ColorRgba(9) };
			}

			// Assert correct storage property values
			Assert.AreEqual(
				1 + Math.Max(
					VertexDeclaration.Get<VertexC1P3>().TypeIndex, 
					VertexDeclaration.Get<VertexC1P3T2>().TypeIndex), 
				memory.TypeIndexCount);
			Assert.AreEqual(1, memory.GetBatchCount<VertexC1P3>());
			Assert.AreEqual(1, memory.GetBatchCount<VertexC1P3T2>());

			// Retrieve specific internal vertex arrays
			VertexBatch<VertexC1P3> batchA = memory.GetBatch<VertexC1P3>(0);
			VertexBatch<VertexC1P3T2> batchB = memory.GetBatch<VertexC1P3T2>(0);
			RawList<VertexC1P3> verticesA = batchA.Vertices;
			RawList<VertexC1P3T2> verticesB = batchB.Vertices;

			// Assert that they contain all the data we submitted in the correct order
			Assert.AreEqual(7, batchA.Count);
			Assert.AreEqual(3, batchB.Count);
			Assert.AreEqual(7, verticesA.Count);
			Assert.AreEqual(3, verticesB.Count);

			Assert.AreEqual(new ColorRgba(0), verticesA[0].Color);
			Assert.AreEqual(new ColorRgba(1), verticesA[1].Color);
			Assert.AreEqual(new ColorRgba(2), verticesA[2].Color);
			Assert.AreEqual(new ColorRgba(3), verticesA[3].Color);

			Assert.AreEqual(new ColorRgba(4), verticesB[0].Color);
			Assert.AreEqual(new ColorRgba(5), verticesB[1].Color);
			Assert.AreEqual(new ColorRgba(6), verticesB[2].Color);

			Assert.AreEqual(new ColorRgba(7), verticesA[4].Color);
			Assert.AreEqual(new ColorRgba(8), verticesA[5].Color);
			Assert.AreEqual(new ColorRgba(9), verticesA[6].Color);

			// Clear all vertices
			memory.Clear();

			// Assert correct storage property values
			Assert.AreEqual(0, memory.TypeIndexCount);
			Assert.AreEqual(0, memory.GetBatchCount<VertexC1P3>());
			Assert.AreEqual(0, memory.GetBatchCount<VertexC1P3T2>());

			// Assert that the vertices are gone, but capacity isn't
			Assert.AreEqual(0, batchA.Count);
			Assert.AreEqual(0, batchB.Count);
			Assert.AreEqual(0, verticesA.Count);
			Assert.AreEqual(0, verticesB.Count);
			Assert.GreaterOrEqual(verticesA.Capacity, 7);
			Assert.GreaterOrEqual(verticesB.Capacity, 3);
		}
		[Test] public void MaxBatchSize()
		{
			VertexBatchStore memory = new VertexBatchStore(4);

			// Rent a few memory slices, but stay below the max batch size
			{
				VertexSlice<VertexC1P3> slice = memory.Rent<VertexC1P3>(2);
				slice[0] = new VertexC1P3 { Color = new ColorRgba(0) };
				slice[1] = new VertexC1P3 { Color = new ColorRgba(1) };
			}
			{
				VertexSlice<VertexC1P3> slice = memory.Rent<VertexC1P3>(1);
				slice[0] = new VertexC1P3 { Color = new ColorRgba(2) };
			}

			// Assert that we only have one batch, with the correct contents
			Assert.AreEqual(1, memory.GetBatchCount<VertexC1P3>());
			Assert.AreEqual(0, memory.GetBatchCount<VertexC1P3T2>());

			VertexBatch<VertexC1P3> batchA = memory.GetBatch<VertexC1P3>(0);
			RawList<VertexC1P3> verticesA = batchA.Vertices;

			Assert.AreEqual(3, batchA.Count);
			Assert.AreEqual(new ColorRgba(0), verticesA[0].Color);
			Assert.AreEqual(new ColorRgba(1), verticesA[1].Color);
			Assert.AreEqual(new ColorRgba(2), verticesA[2].Color);

			// Rent a few more slices, this time exceeding max batch size
			{
				VertexSlice<VertexC1P3> slice = memory.Rent<VertexC1P3>(2);
				slice[0] = new VertexC1P3 { Color = new ColorRgba(3) };
				slice[1] = new VertexC1P3 { Color = new ColorRgba(4) };
			}
			{
				VertexSlice<VertexC1P3> slice = memory.Rent<VertexC1P3>(2);
				slice[0] = new VertexC1P3 { Color = new ColorRgba(5) };
				slice[1] = new VertexC1P3 { Color = new ColorRgba(6) };
			}

			// Assert that we now have two batches, each with the correct contents
			Assert.AreEqual(2, memory.GetBatchCount<VertexC1P3>());
			Assert.AreEqual(0, memory.GetBatchCount<VertexC1P3T2>());

			VertexBatch<VertexC1P3> batchB = memory.GetBatch<VertexC1P3>(1);
			RawList<VertexC1P3> verticesB = batchB.Vertices;

			Assert.AreEqual(3, batchA.Count);
			Assert.AreEqual(new ColorRgba(0), verticesA[0].Color);
			Assert.AreEqual(new ColorRgba(1), verticesA[1].Color);
			Assert.AreEqual(new ColorRgba(2), verticesA[2].Color);

			Assert.AreEqual(4, batchB.Count);
			Assert.AreEqual(new ColorRgba(3), verticesB[0].Color);
			Assert.AreEqual(new ColorRgba(4), verticesB[1].Color);
			Assert.AreEqual(new ColorRgba(5), verticesB[2].Color);
			Assert.AreEqual(new ColorRgba(6), verticesB[3].Color);

			// Clear all vertices
			memory.Clear();

			// Assert that the vertices are gone, but capacity isn't
			Assert.AreEqual(0, memory.GetBatchCount<VertexC1P3>());
			Assert.AreEqual(0, memory.GetBatchCount<VertexC1P3T2>());
			Assert.AreEqual(0, batchA.Count);
			Assert.AreEqual(0, batchB.Count);
			Assert.AreEqual(0, verticesA.Count);
			Assert.AreEqual(0, verticesB.Count);
			Assert.GreaterOrEqual(verticesA.Capacity, 3);
			Assert.GreaterOrEqual(verticesB.Capacity, 4);

			// Rent some vertices again
			memory.Rent<VertexC1P3>(3);
			memory.Rent<VertexC1P3>(4);

			// Assert that the same storage is re-used
			Assert.AreEqual(2, memory.GetBatchCount<VertexC1P3>());
			Assert.AreEqual(0, memory.GetBatchCount<VertexC1P3T2>());
			Assert.AreEqual(3, batchA.Count);
			Assert.AreEqual(4, batchB.Count);
			Assert.AreEqual(3, verticesA.Count);
			Assert.AreEqual(4, verticesB.Count);
		}
	}
}
