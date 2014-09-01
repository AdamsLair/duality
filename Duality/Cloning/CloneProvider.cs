using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Duality.Cloning.Surrogates;

namespace Duality.Cloning
{
	public class CloneProvider : ICloneTargetSetup, ICloneOperation
	{
		private struct CloneBehaviorEntry
		{
			public CloneBehaviorAttribute Behavior;
			public bool Locked;

			public CloneBehaviorEntry(CloneBehaviorAttribute attribute)
			{
				this.Behavior = attribute;
				this.Locked = false;
			}
		}

		private	CloneProviderContext		context				= CloneProviderContext.Default;

		private	object						sourceRoot			= null;
		private	object						currentObject		= null;
		private	CloneType					currentCloneType	= null;
		private	Dictionary<object,object>	objTargets			= new Dictionary<object,object>();
		private	HashSet<object>				lateSetupObjects	= new HashSet<object>();
		private	HashSet<object>				handledObjects		= new HashSet<object>();
		private	HashSet<object>				dropWeakReferences	= new HashSet<object>();
		private	RawList<CloneBehaviorEntry>	localBehavior		= new RawList<CloneBehaviorEntry>();
		

		/// <summary>
		/// [GET] Provides information about the context in which the operation is performed.
		/// </summary>
		public CloneProviderContext Context
		{
			get { return this.context; }
		}
		

		public CloneProvider(CloneProviderContext context = null)
		{
			if (context != null) this.context = context;
		}

		public T CloneObject<T>(T source)
		{
			object target; // Don't use T, we'll need to make sure "target" is a reference Type
			try
			{
				target = this.BeginCloneOperation(source);
				this.PerformCopyObject(source, target, null);
			}
			finally
			{
				this.EndCloneOperation();
			}
			return (T)target;
		}
		public void CopyObject<T>(T source, T target)
		{
			try
			{
				this.BeginCloneOperation(source, target);
				this.PerformCopyObject(source, target, null);
			}
			finally
			{
				this.EndCloneOperation();
			}
		}
		
		private object BeginCloneOperation(object source, object target = null)
		{
			this.sourceRoot = source;
			if (target != null)
			{
				this.SetTargetOf(source, target);
			}
			this.PrepareCloneGraph();
			this.GetTargetOf(source, out target);
			return target;
		}
		private void EndCloneOperation()
		{
			this.sourceRoot = null;
			this.currentObject = null;
			this.currentCloneType = null;
			this.objTargets.Clear();
			this.localBehavior.Clear();
			this.lateSetupObjects.Clear();
			this.handledObjects.Clear();
			this.dropWeakReferences.Clear();
		}

		private void SetTargetOf(object source, object target)
		{
			if (object.ReferenceEquals(source, null)) return;
			this.objTargets[source] = target;
		}
		private bool GetTargetOf(object source, out object target)
		{
			if (object.ReferenceEquals(source, null))
			{
				target = null;
				return true;
			}

			if (!this.objTargets.TryGetValue(source, out target))
			{
				if (this.dropWeakReferences.Contains(source))
				{
					target = null;
					return false;
				}
				target = source;
			}
			return true;
		}
		/// <summary>
		/// Flags the specified object as being already handled during the current clone operation.
		/// </summary>
		/// <param name="source"></param>
		/// <returns>True, if the object is now handled for the first time, false if it has already been handled.</returns>
		private bool HandleObject(object source)
		{
			return this.handledObjects.Add(source);
		}

