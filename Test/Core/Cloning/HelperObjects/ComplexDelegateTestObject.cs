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
	internal class ComplexDelegateTestObject : WeakReferenceTestObject
	{
		[CloneField(CloneFieldFlags.Skip)]
		public bool EventReceived = false;
		public event EventHandler SomeEvent = null;

		public ComplexDelegateTestObject() : this(new ComplexDelegateTestObject[0]) {}
		public ComplexDelegateTestObject(IEnumerable<ComplexDelegateTestObject> children) : base(children)
		{
			foreach (var child in children)
			{
				this.ListenTo(child);
			}
		}

		public ComplexDelegateTestObject GetBottomChild()
		{
			if (this.Children == null || this.Children.Count == 0)
			{
				return this;
			}
			else
			{
				ComplexDelegateTestObject childWithMostChildren = this.Children.OrderByDescending(c => c.Children != null ? c.Children.Count : 0).First() as ComplexDelegateTestObject;
				return childWithMostChildren.GetBottomChild();
			}
		}
		public void FireEvent()
		{
			if (this.SomeEvent != null)
				this.SomeEvent(this, EventArgs.Empty);
		}
		public void ListenTo(ComplexDelegateTestObject other)
		{
			other.SomeEvent += this.ReceiveEvent;
		}
		public bool AnyEventsReceived()
		{
			if (this.EventReceived) return true;
			foreach (ComplexDelegateTestObject child in this.Children)
			{
				if (child.AnyEventsReceived()) return true;
			}
			return false;
		}
		public void ResetAllEventsReceived()
		{
			this.EventReceived = false;
			foreach (ComplexDelegateTestObject child in this.Children)
			{
				child.ResetAllEventsReceived();
			}
		}

		private void ReceiveEvent(object sender, EventArgs e)
		{
			this.EventReceived = true;
			this.FireEvent();
		}
	}
}
