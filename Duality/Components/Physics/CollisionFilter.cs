using System;

namespace Duality.Components.Physics
{
	/// <summary>
	/// Determines whether a collision is allowed to occur.
	/// </summary>
	/// <param name="collision">The collision that is about to occur.</param>
	/// <returns>True, if the collision is valid. False, if the collision should be ignored.</returns>
	public delegate bool CollisionFilter(CollisionFilterData collision);
}