		private void PrepareCloneGraph()
		{
			// Visit the object graph in order to determine which objects to clone
			this.PrepareCloneGraph(this.sourceRoot, true, null);
			this.localBehavior.Clear();

			// Determine which weak references to keep
			if (this.dropWeakReferences.Count > 0)
			{
				foreach (object source in this.objTargets.Keys)
				{
					this.dropWeakReferences.Remove(source);
					if (this.dropWeakReferences.Count == 0) break;
				}
			}

			// Perform late setup for surrogate objects that required it
			foreach (object lateSetupSource in this.lateSetupObjects)
			{
				CloneType sourceType = GetCloneType(lateSetupSource.GetType());
				ICloneSurrogate surrogate = sourceType.Surrogate;

				object lateSetupTarget;
				surrogate.LateSetup(lateSetupSource, out lateSetupTarget, this);
				this.SetTargetOf(lateSetupSource, lateSetupTarget);
			}
		}
		private void PrepareCloneGraph(object source, bool forceRoot, CloneType sourceType)
		{
			// Early-out for null values
			if (object.ReferenceEquals(source, null)) return;
			
			// Determine the object Type and early-out if it's just plain old data
			if (sourceType == null) sourceType = GetCloneType(source.GetType());
			if (sourceType.IsPlainOldData) return;

			// Has a target object already been registered for this source? If this is the case, stop.
			object target;
			bool rootTargetSkip = false;
			if (this.objTargets.TryGetValue(source, out target))
			{
				// Because of CopyTo, where the target root is already specified, but all else is not, we'll need to handle this as a special case.
				if (forceRoot)
					rootTargetSkip = true;
				else
					return;
			}

			this.currentObject = source;
			this.currentCloneType = sourceType;

			// Fetch the currently active clone behavior and react accordingly
			object behaviorLock = null;
			if (!object.ReferenceEquals(source, this.sourceRoot))
			{
				CloneBehavior behavior = this.GetCloneBehavior(sourceType, true, out behaviorLock);
				if (behavior != CloneBehavior.ChildObject)
				{
					if (behavior == CloneBehavior.WeakReference)
					{
						this.dropWeakReferences.Add(source);
					}
					this.UnlockCloneBehavior(behaviorLock);
					return;
				}
			}

			// Check whether there is a surrogate for this object
			if (sourceType.Surrogate != null)
			{
				bool requireLateSetup;
				sourceType.Surrogate.SetupCloneTargets(source, rootTargetSkip, out requireLateSetup, this);
				if (requireLateSetup)
				{
					this.lateSetupObjects.Add(source);
				}
			}
			// Otherwise, use the default algorithm
			else
			{
				// Create target objects
				if (!rootTargetSkip)
				{
					CloneType sourceElementType = null;
					if (sourceType.IsArray)
					{
						Array sourceArray = source as Array;
						sourceElementType = sourceType.ElementType;
						target = Array.CreateInstance(sourceElementType.Type, sourceArray.Length);
					}
					else
					{
						target = sourceType.Type.CreateInstanceOf();
					}
					this.SetTargetOf(source, target);
				}

				// If it implements custom cloning behavior, use that
				ICloneExplicit customSource;
				if ((customSource = source as ICloneExplicit) != null)
				{
					customSource.SetupCloneTargets(this);
				}
				// Otherwise, traverse its child objects using default behavior
				else
				{
					this.PrepareChildCloneGraph(source, sourceType);
				}
			}

			this.UnlockCloneBehavior(behaviorLock);
		}
		private void PrepareChildCloneGraph(object source, CloneType sourceType)
		{
			// If it's an array, we'll need to traverse its elements
			if (sourceType.IsArray)
			{
				Array sourceArray = source as Array;
				if (!sourceType.ElementType.IsPlainOldData)
				{
					for (int i = 0; i < sourceArray.Length; i++)
					{
						this.PrepareCloneGraph(sourceArray.GetValue(i), false, sourceType.ElementType.CouldBeDerived ? null : sourceType.ElementType);
					}
				}
			}
			// If it's an object, we'll need to traverse its fields
			else
			{
				for (int i = 0; i < sourceType.FieldData.Length; i++)
				{
					// Don't need to scan "plain old data" fields
					if (sourceType.FieldData[i].IsPlainOldData) continue;

					// See if there are specific instructions on how to handle this
					CloneBehaviorAttribute behavior = sourceType.FieldData[i].Behavior;
					if (behavior != null) this.PushCloneBehavior(behavior);
					{
						// Handle the fields value
						this.PrepareCloneGraph(sourceType.FieldData[i].Field.GetValue(source), false, null);
					}
					if (behavior != null) this.PopCloneBehavior();
				}
			}
		}

