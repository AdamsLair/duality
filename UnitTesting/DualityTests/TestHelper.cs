using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.IO;
using System.Text;

namespace Duality.Tests
{
	public static class TestHelper
	{
		public static string EmbeddedResourcesDir
		{
			get { return Path.Combine("..", "EmbeddedResources"); }
		}

		public static string GetEmbeddedResourcePath(string resName, string resEnding)
		{
			return Path.Combine(TestHelper.EmbeddedResourcesDir, resName + resEnding);
		}
	}
}
