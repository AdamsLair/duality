/*
* Farseer Physics Engine based on Box2D.XNA port:
* Copyright (c) 2010 Ian Qvist
* 
* Box2D.XNA port of Box2D:
* Copyright (c) 2009 Brandon Furtwangler, Nathan Furtwangler
*
* Original source Box2D:
* Copyright (c) 2006-2009 Erin Catto http://www.gphysics.com 
* 
* This software is provided 'as-is', without any express or implied 
* warranty.  In no event will the authors be held liable for any damages 
* arising from the use of this software. 
* Permission is granted to anyone to use this software for any purpose, 
* including commercial applications, and to alter it and redistribute it 
* freely, subject to the following restrictions: 
* 1. The origin of this software must not be misrepresented; you must not 
* claim that you wrote the original software. If you use this software 
* in a product, an acknowledgment in the product documentation would be 
* appreciated but is not required. 
* 2. Altered source versions must be plainly marked as such, and must not be 
* misrepresented as being the original software. 
* 3. This notice may not be removed or altered from any source distribution. 
*/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using FarseerPhysics.Collision;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using Duality;

namespace FarseerPhysics.Dynamics.Contacts
{
	/// <summary>
	/// A contact edge is used to connect bodies and contacts together
	/// in a contact graph where each body is a node and each contact
	/// is an edge. A contact edge belongs to a doubly linked list
	/// maintained in each attached body. Each contact has two contact
	/// nodes, one for each attached body.
	/// </summary>
	public sealed class ContactEdge
	{
		/// <summary>
		/// The contact
		/// </summary>
		public Contact Contact;

		/// <summary>
		/// The next contact edge in the body's contact list
		/// </summary>
		public ContactEdge Next;

		/// <summary>
		/// Provides quick access to the other body attached.
		/// </summary>
		public Body Other;

		/// <summary>
		/// The previous contact edge in the body's contact list
		/// </summary>
		public ContactEdge Prev;
	}

	[Flags]
	public enum ContactFlags
	{
		None = 0,

		/// <summary>
		/// Used when crawling contact graph when forming islands.
		/// </summary>
		Island = 0x0001,

		/// <summary>
		/// Set when the shapes are touching.
		/// </summary>
		Touching = 0x0002,

		/// <summary>
		/// This contact can be disabled (by user)
		/// </summary>
		Enabled = 0x0004,

		/// <summary>
		/// This contact needs filtering because a fixture filter was changed.
		/// </summary>
		Filter = 0x0008,

		/// <summary>
		/// This bullet contact had a TOI event
		/// </summary>
		BulletHit = 0x0010,

		/// <summary>
		/// This contact has a valid TOI i the field TOI
		/// </summary>
		TOI = 0x0020
	}

	/// <summary>
	/// The class manages contact between two shapes. A contact exists for each overlapping
	/// AABB in the broad-phase (except if filtered). Therefore a contact object may exist
	/// that has no contact points.
	/// </summary>
	public class Contact
	{
		private static EdgeShape _edge = new EdgeShape();

		private static ContactType[,] _registers = new[,]
													   {
														   {
															   ContactType.Circle,
															   ContactType.EdgeAndCircle,
															   ContactType.PolygonAndCircle,
															   ContactType.ChainAndCircle,
														   },
														   {
															   ContactType.EdgeAndCircle,
															   ContactType.NotSupported,
                                                               // 1,1 is invalid (no ContactType.Edge)
                                                               ContactType.EdgeAndPolygon,
															   ContactType.NotSupported,
                                                               // 1,3 is invalid (no ContactType.EdgeAndChain)
                                                           },
														   {
															   ContactType.PolygonAndCircle,
															   ContactType.EdgeAndPolygon,
															   ContactType.Polygon,
															   ContactType.ChainAndPolygon,
														   },
														   {
															   ContactType.ChainAndCircle,
															   ContactType.NotSupported,
                                                               // 3,1 is invalid (no ContactType.EdgeAndChain)
                                                               ContactType.ChainAndPolygon,
															   ContactType.NotSupported,
                                                               // 3,3 is invalid (no ContactType.Chain)
                                                           },
													   };

