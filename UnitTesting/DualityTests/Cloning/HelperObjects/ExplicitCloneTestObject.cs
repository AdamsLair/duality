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
	internal class ExplicitCloneTestObjectA : TestObject, ICloneExplicit
	{
		public ExplicitCloneTestObjectA(Random rnd, int maxChildren) : base(rnd, maxChildren) {}
			
		void ICloneExplicit.SetupCloneTargets(object target, ICloneTargetSetup setup)
		{
			ExplicitCloneTestObjectA targetCast = target as ExplicitCloneTestObjectA;

			setup.HandleObject(this.ListField, targetCast.ListField);
			setup.HandleObject(this.ListField2, targetCast.ListField2);
			setup.HandleObject(this.DictField, targetCast.DictField);
		}
		void ICloneExplicit.CopyDataTo(object targetObj, ICloneOperation operation)
		{
			ExplicitCloneTestObjectA target = targetObj as ExplicitCloneTestObjectA;
			target.StringField = this.StringField;
			target.DataField = this.DataField;
			operation.AutoHandleObject(this.ListField, out target.ListField);
			operation.AutoHandleObject(this.ListField2, out target.ListField2);
			if (operation.GetTarget(this.DictField, out target.DictField))
			{
				foreach (var pair in this.DictField)
				{
					TestObject newObj;
					if (operation.AutoHandleObject(pair.Value, out newObj))
					{
						target.DictField[pair.Key] = newObj;
					}
				}
			}
		}
	}
	internal class ExplicitCloneTestObjectB : TestObject, ICloneExplicit
	{
		[CloneField(CloneFieldFlags.Skip)]
		public bool SpecialSetupDone = false;

		public ExplicitCloneTestObjectB(Random rnd, int maxChildren) : base(rnd, maxChildren) {}

		void ICloneExplicit.SetupCloneTargets(object targetObj, ICloneTargetSetup setup)
		{
			setup.HandleObject(this, targetObj);
		}
		void ICloneExplicit.CopyDataTo(object targetObj, ICloneOperation operation)
		{
			operation.AutoHandleObject(this, targetObj as ExplicitCloneTestObjectB);
			(targetObj as ExplicitCloneTestObjectB).SpecialSetupDone = true;
		}
	}
	internal class ExplicitCloneTestObjectC : TestObject, ICloneExplicit
	{
		public ExplicitCloneTestObjectC(Random rnd, int maxChildren) : base(rnd, maxChildren) {}

		void ICloneExplicit.SetupCloneTargets(object targetObj, ICloneTargetSetup setup) {}
		void ICloneExplicit.CopyDataTo(object targetObj, ICloneOperation operation) {}
	}
}
