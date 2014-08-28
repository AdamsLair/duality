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
	internal class DelegateTestObject : WeakReferenceTestObject
	{
		[CloneField(CloneFieldFlags.Skip)]
		public bool EventReceived = false;
		public event EventHandler SomeEvent = null;

		public DelegateTestObject() : this(new DelegateTestObject[0]) {}
		public DelegateTestObject(IEnumerable<DelegateTestObject> children) : base(children)
		{
			foreach (var child in children)
			{
				this.ConnectTo(child);
			}
		}

		public DelegateTestObject GetBottomChild()
		{
			if (this.Children == null || this.Children.Count == 0)
			{
				return this;
			}
			else
			{
				DelegateTestObject childWithMostChildren = this.Children.OrderByDescending(c => c.Children != null ? c.Children.Count : 0).First() as DelegateTestObject;
				return childWithMostChildren.GetBottomChild();
			}
		}
		public void FireEvent()
		{
			if (this.SomeEvent != null)
				this.SomeEvent(this, EventArgs.Empty);
		}
		public void ConnectTo(DelegateTestObject other)
		{
			other.SomeEvent += this.ReceiveEvent;
		}
		public bool AnyEventsReceived()
		{
			if (this.EventReceived) return true;
			foreach (DelegateTestObject child in this.Children)
			{
				if (child.AnyEventsReceived()) return true;
			}
			return false;
		}
		public void ResetAllEventsReceived()
		{
			this.EventReceived = false;
			foreach (DelegateTestObject child in this.Children)
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
