using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;

using Duality.Drawing;

namespace Duality
{
	/// <summary>
	/// Represents a point in space in the context of <see cref="VisualLog">visual logging</see>. Points do not have a physical size 
	/// and thus are displayed invariantly to parent or perspective scale.
	/// </summary>
	public sealed class VisualLogPointEntry : VisualLogEntry
	{
		private	Vector3 pos;

		/// <summary>
		/// [GET / SET] The points spatial location.
		/// </summary>
		public Vector3 Pos
		{
			get { return this.pos; }
			set { this.pos = value; }
		}

		public override void Draw(Canvas target, Vector3 basePos, float baseRotation, float baseScale)
		{
			float circleRadius = 5.0f;
			float borderRadius = DefaultOutlineWidth;

			// Scale anti-proportional to perspective scale in order to keep a constant size 
			// in screen space even when actually drawing in world space.
			{
				float scale = 1.0f;
				Vector3 posTemp = this.pos + basePos;
				target.DrawDevice.PreprocessCoords(ref posTemp, ref scale);
				circleRadius /= scale;
				borderRadius /= scale;
			}

			// Determine circle position
			Vector3 circlePos = this.pos;
			MathF.TransformCoord(ref circlePos.X, ref circlePos.Y, baseRotation, baseScale);
			circlePos += basePos;

			// Draw circle
			target.State.ColorTint *= this.Color;
			target.FillCircle(
				circlePos.X, 
				circlePos.Y, 
				circlePos.Z, 
				circleRadius - borderRadius * 0.5f);

			// Draw circle outline
			if (target.DrawDevice.DepthWrite) target.State.ZOffset -= 1;
			target.State.ColorTint *= ColorRgba.Black;
			target.FillCircleSegment(
				circlePos.X, 
				circlePos.Y, 
				circlePos.Z, 
				circleRadius, 
				0.0f, 
				MathF.RadAngle360, 
				borderRadius);
		}
	}
}
