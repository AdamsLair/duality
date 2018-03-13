﻿using System;
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
		public HashSet<TestObject> HashsetField;
			
		public TestObject() {}
		public TestObject(Random rnd, int childCount)
		{
			this.StringField	= rnd.Next().ToString();
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
				other.DictField.SetEqual(this.DictField) &&
				other.HashsetField.SetEqual(this.HashsetField);
		}
		public bool AnyReferenceEquals(TestObject other)
		{
			if (object.ReferenceEquals(this, other)) return true;
			if (object.ReferenceEquals(this.ListField, other.ListField) && !object.ReferenceEquals(this.ListField, null)) return true;
			if (object.ReferenceEquals(this.ListField2, other.ListField2) && !object.ReferenceEquals(this.ListField2, null)) return true;
			if (object.ReferenceEquals(this.DictField, other.DictField) && !object.ReferenceEquals(this.DictField, null)) return true;
			if (!object.ReferenceEquals(this.DictField, null) && !object.ReferenceEquals(other.DictField, null))
			{
				foreach (string key in this.DictField.Keys)
				{
					TestObject a = this.DictField[key];
					TestObject b;
					if (other.DictField.TryGetValue(key, out b))
					{
						if (object.ReferenceEquals(a, b) && !object.ReferenceEquals(a, null)) return true;
					}
				}
			}
			if (!object.ReferenceEquals(this.HashsetField, null) && !object.ReferenceEquals(other.HashsetField, null))
			{
				foreach (TestObject a in this.HashsetField)
				{
					foreach (TestObject b in other.HashsetField)
					{
						if (object.ReferenceEquals(a, b) && !object.ReferenceEquals(a, null)) return true;
					}
				}
			}
			return false;
		}
	}
}