		public Fixture FixtureA;
		public Fixture FixtureB;
		internal ContactFlags Flags;

		public Manifold Manifold;

		// Nodes for connecting bodies.
		internal ContactEdge NodeA = new ContactEdge();
		internal ContactEdge NodeB = new ContactEdge();
		public float TOI;
		internal int TOICount;
		private ContactType _type;

		private Contact(Fixture fA, int indexA, Fixture fB, int indexB)
		{
			Reset(fA, indexA, fB, indexB);
		}

		/// Enable/disable this contact. This can be used inside the pre-solve
		/// contact listener. The contact is only disabled for the current
		/// time step (or sub-step in continuous collisions).
		public bool Enabled
		{
			set
			{
				if (value)
				{
					this.Flags |= ContactFlags.Enabled;
				}
				else
				{
					this.Flags &= ~ContactFlags.Enabled;
				}
			}

			get { return (this.Flags & ContactFlags.Enabled) == ContactFlags.Enabled; }
		}

		/// <summary>
		/// Get the child primitive index for fixture A.
		/// </summary>
		/// <value>The child index A.</value>
		public int ChildIndexA { get; internal set; }

		/// <summary>
		/// Get the child primitive index for fixture B.
		/// </summary>
		/// <value>The child index B.</value>
		public int ChildIndexB { get; internal set; }

		/// <summary>
		/// Get the contact manifold. Do not modify the manifold unless you understand the
		/// internals of Box2D.
		/// </summary>
		/// <param name="manifold">The manifold.</param>
		public void GetManifold(out Manifold manifold)
		{
			manifold = this.Manifold;
		}

		/// <summary>
		/// Gets the world manifold.
		/// </summary>
		public void GetWorldManifold(out Vector2 normal, out FixedArray2<Vector2> points)
		{
			Body bodyA = this.FixtureA.Body;
			Body bodyB = this.FixtureB.Body;
			Shape shapeA = this.FixtureA.Shape;
			Shape shapeB = this.FixtureB.Shape;

			Collision.Collision.GetWorldManifold(ref this.Manifold, ref bodyA.Xf, shapeA.Radius, ref bodyB.Xf, shapeB.Radius,
												 out normal, out points);
		}

		/// <summary>
		/// Determines whether this contact is touching.
		/// </summary>
		/// <returns>
		/// 	<c>true</c> if this instance is touching; otherwise, <c>false</c>.
		/// </returns>
		public bool IsTouching()
		{
			return (this.Flags & ContactFlags.Touching) == ContactFlags.Touching;
		}

		/// <summary>
		/// Flag this contact for filtering. Filtering will occur the next time step.
		/// </summary>
		public void FlagForFiltering()
		{
			this.Flags |= ContactFlags.Filter;
		}

		private void Reset(Fixture fA, int indexA, Fixture fB, int indexB)
		{
			this.Flags = ContactFlags.Enabled;

			this.FixtureA = fA;
			this.FixtureB = fB;

			this.ChildIndexA = indexA;
			this.ChildIndexB = indexB;

			this.Manifold.PointCount = 0;

			this.NodeA.Contact = null;
			this.NodeA.Prev = null;
			this.NodeA.Next = null;
			this.NodeA.Other = null;

			this.NodeB.Contact = null;
			this.NodeB.Prev = null;
			this.NodeB.Next = null;
			this.NodeB.Other = null;

			this.TOICount = 0;
		}

