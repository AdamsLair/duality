using System;
using System.Diagnostics;
using System.IO;

using NUnit.Framework;

using Duality.IO;

namespace Duality.Tests.IO
{
	[TestFixture]
	public class PathOpTest
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
			Assert.IsTrue(PathOp.IsPathRooted(@"C:\TestPath\SomeFile.txt"));
			Assert.IsTrue(PathOp.IsPathRooted(@"C:/TestPath/SomeFile.txt"));
			Assert.IsTrue(PathOp.IsPathRooted(@"C:/TestPath\SomeFile.txt"));
			Assert.IsTrue(PathOp.IsPathRooted(@"\TestPath\SomeFile.txt"));
			Assert.IsTrue(PathOp.IsPathRooted(@"/TestPath/SomeFile.txt"));
			Assert.IsTrue(PathOp.IsPathRooted(@"/TestPath\SomeFile.txt"));

			// Relative paths
			Assert.IsFalse(PathOp.IsPathRooted(@"TestPath\SomeFile.txt"));
			Assert.IsFalse(PathOp.IsPathRooted(@"TestPath/SomeFile.txt"));

			// Null and empty paths
			Assert.IsFalse(PathOp.IsPathRooted(null));
			Assert.IsFalse(PathOp.IsPathRooted(string.Empty));
			Assert.IsFalse(PathOp.IsPathRooted(Whitespace));

