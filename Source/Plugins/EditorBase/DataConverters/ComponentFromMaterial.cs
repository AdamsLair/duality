﻿using System;
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
				bool hasAnimation = (mainTex != null && basePixmap != null && basePixmap.AnimFrames > 0);

				// Create a sprite Component in any case
				SpriteRenderer sprite = convert.Result.OfType<SpriteRenderer>().FirstOrDefault();
				if (sprite == null && gameobj != null) sprite = gameobj.GetComponent<SpriteRenderer>();
				if (sprite == null) sprite = new SpriteRenderer();
				sprite.SharedMaterial = mat;
				if (mainTex != null)
				{
					Vector2 spriteSize = new Vector2(mainTex.PixelWidth, mainTex.PixelHeight);
					if (hasAnimation)
					{
						spriteSize.X = (mainTex.PixelWidth / basePixmap.AnimCols) - basePixmap.AnimFrameBorder * 2;
						spriteSize.Y = (mainTex.PixelHeight / basePixmap.AnimRows) - basePixmap.AnimFrameBorder * 2;
					}
					sprite.Rect = Rect.Align(Alignment.Center, 0.0f, 0.0f, spriteSize.X, spriteSize.Y);
				}
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
