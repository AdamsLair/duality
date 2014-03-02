using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Duality;
using Duality.Plugins.Navigation;
using Duality.Components;
using Duality.Components.Physics;

namespace NavigationTestbed
{
	[Serializable]
	[RequiredComponent(typeof(Agent))]
	[RequiredComponent(typeof(Transform))]
	[RequiredComponent(typeof(RigidBody))]
	public class AgentAttributeTranslator : Component, ICmpUpdatable
	{
		public void OnUpdate()
		{
			var transform = this.GameObj.GetComponent<Transform>();
			var rigidBody = this.GameObj.RigidBody;
			var agent = GameObj.GetComponent<Agent>();
			var shapeInfo = (CircleShapeInfo)rigidBody.Shapes.ElementAt(0);
			agent.Radius = shapeInfo.Radius;

			rigidBody.AngularVelocity = 0f;
		}
	}
}
