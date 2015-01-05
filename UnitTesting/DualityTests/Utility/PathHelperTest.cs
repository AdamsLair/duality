using System;
using System.Diagnostics;
using System.IO;
using NUnit.Framework;

namespace Duality.Tests.Utility
{
	[TestFixture]
	public class PathHelperTest
	{
		[Test] public void FileHashEquality()
		{
			byte[] buffer = new byte[1024 * 1024];
			MathF.Rnd.NextBytes(buffer);
			try
			{
				File.WriteAllText("Test.txt", BitConverter.ToString(buffer));
				
				Assert.AreEqual(PathHelper.GetFileHash("Test.txt"), PathHelper.GetFileHash("Test.txt"));
			}
			finally
			{
				if (File.Exists("Test.txt"))
					File.Delete("Test.txt");
			}
		}
		[Test] public void FileEquality()
		{
			byte[] bufferA = new byte[1024 * 1024];
			MathF.Rnd.NextBytes(bufferA);

			byte[] bufferB = new byte[1024 * 1024];
			Buffer.BlockCopy(bufferA, 0, bufferB, 0, bufferA.Length);
			bufferB[0] = (byte)(bufferA[0] ^ 1);

			try
			{
				File.WriteAllText("Test1.txt", BitConverter.ToString(bufferA));
				File.WriteAllText("Test2.txt", BitConverter.ToString(bufferA));
				File.WriteAllText("Test3.txt", BitConverter.ToString(bufferB));
				
				Assert.IsTrue(PathHelper.FilesEqual("Test1.txt", "Test1.txt"));
				Assert.IsTrue(PathHelper.FilesEqual("Test1.txt", "Test2.txt"));
				Assert.IsFalse(PathHelper.FilesEqual("Test1.txt", "Test3.txt"));
			}
			finally
			{
				if (File.Exists("Test1.txt")) File.Delete("Test1.txt");
				if (File.Exists("Test2.txt")) File.Delete("Test2.txt");
				if (File.Exists("Test3.txt")) File.Delete("Test3.txt");
			}
		}
	}
}
