using System;

namespace Duality.Components.Physics
{
	public struct CollisionFilterData
	{
		private GameObject myObject;
		private GameObject otherObject;
		private ShapeInfo myShape;
		private ShapeInfo otherShape;
		
		public GameObject MyGameObj
		{
			get { return this.myObject; }
		}
		public GameObject OtherGameObj
		{
			get { return this.otherObject; }
		}
		public RigidBody MyBody
		{
			get { return this.myShape != null ? this.myShape.Parent : null; }
		}
		public RigidBody OtherBody
		{
			get { return this.otherShape != null ? this.otherShape.Parent : null; }
		}
		public ShapeInfo MyShape
		{
			get { return this.myShape; }
		}
		public ShapeInfo OtherShape
		{
			get { return this.otherShape; }
		}

		public CollisionFilterData(GameObject myObj, GameObject otherObj, ShapeInfo myShape, ShapeInfo otherShape)
		{
			this.myObject = myObj;
			this.otherObject = otherObj;
			this.myShape = myShape;
			this.otherShape = otherShape;
		}
	}
}
