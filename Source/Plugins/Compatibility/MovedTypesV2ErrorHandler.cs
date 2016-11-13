using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Duality;
using Duality.Serialization;

namespace Duality.Plugins.Compatibility
{
	public class MovedTypesV2ErrorHandler : SerializeErrorHandler
	{
		public override void HandleError(SerializeError error)
		{
			ResolveTypeError resolveTypeError = error as ResolveTypeError;
			if (resolveTypeError != null)
			{
				string fixedTypeId = resolveTypeError.TypeId.Replace('+', '.');

				if (fixedTypeId.EndsWith("Duality.Resources.Texture.SizeMode"))
					resolveTypeError.ResolvedType = typeof(Duality.Drawing.TextureSizeMode);
				else if (fixedTypeId.EndsWith("Duality.Resources.BatchInfo"))
					resolveTypeError.ResolvedType = typeof(Duality.Drawing.BatchInfo);
				else if (fixedTypeId.EndsWith("Duality.Resources.BatchInfo.DirtyFlag"))
					resolveTypeError.ResolvedType = typeof(Duality.Drawing.BatchInfo).GetTypeInfo().DeclaredNestedTypes.FirstOrDefault(t => t.Name == "DirtyFlag").AsType();
				else if (fixedTypeId.EndsWith("Duality.Drawing.DefaultRendererVisibilityStrategy"))
					resolveTypeError.ResolvedType = typeof(Duality.Components.DefaultRendererVisibilityStrategy);
				else if (fixedTypeId.EndsWith("Duality.Resources.Pixmap.Layer"))
					resolveTypeError.ResolvedType = typeof(Duality.Drawing.PixelData);
			}

			return;
		}
	}
}
