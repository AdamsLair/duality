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
	internal class SkipFieldTestObject
	{
		public string StringField;
		[CloneField(CloneFieldFlags.Skip)]
		public int SkipField;
		public AlwaysSkippedObject SkippedObject;
			
		public SkipFieldTestObject()
		{
			this.StringField = CloneProviderTest.SharedRandom.Next().ToString();
			this.SkipField = CloneProviderTest.SharedRandom.Next();
			this.SkippedObject = new AlwaysSkippedObject();
		}
	}
	[CloneBehavior(CloneBehavior.WeakReference)]
	internal class AlwaysSkippedObject {}
}
