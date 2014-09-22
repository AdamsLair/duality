using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;
using OpenTK.Input;

using Duality;
using Duality.Components;
using Duality.Resources;
using Duality.Drawing;

namespace DualStickSpaceShooter
{
	[Serializable]
	public class Player : Component, ICmpUpdatable
	{
		private	Ship			controlObj	= null;
		private	ColorRgba		color		= ColorRgba.White;

		[NonSerialized]
		private	InputMapping	input		= null;


		public InputMethod InputMethod
		{
			get { return this.input != null ? this.input.Method : InputMethod.Unknown; }
			set
			{
				if (this.input == null) this.input = new InputMapping();
				this.input.Method = value;
			}
		}
		public Ship ControlObject
		{
			get { return this.controlObj; }
			set
			{
				this.controlObj = value;
				if (this.controlObj != null)
					this.controlObj.UpdatePlayerColor();
			}
		}
		public ColorRgba Color
		{
			get { return this.color; }
			set
			{
				this.color = value;
				if (this.controlObj != null)
					this.controlObj.UpdatePlayerColor();
			}
		}

		void ICmpUpdatable.OnUpdate()
		{
			if (this.controlObj != null)
			{
				if (this.input == null) this.input = new InputMapping();

				this.input.Update(this.controlObj.GameObj.Transform);
				
				this.controlObj.TargetAngle = this.input.ControlLookAngle;
				this.controlObj.TargetThrust = this.input.ControlMovement;
				if (this.input.ControlFireWeapon)
					this.controlObj.FireWeapon();
			}
		}
	}
}
