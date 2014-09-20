using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;
using OpenTK.Input;

using Duality;
using Duality.Components;
using Duality.Resources;

namespace DualStickSpaceShooter
{
	[Serializable]
	public class Player : Component, ICmpUpdatable
	{
		private	Ship	controlObj	= null;

		public Ship ControlObject
		{
			get { return this.controlObj; }
			set { this.controlObj = value; }
		}

		void ICmpUpdatable.OnUpdate()
		{
			if (this.controlObj != null)
			{
				Camera mainCamera = Scene.Current.FindComponent<Camera>();
				Vector3 objPos = this.controlObj.GameObj.Transform.Pos;
				Vector2 objPosOnScreen = mainCamera.GetScreenCoord(objPos).Xy;
				float mouseAngle = (DualityApp.Mouse.Pos - objPosOnScreen).Angle;

				Vector2 thrust = Vector2.Zero;
				if (DualityApp.Keyboard[Key.W])
					thrust += new Vector2(0.0f, -1.0f);
				if (DualityApp.Keyboard[Key.A])
					thrust += new Vector2(-1.0f, 0.0f);
				if (DualityApp.Keyboard[Key.S])
					thrust += new Vector2(0.0f, 1.0f);
				if (DualityApp.Keyboard[Key.D])
					thrust += new Vector2(1.0f, 0.0f);

				if (DualityApp.Mouse[MouseButton.Left])
					this.controlObj.FireWeapon();

				this.controlObj.TargetAngle = mouseAngle;
				this.controlObj.TargetThrust = thrust;
			}
		}
	}
}
