﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

using Duality.Cloning.Surrogates;

namespace Duality.Cloning
{
	public class CloneProvider : ICloneTargetSetup, ICloneOperation
	{
		/// <summary>
		/// Compares two objects for equality strictly by reference. This is needed to build
		/// the object id mapping, since some objects may expose some unfortunate equality behavior,
		/// and we really want to distinguish different objects by reference, and not by "content" here.
		/// </summary>
		private class ReferenceEqualityComparer : IEqualityComparer<object>
		{
			bool IEqualityComparer<object>.Equals(object x, object y)
			{
				return object.ReferenceEquals(x, y);
			}
			int IEqualityComparer<object>.GetHashCode(object obj)
			{
				return !object.ReferenceEquals(obj, null) ? System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(obj) : 0;
			}
		}
		private struct LateSetupEntry
		{
			public object Source;
			public object Target;

			public LateSetupEntry(object source, object target)
			{
				this.Source = source;
				this.Target = target;
			}
			public override bool Equals(object obj)
			{
				if (!(obj is LateSetupEntry)) return false;
				LateSetupEntry other = (LateSetupEntry)obj;
				return
					object.ReferenceEquals(other.Source, this.Source) &&
					object.ReferenceEquals(other.Target, this.Target);
			}
			public override int GetHashCode()
			{
				int hash = 0;
				MathF.CombineHashCode(ref hash, !object.ReferenceEquals(this.Source, null) ? System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(this.Source) : 0);
				MathF.CombineHashCode(ref hash, !object.ReferenceEquals(this.Target, null) ? System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(this.Target) : 0);
				return hash;
			}
		}
		private class LocalCloneBehavior
		{
			private TypeInfo		targetType;
			private CloneBehavior	behavior;
			private bool			locked;

			public TypeInfo TargetType
			{
				get { return this.targetType; }
			}
			public CloneBehavior Behavior
			{
				get { return this.behavior; }
			}
			public bool Locked
			{
				get { return this.locked; }
				set { this.locked = value; }
			}

			public LocalCloneBehavior(TypeInfo targetType, CloneBehavior behavior)
			{
				this.targetType = targetType;
				this.behavior = behavior;
			}
		}

		private	CloneProviderContext		context				= CloneProviderContext.Default;

		private	object						sourceRoot			= null;
		private	object						targetRoot			= null;
		private	object						currentObject		= null;
		private	CloneType					currentCloneType	= null;
		private	Dictionary<object,object>	targetMapping		= new Dictionary<object,object>(new ReferenceEqualityComparer());
		private	HashSet<object>				targetSet			= new HashSet<object>(new ReferenceEqualityComparer());
		private	HashSet<LateSetupEntry>		lateSetupSchedule	= new HashSet<LateSetupEntry>();
		private	HashSet<object>				handledObjects		= new HashSet<object>(new ReferenceEqualityComparer());
		private	RawList<LocalCloneBehavior>	localBehavior		= new RawList<LocalCloneBehavior>();
		

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

		/// <summary>
		/// Clones the specified object and returns the cloned instance.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source"></param>
		/// <param name="preserveCache">
		/// If true, the mapping between source and target object graph is preserved.
		/// 
		/// This can be useful for doing a partial clone operation that is later continued or
		/// repeated using the same <see cref="CloneProvider"/> instance. Since the mapping is
		/// already present, performance of subsequent clone operations within the same object
		/// graph can benefit by this.
		/// </param>
		public T CloneObject<T>(T source, bool preserveCache = false)
		{
			object target; // Don't use T, we'll need to make sure "target" is a reference Type
			try
			{
				target = this.BeginCloneOperation(source);
				this.PerformCopyObject(source, target, null);
			}
			finally
			{
				this.EndCloneOperation(preserveCache);
			}
			return (T)target;
		}
		/// <summary>
		/// Copies the specified source object graph to the specified target object
		/// graph. Where possible, existing objects will be preserved and updated,
		/// rather than being overwritten.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source"></param>
		/// <param name="target"></param>
		/// <param name="preserveCache">
		/// If true, the mapping between source and target object graph is preserved.
		/// 
		/// This can be useful for doing a partial clone operation that is later continued or
		/// repeated using the same <see cref="CloneProvider"/> instance. Since the mapping is
		/// already present, performance of subsequent clone operations within the same object
		/// graph can benefit by this.
		/// </param>
		public void CopyObject<T>(T source, T target, bool preserveCache = false)
		{
			try
			{
				this.BeginCloneOperation(source, target);
				this.PerformCopyObject(source, target, null);
			}
			finally
			{
				this.EndCloneOperation(preserveCache);
			}
		}
		/// <summary>
		/// Clears the clone providers internal mapping between source and target
		/// object graph. This is done automatically, unless the copy or clone
		/// operation has been performed with explicitly preserving the cache.
		/// </summary>
		public void ClearCachedMapping()
		{
			this.targetMapping.Clear();
			this.targetSet.Clear();
		}
		
