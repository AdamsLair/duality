using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;

using Duality.Drawing;

namespace Duality
{
	/// <summary>
	/// Represents a vector based on a certain space origin in the context of <see cref="VisualLog">visual logging</see>.
	/// </summary>
	public sealed class VisualLogVectorEntry : VisualLogEntry
	{
		private	Vector3		origin			= Vector3.Zero;
		private	Vector2		vec				= -Vector2.UnitY * 50.0f;
		private	bool		invariantScale	= false;

		/// <summary>
		/// [GET / SET] The vectors origin in space.
		/// </summary>
		public Vector3 Origin
		{
			get { return this.origin; }
			set { this.origin = value; }
		}
		/// <summary>
		/// [GET / SET] The vector to display.
		/// </summary>
		public Vector2 Vector
		{
			get { return this.vec; }
			set { this.vec = value; }
		}
		/// <summary>
		/// [GET / SET] Whether or not the vector will be displayed at a constant size regardless of perspective scale.
		/// </summary>
		public bool InvariantScale
		{
			get { return this.invariantScale; }
			set { this.invariantScale = value; }
		}

		public override void Draw(Canvas target, Vector3 basePos, float baseRotation, float baseScale)
		{
			float originRadius = 5.0f;
			float vectorThickness = 4.0f;
			float borderRadius = DefaultOutlineWidth;
			float vectorLengthFactor = 1.0f;

			// Scale anti-proportional to perspective scale in order to keep a constant size 
			// in screen space even when actually drawing in world space.
			{
				float scale = 1.0f;
				Vector3 posTemp = this.origin + basePos;
				target.DrawDevice.PreprocessCoords(ref posTemp, ref scale);
				originRadius /= scale;
				borderRadius /= scale;
				vectorThickness /= scale;
				if (this.invariantScale) vectorLengthFactor /= scale;
			}


			// Determine base and target positions
			Vector3 originPos = this.origin;
			Vector3 targetPos = this.origin + new Vector3(this.vec * vectorLengthFactor);
			MathF.TransformCoord(ref originPos.X, ref originPos.Y, baseRotation, baseScale);
			MathF.TransformCoord(ref targetPos.X, ref targetPos.Y, baseRotation, baseScale);
			originPos += basePos;
			targetPos += basePos;

			// Downscale vector arrow, if too small for display otherwise
			float vectorLen = (targetPos.Xy - originPos.Xy).Length;
			float vectorLenScale = MathF.Clamp(vectorLen / (vectorThickness * 7.0f), 0.0f, 1.0f);
			vectorThickness *= vectorLenScale;

			// Create arrow polygon
			Vector2 dirForward = (targetPos.Xy - originPos.Xy).Normalized;
			Vector2 dirLeft = dirForward.PerpendicularLeft;
			Vector2 dirRight = -dirLeft;
			Vector2[] arrow = new Vector2[7];
			arrow[0] = dirLeft * vectorThickness * 0.5f;
			arrow[1] = dirLeft * vectorThickness * 0.5f + dirForward * (vectorLen - vectorThickness * 2);
			arrow[2] = dirLeft * vectorThickness * 1.25f + dirForward * (vectorLen - vectorThickness * 2);
			arrow[3] = dirForward * vectorLen;
			arrow[4] = dirRight * vectorThickness * 1.25f + dirForward * (vectorLen - vectorThickness * 2);
			arrow[5] = dirRight * vectorThickness * 0.5f + dirForward * (vectorLen - vectorThickness * 2);
			arrow[6] = dirRight * vectorThickness * 0.5f;
			Vector2[] arrowHead = new Vector2[3];
			arrowHead[0] = arrow[2];
			arrowHead[1] = arrow[3];
			arrowHead[2] = arrow[4];
			Vector2[] arrowLine = new Vector2[4];
			arrowLine[0] = arrow[0];
			arrowLine[1] = arrow[1];
			arrowLine[2] = arrow[5];
			arrowLine[3] = arrow[6];

			// Draw vector and outline
			ColorRgba areaColor = target.State.ColorTint * this.Color;
			ColorRgba outlineColor = areaColor * ColorRgba.Black;
			target.State.ColorTint = areaColor;
			target.FillPolygon(
				arrowLine, 
				originPos.X, 
				originPos.Y, 
				originPos.Z);
			target.FillPolygon(
				arrowHead, 
				originPos.X, 
				originPos.Y, 
				originPos.Z);
			if (target.DrawDevice.DepthWrite) target.State.ZOffset -= 0.1f;
			target.State.ColorTint = outlineColor;
			target.FillPolygonOutline(
				arrow, 
				borderRadius,
				originPos.X, 
				originPos.Y, 
				originPos.Z);

			// Draw origin and outline
			if (target.DrawDevice.DepthWrite) target.State.ZOffset -= 0.1f;
			target.State.ColorTint = areaColor;
			target.FillCircle(
				originPos.X, 
				originPos.Y, 
				originPos.Z, 
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
		}
	}
}
