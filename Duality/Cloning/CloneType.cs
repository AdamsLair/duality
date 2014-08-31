using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Linq.Expressions;

namespace Duality.Cloning
{
	/// <summary>
	/// The CloneType class provides cached cloning-relevant information
	/// that has been generated basing on a <see cref="System.Type"/>.
	/// </summary>
	public sealed class CloneType
	{
		public delegate void AssignPodFunc(object source, object target);

		public struct CloneField
		{
			private FieldInfo field;
			private CloneFieldFlags flags;
			private	CloneBehaviorAttribute behavior;
			private bool isPlainOldData;
			private bool allowPodShortcut;

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
			public bool AllowPlainOldDataShortcut
			{
				get { return this.allowPodShortcut; }
			}

			public CloneField(FieldInfo field, CloneFieldFlags flags, CloneBehaviorAttribute behavior, bool isPlainOld, bool shortcut)
			{
				this.field = field;
				this.flags = flags;
				this.behavior = behavior;
				this.isPlainOldData = isPlainOld;
				this.allowPodShortcut = shortcut;
			}
		}

		private	Type			type;
		private	CloneType		elementType;
		private	CloneField[]	fieldData;
		private	bool			plainOldData;
		private	CloneBehavior	behavior;
		private	ICloneSurrogate	surrogate;
		private	AssignPodFunc	assignPodFunc;

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
		/// [GET] Returns the default <see cref="CloneBehavior"/> exposed by this type.
		/// </summary>
		public CloneBehavior DefaultCloneBehavior
		{
			get { return this.behavior; }
		}
		/// <summary>
		/// [GET] The surrogate that will handle this types cloning operations.
		/// </summary>
		public ICloneSurrogate Surrogate
		{
			get { return this.surrogate; }
		}
		/// <summary>
		/// [GET] When available, this property returns a compiled lambda function that assigns all plain old data fields of this Type
		/// </summary>
		public AssignPodFunc AssignPlainOldDataFunc
		{
			get { return this.assignPodFunc; }
		}

		/// <summary>
		/// Creates a new CloneType based on a <see cref="System.Type"/>, gathering all the information that is necessary for cloning.
		/// </summary>
		/// <param name="type"></param>
		public CloneType(Type type)
		{
			this.type = type;
			this.plainOldData = this.type.IsPlainOldData();
			this.surrogate = CloneProvider.GetSurrogateFor(this.type);
			if (this.type.IsArray) this.elementType = CloneProvider.GetCloneType(this.type.GetElementType());

			if (!this.type.IsArray && !this.plainOldData)
			{
				List<CloneField> fieldData = new List<CloneField>();
				int podFieldCount = 0;
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
					bool allowShortcut = isPlainOld && (flags & ~CloneFieldFlags.DontSkip) == CloneFieldFlags.None;

					fieldData.Add(new CloneField(field, flags, behaviorAttrib, isPlainOld, allowShortcut));
					if (allowShortcut) ++podFieldCount;
				}
				this.fieldData = fieldData.ToArray();

				if (podFieldCount > 1 && this.surrogate == null && !this.type.IsValueType)
				{
					List<Expression> mainBlock = new List<Expression>();
					ParameterExpression sourceParameter = Expression.Parameter(typeof(object), "source");
					ParameterExpression targetParameter = Expression.Parameter(typeof(object), "target");
					ParameterExpression sourceCastVar = Expression.Variable(type, "sourceCast");
					ParameterExpression targetCastVar = Expression.Variable(type, "targetCast");
					mainBlock.Add(Expression.Assign(sourceCastVar, type.IsValueType ? Expression.Convert(sourceParameter, type) : Expression.TypeAs(sourceParameter, type)));
					mainBlock.Add(Expression.Assign(targetCastVar, type.IsValueType ? Expression.Convert(targetParameter, type) : Expression.TypeAs(targetParameter, type)));
					for (int i = 0; i < this.fieldData.Length; i++)
					{
						if (!this.fieldData[i].IsPlainOldData) continue;
						if (!this.fieldData[i].AllowPlainOldDataShortcut) continue;

						FieldInfo field = this.fieldData[i].Field;
						Expression assignment = Expression.Assign(Expression.Field(targetCastVar, field), Expression.Field(sourceCastVar, field));
						mainBlock.Add(assignment);
					}
					Expression mainBlockExpression = Expression.Block(new[] { sourceCastVar, targetCastVar }, mainBlock);
					this.assignPodFunc = Expression.Lambda<AssignPodFunc>(mainBlockExpression, sourceParameter, targetParameter).Compile();
				}
			}
			else
			{
				this.fieldData = null;
			}

			CloneBehaviorAttribute defaultBehaviorAttrib = CloneProvider.GetCloneBehaviorAttribute(this.type);
			this.behavior = (defaultBehaviorAttrib != null) ? defaultBehaviorAttrib.Behavior : CloneBehavior.ChildObject;
		}
	}
}
