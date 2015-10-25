using System;
using System.Collections.Generic;
using System.Linq;

using Duality;
using Duality.Editor;
using Duality.Resources;
using Duality.Components;
using Duality.Components.Renderers;
using Duality.Drawing;

namespace ParticleSystem
{
	[RequiredComponent(typeof(Transform))]
	public class ParticleEffect : Renderer, ICmpUpdatable, ICmpInitializable
	{
		private ContentRef<Material>  material      = null;
		private Vector2               particleSize  = new Vector2(16, 16);
		private Vector3               constantForce = Vector3.Zero;
		private float                 linearDrag    = 0.3f;
		private float                 angularDrag   = 0.3f;
		private float                 fadeInAt      = 0.0f;
		private float                 fadeOutAt     = 0.75f;
		private List<ParticleEmitter> emitters      = new List<ParticleEmitter>();

		[DontSerialize]
		private float                 boundRadius   = 0.0f;
		[DontSerialize]
		private RawList<Particle>     particles     = null;
		[DontSerialize]
		private RawList<VertexC1P3T2> vertexBuffer  = null;

		
		public ContentRef<Material> ParticleMaterial
		{
			get { return this.material; }
			set { this.material = value; }
		}
		public Vector2 ParticleSize
		{
			get { return this.particleSize; }
			set { this.particleSize = value; }
		}
		public Vector3 ConstantForce
		{
			get { return this.constantForce; }
			set { this.constantForce = value; }
		}
		[EditorHintRange(0.0f, 1.0f)]
		public float LinearDrag
		{
			get { return this.linearDrag; }
			set { this.linearDrag = value; }
		}
		[EditorHintRange(0.0f, 1.0f)]
		public float AngularDrag
		{
			get { return this.angularDrag; }
			set { this.angularDrag = value; }
		}
		[EditorHintRange(0.0f, 1.0f)]
		public float FadeInAt
		{
			get { return this.fadeInAt; }
			set { this.fadeInAt = value; }
		}
		[EditorHintRange(0.0f, 1.0f)]
		public float FadeOutAt
		{
			get { return this.fadeOutAt; }
			set { this.fadeOutAt = value; }
		}
		public List<ParticleEmitter> Emitters
		{
			get { return this.emitters; }
			set { this.emitters = value ?? new List<ParticleEmitter>(); }
		}
		public override float BoundRadius
		{
			get { return this.boundRadius * this.GameObj.Transform.Scale; }
		}


		public void AddParticles(ParticleEmitter emitter, int count)
		{
			// Lookup what sprite sheet we're using to get the number of available frames
			Texture tex = this.RetrieveTexture();
			if (tex == null) return;
			Pixmap img = tex.BasePixmap.Res;
			if (img == null) return;
			
			// Gather data for emitting particles
			Vector3 effectPos = this.GameObj.Transform.Pos;
			float effectAngle = this.GameObj.Transform.Angle;
			float effectScale = this.GameObj.Transform.Scale;

			// Reserve memory for storing the new particles we're spawning
			if (this.particles == null) this.particles = new RawList<Particle>(count);
			int oldCount = this.particles.Count;
			this.particles.Count = this.particles.Count + count;

			// Initialize all those new particles
			Particle[] particleData = this.particles.Data;
			for (int i = oldCount; i < this.particles.Count; i++)
			{
				// Initialize the current particle.
				emitter.InitParticle(ref particleData[i]);
			}
		}

		private void RemoveParticle(int index)
		{
			this.particles.RemoveAt(index);
		}
		private Texture RetrieveTexture()
		{
			if (material.IsAvailable && material.Res.MainTexture.IsAvailable)
				return material.Res.MainTexture.Res;
			else
				return null;
		}

		private void UpdateEmitters()
		{
			for (int i = this.emitters.Count - 1; i >= 0; i--)
			{
				if (this.emitters[i] == null) continue;
				this.emitters[i].Update(this);
			}
		}

