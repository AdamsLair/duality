using System;
using System.Diagnostics;
using System.IO;
using NUnit.Framework;

namespace Duality.Tests.Utility
{
	[TestFixture]
	public class PathHelperTest
	{
		[Test]
		public void FileHashEquality()
		{
			byte[] buffer = new byte[1024 * 1024];
			MathF.Rnd.NextBytes(buffer);
			try
			{
				File.WriteAllText("Test.txt", BitConverter.ToString(buffer));
				
				Assert.AreEqual(PathHelper.GetFileHash("test.txt"), PathHelper.GetFileHash("test.txt"));
			}
			finally
			{
				if (File.Exists("test.txt"))
					File.Delete("test.txt");
			}
		}
	}
}
