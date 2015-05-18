using System;
using System.Collections.Generic;
using System.Linq;

namespace Duality
{
	/// <summary>
	/// Implement this interface in <see cref="Component">Components</see> that require notification of
	/// collision events that occur to the <see cref="GameObject"/> they belong to.
	/// </summary>
	public interface ICmpCollisionListener
	{
		/// <summary>
		/// Called whenever the GameObject starts to collide with something.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		void OnCollisionBegin(Component sender, CollisionEventArgs args);
		/// <summary>
		/// Called whenever the GameObject stops to collide with something.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		void OnCollisionEnd(Component sender, CollisionEventArgs args);
		/// <summary>
		/// Called each frame after solving a collision with the GameObject.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		void OnCollisionSolve(Component sender, CollisionEventArgs args);
	}
}
