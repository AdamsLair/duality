using System;
using System.IO;
using System.Text;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Diagnostics;

using Duality;
using Duality.Resources;
using Duality.Drawing;
using Duality.Tests.Properties;

using NUnit.Framework;

namespace Duality.Tests.Drawing
{
	public class VertexSegmentTest
	{
		[Test] public void Constructor()
		{
			// Create a vertex array and segment
			VertexC1P3[] array = new VertexC1P3[10];
			VertexSegment<VertexC1P3> segment = new VertexSegment<VertexC1P3>(
				array, 
				1, 
				array.Length - 2);

			// Assert that all the segment parameters are set properly
			Assert.AreEqual(1, segment.Offset);
			Assert.AreEqual(array.Length - 2, segment.Length);
		}
		[Test] public void IndexerAccess()
		{
			// Create a vertex array and segment
			VertexC1P3[] array = new VertexC1P3[10];
			VertexSegment<VertexC1P3> segment = new VertexSegment<VertexC1P3>(
				array, 
				1, 
				array.Length - 2);
			
			// Fill the vertex array with data
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Color = new ColorRgba(i);
			}

			// Assert that we can access the vertex data via indexer
			for (int i = 0; i < segment.Length; i++)
			{
				Assert.AreEqual(array[i + 1].Color, segment[i].Color);
			}

			// Assert that we can change data via indexer
			for (int i = 0; i < segment.Length; i++)
			{
				ColorRgba newColor = new ColorRgba(1234);
				segment[i] = new VertexC1P3
				{
					Color = newColor
				};
				Assert.AreEqual(newColor, segment[i].Color);
				Assert.AreEqual(newColor, array[i + 1].Color);
			}
		}
	}
}
