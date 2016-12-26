using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Duality.Resources;
using Duality.Cloning;
using Duality.Serialization;
using Duality.Editor;
using Duality.Properties;

namespace Duality
{
	/// <summary>
	/// Components are isolated logic units that can independently be added to and removed from <see cref="GameObject">GameObjects</see>.
	/// Each Component has a distinct purpose, thus it is not possible to add multiple Components of the same Type to one GameObject.
	/// Also, a Component may not belong to multiple GameObjects at once.
	/// </summary>
	[ManuallyCloned]
	[CloneBehavior(CloneBehavior.Reference)]
	[EditorHintImage(CoreResNames.ImageComponent)]
	public abstract class Component : IManageableObject, IUniqueIdentifyable, ICloneExplicit
	{
		/// <summary>
		/// Describes the kind of initialization that can be performed on a Component
		/// </summary>
		public enum InitContext
		{
			/// <summary>
			/// A saving process has just finished.
			/// </summary>
			Saved,
			/// <summary>
			/// The Component has been fully loaded.
			/// </summary>
			Loaded,
			/// <summary>
			/// The Component is being activated. This can be the result of activating it,
			/// activating its GameObject, adding itsself or its GameObject to the current 
			/// Scene or entering a <see cref="Scene"/> in which this Component is registered.
			/// </summary>
			Activate,
			/// <summary>
			/// The Component has just been added to a GameObject
			/// </summary>
			AddToGameObject
		}
		/// <summary>
		/// Describes the kind of shutdown that can be performed on a Component
		/// </summary>
		public enum ShutdownContext
		{
			/// <summary>
			/// A saving process is about to start
			/// </summary>
			Saving,
			/// <summary>
			/// The Component has been deactivated. This can be the result of deactivating it,
			/// deactivating its GameObject, removing itsself or its GameObject from the 
			/// current Scene or leaving a <see cref="Scene"/> in which this Component is registered.
			/// </summary>
			Deactivate,
			/// <summary>
			/// The Component is being removed from its GameObject.
			/// </summary>
			RemovingFromGameObject
		}


		internal	GameObject	gameobj	= null;
		private		bool		active	= true;

		
		/// <summary>
		/// [GET / SET] Whether or not the Component is currently active. To return true,
		/// both the Component itsself and its parent GameObject need to be active.
		/// </summary>
		/// <seealso cref="ActiveSingle"/>
		public bool Active
		{
			get { return this.ActiveSingle && this.gameobj != null && this.gameobj.Active; }
			set { this.ActiveSingle = value; }
		}
		/// <summary>
		/// [GET / SET] Whether or not the Component is currently active. Unlike <see cref="Active"/>,
		/// this property ignores parent activation states and depends only on this single Component.
		/// The scene graph and other Duality instances usually check <see cref="Active"/>, not ActiveSingle.
		/// </summary>
		/// <seealso cref="Active"/>
		public bool ActiveSingle
		{
			get { return this.active; }
			set 
			{
				if (this.active != value)
				{
					if (this.gameobj != null && this.gameobj.ParentScene != null && this.gameobj.ParentScene.IsCurrent)
					{
						if (value)
						{
							ICmpInitializable cInit = this as ICmpInitializable;
							if (cInit != null) cInit.OnInit(InitContext.Activate);
						}
						else
						{
							ICmpInitializable cInit = this as ICmpInitializable;
							if (cInit != null) cInit.OnShutdown(ShutdownContext.Deactivate);
						}
					}

					this.active = value;
				}
			}
		}
		/// <summary>
		/// [GET] Returns whether this Component has been disposed. Disposed Components are not to be used and should
		/// be treated specifically or as null references by your code.
		/// </summary>
		public bool Disposed
		{
			get { return this.gameobj != null && this.gameobj.Disposed; }
		}
		/// <summary>
		/// [GET / SET] The <see cref="GameObject"/> to which this Component belongs.
		/// </summary>
		public GameObject GameObj
		{
			get { return this.gameobj; }
			set
			{
				if (this.gameobj != null) this.gameobj.RemoveComponent(this);
				if (value != null) value.AddComponent(this);
			}
		}
		
		uint IUniqueIdentifyable.PreferredId
		{
			get 
			{
				unchecked
				{
					int idTemp = this.GetType().GetTypeId().GetHashCode();
					if (this.gameobj != null)
					{
						MathF.CombineHashCode(ref idTemp, this.gameobj.Id.GetHashCode());
					}
					return (uint)idTemp;
				}
			}
		}


		/// <summary>
		/// Disposes this Component. You usually don't need this - use <see cref="ExtMethodsIManageableObject.DisposeLater"/> instead.
		/// </summary>
		/// <seealso cref="ExtMethodsIManageableObject.DisposeLater"/>
		public void Dispose()
		{
			// Remove from GameObject
			if (this.gameobj != null)
				this.gameobj.RemoveComponent(this);
		}
		
