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
	internal class ReferenceBehaviourTestObject
	{
		public ReferencedObject ReferencedObject;
		[CloneBehavior(CloneBehavior.ChildObject)]
		public ReferencedObject OwnedObject;
		[CloneBehavior(CloneBehavior.WeakReference)]
		public ReferencedObject WeakReferencedObject;
	}
}