		/// <summary>
		/// Prepares the clone operation by generating a mapping between source
		/// and target object graph, and creating the target objects where required in the
		/// process. 
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		/// <returns>Returns a reference to the target root object.</returns>
		private object BeginCloneOperation(object source, object target = null)
		{
			if (this.targetSet.Contains(source))
			{
				throw new InvalidOperationException("You may not use a CloneProvider for cloning its own clone results after preserving the internal cache.");
			}

			// Prepare the target object graph we'll copy the source graph's values over to
			this.sourceRoot = source;
			this.targetRoot = target;
			this.PrepareCloneGraph();

			// Get the target object which was either re-used or created in the preparation step
			target = this.GetTargetOf(source);
			this.targetRoot = target;
			return target;
		}
		/// <summary>
		/// Ends the current clone operation by clearing all the working data that was
		/// allocated in the process.
		/// </summary>
		/// <param name="preserveMapping">
		/// If true, the mapping between source and target object graph is preserved.
		/// 
		/// This can be useful for doing a partial clone operation that is later continued or
		/// repeated using the same <see cref="CloneProvider"/> instance. Since the mapping is
		/// already present, performance of subsequent clone operations within the same object
		/// graph can benefit by this.
		/// </param>
		private void EndCloneOperation(bool preserveMapping)
		{
			this.sourceRoot = null;
			this.currentObject = null;
			this.currentCloneType = null;

			this.localBehavior.Clear();
			this.lateSetupSchedule.Clear();
			this.handledObjects.Clear();

			if (!preserveMapping)
				this.ClearCachedMapping();
		}

		/// <summary>
		/// Registers a mapping from the specified source object to its
		/// target object graph equivalent. This is a one-to-one relation.
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		private void SetTargetOf(object source, object target)
		{
			if (object.ReferenceEquals(source, null)) return;
			if (this.targetSet.Add(target))
			{
				this.targetMapping[source] = target;
			}
		}
		/// <summary>
		/// Retrieves the target graph equivalent of the specified source graph object.
		/// 
		/// Note that the resulting target object will (expected to) be the
		/// same as the source object in cases where there is no mapping, but
		/// reference assignment instead.
		/// </summary>
		/// <param name="source"></param>
		private object GetTargetOf(object source)
		{
			if (object.ReferenceEquals(source, null))
				return null;

			object target;
			if (this.targetMapping.TryGetValue(source, out target))
				return target;
			else
				return source;
		}
		/// <summary>
		/// Adds the specified source object to the handled object stack when
		/// required and returns whether the object need to be investigated as 
		/// part of the copy step at all.
		/// </summary>
		/// <param name="source"></param>
		/// <param name="typeData"></param>
		private bool PushCurrentObject(object source, CloneType typeData)
		{
			return typeData.Type.IsValueType || object.ReferenceEquals(source, null) || this.handledObjects.Add(source);
		}
		/// <summary>
		/// Removes the specified source object from the handled object stack
		/// after finishing its copy step.
		/// </summary>
		/// <param name="source"></param>
		/// <param name="typeData"></param>
		private bool PopCurrentObject(object source, CloneType typeData)
		{
			return typeData.Type.IsValueType || object.ReferenceEquals(source, null) || this.handledObjects.Remove(source);
		}

