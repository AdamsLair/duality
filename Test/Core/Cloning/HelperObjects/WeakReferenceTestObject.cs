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

using NUnit.Framework;

namespace Duality.Tests.Cloning.HelperObjects
{
	internal class WeakReferenceTestObject : ICloneExplicit
	{
		[CloneField(CloneFieldFlags.Skip)]
		public WeakReferenceTestObject Parent;
		[CloneBehavior(typeof(WeakReferenceTestObject), CloneBehavior.ChildObject)]
		public List<WeakReferenceTestObject> Children;

		public WeakReferenceTestObject() : this(new WeakReferenceTestObject[0]) {}
		public WeakReferenceTestObject(IEnumerable<WeakReferenceTestObject> children)
		{
			this.Children = children.ToList();
			foreach (var child in this.Children)
			{
				child.Parent = this;
			}
		}

		public bool CheckChildIntegrity()
		{
			foreach (var child in this.Children)
			{
				if (child.Parent != this) return false;
				if (!child.CheckChildIntegrity()) return false;
			}
			return true;
		}
		public bool AnyReferenceEquals(WeakReferenceTestObject other)
		{
			if (object.ReferenceEquals(this, other)) return true;
			if (object.ReferenceEquals(this.Parent, other.Parent) && !object.ReferenceEquals(this.Parent, null)) return true;
			if (object.ReferenceEquals(this.Children, other.Children) && !object.ReferenceEquals(this.Children, null)) return true;
			if (this.Children.Zip(other.Children, (a, b) => a.AnyReferenceEquals(b)).Contains(true)) return true;
			return false;
		}

		void ICloneExplicit.SetupCloneTargets(object target, ICloneTargetSetup setup)
		{
			setup.HandleObject(this, target);
		}
		void ICloneExplicit.CopyDataTo(object target, ICloneOperation operation)
		{
			operation.HandleObject(this, target);

			// If the source objects parent was cloned as well, assign its
			// cloned instance to our target object. 
			WeakReferenceTestObject testTarget = target as WeakReferenceTestObject;
			testTarget.Parent = operation.GetWeakTarget(this.Parent);
		}
	}
}
