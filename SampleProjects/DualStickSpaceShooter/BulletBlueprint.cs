using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;

using Duality;
using Duality.Editor;
using Duality.Resources;
using Duality.Components;
using Duality.Components.Physics;
using Duality.Components.Renderers;

namespace DualStickSpaceShooter
{
	[Serializable]
	public class BulletBlueprint : Resource
	{
		private	float					lifetime		= 8000.0f;
		private	float					recoilForce		= 1.0f;
		private	float					impactForce		= 5.0f;
		private	float					launchSpeed		= 10.0f;
		private	ContentRef<Material>	spriteMaterial	= null;

		public float Lifetime
		{
			get { return this.lifetime; }
			set { this.lifetime = value; }
		}
		public float RecoilForce
		{
			get { return this.recoilForce; }
			set { this.recoilForce = value; }
		}
		public float ImpactForce
		{
			get { return this.impactForce; }
			set { this.impactForce = value; }
		}
		public float LaunchSpeed
		{
			get { return this.launchSpeed; }
			set { this.launchSpeed = value; }
		}
		public ContentRef<Material> SpriteMaterial
		{
			get { return this.spriteMaterial; }
			set { this.spriteMaterial = value; }
		}

		public Bullet CreateBullet()
		{
			GameObject		obj			= new GameObject("Bullet");
			Transform		transform	= obj.AddComponent<Transform>();
			RigidBody		body		= obj.AddComponent<RigidBody>();
			SpriteRenderer	sprite		= obj.AddComponent<SpriteRenderer>();
			Bullet			bullet		= obj.AddComponent<Bullet>();

			Material spriteMaterial = this.spriteMaterial.Res ?? Material.SolidWhite.Res;
			Vector2 spriteSize = spriteMaterial.MainTexture.IsAvailable ? spriteMaterial.MainTexture.Res.Size : new Vector2(5, 5);
			float spriteRadius = MathF.Max(spriteSize.X, spriteSize.Y) * 0.5f;

			body.ClearShapes();
			body.AddShape(new CircleShapeInfo(spriteRadius, Vector2.Zero, 1.0f));
			body.CollidesWith &= ~CollisionCategory.Cat2;

			sprite.SharedMaterial = this.spriteMaterial;
			sprite.Rect = Rect.AlignCenter(0.0f, 0.0f, spriteSize.X * 0.5f, spriteSize.Y * 0.5f);

			bullet.InitFrom(this);

			return bullet;
		}
	}
}