		private void PrepareCloneGraph()
		{
			// Visit the object graph in order to determine which objects to clone
			this.PrepareObjectCloneGraph(this.sourceRoot, this.targetRoot, null, CloneBehavior.ChildObject);
			this.localBehavior.Clear();

			// Perform late setup for surrogate objects that required it
			foreach (LateSetupEntry lateSetup in this.lateSetupSchedule)
			{
				CloneType typeData = GetCloneType((lateSetup.Source ?? lateSetup.Target).GetType());
				ICloneSurrogate surrogate = typeData.Surrogate;

				object lateSetupTarget = lateSetup.Target;
				surrogate.LateSetup(lateSetup.Source, ref lateSetupTarget, this);
				this.SetTargetOf(lateSetup.Source ?? lateSetup.Target, lateSetupTarget);
			}
		}

		private void PrepareObjectCloneGraph(object source, object target, CloneType typeData, CloneBehavior behavior = CloneBehavior.Default)
		{
			// Early-out for null values
			if (object.ReferenceEquals(source, null))
			{
				if (object.ReferenceEquals(target, null)) return;
				if (typeData == null) typeData = GetCloneType(target.GetType());
				if (!typeData.IsMergeSurrogate) return;
			}
			
			// Determine the object Type and early-out if it's just plain old data
			if (typeData == null) typeData = GetCloneType(source.GetType());
			if (typeData.IsCopyByAssignment) return;
			if (typeData.Type.IsValueType && !typeData.InvestigateOwnership) return;
			
			// Determine cloning behavior for this object
			object behaviorLock = null;
			if (!typeData.Type.IsValueType && !object.ReferenceEquals(source, null))
			{
				// If we already registered a target for that source, stop right here.
				if (this.targetMapping.ContainsKey(source))
					return;

				// If no specific behavior was specified, fetch the default one set by class and field attributes
				if (behavior == CloneBehavior.Default)
				{
					behavior = this.GetCloneBehavior(typeData, true, out behaviorLock);
				}
				// Apply the current behavior
				if (behavior != CloneBehavior.ChildObject)
				{
					this.UnlockCloneBehavior(behaviorLock);
					return;
				}

				// If the target doesn't match the source, discard it
				if (target != null && target.GetType() != typeData.Type.AsType())
					target = null;
			}

			object lastObject = this.currentObject;
			CloneType lastCloneType = this.currentCloneType;
			this.currentObject = source;
			this.currentCloneType = typeData;

			// If it's a value type, use the fast lane without surrogate and custom checks
			if (typeData.Type.IsValueType)
			{
				if (object.ReferenceEquals(target, null))
				{
					target = typeData.Type.CreateInstanceOf();
				}
				this.PrepareObjectChildCloneGraph(source, target, typeData);
			}
			// Check whether there is a surrogate for this object
			else if (typeData.Surrogate != null)
			{
				bool requireLateSetup;
				typeData.Surrogate.SetupCloneTargets(source, target, out requireLateSetup, this);
				if (requireLateSetup)
				{
					this.lateSetupSchedule.Add(new LateSetupEntry(source, target));
				}
			}
			// Otherwise, use the default algorithm
			else
			{
				// Create a new target array. Always necessary due to their immutable size.
				Array originalTargetArray = null;
				if (typeData.IsArray)
				{
					Array sourceArray = source as Array;
					originalTargetArray = target as Array;
					target = Array.CreateInstance(typeData.ElementType.Type.AsType(), sourceArray.Length);
				}
				// Only create target object when no reuse is possible
				else if (object.ReferenceEquals(target, null))
				{
					target = typeData.Type.CreateInstanceOf();
				}

				// Create a mapping from the source object to the target object
				this.SetTargetOf(source, target);
				
				// If we are dealing with an array, use the original one for object reuse mapping
				if (originalTargetArray != null) target = originalTargetArray;

				// If it implements custom cloning behavior, use that
				ICloneExplicit customSource = source as ICloneExplicit;
				if (customSource != null)
				{
					customSource.SetupCloneTargets(target, this);
				}
				// Otherwise, traverse its child objects using default behavior
				else
				{
					this.PrepareObjectChildCloneGraph(source, target, typeData);
				}
			}
			
			this.currentObject = lastObject;
			this.currentCloneType = lastCloneType;
			this.UnlockCloneBehavior(behaviorLock);
		}
		private void PrepareObjectChildCloneGraph(object source, object target, CloneType typeData)
		{
			// If the object is a simple and shallow type, there's nothing to investigate.
			if (!typeData.InvestigateOwnership) return;

			// If it's an array, we'll need to traverse its elements
			if (typeData.IsArray)
			{
				CloneType elementTypeData = typeData.ElementType.CouldBeDerived ? null : typeData.ElementType;
				Array sourceArray = source as Array;
				Array targetArray = target as Array;
				for (int i = 0; i < sourceArray.Length; i++)
				{
					object sourceElementValue = sourceArray.GetValue(i);
					object targetElementValue = targetArray.Length > i ? targetArray.GetValue(i) : null;
					this.PrepareObjectCloneGraph(
						sourceElementValue, 
						targetElementValue, 
						elementTypeData);
				}
			}
			// If it's an object, we'll need to traverse its fields
			else if (typeData.PrecompiledSetupFunc != null)
			{
				typeData.PrecompiledSetupFunc(source, target, this);
			}
		}

