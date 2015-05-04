using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Duality;
using Duality.Drawing;
using Duality.Components;
using Duality.Components.Renderers;

namespace DualStickSpaceShooter
{
	[RequiredComponent(typeof(SpriteRenderer))]
	public class SpriteDepthColor : Component, ICmpEditorUpdatable
	{
		private ColorRgba baseColor;

		public ColorRgba BaseColor
		{
			get { return this.baseColor; }
			set { this.baseColor = value; }
		}

		void ICmpEditorUpdatable.OnUpdate()
		{
			Transform transform = this.GameObj.Transform;
			SpriteRenderer sprite = this.GameObj.GetComponent<SpriteRenderer>();

			float brightness = MathF.Clamp(500.0f / MathF.Max(1.0f, 500.0f + transform.Pos.Z), 0.0f, 1.0f);
			sprite.ColorTint = (baseColor * brightness).WithAlpha(this.baseColor.A);
		}
	}
}
