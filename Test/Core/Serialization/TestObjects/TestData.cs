using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using Duality.Serialization;

namespace Duality.Tests.Serialization
{
	public struct TestData : IEquatable<TestData>
	{
		public int IntField;
		public float FloatField;
		public string StringField;
		public SomeEnum EnumField;

		public TestData(Random rnd)
		{
			this.IntField		= rnd.Next();
			this.FloatField		= rnd.NextFloat();
			this.StringField	= rnd.Next().ToString();
			this.EnumField		= (SomeEnum)rnd.Next(10);
		}

		public static bool operator ==(TestData first, TestData second)
		{
			return first.Equals(second);
		}
		public static bool operator !=(TestData first, TestData second)
		{
			return !first.Equals(second);
		}
		public override int GetHashCode()
		{
			return MathF.CombineHashCode(
				this.IntField.GetHashCode(),
				this.FloatField.GetHashCode(),
				this.StringField != null ? this.StringField.GetHashCode() : 0,
				this.EnumField.GetHashCode());
		}
		public override bool Equals(object obj)
		{
			if (obj is TestData)
				return this.Equals((TestData)obj);
			else
				return base.Equals(obj);
		}
		public bool Equals(TestData other)
		{
			return 
				other.IntField == this.IntField &&
				other.FloatField == this.FloatField &&
				other.StringField == this.StringField &&
				other.EnumField == this.EnumField;
		}
	}
}
