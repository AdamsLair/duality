using System;
using System.Collections.Generic;
using System.Linq;

using Duality;
using Duality.Drawing;
using Duality.Tests.Properties;

using NUnit.Framework;

namespace Duality.Tests.Drawing
{
	public class VertexSliceTest
	{
		[Test] public void Constructor()
		{
			// Create a vertex array and slice
			VertexC1P3[] array = new VertexC1P3[10];
			VertexSlice<VertexC1P3> slice = new VertexSlice<VertexC1P3>(
				array, 
				1, 
				array.Length - 2);

			// Assert that all the slice parameters are set properly
			Assert.AreEqual(1, slice.Offset);
			Assert.AreEqual(array.Length - 2, slice.Length);
		}
		[Test] public void IndexerAccess()
		{
			// Create a vertex array and slice
			VertexC1P3[] array = new VertexC1P3[10];
			VertexSlice<VertexC1P3> slice = new VertexSlice<VertexC1P3>(
				array, 
				1, 
				array.Length - 2);
			
			// Fill the vertex array with data
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Color = new ColorRgba(i);
			}

			// Assert that we can access the vertex data via indexer
			for (int i = 0; i < slice.Length; i++)
			{
				Assert.AreEqual(array[i + 1].Color, slice[i].Color);
			}

			// Assert that we can change data via indexer
			for (int i = 0; i < slice.Length; i++)
			{
				ColorRgba newColor = new ColorRgba(1234);
				slice[i] = new VertexC1P3
				{
					Color = newColor
				};
				Assert.AreEqual(newColor, slice[i].Color);
				Assert.AreEqual(newColor, array[i + 1].Color);
			}
		}
	}
}
