using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

using Duality;

using NUnit.Framework;

namespace Duality.Tests.Utility
{
	[TestFixture]
	public class ExtMethodsMemberInfo
	{
		[Test] public void IsEquivalentFields()
		{
			Type typeBase = typeof(MemberInfoEquivalenceBase);
			Type typeDerived = typeof(MemberInfoEquivalence);

			MemberInfo memberBase = typeBase.GetRuntimeFields().First(m => m.Name == "SomeField");
			MemberInfo memberDerived = typeDerived.GetRuntimeFields().First(m => m.Name == "SomeField");
			MemberInfo memberOther = typeDerived.GetRuntimeFields().First(m => m.Name == "SomeField2");

			AssertEquivalence(memberBase, memberDerived);
			AssertNoEquivalence(memberOther, memberBase);
			AssertNoEquivalence(memberOther, memberDerived);
		}
		[Test] public void IsEquivalentProperties()
		{
			Type typeBase = typeof(MemberInfoEquivalenceBase);
			Type typeDerived = typeof(MemberInfoEquivalence);

			MemberInfo memberBase = typeBase.GetRuntimeProperties().First(m => m.Name == "SomeProperty");
			MemberInfo memberDerived = typeDerived.GetRuntimeProperties().First(m => m.Name == "SomeProperty");
			MemberInfo memberOther = typeDerived.GetRuntimeProperties().First(m => m.Name == "SomeProperty2");

			AssertEquivalence(memberBase, memberDerived);
			AssertNoEquivalence(memberOther, memberBase);
			AssertNoEquivalence(memberOther, memberDerived);
		}
		[Test] public void IsEquivalentEvents()
		{
			Type typeBase = typeof(MemberInfoEquivalenceBase);
			Type typeDerived = typeof(MemberInfoEquivalence);

			MemberInfo memberBase = typeBase.GetRuntimeEvents().First(m => m.Name == "SomeEvent");
			MemberInfo memberDerived = typeDerived.GetRuntimeEvents().First(m => m.Name == "SomeEvent");
			MemberInfo memberOther = typeDerived.GetRuntimeEvents().First(m => m.Name == "SomeEvent2");
			
			AssertEquivalence(memberBase, memberDerived);
			AssertNoEquivalence(memberOther, memberBase);
			AssertNoEquivalence(memberOther, memberDerived);
		}
		[Test] public void IsEquivalentMethods()
		{
			Type typeBase = typeof(MemberInfoEquivalenceBase);
			Type typeDerived = typeof(MemberInfoEquivalence);

			MemberInfo memberBase = typeBase.GetRuntimeMethods().First(m => m.Name == "SomeMethod");
			MemberInfo memberDerived = typeDerived.GetRuntimeMethods().First(m => m.Name == "SomeMethod");
			MemberInfo memberOther = typeDerived.GetRuntimeMethods().First(m => m.Name == "SomeMethod2");
			
			AssertEquivalence(memberBase, memberDerived);
			AssertNoEquivalence(memberOther, memberBase);
			AssertNoEquivalence(memberOther, memberDerived);

			MethodInfo memberGenericBase = typeBase.GetRuntimeMethods().First(m => m.Name == "SomeGenericMethod");
			MethodInfo memberGenericDerived = typeDerived.GetRuntimeMethods().First(m => m.Name == "SomeGenericMethod");
			MethodInfo memberGenericOther = typeDerived.GetRuntimeMethods().First(m => m.Name == "SomeGenericMethod2");
			
			AssertEquivalence(memberGenericBase, memberGenericDerived);
			AssertNoEquivalence(memberGenericOther, memberGenericBase);
			AssertNoEquivalence(memberGenericOther, memberGenericDerived);

			AssertEquivalence(
				memberGenericBase.MakeGenericMethod(typeof(int), typeof(float)), 
				memberGenericDerived.MakeGenericMethod(typeof(int), typeof(float)));
			AssertNoEquivalence(
				memberGenericBase.MakeGenericMethod(typeof(int), typeof(float)), 
				memberGenericDerived.MakeGenericMethod(typeof(int), typeof(string)));
			AssertNoEquivalence(
				memberGenericOther.MakeGenericMethod(typeof(int), typeof(float)), 
				memberGenericBase.MakeGenericMethod(typeof(int), typeof(float)));
			AssertNoEquivalence(
				memberGenericOther.MakeGenericMethod(typeof(int), typeof(float)), 
				memberGenericDerived.MakeGenericMethod(typeof(int), typeof(float)));
		}

		private void AssertEquivalence(MemberInfo memberBase, MemberInfo memberEqual)
		{
			Assert.IsTrue(memberEqual.IsEquivalent(memberBase));
			Assert.IsTrue(memberBase.IsEquivalent(memberEqual));
		}
		private void AssertNoEquivalence(MemberInfo memberBase, MemberInfo memberEqual)
		{
			Assert.IsFalse(memberEqual.IsEquivalent(memberBase));
			Assert.IsFalse(memberBase.IsEquivalent(memberEqual));
		}
		
		private class MemberInfoEquivalenceBase
		{
			public int SomeField;
			public int SomeField2;
			public int SomeProperty
			{
				get { return this.SomeField; }
			}
			public int SomeProperty2
			{
				get { return this.SomeField2; }
			}
			public void SomeMethod(int value) { }
			public void SomeMethod2(int value) { }
			public void SomeGenericMethod<T,U>(T value, U value2) { }
			public void SomeGenericMethod2<T,U>(T value, U value2) { }
			public event EventHandler SomeEvent;
			public event EventHandler SomeEvent2;
		}
		private class MemberInfoEquivalence : MemberInfoEquivalenceBase { }
	}
}
