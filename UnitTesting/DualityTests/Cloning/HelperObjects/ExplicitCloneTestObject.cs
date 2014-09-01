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
			
		void ICloneExplicit.SetupCloneTargets(ICloneTargetSetup setup)
		{
			setup.AutoHandleObject(this.ListField);
			setup.AutoHandleObject(this.ListField2);
			setup.AddTarget(this.DictField, new Dictionary<string,TestObject>());
			foreach (var pair in this.DictField)
			{
				setup.AutoHandleObject(pair.Value);
			}
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

		void ICloneExplicit.SetupCloneTargets(ICloneTargetSetup setup)
		{
			setup.AutoHandleObject(this);
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

		void ICloneExplicit.SetupCloneTargets(ICloneTargetSetup setup) {}
		void ICloneExplicit.CopyDataTo(object targetObj, ICloneOperation operation) {}
	}
}
