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
				List<Texture> availTex = convert.Perform<Texture>(ConvertOperation.Operation.Convert).ToList();
				return availTex.Any(t => this.FindMatch(t) != null);
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
				Material targetRes = this.FindMatch(baseRes);
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
		private Material FindMatch(Texture baseRes)
		{
			if (baseRes == null)
			{
				return null;
			}
			else if (baseRes.IsDefaultContent)
			{
				var defaultContent = ContentProvider.GetDefaultContent<Resource>();
				return defaultContent.Res().OfType<Material>().FirstOrDefault(r => r.MainTexture == baseRes);
			}
			else
			{
				// First try a direct approach
				string fileExt = Resource.GetFileExtByType<Material>();
				string targetPath = baseRes.FullName + fileExt;
				Material match = ContentProvider.RequestContent<Material>(targetPath).Res;
				if (match != null) return match;
				
				// If that fails, search for other matches
				string targetName = baseRes.Name + fileExt;
				string[] resFilePaths = Resource.GetResourceFiles().ToArray();
				var resNameMatch = resFilePaths.Where(p => Path.GetFileName(p) == targetName);
				var resQuery = resNameMatch.Concat(resFilePaths).Distinct();
				foreach (string resFile in resQuery)
				{
					if (!resFile.EndsWith(fileExt)) continue;
					match = ContentProvider.RequestContent<Material>(resFile).Res;
					if (match != null && match.MainTexture == baseRes) return match;
				}

				// Give up.
				return null;
			}
		}
	}
}
