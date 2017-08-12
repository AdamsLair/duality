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
    public class MultiSpriteRendererSetup : Component, ICmpInitializable
	{
		private int spriteCount = 1000;


		public int SpriteCount
		{
			get { return this.spriteCount; }
			set { this.spriteCount = value; }
		}


		private GameObject CreateMultiSprite(Vector3 pos, int spriteCount)
		{
			GameObject obj = new GameObject("LotsOfSprites");
			Transform transform = obj.AddComponent<Transform>();
			BenchmarkMultiSpriteRenderer multiSprite = obj.AddComponent<BenchmarkMultiSpriteRenderer>();
			transform.Pos = pos;
			multiSprite.SpriteCount = spriteCount;
			return obj;
		}
		private void SetupScene()
		{
			List<GameObject> multiSprites = new List<GameObject>();
			int spritesPerObj = 1000;
			int objCount = this.spriteCount / spritesPerObj;
			for (int i = 0; i < objCount; i++)
			{
				Vector3 pos;

				// Generate both on-screen and off-screen sprites to
				// make sure culling efficiency is part of the test.
				bool isVisible = (i % 2) == 0;
				if (isVisible)
				{
					pos = new Vector3(0.0f, 0.0f, MathF.Pow(i, 2.0f) * 100.0f);
				}
				else
				{
					pos = new Vector3(10000, 10000, 0);
				}

				GameObject obj = this.CreateMultiSprite(pos, spritesPerObj);
				multiSprites.Add(obj);
			}

			this.GameObj.ParentScene.AddObjects(multiSprites);
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
