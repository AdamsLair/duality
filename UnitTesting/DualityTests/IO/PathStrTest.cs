using System;
using System.Diagnostics;
using System.IO;

using NUnit.Framework;

using Duality.IO;

namespace Duality.Tests.IO
{
	[TestFixture]
	public class PathStrTest
	{
		private static readonly string Whitespace = " ";
		private static readonly string[] InvalidPathSamples = new string[]
		{
			"C:/Test>Path/SomeFile.txt",
			"C:/Test<Path/SomeFile.txt",
			"C:/Test|Path/SomeFile.txt",
			"C:/Test\"Path/SomeFile.txt",
			"C:/Test" + '\x01' + "Path/SomeFile.txt"
		};

		[Test] public void IsPathRooted()
		{
			// Absolute paths
			Assert.IsTrue(PathStr.IsPathRooted(@"C:\TestPath\SomeFile.txt"));
			Assert.IsTrue(PathStr.IsPathRooted(@"C:/TestPath/SomeFile.txt"));
			Assert.IsTrue(PathStr.IsPathRooted(@"C:/TestPath\SomeFile.txt"));
			Assert.IsTrue(PathStr.IsPathRooted(@"\TestPath\SomeFile.txt"));
			Assert.IsTrue(PathStr.IsPathRooted(@"/TestPath/SomeFile.txt"));
			Assert.IsTrue(PathStr.IsPathRooted(@"/TestPath\SomeFile.txt"));

			// Relative paths
			Assert.IsFalse(PathStr.IsPathRooted(@"TestPath\SomeFile.txt"));
			Assert.IsFalse(PathStr.IsPathRooted(@"TestPath/SomeFile.txt"));

			// Null and empty paths
			Assert.IsFalse(PathStr.IsPathRooted(null));
			Assert.IsFalse(PathStr.IsPathRooted(string.Empty));
			Assert.IsFalse(PathStr.IsPathRooted(Whitespace));

			// Invalid paths
			foreach (string invalidPath in InvalidPathSamples)
			{
				Assert.Throws<ArgumentException>(() => PathStr.IsPathRooted(invalidPath));
			}
		}
		[Test] public void Combine()
		{
			char sep = PathStr.DirectorySeparatorChar;

			// Various combinations of relative and absolute paths
			AssertCombineOverloads(
				@"First" + sep + @"Second", 
				@"First",
				@"Second");
			AssertCombineOverloads(
				@"First" + sep + @"Second.txt", 
				@"First", 
				@"Second.txt");
			AssertCombineOverloads(
				@"First.txt" + sep + @"Second", 
				@"First.txt", 
				@"Second");
			AssertCombineOverloads(
				@"First\Second" + sep + @"Third",
				@"First\Second",
				@"Third");
			AssertCombineOverloads(
				@"First" + sep + @"Second\Third", 
				@"First",
				@"Second\Third");
			AssertCombineOverloads(
				@"C:\Root" + sep + "Second", 
				@"C:\Root",
				@"Second");
			AssertCombineOverloads(
				@"C:\Root", 
				@"First",
				@"C:\Root");

			// Null and empty paths
			AssertCombineOverloads(@"First", @"First", null);
			AssertCombineOverloads(@"First", @"First", string.Empty);
			AssertCombineOverloads(@"First", @"First", Whitespace);
			AssertCombineOverloads(@"Second", null, @"Second");
			AssertCombineOverloads(@"Second", string.Empty, @"Second");
			AssertCombineOverloads(@"Second", Whitespace, @"Second");
			AssertCombineOverloads(string.Empty, null, null);
			AssertCombineOverloads(string.Empty, null, string.Empty);
			AssertCombineOverloads(string.Empty, null, Whitespace);
			AssertCombineOverloads(string.Empty, string.Empty, null);
			AssertCombineOverloads(string.Empty, string.Empty, string.Empty);
			AssertCombineOverloads(string.Empty, string.Empty, Whitespace);
			AssertCombineOverloads(string.Empty, Whitespace, null);
			AssertCombineOverloads(string.Empty, Whitespace, string.Empty);
			AssertCombineOverloads(string.Empty, Whitespace, Whitespace);

			// Invalid paths
			foreach (string invalidPath in InvalidPathSamples)
			{
				AssertCombineOverloadsThrow<ArgumentException>(invalidPath, @"Second");
				AssertCombineOverloadsThrow<ArgumentException>(@"First", invalidPath);
			}
		}
		[Test] public void GetDirectoryName()
		{
			// Retrieving directory names from various paths
			Assert.AreEqual(string.Empty,  PathStr.GetDirectoryName(@"C:"));
			Assert.AreEqual(@"C:",         PathStr.GetDirectoryName(@"C:\"));
			Assert.AreEqual(string.Empty,  PathStr.GetDirectoryName(@"File"));
			Assert.AreEqual(string.Empty,  PathStr.GetDirectoryName(@"File.ext"));
			Assert.AreEqual(string.Empty,  PathStr.GetDirectoryName(@"File.multi.ext"));
			Assert.AreEqual(@"C:",         PathStr.GetDirectoryName(@"C:\Folder"));
			Assert.AreEqual(@"C:\Folder",  PathStr.GetDirectoryName(@"C:\Folder\"));
			Assert.AreEqual(@"C:\Folder",  PathStr.GetDirectoryName(@"C:\Folder\File.ext"));
			Assert.AreEqual(@"C:\Folder",  PathStr.GetDirectoryName(@"C:\Folder\File.multi.ext"));
			Assert.AreEqual(@"\Folder",    PathStr.GetDirectoryName(@"\Folder\"));
			Assert.AreEqual(@"\Folder",    PathStr.GetDirectoryName(@"\Folder\File.ext"));
			Assert.AreEqual(@"Folder",     PathStr.GetDirectoryName(@"Folder\File.ext"));
			Assert.AreEqual(@"Folder.txt", PathStr.GetDirectoryName(@"Folder.txt\File.ext"));
			Assert.AreEqual(@"\Folder",    PathStr.GetDirectoryName(@"\Folder\File.multi.ext"));
			Assert.AreEqual(@"Folder",     PathStr.GetDirectoryName(@"Folder\File.multi.ext"));
			Assert.AreEqual(@"Folder.txt", PathStr.GetDirectoryName(@"Folder.txt\File.multi.ext"));

			// Null and empty paths
			Assert.AreEqual(string.Empty, PathStr.GetDirectoryName(null));
			Assert.AreEqual(string.Empty, PathStr.GetDirectoryName(string.Empty));
			Assert.AreEqual(string.Empty, PathStr.GetDirectoryName(Whitespace));

			// Invalid paths
			foreach (string invalidPath in InvalidPathSamples)
			{
				Assert.Throws<ArgumentException>(() => PathStr.GetDirectoryName(invalidPath));
			}
		}
		[Test] public void GetFileName()
		{
			// Retrieving file names from various paths
			Assert.AreEqual(@"C:",              PathStr.GetFileName(@"C:"));
			Assert.AreEqual(string.Empty,       PathStr.GetFileName(@"C:\"));
			Assert.AreEqual(@"File",            PathStr.GetFileName(@"File"));
			Assert.AreEqual(@"File.ext",        PathStr.GetFileName(@"File.ext"));
			Assert.AreEqual(@"File.multi.ext",  PathStr.GetFileName(@"File.multi.ext"));
			Assert.AreEqual(@"Folder",          PathStr.GetFileName(@"C:\Folder"));
			Assert.AreEqual(string.Empty,       PathStr.GetFileName(@"C:\Folder\"));
			Assert.AreEqual(@"File.ext",        PathStr.GetFileName(@"C:\Folder\File.ext"));
			Assert.AreEqual(@"File.multi.ext",  PathStr.GetFileName(@"C:\Folder\File.multi.ext"));
			Assert.AreEqual(string.Empty,       PathStr.GetFileName(@"\Folder\"));
			Assert.AreEqual(@"File.ext",        PathStr.GetFileName(@"\Folder\File.ext"));
			Assert.AreEqual(@"File.ext",        PathStr.GetFileName(@"Folder\File.ext"));
			Assert.AreEqual(@"File.ext",        PathStr.GetFileName(@"Folder.txt\File.ext"));
			Assert.AreEqual(@"File.multi.ext",  PathStr.GetFileName(@"\Folder\File.multi.ext"));
			Assert.AreEqual(@"File.multi.ext",  PathStr.GetFileName(@"Folder\File.multi.ext"));
			Assert.AreEqual(@"File.multi.ext",  PathStr.GetFileName(@"Folder.txt\File.multi.ext"));

			// Null and empty paths
			Assert.AreEqual(string.Empty, PathStr.GetFileName(null));
			Assert.AreEqual(string.Empty, PathStr.GetFileName(string.Empty));
			Assert.AreEqual(string.Empty, PathStr.GetFileName(Whitespace));

			// Invalid paths
			foreach (string invalidPath in InvalidPathSamples)
			{
				Assert.Throws<ArgumentException>(() => PathStr.GetFileName(invalidPath));
			}
		}
		[Test] public void GetFileNameWithoutExtension()
		{
			// Retrieving filenames without extensions from various paths
			Assert.AreEqual(@"C:",          PathStr.GetFileNameWithoutExtension(@"C:"));
			Assert.AreEqual(string.Empty,   PathStr.GetFileNameWithoutExtension(@"C:\"));
			Assert.AreEqual(@"File",        PathStr.GetFileNameWithoutExtension(@"File"));
			Assert.AreEqual(@"File",        PathStr.GetFileNameWithoutExtension(@"File.ext"));
			Assert.AreEqual(@"File.multi",  PathStr.GetFileNameWithoutExtension(@"File.multi.ext"));
			Assert.AreEqual(@"Folder",      PathStr.GetFileNameWithoutExtension(@"C:\Folder"));
			Assert.AreEqual(string.Empty,   PathStr.GetFileNameWithoutExtension(@"C:\Folder\"));
			Assert.AreEqual(@"File",        PathStr.GetFileNameWithoutExtension(@"C:\Folder\File.ext"));
			Assert.AreEqual(@"File.multi",  PathStr.GetFileNameWithoutExtension(@"C:\Folder\File.multi.ext"));
			Assert.AreEqual(string.Empty,   PathStr.GetFileNameWithoutExtension(@"\Folder\"));
			Assert.AreEqual(@"File",        PathStr.GetFileNameWithoutExtension(@"\Folder\File.ext"));
			Assert.AreEqual(@"File",        PathStr.GetFileNameWithoutExtension(@"Folder\File.ext"));
			Assert.AreEqual(@"File",        PathStr.GetFileNameWithoutExtension(@"Folder.txt\File.ext"));
			Assert.AreEqual(@"File.multi",  PathStr.GetFileNameWithoutExtension(@"\Folder\File.multi.ext"));
			Assert.AreEqual(@"File.multi",  PathStr.GetFileNameWithoutExtension(@"Folder\File.multi.ext"));
			Assert.AreEqual(@"File.multi",  PathStr.GetFileNameWithoutExtension(@"Folder.txt\File.multi.ext"));

			// Retrieving filenames without multi-extensions from various paths
			Assert.AreEqual(@"C:",         PathStr.GetFileNameWithoutExtension(@"C:", true));
			Assert.AreEqual(string.Empty,  PathStr.GetFileNameWithoutExtension(@"C:\", true));
			Assert.AreEqual(@"File",       PathStr.GetFileNameWithoutExtension(@"File", true));
			Assert.AreEqual(@"File",       PathStr.GetFileNameWithoutExtension(@"File.ext", true));
			Assert.AreEqual(@"File",       PathStr.GetFileNameWithoutExtension(@"File.multi.ext", true));
			Assert.AreEqual(@"Folder",     PathStr.GetFileNameWithoutExtension(@"C:\Folder", true));
			Assert.AreEqual(string.Empty,  PathStr.GetFileNameWithoutExtension(@"C:\Folder\", true));
			Assert.AreEqual(@"File",       PathStr.GetFileNameWithoutExtension(@"C:\Folder\File.ext", true));
			Assert.AreEqual(@"File",       PathStr.GetFileNameWithoutExtension(@"C:\Folder\File.multi.ext", true));
			Assert.AreEqual(string.Empty,  PathStr.GetFileNameWithoutExtension(@"\Folder\", true));
			Assert.AreEqual(@"File",       PathStr.GetFileNameWithoutExtension(@"\Folder\File.ext", true));
			Assert.AreEqual(@"File",       PathStr.GetFileNameWithoutExtension(@"Folder\File.ext", true));
			Assert.AreEqual(@"File",       PathStr.GetFileNameWithoutExtension(@"Folder.txt\File.ext", true));
			Assert.AreEqual(@"File",       PathStr.GetFileNameWithoutExtension(@"\Folder\File.multi.ext", true));
			Assert.AreEqual(@"File",       PathStr.GetFileNameWithoutExtension(@"Folder\File.multi.ext", true));
			Assert.AreEqual(@"File",       PathStr.GetFileNameWithoutExtension(@"Folder.txt\File.multi.ext", true));

			// Null and empty paths
			Assert.AreEqual(string.Empty, PathStr.GetFileNameWithoutExtension(null));
			Assert.AreEqual(string.Empty, PathStr.GetFileNameWithoutExtension(string.Empty));
			Assert.AreEqual(string.Empty, PathStr.GetFileNameWithoutExtension(Whitespace));

			// Invalid paths
			foreach (string invalidPath in InvalidPathSamples)
			{
				Assert.Throws<ArgumentException>(() => PathStr.GetFileNameWithoutExtension(invalidPath));
			}
		}
		[Test] public void GetExtension()
		{
			// Retrieving extensions from various paths
			Assert.AreEqual(string.Empty,  PathStr.GetExtension(@"C:"));
			Assert.AreEqual(string.Empty,  PathStr.GetExtension(@"C:\"));
			Assert.AreEqual(string.Empty,  PathStr.GetExtension(@"File"));
			Assert.AreEqual(@".ext",       PathStr.GetExtension(@"File.ext"));
			Assert.AreEqual(@".ext",       PathStr.GetExtension(@"File.multi.ext"));
			Assert.AreEqual(string.Empty,  PathStr.GetExtension(@"C:\Folder"));
			Assert.AreEqual(string.Empty,  PathStr.GetExtension(@"C:\Folder\"));
			Assert.AreEqual(@".ext",       PathStr.GetExtension(@"C:\Folder\File.ext"));
			Assert.AreEqual(@".ext",       PathStr.GetExtension(@"C:\Folder\File.multi.ext"));
			Assert.AreEqual(string.Empty,  PathStr.GetExtension(@"\Folder\"));
			Assert.AreEqual(@".ext",       PathStr.GetExtension(@"\Folder\File.ext"));
			Assert.AreEqual(@".ext",       PathStr.GetExtension(@"Folder\File.ext"));
			Assert.AreEqual(@".ext",       PathStr.GetExtension(@"Folder.txt\File.ext"));
			Assert.AreEqual(@".ext",       PathStr.GetExtension(@"\Folder\File.multi.ext"));
			Assert.AreEqual(@".ext",       PathStr.GetExtension(@"Folder\File.multi.ext"));
			Assert.AreEqual(@".ext",       PathStr.GetExtension(@"Folder.txt\File.multi.ext"));
			
			// Retrieving multi-extensions from various paths
			Assert.AreEqual(string.Empty,  PathStr.GetExtension(@"C:", true));
			Assert.AreEqual(string.Empty,  PathStr.GetExtension(@"C:\", true));
			Assert.AreEqual(string.Empty,  PathStr.GetExtension(@"File", true));
			Assert.AreEqual(@".ext",       PathStr.GetExtension(@"File.ext", true));
			Assert.AreEqual(@".multi.ext", PathStr.GetExtension(@"File.multi.ext", true));
			Assert.AreEqual(string.Empty,  PathStr.GetExtension(@"C:\Folder", true));
			Assert.AreEqual(string.Empty,  PathStr.GetExtension(@"C:\Folder\", true));
			Assert.AreEqual(@".ext",       PathStr.GetExtension(@"C:\Folder\File.ext", true));
			Assert.AreEqual(@".multi.ext", PathStr.GetExtension(@"C:\Folder\File.multi.ext", true));
			Assert.AreEqual(string.Empty,  PathStr.GetExtension(@"\Folder\", true));
			Assert.AreEqual(@".ext",       PathStr.GetExtension(@"\Folder\File.ext", true));
			Assert.AreEqual(@".ext",       PathStr.GetExtension(@"Folder\File.ext", true));
			Assert.AreEqual(@".ext",       PathStr.GetExtension(@"Folder.txt\File.ext", true));
			Assert.AreEqual(@".multi.ext", PathStr.GetExtension(@"\Folder\File.multi.ext", true));
			Assert.AreEqual(@".multi.ext", PathStr.GetExtension(@"Folder\File.multi.ext", true));
			Assert.AreEqual(@".multi.ext", PathStr.GetExtension(@"Folder.txt\File.multi.ext", true));

			// Null and empty paths
			Assert.AreEqual(string.Empty, PathStr.GetExtension(null));
			Assert.AreEqual(string.Empty, PathStr.GetExtension(string.Empty));
			Assert.AreEqual(string.Empty, PathStr.GetExtension(Whitespace));

			// Invalid paths
			foreach (string invalidPath in InvalidPathSamples)
			{
				Assert.Throws<ArgumentException>(() => PathStr.GetExtension(invalidPath));
			}
		}

		private static void AssertCombineOverloads(string expected, string first, string second)
		{
			Assert.AreEqual(expected, PathStr.Combine(first, second));
			Assert.AreEqual(expected, PathStr.Combine(new string[] { first, second }));
		}
		private static void AssertCombineOverloadsThrow<T>(string first, string second) where T : Exception
		{
			Assert.Throws<T>(() => PathStr.Combine(first, second));
			Assert.Throws<T>(() => PathStr.Combine(new string[] { first, second }));
		}
	}
}
