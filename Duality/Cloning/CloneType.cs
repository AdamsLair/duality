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
		public delegate void AssignmentFunc(object source, object target, ICloneOperation operation);
		public delegate void SetupFunc(object source, object target, ICloneTargetSetup setup);
		public delegate void ValueSetupFunc<T>(ref T source, ref T target, ICloneTargetSetup setup) where T : struct;

		public struct CloneField
		{
			private FieldInfo field;
			private CloneType typeInfo;
			private CloneFieldFlags flags;
			private	CloneBehaviorAttribute behavior;
			private bool isAlwaysReference;

			public FieldInfo Field
			{
				get { return this.field; }
			}
			public CloneType FieldType
			{
				get { return this.typeInfo; }
			}
			public CloneFieldFlags Flags
			{
				get { return this.flags; }
			}
			public CloneBehaviorAttribute Behavior
			{
				get { return this.behavior; }
			}
			public bool IsAlwaysReference
			{
				get { return this.isAlwaysReference; }
			}

			public CloneField(FieldInfo field, CloneType typeInfo, CloneFieldFlags flags, CloneBehaviorAttribute behavior, bool isAlwaysReference)
			{
				this.field = field;
				this.typeInfo = typeInfo;
				this.flags = flags;
				this.behavior = behavior;
				this.isAlwaysReference = isAlwaysReference;
			}
			public override string ToString()
			{
				return string.Format("Field {0}", Log.FieldInfo(this.field, false));
			}
		}

		private	Type				type;
		private	CloneType			elementType;
		private	CloneField[]		fieldData;
		private	bool				plainOldData;
		private	bool				investigateOwnership;
		private	CloneBehavior		behavior;
		private	ICloneSurrogate		surrogate;
		private	AssignmentFunc		assignPodFunc;
		private	SetupFunc			setupFunc;
		private	Delegate			valueSetupFunc;

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
		/// [GET] Specifies whether this Type requires any ownership handling, i.e. contains children or weak references.
		/// </summary>
		public bool InvestigateOwnership
		{
			get { return this.investigateOwnership; }
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
		public AssignmentFunc PrecompiledAssignmentFunc
		{
			get { return this.assignPodFunc; }
		}
		public SetupFunc PrecompiledSetupFunc
		{
			get { return this.setupFunc; }
		}
		public Delegate PrecompiledValueSetupFunc
		{
			get { return this.valueSetupFunc; }
		}

		/// <summary>
		/// Creates a new CloneType based on a <see cref="System.Type"/>, gathering all the information that is necessary for cloning.
		/// </summary>
		/// <param name="type"></param>
		public CloneType(Type type)
		{
			this.type = type;
			this.plainOldData =
				this.type.IsPlainOldData() ||
				typeof(MemberInfo).IsAssignableFrom(this.type); /* Handle MemberInfo like POD */ 
			this.investigateOwnership = !this.plainOldData;
			this.surrogate = CloneProvider.GetSurrogateFor(this.type);
			if (this.type.IsArray) this.elementType = CloneProvider.GetCloneType(this.type.GetElementType());

			CloneBehaviorAttribute defaultBehaviorAttrib = CloneProvider.GetCloneBehaviorAttribute(this.type);
			if (defaultBehaviorAttrib != null && defaultBehaviorAttrib.Behavior != CloneBehavior.Default)
				this.behavior = defaultBehaviorAttrib.Behavior;
			else
				this.behavior = CloneBehavior.ChildObject;
		}

		public void Init()
		{
			if (this.surrogate != null) return;
			if (this.plainOldData) return;

			if (this.type.IsArray)
			{
				this.investigateOwnership = !(this.elementType.IsPlainOldData || (this.elementType.Type.IsValueType && !this.elementType.InvestigateOwnership));
				return;
			}
			else
			{
				this.investigateOwnership = typeof(ICloneExplicit).IsAssignableFrom(this.type) || this.surrogate != null;
			}

			// Retrieve field data
			List<CloneField> fieldData = new List<CloneField>();
			foreach (FieldInfo field in this.type.GetAllFields(ReflectionHelper.BindInstanceAll))
			{
				if (field.GetCustomAttributes<ManuallyClonedAttribute>().Any()) continue;
				if (field.DeclaringType.GetCustomAttributes<ManuallyClonedAttribute>().Any()) continue;

				CloneFieldFlags flags = CloneFieldFlags.None;
				CloneFieldAttribute fieldAttrib = field.GetCustomAttributes<CloneFieldAttribute>().FirstOrDefault();
				if (fieldAttrib != null) flags = fieldAttrib.Flags;

				if (field.IsNotSerialized && !flags.HasFlag(CloneFieldFlags.DontSkip))
					continue;
				if (flags.HasFlag(CloneFieldFlags.Skip))
					continue;

				CloneBehaviorAttribute behaviorAttrib = field.GetCustomAttributes<CloneBehaviorAttribute>().FirstOrDefault();
				CloneType fieldType = CloneProvider.GetCloneType(field.FieldType);
				bool isAlwaysReference = 
					(behaviorAttrib != null) && 
					(behaviorAttrib.TargetType == null || field.FieldType.IsAssignableFrom(behaviorAttrib.TargetType)) &&
					(behaviorAttrib.Behavior == CloneBehavior.Reference);

				// Can this field own any objects itself?
				if (!this.investigateOwnership)
				{
					bool fieldCanOwnObjects = true;
					if (fieldType.IsPlainOldData)
						fieldCanOwnObjects = false;
					if (isAlwaysReference)
						fieldCanOwnObjects = false;
					if (fieldType.Type.IsValueType && !fieldType.InvestigateOwnership)
						fieldCanOwnObjects = false;

					if (fieldCanOwnObjects)
						this.investigateOwnership = true;
				}

				CloneField fieldEntry = new CloneField(field, fieldType, flags, behaviorAttrib, isAlwaysReference);
				fieldData.Add(fieldEntry);
			}
			this.fieldData = fieldData.ToArray();

			// Build precompile functions for setup and (partially) assignment
			this.CompileAssignmentFunc();
			this.CompileSetupFunc();
			this.CompileValueSetupFunc();
		}
		private void CompileAssignmentFunc()
		{
			if (this.surrogate != null) return;
			if (this.type.IsValueType) return;
			if (this.fieldData.Length == 0) return;

			List<Expression> mainBlock = new List<Expression>();
			ParameterExpression sourceParameter = Expression.Parameter(typeof(object), "source");
			ParameterExpression targetParameter = Expression.Parameter(typeof(object), "target");
			ParameterExpression operationParameter = Expression.Parameter(typeof(ICloneOperation), "operation");
			ParameterExpression sourceCastVar = Expression.Variable(type, "sourceCast");
			ParameterExpression targetCastVar = Expression.Variable(type, "targetCast");
			mainBlock.Add(Expression.Assign(sourceCastVar, type.IsValueType ? Expression.Convert(sourceParameter, this.type) : Expression.TypeAs(sourceParameter, this.type)));
			mainBlock.Add(Expression.Assign(targetCastVar, type.IsValueType ? Expression.Convert(targetParameter, this.type) : Expression.TypeAs(targetParameter, this.type)));
			for (int i = 0; i < this.fieldData.Length; i++)
			{
				FieldInfo field = this.fieldData[i].Field;
				Expression assignment;

				if (this.fieldData[i].FieldType.IsPlainOldData)
				{
					assignment = Expression.Assign(
						Expression.Field(targetCastVar, field), 
						Expression.Field(sourceCastVar, field));
				}
				else if (this.fieldData[i].FieldType.Type.IsValueType)
				{
					assignment = Expression.Call(operationParameter, 
						CopyHandleValue.MakeGenericMethod(field.FieldType), 
						Expression.Field(sourceCastVar, field), 
						Expression.Field(targetCastVar, field));
				}
				else
				{
					assignment = Expression.Call(operationParameter, 
						CopyHandleObject.MakeGenericMethod(field.FieldType), 
						Expression.Field(sourceCastVar, field), 
						Expression.Field(targetCastVar, field));
				}

				if ((this.fieldData[i].Flags & CloneFieldFlags.IdentityRelevant) != CloneFieldFlags.None)
				{
					assignment = Expression.IfThen(
						Expression.Not(Expression.Property(Expression.Property(operationParameter, "Context"), "PreserveIdentity")),
						assignment);
				}
				mainBlock.Add(assignment);
			}

			Expression mainBlockExpression = Expression.Block(new[] { sourceCastVar, targetCastVar }, mainBlock);
			this.assignPodFunc = Expression.Lambda<AssignmentFunc>(mainBlockExpression, sourceParameter, targetParameter, operationParameter).Compile();
		}
		private void CompileSetupFunc()
		{
			if (this.surrogate != null) return;
			if (this.fieldData.Length == 0) return;
			if (!this.investigateOwnership) return;

			ParameterExpression sourceParameter = Expression.Parameter(typeof(object), "source");
			ParameterExpression targetParameter = Expression.Parameter(typeof(object), "target");
			ParameterExpression setupParameter = Expression.Parameter(typeof(ICloneTargetSetup), "setup");

			ParameterExpression sourceCastVar = Expression.Variable(type, "sourceCast");
			ParameterExpression targetCastVar = Expression.Variable(type, "targetCast");

			List<Expression> mainBlock = this.CreateSetupFuncContent(setupParameter, sourceCastVar, targetCastVar);
			if (mainBlock == null) return;

			mainBlock.Insert(0, Expression.Assign(sourceCastVar, type.IsValueType ? Expression.Convert(sourceParameter, this.type) : Expression.TypeAs(sourceParameter, this.type)));
			mainBlock.Insert(1, Expression.Assign(targetCastVar, type.IsValueType ? Expression.Convert(targetParameter, this.type) : Expression.TypeAs(targetParameter, this.type)));
			Expression mainBlockExpression = Expression.Block(new[] { sourceCastVar, targetCastVar }, mainBlock);
			this.setupFunc = Expression.Lambda<SetupFunc>(mainBlockExpression, sourceParameter, targetParameter, setupParameter).Compile();
		}
		private void CompileValueSetupFunc()
		{
			if (!this.type.IsValueType) return;
			if (this.surrogate != null) return;
			if (this.fieldData.Length == 0) return;
			if (!this.investigateOwnership) return;

			ParameterExpression sourceParameter = Expression.Parameter(this.type.MakeByRefType(), "source");
			ParameterExpression targetParameter = Expression.Parameter(this.type.MakeByRefType(), "target");
			ParameterExpression setupParameter = Expression.Parameter(typeof(ICloneTargetSetup), "setup");

			List<Expression> mainBlock = this.CreateSetupFuncContent(setupParameter, sourceParameter, targetParameter);
			if (mainBlock == null) return;

			Expression mainBlockExpression = Expression.Block(mainBlock);
			this.valueSetupFunc = Expression.Lambda(typeof(ValueSetupFunc<>).MakeGenericType(this.type), mainBlockExpression, sourceParameter, targetParameter, setupParameter).Compile();
		}
		private List<Expression> CreateSetupFuncContent(Expression setup, Expression source, Expression target)
		{
			List<Expression> mainBlock = new List<Expression>();
			bool anyContent = false;
			for (int i = 0; i < this.fieldData.Length; i++)
			{
				// Don't need to scan "plain old data" and reference fields
				if (this.fieldData[i].FieldType.IsPlainOldData) continue;
				if (this.fieldData[i].IsAlwaysReference) continue;
				if (this.fieldData[i].FieldType.Type.IsValueType && !this.fieldData[i].FieldType.InvestigateOwnership) continue;
				anyContent = true;

				// Call HandleObject on the fields value
				CloneBehaviorAttribute behaviorAttribute = this.fieldData[i].Behavior;
				FieldInfo field = this.fieldData[i].Field;
				Expression handleObjectExpression;
				if (this.fieldData[i].FieldType.Type.IsValueType)
				{
					if (behaviorAttribute == null)
					{
						handleObjectExpression = Expression.Call(setup, 
							SetupHandleValue.MakeGenericMethod(field.FieldType), 
							Expression.Field(source, field), 
							Expression.Field(target, field),
							Expression.Constant(CloneBehavior.Default),
							Expression.Constant(null, typeof(Type)));
					}
					else
					{
						handleObjectExpression = Expression.Call(setup, 
							SetupHandleValue.MakeGenericMethod(field.FieldType), 
							Expression.Field(source, field), 
							Expression.Field(target, field), 
							Expression.Constant(behaviorAttribute.Behavior), 
							Expression.Constant(behaviorAttribute.TargetType));
					}
				}
				else
				{
					if (behaviorAttribute == null)
					{
						handleObjectExpression = Expression.Call(setup, 
							SetupHandleObject.MakeGenericMethod(field.FieldType), 
							Expression.Field(source, field), 
							Expression.Field(target, field),
							Expression.Constant(CloneBehavior.Default),
							Expression.Constant(null, typeof(Type)));
					}
					else if (behaviorAttribute.TargetType == null || field.FieldType.IsAssignableFrom(behaviorAttribute.TargetType))
					{
						handleObjectExpression = Expression.Call(setup, 
							SetupHandleObject.MakeGenericMethod(field.FieldType), 
							Expression.Field(source, field), 
							Expression.Field(target, field), 
							Expression.Constant(behaviorAttribute.Behavior),
							Expression.Constant(null, typeof(Type)));
					}
					else
					{
						handleObjectExpression = Expression.Call(setup, 
							SetupHandleObject.MakeGenericMethod(field.FieldType), 
							Expression.Field(source, field), 
							Expression.Field(target, field), 
							Expression.Constant(behaviorAttribute.Behavior), 
							Expression.Constant(behaviorAttribute.TargetType));
					}
				}
				mainBlock.Add(handleObjectExpression);
			}
			if (!anyContent) return null;
			return mainBlock;
		}

		public override string ToString()
		{
			return string.Format("CloneType {0}", Log.Type(this.type));
		}

		private static readonly MethodInfo SetupHandleObject = typeof(ICloneTargetSetup).GetMethod("HandleObject");
		private static readonly MethodInfo SetupHandleValue = typeof(ICloneTargetSetup).GetMethod("HandleValue");
		private static readonly MethodInfo CopyHandleObject = typeof(ICloneOperation).GetMethod("HandleObject");
		private static readonly MethodInfo CopyHandleValue = typeof(ICloneOperation).GetMethod("HandleValue");
	}
}
