using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

using Duality;

using NUnit.Framework;

namespace Duality.Tests.Utility
{
	[TestFixture]
	public class GridTest
	{
		[Test] public void Basics()
		{
			Grid<int> grid = new Grid<int>(2, 2);
			grid[0, 0] = 0;
			grid[1, 0] = 1;
			grid[0, 1] = 2;
			grid[1, 1] = 3;

			Assert.AreEqual(4, grid.Capacity);
			Assert.AreEqual(2, grid.Width);
			Assert.AreEqual(2, grid.Height);
			CollectionAssert.AreEqual(new[] { 
				0, 1, 
				2, 3 }, grid);

			Assert.IsTrue(grid.Contains(3));
			Assert.AreEqual(new Point(1, 1), grid.IndexOf(3));
			Assert.IsTrue(grid.Remove(3));
			Assert.IsFalse(grid.Remove(3));
			Assert.IsFalse(grid.Contains(3));
			Assert.AreEqual(new Point(-1, -1), grid.IndexOf(3));
			CollectionAssert.AreEqual(new[] { 
				0, 1, 
				2, 0 }, grid);

			grid.Resize(4, 4, Alignment.Center);
			Assert.AreEqual(16, grid.Capacity);
			Assert.AreEqual(4, grid.Width);
			Assert.AreEqual(4, grid.Height);
			CollectionAssert.AreEqual(new[] { 
				0, 0, 0, 0,
				0, 0, 1, 0,
				0, 2, 0, 0,
				0, 0, 0, 0 }, grid);

			grid.ShrinkToFit();
			CollectionAssert.AreEqual(new[] { 
				0, 1, 
				2, 0 }, grid);

			grid.Clear();
			CollectionAssert.AreEqual(Enumerable.Repeat(0, grid.Capacity), grid);
		}
		[Test] public void Resize()
		{
			Grid<int> grid = new Grid<int>(2, 2);
			grid[0, 0] = 0;
			grid[1, 0] = 1;
			grid[0, 1] = 2;
			grid[1, 1] = 3;

			Assert.AreEqual(4, grid.Capacity);
			Assert.AreEqual(2, grid.Width);
			Assert.AreEqual(2, grid.Height);
			CollectionAssert.AreEqual(new[] { 
				0, 1, 
				2, 3 }, grid);

			grid.Resize(4, 4, Alignment.Center);
			Assert.AreEqual(16, grid.Capacity);
			Assert.AreEqual(4, grid.Width);
			Assert.AreEqual(4, grid.Height);
			CollectionAssert.AreEqual(new[] { 
				0, 0, 0, 0,
				0, 0, 1, 0,
				0, 2, 3, 0,
				0, 0, 0, 0 }, grid);

			grid.ShrinkToFit();
			CollectionAssert.AreEqual(new[] { 
				0, 1, 
				2, 3 }, grid);
		}
		[Test] public void AssumeRect()
		{
			Grid<int> grid = new Grid<int>(2, 2);
			grid[0, 0] = 0;
			grid[1, 0] = 1;
			grid[0, 1] = 2;
			grid[1, 1] = 3;

			Assert.AreEqual(4, grid.Capacity);
			Assert.AreEqual(2, grid.Width);
			Assert.AreEqual(2, grid.Height);
			CollectionAssert.AreEqual(new[] { 
				0, 1, 
				2, 3 }, grid);

			grid.AssumeRect(1, 1, 4, 4);
			Assert.AreEqual(16, grid.Capacity);
			Assert.AreEqual(4, grid.Width);
			Assert.AreEqual(4, grid.Height);
			CollectionAssert.AreEqual(new[] { 
				3, 0, 0, 0,
				0, 0, 0, 0,
				0, 0, 0, 0,
				0, 0, 0, 0 }, grid);

			grid.ShrinkToFit();
			CollectionAssert.AreEqual(new[] { 
				3 }, grid);

			Random rnd = new Random();
			grid.Resize(4, 4);
			for (int i = 0; i < grid.Capacity; i++) grid.RawData[i] = rnd.Next();
			Grid<int> grid2 = new Grid<int>(6, 6);
			grid.CopyTo(grid2, 1, 1);
			Grid<int> grid3 = new Grid<int>(grid);
			grid3.AssumeRect(-1, -1, 6, 6);
			CollectionAssert.AreEqual(grid2, grid3);
		}
		[Test] public void Objects()
		{
			Grid<string> grid = new Grid<string>(2, 2);
			grid[0, 0] = "Cell A";
			grid[1, 0] = "Cell B";
			grid[0, 1] = "Cell C";
			grid[1, 1] = "Cell D";

			grid.Remove("Cell B");
			Assert.AreEqual(null, grid[1, 0]);
		}
		[Test] public void Fill()
		{
			Grid<int> grid = new Grid<int>(4, 4);
			grid.Fill(7, 1, 1, 2, 2);
			CollectionAssert.AreEqual(new[] { 
				0, 0, 0, 0,
				0, 7, 7, 0,
				0, 7, 7, 0,
				0, 0, 0, 0 }, grid);
		}
		[Test] public void CopyConvert()
		{
			Grid<int> sourceGrid = new Grid<int>(4, 4);
			Grid<float> targetGrid = new Grid<float>(4, 4);
			sourceGrid.Fill(7, 1, 1, 2, 2);
			sourceGrid.CopyTo(targetGrid);
			CollectionAssert.AreEqual(new[] { 
				0.0f, 0.0f, 0.0f, 0.0f,
				0.0f, 7.0f, 7.0f, 0.0f,
				0.0f, 7.0f, 7.0f, 0.0f,
				0.0f, 0.0f, 0.0f, 0.0f }, targetGrid);
		}
		[Test] public void CopySelect()
		{
			Grid<int> sourceGrid = new Grid<int>(4, 4);
			Grid<string> targetGrid = new Grid<string>(4, 4);
			sourceGrid.Fill(7, 1, 1, 2, 2);
			targetGrid.Fill("Keep", 0, 0, 2, 2);
			sourceGrid.CopyTo(targetGrid, selector: (i, s) => s != "Keep" ? i.ToString() : s);
			CollectionAssert.AreEqual(new[] { 
				"Keep", "Keep", "0", "0",
				"Keep", "Keep", "7", "0",
				"0",	"7",	"7", "0",
				"0",	"0",	"0", "0" }, targetGrid);
		}
		[Test] public void Find()
		{
			Grid<int> grid = new Grid<int>(2, 2);
			grid[0, 0] = 0;
			grid[1, 0] = 1;
			grid[0, 1] = 2;
			grid[1, 1] = 3;

			Assert.AreEqual(new Point(1, 0), grid.FindIndex(i => i == 1));
			Assert.AreEqual(1, grid.Find(i => i == 1));
			CollectionAssert.AreEqual(new[] { new Point(0, 1), new Point(1, 1) }, grid.FindAllIndices(i => i > 1));
			CollectionAssert.AreEqual(new[] { 2, 3 }, grid.FindAll(i => i > 1));
		}
	}
}
