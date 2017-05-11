using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Duality;
using Duality.Serialization;
using Duality.Components.Physics;

namespace Duality.Plugins.Compatibility
{
	public class InternalFieldRenameErrorhandler : SerializeErrorHandler
	{
		public override void HandleError(SerializeError error)
		{
			AssignFieldError assignFieldError = error as AssignFieldError;
			if (assignFieldError == null) return;

			// RigidBody."continous" was renamed to "useCCD"
			RigidBody body = assignFieldError.TargetObject as RigidBody;
			if (body != null && assignFieldError.FieldName == "continous")
			{
				body.ContinousCollision = (bool)assignFieldError.FieldValue;
				assignFieldError.AssignSuccess = true;
			}

			return;
		}
	}
}
