using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using OpenTK;

using Duality.Resources;
using Duality.EditorHints;

namespace Duality
{
	/// <summary>
	/// Implement this interface in <see cref="Component">Components</see> that require per-frame updates.
	/// </summary>
	public interface ICmpUpdatable
	{
		/// <summary>
		/// Called once per frame in order to update the Component.
		/// </summary>
		void OnUpdate();
	}
	/// <summary>
	/// Implement this interface in C<see cref="Component">Components</see> that require per-frame updates in the editor.
	/// </summary>
	public interface ICmpEditorUpdatable
	{
		/// <summary>
		/// Called once per frame in order to update the Component in the editor.
		/// </summary>
		void OnUpdate();
	}
	/// <summary>
	/// Implement this interface in <see cref="Component">Components</see> that require notifications for other Components 
	/// being added or removed at the same GameObject.
	/// </summary>
	public interface ICmpComponentListener
	{
		/// <summary>
		/// Called whenever another Component has been added to this Components GameObject.
		/// </summary>
		/// <param name="comp">The Component that has been added</param>
		void OnComponentAdded(Component comp);
		/// <summary>
		/// Called whenever another Component is being removed from this Components GameObject.
		/// </summary>
		/// <param name="comp">The Component that is being removed</param>
		void OnComponentRemoving(Component comp);
	}
	/// <summary>
	/// Implement this interface in <see cref="Component">Components</see> that require notification if the location of 
	/// their GameObject inside the scene graph changes.
	/// </summary>
	public interface ICmpGameObjectListener
	{
		/// <summary>
		/// Called whenever this Components GameObjects <see cref="GameObject.Parent"/> has changed.
		/// </summary>
		/// <param name="oldParent">The old parent object.</param>
		/// <param name="newParent">The new parent object.</param>
		void OnGameObjectParentChanged(GameObject oldParent, GameObject newParent);
	}
	/// <summary>
	/// Implement this interface in <see cref="Component">Components</see> that require specific init and shutdown logic.
	/// </summary>
	public interface ICmpInitializable
	{
		/// <summary>
		/// Called in order to initialize the Component in a specific way.
		/// </summary>
		/// <param name="context">The kind of initialization that is intended.</param>
		void OnInit(Component.InitContext context);
		/// <summary>
		/// Called in order to shutdown the Component in a specific way.
		/// </summary>
		/// <param name="context">The kind of shutdown that is intended.</param>
		void OnShutdown(Component.ShutdownContext context);
	}
	/// <summary>
	/// Implement this interface in <see cref="Component">Components</see> that are considered renderable. 
	/// </summary>
	public interface ICmpRenderer
	{
		/// <summary>
		/// [GET] The Renderers bounding radius.
		/// </summary>
		float BoundRadius { get; }

		/// <summary>
		/// Determines whether or not this renderer is visible to the specified <see cref="IDrawDevice"/>.
		/// </summary>
		/// <param name="device">The <see cref="IDrawDevice"/> to which visibility is determined.</param>
		/// <returns>True, if this renderer is visible to the <see cref="IDrawDevice"/>. False, if not.</returns>
		bool IsVisible(IDrawDevice device);
		/// <summary>
		/// Draws the object.
		/// </summary>
		/// <param name="device">The <see cref="IDrawDevice"/> to which the object is drawn.</param>
		void Draw(IDrawDevice device);
	}
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
	/// <summary>
	/// Provides detailed information about a collision event.
	/// </summary>
	public class CollisionData
	{
		private	Vector2 pos;
		private	Vector2	normal;
		private	float	normalImpulse;
		private	float	normalMass;
		private	float	tangentImpulse;
		private	float	tangentMass;

