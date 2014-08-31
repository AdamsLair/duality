using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Duality.Cloning
{
	/// <summary>
	/// The CloneType class provides cached cloning-relevant information
	/// that has been generated basing on a <see cref="System.Type"/>.
	/// </summary>
	public sealed class CloneType
	{
		public struct CloneField
		{
			private FieldInfo field;
			private CloneFieldFlags flags;
			private	CloneBehaviorAttribute behavior;

			public FieldInfo Field
			{
				get { return this.field; }
			}
			public CloneFieldFlags Flags
			{
				get { return this.flags; }
			}
			public CloneBehaviorAttribute Behavior
			{
				get { return this.behavior; }
			}

			public CloneField(FieldInfo field, CloneFieldFlags flags, CloneBehaviorAttribute behavior)
			{
				this.field = field;
				this.flags = flags;
				this.behavior = behavior;
			}
		}

		private	Type			type;
		private	CloneField[]	fieldData;
		private	bool			plainOldData;

		/// <summary>
		/// [GET] The <see cref="System.Type"/> that is described.
		/// </summary>
		public Type Type
		{
			get { return this.type; }
		}
		/// <summary>
		/// [GET] An array of <see cref="System.Reflection.FieldInfo">fields</see> which are cloned.
		/// </summary>
		public CloneField[] FieldData
		{
			get { return this.fieldData; }
		}
		/// <summary>
		/// [GET] Specifies whether this Type can be considered plain old data, i.e. can be cloned by assignment.
		/// </summary>
		public bool IsPlainOldData
		{
			get { return this.plainOldData; }
		}

		/// <summary>
		/// Creates a new CloneType based on a <see cref="System.Type"/>, gathering all the information that is necessary for cloning.
		/// </summary>
		/// <param name="t"></param>
		public CloneType(Type t)
		{
			this.type = t;

			List<CloneField> fieldData = new List<CloneField>();
			foreach (FieldInfo field in this.type.GetAllFields(ReflectionHelper.BindInstanceAll))
			{
				CloneFieldFlags flags = CloneFieldFlags.None;
				CloneFieldAttribute fieldAttrib = field.GetCustomAttributes<CloneFieldAttribute>().FirstOrDefault();
				if (fieldAttrib != null) flags = fieldAttrib.Flags;

				if (field.IsNotSerialized && !flags.HasFlag(CloneFieldFlags.DontSkip))
					continue;
				if (flags.HasFlag(CloneFieldFlags.Skip))
					continue;

				CloneBehaviorAttribute behaviorAttrib = field.GetCustomAttributes<CloneBehaviorAttribute>().FirstOrDefault();

				fieldData.Add(new CloneField(field, flags, behaviorAttrib));
			}
			this.fieldData = fieldData.ToArray();
			this.plainOldData = this.type.IsPlainOldData();
		}
	}
}
