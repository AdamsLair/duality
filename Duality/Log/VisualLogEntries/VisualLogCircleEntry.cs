using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;

using Duality.Drawing;

namespace Duality
{
	/// <summary>
	/// Represents a circular area in space in the context of <see cref="VisualLog">visual logging</see>.
	/// </summary>
	public sealed class VisualLogCircleEntry : VisualLogEntry
	{
		private	Vector3 pos				= Vector3.Zero;
		private	bool	invariantScale	= false;
		private	float	radius			= 100.0f;
		private	float	minAngle		= 0.0f;
		private	float	maxAngle		= MathF.RadAngle360;
		
		/// <summary>
		/// [GET / SET] The circular areas center location.
		/// </summary>
		public Vector3 Pos
		{
			get { return this.pos; }
			set { this.pos = value; }
		}
		/// <summary>
		/// [GET / SET] The circles radius.
		/// </summary>
		public float Radius
		{
			get { return this.radius; }
			set { this.radius = value; }
		}
		/// <summary>
		/// [GET / SET] The minimum angle of the displayed circle area segment.
		/// </summary>
		public float MinAngle
		{
			get { return this.minAngle; }
			set { this.minAngle = value; }
		}
		/// <summary>
		/// [GET / SET] The maximum angle of the displayed circle area segment.
		/// </summary>
		public float MaxAngle
		{
			get { return this.maxAngle; }
			set { this.maxAngle = value; }
		}
		/// <summary>
		/// [GET / SET] Whether or not the circle area will be displayed at a constant size regardless of perspective scale.
		/// </summary>
		public bool InvariantScale
		{
			get { return this.invariantScale; }
			set { this.invariantScale = value; }
		}

		public override void Draw(Canvas target, Vector3 basePos, float baseRotation, float baseScale)
		{
			float borderRadius = DefaultOutlineWidth;
			float circleRadius = this.radius * baseScale;

			// Scale anti-proportional to perspective scale in order to keep a constant size 
			// in screen space even when actually drawing in world space.
			{
				float scale = 1.0f;
				Vector3 posTemp = this.pos + basePos;
				target.DrawDevice.PreprocessCoords(ref posTemp, ref scale);
				borderRadius /= scale;
				if (this.invariantScale) circleRadius /= scale;
			}

			// Determine circle position
			Vector3 circlePos = this.pos;
			MathF.TransformCoord(ref circlePos.X, ref circlePos.Y, baseRotation, baseScale);
			circlePos += basePos;

			// Draw circle
			target.State.ColorTint *= this.Color;
			target.FillCircleSegment(
				circlePos.X, 
				circlePos.Y, 
				circlePos.Z, 
				circleRadius - borderRadius * 0.5f,
				this.minAngle + baseRotation,
				this.maxAngle + baseRotation);

			// Draw circle outline
			if (target.DrawDevice.DepthWrite) target.State.ZOffset -= 0.1f;
			target.State.ColorTint *= ColorRgba.Black;
			target.FillCircleSegment(
				circlePos.X, 
				circlePos.Y, 
				circlePos.Z, 
				circleRadius, 
				this.minAngle + baseRotation, 
				this.maxAngle + baseRotation, 
				borderRadius);
			if (MathF.CircularDist(this.minAngle, this.maxAngle) > MathF.RadAngle1 * 0.001f)
			{
				Vector2 minAngleVec = Vector2.FromAngleLength(this.minAngle + baseRotation, circleRadius);
				Vector2 maxAngleVec = Vector2.FromAngleLength(this.maxAngle + baseRotation, circleRadius);
				target.FillThickLine(
					circlePos.X, 
					circlePos.Y, 
					circlePos.Z,
					circlePos.X + minAngleVec.X, 
					circlePos.Y + minAngleVec.Y, 
					circlePos.Z,
					borderRadius);
				target.FillThickLine(
					circlePos.X, 
					circlePos.Y, 
					circlePos.Z,
					circlePos.X + maxAngleVec.X, 
					circlePos.Y + maxAngleVec.Y, 
					circlePos.Z,
					borderRadius);
			}
		}
	}
}