		private void PrepareValueCloneGraph<T>(ref T source, ref T target, CloneType typeData) where T : struct
		{
			// Determine the object Type and early-out if it's just plain old data
			if (typeData == null) typeData = GetCloneType(typeof(T));
			if (typeData.IsCopyByAssignment) return;
			if (!typeData.InvestigateOwnership) return;

			object lastObject = this.currentObject;
			CloneType lastCloneType = this.currentCloneType;
			this.currentObject = source;
			this.currentCloneType = typeData;

			this.PrepareValueChildCloneGraph<T>(ref source, ref target, typeData);
			
			this.currentObject = lastObject;
			this.currentCloneType = lastCloneType;
		}
		private void PrepareValueChildCloneGraph<T>(ref T source, ref T target, CloneType typeData) where T : struct
		{
			CloneType.ValueSetupFunc<T> typedSetupFunc = typeData.PrecompiledValueSetupFunc as CloneType.ValueSetupFunc<T>;
			if (typedSetupFunc != null)
			{
				typedSetupFunc(ref source, ref target, this);
			}
		}

		private void PerformCopyObject(object source, object target, CloneType typeData)
		{
			// Early-out for same-instance values
			if (object.ReferenceEquals(source, target)) return;

			// Early-out for null values
			if (object.ReferenceEquals(source, null))
			{
				if (typeData == null) typeData = GetCloneType(target.GetType());
				if (!typeData.IsMergeSurrogate) return;
			}

			// If we already handled this object, back out to avoid loops.
			if (typeData == null) typeData = GetCloneType(source.GetType());
			if (!this.PushCurrentObject(source, typeData)) return;
			
			object lastObject = this.currentObject;
			CloneType lastCloneType = this.currentCloneType;
			this.currentObject = source;
			this.currentCloneType = typeData;
			
			// Check whether there is a surrogare for this object
			ICloneExplicit customSource;
			if (typeData.Surrogate != null)
			{
				typeData.Surrogate.CopyDataTo(source, target, this);
			}
			// If it implements custom cloning behavior, use that
			else if ((customSource = source as ICloneExplicit) != null)
			{
				customSource.CopyDataTo(target, this);
			}
			// Otherwise, traverse its child objects using default behavior
			else if (!typeData.IsCopyByAssignment)
			{
				this.PerformCopyChildObject(source, target, typeData);
			}

			this.currentObject = lastObject;
			this.currentCloneType = lastCloneType;
			this.PopCurrentObject(source, typeData);
		}
		private void PerformCopyChildObject(object source, object target, CloneType typeData)
		{
			// Handle array data
			if (typeData.IsArray)
			{
				Array sourceArray = source as Array;
				Array targetArray = target as Array;
				CloneType sourceElementType = typeData.ElementType;

				// If the array contains plain old data, no further handling is required
				if (sourceElementType.IsCopyByAssignment)
				{
					sourceArray.CopyTo(targetArray, 0);
				}
				// If the array contains a value type, handle each element in order to allow them to perform a mapping
				else if (sourceElementType.Type.IsValueType)
				{
					for (int i = 0; i < sourceArray.Length; ++i)
					{
						object sourceElement = sourceArray.GetValue(i);
						object targetElement = targetArray.GetValue(i);
						this.PerformCopyObject(
							sourceElement, 
							targetElement, 
							sourceElementType);
						targetArray.SetValue(targetElement, i);
					}
				}
				// If it contains reference types, a direct element mapping is necessary, as well as complex value handling
				else
				{
					bool couldRequireMerge = sourceElementType.CouldBeDerived || sourceElementType.IsMergeSurrogate;
					for (int i = 0; i < sourceArray.Length; ++i)
					{
						CloneType elementTypeData = sourceElementType.CouldBeDerived ? null : sourceElementType;

						object sourceElement = sourceArray.GetValue(i);
						object targetElement;

						// If there is no source value, check if we're dealing with a merge surrogate and get the old target value when necessary.
						bool sourceNullMerge = false;
						if (couldRequireMerge && object.ReferenceEquals(sourceElement, null))
						{
							if (elementTypeData == null || elementTypeData.IsMergeSurrogate)
							{
								sourceElement = targetArray.GetValue(i);
								if (!object.ReferenceEquals(sourceElement, null))
								{
									if (elementTypeData == null) elementTypeData = GetCloneType(sourceElement.GetType());
									if (elementTypeData.IsMergeSurrogate)
										sourceNullMerge = true;
									else
										sourceElement = null;
								}
							}
						}

						// Perform target mapping and assign the copied value to the target field
						targetElement = this.GetTargetOf(sourceElement);
						this.PerformCopyObject(sourceNullMerge ? null : sourceElement, targetElement, elementTypeData);
						targetArray.SetValue(targetElement, i);
					}
				}
			}
			// Handle structural data
			else
			{
				// When available, take the shortcut for assigning all POD fields
				if (typeData.PrecompiledAssignmentFunc != null)
				{
					typeData.PrecompiledAssignmentFunc(source, target, this);
				}
				// Otherwise, fall back to reflection. This is currently necessary for value types.
				else
				{
					for (int i = 0; i < typeData.FieldData.Length; i++)
					{
						if ((typeData.FieldData[i].Flags & CloneFieldFlags.IdentityRelevant) != CloneFieldFlags.None && this.context.PreserveIdentity)
							continue;
						this.PerformCopyField(source, target, typeData.FieldData[i].Field, typeData.FieldData[i].FieldType.IsCopyByAssignment);
					}
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
			// If this field stores a value type, no assignment or mapping is necessary. Just handle the struct.
			else if (field.FieldType.GetTypeInfo().IsValueType)
			{
				object sourceFieldValue = field.GetValue(source);
				object targetFieldValue = field.GetValue(target);
				this.PerformCopyObject(
					sourceFieldValue, 
					targetFieldValue, 
					GetCloneType(field.FieldType));
				field.SetValue(target, targetFieldValue);
			}
			// If it's a reference type, the value needs to be mapped from source to target
			else
			{
				object sourceFieldValue = field.GetValue(source);
				object targetFieldValue;

				// If there is no source value, check if we're dealing with a merge surrogate and get the old target value when necessary.
				bool sourceNullMerge = false;
				CloneType typeData = null;
				if (object.ReferenceEquals(sourceFieldValue, null))
				{
					sourceFieldValue = field.GetValue(target);
					if (!object.ReferenceEquals(sourceFieldValue, null))
					{
						typeData = GetCloneType(sourceFieldValue.GetType());
						if (typeData.IsMergeSurrogate)
							sourceNullMerge = true;
						else
							sourceFieldValue = null;
					}
				}

				// Perform target mapping and assign the copied value to the target field
				targetFieldValue = this.GetTargetOf(sourceFieldValue);
				this.PerformCopyObject(sourceNullMerge ? null : sourceFieldValue, targetFieldValue, typeData);
				field.SetValue(target, targetFieldValue);
			}
		}
		
		private void PerformCopyValue<T>(ref T source, ref T target, CloneType typeData) where T : struct
		{
			if (typeData == null) typeData = GetCloneType(typeof(T));
			
			object lastObject = this.currentObject;
			CloneType lastCloneType = this.currentCloneType;
			this.currentObject = source;
			this.currentCloneType = typeData;
			
			if (typeData.IsCopyByAssignment)
			{
				target = source;
			}
			else
			{
				this.PerformCopyChildValue(ref source, ref target, typeData);
			}

			this.currentObject = lastObject;
			this.currentCloneType = lastCloneType;
		}
		private void PerformCopyChildValue<T>(ref T source, ref T target, CloneType typeData) where T : struct
		{
			CloneType.ValueAssignmentFunc<T> typedAssignmentFunc = typeData.PrecompiledValueAssignmentFunc as CloneType.ValueAssignmentFunc<T>;
			if (typedAssignmentFunc != null)
			{
				typedAssignmentFunc(ref source, ref target, this);
			}
		}

		private void PushCloneBehavior(LocalCloneBehavior behavior)
		{
			this.localBehavior.Add(behavior);
		}
		private void PopCloneBehavior()
		{
			this.localBehavior.RemoveAt(this.localBehavior.Count - 1);
		}
		private CloneBehavior GetCloneBehavior(CloneType sourceType, bool lockBehavior, out object acquiredLock)
		{
			CloneBehavior defaultBehavior = (sourceType != null) ? sourceType.DefaultCloneBehavior : CloneBehavior.ChildObject;

			// Local behavior rules
			acquiredLock = null;
			var localBehaviorData = this.localBehavior.Data;
			for (int i = this.localBehavior.Count - 1; i >= 0; i--)
			{
				if (localBehaviorData[i].Locked) continue;
				if (localBehaviorData[i].TargetType == null || (sourceType != null && localBehaviorData[i].TargetType.IsAssignableFrom(sourceType.Type)))
				{
					acquiredLock = localBehaviorData[i];
					localBehaviorData[i].Locked = lockBehavior;
					CloneBehavior behavior = localBehaviorData[i].Behavior;
					return (behavior != CloneBehavior.Default) ? behavior : defaultBehavior;
				}
			}

			// Global behavior rules
			return defaultBehavior;
		}
		private void UnlockCloneBehavior(object behaviorLock)
		{
			if (behaviorLock == null) return;

			var localBehaviorData = this.localBehavior.Data;
			for (int i = this.localBehavior.Count - 1; i >= 0; i--)
			{
				if (localBehaviorData[i].Locked && localBehaviorData[i] == behaviorLock)
				{
					localBehaviorData[i].Locked = false;
				}
			}
		}
		
		void ICloneTargetSetup.AddTarget<T>(T source, T target)
		{
			this.SetTargetOf(source, target);
		}
		void ICloneTargetSetup.HandleObject<T>(T source, T target, CloneBehavior behavior, TypeInfo behaviorTarget)
		{
			// Since "fallback to default" is triggered by source being equal to the currently handled object,
			// we need to make sure that this cannot be triggered accidentally by auto-generated clone lambdas.
			// Only allow the fallback when the object in question is actually cloned by user code!
			bool calledFromUserCode = this.currentObject is ICloneExplicit || this.currentCloneType.Surrogate != null;
			if (object.ReferenceEquals(source, this.currentObject) && calledFromUserCode)
			{
				this.PrepareObjectChildCloneGraph(this.currentObject, target, this.currentCloneType);
			}
			else if (behaviorTarget != null)
			{
				this.PushCloneBehavior(new LocalCloneBehavior(behaviorTarget, behavior));
				this.PrepareObjectCloneGraph(source, target, null);
				this.PopCloneBehavior();
			}
			else if (behavior == CloneBehavior.Reference)
			{
				return;
			}
			else
			{
				this.PrepareObjectCloneGraph(source, target, null, behavior);
			}
		}
		void ICloneTargetSetup.HandleValue<T>(ref T source, ref T target, CloneBehavior behavior, TypeInfo behaviorTarget)
		{
			if (typeof(T) == this.currentCloneType.Type.AsType())
			{
				// Structs can't contain themselfs. If source's type is equal to our current clone type, this is a handle-self call.
				this.PrepareValueChildCloneGraph<T>(ref source, ref target, this.currentCloneType);
			}
			else if (behaviorTarget != null)
			{
				this.PushCloneBehavior(new LocalCloneBehavior(behaviorTarget, behavior));
				this.PrepareValueCloneGraph<T>(ref source, ref target, null);
				this.PopCloneBehavior();
			}
			else
			{
				this.PrepareValueCloneGraph<T>(ref source, ref target, null);
			}
		}

		bool ICloneOperation.IsTarget<T>(T target)
		{
			return this.targetSet.Contains(target);
		}
		T ICloneOperation.GetTarget<T>(T source)
		{
			object targetObj = this.GetTargetOf(source);
			return (T)targetObj;
		}
		void ICloneOperation.HandleObject<T>(T source, ref T target)
		{
			// If we're just handling ourselfs, don't bother doing anything else.
			// Since "fallback to default" is triggered by source being equal to the currently handled object,
			// we need to make sure that this cannot be triggered accidentally by auto-generated clone lambdas.
			// Only allow the fallback when the object in question is actually cloned by user code!
			bool calledFromUserCode = this.currentObject is ICloneExplicit || this.currentCloneType.Surrogate != null;
			if (object.ReferenceEquals(source, this.currentObject) && calledFromUserCode)
			{
				if (!this.currentCloneType.IsCopyByAssignment)
				{
					this.PerformCopyChildObject(source, target, this.currentCloneType);
				}
				return;
			}

			// If there is no source value, check if we're dealing with a merge surrogate and get the old target value when necessary.
			bool sourceNullMerge = false;
			CloneType typeData = null;
			if (object.ReferenceEquals(source, null))
			{
				source = target;
				if (!object.ReferenceEquals(source, null))
				{
					typeData = GetCloneType(source.GetType());
					if (typeData.IsMergeSurrogate)
						sourceNullMerge = true;
					else
						source = default(T);
				}
			}
			
			// Perform target mapping and assign the copied value to the target field
			object registeredTarget = this.GetTargetOf(source);
			target = (T)registeredTarget;
			this.PerformCopyObject(sourceNullMerge ? default(T) : source, target, typeData);
		}
		void ICloneOperation.HandleValue<T>(ref T source, ref T target)
		{
			this.PerformCopyValue(ref source, ref target, null);
		}


		private	static List<ICloneSurrogate>					surrogates				= null;
		private	static Dictionary<Type,CloneType>				cloneTypeCache			= new Dictionary<Type,CloneType>();
		private	static Dictionary<Type,CloneBehaviorAttribute>	cloneBehaviorCache		= new Dictionary<Type,CloneBehaviorAttribute>();
		private static CloneBehaviorAttribute					memberInfoCloneBehavior = new CloneBehaviorAttribute(typeof(MemberInfo), CloneBehavior.Reference);

		/// <summary>
		/// Returns the <see cref="CloneType"/> of a Type.
		/// </summary>
		/// <param name="type"></param>
		protected internal static CloneType GetCloneType(Type type)
		{
			if (type == null) return null;

			CloneType result;
			if (cloneTypeCache.TryGetValue(type, out result)) return result;

			result = new CloneType(type);
			cloneTypeCache[type] = result;
			result.Init();
			return result;
		}
		internal static ICloneSurrogate GetSurrogateFor(TypeInfo type)
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
		internal static CloneBehaviorAttribute GetCloneBehaviorAttribute(TypeInfo typeInfo)
		{
			// Hardcoded cloning behavior for MemberInfo metadata classes
			if (typeof(MemberInfo).GetTypeInfo().IsAssignableFrom(typeInfo))
			{
				return memberInfoCloneBehavior;
			}

			// Attributes attached directly to this Type
			CloneBehaviorAttribute directAttrib;
			if (!cloneBehaviorCache.TryGetValue(typeInfo.AsType(), out directAttrib))
			{
				directAttrib = typeInfo.GetAttributesCached<CloneBehaviorAttribute>().FirstOrDefault();
				cloneBehaviorCache[typeInfo.AsType()] = directAttrib;
			}
			return directAttrib;
		}

		internal static void ClearTypeCache()
		{
			surrogates = null;
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
