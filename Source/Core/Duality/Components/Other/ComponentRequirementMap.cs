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
	/// Retrieves, processes and caches type information about how different <see cref="Component"/>
	/// types are interconnected using the <see cref="RequiredComponentAttribute"/>.
	/// </summary>
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
						LogFormat.Type(this.RequiredType));
				}
				else
				{
					return string.Format("Require {0} or create {1}", 
						LogFormat.Type(this.RequiredType), 
						LogFormat.Type(this.CreateType));
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

					this.Requirements.AddRange(map.GetRequirements(reqType).Where(t => !this.Requirements.Contains(t)));
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


		/// <summary>
		/// Returns whether the first <see cref="Component"/> requires the second one.
		/// In cases where a requirement can be satisfied by multiple different <see cref="Component"/>
		/// types, this method will return true for all of them.
		/// </summary>
		/// <param name="componentType"></param>
		/// <param name="requiredType"></param>
		/// <returns></returns>
		public bool IsRequired(Type componentType, Type requiredType)
		{
			if (componentType == requiredType) return false;

			TypeInfo requiredTypeInfo = requiredType.GetTypeInfo();
			foreach (Type type in this.GetRequirements(componentType))
			{
				if (type.GetTypeInfo().IsAssignableFrom(requiredTypeInfo))
					return true;
			}

			return false;
		}
		/// <summary>
		/// Returns whether the <see cref="Component"/> requirements for a given <see cref="Component"/> type are met on
		/// the specified <see cref="GameObject"/>, and whether they would be met if a specified set <see cref="Component"/>
		/// types would be added prior.
		/// </summary>
		/// <param name="targetObj"></param>
		/// <param name="targetComponentType"></param>
		/// <param name="whenAddingThose"></param>
		/// <returns></returns>
		public bool IsRequirementMet(GameObject targetObj, Type targetComponentType, IEnumerable<Type> whenAddingThose = null)
		{
			IEnumerable<Type> reqTypes = this.GetRequirements(targetComponentType);
			foreach (Type reqType in reqTypes)
			{
				TypeInfo reqTypeInfo = reqType.GetTypeInfo();
				if (targetObj.GetComponent(reqType) == null)
				{
					if (whenAddingThose == null)
						return false;
					else if (!whenAddingThose.Any(c => reqTypeInfo.IsAssignableFrom(c.GetTypeInfo())))
						return false;
				}
			}

			return true;
		}
		/// <summary>
		/// Enumerates all requirements of the specified <see cref="Component"/> type. 
		/// These may include abstract classes or interface definitions.
		/// </summary>
		/// <param name="componentType"></param>
		/// <returns></returns>
		public IEnumerable<Type> GetRequirements(Type componentType)
		{
			TypeData data;
			if (!this.typeDataCache.TryGetValue(componentType, out data))
			{
				data = new TypeData(componentType);
				this.typeDataCache[componentType] = data;
			}
			if (data.Requirements == null)
				data.InitRequirements(this);

			return data.Requirements;
		}
		/// <summary>
		/// Given the specified target <see cref="GameObject"/> and <see cref="Component"/> type,
		/// this method enumerates all <see cref="Component"/> types that will have to be created
		/// on the target object in order to satisfy its requirements. The result will be sorted
		/// in order of creation.
		/// </summary>
		/// <param name="targetObj"></param>
		/// <param name="targetComponentType"></param>
		/// <returns></returns>
		public IEnumerable<Type> GetRequirementsToCreate(GameObject targetObj, Type targetComponentType)
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
		/// <summary>
		/// Clears the internal type data that this map has been storing internally.
		/// </summary>
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
