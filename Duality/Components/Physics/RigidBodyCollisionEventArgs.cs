using System;

namespace Duality.Components.Physics
{
	public class RigidBodyCollisionEventArgs : CollisionEventArgs
	{
		private	ShapeInfo	colShapeA;
		private	ShapeInfo	colShapeB;

		public ShapeInfo MyShape
		{
			get { return this.colShapeA; }
		}
		public ShapeInfo OtherShape
		{
			get { return this.colShapeB; }
		}

		public RigidBodyCollisionEventArgs(GameObject obj, CollisionData data, ShapeInfo shapeA, ShapeInfo shapeB) : base(obj, data)
		{
			this.colShapeA = shapeA;
			this.colShapeB = shapeB;
		}
	}
}
