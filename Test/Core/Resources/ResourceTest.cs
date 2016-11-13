using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.IO;

using Duality;
using Duality.Drawing;
using Duality.Resources;
using Duality.Components;
using Duality.Components.Renderers;

using NUnit.Framework;


namespace Duality.Tests.Resources
{
	[TestFixture]
	public class ResourceTest
	{
		[Test] public void GetTypeByFileName()
		{
			// Regular resource paths
			Assert.AreSame(typeof(AudioData), Resource.GetTypeByFileName(@"Data\Test.AudioData.res"));
			Assert.AreSame(typeof(Texture), Resource.GetTypeByFileName(@"Data\Test.Texture.res"));
			Assert.AreSame(typeof(Pixmap), Resource.GetTypeByFileName(@"Data\Test.Pixmap.res"));

			// Special case for embedded content
			Assert.IsNull(Resource.GetTypeByFileName(@"Default:Textures:Test"));

			// Various expected failure cases that should be handled gracefully
			Assert.IsNull(Resource.GetTypeByFileName(null));
			Assert.IsNull(Resource.GetTypeByFileName(""));
			Assert.IsNull(Resource.GetTypeByFileName(@"Data\Test.res"));
			Assert.IsNull(Resource.GetTypeByFileName(@"Data\Test.Foo.res"));
		}
		[Test] public void GetFileExtByType()
		{
			// Test some resource types
			Assert.AreEqual(".AudioData.res", Resource.GetFileExtByType<AudioData>());
			Assert.AreEqual(".Texture.res", Resource.GetFileExtByType<Texture>());
			Assert.AreEqual(".Pixmap.res", Resource.GetFileExtByType<Pixmap>());

			// Make sure the results of generic and non-generic method are the same
			Assert.AreEqual(Resource.GetFileExtByType(typeof(AudioData)), Resource.GetFileExtByType<AudioData>());
			Assert.AreEqual(Resource.GetFileExtByType(typeof(Texture)), Resource.GetFileExtByType<Texture>());
			Assert.AreEqual(Resource.GetFileExtByType(typeof(Pixmap)), Resource.GetFileExtByType<Pixmap>());

			// Check special cases for null and base type
			Assert.AreEqual(".res", Resource.GetFileExtByType<Resource>());
			Assert.AreEqual(".res", Resource.GetFileExtByType(typeof(Resource)));
			Assert.AreEqual(".res", Resource.GetFileExtByType(null));
		}
	}
}
