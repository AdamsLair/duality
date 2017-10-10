using System;
using System.Collections.Generic;
using System.Linq;

using Duality.Components;
using Duality.Components.Physics;
using Duality.Resources;

using NUnit.Framework;

namespace Duality.Tests.Components
{
	[TestFixture]
	public class RigidBodyTest
	{
		[Test] public void FallingBallOnPlatform()
		{
			Scene scene = new Scene();
			Scene.SwitchTo(scene);

			// Create the ball
			GameObject ball = new GameObject("Ball");
			Transform ballTransform = ball.AddComponent<Transform>();
			RigidBody ballBody = ball.AddComponent<RigidBody>();
			ballBody.Restitution = 0f;
			CollisionEventReceiver listener = ball.AddComponent<CollisionEventReceiver>();
			ballBody.AddShape(new CircleShapeInfo(1, new Vector2(0, 0), 1));
			scene.AddObject(ball);

			// Create the platform
			GameObject platform = new GameObject("Platform");
			Transform platformTrans = platform.AddComponent<Transform>();
			platformTrans.Pos = new Vector3(0, 5, 0);
			RigidBody platformBody = platform.AddComponent<RigidBody>();
			platformBody.Restitution = 0f;
			platformBody.AddShape(new ChainShapeInfo(new[] { new Vector2(-1, 0), new Vector2(1, 0), }));
			platformBody.BodyType = BodyType.Static;
			scene.AddObject(platform);

			// Simulate some update steps
			for (int i = 0; i < 10; i++)
			{
				DualityApp.Update(true);
			}

			// Check if the position is within expected values
			Assert.IsTrue(ballTransform.Pos.Y > 3f);
			Assert.IsTrue(ballTransform.Pos.Y < 5f);
			Assert.IsTrue(Math.Abs(ballTransform.Pos.X) < 0.01f);
			Assert.IsTrue(Math.Abs(ballTransform.Pos.Z) < 0.01f);

			// Check if the collision events were triggered
			// First one should always be a begin
			Assert.IsTrue(listener.Collisions[0].Type == CollisionEventReceiver.CollisionType.Begin); 
			// The ones after that should all be of type Solve. There should be no event of type End since there is no bouncing.
			Assert.IsTrue(listener.Collisions.Skip(1).All(x => x.Type == CollisionEventReceiver.CollisionType.Solve)); 
		}

