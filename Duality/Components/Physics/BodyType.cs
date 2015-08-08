using System;

namespace Duality.Components.Physics
{
	/// <summary>
	/// The type of a <see cref="RigidBody">Colliders</see> physical body.
	/// </summary>
	public enum BodyType
	{
		/// <summary>
		/// A static body will never move due to physical forces.
		/// </summary>
		Static,
		/// <summary>
		/// A dynamic body's movement is determined by physical effects.
		/// </summary>
		Dynamic,
		/// <summary>
		/// A kinematic body can push around dynamic bodies, but is itself unaffected by 
		/// physical influences due to collisions. It can't collide with static bodies.
		/// Use with caution.
		/// </summary>
		Kinematic
	}
}