			// Invalid paths
			foreach (string invalidPath in InvalidPathSamples)
			{
				Assert.Throws<ArgumentException>(() => PathOp.IsPathRooted(invalidPath));
			}
		}
		[Test] public void Combine()
		{
			char sep = PathOp.DirectorySeparatorChar;

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
			{
				PathPermutationTester tester = new PathPermutationTester(s => PathOp.GetDirectoryName(s));
				tester.AssertEqual(string.Empty,  @"C:");
				tester.AssertEqual(@"C:",         @"C:\");
				tester.AssertEqual(string.Empty,  @"File");
				tester.AssertEqual(string.Empty,  @"File.ext");
				tester.AssertEqual(string.Empty,  @"File.multi.ext");
				tester.AssertEqual(@"C:",         @"C:\Folder");
				tester.AssertEqual(@"C:\Folder",  @"C:\Folder\");
				tester.AssertEqual(@"C:\Folder",  @"C:\Folder\File.ext");
				tester.AssertEqual(@"C:\Folder",  @"C:\Folder\File.multi.ext");
				tester.AssertEqual(@"\Folder",    @"\Folder\");
				tester.AssertEqual(@"\Folder",    @"\Folder\File.ext");
				tester.AssertEqual(@"Folder",     @"Folder\File.ext");
				tester.AssertEqual(@"Folder.txt", @"Folder.txt\File.ext");
				tester.AssertEqual(@"\Folder",    @"\Folder\File.multi.ext");
				tester.AssertEqual(@"Folder",     @"Folder\File.multi.ext");
				tester.AssertEqual(@"Folder.txt", @"Folder.txt\File.multi.ext");
			}

			// Null and empty paths
			Assert.AreEqual(string.Empty, PathOp.GetDirectoryName(null));
			Assert.AreEqual(string.Empty, PathOp.GetDirectoryName(string.Empty));
			Assert.AreEqual(string.Empty, PathOp.GetDirectoryName(Whitespace));

			// Invalid paths
			foreach (string invalidPath in InvalidPathSamples)
			{
				Assert.Throws<ArgumentException>(() => PathOp.GetDirectoryName(invalidPath));
			}
		}
		[Test] public void GetFileName()
		{
			// Retrieving file names from various paths
			{
				PathPermutationTester tester = new PathPermutationTester(s => PathOp.GetFileName(s));
				tester.AssertEqual(@"C:",             @"C:");
				tester.AssertEqual(string.Empty,      @"C:\");
				tester.AssertEqual(@"File",           @"File");
				tester.AssertEqual(@"File.ext",       @"File.ext");
				tester.AssertEqual(@"File.multi.ext", @"File.multi.ext");
				tester.AssertEqual(@"Folder",         @"C:\Folder");
				tester.AssertEqual(string.Empty,      @"C:\Folder\");
				tester.AssertEqual(@"File.ext",       @"C:\Folder\File.ext");
				tester.AssertEqual(@"File.multi.ext", @"C:\Folder\File.multi.ext");
				tester.AssertEqual(string.Empty,      @"\Folder\");
				tester.AssertEqual(@"File.ext",       @"\Folder\File.ext");
				tester.AssertEqual(@"File.ext",       @"Folder\File.ext");
				tester.AssertEqual(@"File.ext",       @"Folder.txt\File.ext");
				tester.AssertEqual(@"File.multi.ext", @"\Folder\File.multi.ext");
				tester.AssertEqual(@"File.multi.ext", @"Folder\File.multi.ext");
				tester.AssertEqual(@"File.multi.ext", @"Folder.txt\File.multi.ext");
			}

			// Null and empty paths
			Assert.AreEqual(string.Empty, PathOp.GetFileName(null));
			Assert.AreEqual(string.Empty, PathOp.GetFileName(string.Empty));
			Assert.AreEqual(string.Empty, PathOp.GetFileName(Whitespace));

			// Invalid paths
			foreach (string invalidPath in InvalidPathSamples)
			{
				Assert.Throws<ArgumentException>(() => PathOp.GetFileName(invalidPath));
			}
		}
		[Test] public void GetFileNameWithoutExtension()
		{
			// Retrieving filenames without extensions from various paths
			{
				PathPermutationTester tester = new PathPermutationTester(s => PathOp.GetFileNameWithoutExtension(s));
				tester.AssertEqual(@"C:",         @"C:");
				tester.AssertEqual(string.Empty,  @"C:\");
				tester.AssertEqual(@"File",       @"File");
				tester.AssertEqual(@"File",       @"File.ext");
				tester.AssertEqual(@"File.multi", @"File.multi.ext");
				tester.AssertEqual(@"Folder",     @"C:\Folder");
				tester.AssertEqual(string.Empty,  @"C:\Folder\");
				tester.AssertEqual(@"File",       @"C:\Folder\File.ext");
				tester.AssertEqual(@"File.multi", @"C:\Folder\File.multi.ext");
				tester.AssertEqual(string.Empty,  @"\Folder\");
				tester.AssertEqual(@"File",       @"\Folder\File.ext");
				tester.AssertEqual(@"File",       @"Folder\File.ext");
				tester.AssertEqual(@"File",       @"Folder.txt\File.ext");
				tester.AssertEqual(@"File.multi", @"\Folder\File.multi.ext");
				tester.AssertEqual(@"File.multi", @"Folder\File.multi.ext");
				tester.AssertEqual(@"File.multi", @"Folder.txt\File.multi.ext");
			}

			// Retrieving filenames without multi-extensions from various paths
			{
				PathPermutationTester tester = new PathPermutationTester(s => PathOp.GetFileNameWithoutExtension(s, true));
				tester.AssertEqual(@"C:",         @"C:");
				tester.AssertEqual(string.Empty,  @"C:\");
				tester.AssertEqual(@"File",       @"File");
				tester.AssertEqual(@"File",       @"File.ext");
				tester.AssertEqual(@"File",       @"File.multi.ext");
				tester.AssertEqual(@"Folder",     @"C:\Folder");
				tester.AssertEqual(string.Empty,  @"C:\Folder\");
				tester.AssertEqual(@"File",       @"C:\Folder\File.ext");
				tester.AssertEqual(@"File",       @"C:\Folder\File.multi.ext");
				tester.AssertEqual(string.Empty,  @"\Folder\");
				tester.AssertEqual(@"File",       @"\Folder\File.ext");
				tester.AssertEqual(@"File",       @"Folder\File.ext");
				tester.AssertEqual(@"File",       @"Folder.txt\File.ext");
				tester.AssertEqual(@"File",       @"\Folder\File.multi.ext");
				tester.AssertEqual(@"File",       @"Folder\File.multi.ext");
				tester.AssertEqual(@"File",       @"Folder.txt\File.multi.ext");
			}

			// Null and empty paths
			Assert.AreEqual(string.Empty, PathOp.GetFileNameWithoutExtension(null));
			Assert.AreEqual(string.Empty, PathOp.GetFileNameWithoutExtension(string.Empty));
			Assert.AreEqual(string.Empty, PathOp.GetFileNameWithoutExtension(Whitespace));

			// Invalid paths
			foreach (string invalidPath in InvalidPathSamples)
			{
				Assert.Throws<ArgumentException>(() => PathOp.GetFileNameWithoutExtension(invalidPath));
			}
		}
		[Test] public void GetExtension()
		{
			// Retrieving extensions from various paths
			{
				PathPermutationTester tester = new PathPermutationTester(s => PathOp.GetExtension(s));
				tester.AssertEqual(string.Empty,  @"C:");
				tester.AssertEqual(string.Empty,  @"C:\");
				tester.AssertEqual(string.Empty,  @"File");
				tester.AssertEqual(@".ext",       @"File.ext");
				tester.AssertEqual(@".ext",       @"File.multi.ext");
				tester.AssertEqual(string.Empty,  @"C:\Folder");
				tester.AssertEqual(string.Empty,  @"C:\Folder\");
				tester.AssertEqual(@".ext",       @"C:\Folder\File.ext");
				tester.AssertEqual(@".ext",       @"C:\Folder\File.multi.ext");
				tester.AssertEqual(string.Empty,  @"\Folder\");
				tester.AssertEqual(@".ext",       @"\Folder\File.ext");
				tester.AssertEqual(@".ext",       @"Folder\File.ext");
				tester.AssertEqual(@".ext",       @"Folder.txt\File.ext");
				tester.AssertEqual(@".ext",       @"\Folder\File.multi.ext");
				tester.AssertEqual(@".ext",       @"Folder\File.multi.ext");
				tester.AssertEqual(@".ext",       @"Folder.txt\File.multi.ext");
			}

			// Retrieving multi-extensions from various paths
			{
				PathPermutationTester tester = new PathPermutationTester(s => PathOp.GetExtension(s, true));
				tester.AssertEqual(string.Empty,  @"C:");
				tester.AssertEqual(string.Empty,  @"C:\");
				tester.AssertEqual(string.Empty,  @"File");
				tester.AssertEqual(@".ext",       @"File.ext");
				tester.AssertEqual(@".multi.ext", @"File.multi.ext");
				tester.AssertEqual(string.Empty,  @"C:\Folder");
				tester.AssertEqual(string.Empty,  @"C:\Folder\");
				tester.AssertEqual(@".ext",       @"C:\Folder\File.ext");
				tester.AssertEqual(@".multi.ext", @"C:\Folder\File.multi.ext");
				tester.AssertEqual(string.Empty,  @"\Folder\");
				tester.AssertEqual(@".ext",       @"\Folder\File.ext");
				tester.AssertEqual(@".ext",       @"Folder\File.ext");
				tester.AssertEqual(@".ext",       @"Folder.txt\File.ext");
				tester.AssertEqual(@".multi.ext", @"\Folder\File.multi.ext");
				tester.AssertEqual(@".multi.ext", @"Folder\File.multi.ext");
				tester.AssertEqual(@".multi.ext", @"Folder.txt\File.multi.ext");
			}

			// Null and empty paths
			Assert.AreEqual(string.Empty, PathOp.GetExtension(null));
			Assert.AreEqual(string.Empty, PathOp.GetExtension(string.Empty));
			Assert.AreEqual(string.Empty, PathOp.GetExtension(Whitespace));

			// Invalid paths
			foreach (string invalidPath in InvalidPathSamples)
			{
				Assert.Throws<ArgumentException>(() => PathOp.GetExtension(invalidPath));
			}
		}

		private class PathPermutationTester
		{
			private Func<string,string> method;

			public PathPermutationTester(Func<string,string> method)
			{
				this.method = method;
			}
			public void AssertEqual(string expected, string input)
			{
				string inputAlt1 = input != null ? input.Replace(PathOp.DirectorySeparatorChar, PathOp.AltDirectorySeparatorChar) : null;
				string inputAlt2 = input != null ? input.Replace(PathOp.AltDirectorySeparatorChar, PathOp.DirectorySeparatorChar) : null;
				string expectedAlt1 = expected != null ? expected.Replace(PathOp.DirectorySeparatorChar, PathOp.AltDirectorySeparatorChar) : null;
				string expectedAlt2 = expected != null ? expected.Replace(PathOp.AltDirectorySeparatorChar, PathOp.DirectorySeparatorChar) : null;

				Assert.AreEqual(expected, this.method(input));
				Assert.AreEqual(expectedAlt1, this.method(inputAlt1));
				Assert.AreEqual(expectedAlt2, this.method(inputAlt2));
			}
		}

		private static void AssertCombineOverloads(string expected, string first, string second)
		{
			Assert.AreEqual(expected, PathOp.Combine(first, second));
			Assert.AreEqual(expected, PathOp.Combine(new string[] { first, second }));
		}
		private static void AssertCombineOverloadsThrow<T>(string first, string second) where T : Exception
		{
			Assert.Throws<T>(() => PathOp.Combine(first, second));
			Assert.Throws<T>(() => PathOp.Combine(new string[] { first, second }));
		}
	}
}
