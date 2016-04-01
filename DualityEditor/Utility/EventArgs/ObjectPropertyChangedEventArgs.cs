using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Duality;

namespace Duality.Editor
{
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
			// If the property is mentioned explicitly, we have a match
			if (this.propInfos.Any(i => i.IsEquivalent(info)))
				return true;

			// If we represent a complete, object-wide change, we have a match if
			// there is at least one object that has the specified property.
			if (this.completeChange && this.obj.Objects.Any(o => info.DeclaringType.IsAssignableFrom(o.GetType())))
				return true;

			// No match found
			return false;
		}
		public bool HasAnyProperty(params PropertyInfo[] info)
		{
			// If a property is mentioned explicitly, we have a match
			foreach (PropertyInfo property in info)
			{
				if (this.propInfos.Any(i => i.IsEquivalent(property)))
					return true;
			}

			// If we represent a complete, object-wide change, we have a match if
			// there is at least one object that has one of the specified properties.
			if (this.completeChange)
			{
				foreach (PropertyInfo property in info)
				{
					if (this.obj.Objects.Any(o => property.DeclaringType.IsAssignableFrom(o.GetType())))
						return true;
				}
			}

			// No match found
			return false;
		}
	}
}