		/// <summary>
		/// Update the contact manifold and touching status.
		/// Note: do not assume the fixture AABBs are overlapping or are valid.
		/// </summary>
		/// <param name="contactManager">The contact manager.</param>
		internal void Update(ContactManager contactManager)
		{
			Manifold oldManifold = this.Manifold;

			// Re-enable this contact.
			this.Flags |= ContactFlags.Enabled;

			bool touching;
			bool wasTouching = (this.Flags & ContactFlags.Touching) == ContactFlags.Touching;

			bool sensor = this.FixtureA.IsSensor || this.FixtureB.IsSensor;

			Body bodyA = this.FixtureA.Body;
			Body bodyB = this.FixtureB.Body;

			// Is this contact a sensor?
			if (sensor)
			{
				Shape shapeA = this.FixtureA.Shape;
				Shape shapeB = this.FixtureB.Shape;
				touching = AABB.TestOverlap(shapeA, this.ChildIndexA, shapeB, this.ChildIndexB, ref bodyA.Xf, ref bodyB.Xf);

				// Sensors don't generate manifolds.
				this.Manifold.PointCount = 0;
			}
			else
			{
				Evaluate(ref this.Manifold, ref bodyA.Xf, ref bodyB.Xf);
				touching = this.Manifold.PointCount > 0;

				// Match old contact ids to new contact ids and copy the
				// stored impulses to warm start the solver.
				for (int i = 0; i < this.Manifold.PointCount; ++i)
				{
					ManifoldPoint mp2 = this.Manifold.Points[i];
					mp2.NormalImpulse = 0.0f;
					mp2.TangentImpulse = 0.0f;
					ContactID id2 = mp2.Id;
					bool found = false;

					for (int j = 0; j < oldManifold.PointCount; ++j)
					{
						ManifoldPoint mp1 = oldManifold.Points[j];

						if (mp1.Id.Key == id2.Key)
						{
							mp2.NormalImpulse = mp1.NormalImpulse;
							mp2.TangentImpulse = mp1.TangentImpulse;
							found = true;
							break;
						}
					}
					if (found == false)
					{
						mp2.NormalImpulse = 0.0f;
						mp2.TangentImpulse = 0.0f;
					}

					this.Manifold.Points[i] = mp2;
				}

				if (touching != wasTouching)
				{
					bodyA.Awake = true;
					bodyB.Awake = true;
				}
			}

			if (touching)
			{
				this.Flags |= ContactFlags.Touching;
			}
			else
			{
				this.Flags &= ~ContactFlags.Touching;
			}

			if (wasTouching == false && touching)
			{
				//Report the collision to both participants:
				this.Enabled = this.Enabled && this.FixtureA.Body.OnCollision(this.FixtureA, this.FixtureB, this);

				//Reverse the order of the reported fixtures. The first fixture is always the one that the
				//user subscribed to.
				this.Enabled = this.Enabled && this.FixtureB.Body.OnCollision(this.FixtureB, this.FixtureA, this);

				//BeginContact can also return false and disable the contact
				if (contactManager.BeginContact != null)
					this.Enabled = contactManager.BeginContact(this);

				//if the user disabled the contact (needed to exclude it in TOI solver), we also need to mark
				//it as not touching.
				if (this.Enabled == false)
					this.Flags &= ~ContactFlags.Touching;
			}

			if (wasTouching && touching == false)
			{
				//Report the separation to both participants:
				if (this.FixtureA != null) this.FixtureA.Body.OnSeparation(this.FixtureA, this.FixtureB);

				//Reverse the order of the reported fixtures. The first fixture is always the one that the
				//user subscribed to.
				if (this.FixtureB != null) this.FixtureB.Body.OnSeparation(this.FixtureB, this.FixtureA);

				if (contactManager.EndContact != null)
					contactManager.EndContact(this);
			}

			if (sensor)
				return;

			if (contactManager.PreSolve != null)
				contactManager.PreSolve(this, ref oldManifold);
		}

