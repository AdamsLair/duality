using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;

using Duality.Editor.Properties;
using Duality.Properties;

namespace Duality.Editor.Controls.TreeModels.TypeHierarchy
{
	public abstract class BaseItem
	{
		private BaseItem parent = null;
		public BaseItem Parent
		{
			get { return parent; }
		}
		public abstract Image Icon { get; }
		public abstract string Name { get; }
		public abstract string Namespace { get; }

		public BaseItem(BaseItem parent)
		{
			this.parent = parent;
		}
	}
	public class TypeItem : BaseItem
	{
		private Type type = null;
		private Image icon = null;
		
		public Type TypeInfo
		{
			get { return this.type; }
		}
		public override Image Icon
		{
			get { return this.icon; }
		}
		public override string Name
		{
			get { return this.type.Name; }
		}
		public override string Namespace
		{
			get { return this.type.Namespace; }
		}

		public TypeItem(Type type, BaseItem parent) : base(parent)
		{
			this.type = type;
			this.icon = type.GetEditorImage() ?? CoreRes.IconClass;
		}
	}
	public class NamespaceItem : BaseItem
	{
		private string name = null;
		
		public override Image Icon
		{
			get { return GeneralRes.IconNamespace; }
		}
		public override string Name
		{
			get { return this.name; }
		}
		public override string Namespace
		{
			get { return this.name; }
		}

		public NamespaceItem(string name, BaseItem parent) : base(parent)
		{
			this.name = name;
		}
	}
}
