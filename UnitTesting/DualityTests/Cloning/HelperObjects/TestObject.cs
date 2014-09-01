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

	internal class TestObject : IEquatable<TestObject>
	{
		public string StringField;
		public TestData DataField;
		public List<int> ListField;
		public List<string> ListField2;
		public Dictionary<string,TestObject> DictField;
			
		public TestObject() {}
		public TestObject(Random rnd, int childCount)
		{
			this.StringField	= rnd.Next().ToString();
			this.DataField		= new TestData(rnd);
			this.ListField		= Enumerable.Range(rnd.Next(-1000, 1000), 50).ToList();
			this.ListField2		= Enumerable.Range(rnd.Next(-1000, 1000), 50).Select(i => i.ToString()).ToList();
			this.DictField		= new Dictionary<string,TestObject>();

			for (int i = childCount; i > 0; i--)
			{
				this.DictField.Add(rnd.Next().ToString(), new TestObject(rnd, childCount / 2));
			}
		}

		public override bool Equals(object obj)
		{
			if (obj is TestObject)
				return this.Equals((TestObject)obj);
			else
				return base.Equals(obj);
		}
		public bool Equals(TestObject other)
		{
			return 
				other.StringField == this.StringField &&
				object.Equals(other.DataField, this.DataField) &&
				other.ListField.SequenceEqual(this.ListField) &&
				other.ListField2.SequenceEqual(this.ListField2) &&
				other.DictField.SetEqual(this.DictField);
		}
		public bool AnyReferenceEquals(TestObject other)
		{
			if (object.ReferenceEquals(this, other)) return true;
			if (object.ReferenceEquals(this.ListField, other.ListField) && !object.ReferenceEquals(this.ListField, null)) return true;
			if (object.ReferenceEquals(this.ListField2, other.ListField2) && !object.ReferenceEquals(this.ListField2, null)) return true;
			if (object.ReferenceEquals(this.DictField, other.DictField) && !object.ReferenceEquals(this.DictField, null)) return true;
			if (!object.ReferenceEquals(this.DictField, null) && !object.ReferenceEquals(other.DictField, null))
			{
				foreach (var key in this.DictField.Keys)
				{
					var a = this.DictField[key];
					var b = other.DictField[key];
					if (object.ReferenceEquals(a, b) && !object.ReferenceEquals(a, null)) return true;
				}
			}
			return false;
		}
	}
}
