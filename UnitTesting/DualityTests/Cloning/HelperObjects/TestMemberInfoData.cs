using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Reflection;

using Duality;
using Duality.Cloning;
using Duality.Drawing;
using Duality.Resources;
using Duality.Components;
using Duality.Components.Renderers;

using OpenTK;
using NUnit.Framework;

namespace Duality.Tests.Cloning.HelperObjects
{
	#pragma warning disable 659  // GetHashCode not implemented

	internal struct TestMemberInfoData : IEquatable<TestMemberInfoData>
	{
		public MemberInfo MemberInfoField;
		public Type TypeField;
		public FieldInfo FieldInfoField;
		public PropertyInfo PropertyInfoField;

		public TestMemberInfoData(Random rnd)
		{
			this.TypeField			= rnd.OneOf(new Type[] { typeof(List<int>), typeof(Queue<float>), typeof(Stack<string>) });
			this.MemberInfoField	= rnd.OneOf(this.TypeField.GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance));
			this.FieldInfoField		= rnd.OneOf(this.TypeField.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance));
			this.PropertyInfoField	= rnd.OneOf(this.TypeField.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance));
		}

		public override bool Equals(object obj)
		{
			if (obj is TestMemberInfoData)
				return this.Equals((TestMemberInfoData)obj);
			else
				return base.Equals(obj);
		}
		public bool Equals(TestMemberInfoData other)
		{
			return 
				other.MemberInfoField == this.MemberInfoField &&
				other.TypeField == this.TypeField &&
				other.FieldInfoField == this.FieldInfoField &&
				other.PropertyInfoField == this.PropertyInfoField;
		}
	}
}
