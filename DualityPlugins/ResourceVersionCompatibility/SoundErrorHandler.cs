using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Duality;
using Duality.Resources;
using Duality.Serialization;

namespace Duality.Plugins.Compatibility
{
	public class SoundErrorHandler : SerializeErrorHandler
	{
		public override void HandleError(SerializeError error)
		{
			AssignFieldError fieldError = error as AssignFieldError;
			if (fieldError != null && fieldError.TargetObjectType.Type == typeof(Sound))
			{
				Sound targetObject = fieldError.TargetObject as Sound;
				if (targetObject != null)
				{
					// ContentRef<AudioData> audioData		-->		List<ContentRef<AudioData>> audioData
					if (fieldError.FieldName == "audioData" && fieldError.FieldValue is ContentRef<AudioData>)
					{
						targetObject.MainData = (ContentRef<AudioData>)fieldError.FieldValue;
						fieldError.AssignSuccess = true;
					}
				}
			}

			return;
		}
	}
}
