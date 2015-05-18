using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

using Duality;
using Duality.Components;
using Duality.Components.Renderers;
using Duality.Resources;
using Duality.Drawing;
using Duality.Editor;


namespace Duality.Editor.Plugins.Base.DataConverters
{
	public class AudioDataFromSound : DataConverter
	{
		public override Type TargetType
		{
			get { return typeof(AudioData); }
		}

		public override bool CanConvertFrom(ConvertOperation convert)
		{
			return 
				convert.AllowedOperations.HasFlag(ConvertOperation.Operation.Convert) && 
				convert.CanPerform<Sound>();
		}
		public override bool Convert(ConvertOperation convert)
		{
			bool finishConvertOp = false;

			List<Sound> availData = convert.Perform<Sound>().ToList();

			// Append objects
			foreach (Sound baseRes in availData)
			{
				if (convert.IsObjectHandled(baseRes)) continue;
				if (baseRes.Data != null)
				{
					for (int i = 0; i < baseRes.Data.Count; i++)
					{
						if (!baseRes.Data[i].IsAvailable) continue;
						convert.AddResult(baseRes.Data[i].Res);
						finishConvertOp = true;
					}
				}
				convert.MarkObjectHandled(baseRes);
			}

			return finishConvertOp;
		}
	}
}
