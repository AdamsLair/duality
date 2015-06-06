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
		public delegate object Resolver(string manifestResourceName);

		private	object		iconImageObj			= null;
		private	string		manifestResourceName	= null;
		private	bool		resolved				= false;

		private static List<Resolver> registeredResolvers = new List<Resolver>();
		public static event Resolver ImageResolvers
		{
			add { registeredResolvers.Add(value); }
			remove { registeredResolvers.Remove(value); }
		}

		/// <summary>
		/// [GET] The icon image object that will be used to represent this Type.
		/// </summary>
		public object IconImageObject
		{
			get
			{
				if (!this.resolved) this.ResolveImageObject();
				return this.iconImageObj;
			}
		}

		public EditorHintImageAttribute(string manifestResourceName)
		{
			this.manifestResourceName = manifestResourceName;
		}

		private void ResolveImageObject()
		{
			this.resolved = true;
			this.iconImageObj = null;
			if (!string.IsNullOrEmpty(this.manifestResourceName))
			{
				foreach (Resolver resolver in registeredResolvers)
				{
					this.iconImageObj = resolver(this.manifestResourceName);
					if (this.iconImageObj != null) break;
				}
			}
		}
	}
}
