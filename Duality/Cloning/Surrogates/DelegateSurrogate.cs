using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Duality.Cloning.Surrogates
{
	public class DelegateSurrogate : Surrogate<Delegate>
	{
		public override bool MatchesType(Type t)
		{
			return typeof(Delegate).IsAssignableFrom(t);
		}

		public override void CreateTargetObject(Delegate source, out Delegate target, ICloneTargetSetup setup)
		{
			Delegate[] baseInvokeList = source.GetInvocationList();
			Delegate[] cloneInvokeList = new Delegate[baseInvokeList.Length];

			for (int i = 0; i < baseInvokeList.Length; i++)
			{
				// Shallow-copy the delegate first
				cloneInvokeList[i] = baseInvokeList[i].Clone() as Delegate;

				// Register the new delegate
				setup.AddTarget(baseInvokeList[i], cloneInvokeList[i]);
				setup.MakeWeakReference(cloneInvokeList[i].Target);
			}

			target = Delegate.Combine(cloneInvokeList);
		}
		public override void SetupCloneTargets(Delegate source, ICloneTargetSetup setup) {}
		public override void CopyDataTo(Delegate source, Delegate target, ICloneOperation operation)
		{
			Type delType = source.GetType();

			Delegate[] baseInvokeList = source.GetInvocationList();
			for (int i = 0; i < baseInvokeList.Length; i++)
			{
				// Adjust previously shallow-copied target to reference actual target instances
				Delegate targetInvoke;
				if (operation.GetTarget(baseInvokeList[i], out targetInvoke) && targetInvoke != null)
				{
					FieldInfo targetField = delType.GetField("_target", ReflectionHelper.BindInstanceAll);
					object targetInvokeTarget;
					if (operation.GetTarget(targetInvoke.Target, out targetInvokeTarget))
					{
						targetField.SetValue(targetInvoke, targetInvokeTarget);
					}
					else
					{
					}
				}
			}
		}
	}
}
