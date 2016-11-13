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
using Duality.Plugins.Tilemaps;


namespace Duality.Editor.Plugins.Tilemaps.DataConverters
{
	/// <summary>
	/// A <see cref="DataConverter"/> that is able to provide matching existing <see cref="Tileset"/>
	/// Resources to a given <see cref="Pixmap"/>, as well as create them on demand where requested.
	/// </summary>
	public class TilesetFromPixmap : DataConverter
	{
		public override Type TargetType
		{
			get { return typeof(Tileset); }
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
					this.FindMatchingResources<Pixmap,Tileset>(t, IsMatch)
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
				Tileset targetRes = 
					this.FindMatchingResources<Pixmap,Tileset>(baseRes, IsMatch)
					.FirstOrDefault();
				if (targetRes == null && convert.AllowedOperations.HasFlag(ConvertOperation.Operation.CreateRes))
				{
					string texPath = PathHelper.GetFreePath(baseRes.FullName, Resource.GetFileExtByType<Tileset>());
					targetRes = new Tileset();
					targetRes.RenderConfig.Add(new TilesetRenderInput
					{
						SourceData = baseRes
					});
					targetRes.Compile();
					targetRes.Save(texPath);
				}

				if (targetRes == null) continue;
				convert.AddResult(targetRes);
				finishConvertOp = true;
				convert.MarkObjectHandled(baseRes);
			}

			return finishConvertOp;
		}

		private static bool IsMatch(Pixmap source, Tileset target)
		{
			return 
				target.RenderConfig != null &&
				target.RenderConfig.Any(config => config.SourceData == source);
		}
	}
}
