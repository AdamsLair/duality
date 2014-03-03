using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Duality;

namespace Duality.Editor
{
	public class HelpStackChangedEventArgs : EventArgs
	{
		private	HelpInfo	last;
		private	HelpInfo	current;

		public HelpInfo LastHelp
		{
			get { return this.last; }
		}
		public HelpInfo CurrentHelp
		{
			get { return this.current; }
		}

		public HelpStackChangedEventArgs(HelpInfo last, HelpInfo current)
		{
			this.last = last;
			this.current = current;
		}
	}

	public class SelectionChangedEventArgs : EventArgs
	{
		private	ObjectSelection				current;
		private	ObjectSelection				previous;
		private ObjectSelection.Category	diffCat;
		private ObjectSelection				added;
		private ObjectSelection				removed;

		public ObjectSelection Current
		{
			get { return this.current; }
		}
		public ObjectSelection Previous
		{
			get { return this.previous; }
		}
		public ObjectSelection.Category AffectedCategories
		{
			get { return this.diffCat; }
		}
		public ObjectSelection Added
		{
			get { return this.added; }
		}
		public ObjectSelection Removed
		{
			get { return this.removed; }
		}
		public bool SameObjects
		{
			get { return this.added.Empty && this.removed.Empty; }
		}

		public SelectionChangedEventArgs(ObjectSelection current, ObjectSelection previous, ObjectSelection.Category changedCategoryFallback)
		{
			this.current = current;
			this.previous = previous;

			this.diffCat = ObjectSelection.GetAffectedCategories(this.previous, this.current);
			if (this.diffCat == ObjectSelection.Category.None) this.diffCat = changedCategoryFallback;
			this.added = this.current - this.previous;
			this.removed = this.previous - this.current;
		}
	}

	public class HighlightObjectEventArgs : EventArgs
	{
		private	ObjectSelection	target;
		private	HighlightMode	mode;

		public ObjectSelection Target
		{
			get { return this.target; }
		}
		public HighlightMode Mode
		{
			get { return this.mode; }
		}

		public HighlightObjectEventArgs(ObjectSelection target, HighlightMode mode)
		{
			this.target = target;
			this.mode = mode;
		}
	}

	public class ObjectPropertyChangedEventArgs : EventArgs
	{
		private	ObjectSelection		obj;
		private	List<PropertyInfo>	propInfos;
		private	List<string>		propNames;
		private	bool				completeChange;
		private	bool				persistCritical;

		public ObjectSelection Objects
		{
			get { return this.obj; }
		}
		public IEnumerable<PropertyInfo> PropInfos
		{
			get { return this.propInfos; }
		}
		public IEnumerable<string> PropNames
		{
			get { return this.propNames; }
		}
		public bool CompleteChange
		{
			get { return this.completeChange; }
		}
		public bool PersistenceCritical
		{
			get { return this.persistCritical; }
		}

		public ObjectPropertyChangedEventArgs(ObjectSelection obj, bool persistenceCritical = true) : this(obj, null, persistenceCritical) {}
		public ObjectPropertyChangedEventArgs(ObjectSelection obj, IEnumerable<PropertyInfo> infos, bool persistenceCritical = true)
		{
			if (infos == null) infos = new PropertyInfo[0];

			this.obj = obj;
			this.propInfos = infos.ToList();
			this.propNames = this.propInfos.Select(i => i.Name).ToList();
			this.completeChange = this.propInfos.Count == 0;
			this.persistCritical = persistenceCritical;
		}

		public bool HasObject(object obj)
		{
			return this.obj.Contains(obj);
		}
		public bool HasAnyObject(IEnumerable<object> objEnum)
		{
			return objEnum.Any(o => this.obj.Contains(o));
		}

		public bool HasProperty(PropertyInfo info)
		{
			return this.completeChange || this.propInfos.Any(i => ReflectionHelper.MemberInfoEquals(i, info));
		}
		public bool HasProperty(string name)
		{
			return this.completeChange || this.propNames.Contains(name);
		}

		public bool HasAnyProperty(params PropertyInfo[] info)
		{
			return info.Any(this.HasProperty);
		}
		public bool HasAnyProperty(params string[] name)
		{
			return name.Any(this.HasProperty);
		}
	}

	public class PrefabAppliedEventArgs : ObjectPropertyChangedEventArgs
	{
		public PrefabAppliedEventArgs(ObjectSelection obj) : base(obj.HierarchyExpand(), new PropertyInfo[0]) {}
	}

	public class ResourceRenamedEventArgs : ResourceEventArgs
	{
		private	string	oldPath;

		public string OldPath
		{
			get { return this.oldPath; }
		}
		public ContentRef<Resource> OldContent
		{
			get { return this.IsDirectory ? ContentRef<Resource>.Null : new ContentRef<Resource>(null, this.oldPath); }
		}

		public ResourceRenamedEventArgs(string path, string oldPath) : base(path)
		{
			this.oldPath = oldPath;
		}
	}

	public class BeginGlobalRenameEventArgs : ResourceRenamedEventArgs
	{
		private	bool	cancel	= false;
		public bool Cancel
		{
			get { return this.cancel; }
			set { this.cancel = value; }
		}
		public BeginGlobalRenameEventArgs(string path, string oldPath) : base(path, oldPath) {}
	}
}
