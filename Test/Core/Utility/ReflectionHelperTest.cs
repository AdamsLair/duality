using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;

using Duality;
using Duality.Drawing;
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
		[Test] public void IsReferenceOrContainsReferences()
		{
			// Primitives
			Assert.IsFalse(typeof(bool).GetTypeInfo().IsReferenceOrContainsReferences());
			Assert.IsFalse(typeof(byte).GetTypeInfo().IsReferenceOrContainsReferences());
			Assert.IsFalse(typeof(sbyte).GetTypeInfo().IsReferenceOrContainsReferences());
			Assert.IsFalse(typeof(short).GetTypeInfo().IsReferenceOrContainsReferences());
			Assert.IsFalse(typeof(ushort).GetTypeInfo().IsReferenceOrContainsReferences());
			Assert.IsFalse(typeof(int).GetTypeInfo().IsReferenceOrContainsReferences());
			Assert.IsFalse(typeof(uint).GetTypeInfo().IsReferenceOrContainsReferences());
			Assert.IsFalse(typeof(long).GetTypeInfo().IsReferenceOrContainsReferences());
			Assert.IsFalse(typeof(ulong).GetTypeInfo().IsReferenceOrContainsReferences());
			Assert.IsFalse(typeof(float).GetTypeInfo().IsReferenceOrContainsReferences());
			Assert.IsFalse(typeof(double).GetTypeInfo().IsReferenceOrContainsReferences());
			Assert.IsFalse(ReflectionHelper.IsReferenceOrContainsReferences<bool>());
			Assert.IsFalse(ReflectionHelper.IsReferenceOrContainsReferences<byte>());
			Assert.IsFalse(ReflectionHelper.IsReferenceOrContainsReferences<sbyte>());
			Assert.IsFalse(ReflectionHelper.IsReferenceOrContainsReferences<short>());
			Assert.IsFalse(ReflectionHelper.IsReferenceOrContainsReferences<ushort>());
			Assert.IsFalse(ReflectionHelper.IsReferenceOrContainsReferences<int>());
			Assert.IsFalse(ReflectionHelper.IsReferenceOrContainsReferences<uint>());
			Assert.IsFalse(ReflectionHelper.IsReferenceOrContainsReferences<long>());
			Assert.IsFalse(ReflectionHelper.IsReferenceOrContainsReferences<ulong>());
			Assert.IsFalse(ReflectionHelper.IsReferenceOrContainsReferences<float>());
			Assert.IsFalse(ReflectionHelper.IsReferenceOrContainsReferences<double>());

			// Primitive special cases
			Assert.IsFalse(typeof(decimal).GetTypeInfo().IsReferenceOrContainsReferences());
			Assert.IsFalse(typeof(Alignment).GetTypeInfo().IsReferenceOrContainsReferences());
			Assert.IsFalse(ReflectionHelper.IsReferenceOrContainsReferences<decimal>());
			Assert.IsFalse(ReflectionHelper.IsReferenceOrContainsReferences<Alignment>());

			// Pure POD compound types
			Assert.IsFalse(typeof(Vector2).GetTypeInfo().IsReferenceOrContainsReferences());
			Assert.IsFalse(typeof(ColorRgba).GetTypeInfo().IsReferenceOrContainsReferences());
			Assert.IsFalse(ReflectionHelper.IsReferenceOrContainsReferences<Vector2>());
			Assert.IsFalse(ReflectionHelper.IsReferenceOrContainsReferences<ColorRgba>());

			// Non-pure compound types
			Assert.IsTrue(typeof(ContentRef<Resource>).GetTypeInfo().IsReferenceOrContainsReferences());
			Assert.IsTrue(ReflectionHelper.IsReferenceOrContainsReferences<ContentRef<Resource>>());

			// Various kinds of by-reference types
			Assert.IsTrue(typeof(string).GetTypeInfo().IsReferenceOrContainsReferences());
			Assert.IsTrue(typeof(object).GetTypeInfo().IsReferenceOrContainsReferences());
			Assert.IsTrue(typeof(int[]).GetTypeInfo().IsReferenceOrContainsReferences());
			Assert.IsTrue(typeof(GameObject).GetTypeInfo().IsReferenceOrContainsReferences());
			Assert.IsTrue(typeof(IComparable).GetTypeInfo().IsReferenceOrContainsReferences());
			Assert.IsTrue(typeof(IContentRef).GetTypeInfo().IsReferenceOrContainsReferences());
			Assert.IsTrue(typeof(IComparable<int>).GetTypeInfo().IsReferenceOrContainsReferences());
			Assert.IsTrue(ReflectionHelper.IsReferenceOrContainsReferences<string>());
			Assert.IsTrue(ReflectionHelper.IsReferenceOrContainsReferences<object>());
			Assert.IsTrue(ReflectionHelper.IsReferenceOrContainsReferences<int[]>());
			Assert.IsTrue(ReflectionHelper.IsReferenceOrContainsReferences<GameObject>());
			Assert.IsTrue(ReflectionHelper.IsReferenceOrContainsReferences<IComparable>());
			Assert.IsTrue(ReflectionHelper.IsReferenceOrContainsReferences<IContentRef>());
			Assert.IsTrue(ReflectionHelper.IsReferenceOrContainsReferences<IComparable<int>>());
		}
		[Test] public void IsDeepCopyByAssignment()
		{
			// Primitives
			Assert.IsTrue(typeof(bool).GetTypeInfo().IsDeepCopyByAssignment());
			Assert.IsTrue(typeof(byte).GetTypeInfo().IsDeepCopyByAssignment());
			Assert.IsTrue(typeof(sbyte).GetTypeInfo().IsDeepCopyByAssignment());
			Assert.IsTrue(typeof(short).GetTypeInfo().IsDeepCopyByAssignment());
			Assert.IsTrue(typeof(ushort).GetTypeInfo().IsDeepCopyByAssignment());
			Assert.IsTrue(typeof(int).GetTypeInfo().IsDeepCopyByAssignment());
			Assert.IsTrue(typeof(uint).GetTypeInfo().IsDeepCopyByAssignment());
			Assert.IsTrue(typeof(long).GetTypeInfo().IsDeepCopyByAssignment());
			Assert.IsTrue(typeof(ulong).GetTypeInfo().IsDeepCopyByAssignment());
			Assert.IsTrue(typeof(float).GetTypeInfo().IsDeepCopyByAssignment());
			Assert.IsTrue(typeof(double).GetTypeInfo().IsDeepCopyByAssignment());
			Assert.IsTrue(ReflectionHelper.IsDeepCopyByAssignment<bool>());
			Assert.IsTrue(ReflectionHelper.IsDeepCopyByAssignment<byte>());
			Assert.IsTrue(ReflectionHelper.IsDeepCopyByAssignment<sbyte>());
			Assert.IsTrue(ReflectionHelper.IsDeepCopyByAssignment<short>());
			Assert.IsTrue(ReflectionHelper.IsDeepCopyByAssignment<ushort>());
			Assert.IsTrue(ReflectionHelper.IsDeepCopyByAssignment<int>());
			Assert.IsTrue(ReflectionHelper.IsDeepCopyByAssignment<uint>());
			Assert.IsTrue(ReflectionHelper.IsDeepCopyByAssignment<long>());
			Assert.IsTrue(ReflectionHelper.IsDeepCopyByAssignment<ulong>());
			Assert.IsTrue(ReflectionHelper.IsDeepCopyByAssignment<float>());
			Assert.IsTrue(ReflectionHelper.IsDeepCopyByAssignment<double>());

			// Primitive special cases
			Assert.IsTrue(typeof(decimal).GetTypeInfo().IsDeepCopyByAssignment());
			Assert.IsTrue(typeof(string).GetTypeInfo().IsDeepCopyByAssignment());
			Assert.IsTrue(typeof(Alignment).GetTypeInfo().IsDeepCopyByAssignment());
			Assert.IsTrue(ReflectionHelper.IsDeepCopyByAssignment<decimal>());
			Assert.IsTrue(ReflectionHelper.IsDeepCopyByAssignment<string>());
			Assert.IsTrue(ReflectionHelper.IsDeepCopyByAssignment<Alignment>());

			// Pure POD compound types
			Assert.IsTrue(typeof(Vector2).GetTypeInfo().IsDeepCopyByAssignment());
			Assert.IsTrue(typeof(ColorRgba).GetTypeInfo().IsDeepCopyByAssignment());
			Assert.IsTrue(ReflectionHelper.IsDeepCopyByAssignment<Vector2>());
			Assert.IsTrue(ReflectionHelper.IsDeepCopyByAssignment<ColorRgba>());

			// Non-pure compound types
			Assert.IsFalse(typeof(ContentRef<Resource>).GetTypeInfo().IsDeepCopyByAssignment());
			Assert.IsFalse(ReflectionHelper.IsDeepCopyByAssignment<ContentRef<Resource>>());

			// Various kinds of by-reference types
			Assert.IsFalse(typeof(object).GetTypeInfo().IsDeepCopyByAssignment());
			Assert.IsFalse(typeof(int[]).GetTypeInfo().IsDeepCopyByAssignment());
			Assert.IsFalse(typeof(GameObject).GetTypeInfo().IsDeepCopyByAssignment());
			Assert.IsFalse(typeof(IComparable).GetTypeInfo().IsDeepCopyByAssignment());
			Assert.IsFalse(typeof(IContentRef).GetTypeInfo().IsDeepCopyByAssignment());
			Assert.IsFalse(typeof(IComparable<int>).GetTypeInfo().IsDeepCopyByAssignment());
			Assert.IsFalse(ReflectionHelper.IsDeepCopyByAssignment<object>());
			Assert.IsFalse(ReflectionHelper.IsDeepCopyByAssignment<int[]>());
			Assert.IsFalse(ReflectionHelper.IsDeepCopyByAssignment<GameObject>());
			Assert.IsFalse(ReflectionHelper.IsDeepCopyByAssignment<IComparable>());
			Assert.IsFalse(ReflectionHelper.IsDeepCopyByAssignment<IContentRef>());
			Assert.IsFalse(ReflectionHelper.IsDeepCopyByAssignment<IComparable<int>>());
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
