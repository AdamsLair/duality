﻿using System;
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
			Assert.AreEqual(TestErrorHandler.TestResolveType, ReflectionHelper.ResolveType(TestErrorHandler.TestResolveTypeId));
			Assert.AreEqual(TestErrorHandler.TestResolveMember, ReflectionHelper.ResolveMember(TestErrorHandler.TestResolveMemberId));
		}
		[Test] public void VisitorContentRef()
		{
			TestVisitorClass visitedRoot = new TestVisitorClass
			{
				StringField = "Hello World",
				ByteField = 42,
				ShortListField = new List<short> { 0, 1, 2 },
				ContentRefField = new ContentRef<Resource>(null, "ResourceReference"),
				ObjectField = "Hidden"
			};
			int visitedCount;

			// None. We do not have a bool here.
			visitedCount = 0;
			ReflectionHelper.VisitObjectsDeep<bool>(visitedRoot, s => { visitedCount++; return s; });
			Assert.AreEqual(0, visitedCount);

			// Only the byte field
			visitedCount = 0;
			ReflectionHelper.VisitObjectsDeep<byte>(visitedRoot, s => { visitedCount++; return s; });
			Assert.AreEqual(1, visitedCount);

			// At least the three list elements. Probably more due to internal array allocations
			visitedCount = 0;
			ReflectionHelper.VisitObjectsDeep<short>(visitedRoot, s => { visitedCount++; return s; });
			Assert.GreaterOrEqual(visitedCount, 3);

			// Expecting StringField, (hidden) ObjectField and ContentRefField's internal path.
			visitedCount = 0;
			ReflectionHelper.VisitObjectsDeep<string>(visitedRoot, s => { visitedCount++; return s; });
			Assert.AreEqual(3, visitedCount);
		}
		[Test] public void IsPlainOldData()
		{
			// Primitives
			Assert.IsTrue(typeof(bool).GetTypeInfo().IsPlainOldData());
			Assert.IsTrue(typeof(byte).GetTypeInfo().IsPlainOldData());
			Assert.IsTrue(typeof(sbyte).GetTypeInfo().IsPlainOldData());
			Assert.IsTrue(typeof(short).GetTypeInfo().IsPlainOldData());
			Assert.IsTrue(typeof(ushort).GetTypeInfo().IsPlainOldData());
			Assert.IsTrue(typeof(int).GetTypeInfo().IsPlainOldData());
			Assert.IsTrue(typeof(uint).GetTypeInfo().IsPlainOldData());
			Assert.IsTrue(typeof(long).GetTypeInfo().IsPlainOldData());
			Assert.IsTrue(typeof(ulong).GetTypeInfo().IsPlainOldData());
			Assert.IsTrue(typeof(float).GetTypeInfo().IsPlainOldData());
			Assert.IsTrue(typeof(double).GetTypeInfo().IsPlainOldData());

			// Primitive special cases
			Assert.IsTrue(typeof(decimal).GetTypeInfo().IsPlainOldData());
			Assert.IsTrue(typeof(string).GetTypeInfo().IsPlainOldData());
			Assert.IsTrue(typeof(Alignment).GetTypeInfo().IsPlainOldData());

			// Pure POD compound types
			Assert.IsTrue(typeof(Vector2).GetTypeInfo().IsPlainOldData());

			// Non-pure compound types
			Assert.IsFalse(typeof(ContentRef<Resource>).GetTypeInfo().IsPlainOldData());

			// Various kinds of by-reference types
			Assert.IsFalse(typeof(object).GetTypeInfo().IsPlainOldData());
			Assert.IsFalse(typeof(int[]).GetTypeInfo().IsPlainOldData());
			Assert.IsFalse(typeof(GameObject).GetTypeInfo().IsPlainOldData());
			Assert.IsFalse(typeof(IComparable).GetTypeInfo().IsPlainOldData());
			Assert.IsFalse(typeof(IContentRef).GetTypeInfo().IsPlainOldData());
			Assert.IsFalse(typeof(IComparable<int>).GetTypeInfo().IsPlainOldData());
		}

		private class TestVisitorClass
		{
			public string StringField;
			public byte ByteField;
			public List<short> ShortListField;
			public ContentRef<Resource> ContentRefField;
			public object ObjectField;
		}

		internal class TestMemberClass
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
