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
	public class ComponentFromMaterial : DataConverter
	{
		public override Type TargetType
		{
			get { return typeof(SpriteRenderer); }
		}

		public override bool CanConvertFrom(ConvertOperation convert)
		{
			return 
				convert.AllowedOperations.HasFlag(ConvertOperation.Operation.CreateObj) && 
				convert.CanPerform<Material>();
		}
		public override bool Convert(ConvertOperation convert)
		{
			// If we already have a renderer in the result set, consider generating
			// another one to be not the right course of action.
			if (convert.Result.OfType<ICmpRenderer>().Any())
				return false;

			List<object> results = new List<object>();
			List<Material> availData = convert.Perform<Material>().ToList();

			// Generate objects
			foreach (Material mat in availData)
			{
				if (convert.IsObjectHandled(mat)) continue;
				Texture mainTex = mat.MainTexture.Res;
				Pixmap basePixmap = (mainTex != null) ? mainTex.BasePixmap.Res : null;
				GameObject gameobj = convert.Result.OfType<GameObject>().FirstOrDefault();

				if (mainTex == null || basePixmap == null || basePixmap.AnimFrames == 0)
				{
					SpriteRenderer sprite = convert.Result.OfType<SpriteRenderer>().FirstOrDefault();
					if (sprite == null && gameobj != null) sprite = gameobj.GetComponent<SpriteRenderer>();
					if (sprite == null) sprite = new SpriteRenderer();
					sprite.SharedMaterial = mat;
					if (mainTex != null) sprite.Rect = Rect.Align(Alignment.Center, 0.0f, 0.0f, mainTex.PixelWidth, mainTex.PixelHeight);
					convert.SuggestResultName(sprite, mat.Name);
					results.Add(sprite);
				}
				else
				{
					AnimSpriteRenderer sprite = convert.Result.OfType<AnimSpriteRenderer>().FirstOrDefault();
					if (sprite == null && gameobj != null) sprite = gameobj.GetComponent<AnimSpriteRenderer>();
					if (sprite == null) sprite = new AnimSpriteRenderer();
					sprite.SharedMaterial = mat;
					sprite.Rect = Rect.Align(Alignment.Center, 
						0.0f, 
						0.0f, 
						(mainTex.PixelWidth / basePixmap.AnimCols) - basePixmap.AnimFrameBorder * 2, 
						(mainTex.PixelHeight / basePixmap.AnimRows) - basePixmap.AnimFrameBorder * 2);
					sprite.AnimDuration = 5.0f;
					sprite.AnimFrameCount = basePixmap.AnimFrames;
					convert.SuggestResultName(sprite, mat.Name);
					results.Add(sprite);
				}

				convert.MarkObjectHandled(mat);
			}

			convert.AddResult(results);
			return false;
		}
	}
}
