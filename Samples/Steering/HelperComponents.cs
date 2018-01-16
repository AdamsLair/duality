using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Duality;
using Duality.Components;
using Duality.Components.Physics;
using Duality.Editor;
using Duality.Properties;

namespace Steering
{
	/// <summary>
	/// This Component assigns the objects RigidBody radius (taken from its first circle shape) directly to its
	/// Agent radius, and applies the Agents suggested velocity back to the RigidBody. The sole purpose of this
	/// Component is to visualize Agent behavior.
	/// </summary>
	[RequiredComponent(typeof(Agent))]
	[RequiredComponent(typeof(Transform))]
	[RequiredComponent(typeof(RigidBody))]
	[EditorHintCategory(CoreResNames.CategoryAI)]
	public class AgentAttributeTranslator : Component, ICmpUpdatable
	{
		public void OnUpdate()
		{
			RigidBody		rigidBody	= this.GameObj.GetComponent<RigidBody>();
			Agent			agent		= this.GameObj.GetComponent<Agent>();
			CircleShapeInfo shapeInfo	= rigidBody.Shapes.OfType<CircleShapeInfo>().FirstOrDefault();
			if (shapeInfo != null)
			{
				agent.Radius = shapeInfo.Radius;
			}
			rigidBody.AngularVelocity = 0.0f;
			rigidBody.LinearVelocity = agent.SuggestedVel;
		}
	}
}
