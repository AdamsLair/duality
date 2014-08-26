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
	internal class IdentityTestObjectA
	{
		public string StringField;
		[CloneField(CloneFieldFlags.IdentityRelevant)]
		public Guid Identity;
			
		public IdentityTestObjectA()
		{
			this.StringField = CloneProviderTest.SharedRandom.Next().ToString();
			this.Identity = Guid.NewGuid();
		}
	}
	internal class IdentityTestObjectB
	{
		public string StringField;
		[CloneField(CloneFieldFlags.IdentityRelevant)]
		public ReferencedObject Identity;
			
		public IdentityTestObjectB()
		{
			this.StringField = CloneProviderTest.SharedRandom.Next().ToString();
			this.Identity = new ReferencedObject();
		}
	}
	internal class IdentityTestObjectC
	{
		public string StringField;
		[CloneField(CloneFieldFlags.IdentityRelevant)]
		public int Identity;
			
		public IdentityTestObjectC()
		{
			this.StringField = CloneProviderTest.SharedRandom.Next().ToString();
			this.Identity = CloneProviderTest.SharedRandom.Next();
		}
	}
}