		/// <summary>
		/// [GET] The position at which the collision occurred in absolute world coordinates.
		/// </summary>
		public Vector2 Pos
		{
			get { return this.pos; }
		}
		/// <summary>
		/// [GET] The normal vector of the collision impulse, in the global coordinate system.
		/// </summary>
		public Vector2 Normal
		{
			get { return this.normal; }
		}
		/// <summary>
		/// [GET] The impulse that is delivered along the provided normal vector.
		/// </summary>
		public float NormalImpulse
		{
			get { return this.normalImpulse; }
		}
		/// <summary>
		/// [GET] The mass that is interacting along the provided normal vector.
		/// </summary>
		public float NormalMass
		{
			get { return this.normalMass; }
		}
		/// <summary>
		/// [GET] The speed change that will occur when applying <see cref="NormalImpulse"/> to <see cref="NormalMass"/>.
		/// </summary>
		public float NormalSpeed
		{
			get { return this.normalImpulse / this.normalMass; }
		}
		/// <summary>
		/// [GET] The tangent vector of the collision impulse, in the global coordinate system.
		/// </summary>
		public Vector2 Tangent
		{
			get { return this.normal.PerpendicularRight; }
		}
		/// <summary>
		/// [GET] The impulse that is delivered along the provided tangent vector.
		/// </summary>
		public float TangentImpulse
		{
			get { return this.tangentImpulse; }
		}
		/// <summary>
		/// [GET] The mass that is interacting along the provided tangent vector.
		/// </summary>
		public float TangentMass
		{
			get { return this.tangentMass; }
		}
		/// <summary>
		/// [GET] The speed change that will occur when applying <see cref="TangentImpulse"/> to <see cref="TangentMass"/>.
		/// </summary>
		public float TangentSpeed
		{
			get { return this.tangentImpulse / this.tangentMass; }
		}

		public CollisionData(Vector2 pos, Vector2 normal, float normalImpulse, float tangentImpulse, float normalMass, float tangentMass)
		{
			this.pos = pos;
			this.normal = normal;
			this.normalImpulse = normalImpulse;
			this.tangentImpulse = tangentImpulse;
			this.normalMass = normalMass;
			this.tangentMass = tangentMass;
		}
		internal CollisionData(FarseerPhysics.Dynamics.Body localBody, FarseerPhysics.Dynamics.Contacts.ContactConstraint impulse, int pointIndex)
		{
			if (localBody == impulse.BodyA)
			{
				this.pos = PhysicsConvert.ToDualityUnit(impulse.Points[pointIndex].rA + impulse.BodyA.Position);
				this.normal = impulse.Normal;
				this.normalImpulse = PhysicsConvert.ToDualityUnit(PhysicsConvert.ToDualityUnit(impulse.Points[pointIndex].NormalImpulse * Time.SPFMult));
				this.tangentImpulse = PhysicsConvert.ToDualityUnit(PhysicsConvert.ToDualityUnit(impulse.Points[pointIndex].TangentImpulse * Time.SPFMult));
				this.normalMass = PhysicsConvert.ToDualityUnit(impulse.Points[pointIndex].NormalMass);
				this.tangentMass = PhysicsConvert.ToDualityUnit(impulse.Points[pointIndex].TangentMass);
			}
			else if (localBody == impulse.BodyB)
			{
				this.pos = PhysicsConvert.ToDualityUnit(impulse.Points[pointIndex].rB + impulse.BodyB.Position);
				this.normal = -impulse.Normal;
				this.normalImpulse = PhysicsConvert.ToDualityUnit(PhysicsConvert.ToDualityUnit(impulse.Points[pointIndex].NormalImpulse * Time.SPFMult));
				this.tangentImpulse = PhysicsConvert.ToDualityUnit(PhysicsConvert.ToDualityUnit(impulse.Points[pointIndex].TangentImpulse * Time.SPFMult));
				this.normalMass = PhysicsConvert.ToDualityUnit(impulse.Points[pointIndex].NormalMass);
				this.tangentMass = PhysicsConvert.ToDualityUnit(impulse.Points[pointIndex].TangentMass);
			}
			else
				throw new ArgumentException("Local body is not part of the collision", "localBody");
		}
	}
}
