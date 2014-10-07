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
			// If the object we're controlling has been destroyed, forget about it
			if (this.controlObj != null && this.controlObj.Disposed)
				this.controlObj = null;
			
			// See what player inputs there are to handle
			if (this.input == null) this.input = new InputMapping();
			if (this.controlObj != null)
			{
				this.input.Update(this.controlObj.GameObj.Transform);
			}
			else
			{
				this.input.Update(null);
			}

			// Control the object this player is supposed to
			if (this.controlObj != null)
			{
				// Apply control inputs to the controlled object
				this.controlObj.TargetAngle = this.input.ControlLookAngle;
				this.controlObj.TargetAngleRatio = this.input.ControlLookSpeed;
				this.controlObj.TargetThrust = this.input.ControlMovement;
				if (this.input.ControlFireWeapon)
					this.controlObj.FireWeapon();
			}

			// Quit the game when requested
			if (this.input.ControlQuit)
				DualityApp.Terminate();
		}
	}
}
