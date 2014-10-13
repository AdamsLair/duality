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
	public class ShipBlueprint : Resource
	{
		private	float						thrusterPower			= 0.0f;
		private	float						turnPower				= 0.0f;
		private	float						healRate				= 0.0f;
		private	float						maxHitpoints			= 100.0f;
		private	float						maxSpeed				= 0.0f;
		private	float						maxTurnSpeed			= 0.0f;
		private	ContentRef<Prefab>			damageEffect			= null;
		private	ContentRef<Prefab>[]		deathEffects			= null;
		private	ContentRef<BulletBlueprint>	bulletType				= null;
		private	float						weaponDelay				= 0.0f;

		
		public float ThrusterPower
		{
			get { return this.thrusterPower; }
			set { this.thrusterPower = value; }
		}
		public float TurnPower
		{
			get { return this.turnPower; }
			set { this.turnPower = value; }
		}
		[EditorHintDecimalPlaces(0)]
		public float HealRate
		{
			get { return this.healRate; }
			set { this.healRate = value; }
		}
		[EditorHintDecimalPlaces(0)]
		public float MaxHitpoints
		{
			get { return this.maxHitpoints; }
			set { this.maxHitpoints = value; }
		}
		public float MaxSpeed
		{
			get { return this.maxSpeed; }
			set { this.maxSpeed = value; }
		}
		public float MaxTurnSpeed
		{
			get { return this.maxTurnSpeed; }
			set { this.maxTurnSpeed = value; }
		}
		public ContentRef<Prefab> DamageEffect
		{
			get { return this.damageEffect; }
			set { this.damageEffect = value; }
		}
		public ContentRef<Prefab>[] DeathEffects
		{
			get { return this.deathEffects; }
			set { this.deathEffects = value; }
		}
		public ContentRef<BulletBlueprint> BulletType
		{
			get { return this.bulletType; }
			set { this.bulletType = value; }
		}
		public float WeaponDelay
		{
			get { return this.weaponDelay; }
			set { this.weaponDelay = value; }
		}
	}
}
