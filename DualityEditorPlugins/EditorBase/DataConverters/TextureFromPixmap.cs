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
	public class TextureFromPixmap : DataConverter
	{
		public override Type TargetType
		{
			get { return typeof(Texture); }
		}

		public override bool CanConvertFrom(ConvertOperation convert)
		{
			if (convert.AllowedOperations.HasFlag(ConvertOperation.Operation.CreateRes))
			{
				return convert.CanPerform<Pixmap>();
			}
			
			if (convert.AllowedOperations.HasFlag(ConvertOperation.Operation.Convert))
			{
				List<Pixmap> availData = convert.Perform<Pixmap>(ConvertOperation.Operation.Convert).ToList();
				return availData.Any(t => this.FindMatch(t) != null);
			}

			return false;
		}
		public override bool Convert(ConvertOperation convert)
		{
			bool finishConvertOp = false;
			List<Pixmap> availData = convert.Perform<Pixmap>().ToList();

			// Generate objects
			foreach (Pixmap baseRes in availData)
			{
				if (convert.IsObjectHandled(baseRes)) continue;

				// Find target Resource matching the source - or create one.
				Texture targetRes = this.FindMatch(baseRes);
				if (targetRes == null && convert.AllowedOperations.HasFlag(ConvertOperation.Operation.CreateRes))
				{
					targetRes = Texture.CreateFromPixmap(baseRes).Res;
				}

				if (targetRes == null) continue;
				convert.AddResult(targetRes);
				finishConvertOp = true;
				convert.MarkObjectHandled(baseRes);
			}

			return finishConvertOp;
		}
		private Texture FindMatch(Pixmap baseRes)
		{
			if (baseRes == null)
			{
				return null;
			}
			else if (baseRes.IsDefaultContent)
			{
				var defaultContent = ContentProvider.GetDefaultContent<Resource>();
				return defaultContent.Res().OfType<Texture>().FirstOrDefault(r => r.BasePixmap == baseRes);
			}
			else
			{
				// First try a direct approach
				string fileExt = Resource.GetFileExtByType<Texture>();
				string targetPath = baseRes.FullName + fileExt;
				Texture match = ContentProvider.RequestContent<Texture>(targetPath).Res;
				if (match != null) return match;

				// If that fails, search for other matches
				string targetName = baseRes.Name + fileExt;
				List<string> resFilePaths = Resource.GetResourceFiles();
				var resNameMatch = resFilePaths.Where(p => Path.GetFileName(p) == targetName);
				var resQuery = resNameMatch.Concat(resFilePaths).Distinct();
				foreach (string resFile in resQuery)
				{
					if (!resFile.EndsWith(fileExt)) continue;
					match = ContentProvider.RequestContent<Texture>(resFile).Res;
					if (match != null && match.BasePixmap == baseRes) return match;
				}

				// Give up.
				return null;
			}
		}
	}
}
