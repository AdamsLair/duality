using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;
using OpenTK.Input;

using Duality;
using Duality.Editor;
using Duality.Components;
using Duality.Resources;

namespace DualStickSpaceShooter
{
	[Serializable]
	[RequiredComponent(typeof(Camera))]
	public class CameraController : Component, ICmpUpdatable
	{
		private	List<Transform>	followObjects	= null;
		private	float			softness		= 1.0f;

		public List<Transform> FollowObjects
		{
			get { return this.followObjects; }
			set { this.followObjects = value; }
		}
		[EditorHintRange(0.0f, 2.0f)]
		public float Softness
		{
			get { return this.softness; }
			set { this.softness = value; }
		}

		void ICmpUpdatable.OnUpdate()
		{
			if (this.followObjects == null) return;
			if (this.followObjects.Count == 0) return;

			Camera camera = this.GameObj.Camera;
			Transform transform = this.GameObj.Transform;

			Vector3 focusPos = Vector3.Zero;
			foreach (Transform obj in this.followObjects)
			{
				focusPos += obj.Pos;
			}
			focusPos /= this.followObjects.Count;

			Vector3 targetPos = focusPos - new Vector3(0.0f, 0.0f, camera.FocusDist);
			transform.MoveByAbs((targetPos - transform.Pos) * MathF.Pow(10.0f, -this.softness) * Time.TimeMult);
		}
	}
}