		/// <summary>
		/// Creates a deep copy of this Component.
		/// </summary>
		/// <returns>A reference to a newly created deep copy of this Component.</returns>
		public Component Clone()
		{
			return this.DeepClone();
		}
		/// <summary>
		/// Deep-copies this Components data to the specified target Component. If source and 
		/// target Component Type do not match, the operation will fail.
		/// </summary>
		/// <param name="target">The target Component to copy to.</param>
		public void CopyTo(Component target)
		{
			this.DeepCopyTo(target);
		}

		void ICloneExplicit.SetupCloneTargets(object targetObj, ICloneTargetSetup setup)
		{
			Component target = targetObj as Component;
			this.OnSetupCloneTargets(targetObj, setup);
		}
		void ICloneExplicit.CopyDataTo(object targetObj, ICloneOperation operation)
		{
			Component target = targetObj as Component;
			target.active = this.active;
			this.OnCopyDataTo(targetObj, operation);
		}
		/// <summary>
		/// This method prepares the <see cref="CopyTo"/> operation for custom Component Types.
		/// It uses reflection to prepare the cloning operation automatically, but you can implement
		/// this method in order to handle certain fields and cases manually. See <see cref="ICloneExplicit.SetupCloneTargets"/>
		/// for a more thorough explanation.
		/// </summary>
		/// <param name="setup"></param>
		protected virtual void OnSetupCloneTargets(object target, ICloneTargetSetup setup)
		{
			setup.HandleObject(this, target);
		}
		/// <summary>
		/// This method performs the <see cref="CopyTo"/> operation for custom Component Types.
		/// It uses reflection to perform the cloning operation automatically, but you can implement
		/// this method in order to handle certain fields and cases manually. See <see cref="ICloneExplicit.CopyDataTo"/>
		/// for a more thorough explanation.
		/// </summary>
		/// <param name="target">The target Component where this Components data is copied to.</param>
		/// <param name="operation"></param>
		protected virtual void OnCopyDataTo(object target, ICloneOperation operation)
		{
			operation.HandleObject(this, target);
		}

		/// <summary>
		/// Returns whether this Component requires a Component of the specified Type.
		/// </summary>
		/// <param name="requiredType">The Component Type that might be required.</param>
		/// <returns>True, if there is a requirement, false if not</returns>
		public bool RequiresComponent(Type requiredType)
		{
			return RequiresComponent(this.GetType(), requiredType);
		}
		/// <summary>
		/// Returns whether this objects Component requirement is met.
		/// </summary>
		/// <param name="evenWhenRemovingThis">If not null, the specified Component is assumed to be missing.</param>
		/// <returns>True, if the Component requirement is met, false if not.</returns>
		public bool IsComponentRequirementMet(Component evenWhenRemovingThis = null)
		{
			var reqTypes = this.GetRequiredComponents();
			return reqTypes.All(r => this.GameObj.GetComponents(r).Any(c => c != evenWhenRemovingThis));
		}
		/// <summary>
		/// Returns whether this objects Component requirement is met assuming a different <see cref="GameObj">parent GameObject</see>
		/// </summary>
		/// <param name="isMetInObj">The specified object is assumed as parent object.</param>
		/// <param name="whenAddingThose">If not null, the specified Components are assumed to be present in the specified parent object.</param>
		/// <returns>True, if the Component requirement is met, false if not.</returns>
		public bool IsComponentRequirementMet(GameObject isMetInObj, IEnumerable<Component> whenAddingThose = null)
		{
			IEnumerable<Type> reqTypes = this.GetRequiredComponents();
			foreach (Type reqType in reqTypes)
			{
				TypeInfo reqTypeInfo = reqType.GetTypeInfo();
				if (isMetInObj.GetComponent(reqType) == null)
				{
					if (whenAddingThose == null) return false;
					else if (!whenAddingThose.Any(c => reqTypeInfo.IsInstanceOfType(c))) return false;
				}
			}

			return true;
		}
		/// <summary>
		/// Returns all Component Types this Component requires.
		/// </summary>
		/// <returns>An array of required Component Types.</returns>
		public IEnumerable<Type> GetRequiredComponents()
		{
			return GetRequiredComponents(this.GetType());
		}

		public override string ToString()
		{
			if (this.gameobj == null)
				return this.GetType().Name;
			else
				return string.Format("{0} in \"{1}\"", this.GetType().Name, this.gameobj.FullName);
		}


		private static Dictionary<Type,TypeData> typeCache = new Dictionary<Type,TypeData>();
		private struct CreationChainItem
		{
			public Type RequiredType;
			public Type CreateType;
			public int SkipIfExists;

