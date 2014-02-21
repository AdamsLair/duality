using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;

using Duality.Drawing;

namespace Duality
{
	/// <summary>
	/// Represents a connection between two points in space in the context of <see cref="VisualLog">visual logging</see>.
	/// </summary>
	public sealed class VisualLogConnectionEntry : VisualLogEntry
	{
		private	Vector3 posA;
		private	Vector3 posB;
		
		/// <summary>
		/// [GET / SET] The first points spatial location.
		/// </summary>
		public Vector3 PosA
		{
			get { return this.posA; }
			set { this.posA = value; }
		}
		/// <summary>
		/// [GET / SET] The second points spatial location.
		/// </summary>
		public Vector3 PosB
		{
			get { return this.posB; }
			set { this.posB = value; }
		}

		public override void Draw(Canvas target, Vector3 basePos, float baseRotation, float baseScale)
		{
			float originRadius = 5.0f;
			float vectorThickness = 4.0f;
			float borderRadius = DefaultOutlineWidth;

			// Scale anti-proportional to perspective scale in order to keep a constant size 
			// in screen space even when actually drawing in world space.
			{
				float scale = 1.0f;
				Vector3 posTemp = this.posA + basePos;
				target.DrawDevice.PreprocessCoords(ref posTemp, ref scale);
				originRadius /= scale;
				borderRadius /= scale;
				vectorThickness /= scale;
			}


			// Determine base and target positions
			Vector3 originPos = this.posA;
			Vector3 targetPos = this.posB;
			MathF.TransformCoord(ref originPos.X, ref originPos.Y, baseRotation, baseScale);
			MathF.TransformCoord(ref targetPos.X, ref targetPos.Y, baseRotation, baseScale);
			originPos += basePos;
			targetPos += basePos;

			// Create connection polygon
			float vectorLen = (targetPos.Xy - originPos.Xy).Length;
			Vector2 dirForward = (targetPos.Xy - originPos.Xy).Normalized;
			Vector2 dirLeft = dirForward.PerpendicularLeft;
			Vector2 dirRight = -dirLeft;
			Vector2[] connection = new Vector2[4];
			connection[0] = dirLeft * vectorThickness * 0.5f;
			connection[1] = dirLeft * vectorThickness * 0.5f + dirForward * vectorLen;
			connection[2] = dirRight * vectorThickness * 0.5f + dirForward * vectorLen;
			connection[3] = dirRight * vectorThickness * 0.5f;

			// Draw vector and outline
			ColorRgba areaColor = target.State.ColorTint * this.Color;
			ColorRgba outlineColor = areaColor * ColorRgba.Black;
			target.State.ColorTint = areaColor;
			target.FillPolygon(
				connection, 
				originPos.X, 
				originPos.Y, 
				originPos.Z);
			if (target.DrawDevice.DepthWrite) target.State.ZOffset -= 0.1f;
			target.State.ColorTint = outlineColor;
			target.FillPolygonOutline(
				connection, 
				borderRadius,
				originPos.X, 
				originPos.Y, 
				originPos.Z);

			// Draw connection points and outline
			if (target.DrawDevice.DepthWrite) target.State.ZOffset -= 0.1f;
			target.State.ColorTint = areaColor;
			target.FillCircle(
				originPos.X, 
				originPos.Y, 
				originPos.Z, 
				originRadius - borderRadius * 0.5f);
			target.FillCircle(
				targetPos.X, 
				targetPos.Y, 
				targetPos.Z, 
				originRadius - borderRadius * 0.5f);
			if (target.DrawDevice.DepthWrite) target.State.ZOffset -= 0.1f;
			target.State.ColorTint = outlineColor;
			target.FillCircleSegment(
				originPos.X, 
				originPos.Y, 
				originPos.Z, 
				originRadius, 
				0.0f, 
				MathF.RadAngle360, 
				borderRadius);
			target.FillCircleSegment(
				targetPos.X, 
				targetPos.Y, 
				targetPos.Z, 
				originRadius, 
				0.0f, 
				MathF.RadAngle360, 
				borderRadius);
		}
	}
}
