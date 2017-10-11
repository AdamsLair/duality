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
    public class DummyRendererSetup : Component, ICmpInitializable
	{
		private int spriteCount = 1000;

		[DontSerialize] private Random random = new Random(1);


		public int SpriteCount
		{
			get { return this.spriteCount; }
			set { this.spriteCount = value; }
		}


		private GameObject CreateSprite(Vector3 pos)
		{
			GameObject obj = new GameObject("Sprite");
			Transform transform = obj.AddComponent<Transform>();
			BenchmarkDummyRenderer sprite = obj.AddComponent<BenchmarkDummyRenderer>();

			transform.Pos = pos;
			transform.Angle = this.random.NextFloat(MathF.RadAngle360);

			return obj;
		}
		private void SetupScene()
		{
			List<GameObject> sprites = new List<GameObject>();
			Vector3 spriteBoxSize = new Vector3(1024, 1024, 5000);
			for (int i = 0; i < this.spriteCount; i++)
			{
				Vector3 pos;

				// Generate both on-screen and off-screen sprites to
				// make sure culling efficiency is part of the test.
				bool isVisible = (i % 2) == 0;
				if (isVisible)
				{
					pos = this.random.NextVector3(
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

				GameObject obj = this.CreateSprite(pos);
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
