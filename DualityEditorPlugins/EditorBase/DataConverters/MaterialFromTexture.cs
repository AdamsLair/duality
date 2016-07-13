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
	public class MaterialFromTexture : DataConverter
	{
		public override Type TargetType
		{
			get { return typeof(Material); }
		}

		public override bool CanConvertFrom(ConvertOperation convert)
		{
			if (convert.AllowedOperations.HasFlag(ConvertOperation.Operation.CreateRes))
			{
				return convert.CanPerform<Texture>();
			}
			
			if (convert.AllowedOperations.HasFlag(ConvertOperation.Operation.Convert))
			{
				List<Texture> availData = convert.Perform<Texture>(ConvertOperation.Operation.Convert).ToList();
				return availData.Any(t => 
					this.FindMatchingResources<Texture,Material>(t, IsMatch)
					.Any());
			}

			return false;
		}
		public override bool Convert(ConvertOperation convert)
		{
			bool finishConvertOp = false;
			List<Texture> availData = convert.Perform<Texture>().ToList();

			// Generate objects
			foreach (Texture baseRes in availData)
			{
				if (convert.IsObjectHandled(baseRes)) continue;

				// Find target Resource matching the source - or create one.
				Material targetRes = 
					this.FindMatchingResources<Texture,Material>(baseRes, IsMatch)
					.FirstOrDefault();
				if (targetRes == null && convert.AllowedOperations.HasFlag(ConvertOperation.Operation.CreateRes))
				{
					string resPath = PathHelper.GetFreePath(baseRes.FullName, Resource.GetFileExtByType<Material>());
					targetRes = new Material(DrawTechnique.Mask, ColorRgba.White, baseRes);
					targetRes.Save(resPath);
				}

				if (targetRes == null) continue;
				convert.AddResult(targetRes);
				finishConvertOp = true;
				convert.MarkObjectHandled(baseRes);
			}

			return finishConvertOp;
		}

		private static bool IsMatch(Texture source, Material target)
		{
			return target.MainTexture == source;
		}
	}
}
