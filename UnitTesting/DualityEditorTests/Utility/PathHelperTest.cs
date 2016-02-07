using System;
using System.Diagnostics;
using System.IO;

using NUnit.Framework;

using Duality.Editor;
using Duality.IO;

namespace Duality.Editor.Tests
{
	[TestFixture]
	public class PathHelperTest
	{
		[Test] public void MakeFilePathRelative()
		{
			// Making a path relative should be the exact negation of a combine path operation
			Assert.IsTrue(PathOp.ArePathsEqual(
				"File.txt", 
				PathHelper.MakeFilePathRelative(PathOp.Combine("DirA", "File.txt"), "DirA")));

			// Go a few directories up and down again when necessary
			Assert.IsTrue(PathOp.ArePathsEqual(
				PathOp.Combine("..", "..", "DirC", "File.txt"), 
				PathHelper.MakeFilePathRelative(PathOp.Combine("DirC", "File.txt"), PathOp.Combine("DirA", "DirB"))));

			// Fail explicitly when attempting to switch drives
			Assert.IsNull(PathHelper.MakeFilePathRelative(PathOp.Combine("D:", "DirC", "File.txt"), PathOp.Combine("C:", "DirA", "DirB")));
		}
	}
}
