using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Duality.Cloning.Surrogates
{
	public class DelegateSurrogate : CloneSurrogate<Delegate>
	{
		protected override bool IsImmutableTarget
		{
			get { return true; }
		}
		public override bool MatchesType(Type t)
		{
			return typeof(Delegate).IsAssignableFrom(t);
		}

		public override void CreateTargetObject(Delegate source, out Delegate target, ICloneTargetSetup setup)
		{
			// Because delegates are immutable, we'll need to defer their creation until we know exactly how the cloned object graph looks like.
			target = null;
		}
		public override void SetupCloneTargets(Delegate source, Delegate target, ICloneTargetSetup setup)
		{
			// Flag all invocation targets as weak references.
			Delegate[] invokeList = source.GetInvocationList();
			for (int i = 0; i < invokeList.Length; i++)
			{
				setup.HandleObject(invokeList[i].Target, CloneBehavior.WeakReference);
			}
		}
		public override void CreateTargetObjectLate(Delegate source, out Delegate target, ICloneOperation operation)
		{
			Delegate[] sourceInvokeList = source.GetInvocationList();
			RawList<Delegate> targetInvokeList = new RawList<Delegate>(sourceInvokeList.Length);

			// Iterate over our sources invocation list and see which entries are part of the target object graph
			for (int i = 0; i < sourceInvokeList.Length; i++)
			{
				object invokeTargetObject;
				if (sourceInvokeList[i].Target == null)
				{
					Delegate targetSubDelegate = Delegate.CreateDelegate(sourceInvokeList[i].GetType(), null, sourceInvokeList[i].Method);
					targetInvokeList.Add(targetSubDelegate);
				}
				else if (operation.GetTarget(sourceInvokeList[i].Target, out invokeTargetObject))
				{
					Delegate targetSubDelegate = Delegate.CreateDelegate(sourceInvokeList[i].GetType(), invokeTargetObject, sourceInvokeList[i].Method);
					targetInvokeList.Add(targetSubDelegate);
				}
			}

			// Create a new delegate instance
			Delegate[] targetInvokeArray = targetInvokeList.Data;
			if (targetInvokeArray.Length != targetInvokeList.Count)
			{
				Array.Resize(ref targetInvokeArray, targetInvokeList.Count);
			}
			target = Delegate.Combine(targetInvokeArray);
		}
		public override void CopyDataTo(Delegate source, Delegate target, ICloneOperation operation)
		{
			// Delegates are immutable. Nothing to do here.
		}
	}
}