		public override void Draw(IDrawDevice device)
		{
			if (this.particles == null) return;
			
			Texture tex = this.RetrieveTexture();
			if (tex == null) return;

			Vector2 particleHalfSize = this.particleSize * 0.5f;
			float objAngle = this.GameObj.Transform.Angle;
			float objScale = this.GameObj.Transform.Scale;
			Vector3 objPos = this.GameObj.Transform.Pos;
			
			Vector2 objXDot, objYDot;
			MathF.GetTransformDotVec(objAngle, objScale, out objXDot, out objYDot);

			if (this.vertexBuffer == null) this.vertexBuffer = new RawList<VertexC1P3T2>(this.particles.Count * 4);
			this.vertexBuffer.Count = this.vertexBuffer.Count = this.particles.Count * 4;
			
			VertexC1P3T2[] vertexData = this.vertexBuffer.Data;
			Particle[] particleData = this.particles.Data;
			int particleCount = this.particles.Count;
			for (int i = 0; i < particleCount; i++)
			{
				ColorRgba color = particleData[i].Color;
				float alpha = (float)color.A / 255.0f;
				if (this.fadeOutAt < 1.0f) alpha *= MathF.Clamp((1.0f - particleData[i].AgeFactor) / this.fadeOutAt, 0.0f, 1.0f);
				if (this.fadeInAt > 0.0f) alpha *= MathF.Clamp(particleData[i].AgeFactor / this.fadeInAt, 0.0f, 1.0f);
				color.A = (byte)(alpha * 255.0f);

				Rect uvRect;
				tex.LookupAtlas(particleData[i].SpriteIndex, out uvRect);

				Vector3 particlePos = particleData[i].Position;
				MathF.TransformDotVec(ref particlePos, ref objXDot, ref objYDot);
				particlePos += objPos;

				float particleAngle = objAngle + particleData[i].Angle;
				float particleScale = objScale;

				device.PreprocessCoords(ref particlePos, ref particleScale);

				Vector2 xDot, yDot;
				MathF.GetTransformDotVec(particleAngle, particleScale, out xDot, out yDot);

				Vector2 edgeTopLeft		= new Vector2(-particleHalfSize.X, -particleHalfSize.Y);
				Vector2 edgeBottomLeft	= new Vector2(-particleHalfSize.X, particleHalfSize.Y);
				Vector2 edgeBottomRight = new Vector2(particleHalfSize.X, particleHalfSize.Y);
				Vector2 edgeTopRight	= new Vector2(particleHalfSize.X, -particleHalfSize.Y);

				MathF.TransformDotVec(ref edgeTopLeft,		ref xDot, ref yDot);
				MathF.TransformDotVec(ref edgeBottomLeft,	ref xDot, ref yDot);
				MathF.TransformDotVec(ref edgeBottomRight,	ref xDot, ref yDot);
				MathF.TransformDotVec(ref edgeTopRight,		ref xDot, ref yDot);
				
				int vertexBaseIndex = i * 4;
				vertexData[vertexBaseIndex + 0].Pos.X = particlePos.X + edgeTopLeft.X;
				vertexData[vertexBaseIndex + 0].Pos.Y = particlePos.Y + edgeTopLeft.Y;
				vertexData[vertexBaseIndex + 0].Pos.Z = particlePos.Z;
				vertexData[vertexBaseIndex + 0].TexCoord.X = uvRect.X;
				vertexData[vertexBaseIndex + 0].TexCoord.Y = uvRect.Y;
				vertexData[vertexBaseIndex + 0].Color = color;

				vertexData[vertexBaseIndex + 1].Pos.X = particlePos.X + edgeBottomLeft.X;
				vertexData[vertexBaseIndex + 1].Pos.Y = particlePos.Y + edgeBottomLeft.Y;
				vertexData[vertexBaseIndex + 1].Pos.Z = particlePos.Z;
				vertexData[vertexBaseIndex + 1].TexCoord.X = uvRect.X;
				vertexData[vertexBaseIndex + 1].TexCoord.Y = uvRect.BottomY;
				vertexData[vertexBaseIndex + 1].Color = color;

				vertexData[vertexBaseIndex + 2].Pos.X = particlePos.X + edgeBottomRight.X;
				vertexData[vertexBaseIndex + 2].Pos.Y = particlePos.Y + edgeBottomRight.Y;
				vertexData[vertexBaseIndex + 2].Pos.Z = particlePos.Z;
				vertexData[vertexBaseIndex + 2].TexCoord.X = uvRect.RightX;
				vertexData[vertexBaseIndex + 2].TexCoord.Y = uvRect.BottomY;
				vertexData[vertexBaseIndex + 2].Color = color;
				
				vertexData[vertexBaseIndex + 3].Pos.X = particlePos.X + edgeTopRight.X;
				vertexData[vertexBaseIndex + 3].Pos.Y = particlePos.Y + edgeTopRight.Y;
				vertexData[vertexBaseIndex + 3].Pos.Z = particlePos.Z;
				vertexData[vertexBaseIndex + 3].TexCoord.X = uvRect.RightX;
				vertexData[vertexBaseIndex + 3].TexCoord.Y = uvRect.Y;
				vertexData[vertexBaseIndex + 3].Color = color;
			}

			device.AddVertices(this.material, VertexMode.Quads, vertexData, this.vertexBuffer.Count);
		}
		void ICmpUpdatable.OnUpdate()
		{
			// Update all existing particles
			Vector3 boundMax = Vector3.Zero;
			if (this.particles != null)
			{
				float timeMult = Time.TimeMult;
				float timePassed = Time.MsPFMult * timeMult;
				
				Particle[] particleData = this.particles.Data;
				int particleCount = this.particles.Count;
				for (int i = particleCount - 1; i >= 0; i--)
				{
					particleData[i].Position		+= particleData[i].Velocity * timeMult;
					particleData[i].Angle			+= particleData[i].AngleVelocity * timeMult;
					particleData[i].Velocity		+= this.constantForce * 0.01f * timeMult;
					particleData[i].Velocity		*= MathF.Pow(1.0f - (this.linearDrag * 0.1f), timeMult);
					particleData[i].AngleVelocity	*= MathF.Pow(1.0f - (this.angularDrag * 0.1f), timeMult);
					particleData[i].AgeFactor		+= timePassed / particleData[i].TimeToLive;
					if (particleData[i].AgeFactor > 1.0f)
						this.RemoveParticle(i);

					boundMax.X = MathF.Max(boundMax.X, MathF.Abs(particleData[i].Position.X));
					boundMax.Y = MathF.Max(boundMax.Y, MathF.Abs(particleData[i].Position.Y));
					boundMax.Z = MathF.Max(boundMax.Z, MathF.Abs(particleData[i].Position.Z));
				}
			}
			this.boundRadius = boundMax.Length;
			this.boundRadius += this.particleSize.Length;

			// Update particle emission
			this.UpdateEmitters();
		}
		void ICmpInitializable.OnInit(Component.InitContext context)
		{
			if (context == InitContext.Activate)
			{
				// When activating, directly update particle emitters once, so there is already something to see.
				this.UpdateEmitters();
			}
		}
		void ICmpInitializable.OnShutdown(Component.ShutdownContext context) {}
	}
}
