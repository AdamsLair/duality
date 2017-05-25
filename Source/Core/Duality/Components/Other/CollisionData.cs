using System;
using System.Collections.Generic;
using System.Linq;

using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;

namespace Duality
{
	/// <summary>
	/// Provides detailed information about a collision event.
	/// </summary>
	public class CollisionData
	{
		private	Vector2 pos;
		private	Vector2	normal;
		private	float	normalImpulse;
		private	float	normalMass;
		private	float	tangentImpulse;
		private	float	tangentMass;

		/// <summary>
		/// [GET] The position at which the collision occurred in absolute world coordinates.
		/// </summary>
		public Vector2 Pos
		{
			get { return this.pos; }
		}
		/// <summary>
		/// [GET] The normal vector of the collision impulse, in the global coordinate system.
		/// </summary>
		public Vector2 Normal
		{
			get { return this.normal; }
		}
		/// <summary>
		/// [GET] The impulse that is delivered along the provided normal vector.
		/// </summary>
		public float NormalImpulse
		{
			get { return this.normalImpulse; }
		}
		/// <summary>
		/// [GET] The mass that is interacting along the provided normal vector.
		/// </summary>
		public float NormalMass
		{
			get { return this.normalMass; }
		}
		/// <summary>
		/// [GET] The speed change that will occur when applying <see cref="NormalImpulse"/> to <see cref="NormalMass"/>.
		/// </summary>
		public float NormalSpeed
		{
			get { return this.normalImpulse / this.normalMass; }
		}
		/// <summary>
		/// [GET] The tangent vector of the collision impulse, in the global coordinate system.
		/// </summary>
		public Vector2 Tangent
		{
			get { return this.normal.PerpendicularRight; }
		}
		/// <summary>
		/// [GET] The impulse that is delivered along the provided tangent vector.
		/// </summary>
		public float TangentImpulse
		{
			get { return this.tangentImpulse; }
		}
		/// <summary>
		/// [GET] The mass that is interacting along the provided tangent vector.
		/// </summary>
		public float TangentMass
		{
			get { return this.tangentMass; }
		}
		/// <summary>
		/// [GET] The speed change that will occur when applying <see cref="TangentImpulse"/> to <see cref="TangentMass"/>.
		/// </summary>
		public float TangentSpeed
		{
			get { return this.tangentImpulse / this.tangentMass; }
		}

		public CollisionData(Vector2 pos, Vector2 normal, float normalImpulse, float tangentImpulse, float normalMass, float tangentMass)
		{
			this.pos = pos;
			this.normal = normal;
			this.normalImpulse = normalImpulse;
			this.tangentImpulse = tangentImpulse;
			this.normalMass = normalMass;
			this.tangentMass = tangentMass;
		}
		internal CollisionData(Body localBody, ContactConstraint impulse, int pointIndex)
		{
			if (localBody == impulse.BodyA)
			{
				this.pos = PhysicsUnit.LengthToDuality * (impulse.Points[pointIndex].rA + impulse.BodyA.WorldCenter);
				this.normal = impulse.Normal;
				this.normalImpulse = PhysicsUnit.ImpulseToDuality * impulse.Points[pointIndex].NormalImpulse;
				this.tangentImpulse = PhysicsUnit.ImpulseToDuality * impulse.Points[pointIndex].TangentImpulse;
				this.normalMass = PhysicsUnit.MassToDuality * impulse.Points[pointIndex].NormalMass;
				this.tangentMass = PhysicsUnit.MassToDuality * impulse.Points[pointIndex].TangentMass;
			}
			else if (localBody == impulse.BodyB)
			{
				this.pos = PhysicsUnit.LengthToDuality * (impulse.Points[pointIndex].rB + impulse.BodyB.WorldCenter);
				this.normal = -impulse.Normal;
				this.normalImpulse = PhysicsUnit.ImpulseToDuality * impulse.Points[pointIndex].NormalImpulse;
				this.tangentImpulse = PhysicsUnit.ImpulseToDuality * impulse.Points[pointIndex].TangentImpulse;
				this.normalMass = PhysicsUnit.MassToDuality * impulse.Points[pointIndex].NormalMass;
				this.tangentMass = PhysicsUnit.MassToDuality * impulse.Points[pointIndex].TangentMass;
			}
			else
				throw new ArgumentException("Local body is not part of the collision", "localBody");
		}
	}
}
