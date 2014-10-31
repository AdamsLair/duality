using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Duality;
using Duality.Components.Physics;
using Duality.Serialization;

namespace Duality.Plugins.Compatibility
{
	public class RigidBodyJointErrorHandler : SerializeErrorHandler
	{
		public override void HandleError(SerializeError error)
		{
			AssignFieldError fieldError = error as AssignFieldError;
			if (fieldError != null && fieldError.TargetObject is JointInfo)
			{
				RigidBody target = fieldError.FieldValue as RigidBody;
				JointInfo joint = fieldError.TargetObject as JointInfo;

				bool other = false;
				FieldInfo field = null;
				if (fieldError.FieldName == "colA")
				{
					field = fieldError.TargetObjectType.Fields.FirstOrDefault(f => f.Name == "parentBody");
				}
				else if (fieldError.FieldName == "colB")
				{
					field = fieldError.TargetObjectType.Fields.FirstOrDefault(f => f.Name == "otherBody");
					other = true;
				}

				if (field != null)
				{
					if (other)
					{
						FieldInfo jointsField = typeof(RigidBody).GetField("joints", BindingFlags.Instance | BindingFlags.NonPublic);
						IList<JointInfo> jointList = jointsField.GetValue(target) as IList<JointInfo>;
						jointList.Remove(joint);
					}

					field.SetValue(fieldError.TargetObject, fieldError.FieldValue);
					fieldError.AssignSuccess = true;
				}
			}

			return;
		}
	}
}
