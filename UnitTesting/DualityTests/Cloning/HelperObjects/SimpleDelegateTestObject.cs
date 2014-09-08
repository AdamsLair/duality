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
	internal class SimpleDelegateTestObject
	{
		[CloneField(CloneFieldFlags.Skip)]
		public int EventReceived = 0;
		public event EventHandler SomeEvent = null;

		public void FireEvent()
		{
			if (this.SomeEvent != null)
				this.SomeEvent(this, EventArgs.Empty);
		}
		public void ListenTo(SimpleDelegateTestObject other)
		{
			other.SomeEvent += this.ReceiveEvent;
		}
		public bool PopEventReceived()
		{
			bool wasTrue = this.EventReceived > 0;
			this.EventReceived = Math.Max(0, this.EventReceived - 1);
			return wasTrue;
		}
		private void ReceiveEvent(object sender, EventArgs e)
		{
			this.EventReceived++;
		}
	}
}
