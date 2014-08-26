using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Duality.Cloning.Surrogates;

namespace Duality.Cloning
{
	public class CloneProvider
	{
		private struct LocalBehaviorLock
		{
			public int Index;
			public CloneBehaviorAttribute LockedAttribute;
		}
		private struct LocalCloneBehavior : ICloneBehavior
		{
			public static readonly LocalCloneBehavior Default = new LocalCloneBehavior { Mode = CloneMode.ChildObject, Flags = CloneFlags.None };

			public CloneMode Mode;
			public CloneFlags Flags;

			CloneMode ICloneBehavior.Mode
			{
				get { return this.Mode; }
			}
			CloneFlags ICloneBehavior.Flags
			{
				get { return this.Flags; }
			}
		}

		private	CloneProviderContext			context				= CloneProviderContext.Default;

		private	object							sourceRoot			= null;
		private	Dictionary<object,object>		objTargets			= new Dictionary<object,object>();
		private	HashSet<object>					handledObjects		= new HashSet<object>();
		private	HashSet<object>					dropWeakReferences	= new HashSet<object>();
		private	List<CloneBehaviorAttribute>	localBehavior		= new List<CloneBehaviorAttribute>();
		private	List<LocalBehaviorLock>			localBehaviorLocks	= new List<LocalBehaviorLock>();
		

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
			this.SetTargetOf(source, target);
			this.PrepareCloneGraph();
			this.GetTargetOf(source, out target);
			return target;
		}
		private void EndCloneOperation()
		{
			this.sourceRoot = null;
			this.objTargets.Clear();
			this.localBehavior.Clear();
			this.localBehaviorLocks.Clear();
			this.handledObjects.Clear();
			this.dropWeakReferences.Clear();
		}

		private void SetTargetOf(object source, object target)
		{
			if (object.ReferenceEquals(source, null)) return;
			if (object.ReferenceEquals(target, null)) return;
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

			// Determine which weak references to keep
			if (this.dropWeakReferences.Count > 0)
			{
				foreach (object source in this.objTargets.Keys)
				{
					this.dropWeakReferences.Remove(source);
					if (this.dropWeakReferences.Count == 0) break;
				}
			}

			this.localBehavior.Clear();
			this.localBehaviorLocks.Clear();
		}
		private void PrepareCloneGraph(object source)
		{
			// Early-out for null values
			if (object.ReferenceEquals(source, null)) return;
			
			// Determine the object Type and early-out if it's just plain old data
			Type sourceType = source.GetType();
			if (this.CanCloneByAssignment(sourceType)) return;

			// Has a target object already been registered for this source? If this is the case, stop.
			object target;
			if (this.objTargets.TryGetValue(source, out target))
				return;

			// Fetch the currently active clone behavior and react accordingly
			if (!object.ReferenceEquals(source, this.sourceRoot))
			{
				ICloneBehavior behavior = this.LockCloneBehavior(sourceType);
				if (behavior.Mode != CloneMode.ChildObject)
				{
					if (behavior.Mode == CloneMode.WeakReference)
					{
						this.dropWeakReferences.Add(source);
					}
					this.UnlockCloneBehavior();
					return;
				}
			}

			// If it's an array, we'll need to traverse its elements
			if (sourceType.IsArray)
			{
				Array sourceArray = source as Array;
				Type sourceElementType = sourceType.GetElementType();

				target = Array.CreateInstance(sourceElementType, sourceArray.Length);
				this.SetTargetOf(source, target);

				// Traverse the arrays elements
				if (!this.CanCloneByAssignment(sourceElementType))
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
				target = sourceType.CreateInstanceOf() ?? sourceType.CreateInstanceOf(true);
				this.SetTargetOf(source, target);

				// Traverse the objects fields
				IEnumerable<FieldInfo> fields = sourceType.GetAllFields(ReflectionHelper.BindInstanceAll);
				foreach (FieldInfo field in fields)
				{
					// See if there are specific instructions on how to handle this
					CloneBehaviorAttribute behaviorAttrib = field.GetCustomAttributes<CloneBehaviorAttribute>().FirstOrDefault();
					this.PushCloneBehavior(behaviorAttrib);
					{
						// Handle the fields value
						this.PrepareCloneGraph(field.GetValue(source));
					}
					this.PopCloneBehavior(behaviorAttrib);
				}
			}

			this.UnlockCloneBehavior();
		}
		private void PerformCopyObject(object source, object target)
		{
			// Early-out for null and same-instance values
			if (object.ReferenceEquals(source, null)) return;
			if (object.ReferenceEquals(source, target)) return;

			// Determine the object Type in order to decide what's next
			Type sourceType = source.GetType();

			// Plain old (struct) data can be deep-copied by assignment
			if (this.CanCloneByAssignment(sourceType))
			{
				target = source;
			}
			// Arrays will need to be traversed, unless consisting of plain old data
			else if (sourceType.IsArray)
			{
				if (!this.HandleObject(source)) return;

				Array sourceArray = source as Array;
				Array targetArray = target as Array;
				Type sourceElementType = sourceType.GetElementType();

				if (!this.CanCloneByAssignment(sourceElementType))
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
				if (!this.HandleObject(source)) return;

				IEnumerable<FieldInfo> fields = sourceType.GetAllFields(ReflectionHelper.BindInstanceAll);
				foreach (FieldInfo field in fields)
				{
					this.PerformCopyField(source, target, field);
				}
			}
		}
		private void PerformCopyField(object source, object target, FieldInfo field)
		{
			// See if there are specific instructions on how to handle this
			CloneBehaviorAttribute behaviorAttrib = field.GetCustomAttributes<CloneBehaviorAttribute>().FirstOrDefault();
			this.PushCloneBehavior(behaviorAttrib);
			{
				object sourceFieldValue = field.GetValue(source);
				object targetFieldValue;
				if (this.GetTargetOf(sourceFieldValue, out targetFieldValue))
				{
					this.PerformCopyObject(sourceFieldValue, targetFieldValue);
					field.SetValue(target, targetFieldValue);
				}
			}
			this.PopCloneBehavior(behaviorAttrib);
		}

