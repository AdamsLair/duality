using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Duality;
using Duality.Editor;
using Duality.Input;
using Duality.Resources;
using Duality.Drawing;
using Duality.Components;
using Duality.Components.Renderers;

namespace Duality.Samples.Benchmarks
{
	[EditorHintCategory("Benchmarks")]
    public class SpriteRendererSetup : Component, ICmpInitializable
	{
		private int spriteCount = 1000;
		private bool alternatingMaterials = false;


		public int SpriteCount
		{
			get { return this.spriteCount; }
			set { this.spriteCount = value; }
		}
		public bool AlternatingMaterials
		{
			get { return this.alternatingMaterials; }
			set { this.alternatingMaterials = value; }
		}


		private GameObject CreateSprite(Vector3 pos, Material material)
		{
			GameObject obj = new GameObject("Sprite");
			Transform transform = obj.AddComponent<Transform>();
			SpriteRenderer sprite = obj.AddComponent<SpriteRenderer>();

			transform.Pos = pos;
			transform.Angle = MathF.Rnd.NextFloat(MathF.RadAngle360);
			sprite.SharedMaterial = material;
			sprite.Rect = Rect.Align(Alignment.Center, 0.0f, 0.0f, 64.0f, 64.0f);

			return obj;
		}
		private void SetupScene()
		{
			List<Material> materials = new List<Material>();
			if (this.alternatingMaterials)
			{
				ColorRgba[] colors = new ColorRgba[]
				{
					ColorRgba.White,
					ColorRgba.Black,
					ColorRgba.Red,
					ColorRgba.Green,
					ColorRgba.Blue,
					ColorRgba.Red + ColorRgba.Green,
					ColorRgba.Red + ColorRgba.Blue,
					ColorRgba.Green + ColorRgba.Blue
				};
				for (int i = 0; i < colors.Length; i++)
				{
					Material material = new Material(DrawTechnique.Mask, Texture.DualityIcon);
					material.MainColor = colors[i];
					materials.Add(material);
				}
			}
			else
			{
				materials.Add(new Material(DrawTechnique.Mask, Texture.DualityIcon));
			}

			List<GameObject> sprites = new List<GameObject>();
			Vector3 spriteBoxSize = new Vector3(1024, 1024, 5000);
			for (int i = 0; i < this.spriteCount; i++)
			{
				Material material = materials[(i / 5) % materials.Count];
				Vector3 pos;

				// Generate both on-screen and off-screen sprites to
				// make sure culling efficiency is part of the test.
				bool isVisible = (i % 2) == 0;
				if (isVisible)
				{
					pos = MathF.Rnd.NextVector3(
						-spriteBoxSize.X * 0.5f, 
						-spriteBoxSize.Y * 0.5f, 
						0.0f, 
						spriteBoxSize.X, 
						spriteBoxSize.Y, 
						spriteBoxSize.Z);
				}
				else
				{
					pos = new Vector3(10000, 10000, 0);
				}

				GameObject obj = this.CreateSprite(pos, material);
				sprites.Add(obj);
			}

			this.GameObj.ParentScene.AddObjects(sprites);
		}

		void ICmpInitializable.OnInit(Component.InitContext context)
		{
			if (context == InitContext.Activate && DualityApp.ExecContext == DualityApp.ExecutionContext.Game)
			{
				this.SetupScene();
			}
		}
		void ICmpInitializable.OnShutdown(Component.ShutdownContext context) { }
	}
}
