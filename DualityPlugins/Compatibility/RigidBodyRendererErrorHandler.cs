using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Duality;
using Duality.Components.Renderers;
using Duality.Serialization;

namespace Duality.Plugins.Compatibility
{
	public class RigidBodyRendererErrorHandler : SerializeErrorHandler
	{
		public override void HandleError(SerializeError error)
		{
			ResolveTypeError resolveTypeError = error as ResolveTypeError;
			if (resolveTypeError != null && resolveTypeError.TypeId.EndsWith("RigidBodyRenderer"))
			{
				resolveTypeError.ResolvedType = typeof(RigidBodyRenderer);
			}

			return;
		}
	}
}
