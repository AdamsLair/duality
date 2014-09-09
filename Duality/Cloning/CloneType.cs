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
			private bool isReference;
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
			public bool IsReference
			{
				get { return this.isReference; }
			}
			public bool AllowPlainOldDataShortcut
			{
				get { return this.allowPodShortcut; }
			}

			public CloneField(FieldInfo field, CloneFieldFlags flags, CloneBehaviorAttribute behavior, bool isPlainOld, bool shortcut, bool isReference)
			{
				this.field = field;
				this.flags = flags;
				this.behavior = behavior;
				this.isPlainOldData = isPlainOld;
				this.isReference = isReference;
				this.allowPodShortcut = shortcut;
			}
		}

		private	Type			type;
		private	CloneType		elementType;
		private	CloneField[]	fieldData;
		private	bool			plainOldData;
		private	bool			canContainChildren;
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
		/// [GET] Returns whether the cached Type could contain child objects that need to be investiaged building the ownership graph.
		/// </summary>
		public bool CanContainChildren
		{
			get { return this.canContainChildren; }
		}
		/// <summary>
		/// [GET] Returns whether the cached type is handled by a <see cref="ICloneSurrogate.RequireMerge">merge surrogate</see>.
		/// </summary>
		public bool IsMergeSurrogate
		{
			get { return this.surrogate != null && this.surrogate.RequireMerge; }
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
			this.canContainChildren = true;
			this.type = type;
			this.plainOldData = this.type.IsPlainOldData();
			this.surrogate = CloneProvider.GetSurrogateFor(this.type);
			if (this.type.IsArray) this.elementType = CloneProvider.GetCloneType(this.type.GetElementType());

			CloneBehaviorAttribute defaultBehaviorAttrib = CloneProvider.GetCloneBehaviorAttribute(this.type);
			if (defaultBehaviorAttrib != null && defaultBehaviorAttrib.Behavior != CloneBehavior.Default)
				this.behavior = defaultBehaviorAttrib.Behavior;
			else
				this.behavior = CloneBehavior.ChildObject;
		}

		public void InitFields()
		{
			if (this.type.IsArray) return;
			if (this.plainOldData) return;

			// Retrieve field data
			List<CloneField> fieldData = new List<CloneField>();
			int shortcutFieldCount = 0;
			this.canContainChildren = false;
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
				CloneType fieldType = CloneProvider.GetCloneType(field.FieldType);
				bool affectedByLocalBehavior = 
					(behaviorAttrib != null) && 
					(behaviorAttrib.TargetType == null || field.FieldType.IsAssignableFrom(behaviorAttrib.TargetType)) && 
					behaviorAttrib.Behavior != fieldType.DefaultCloneBehavior;
				bool isPlainOld = field.FieldType.IsPlainOldData();
				bool isReference = (fieldType.DefaultCloneBehavior == CloneBehavior.Reference) && !affectedByLocalBehavior;
				bool allowShortcut = isPlainOld && (flags & ~CloneFieldFlags.DontSkip) == CloneFieldFlags.None;

				if (!isPlainOld && !isReference) this.canContainChildren = true;

				fieldData.Add(new CloneField(field, flags, behaviorAttrib, isPlainOld, allowShortcut, isReference));
				if (allowShortcut) ++shortcutFieldCount;
			}
			this.fieldData = fieldData.ToArray();

			// Build a shortcut expression to copy all the plain old data fields without reflection
			if (shortcutFieldCount > 1 && this.surrogate == null && !this.type.IsValueType)
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
	}
}
