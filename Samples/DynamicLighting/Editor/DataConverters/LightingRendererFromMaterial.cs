using System;
using System.Collections.Generic;
using System.Linq;

using Duality;
using Duality.Resources;
using Duality.Plugins.DynamicLighting;

using Duality.Editor;
using Duality.Editor.Properties;

namespace Duality.Editor.Plugins.DynamicLighting.DataConverters
{
	public class LightingRendererFromMaterial : DataConverter
	{
		public override Type TargetType
		{
			get { return typeof(LightingSpriteRenderer); }
		}
		public override int Priority
		{
			get { return DataConverter.PrioritySpecialized; }
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

				DrawTechnique tech = mat.Technique.Res;
				LightingTechnique lightTech = tech as LightingTechnique;
				if (tech == null) continue;

				bool isDynamicLighting = lightTech != null ||
					tech.PreferredVertexFormat == VertexC1P3T2A4.Declaration ||
					tech.PreferredVertexFormat == VertexC1P3T4A4A1.Declaration;
				if (!isDynamicLighting) continue;

				Texture mainTex = mat.MainTexture.Res;
				Pixmap basePixmap = (mainTex != null) ? mainTex.BasePixmap.Res : null;
				GameObject gameobj = convert.Result.OfType<GameObject>().FirstOrDefault();

				if (mainTex == null || basePixmap == null || basePixmap.AnimFrames == 0)
				{
					LightingSpriteRenderer sprite = convert.Result.OfType<LightingSpriteRenderer>().FirstOrDefault();
					if (sprite == null && gameobj != null) sprite = gameobj.GetComponent<LightingSpriteRenderer>();
					if (sprite == null) sprite = new LightingSpriteRenderer();
					sprite.SharedMaterial = mat;
					if (mainTex != null) sprite.Rect = Rect.Align(Alignment.Center, 0.0f, 0.0f, mainTex.PixelWidth, mainTex.PixelHeight);
					convert.SuggestResultName(sprite, mat.Name);
					results.Add(sprite);
				}
				else
				{
					LightingAnimSpriteRenderer sprite = convert.Result.OfType<LightingAnimSpriteRenderer>().FirstOrDefault();
					if (sprite == null && gameobj != null) sprite = gameobj.GetComponent<LightingAnimSpriteRenderer>();
					if (sprite == null) sprite = new LightingAnimSpriteRenderer();
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
