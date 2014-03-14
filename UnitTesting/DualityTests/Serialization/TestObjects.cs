using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Duality.Tests.Serialization
{
	public enum SomeEnum
	{
		Zero,
		First,
		Second,
		Third
	}
	[Serializable]
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
	[Serializable]
	public class TestObject : IEquatable<TestObject>
	{
		public string StringField;
		public Type TypeField;
		public Func<int,bool> DelegateField;
		public TestData DataField;
		public List<int> ListField;
		public List<string> ListField2;
		public Dictionary<string,TestObject> DictField;
			
		public TestObject(Random rnd, int maxChildren = 5)
		{
			this.StringField	= rnd.Next().ToString();
			this.TypeField		= rnd.OneOf(new[] { typeof(int), typeof(Component), typeof(GenericOperator) });
			this.DelegateField	= rnd.OneOf(new Func<int,bool>[] { DelegateA, DelegateB });
			this.DataField		= new TestData(rnd);
			this.ListField		= Enumerable.Range(rnd.Next(-1000, 1000), rnd.Next(0, 50)).ToList();
			this.ListField2		= Enumerable.Range(rnd.Next(-1000, 1000), rnd.Next(0, 50)).Select(i => i.ToString()).ToList();
			this.DictField		= new Dictionary<string,TestObject>();

			for (int i = rnd.Next(0, maxChildren); i > 0; i--)
			{
				this.DictField.Add(rnd.Next().ToString(), new TestObject(rnd, maxChildren / 2));
			}
		}

		private static bool DelegateA(int v) { return false; } 
		private static bool DelegateB(int v) { return true; } 

		public static bool operator ==(TestObject first, TestObject second)
		{
			return first.Equals(second);
		}
		public static bool operator !=(TestObject first, TestObject second)
		{
			return !first.Equals(second);
		}
		public override bool Equals(object obj)
		{
			if (obj is TestObject)
				return this.Equals((TestObject)obj);
			else
				return base.Equals(obj);
		}
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
		public bool Equals(TestObject other)
		{
			return 
				other.StringField == this.StringField &&
				other.TypeField == this.TypeField &&
				other.DelegateField == this.DelegateField &&
				other.DataField == this.DataField &&
				ListsEqual(other.ListField, this.ListField) &&
				ListsEqual(other.ListField2, this.ListField2) &&
				DictionariesEqual(other.DictField, this.DictField);
		}

		private static bool ListsEqual<T>(IList<T> first, IList<T> second)
		{
			if (object.ReferenceEquals(first, second)) return true;
			if (object.ReferenceEquals(first, null)) return false;
			if (object.ReferenceEquals(second, null)) return false;
			return first.SequenceEqual(second);
		}
		private static bool DictionariesEqual<T,U>(IDictionary<T,U> first, IDictionary<T,U> second)
		{
			if (object.ReferenceEquals(first, second)) return true;
			if (object.ReferenceEquals(first, null)) return false;
			if (object.ReferenceEquals(second, null)) return false;
			return first.SetEqual(second);
		}
	}
}
