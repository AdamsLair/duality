using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Duality;
using Duality.Serialization;

namespace Duality.Plugins.Compatibility
{
	public class MovedTextureSizeModeErrorHandler : SerializeErrorHandler
	{
		public override void HandleError(SerializeError error)
		{
			ResolveTypeError resolveTypeError = error as ResolveTypeError;
			if (resolveTypeError != null)
			{
				string fixedTypeId = resolveTypeError.TypeId.Replace('+', '.');
				if (fixedTypeId.EndsWith("Duality.Resources.Texture.SizeMode"))
				{
					resolveTypeError.ResolvedType = typeof(Duality.Drawing.TextureSizeMode);
				}
			}

			return;
		}
	}
}
