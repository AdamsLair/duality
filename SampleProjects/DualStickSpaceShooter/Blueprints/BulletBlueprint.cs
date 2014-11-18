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
		private	float					impactMass		= 1.5f;
		private	float					launchSpeed		= 10.0f;
		private	float					damage			= 5.0f;
		private	ContentRef<Material>	spriteMaterial	= null;
		private ContentRef<Prefab>		hitEffect		= null;
		private ContentRef<Prefab>		hitWorldEffect	= null;
		private ContentRef<Sound>		hitSound		= null;
		private ContentRef<Sound>		hitObjectSound	= null;

		public float Lifetime
		{
			get { return this.lifetime; }
			set { this.lifetime = value; }
		}
		public float ImpactMass
		{
			get { return this.impactMass; }
			set { this.impactMass = value; }
		}
		public float LaunchSpeed
		{
			get { return this.launchSpeed; }
			set { this.launchSpeed = value; }
		}
		public float Damage
		{
			get { return this.damage; }
			set { this.damage = value; }
		}
		public ContentRef<Material> SpriteMaterial
		{
			get { return this.spriteMaterial; }
			set { this.spriteMaterial = value; }
		}
		public ContentRef<Prefab> HitEffect
		{
			get { return this.hitEffect; }
			set { this.hitEffect = value; }
		}
		public ContentRef<Prefab> HitWorldEffect
		{
			get { return this.hitWorldEffect; }
			set { this.hitWorldEffect = value; }
		}
		public ContentRef<Sound> HitSound
		{
			get { return this.hitSound; }
			set { this.hitSound = value; }
		}
		public ContentRef<Sound> HitObjectSound
		{
			get { return this.hitObjectSound; }
			set { this.hitObjectSound = value; }
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
			float spriteRadius = MathF.Max(spriteSize.X, spriteSize.Y) * 0.25f;

			body.ClearShapes();
			CircleShapeInfo circleShape = new CircleShapeInfo(spriteRadius, Vector2.Zero, 1.0f);
			circleShape.IsSensor = true;
			body.AddShape(circleShape);
			body.CollisionCategory = CollisionCategory.Cat3;
			body.CollidesWith &= ~CollisionCategory.Cat3;

			sprite.SharedMaterial = this.spriteMaterial;
			sprite.Rect = Rect.AlignCenter(0.0f, 0.0f, spriteSize.X * 0.5f, spriteSize.Y * 0.5f);

			bullet.InitFrom(this);

			return bullet;
		}
	}
}
