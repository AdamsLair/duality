using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Duality;
using Duality.Components;
using Duality.Components.Physics;
using Duality.Editor;
using Duality.Properties;

namespace Duality.Plugins.Steering.Testbed
{
	[Serializable]
	[RequiredComponent(typeof(Agent))]
	[RequiredComponent(typeof(Transform))]
	[RequiredComponent(typeof(RigidBody))]
	[EditorHintCategory(typeof(CoreRes), CoreResNames.CategoryAI)]
	public class AgentAttributeTranslator : Component, ICmpUpdatable
	{
		public void OnUpdate()
		{
			RigidBody		rigidBody	= this.GameObj.RigidBody;
			Agent			agent		= GameObj.GetComponent<Agent>();
			CircleShapeInfo shapeInfo	= rigidBody.Shapes.ElementAtOrDefault(0) as CircleShapeInfo;
			if (shapeInfo != null)
			{
				agent.Radius = shapeInfo.Radius;
			}
			rigidBody.AngularVelocity = 0.0f;
			rigidBody.LinearVelocity = agent.SuggestedVel;
		}
	}
}
