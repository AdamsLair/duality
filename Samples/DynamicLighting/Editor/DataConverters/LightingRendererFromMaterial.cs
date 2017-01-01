﻿using System;
using System.Collections.Generic;
using System.Linq;

using Duality;
using Duality.Resources;
using Duality.Components.Renderers;

using Duality.Editor;
using Duality.Editor.Properties;

namespace DynamicLighting.DataConverters
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
				bool hasAnimation = (mainTex != null && basePixmap != null && basePixmap.AnimFrames > 0);

				// Create a sprite Component in any case
				LightingSpriteRenderer sprite = convert.Result.OfType<LightingSpriteRenderer>().FirstOrDefault();
				if (sprite == null && gameobj != null) sprite = gameobj.GetComponent<LightingSpriteRenderer>();
				if (sprite == null) sprite = new LightingSpriteRenderer();
				sprite.SharedMaterial = mat;
				Vector2 spriteSize = new Vector2(mainTex.PixelWidth, mainTex.PixelHeight);
				if (hasAnimation)
				{
					spriteSize.X = (mainTex.PixelWidth / basePixmap.AnimCols) - basePixmap.AnimFrameBorder * 2;
					spriteSize.Y = (mainTex.PixelHeight / basePixmap.AnimRows) - basePixmap.AnimFrameBorder * 2;
				}
				sprite.Rect = Rect.Align(Alignment.Center, 0.0f, 0.0f, spriteSize.X, spriteSize.Y);
				results.Add(sprite);

				// If we have animation data, create an animator component as well
				if (hasAnimation)
				{
					SpriteAnimator animator = convert.Result.OfType<SpriteAnimator>().FirstOrDefault();
					if (animator == null && gameobj != null) animator = gameobj.GetComponent<SpriteAnimator>();
					if (animator == null) animator = new SpriteAnimator();
					animator.AnimDuration = 5.0f;
					animator.FrameCount = basePixmap.AnimFrames;
					results.Add(animator);
				}
				
				convert.SuggestResultName(sprite, mat.Name);
				convert.MarkObjectHandled(mat);
			}

			convert.AddResult(results);
			return false;
		}
	}
}
