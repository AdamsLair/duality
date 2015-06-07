using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

using Duality;

using NUnit.Framework;

namespace Duality.Tests.Utility
{
	[TestFixture]
	public class ExtMethodsTypeInfo
	{
		[Test] public void BaseTypeInfo()
		{
			Assert.IsNull(typeof(object).GetTypeInfo().GetBaseTypeInfo());
			Assert.AreEqual(
				typeof(object).GetTypeInfo(), 
				typeof(TestInheritanceDepthBase).GetTypeInfo().GetBaseTypeInfo());
			Assert.AreEqual(
				typeof(TestInheritanceDepthBase).GetTypeInfo(), 
				typeof(TestInheritanceDepth).GetTypeInfo().GetBaseTypeInfo());
		}
		[Test] public void InheritanceDepth()
		{
			Assert.AreEqual(0, typeof(object).GetTypeInfo().GetInheritanceDepth());
			Assert.AreEqual(1, typeof(string).GetTypeInfo().GetInheritanceDepth());
			Assert.AreEqual(1, typeof(TestInheritanceDepthBase).GetTypeInfo().GetInheritanceDepth());
			Assert.AreEqual(2, typeof(TestInheritanceDepth).GetTypeInfo().GetInheritanceDepth());
		}
		[Test] public void DeclaredFieldsDeep()
		{
			FieldInfo[] deepFields = typeof(TestDeepFields).GetTypeInfo().DeclaredFieldsDeep().ToArray();

			Assert.IsFalse(HasMember(deepFields, "DoesntExist"));
			Assert.IsTrue(HasMember(deepFields, "PrivateInstanceField"));
			Assert.IsTrue(HasMember(deepFields, "PrivateStaticField"));
			Assert.IsTrue(HasMember(deepFields, "PublicInstanceField"));
			Assert.IsTrue(HasMember(deepFields, "PublicStaticField"));
			Assert.IsTrue(HasMember(deepFields, "PrivateInstanceBaseField"));
			Assert.IsTrue(HasMember(deepFields, "PrivateStaticBaseField"));
			Assert.IsTrue(HasMember(deepFields, "PublicInstanceBaseField"));
			Assert.IsTrue(HasMember(deepFields, "PublicStaticBaseField"));
		}

		private class TestInheritanceDepth : TestInheritanceDepthBase { }
		private class TestInheritanceDepthBase {}

		private class TestDeepFields : TestDeepFieldsBase
		{
			private int PrivateInstanceField;
			public int PublicInstanceField;
			private static int PrivateStaticField;
			public static int PublicStaticField;
		}
		private class TestDeepFieldsBase
		{
			private int PrivateInstanceBaseField;
			public int PublicInstanceBaseField;
			private static int PrivateStaticBaseField;
			public static int PublicStaticBaseField;
		}

		private static bool HasMember(IEnumerable<MemberInfo> members, string name)
		{
			return members.Any(item => item.Name == name);
		}
	}
}