		private void PushCloneBehavior(CloneBehaviorAttribute localBehavior)
		{
			if (localBehavior == null) return;
			this.localBehavior.Add(localBehavior);
		}
		private void PopCloneBehavior(CloneBehaviorAttribute localBehavior)
		{
			if (localBehavior == null) return;
			this.localBehavior.Remove(localBehavior);
		}
		private ICloneBehavior LockCloneBehavior(Type sourceType)
		{
			// Local behavior rules
			CloneBehaviorAttribute behaviorAttribute = null;
			for (int i = this.localBehavior.Count - 1; i >= 0; i--)
			{
				if (this.localBehavior[i].TargetType == null || this.localBehavior[i].TargetType.IsAssignableFrom(sourceType))
				{
					behaviorAttribute = this.localBehavior[i];
					this.localBehaviorLocks.Add(new LocalBehaviorLock
					{
						Index = i,
						LockedAttribute = behaviorAttribute
					});
					this.localBehavior.RemoveAt(i);
					break;
				}
			}

			// Global behavior rules
			if (behaviorAttribute == null)
			{
				behaviorAttribute = GetCloneBehaviorAttribute(sourceType);
			}

			// Results
			return behaviorAttribute != null ? 
				new LocalCloneBehavior 
				{
					Mode = behaviorAttribute.Mode, 
					Flags = behaviorAttribute.Flags
				} : 
				LocalCloneBehavior.Default;
		}
		private void UnlockCloneBehavior()
		{
			if (this.localBehaviorLocks.Count == 0) return;

			LocalBehaviorLock behaviorLock = this.localBehaviorLocks[this.localBehaviorLocks.Count - 1];
			if (behaviorLock.Index < this.localBehavior.Count)
				this.localBehavior.Insert(behaviorLock.Index, behaviorLock.LockedAttribute);
			else
				this.localBehavior.Add(behaviorLock.LockedAttribute);

			this.localBehaviorLocks.RemoveAt(this.localBehaviorLocks.Count - 1);
		}

		private bool CanCloneByAssignment(Type type)
		{
			return type.IsPlainOldData();
		}


		private	static List<ICloneSurrogate> surrogates = null;
		private static CloneBehaviorAttribute[] globalCloneBehavior = null;

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

		public static T DeepClone<T>(T baseObj, CloneProviderContext context = null)
		{
			CloneProvider provider = new CloneProvider(context);
			return (T)provider.CloneObject(baseObj);
		}
		public static void DeepCopy<T>(T baseObj, T targetObj, CloneProviderContext context = null)
		{
			CloneProvider provider = new CloneProvider(context);
			provider.CopyObject(baseObj, targetObj);
		}

		internal static void PerformReflectionFallback<T>(string copyMethodName, T baseObj, T targetObj, CloneProvider provider)
		{
			if (copyMethodName == null) throw new ArgumentNullException("copyMethodName");
			if (baseObj == null) throw new ArgumentNullException("baseObj");
			if (targetObj == null) throw new ArgumentNullException("targetObj");

			// Travel up the inheritance hierarchy
			// Don't fallback for types from the Duality Assembly. Those are required to do explicit copying.
			Type curType = baseObj.GetType();
			while (curType.Assembly != typeof(DualityApp).Assembly && typeof(T).IsAssignableFrom(curType))
			{
				// Retrieve OnCopyTo method, if declared in this specific class (local override)
				MethodInfo localOnCopyTo = curType.GetMethod(
					copyMethodName, 
					BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly, 
					null, 
					new[] { typeof(T), typeof(CloneProvider) }, 
					null);
				// Apply default behaviour to any class that doesn't have its own OnCopyTo override
				if (localOnCopyTo == null)
				{
					//provider.CopyObject(
					//    baseObj, 
					//    targetObj, 
					//    curType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));
				}

				curType = curType.BaseType;
			}
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
			return CloneProvider.DeepClone<T>(baseObj, context);
		}
		public static void DeepCopyTo<T>(this T baseObj, T targetObj, CloneProviderContext context = null)
		{
			CloneProvider.DeepCopy<T>(baseObj, targetObj, context);
		}
	}
}