		private void PerformCopyObject(object source, object target, CloneType sourceType)
		{
			// Early-out for null and same-instance values
			if (object.ReferenceEquals(source, null)) return;
			if (object.ReferenceEquals(source, target)) return;

			// If we already handled this object, back out to avoid loops.
			if (sourceType == null) sourceType = GetCloneType(source.GetType());
			if (!sourceType.Type.IsValueType && !this.HandleObject(source)) return;

			this.currentObject = source;
			this.currentCloneType = sourceType;
			
			// Check whether there is a surrogare for this object
			ICloneExplicit customSource;
			if (sourceType.Surrogate != null)
			{
				sourceType.Surrogate.CopyDataTo(source, target, this);
			}
			// If it implements custom cloning behavior, use that
			else if ((customSource = source as ICloneExplicit) != null)
			{
				customSource.CopyDataTo(target, this);
			}
			// Otherwise, traverse its child objects using default behavior
			else
			{
				this.PerformCopyChildObject(source, target, sourceType);
			}
		}
		private void PerformCopyChildObject(object source, object target, CloneType sourceType)
		{
			// Plain old (struct) data can be deep-copied by assignment. Nothing to do here.
			if (sourceType.IsPlainOldData)
			{
				return;
			}
			// Arrays will need to be traversed, unless consisting of plain old data
			else if (sourceType.IsArray)
			{
				Array sourceArray = source as Array;
				Array targetArray = target as Array;
				CloneType sourceElementType = sourceType.ElementType;

				if (!sourceElementType.IsPlainOldData)
				{
					for (int i = 0; i < sourceArray.Length; ++i)
					{
						object sourceElement = sourceArray.GetValue(i);
						object targetElement;
						if (this.GetTargetOf(sourceElement, out targetElement))
						{
							this.PerformCopyObject(sourceElement, targetElement, sourceElementType.CouldBeDerived ? null : sourceElementType);
							targetArray.SetValue(targetElement, i);
						}
					}
				}
				else
				{
					sourceArray.CopyTo(targetArray, 0);
				}
			}
			// Objects will need to be traversed field by field
			else
			{
				// When available, take the shortcut for assigning all POD fields
				bool shortcutActive = false;
				if (sourceType.AssignPlainOldDataFunc != null)
				{
					shortcutActive = true;
					sourceType.AssignPlainOldDataFunc(source, target);
				}
				for (int i = 0; i < sourceType.FieldData.Length; i++)
				{
					// Skip certain fields when requested
					if ((sourceType.FieldData[i].Flags & CloneFieldFlags.IdentityRelevant) != CloneFieldFlags.None && this.context.PreserveIdentity)
						continue;
					// Skip fields that were assigned using the above shortcut method
					if (shortcutActive && sourceType.FieldData[i].AllowPlainOldDataShortcut)
						continue;

					// Actually copy the current field
					this.PerformCopyField(source, target, sourceType.FieldData[i].Field, sourceType.FieldData[i].IsPlainOldData);
				}
			}
		}
		private void PerformCopyField(object source, object target, FieldInfo field, bool isPlainOldData)
		{
			// Perform the quick version for plain old data
			if (isPlainOldData)
			{
				field.SetValue(target, field.GetValue(source));
			}
			// Perform the "always-correct" version for all else
			else
			{
				object sourceFieldValue = field.GetValue(source);
				object targetFieldValue;
				if (this.GetTargetOf(sourceFieldValue, out targetFieldValue))
				{
					this.PerformCopyObject(sourceFieldValue, targetFieldValue, null);
					field.SetValue(target, targetFieldValue);
				}
			}
		}

		private void PushCloneBehavior(CloneBehaviorAttribute attribute)
		{
			this.localBehavior.Add(new CloneBehaviorEntry(attribute));
		}
		private void PopCloneBehavior()
		{
			this.localBehavior.RemoveAt(this.localBehavior.Count - 1);
		}
		private CloneBehavior GetCloneBehavior(CloneType sourceType, bool lockBehavior, out object acquiredLock)
		{
			// Local behavior rules
			acquiredLock = null;
			var localBehaviorData = this.localBehavior.Data;
			for (int i = this.localBehavior.Count - 1; i >= 0; i--)
			{
				if (localBehaviorData[i].Locked) continue;
				if (localBehaviorData[i].Behavior.TargetType == null || (sourceType != null && localBehaviorData[i].Behavior.TargetType.IsAssignableFrom(sourceType.Type)))
				{
					acquiredLock = localBehaviorData[i].Behavior;
					localBehaviorData[i].Locked = lockBehavior;
					return localBehaviorData[i].Behavior.Behavior;
				}
			}

			// Global behavior rules
			return (sourceType != null) ? sourceType.DefaultCloneBehavior : CloneBehavior.ChildObject;
		}
		private void UnlockCloneBehavior(object behaviorLock)
		{
			if (behaviorLock == null) return;

			var localBehaviorData = this.localBehavior.Data;
			for (int i = this.localBehavior.Count - 1; i >= 0; i--)
			{
				if (localBehaviorData[i].Locked && localBehaviorData[i].Behavior == behaviorLock)
				{
					localBehaviorData[i].Locked = false;
				}
			}
		}
		
