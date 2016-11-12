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
	public class BatchInfoFromMaterial : DataConverter
	{
		public override Type TargetType
		{
			get { return typeof(BatchInfo); }
		}

		public override bool CanConvertFrom(ConvertOperation convert)
		{
			return 
				convert.AllowedOperations.HasFlag(ConvertOperation.Operation.Convert) && 
				convert.CanPerform<Material>();
		}
		public override bool Convert(ConvertOperation convert)
		{
			bool finishConvertOp = false;

			List<Material> availData = convert.Perform<Material>().ToList();

			// Append objects
			foreach (Material mat in availData)
			{
				if (convert.IsObjectHandled(mat)) continue;

				convert.AddResult(new BatchInfo(mat));
				finishConvertOp = true;
				convert.MarkObjectHandled(mat);
			}

			return finishConvertOp;
		}
	}
}
