using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.IO;
using System.Drawing;

namespace Duality.Editor
{
	/// <summary>
	/// Provides an icon or image that can be used to represent the given Type within the editor.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false)]
	public class EditorHintImageAttribute : EditorHintAttribute
	{
		public delegate Image Resolver(string resourceTypeId, string propertyName);

		private	Image		iconImage		= null;
		private	string		resourceTypeId	= null;
		private	string		propertyName	= null;
		private	bool		resolved		= false;

		private static List<Resolver> registeredResolvers = new List<Resolver> { DefaultResolver };
		public static event Resolver ImageResolvers
		{
			add { registeredResolvers.Add(value); }
			remove { registeredResolvers.Remove(value); }
		}

		/// <summary>
		/// [GET] The icon image that will be used to represent this Type.
		/// </summary>
		public Image IconImage
		{
			get
			{
				if (!this.resolved) this.ResolveImage();
				return this.iconImage;
			}
		}

		public EditorHintImageAttribute(string resourceTypeId, string propertyName)
		{
			this.resourceTypeId = resourceTypeId;
			this.propertyName = propertyName;
		}
		public EditorHintImageAttribute(Type resourceType, string propertyName)
		{
			this.resourceTypeId = resourceType.GetTypeId();
			this.propertyName = propertyName;
		}

		private void ResolveImage()
		{
			this.resolved = true;
			this.iconImage = null;
			if (!string.IsNullOrEmpty(this.resourceTypeId) && !string.IsNullOrEmpty(this.propertyName))
			{
				foreach (Resolver resolver in registeredResolvers)
				{
					this.iconImage = resolver(this.resourceTypeId, this.propertyName);
					if (this.iconImage != null) break;
				}
			}
		}

		private static Image DefaultResolver(string resourceTypeId, string propertyName)
		{
			Type resourceClass = ReflectionHelper.ResolveType(resourceTypeId);
			PropertyInfo resourceProperty = resourceClass != null ? resourceClass.GetProperty(propertyName, ReflectionHelper.BindStaticAll) : null;
			if (resourceProperty != null && typeof(Image).IsAssignableFrom(resourceProperty.PropertyType))
			{
				try
				{
					return resourceProperty.GetValue(null, null) as Image;
				}
				catch (Exception) {}
			}

			return null;
		}
	}
}