			public override string ToString()
			{
				if (this.SkipIfExists > 0)
				{
					return string.Format("Skip {0} if {1} exists", 
						this.SkipIfExists, 
						Log.Type(this.RequiredType));
				}
				else
				{
					return string.Format("Require {0} or create {1}", 
						Log.Type(this.RequiredType), 
						Log.Type(this.CreateType));
				}
			}
		}
		private class TypeData
		{
			public Type Component;
			public List<Type> Requirements;
			public List<Type> RequiredBy;
			public List<CreationChainItem> CreationChain;

			public TypeData(Type type)
			{
				this.Component = type;
			}

			public void InitRequirements()
			{
				if (this.Requirements != null) return;

				this.Requirements = new List<Type>();
				IEnumerable<RequiredComponentAttribute> attribs = this.Component.GetTypeInfo().GetAttributesCached<RequiredComponentAttribute>();
				foreach (RequiredComponentAttribute a in attribs)
				{
					Type reqType = a.RequiredComponentType;

					// Don't require itself
					if (reqType == this.Component) continue;

					this.Requirements.AddRange(GetRequiredComponents(reqType).Where(t => !this.Requirements.Contains(t)));
					if (!this.Requirements.Contains(reqType))
						this.Requirements.Add(reqType);
				}
			}
			public void InitRequiredBy()
			{
				if (this.RequiredBy != null) return;
				this.RequiredBy = new List<Type>();
				foreach (TypeInfo cmpTypeInfo in DualityApp.GetAvailDualityTypes(typeof(Component)))
				{
					Type cmpType = cmpTypeInfo.AsType();

					// Don't require itself
					if (cmpType == this.Component) continue;

					if (RequiresComponent(cmpType, this.Component))
						this.RequiredBy.Add(cmpType);
				}
			}
			public void InitCreationChain()
			{
				if (this.CreationChain != null) return;

				this.CreationChain = new List<CreationChainItem>();
				IEnumerable<RequiredComponentAttribute> attributes = this.Component.GetTypeInfo().GetAttributesCached<RequiredComponentAttribute>();
				foreach (RequiredComponentAttribute attrib in attributes)
				{
					// If this is a conditional creation, add the sub-chain of the Component to create
					if (attrib.CreateDefaultType != attrib.RequiredComponentType)
						this.AddCreationChainElements(attrib, attrib.CreateDefaultType);

					// In any case, add the sub-chain of direct requirements
					this.AddCreationChainElements(attrib, attrib.RequiredComponentType);
				}

				// Remove any duplicates that we might have generated in the creation chain
				this.RemoveCreationChainDuplicates();
			}

			private void AddCreationChainElements(RequiredComponentAttribute attrib, Type subChainType)
			{
				if (subChainType == this.Component) return;

				int baseIndex = this.CreationChain.Count;
				int subChainLength = 0;

				// Retrieve the creation sub-chain to satisfy this item's requirements
				List<CreationChainItem> createTypeSubChain = GetCreationChain(subChainType);
				foreach (CreationChainItem subItem in createTypeSubChain)
				{
					this.CreationChain.Add(subItem);
					subChainLength++;
				}

				// Add the main item after its requirement items so we're always creating bottom-up
				this.CreationChain.Add(new CreationChainItem
				{
					RequiredType = attrib.RequiredComponentType,
					CreateType = attrib.CreateDefaultType
				});

				// If this is a conditional requirement, add a control item that checks the
				// requirement and skips this item and its sub-chains if it's already met.
				if (subChainLength > 0 && attrib.CreateDefaultType != attrib.RequiredComponentType)
				{
					this.CreationChain.Insert(baseIndex, new CreationChainItem
					{
						RequiredType = attrib.RequiredComponentType,
						SkipIfExists = 1 + subChainLength
					});
				}
			}
			private void RemoveCreationChainDuplicates()
			{
				// We'll iterate over the creation chain assuming that each item
				// is found to be already existing (so we can't assume to have created
				// any specific type for abstract and interface requirements).
				List<TypeInfo> guaranteedTypes = new List<TypeInfo>();
				int uncertainCounter = 0;
				int parentIndex = -1;
				for (int i = 0; i < this.CreationChain.Count; i++)
				{
					// Can we guarantee that all requirements of this item will have been met?
					TypeInfo requriedTypeInfo = this.CreationChain[i].RequiredType.GetTypeInfo();
					bool requirementMet = guaranteedTypes.Any(t => requriedTypeInfo.IsAssignableFrom(t));

					// If yes, remove the item and its sub-chains
					if (requirementMet)
					{
						int removeCount = 1 + this.CreationChain[i].SkipIfExists;
						this.CreationChain.RemoveRange(i, removeCount);
						if (uncertainCounter > 0)
						{
							CreationChainItem parentItem = this.CreationChain[parentIndex];
							parentItem.SkipIfExists -= removeCount;
							this.CreationChain[parentIndex] = parentItem;
							uncertainCounter -= removeCount;
						}
						i--;
					}
					// Otherwise, proceed to the next item
					else
					{
						if (uncertainCounter == 0)
						{
							// If this item will create a Component, we can guarantee that its requirement is met
							if (this.CreationChain[i].CreateType != null)
								guaranteedTypes.Add(requriedTypeInfo);
							// If this item might skip followup items, only allow to assume that the very last
							// item's requirement will have been met, since after that one we can be sure that
							// it was either there all along or was now created.
							uncertainCounter += Math.Max(0, this.CreationChain[i].SkipIfExists - 1);
							parentIndex = i;
						}
						else
						{
							uncertainCounter--;
						}
					}
				}
			}
		}

