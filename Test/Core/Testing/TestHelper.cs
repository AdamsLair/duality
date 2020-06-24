using System.IO;

namespace Duality.Tests
{
	public static class TestHelper
	{
		public static string EmbeddedResourcesDir
		{
			get { return Path.Combine("..", "..", "Test", "Core", "EmbeddedResources"); }
		}

		public static string GetEmbeddedResourcePath(string resName, string resEnding)
		{
			return Path.Combine(TestHelper.EmbeddedResourcesDir, resName + resEnding);
		}
	}
}
