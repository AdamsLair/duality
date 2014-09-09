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
		public override bool RequireMerge
		{
			get { return true; }
		}

		public override void CreateTargetObject(Delegate source, ref Delegate target, ICloneTargetSetup setup)
		{
			// Because delegates are immutable, we'll need to defer their creation until we know exactly how the cloned object graph looks like.
			target = null;
		}
		public override void SetupCloneTargets(Delegate source, Delegate target, ICloneTargetSetup setup)
		{
			if (source == null) return;

			// Flag all invocation targets as weak references.
			Delegate[] invokeList = source.GetInvocationList();
			for (int i = 0; i < invokeList.Length; i++)
			{
				setup.HandleObject(invokeList[i].Target, null, CloneBehavior.WeakReference);
			}
		}
		public override void CreateTargetObjectLate(Delegate source, ref Delegate target, ICloneOperation operation)
		{
			Delegate[] sourceInvokeList = (source != null) ? source.GetInvocationList() : null;
			Delegate[] targetInvokeList = (target != null) ? target.GetInvocationList() : null;
			RawList<Delegate> mergedInvokeList = new RawList<Delegate>(
				((sourceInvokeList != null) ? sourceInvokeList.Length : 0) + 
				((targetInvokeList != null) ? targetInvokeList.Length : 0));

			// Iterate over our sources invocation list and copy entries are part of the target object graph
			if (sourceInvokeList != null)
			{
				for (int i = 0; i < sourceInvokeList.Length; i++)
				{
					if (sourceInvokeList[i].Target == null) continue;

					object invokeTargetObject = null;
					if (operation.GetTarget(sourceInvokeList[i].Target, ref invokeTargetObject))
					{
						Delegate targetSubDelegate = Delegate.CreateDelegate(sourceInvokeList[i].GetType(), invokeTargetObject, sourceInvokeList[i].Method);
						mergedInvokeList.Add(targetSubDelegate);
					}
				}
			}

			// Iterate over our targets invocation list and keep entries that are NOT part of the target object graph
			if (targetInvokeList != null)
			{
				for (int i = 0; i < targetInvokeList.Length; i++)
				{
					if (targetInvokeList[i].Target == null) continue;

					if (!operation.IsTarget(targetInvokeList[i].Target))
					{
						Delegate targetSubDelegate = Delegate.CreateDelegate(targetInvokeList[i].GetType(), targetInvokeList[i].Target, targetInvokeList[i].Method);
						mergedInvokeList.Add(targetSubDelegate);
					}
				}
			}

			// Create a new delegate instance
			Delegate[] mergedInvokeArray = mergedInvokeList.Data;
			if (mergedInvokeArray.Length != mergedInvokeList.Count)
			{
				Array.Resize(ref mergedInvokeArray, mergedInvokeList.Count);
			}
			target = Delegate.Combine(mergedInvokeArray);
		}
		public override void CopyDataTo(Delegate source, Delegate target, ICloneOperation operation)
		{
			// Delegates are immutable. Nothing to do here.
		}
	}
}
