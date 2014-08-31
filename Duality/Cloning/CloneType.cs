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
			private bool isPlainOldData;

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
			public bool IsPlainOldData
			{
				get { return this.isPlainOldData; }
			}

			public CloneField(FieldInfo field, CloneFieldFlags flags, CloneBehaviorAttribute behavior, bool isPlainOld)
			{
				this.field = field;
				this.flags = flags;
				this.behavior = behavior;
				this.isPlainOldData = isPlainOld;
			}
		}

		private	Type			type;
		private	CloneType		elementType;
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
		/// [GET] Returns whether the encapsulated Type is an array.
		/// </summary>
		public bool IsArray
		{
			get { return this.type.IsArray; }
		}
		/// <summary>
		/// [GET] Returns the elements <see cref="CloneType"/>, if this one is an array.
		/// </summary>
		public CloneType ElementType
		{
			get { return this.elementType; }
		}
		/// <summary>
		/// [GET] Returns whether the cached Type could be derived by others.
		/// </summary>
		public bool CouldBeDerived
		{
			get { return !this.type.IsValueType && !this.type.IsSealed; }
		}

		/// <summary>
		/// Creates a new CloneType based on a <see cref="System.Type"/>, gathering all the information that is necessary for cloning.
		/// </summary>
		/// <param name="type"></param>
		public CloneType(Type type)
		{
			this.type = type;

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
				bool isPlainOld = field.FieldType.IsPlainOldData();

				fieldData.Add(new CloneField(field, flags, behaviorAttrib, isPlainOld));
			}
			this.fieldData = fieldData.ToArray();
			this.plainOldData = this.type.IsPlainOldData();

			if (this.type.IsArray) this.elementType = CloneProvider.GetCloneType(this.type.GetElementType());
		}
	}
}