		[Test] public void CreateEmptyBody()
		{
			// Create a new RigidBody
			GameObject obj = new GameObject("Object");
			obj.AddComponent<Transform>();
			RigidBody body = obj.AddComponent<RigidBody>();

			// Expect the body to be empty
			Assert.IsTrue(body.Shapes == null || !body.Shapes.Any());
			Assert.IsTrue(body.Joints == null || !body.Joints.Any());
		}
		[Test] public void CopyAddShapes()
		{
			// Create two bodies
			GameObject objA = new GameObject("ObjectA");
			GameObject objB = new GameObject("ObjectB", objA);
			objA.AddComponent<Transform>();
			objB.AddComponent<Transform>();
			RigidBody bodyA = objA.AddComponent<RigidBody>();
			RigidBody bodyB = objB.AddComponent<RigidBody>();

			// Add a shape to the first one
			bodyA.AddShape(new CircleShapeInfo(128, Vector2.Zero, 1.0f));

			// Is the second body empty and the first isn't?
			Assert.IsFalse(bodyA.Shapes == null || !bodyA.Shapes.Any());
			Assert.IsTrue(bodyB.Shapes == null || !bodyB.Shapes.Any());

			// Copy the first over to the second
			bodyA.CopyTo(bodyB);

			// Are they both non-empty now?
			Assert.IsFalse(bodyA.Shapes == null || !bodyA.Shapes.Any());
			Assert.IsFalse(bodyB.Shapes == null || !bodyB.Shapes.Any());
		}
		[Test] public void CopyRemoveShapes()
		{
			// Create two bodies
			GameObject objA = new GameObject("ObjectA");
			GameObject objB = new GameObject("ObjectB", objA);
			objA.AddComponent<Transform>();
			objB.AddComponent<Transform>();
			RigidBody bodyA = objA.AddComponent<RigidBody>();
			RigidBody bodyB = objB.AddComponent<RigidBody>();

			// Add a shape to the second one
			bodyB.AddShape(new CircleShapeInfo(128, Vector2.Zero, 1.0f));

			// Is the first body empty and the second isn't?
			Assert.IsTrue(bodyA.Shapes == null || !bodyA.Shapes.Any());
			Assert.IsFalse(bodyB.Shapes == null || !bodyB.Shapes.Any());

			// Copy the first over to the second
			bodyA.CopyTo(bodyB);

			// Are they both empty now?
			Assert.IsTrue(bodyA.Shapes == null || !bodyA.Shapes.Any());
			Assert.IsTrue(bodyB.Shapes == null || !bodyB.Shapes.Any());
		}
		[Test] public void CopyModifyShapes()
		{
			int radiusA = MathF.Rnd.Next();
			int radiusB = MathF.Rnd.Next();

			// Create two bodies
			GameObject objA = new GameObject("ObjectA");
			GameObject objB = new GameObject("ObjectB", objA);
			objA.AddComponent<Transform>();
			objB.AddComponent<Transform>();
			RigidBody bodyA = objA.AddComponent<RigidBody>();
			RigidBody bodyB = objB.AddComponent<RigidBody>();

			// Add a similar shape to both
			bodyA.AddShape(new CircleShapeInfo(radiusA, Vector2.Zero, 1.0f));
			bodyB.AddShape(new CircleShapeInfo(radiusB, Vector2.Zero, 1.0f));

			// Check if each body carries its designated shape
			Assert.IsTrue(bodyA.Shapes != null && (bodyA.Shapes.First() as CircleShapeInfo).Radius == radiusA);
			Assert.IsTrue(bodyB.Shapes != null && (bodyB.Shapes.First() as CircleShapeInfo).Radius == radiusB);

			// Copy the first over to the second
			bodyA.CopyTo(bodyB);

			// Are they both equal now?
			Assert.AreNotSame(bodyA.Shapes, bodyB.Shapes);
			Assert.AreNotSame(bodyA.Shapes.First(), bodyB.Shapes.First());
			Assert.IsTrue(bodyA.Shapes != null && (bodyA.Shapes.First() as CircleShapeInfo).Radius == radiusA);
			Assert.IsTrue(bodyB.Shapes != null && (bodyB.Shapes.First() as CircleShapeInfo).Radius == radiusA);
		}
		[Test] public void CopyReplaceShapes()
		{
			int radius = MathF.Rnd.Next();

			// Create two bodies
			GameObject objA = new GameObject("ObjectA");
			GameObject objB = new GameObject("ObjectB", objA);
			objA.AddComponent<Transform>();
			objB.AddComponent<Transform>();
			RigidBody bodyA = objA.AddComponent<RigidBody>();
			RigidBody bodyB = objB.AddComponent<RigidBody>();

			// Add a different kind of shape to both
			bodyA.AddShape(new CircleShapeInfo(radius, Vector2.Zero, 1.0f));
			bodyB.AddShape(new PolyShapeInfo(new Vector2[] { Vector2.Zero, Vector2.UnitX, Vector2.UnitY }, 1.0f));

			// Check if each body carries its designated shape
			Assert.IsNotNull(bodyA.Shapes);
			Assert.IsNotNull(bodyA.Shapes.First() as CircleShapeInfo);
			Assert.IsNotNull(bodyB.Shapes);
			Assert.IsNotNull(bodyB.Shapes.First() as PolyShapeInfo);

			// Copy the first over to the second
			bodyA.CopyTo(bodyB);

			// Are they both equal now?
			Assert.AreNotSame(bodyA.Shapes, bodyB.Shapes);
			Assert.AreNotSame(bodyA.Shapes.First(), bodyB.Shapes.First());
			Assert.IsTrue(bodyA.Shapes != null && (bodyA.Shapes.First() as CircleShapeInfo).Radius == radius);
			Assert.IsTrue(bodyB.Shapes != null && (bodyB.Shapes.First() as CircleShapeInfo).Radius == radius);
		}
	}
}
