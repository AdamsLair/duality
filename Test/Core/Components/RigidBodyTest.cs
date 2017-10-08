using System.Linq;
using System.Threading;
using Duality.Components;
using Duality.Components.Physics;
using Duality.Resources;
using NUnit.Framework;

namespace Duality.Tests.Components
{
	[TestFixture]
	public class RigidBodyTest
	{
		[Test]
		public void FallingBallOnPlatform()
		{
			var scene = new Scene();
			Scene.SwitchTo(scene);

			//Create the ball
			var ball = new GameObject("Ball");
			var ballTransform = ball.AddComponent<Transform>();
			var ballBody = ball.AddComponent<RigidBody>();
			var listener = ball.AddComponent<CollisionListener>();
			bool onCollisionBeginCalled = false, onCollisionEndCalled = false, onCollisionSolveCalled = false;
			listener.CollisionBegin += (sender, args) => onCollisionBeginCalled = true;
			listener.CollisionEnd += (sender, args) => onCollisionEndCalled = true;
			listener.CollisionSolve += (sender, args) => onCollisionSolveCalled = true;
			ballBody.AddShape(new CircleShapeInfo(1, new Vector2(0, 0), 1));
			scene.AddObject(ball);

			//Create the platform
			var platform = new GameObject("Platform");
			var platformTrans = platform.AddComponent<Transform>();
			var platformBody = platform.AddComponent<RigidBody>();
			platformBody.AddShape(new ChainShapeInfo(new[] { new Vector2(-1, 0), new Vector2(1, 0), }));
			platformBody.BodyType = BodyType.Static;
			scene.AddObject(platform);
			platformTrans.Pos = new Vector3(0, 5, 0);

			for (int i = 0; i < 6; i++)
			{
				Thread.Sleep(16); //Simulate ~60 fps.
				DualityApp.Update();
			}
			Assert.IsTrue(ballTransform.Pos.Y > 0);
			Assert.IsTrue(ballTransform.Pos.X == 0);
			Assert.IsTrue(ballTransform.Pos.Z == 0);
			Assert.IsTrue(onCollisionBeginCalled);
			Assert.IsTrue(onCollisionEndCalled);
			Assert.IsTrue(onCollisionSolveCalled);
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
