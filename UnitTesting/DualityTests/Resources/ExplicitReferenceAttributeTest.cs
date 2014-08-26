using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

using Duality;
using Duality.Resources;

using NUnit.Framework;


namespace Duality.Tests.Resources
{
	[TestFixture]
	public class ExplicitReferenceAttributeTest
	{
		[Test] public void RegularReferences()
		{
			Assert.IsTrue(ReflectionHelper.CanReferenceResource(typeof(RegularReferenceResource), typeof(VoidReferenceResource)));
			Assert.IsTrue(ReflectionHelper.CanReferenceResource(typeof(RegularReferenceResource), typeof(DerivedVoidReferenceResource)));
			Assert.IsFalse(ReflectionHelper.CanReferenceResource(typeof(RegularReferenceResource), typeof(RegularReferenceResource)));
			Assert.IsFalse(ReflectionHelper.CanReferenceResource(typeof(RegularReferenceResource), typeof(Resource)));
		}
		[Test] public void CircularReferences()
		{
			Assert.IsTrue(ReflectionHelper.CanReferenceResource(typeof(CircularReferenceResourceA), typeof(CircularReferenceResourceB)));
			Assert.IsTrue(ReflectionHelper.CanReferenceResource(typeof(CircularReferenceResourceA), typeof(CircularReferenceResourceA)));
			Assert.IsTrue(ReflectionHelper.CanReferenceResource(typeof(CircularReferenceResourceB), typeof(CircularReferenceResourceA)));
			Assert.IsTrue(ReflectionHelper.CanReferenceResource(typeof(CircularReferenceResourceB), typeof(CircularReferenceResourceB)));
			Assert.IsFalse(ReflectionHelper.CanReferenceResource(typeof(CircularReferenceResourceA), typeof(NoExplicitReferenceResource)));
			Assert.IsFalse(ReflectionHelper.CanReferenceResource(typeof(CircularReferenceResourceA), typeof(Resource)));
			Assert.IsFalse(ReflectionHelper.CanReferenceResource(typeof(CircularReferenceResourceB), typeof(NoExplicitReferenceResource)));
			Assert.IsFalse(ReflectionHelper.CanReferenceResource(typeof(CircularReferenceResourceB), typeof(Resource)));
		}
		[Test] public void NoExplicitReferences()
		{
			Assert.IsTrue(ReflectionHelper.CanReferenceResource(typeof(NoExplicitReferenceResource), typeof(NoExplicitReferenceResource)));
			Assert.IsTrue(ReflectionHelper.CanReferenceResource(typeof(NoExplicitReferenceResource), typeof(VoidReferenceResource)));
			Assert.IsTrue(ReflectionHelper.CanReferenceResource(typeof(NoExplicitReferenceResource), typeof(DerivedVoidReferenceResource)));
			Assert.IsTrue(ReflectionHelper.CanReferenceResource(typeof(NoExplicitReferenceResource), typeof(Resource)));
		}
		[Test] public void VoidReferences()
		{
			Assert.IsFalse(ReflectionHelper.CanReferenceResource(typeof(VoidReferenceResource), typeof(Resource)));
			Assert.IsFalse(ReflectionHelper.CanReferenceResource(typeof(VoidReferenceResource), typeof(RegularReferenceResource)));
			Assert.IsFalse(ReflectionHelper.CanReferenceResource(typeof(VoidReferenceResource), typeof(VoidReferenceResource)));
			Assert.IsFalse(ReflectionHelper.CanReferenceResource(typeof(VoidReferenceResource), typeof(NoExplicitReferenceResource)));
		}

		private class NoExplicitReferenceResource : Resource {}
		[ExplicitResourceReference()]
		private class VoidReferenceResource : Resource {}
		private class DerivedVoidReferenceResource : VoidReferenceResource {}
		[ExplicitResourceReference(typeof(VoidReferenceResource))]
		private class RegularReferenceResource : Resource {}
		[ExplicitResourceReference(typeof(CircularReferenceResourceB))]
		private class CircularReferenceResourceA : Resource {}
		[ExplicitResourceReference(typeof(CircularReferenceResourceA))]
		private class CircularReferenceResourceB : Resource {}
	}
}
