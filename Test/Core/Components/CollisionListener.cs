﻿using System;

namespace Duality.Tests.Components
{
	/// <summary>
	/// A collision listener to expose the collision events to tests
	/// </summary>
	public class CollisionListener : Component, ICmpCollisionListener
	{
		public event EventHandler<CollisionEventArgs> CollisionBegin;
		public event EventHandler<CollisionEventArgs> CollisionEnd;
		public event EventHandler<CollisionEventArgs> CollisionSolve;

		public void OnCollisionBegin(Component sender, CollisionEventArgs args)
		{
			if (CollisionBegin != null) CollisionBegin.Invoke(sender, args);
		}

		public void OnCollisionEnd(Component sender, CollisionEventArgs args)
		{
			if (CollisionEnd != null) CollisionEnd.Invoke(sender, args);
		}

		public void OnCollisionSolve(Component sender, CollisionEventArgs args)
		{
			if (CollisionSolve != null) CollisionSolve.Invoke(sender, args);
		}
	}
}
