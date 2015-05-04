using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Duality;
using Duality.Editor;
using Duality.Resources;
using Duality.Components;
using Duality.Components.Physics;
using Duality.Components.Renderers;

namespace DualStickSpaceShooter
{
	public class EnemyBlueprint : Resource
	{
		private	ContentRef<Prefab>[]	exploEffects	= null;
		private	ContentRef<Sound>		exploSound		= null;
		private	float					exploDamage		= 200.0f;
		private	float					exploRadius		= 100.0f;
		private	float					exploForce		= 50.0f;
		private	float					exploMaxVel		= 5.0f;
		private	ContentRef<Sound>		moveSound		= null;
		private	ContentRef<Sound>		attackSound		= null;


		public ContentRef<Prefab>[] ExplosionEffects
		{
			get { return this.exploEffects; }
			set { this.exploEffects = value; }
		}
		public ContentRef<Sound> ExplosionSound
		{
			get { return this.exploSound; }
			set { this.exploSound = value; }
		}
		[EditorHintDecimalPlaces(0)]
		public float ExplosionDamage
		{
			get { return this.exploDamage; }
			set { this.exploDamage = value; }
		}
		[EditorHintDecimalPlaces(0)]
		public float ExplosionRadius
		{
			get { return this.exploRadius; }
			set { this.exploRadius = value; }
		}
		[EditorHintDecimalPlaces(1)]
		public float ExplosionForce
		{
			get { return this.exploForce; }
			set { this.exploForce = value; }
		}
		[EditorHintDecimalPlaces(1)]
		public float ExplosionMaxVelocity
		{
			get { return this.exploMaxVel; }
			set { this.exploMaxVel = value; }
		}
		public ContentRef<Sound> MoveSound
		{
			get { return this.moveSound; }
			set { this.moveSound = value; }
		}
		public ContentRef<Sound> AttackSound
		{
			get { return this.attackSound; }
			set { this.attackSound = value; }
		}
	}
}
