using System;

namespace Duality.Components.Physics
{
	/// <summary>
	/// The type of a <see cref="RigidBody">Colliders</see> physical body.
	/// </summary>
	public enum BodyType
	{
		/// <summary>
		/// A static body. It will never move due to physical forces.
		/// </summary>
		Static,
		/// <summary>
		/// A dynamic body. Its movement is determined by physical effects.
		/// </summary>
		Dynamic
	}
}
