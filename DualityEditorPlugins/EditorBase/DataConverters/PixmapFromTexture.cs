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
	public class PixmapFromTexture : DataConverter
	{
		public override Type TargetType
		{
			get { return typeof(Pixmap); }
		}

		public override bool CanConvertFrom(ConvertOperation convert)
		{
			return 
				convert.AllowedOperations.HasFlag(ConvertOperation.Operation.Convert) && 
				convert.CanPerform<Texture>();
		}
		public override bool Convert(ConvertOperation convert)
		{
			bool finishConvertOp = false;

			List<Texture> availData = convert.Perform<Texture>().ToList();

			// Append objects
			foreach (Texture baseRes in availData)
			{
				if (convert.IsObjectHandled(baseRes)) continue;
				if (!baseRes.BasePixmap.IsAvailable) continue;

				convert.AddResult(baseRes.BasePixmap.Res);
				finishConvertOp = true;
				convert.MarkObjectHandled(baseRes);
			}

			return finishConvertOp;
		}
	}
}
