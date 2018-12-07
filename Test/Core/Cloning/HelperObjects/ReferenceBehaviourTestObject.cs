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
	internal class ReferenceBehaviourTestObject : ICloneExplicit
	{
		public ReferencedObject ReferencedObject;
		[CloneBehavior(CloneBehavior.ChildObject)]
		public ReferencedObject OwnedObject;
		[CloneField(CloneFieldFlags.Skip)]
		public ReferencedObject WeakReferencedObject;

		void ICloneExplicit.SetupCloneTargets(object target, ICloneTargetSetup setup)
		{
			setup.HandleObject(this, target);
		}
		void ICloneExplicit.CopyDataTo(object target, ICloneOperation operation)
		{
			operation.HandleObject(this, target);

			// Only assign WeakReferencedObject, if it was cloned as well. Otherwise, discard.
			ReferenceBehaviourTestObject testTarget = target as ReferenceBehaviourTestObject;
			testTarget.WeakReferencedObject = operation.GetWeakTarget(this.WeakReferencedObject);
		}
	}
}
