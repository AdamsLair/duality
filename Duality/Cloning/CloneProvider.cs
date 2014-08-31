using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Duality.Cloning.Surrogates;

namespace Duality.Cloning
{
	public class CloneProvider : ICloneTargetSetup, ICloneOperation
	{
		private static readonly CloneBehaviorAttribute DefaultBehavior = new CloneBehaviorAttribute(CloneBehavior.ChildObject);
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
				this.PerformCopyObject(source, target);
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
				this.PerformCopyObject(source, target);
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
			if (this.handledObjects.Contains(source)) return false;
			this.handledObjects.Add(source);
			return true;
		}

		private void PrepareCloneGraph()
		{
			// Visit the object graph in order to determine which objects to clone
			this.PrepareCloneGraph(this.sourceRoot);
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
				ICloneSurrogate surrogate = GetSurrogateFor(sourceType.Type);

				object lateSetupTarget;
				surrogate.LateSetup(lateSetupSource, out lateSetupTarget, this);
				this.SetTargetOf(lateSetupSource, lateSetupTarget);
			}
		}
		private void PrepareCloneGraph(object source)
		{
			// Early-out for null values
			if (object.ReferenceEquals(source, null)) return;
			
			// Determine the object Type and early-out if it's just plain old data
			CloneType sourceType = GetCloneType(source.GetType());
			if (sourceType.IsPlainOldData) return;

			// Has a target object already been registered for this source? If this is the case, stop.
			object target;
			if (this.objTargets.TryGetValue(source, out target))
				return;
			this.currentObject = source;

			// Fetch the currently active clone behavior and react accordingly
			CloneBehaviorAttribute behavior = null;
			if (!object.ReferenceEquals(source, this.sourceRoot))
			{
				behavior = this.GetCloneBehavior(sourceType.Type, true);
				if (behavior.Behavior != CloneBehavior.ChildObject)
				{
					if (behavior.Behavior == CloneBehavior.WeakReference)
					{
						this.dropWeakReferences.Add(source);
					}
					this.UnlockCloneBehavior(behavior);
					return;
				}
			}

			// Check whether there is a surrogare for this object
			ICloneSurrogate surrogate = GetSurrogateFor(sourceType.Type);
			if (surrogate != null)
			{
				bool requireLateSetup;
				surrogate.SetupCloneTargets(source, out requireLateSetup, this);
				if (requireLateSetup)
				{
					this.lateSetupObjects.Add(source);
				}
			}
			// Otherwise, use the default algorithm
			else
			{
				// Create target objects
				Type sourceElementType = null;
				if (sourceType.Type.IsArray)
				{
					Array sourceArray = source as Array;
					sourceElementType = sourceType.Type.GetElementType();
					target = Array.CreateInstance(sourceElementType, sourceArray.Length);
				}
				else
				{
					target = sourceType.Type.CreateInstanceOf();
				}
				this.SetTargetOf(source, target);

				// If it implements custom cloning behavior, use that
				if (source is ICloneExplicit)
				{
					ICloneExplicit customSource = source as ICloneExplicit;
					customSource.SetupCloneTargets(this);
				}
				// Otherwise, traverse its child objects using default behavior
				else
				{
					this.PrepareChildCloneGraph(source, sourceType);
				}
			}

			this.UnlockCloneBehavior(behavior);
		}
		private void PrepareChildCloneGraph(object source, CloneType sourceType)
		{
			// If it's an array, we'll need to traverse its elements
			if (sourceType.Type.IsArray)
			{
				Type sourceElementType = sourceType.Type.GetElementType();
				Array sourceArray = source as Array;
				if (!sourceElementType.IsPlainOldData())
				{
					for (int i = 0; i < sourceArray.Length; i++)
					{
						this.PrepareCloneGraph(sourceArray.GetValue(i));
					}
				}
			}
			// If it's an object, we'll need to traverse its fields
			else
			{
				for (int i = 0; i < sourceType.FieldData.Length; i++)
				{
					FieldInfo field = sourceType.FieldData[i].Field;
					CloneBehaviorAttribute behavior = sourceType.FieldData[i].Behavior;

					// See if there are specific instructions on how to handle this
					if (behavior != null) this.PushCloneBehavior(behavior);
					{
						// Handle the fields value
						this.PrepareCloneGraph(field.GetValue(source));
					}
					if (behavior != null) this.PopCloneBehavior();
				}
			}
		}

		private void PerformCopyObject(object source, object target)
		{
			// Early-out for null and same-instance values
			if (object.ReferenceEquals(source, null)) return;
			if (object.ReferenceEquals(source, target)) return;

			// Determine the object Type in order to decide what's next
			this.currentObject = source;
			CloneType sourceType = GetCloneType(source.GetType());
			if (!sourceType.Type.IsValueType && !this.HandleObject(source)) return;
			
			// Check whether there is a surrogare for this object
			ICloneSurrogate surrogate = GetSurrogateFor(sourceType.Type);
			if (surrogate != null)
			{
				surrogate.CopyDataTo(source, target, this);
			}
			// If it implements custom cloning behavior, use that
			else if (source is ICloneExplicit)
			{
				ICloneExplicit customSource = source as ICloneExplicit;
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
			else if (sourceType.Type.IsArray)
			{
				Array sourceArray = source as Array;
				Array targetArray = target as Array;
				Type sourceElementType = sourceType.Type.GetElementType();

				if (!sourceElementType.IsPlainOldData())
				{
					for (int i = 0; i < sourceArray.Length; ++i)
					{
						object sourceElement = sourceArray.GetValue(i);
						object targetElement;
						if (this.GetTargetOf(sourceElement, out targetElement))
						{
							this.PerformCopyObject(sourceElement, targetElement);
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
				for (int i = 0; i < sourceType.FieldData.Length; i++)
				{
					FieldInfo field = sourceType.FieldData[i].Field;

					// Skip certain fields when requested
					if ((sourceType.FieldData[i].Flags & CloneFieldFlags.IdentityRelevant) != CloneFieldFlags.None && this.context.PreserveIdentity)
						continue;

					// Actually copy the current field
					this.PerformCopyField(source, target, field);
				}
			}
		}
		private void PerformCopyField(object source, object target, FieldInfo field)
		{
			object sourceFieldValue = field.GetValue(source);
			object targetFieldValue;
			if (this.GetTargetOf(sourceFieldValue, out targetFieldValue))
			{
				this.PerformCopyObject(sourceFieldValue, targetFieldValue);
				field.SetValue(target, targetFieldValue);
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
		private CloneBehaviorAttribute GetCloneBehavior(Type sourceType, bool lockBehavior)
		{
			// Local behavior rules
			CloneBehaviorAttribute behavior = null;
			var localBehaviorData = this.localBehavior.Data;
			for (int i = this.localBehavior.Count - 1; i >= 0; i--)
			{
				if (localBehaviorData[i].Locked) continue;
				if (localBehaviorData[i].Behavior.TargetType == null || (sourceType != null && localBehaviorData[i].Behavior.TargetType.IsAssignableFrom(sourceType)))
				{
					behavior = localBehaviorData[i].Behavior;
					localBehaviorData[i].Locked = lockBehavior;
					break;
				}
			}

			// Global behavior rules
			if (behavior == null && sourceType != null)
			{
				behavior = GetCloneBehaviorAttribute(sourceType);
			}

			// Results
			return behavior ?? DefaultBehavior;
		}
		private void UnlockCloneBehavior(CloneBehaviorAttribute behavior)
		{
			if (behavior == null) return;

			var localBehaviorData = this.localBehavior.Data;
			for (int i = this.localBehavior.Count - 1; i >= 0; i--)
			{
				if (localBehaviorData[i].Locked && localBehaviorData[i].Behavior == behavior)
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
				this.PrepareChildCloneGraph(source, GetCloneType(source.GetType()));
			else
				this.PrepareCloneGraph(source);
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
				this.PerformCopyChildObject(source, target, GetCloneType(source.GetType()));
			else
				this.PerformCopyObject(source, target);
		}
		void ICloneOperation.AutoHandleField(FieldInfo field, object source, object target)
		{
			this.PerformCopyField(source, target, field);
		}


		private	static List<ICloneSurrogate>		surrogates			= null;
		private	static	Dictionary<Type,CloneType>	cloneTypeCache		= new Dictionary<Type,CloneType>();
		private static CloneBehaviorAttribute[]		globalCloneBehavior = null;
		
		/// <summary>
		/// Returns the <see cref="CloneType"/> of a Type.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		protected static CloneType GetCloneType(Type type)
		{
			if (type == null) return null;

			CloneType result;
			if (cloneTypeCache.TryGetValue(type, out result)) return result;

			result = new CloneType(type);
			cloneTypeCache[type] = result;
			return result;
		}
		private static ICloneSurrogate GetSurrogateFor(Type type)
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
			return surrogates.FirstOrDefault(s => s.MatchesType(type));
		}
		private static CloneBehaviorAttribute GetCloneBehaviorAttribute(Type type)
		{
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
			CloneBehaviorAttribute directAttrib = type.GetCustomAttributes<CloneBehaviorAttribute>().FirstOrDefault();
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
