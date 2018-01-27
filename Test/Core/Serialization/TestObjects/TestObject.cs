﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using Duality.Serialization;

namespace Duality.Tests.Serialization
{
	public class TestObject : IEquatable<TestObject>
	{
		public string StringField;
		public Type TypeField;
		public Func<int,bool> DelegateField;
		public TestData DataField;
		public List<int> ListField;
		public List<string> ListField2;
		public Dictionary<string,TestObject> DictField;
		public HashSet<TestObject> HashsetField;
		
		private TestObject() {}
		public TestObject(Random rnd, int childCount = 5)
		{
			this.StringField	= rnd.Next().ToString();
			this.TypeField		= rnd.OneOf(new[] { typeof(int), typeof(Component), typeof(GenericOperator) });
			this.DelegateField	= rnd.OneOf(new Func<int,bool>[] { DelegateA, DelegateB });
			this.DataField		= new TestData(rnd);
			this.ListField		= Enumerable.Range(rnd.Next(-1000, 1000), 50).ToList();
			this.ListField2		= Enumerable.Range(rnd.Next(-1000, 1000), 50).Select(i => i.ToString()).ToList();
			this.DictField		= new Dictionary<string,TestObject>();
			this.HashsetField	= new HashSet<TestObject>();

			for (int i = (childCount + 1) / 2; i > 0; i--)
			{
				this.DictField.Add(rnd.Next().ToString(), new TestObject(rnd, childCount / 2));
			}
			for (int i = childCount / 2; i > 0; i--)
			{
				this.HashsetField.Add(new TestObject(rnd, childCount / 2));
			}
		}
		public static TestObject CreateBackwardsCompatible(Random rnd, int maxChildren = 5)
		{
			TestObject obj = new TestObject();

			obj.StringField	= rnd.Next().ToString();
			obj.TypeField		= rnd.OneOf(new[] { typeof(int), typeof(Component), typeof(GenericOperator) });
			obj.DelegateField	= rnd.OneOf(new Func<int,bool>[] { DelegateA, DelegateB });
			obj.DataField		= new TestData(rnd);
			obj.ListField		= Enumerable.Range(rnd.Next(-1000, 1000), rnd.Next(0, 50)).ToList();
			obj.ListField2		= Enumerable.Range(rnd.Next(-1000, 1000), rnd.Next(0, 50)).Select(i => i.ToString()).ToList();
			obj.DictField		= new Dictionary<string,TestObject>();
			obj.HashsetField	= null;

			for (int i = rnd.Next(0, maxChildren); i > 0; i--)
			{
				obj.DictField.Add(rnd.Next().ToString(), CreateBackwardsCompatible(rnd, maxChildren / 2));
			}

			return obj;
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
				DictionariesEqual(other.DictField, this.DictField) &&
				HashsetsEqual(other.HashsetField, this.HashsetField);
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
		private static bool HashsetsEqual<T>(HashSet<T> first, HashSet<T> second)
		{
			if (object.ReferenceEquals(first, second)) return true;
			if (object.ReferenceEquals(first, null)) return false;
			if (object.ReferenceEquals(second, null)) return false;
			return first.SetEqual(second);
		}
	}
}
