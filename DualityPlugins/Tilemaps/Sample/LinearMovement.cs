using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

using Duality;
using Duality.Components;
using Duality.Editor;
using Duality.Drawing;
using Duality.Resources;
using Duality.Plugins.Tilemaps;

namespace Duality.Plugins.Tilemaps.Sample
{
	[EditorHintCategory("Test")]
	[RequiredComponent(typeof(Transform))]
	public class LinearMovement : Component, ICmpUpdatable
	{
		private Vector3 velocity;

		public Vector3 Velocity
		{
			get { return this.velocity; }
			set { this.velocity = value; }
		}

		void ICmpUpdatable.OnUpdate()
		{
			this.GameObj.Transform.MoveBy(this.velocity * Time.TimeMult);
		}
	}
}
