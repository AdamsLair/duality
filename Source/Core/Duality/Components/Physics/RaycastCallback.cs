using System;

namespace Duality.Components.Physics
{
	/// <summary>
	/// Called for each shape found in the query. You control how the ray cast proceeds by returning a float.
	/// </summary>
	/// <returns>-1 to ignore the curret shape, 0 to terminate the raycast, data.Fraction to clip the ray for current hit, or 1 to continue.</returns>
	public delegate float RayCastCallback(RayCastData data);
}
