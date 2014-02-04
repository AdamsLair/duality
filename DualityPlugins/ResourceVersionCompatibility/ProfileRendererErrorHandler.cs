using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Duality;
using Duality.Components.Diagnostics;
using Duality.Serialization;

namespace ResourceVersionCompatibility
{
	public class ProfileRendererErrorHandler : SerializeErrorHandler
	{
		public override void HandleError(SerializeError error)
		{
			ResolveTypeError resolveTypeError = error as ResolveTypeError;
			if (resolveTypeError != null && resolveTypeError.TypeId.EndsWith("ProfileRenderer"))
			{
				resolveTypeError.ResolvedType = typeof(ProfileRenderer);
			}

			return;
		}
	}
}
