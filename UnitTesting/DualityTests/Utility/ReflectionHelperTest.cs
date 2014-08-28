using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;

using Duality;
using Duality.Serialization;

using NUnit.Framework;

namespace Duality.Tests.Utility
{
	[TestFixture]
	public class ReflectionHelperTest
	{
		[Test] public void RegularTypeResolve()
		{
			Assert.IsTrue(CheckResolveType<int>());
			Assert.IsTrue(CheckResolveType<string>());
			Assert.IsTrue(CheckResolveType<object>());
			Assert.IsTrue(CheckResolveType<MemberInfo>());
			Assert.IsTrue(CheckResolveType<TestMemberClass>());
			Assert.IsTrue(CheckResolveType<TestMemberClass.SomeClass>());

			Assert.IsTrue(CheckResolveMember<TestMemberClass>(MemberTypes.Field));
			Assert.IsTrue(CheckResolveMember<TestMemberClass>(MemberTypes.Property));
			Assert.IsTrue(CheckResolveMember<TestMemberClass>(MemberTypes.Event));
			Assert.IsTrue(CheckResolveMember<TestMemberClass>(MemberTypes.NestedType));
			Assert.IsTrue(CheckResolveMember<TestMemberClass>(MemberTypes.Constructor));
		}
		[Test] public void CustomTypeResolve()
		{
			Assert.AreEqual(ReflectionHelper.ResolveType(TestErrorHandler.TestResolveTypeId), TestErrorHandler.TestResolveType);
			Assert.AreEqual(ReflectionHelper.ResolveMember(TestErrorHandler.TestResolveMemberId), TestErrorHandler.TestResolveMember);
		}

		public class TestMemberClass
		{
			public string SomeField;
			public event EventHandler SomeEvent;
			public string SomeProperty
			{
				get { return this.SomeField; }
			}
			public int this[int index]
			{
				get { return index; }
			}
			public void SomeMethod() {}
			public TestMemberClass()
			{

			}
			public class SomeClass {}
		}

		private bool CheckResolveType<T>()
		{
			Type type = typeof(T);
			return ReflectionHelper.ResolveType(type.GetTypeId()) == type;
		}
		private bool CheckResolveMember<T>(MemberTypes memberType)
		{
			Type type = typeof(T);
			MemberInfo member = type.GetMembers().Where(m => m.MemberType == memberType).FirstOrDefault();
			return ReflectionHelper.ResolveMember(member.GetMemberId()) == member;
		}
	}

	public class TestErrorHandler : SerializeErrorHandler
	{
		public static readonly Type TestResolveType = typeof(ReflectionHelperTest.TestMemberClass);
		public static readonly MemberInfo TestResolveMember = typeof(ReflectionHelperTest.TestMemberClass).GetFields().First();
		public static readonly string TestResolveTypeId = "MyReallyCoolType";
		public static readonly string TestResolveMemberId = "MyReallyCoolMember";

		public override void HandleError(SerializeError error)
		{
			ResolveTypeError resolveTypeError = error as ResolveTypeError;
			if (resolveTypeError != null)
			{
				if (resolveTypeError.TypeId == TestResolveTypeId)
					resolveTypeError.ResolvedType = TestResolveType;
			}
			ResolveMemberError resolveMemberError = error as ResolveMemberError;
			if (resolveMemberError != null)
			{
				if (resolveMemberError.MemberId == TestResolveMemberId)
					resolveMemberError.ResolvedMember = TestResolveMember;
			}

			return;
		}
	}
}
