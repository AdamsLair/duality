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
	public class ParticleEmitter
	{
		private Range     burstDelay       = 100.0f;
		private Range     burstParticleNum = 1;
		private int       maxBurstCount    = -1;
		private Range     particleLifetime = 1000;
		private Vector3   basePos          = Vector3.Zero;
		private Range     randomPos        = 0.0f;
		private Range     randomAngle      = new Range(0.0f, MathF.RadAngle360);
		private Vector3   baseVel          = Vector3.Zero;
		private Range     randomVel        = new Range(0.0f, 3.0f);
		private Range     randomAngleVel   = new Range(-0.05f, 0.05f);
		private ColorHsva minColor         = ColorHsva.White;
		private ColorHsva maxColor         = ColorHsva.White;
		private Range     spriteIndex      = 0;
		private float     depthMult        = 1.0f;

		[DontSerialize]
		private int       burstCount       = 0;
		[DontSerialize]
		private float     burstTimer       = 0.0f;

		[EditorHintDecimalPlaces(0)]
		public Range BurstDelay
		{
			get { return this.burstDelay; }
			set { this.burstDelay = value; }
		}
		[EditorHintDecimalPlaces(0)]
		public Range BurstParticleNum
		{
			get { return this.burstParticleNum; }
			set { this.burstParticleNum = value; }
		}
		public int MaxBurstCount
		{
			get { return this.maxBurstCount; }
			set { this.maxBurstCount = value; }
		}
		[EditorHintDecimalPlaces(0)]
		public Range ParticleLifetime
		{
			get { return this.particleLifetime; }
			set { this.particleLifetime = value; }
		}
		public Vector3 BasePos
		{
			get { return this.basePos; }
			set { this.basePos = value; }
		}
		public Range RandomPos
		{
			get { return this.randomPos; }
			set { this.randomPos = value; }
		}
		public Range RandomAngle
		{
			get { return this.randomAngle; }
			set { this.randomAngle = value; }
		}
		public Vector3 BaseVel
		{
			get { return this.baseVel; }
			set { this.baseVel = value; }
		}
		public Range RandomVel
		{
			get { return this.randomVel; }
			set { this.randomVel = value; }
		}
		public Range RandomAngleVel
		{
			get { return this.randomAngleVel; }
			set { this.randomAngleVel = value; }
		}
		public ColorHsva MinColor
		{
			get { return this.minColor; }
			set { this.minColor = value; }
		}
		public ColorHsva MaxColor
		{
			get { return this.maxColor; }
			set { this.maxColor = value; }
		}
		[EditorHintDecimalPlaces(0)]
		public Range SpriteIndex
		{
			get { return this.spriteIndex; }
			set { this.spriteIndex = value; }
		}
		public float DepthMultiplier
		{
			get { return this.depthMult; }
			set { this.depthMult = value; }
		}

		public void Update(ParticleEffect effect)
		{
			this.burstTimer -= Time.MsPFMult * Time.TimeMult;
			while (this.burstTimer <= 0.0f && (this.burstCount < this.maxBurstCount || this.maxBurstCount < 0))
			{
				this.burstTimer += MathF.Rnd.NextFloat(this.burstDelay.MinValue, this.burstDelay.MaxValue);
				this.burstCount++;

				int count = MathF.Rnd.Next((int)this.burstParticleNum.MinValue, (int)this.burstParticleNum.MaxValue);
				effect.AddParticles(this, count);
			}
		}
		public void InitParticle(ref Particle particle)
		{
			Random random = MathF.Rnd;

			if (this.depthMult != 0.0f)
			{
				particle.Position	= this.basePos + random.NextVector3(this.randomPos.MinValue, this.randomPos.MaxValue) * new Vector3(1.0f, 1.0f, this.depthMult);
				particle.Velocity	= this.baseVel + random.NextVector3(this.randomVel.MinValue, this.randomVel.MaxValue) * new Vector3(1.0f, 1.0f, this.depthMult);
			}
			else
			{
				particle.Position	= this.basePos + new Vector3(random.NextVector2(this.randomPos.MinValue, this.randomPos.MaxValue));
				particle.Velocity	= this.baseVel + new Vector3(random.NextVector2(this.randomVel.MinValue, this.randomVel.MaxValue));
			}

			particle.Angle			= random.NextFloat(this.randomAngle.MinValue, this.randomAngle.MaxValue);
			particle.AngleVelocity	= random.NextFloat(this.randomAngleVel.MinValue, this.randomAngleVel.MaxValue);
			particle.TimeToLive		= random.NextFloat(this.particleLifetime.MinValue, this.particleLifetime.MaxValue);
			particle.SpriteIndex	= random.Next((int)this.spriteIndex.MinValue, (int)this.spriteIndex.MaxValue);
			particle.Color			= random.NextColorHsva(this.minColor, this.maxColor).ToRgba();
			particle.AgeFactor		= 0.0f;
		}
	}
}