		/// <summary>
		/// Evaluate this contact with your own manifold and transforms.   
		/// </summary>
		/// <param name="manifold">The manifold.</param>
		/// <param name="transformA">The first transform.</param>
		/// <param name="transformB">The second transform.</param>
		private void Evaluate(ref Manifold manifold, ref Transform transformA, ref Transform transformB)
		{
			switch (this._type)
			{
				case ContactType.Polygon:
					Collision.Collision.CollidePolygons(ref manifold,
														(PolygonShape)this.FixtureA.Shape, ref transformA,
														(PolygonShape)this.FixtureB.Shape, ref transformB);
					break;
				case ContactType.PolygonAndCircle:
					Collision.Collision.CollidePolygonAndCircle(ref manifold,
																(PolygonShape)this.FixtureA.Shape, ref transformA,
																(CircleShape)this.FixtureB.Shape, ref transformB);
					break;
				case ContactType.EdgeAndCircle:
					Collision.Collision.CollideEdgeAndCircle(ref manifold,
															 (EdgeShape)this.FixtureA.Shape, ref transformA,
															 (CircleShape)this.FixtureB.Shape, ref transformB);
					break;
				case ContactType.EdgeAndPolygon:
					Collision.Collision.CollideEdgeAndPolygon(ref manifold,
															  (EdgeShape)this.FixtureA.Shape, ref transformA,
															  (PolygonShape)this.FixtureB.Shape, ref transformB);
					break;
				case ContactType.ChainAndCircle:
					ChainShape chain = (ChainShape)this.FixtureA.Shape;
					chain.GetChildEdge(ref _edge, this.ChildIndexA);
					Collision.Collision.CollideEdgeAndCircle(ref manifold, _edge, ref transformA,
															 (CircleShape)this.FixtureB.Shape, ref transformB);
					break;
				case ContactType.ChainAndPolygon:
					ChainShape chain2 = (ChainShape)this.FixtureA.Shape;
					chain2.GetChildEdge(ref _edge, this.ChildIndexA);
					Collision.Collision.CollideEdgeAndPolygon(ref manifold, _edge, ref transformA,
															  (PolygonShape)this.FixtureB.Shape, ref transformB);
					break;
				case ContactType.Circle:
					Collision.Collision.CollideCircles(ref manifold,
													   (CircleShape)this.FixtureA.Shape, ref transformA,
													   (CircleShape)this.FixtureB.Shape, ref transformB);
					break;
			}
		}

		internal static Contact Create(Fixture fixtureA, int indexA, Fixture fixtureB, int indexB)
		{
			ShapeType type1 = fixtureA.ShapeType;
			ShapeType type2 = fixtureB.ShapeType;

			Debug.Assert(ShapeType.Unknown < type1 && type1 < ShapeType.TypeCount);
			Debug.Assert(ShapeType.Unknown < type2 && type2 < ShapeType.TypeCount);

			Contact c;
			Queue<Contact> pool = fixtureA.Body.World.ContactPool;
			if (pool.Count > 0)
			{
				c = pool.Dequeue();
				if ((type1 >= type2 || (type1 == ShapeType.Edge && type2 == ShapeType.Polygon))
					&&
					!(type2 == ShapeType.Edge && type1 == ShapeType.Polygon))
				{
					c.Reset(fixtureA, indexA, fixtureB, indexB);
				}
				else
				{
					c.Reset(fixtureB, indexB, fixtureA, indexA);
				}
			}
			else
			{
				// Edge+Polygon is non-symetrical due to the way Erin handles collision type registration.
				if ((type1 >= type2 || (type1 == ShapeType.Edge && type2 == ShapeType.Polygon))
					&&
					!(type2 == ShapeType.Edge && type1 == ShapeType.Polygon))
				{
					c = new Contact(fixtureA, indexA, fixtureB, indexB);
				}
				else
				{
					c = new Contact(fixtureB, indexB, fixtureA, indexA);
				}
			}

			c._type = _registers[(int)type1, (int)type2];

			return c;
		}

		internal void Destroy()
		{
			this.FixtureA.Body.World.ContactPool.Enqueue(this);
			Reset(null, 0, null, 0);
		}

		#region Nested type: ContactType

		private enum ContactType
		{
			NotSupported,
			Polygon,
			PolygonAndCircle,
			Circle,
			EdgeAndPolygon,
			EdgeAndCircle,
			ChainAndPolygon,
			ChainAndCircle,
		}

		#endregion
	}
}