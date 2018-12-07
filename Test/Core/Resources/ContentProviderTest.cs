using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

using Duality;
using Duality.IO;
using Duality.Resources;

using NUnit.Framework;


namespace Duality.Tests.Resources
{
	[TestFixture]
	public class ContentProviderTest
	{
		[Test] public void AddRemoveContent()
		{
			Pixmap resource = new Pixmap();
			string alias = "Foo";


			// Register the new resource with the provider
			ContentProvider.AddContent(alias, resource);

			// Expect it to be registered and show up in all relevant API calls
			Assert.IsTrue(ContentProvider.HasContent(alias));
			Assert.AreSame(resource, ContentProvider.RequestContent<Pixmap>(alias).Res);
			Assert.AreSame(resource, ContentProvider.RequestContent(alias).Res);
			CollectionAssert.Contains(ContentProvider.GetLoadedContent<Pixmap>(), (ContentRef<Pixmap>)resource);

			// Expect it to not show up in specialized API calls that have nothing to do with the registered resource
			CollectionAssert.DoesNotContain(ContentProvider.GetDefaultContent<Pixmap>(), (ContentRef<Pixmap>)resource);
			CollectionAssert.DoesNotContain(ContentProvider.GetAvailableContent<Pixmap>(), (ContentRef<Pixmap>)resource);

			// Expect the resource itself to reference back to its new primary path
			Assert.AreEqual(alias, resource.Path);
			Assert.IsFalse(resource.IsRuntimeResource);


			// Un-register the new resource, but don't dispose it
			ContentProvider.RemoveContent(alias, false);

			// Expect the resource to still be valid
			Assert.IsFalse(resource.Disposed);

			// Expect it to no longer show up in any of the relevant API calls
			Assert.IsFalse(ContentProvider.HasContent(alias));
			Assert.IsNull(ContentProvider.RequestContent<Pixmap>(alias).Res);
			Assert.IsNull(ContentProvider.RequestContent(alias).Res);
			CollectionAssert.DoesNotContain(ContentProvider.GetLoadedContent<Pixmap>(), (ContentRef<Pixmap>)resource);

			// Expect the resource itself to no longer back-reference to its former path
			Assert.IsNull(resource.Path);
			Assert.IsTrue(resource.IsRuntimeResource);


			// Re-register the resource, and immediately unregister-dispose it afterwards
			ContentProvider.AddContent(alias, resource);
			ContentProvider.RemoveContent(alias, true);

			// Expect the resource to be disposed and no longer show up in API calls
			Assert.IsTrue(resource.Disposed);
			Assert.IsFalse(ContentProvider.HasContent(alias));
			Assert.IsNull(ContentProvider.RequestContent<Pixmap>(alias).Res);
			Assert.IsNull(ContentProvider.RequestContent(alias).Res);
			CollectionAssert.DoesNotContain(ContentProvider.GetLoadedContent<Pixmap>(), (ContentRef<Pixmap>)resource);

			// Expect the resource path to remain unchanged, in order to allow reload-recovery
			Assert.AreEqual(alias, resource.Path);
			Assert.IsFalse(resource.IsRuntimeResource);
		}
		[Test] public void DisposeRemovesContent()
		{
			Pixmap resource = new Pixmap();
			string alias = "Foo";

			// Register the new resource with the provider
			ContentProvider.AddContent(alias, resource);

			// Dispose the resource
			resource.Dispose();

			// Expect the resource to be disposed and no longer show up in API calls
			Assert.IsTrue(resource.Disposed);
			Assert.IsFalse(ContentProvider.HasContent(alias));
			Assert.IsNull(ContentProvider.RequestContent<Pixmap>(alias).Res);
			Assert.IsNull(ContentProvider.RequestContent(alias).Res);
			CollectionAssert.DoesNotContain(ContentProvider.GetLoadedContent<Pixmap>(), (ContentRef<Pixmap>)resource);

			// Expect the resource path to remain unchanged, in order to allow reload-recovery
			Assert.AreEqual(alias, resource.Path);
		}
		[Test] public void AddOverwritesContent()
		{
			Pixmap resourceA = new Pixmap();
			Pixmap resourceB = new Pixmap();
			string alias = "Foo";

			// Register both first and second resource on the same alias
			ContentProvider.AddContent(alias, resourceA);
			ContentProvider.AddContent(alias, resourceB);

			// Expect the first resource to be disposed, and the second to be properly registered
			Assert.IsTrue(resourceA.Disposed);
			Assert.AreEqual(alias, resourceA.Path);
			Assert.IsTrue(ContentProvider.HasContent(alias));
			Assert.AreSame(resourceB, ContentProvider.RequestContent<Pixmap>(alias).Res);
			Assert.AreSame(resourceB, ContentProvider.RequestContent(alias).Res);
			CollectionAssert.Contains(ContentProvider.GetLoadedContent<Pixmap>(), (ContentRef<Pixmap>)resourceB);
		}
		[Test] public void RenameContent()
		{
			Pixmap resource = new Pixmap();
			string aliasA = "Foo";
			string aliasB = "Bar";

			// Register the resource with one alias, then rename it to use another
			ContentProvider.AddContent(aliasA, resource);
			ContentProvider.RenameContent(aliasA, aliasB);

			// Expect the resource to use the second alias only, and the first to be unused
			Assert.AreEqual(aliasB, resource.Path);
			Assert.IsTrue(ContentProvider.HasContent(aliasB));
			Assert.AreSame(resource, ContentProvider.RequestContent<Pixmap>(aliasB).Res);
			Assert.AreSame(resource, ContentProvider.RequestContent(aliasB).Res);
			Assert.IsFalse(ContentProvider.HasContent(aliasA));
			Assert.IsNull(ContentProvider.RequestContent<Pixmap>(aliasA).Res);
			Assert.IsNull(ContentProvider.RequestContent(aliasA).Res);
			CollectionAssert.Contains(ContentProvider.GetLoadedContent<Pixmap>(), (ContentRef<Pixmap>)resource);
		}
		[Test] public void RenameContentTree()
		{
			Pixmap resource1 = new Pixmap();
			Pixmap resource2 = new Pixmap();
			string dirA = "Foo";
			string dirB = "Bar";

			// Let's choose some tricky path names that would break when doing a path-unaware string replace
			string alias1A = PathOp.Combine(dirA, "Foo", "Resource1");
			string alias2A = PathOp.Combine(dirA, "Bar", "Resource2");
			string alias1B = PathOp.Combine(dirB, "Foo", "Resource1");
			string alias2B = PathOp.Combine(dirB, "Bar", "Resource2");

			// Register the resources with their first alias, then rename their directory
			ContentProvider.AddContent(alias1A, resource1);
			ContentProvider.AddContent(alias2A, resource2);
			ContentProvider.RenameContentTree(dirA, dirB);

			// Expect the resources to use their second alias only, and the first to be unused
			Assert.AreEqual(alias1B, resource1.Path);
			Assert.AreEqual(alias2B, resource2.Path);
			Assert.IsTrue(ContentProvider.HasContent(alias1B));
			Assert.IsTrue(ContentProvider.HasContent(alias2B));
			Assert.AreSame(resource1, ContentProvider.RequestContent<Pixmap>(alias1B).Res);
			Assert.AreSame(resource2, ContentProvider.RequestContent<Pixmap>(alias2B).Res);
			Assert.AreSame(resource1, ContentProvider.RequestContent(alias1B).Res);
			Assert.AreSame(resource2, ContentProvider.RequestContent(alias2B).Res);
			Assert.IsFalse(ContentProvider.HasContent(alias1A));
			Assert.IsFalse(ContentProvider.HasContent(alias2A));
			Assert.IsNull(ContentProvider.RequestContent<Pixmap>(alias1A).Res);
			Assert.IsNull(ContentProvider.RequestContent<Pixmap>(alias2A).Res);
			Assert.IsNull(ContentProvider.RequestContent(alias1A).Res);
			Assert.IsNull(ContentProvider.RequestContent(alias2A).Res);
			CollectionAssert.Contains(ContentProvider.GetLoadedContent<Pixmap>(), (ContentRef<Pixmap>)resource1);
			CollectionAssert.Contains(ContentProvider.GetLoadedContent<Pixmap>(), (ContentRef<Pixmap>)resource2);
		}
		[Test] public void RemoveContentTree()
		{
			Pixmap resource1 = new Pixmap();
			Pixmap resource2 = new Pixmap();
			Pixmap resource3 = new Pixmap();
			string dirA = "Foo";
			string dirB = "Bar";
			string alias1 = dirA + "/" + "Resource1";
			string alias2 = dirA + "/" + "Resource2";
			string alias3 = dirB + "/" + "Resource3";

			// Register all resources, then remove one of their directories
			ContentProvider.AddContent(alias1, resource1);
			ContentProvider.AddContent(alias2, resource2);
			ContentProvider.AddContent(alias3, resource3);
			ContentProvider.RemoveContentTree(dirA, true);

			// Expect the resources from the removed directory to be disposed, but not any other resource
			Assert.AreEqual(alias1, resource1.Path);
			Assert.AreEqual(alias2, resource2.Path);
			Assert.AreEqual(alias3, resource3.Path);
			Assert.IsFalse(ContentProvider.HasContent(alias1));
			Assert.IsFalse(ContentProvider.HasContent(alias2));
			Assert.IsTrue(ContentProvider.HasContent(alias3));
			Assert.IsNull(ContentProvider.RequestContent<Pixmap>(alias1).Res);
			Assert.IsNull(ContentProvider.RequestContent<Pixmap>(alias2).Res);
			Assert.AreSame(resource3, ContentProvider.RequestContent<Pixmap>(alias3).Res);
			Assert.IsNull(ContentProvider.RequestContent(alias1).Res);
			Assert.IsNull(ContentProvider.RequestContent(alias2).Res);
			Assert.AreSame(resource3, ContentProvider.RequestContent(alias3).Res);
			Assert.IsFalse(ContentProvider.HasContent(alias1));
			Assert.IsFalse(ContentProvider.HasContent(alias2));
			Assert.IsTrue(ContentProvider.HasContent(alias3));
			CollectionAssert.DoesNotContain(ContentProvider.GetLoadedContent<Pixmap>(), (ContentRef<Pixmap>)resource1);
			CollectionAssert.DoesNotContain(ContentProvider.GetLoadedContent<Pixmap>(), (ContentRef<Pixmap>)resource2);
			CollectionAssert.Contains(ContentProvider.GetLoadedContent<Pixmap>(), (ContentRef<Pixmap>)resource3);
		}
		[Test] public void RequestContent()
		{
			// Create a test resource and save it somewhere to be retrieved later
			Pixmap resource = new Pixmap();
			string path = PathOp.Combine(DualityApp.DataDirectory, "Test" + Resource.GetFileExtByType<Pixmap>());
			resource.Save(path, false);
			resource.Dispose();
			resource = null;

			// Request the resource from its path and expect it to be valid
			Assert.IsNotNull(ContentProvider.RequestContent<Pixmap>(path).Res);
			Assert.IsNotNull(ContentProvider.RequestContent(path).Res);
			Assert.IsTrue(ContentProvider.HasContent(path));
			CollectionAssert.Contains(ContentProvider.GetLoadedContent<Pixmap>(), new ContentRef<Pixmap>(null, path));
			CollectionAssert.Contains(ContentProvider.GetAvailableContent<Pixmap>(), new ContentRef<Pixmap>(null, path));

			// Request the resource multiple times and expect the same instance to be returned
			ContentRef<Pixmap> requestA = ContentProvider.RequestContent<Pixmap>(path);
			ContentRef<Pixmap> requestB = ContentProvider.RequestContent<Pixmap>(path);
			Assert.AreEqual(requestA, requestB);
			Assert.AreSame(requestA.Res, requestB.Res);

			// Dispose the resource and expect an automatic reload on access
			Pixmap oldResource = requestA.Res;
			oldResource.Dispose();
			Assert.IsNotNull(requestA.Res);
			Assert.IsNotNull(requestB.Res);
			Assert.AreEqual(requestA, requestB);
			Assert.AreSame(requestA.Res, requestB.Res);
		}
	}
}
