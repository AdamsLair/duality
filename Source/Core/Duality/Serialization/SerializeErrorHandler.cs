using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Duality.Serialization
{
	/// <summary>
	/// An abstract base class for errors that can occur during serialization to provide
	/// an interface for custom serialization fallback behaivor.
	/// </summary>
	public abstract class SerializeError
	{
		protected bool handled = false;

		/// <summary>
		/// [GET] Returns whether the error has been handled successfully.
		/// </summary>
		public bool Handled
		{
			get { return this.handled; }
		}
	}
	
	/// <summary>
	/// A serialization error that occurred during the resolve operation of a Type.
	/// </summary>
	public class ResolveTypeError : SerializeError
	{
		private string typeId = null;
		private Type resolvedType = null;
		
		/// <summary>
		/// [GET] The Type id to resolve.
		/// </summary>
		public string TypeId
		{
			get { return this.typeId; }
		}
		/// <summary>
		/// [GET / SET] The resolved Type.
		/// </summary>
		public Type ResolvedType
		{
			get { return this.resolvedType; }
			set
			{
				this.resolvedType = value;
				this.handled = (this.resolvedType != null);
			}
		}

		public ResolveTypeError(string typeId)
		{
			this.typeId = typeId;
		}
	}
	
	/// <summary>
	/// A serialization error that occurred during the resolve operation of a Member.
	/// </summary>
	public class ResolveMemberError : SerializeError
	{
		private string memberId = null;
		private MemberInfo resolvedMember = null;

		/// <summary>
		/// [GET] The Member id to resolve.
		/// </summary>
		public string MemberId
		{
			get { return this.memberId; }
		}
		/// <summary>
		/// [GET / SET] The resolved Member.
		/// </summary>
		public MemberInfo ResolvedMember
		{
			get { return this.resolvedMember; }
			set
			{
				this.resolvedMember = value;
				this.handled = (this.resolvedMember != null);
			}
		}

		public ResolveMemberError(string memberId)
		{
			this.memberId = memberId;
		}
	}

	/// <summary>
	/// A serialization error that occurred during the assignment of a Field. Possible causes are the Field not being
	/// available, being flagged as [DontSerialize] or having the wrong FieldType.
	/// </summary>
	public class AssignFieldError : SerializeError
	{
		private SerializeType targetObjType;
		private object targetObj;
		private string fieldName;
		private object fieldValue;

		/// <summary>
		/// [GET] The target objects serialization Type data.
		/// </summary>
		public SerializeType TargetObjectType
		{
			get { return this.targetObjType; }
		}
		/// <summary>
		/// [GET] Object on which the field value should be assigned.
		/// </summary>
		public object TargetObject
		{
			get { return this.targetObj; }
		}
		/// <summary>
		/// [GET] Name of the field to assign the value to.
		/// </summary>
		public string FieldName
		{
			get { return this.fieldName; }
		}
		/// <summary>
		/// [GET] The value to assign.
		/// </summary>
		public object FieldValue
		{
			get { return this.fieldValue; }
		}
		/// <summary>
		/// [GET / SET] Whether or not the assignment was a success. Set this property to true, if you handled the error successfully.
		/// </summary>
		public bool AssignSuccess
		{
			get { return this.handled; }
			set { this.handled = value; }
		}

		public AssignFieldError(SerializeType targetObjType, object targetObj, string fieldName, object fieldValue)
		{
			this.targetObjType = targetObjType;
			this.targetObj = targetObj;
			this.fieldName = fieldName;
			this.fieldValue = fieldValue;
		}
	}

	/// <summary>
	/// Provides an abstract interface for manual serialization fallbacks to resolve Types and Members or assign Fields.
	/// </summary>
	public abstract class SerializeErrorHandler
	{
		/// <summary>
		/// The handlers priority. A higher value makes it more significant above others.
		/// </summary>
		public virtual int Priority
		{
			get { return 0; }
		}

		/// <summary>
		/// Handles the specified error.
		/// </summary>
		/// <param name="error"></param>
		public abstract void HandleError(SerializeError error);
	}
}
