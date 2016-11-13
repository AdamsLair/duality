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
				return availData.Any(t => 
					this.FindMatchingResources<Pixmap,Texture>(t, IsMatch)
					.Any());
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
				Texture targetRes = 
					this.FindMatchingResources<Pixmap,Texture>(baseRes, IsMatch)
					.FirstOrDefault();
				if (targetRes == null && convert.AllowedOperations.HasFlag(ConvertOperation.Operation.CreateRes))
				{
					string texPath = PathHelper.GetFreePath(baseRes.FullName, Resource.GetFileExtByType<Texture>());
					targetRes = new Texture(baseRes);
					targetRes.Save(texPath);
				}

				if (targetRes == null) continue;
				convert.AddResult(targetRes);
				finishConvertOp = true;
				convert.MarkObjectHandled(baseRes);
			}

			return finishConvertOp;
		}

		private static bool IsMatch(Pixmap source, Texture target)
		{
			return target.BasePixmap == source;
		}
	}
}