		bool ICloneTargetSetup.AddTarget<T>(T source, T target)
		{
			this.SetTargetOf(source, target);
			return true;
		}
		void ICloneTargetSetup.MakeWeakReference(object source)
		{
			this.dropWeakReferences.Add(source);
		}
		void ICloneTargetSetup.AutoHandleObject(object source)
		{
			if (object.ReferenceEquals(source, null)) return;
			if (source == this.currentObject)
				this.PrepareChildCloneGraph(source, this.currentCloneType);
			else
				this.PrepareCloneGraph(source, false, null);
		}

		bool ICloneOperation.GetTarget<T>(T source, out T target)
		{
			object targetObj;
			if (!this.GetTargetOf(source, out targetObj))
			{
				target = default(T);
				return false;
			}
			else
			{
				target = (T)targetObj;
				return true;
			}
		}
		void ICloneOperation.AutoHandleObject(object source, object target)
		{
			if (object.ReferenceEquals(source, null)) return;
			if (source == this.currentObject)
				this.PerformCopyChildObject(source, target, this.currentCloneType);
			else
				this.PerformCopyObject(source, target, null);
		}
		void ICloneOperation.AutoHandleField(FieldInfo field, object source, object target)
		{
			this.PerformCopyField(source, target, field, false);
		}


		private	static List<ICloneSurrogate>					surrogates			= null;
		private	static Dictionary<Type,CloneType>				cloneTypeCache		= new Dictionary<Type,CloneType>();
		private	static Dictionary<Type,CloneBehaviorAttribute>	cloneBehaviorCache	= new Dictionary<Type,CloneBehaviorAttribute>();
		private static CloneBehaviorAttribute[]					globalCloneBehavior = null;

		/// <summary>
		/// Returns the <see cref="CloneType"/> of a Type.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		protected internal static CloneType GetCloneType(Type type)
		{
			if (type == null) return null;

			CloneType result;
			if (cloneTypeCache.TryGetValue(type, out result)) return result;

			result = new CloneType(type);
			cloneTypeCache[type] = result;
			return result;
		}
		internal static ICloneSurrogate GetSurrogateFor(Type type)
		{
			if (surrogates == null)
			{
				surrogates = 
					DualityApp.GetAvailDualityTypes(typeof(ICloneSurrogate))
					.Select(t => t.CreateInstanceOf())
					.OfType<ICloneSurrogate>()
					.NotNull()
					.ToList();
				surrogates.StableSort((s1, s2) => s1.Priority - s2.Priority);
			}
			for (int i = 0; i < surrogates.Count; i++)
			{
				if (surrogates[i].MatchesType(type))
					return surrogates[i];
			}
			return null;
		}
		internal static CloneBehaviorAttribute GetCloneBehaviorAttribute(Type type)
		{
			// Assembly-level attributes pointing to this Type
			if (globalCloneBehavior == null)
			{
				globalCloneBehavior = ReflectionHelper.GetCustomAssemblyAttributes<CloneBehaviorAttribute>().ToArray();
			}
			for (int i = 0; i < globalCloneBehavior.Length; i++)
			{
				CloneBehaviorAttribute globalAttrib = globalCloneBehavior[i];
				if (globalAttrib.TargetType.IsAssignableFrom(type))
					return globalAttrib;
			}

			// Attributes attached directly to this Type
			CloneBehaviorAttribute directAttrib;
			if (!cloneBehaviorCache.TryGetValue(type, out directAttrib))
			{
				directAttrib = type.GetCustomAttributes<CloneBehaviorAttribute>().FirstOrDefault();
				cloneBehaviorCache[type] = directAttrib;
			}
			return directAttrib;
		}

		internal static void ClearTypeCache()
		{
			surrogates = null;
			globalCloneBehavior = null;
		}
	}

	public static class ExtMethodsCloning
	{
		public static T DeepClone<T>(this T baseObj, CloneProviderContext context = null)
		{
			CloneProvider provider = new CloneProvider(context);
			return (T)provider.CloneObject(baseObj);
		}
		public static void DeepCopyTo<T>(this T baseObj, T targetObj, CloneProviderContext context = null)
		{
			CloneProvider provider = new CloneProvider(context);
			provider.CopyObject(baseObj, targetObj);
		}
	}
}
