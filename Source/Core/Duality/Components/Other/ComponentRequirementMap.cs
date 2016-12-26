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
	public class ComponentRequirementMap
	{
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
			public List<CreationChainItem> CreationChain;

			public TypeData(Type type)
			{
				this.Component = type;
			}

			public void InitRequirements(ComponentRequirementMap map)
			{
				this.Requirements = new List<Type>();
				IEnumerable<RequiredComponentAttribute> attribs = this.Component.GetTypeInfo().GetAttributesCached<RequiredComponentAttribute>();
				foreach (RequiredComponentAttribute a in attribs)
				{
					Type reqType = a.RequiredComponentType;

					// Don't require itself
					if (reqType == this.Component) continue;

					this.Requirements.AddRange(map.GetRequiredComponents(reqType).Where(t => !this.Requirements.Contains(t)));
					if (!this.Requirements.Contains(reqType))
						this.Requirements.Add(reqType);
				}
			}
			public void InitCreationChain(ComponentRequirementMap map)
			{
				this.CreationChain = new List<CreationChainItem>();
				IEnumerable<RequiredComponentAttribute> attributes = this.Component.GetTypeInfo().GetAttributesCached<RequiredComponentAttribute>();
				foreach (RequiredComponentAttribute attrib in attributes)
				{
					// If this is a conditional creation, add the sub-chain of the Component to create
					if (attrib.CreateDefaultType != attrib.RequiredComponentType)
						this.AddCreationChainElements(map, attrib, attrib.CreateDefaultType);

					// In any case, add the sub-chain of direct requirements
					this.AddCreationChainElements(map, attrib, attrib.RequiredComponentType);
				}

				// Remove any duplicates that we might have generated in the creation chain
				this.RemoveCreationChainDuplicates();
			}

			private void AddCreationChainElements(ComponentRequirementMap map, RequiredComponentAttribute attrib, Type subChainType)
			{
				if (subChainType == this.Component) return;

				int baseIndex = this.CreationChain.Count;
				int subChainLength = 0;

				// Retrieve the creation sub-chain to satisfy this item's requirements
				List<CreationChainItem> createTypeSubChain = map.GetCreationChain(subChainType);
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


		private Dictionary<Type,TypeData> typeDataCache = new Dictionary<Type,TypeData>();


		public bool RequiresComponent(Type cmpType, Type requiredType)
		{
			if (cmpType == requiredType) return false;

			TypeInfo requiredTypeInfo = requiredType.GetTypeInfo();
			foreach (Type type in this.GetRequiredComponents(cmpType))
			{
				if (type.GetTypeInfo().IsAssignableFrom(requiredTypeInfo))
					return true;
			}

			return false;
		}
		public bool IsComponentRequirementMet(GameObject targetObj, Type targetComponentType, IEnumerable<Component> whenAddingThose = null)
		{
			IEnumerable<Type> reqTypes = this.GetRequiredComponents(targetComponentType);
			foreach (Type reqType in reqTypes)
			{
				TypeInfo reqTypeInfo = reqType.GetTypeInfo();
				if (targetObj.GetComponent(reqType) == null)
				{
					if (whenAddingThose == null) return false;
					else if (!whenAddingThose.Any(c => reqTypeInfo.IsInstanceOfType(c))) return false;
				}
			}

			return true;
		}
		public IEnumerable<Type> GetRequiredComponents(Type cmpType)
		{
			TypeData data;
			if (!this.typeDataCache.TryGetValue(cmpType, out data))
			{
				data = new TypeData(cmpType);
				this.typeDataCache[cmpType] = data;
			}
			if (data.Requirements == null)
				data.InitRequirements(this);

			return data.Requirements;
		}
		public IEnumerable<Type> GetRequiredComponentsToCreate(GameObject targetObj, Type targetComponentType)
		{
			// Retrieve the component's requirements
			TypeData data;
			if (!this.typeDataCache.TryGetValue(targetComponentType, out data))
			{
				data = new TypeData(targetComponentType);
				this.typeDataCache[targetComponentType] = data;
			}
			if (data.CreationChain == null)
				data.InitCreationChain(this);

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
		public void ClearTypeCache()
		{
			this.typeDataCache.Clear();
		}
		
		private List<CreationChainItem> GetCreationChain(Type cmpType)
		{
			TypeData data;
			if (!this.typeDataCache.TryGetValue(cmpType, out data))
			{
				data = new TypeData(cmpType);
				this.typeDataCache[cmpType] = data;
			}
			if (data.CreationChain == null)
				data.InitCreationChain(this);

			return data.CreationChain;
		}
	}
}
