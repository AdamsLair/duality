using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Duality;
using Duality.Serialization;
using Duality.Resources;

namespace Duality.Plugins.Compatibility
{
	public class RenamedV3TypesErrorHandler : SerializeErrorHandler
	{
		private enum ObsoleteEnum { };

		public override void HandleError(SerializeError error)
		{
			ResolveTypeError resolveTypeError = error as ResolveTypeError;
			ResolveMemberError resolveMemberError = error as ResolveMemberError;

			// From v2 to v3, a lot of Duality types were renamed or moved
			if (resolveTypeError != null)
			{
				switch (resolveTypeError.TypeId)
				{
					case "Duality.Resources.BatchInfo": resolveTypeError.ResolvedType = typeof(Duality.Drawing.BatchInfo); break;
					case "Duality.Resources.BatchInfo+DirtyFlag": resolveTypeError.ResolvedType = typeof(ObsoleteEnum); break;
					case "Duality.Resources.Pixmap+Layer": resolveTypeError.ResolvedType = typeof(Duality.Drawing.PixelData); break;
					case "Duality.Drawing.IRendererVisibilityStrategy": resolveTypeError.ResolvedType = typeof(Duality.Components.IRendererVisibilityStrategy); break;
					case "Duality.Drawing.DefaultRendererVisibilityStrategy": resolveTypeError.ResolvedType = typeof(Duality.Components.DefaultRendererVisibilityStrategy); break;
					case "Duality.Components.Renderers.AnimSpriteRenderer": resolveTypeError.ResolvedType = typeof(Duality.Components.Renderers.SpriteRenderer); break;
				}
			}
			else if (resolveMemberError != null)
			{
				switch (resolveMemberError.MemberId)
				{
					case "P:Duality.Components.Transform:RelativePos": resolveMemberError.ResolvedMember = typeof(Duality.Components.Transform).GetRuntimeProperty("LocalPos"); break;
					case "P:Duality.Components.Transform:RelativeAngle": resolveMemberError.ResolvedMember = typeof(Duality.Components.Transform).GetRuntimeProperty("LocalAngle"); break;
					case "P:Duality.Components.Transform:RelativeScale": resolveMemberError.ResolvedMember = typeof(Duality.Components.Transform).GetRuntimeProperty("LocalScale"); break;
				}
			}

			return;
		}
	}
}
