using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using OpenTK;

using Duality.Resources;
using Duality.Cloning;
using Duality.EditorHints;

using ICloneable = Duality.Cloning.ICloneable;

namespace Duality
{
	/// <summary>
	/// This attribute indicates a <see cref="Component">Components</see> requirement for another Component
	/// of a specific Type, that is attached to the same <see cref="GameObject"/>.
	/// </summary>
	/// <example>
	/// The following code uses a RequiredComponentAttribute to indicate that a <see cref="Duality.Components.SoundEmitter"/>
	/// always needs a <see cref="Duality.Components.Transform"/> available as well.
	/// <code>
	/// [RequiredComponent(typeof(Transform))]
	/// public sealed class SoundEmitter : Component, ICmpUpdatable, ICmpInitializable
	/// {
	///		// ...
	/// }
	/// </code>
	/// </example>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class RequiredComponentAttribute : Attribute
	{
		private	Type	cmpType;

		/// <summary>
		/// The Component Type that is required by this Component.
		/// </summary>
		public Type RequiredComponentType
		{
			get { return this.cmpType; }
		}

		public RequiredComponentAttribute(Type cmpType)
		{
			this.cmpType = cmpType;
		}
	}

	/// <summary>
	/// Components are isolated logic units that can independently be added to and removed from <see cref="GameObject">GameObjects</see>.
	/// Each Component has a distinct purpose, thus it is not possible to add multiple Components of the same Type to one GameObject.
	/// Also, a Component may not belong to multiple GameObjects at once.
	/// </summary>
	[Serializable]
	public abstract class Component : IManageableObject, ICloneable, Serialization.IUniqueIdentifyable
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
			/// The Component is being activated. This can be the result of <see cref="Active">activating</see> it,
			/// <see cref="GameObject.Active">activating</see> its GameObject, adding itsself or its GameObject
			/// to the <see cref="Duality.Resources.Scene.Current">current Scene</see> or entering a <see cref="Scene"/>
			/// in which this Component is registered.
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
			/// The Component has been deactivated. This can be the result of <see cref="Active">deactivating</see> it,
			/// <see cref="GameObject.Active">deactivating</see> its GameObject, removing itsself or its GameObject
			/// from the <see cref="Duality.Resources.Scene.Current">current Scene</see> or leaving a <see cref="Scene"/>
			/// in which this Component is registered.
			/// </summary>
			Deactivate,
			/// <summary>
			/// The Component is being removed from its GameObject.
			/// </summary>
			RemovingFromGameObject
		}


		internal	GameObject		gameobj		= null;
		private		InitState		initState	= InitState.Initialized;
		private		bool			active		= true;

		
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
			get { return this.active && this.initState.IsActive(); }
			set 
			{
				if (this.active != value)
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
			get { return this.initState == InitState.Disposed; }
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
		
		uint Serialization.IUniqueIdentifyable.PreferredId
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
			if (this.initState == InitState.Initialized)
			{
				this.initState = InitState.Disposing;

				// Remove from GameObject
				if (this.gameobj != null) 
					this.gameobj.RemoveComponent(this);

				this.initState = InitState.Disposed;
			}
		}
		
		/// <summary>
		/// Creates a deep copy of this Component.
		/// </summary>
		/// <returns>A reference to a newly created deep copy of this Component.</returns>
		public Component Clone()
		{
			return CloneProvider.DeepClone(this);
		}
		/// <summary>
		/// Deep-copies this Components data to the specified target Component. If source and 
		/// target Component Type do not match, the operation will fail.
		/// </summary>
		/// <param name="target">The target Component to copy to.</param>
		public void CopyTo(Component target)
		{
			CloneProvider.DeepCopyTo(this, target);
		}
		/// <summary>
		/// This method Performs the <see cref="CopyTo"/> operation for custom Component Types.
		/// It uses reflection to copy each field that is declared inside a Duality plugin automatically.
		/// However, you may override this method to specify your own behaviour or simply speed things
		/// up a bit by not using Reflection.
		/// </summary>
		/// <remarks>
		/// Note: Since PrefabLinks may contain references to object-local values,
		/// all objects fields should either be overwritten or left untouched.
		/// DO NOT modify any referenced objects. Instead, discard them and create new.
		/// </remarks>
		/// <param name="target">The target Component where this Components data is copied to.</param>
		protected virtual void OnCopyTo(Component target, CloneProvider provider)
		{
			// Copy "pure" data
			target.active		= this.active;
			target.initState	= this.initState;

			// If any derived Component type doesn't override OnCopyTo, use a reflection-driven default behavior.
			CloneProvider.PerformReflectionFallback("OnCopyTo", this, target, provider);
		}
		void ICloneable.CopyDataTo(object targetObj, CloneProvider provider)
		{
			this.OnCopyTo(targetObj as Component, provider);
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
			var reqTypes = this.GetRequiredComponents();
			foreach (Type r in reqTypes)
			{
				if (isMetInObj.GetComponent(r) == null)
				{
					if (whenAddingThose == null) return false;
					else if (!whenAddingThose.Any(c => r.IsInstanceOfType(c))) return false;
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
		private class TypeData
		{
			public Type Component;
			public List<Type> Requirements;
			public List<Type> RequiredBy;

			public TypeData(Type type)
			{
				this.Component = type;
			}

			public void InitRequirements()
			{
				if (this.Requirements != null) return;
				this.Requirements = new List<Type>();
				IEnumerable<RequiredComponentAttribute> attribs = 
					this.Component.GetCustomAttributes(typeof(RequiredComponentAttribute), true).
					Cast<RequiredComponentAttribute>();
				foreach (RequiredComponentAttribute a in attribs)
				{
					Type reqType = a.RequiredComponentType;
					if (reqType == this.Component) continue; // Don't require itself
					this.Requirements.AddRange(GetRequiredComponents(reqType).Where(t => !this.Requirements.Contains(t)));
					this.Requirements.Add(reqType);
				}
			}
			public void InitRequiredBy()
			{
				if (this.RequiredBy != null) return;
				this.RequiredBy = new List<Type>();
				foreach (Type cmp in DualityApp.GetAvailDualityTypes(typeof(Component)))
				{
					if (cmp == this.Component) continue; // Don't require itself
					if (RequiresComponent(cmp, this.Component))
						this.RequiredBy.Add(cmp);
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
			TypeData data;
			if (!typeCache.TryGetValue(cmpType, out data))
			{
				data = new TypeData(cmpType);
				typeCache[cmpType] = data;
			}
			data.InitRequirements();
			return data.Requirements.Any(r => r.IsAssignableFrom(requiredType));
		}
		/// <summary>
		/// Returns all required Component Types of a specified Component Type.
		/// </summary>
		/// <param name="cmpType">The Component Type that might require other Component Types.</param>
		/// <param name="recursive">If true, also indirect requirements are returned.</param>
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
		/// Clears the ReflectionHelpers Type cache.
		/// </summary>
		internal static void ClearTypeCache()
		{
			typeCache.Clear();
		}
	}

	public class ComponentTypeComparer : IEqualityComparer<Component>
	{
		public static readonly ComponentTypeComparer Default = new ComponentTypeComparer();

		public bool Equals(Component x, Component y)
		{
			if (x == y) return true;
			if (x == null || y == null) return false;
			return x.GetType() == y.GetType();
		}
		public int GetHashCode(Component obj)
		{
			return obj != null ? obj.GetType().GetHashCode() : 0;
		}
	}
}
