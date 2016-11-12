using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

using Duality;
using Duality.IO;
using Duality.Components;
using Duality.Components.Renderers;
using Duality.Resources;
using Duality.Drawing;
using Duality.Editor;


namespace Duality.Editor.Plugins.Base.DataConverters
{
	public class MaterialFromBatchInfo : DataConverter
	{
		public override Type TargetType
		{
			get { return typeof(Material); }
		}

		public override bool CanConvertFrom(ConvertOperation convert)
		{
			return 
				convert.AllowedOperations.HasFlag(ConvertOperation.Operation.CreateRes) && 
				convert.CanPerform<BatchInfo>();
		}
		public override bool Convert(ConvertOperation convert)
		{
			bool finishConvertOp = false;
			List<BatchInfo> availData = convert.Perform<BatchInfo>().ToList();

			// Generate objects
			foreach (BatchInfo info in availData)
			{
				if (convert.IsObjectHandled(info)) continue;

				// Auto-Generate Material
				string matName = "Material";
				if (!info.MainTexture.IsExplicitNull) matName = info.MainTexture.FullName;
				string matPath = PathHelper.GetFreePath(matName, Resource.GetFileExtByType<Material>());
				Material mat = new Material(info);
				mat.Save(matPath);

				convert.AddResult(mat);
				finishConvertOp = true;
				convert.MarkObjectHandled(info);
			}

			return finishConvertOp;
		}
	}
}
