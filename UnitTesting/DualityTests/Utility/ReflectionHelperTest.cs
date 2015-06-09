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
		[Test] public void CreateInstanceOf()
		{
			// Primitive types
			Assert.IsTrue(CheckCreateInstance<bool>());
			Assert.IsTrue(CheckCreateInstance<byte>());
			Assert.IsTrue(CheckCreateInstance<sbyte>());
			Assert.IsTrue(CheckCreateInstance<short>());
			Assert.IsTrue(CheckCreateInstance<ushort>());
			Assert.IsTrue(CheckCreateInstance<int>());
			Assert.IsTrue(CheckCreateInstance<uint>());
			Assert.IsTrue(CheckCreateInstance<float>());
			Assert.IsTrue(CheckCreateInstance<double>());

			// Pseudo-primitive types
			Assert.IsTrue(CheckCreateInstance<string>());
			Assert.IsTrue(CheckCreateInstance<decimal>());

			// Arrays
			Assert.IsTrue(CheckCreateInstance<int[]>());
			Assert.IsTrue(CheckCreateInstance<string[]>());
			Assert.IsTrue(CheckCreateInstance<object[]>());

			// Structs
			Assert.IsTrue(CheckCreateInstance<TestCreateStructRegular>());

			// Classes
			Assert.IsTrue(CheckCreateInstance<TestCreateClassRegular>());
			Assert.IsTrue(CheckCreateInstance<TestCreateClassPrivate>());
			Assert.IsTrue(CheckCreateInstance<TestCreateClassNonEmpty>());

			// Some Collections
			Assert.IsTrue(CheckCreateInstance<List<int>>());
			Assert.IsTrue(CheckCreateInstance<Dictionary<string,object>>());

			// Things that can't be created
			Assert.IsFalse(CheckCreateInstance<IDisposable>());
			Assert.IsFalse(CheckCreateInstance<TestCreateClassAbstract>());
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

		private struct TestCreateStructRegular { }
		public class TestCreateClassRegular
		{
			public TestCreateClassRegular() { }
		}
		public class TestCreateClassPrivate
		{
			private TestCreateClassPrivate() { }
		}
		public class TestCreateClassNonEmpty
		{
			public TestCreateClassNonEmpty(string value) { }
		}
		public abstract class TestCreateClassAbstract { }

		private bool CheckCreateInstance<T>()
		{
			if (typeof(T).IsValueType)
				return object.Equals(default(T), typeof(T).CreateInstanceOf());
			else
				return typeof(T).CreateInstanceOf() != null;
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
