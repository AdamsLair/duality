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

	internal class OwnershipTestObject : IEquatable<OwnershipTestObject>
	{
		[CloneBehavior(CloneBehavior.ChildObject)]
		public ReferencedObject NestedObject;
		[CloneBehavior(typeof(ReferencedObject), CloneBehavior.ChildObject)]
		public Dictionary<string,ReferencedObject> ObjectStore;
			
		public OwnershipTestObject()
		{
			this.NestedObject = new ReferencedObject { TestProperty = CloneProviderTest.SharedRandom.Next().ToString() };
			this.ObjectStore = new Dictionary<string,ReferencedObject>();

			for (int i = CloneProviderTest.SharedRandom.Next(0, 3); i > 0; i--)
			{
				string name = CloneProviderTest.SharedRandom.Next().ToString();
				this.ObjectStore.Add(name, new ReferencedObject { TestProperty = name });
			}
		}

		public override bool Equals(object obj)
		{
			if (obj is OwnershipTestObject)
				return this.Equals((OwnershipTestObject)obj);
			else
				return base.Equals(obj);
		}
		public bool Equals(OwnershipTestObject other)
		{
			if (other.NestedObject != null && this.NestedObject != null)
			{
				if (other.NestedObject.TestProperty != this.NestedObject.TestProperty) return false;
			}
			if (other.ObjectStore != null && this.ObjectStore != null)
			{
				if (other.ObjectStore.Count != this.ObjectStore.Count) return false;
				foreach (var pair in other.ObjectStore)
				{
					if (!this.ObjectStore.ContainsKey(pair.Key)) return false;
					var otherObj = this.ObjectStore[pair.Key];
					if (otherObj != null && pair.Value != null)
					{
						if (otherObj.TestProperty != pair.Value.TestProperty) return false;
					}
				}
			}
			return true;
		}
		public bool AnyReferenceEquals(OwnershipTestObject other)
		{
			if (object.ReferenceEquals(this, other)) return true;
			if (object.ReferenceEquals(this.NestedObject, other.NestedObject) && !object.ReferenceEquals(this.NestedObject, null)) return true;
			if (object.ReferenceEquals(this.ObjectStore, other.ObjectStore) && !object.ReferenceEquals(this.ObjectStore, null)) return true;
			foreach (var key in this.ObjectStore.Keys)
			{
				var a = this.ObjectStore[key];
				var b = other.ObjectStore[key];
				if (object.ReferenceEquals(a, b) && !object.ReferenceEquals(a, null)) return true;
			}
			return false;
		}
	}
}
