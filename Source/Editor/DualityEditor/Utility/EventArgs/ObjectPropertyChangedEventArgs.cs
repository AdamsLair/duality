using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Duality;

namespace Duality.Editor
{
	/// <summary>
	/// Event arguments that provide information about the properties of a set of objects being modified.
	/// </summary>
	public class ObjectPropertyChangedEventArgs : EventArgs
	{
		private ObjectSelection	obj;
		private List<PropertyInfo> propInfos;
		private bool			   completeChange;
		private bool			   persistCritical;

		/// <summary>
		/// [GET] The list of affected objects.
		/// </summary>
		public ObjectSelection Objects
		{
			get { return this.obj; }
		}
		/// <summary>
		/// [GET] The list of explicitly affected properties. However, properties can also be
		/// affected indirectly, for example when a change is considered object-wide. To account
		/// for these cases and check for specific properties, use <see cref="HasProperty"/> instead.
		/// </summary>
		public IEnumerable<PropertyInfo> PropInfos
		{
			get { return this.propInfos; }
		}
		/// <summary>
		/// [GET] Whether the modification is considered to be complete / object-wide for the
		/// specified set of objects.
		/// </summary>
		public bool CompleteChange
		{
			get { return this.completeChange; }
		}
		/// <summary>
		/// [GET] Whether the modification is considered to be persistence-critical, i.e. should
		/// trigger flagging an object as unsaved.
		/// </summary>
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
			this.completeChange = this.propInfos.Count == 0;
			this.persistCritical = persistenceCritical;
		}

		/// <summary>
		/// Determines whether the specified object has been modified.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public bool HasObject(object obj)
		{
			return this.obj.Contains(obj);
		}
		/// <summary>
		/// Determines whether any of the specified objects has been modified.
		/// </summary>
		/// <param name="objEnum"></param>
		/// <returns></returns>
		public bool HasAnyObject(IEnumerable<object> objEnum)
		{
			return objEnum.Any(o => this.obj.Contains(o));
		}

		/// <summary>
		/// Determines whether the specified property has been modified.
		/// </summary>
		/// <param name="info"></param>
		/// <returns></returns>
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
		/// <summary>
		/// Determines whether any of the specified properties has been modified.
		/// </summary>
		/// <param name="info"></param>
		/// <returns></returns>
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