		/// <summary>
		/// Returns whether a Component Type requires another Component Type to work properly.
		/// </summary>
		/// <param name="cmpType">The Component Type that might require another Component Type.</param>
		/// <param name="requiredType">The Component Type that might be required.</param>
		/// <returns>True, if there is a requirement, false if not</returns>
		public static bool RequiresComponent(Type cmpType, Type requiredType)
		{
			if (cmpType == requiredType) return false;

			TypeInfo requiredTypeInfo = requiredType.GetTypeInfo();

			TypeData data;
			if (!typeCache.TryGetValue(cmpType, out data))
			{
				data = new TypeData(cmpType);
				typeCache[cmpType] = data;
			}
			data.InitRequirements();
			return data.Requirements.Any(reqType => reqType.GetTypeInfo().IsAssignableFrom(requiredTypeInfo));
		}
		/// <summary>
		/// Returns all required Component Types of a specified Component Type.
		/// </summary>
		/// <param name="cmpType">The Component Type that might require other Component Types.</param>
		/// <returns>An array of Component Types to require.</returns>
		public static IEnumerable<Type> GetRequiredComponents(Type cmpType)
		{
			TypeData data;
			if (!typeCache.TryGetValue(cmpType, out data))
			{
				data = new TypeData(cmpType);
				typeCache[cmpType] = data;
			}
			data.InitRequirements();
			return data.Requirements;
		}
		/// <summary>
		/// Returns the number of Component Types that require the specified Component Type.
		/// This can be used as a measure of relative Component significance.
		/// </summary>
		/// <param name="cmpType"></param>
		/// <returns></returns>
		public static IEnumerable<Type> GetRequiringComponents(Type requiredType)
		{
			TypeData data;
			if (!typeCache.TryGetValue(requiredType, out data))
			{
				data = new TypeData(requiredType);
				typeCache[requiredType] = data;
			}
			data.InitRequiredBy();
			return data.RequiredBy;
		}
		/// <summary>
		/// Initializes or returns the creation chain for the specified <see cref="Component"/> type.
		/// </summary>
		/// <param name="cmpType"></param>
		/// <returns></returns>
		private static List<CreationChainItem> GetCreationChain(Type cmpType)
		{
			TypeData data;
			if (!typeCache.TryGetValue(cmpType, out data))
			{
				data = new TypeData(cmpType);
				typeCache[cmpType] = data;
			}
			data.InitCreationChain();
			return data.CreationChain;
		}
		/// <summary>
		/// Given the specified target <see cref="GameObject"/> and <see cref="Component"/> type,
		/// this method will enumerate all <see cref="Component"/> types that need to
		/// be added in order to satisfy its requirements.
		/// </summary>
		/// <param name="targetObj"></param>
		/// <param name="targetComponentType"></param>
		/// <returns></returns>
		public static IEnumerable<Type> GetRequiredComponentsToCreate(GameObject targetObj, Type targetComponentType)
		{
			// Retrieve the component's requirements
			TypeData data;
			if (!typeCache.TryGetValue(targetComponentType, out data))
			{
				data = new TypeData(targetComponentType);
				typeCache[targetComponentType] = data;
			}
			data.InitCreationChain();

			// Create a sorted list of all components that need to be instantiated
			// in order to satisfy the requirements for adding the given component to
			// the specified object.
			List<Type> createList = new List<Type>(data.CreationChain.Count);
			for (int i = 0; i < data.CreationChain.Count; i++)
			{
				CreationChainItem item = data.CreationChain[i];
				if (targetObj.GetComponent(item.RequiredType) != null)
				{
					i += item.SkipIfExists;
					continue;
				}
				if (item.CreateType != null && !createList.Contains(item.CreateType))
					createList.Add(item.CreateType);
			}
			return createList;
		}
		/// <summary>
		/// Clears the ReflectionHelpers Type cache.
		/// </summary>
		internal static void ClearTypeCache()
		{
			typeCache.Clear();
		}
	}
}
