using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Duality;
using Duality.Serialization;
using Duality.Drawing;
using Duality.Resources;

namespace Duality.Plugins.Compatibility
{
	public class BatchInfoInternalsErrorHandler : SerializeErrorHandler
	{
		public override void HandleError(SerializeError error)
		{
			AssignFieldError assignFieldError = error as AssignFieldError;
			ResolveTypeError resolveTypeError = error as ResolveTypeError;

			// BatchInfo uniforms and textures were moved to a nested ShaderParameters
			if (assignFieldError != null)
			{
				BatchInfo batchInfo = assignFieldError.TargetObject as BatchInfo;
				if (batchInfo != null)
				{
					if (assignFieldError.FieldName == "uniforms")
					{
						Dictionary<string,float[]> uniforms = assignFieldError.FieldValue as Dictionary<string,float[]>;
						if (uniforms != null)
						{
							foreach (var pair in uniforms)
							{
								batchInfo.SetArray(pair.Key, pair.Value);
							}
							assignFieldError.AssignSuccess = true;
						}
					}
					else if (assignFieldError.FieldName == "textures")
					{
						Dictionary<string,ContentRef<Texture>> textures = assignFieldError.FieldValue as Dictionary<string,ContentRef<Texture>>;
						if (textures != null)
						{
							foreach (var pair in textures)
							{
								ContentRef<Texture> texRef = pair.Value;
								texRef.EnsureLoaded();
								batchInfo.SetTexture(pair.Key, texRef);
							}
							assignFieldError.AssignSuccess = true;
						}
					}
				}
			}

			return;
		}
	}
}
