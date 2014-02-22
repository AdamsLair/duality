using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;

using Duality.Drawing;

namespace Duality
{
	/// <summary>
	/// Represents a polygon in the context of <see cref="VisualLog">visual logging</see>.
	/// </summary>
	public sealed class VisualLogPolygonEntry : VisualLogEntry
	{
		private	Vector3		pos				= Vector3.Zero;
		private	Vector2[]	vertices		= null;
		private	bool		invariantScale	= false;
		
		/// <summary>
		/// [GET / SET] The polygons origin in space.
		/// </summary>
		public Vector3 Pos
		{
			get { return this.pos; }
			set { this.pos = value; }
		}
		/// <summary>
		/// [GET / SET] The vertices that form the displayed polygon.
		/// </summary>
		public Vector2[] Vertices
		{
			get { return this.vertices; }
			set { this.vertices = value; }
		}
		/// <summary>
		/// [GET / SET] Whether or not the polygon area will be displayed at a constant size regardless of perspective scale.
		/// </summary>
		public bool InvariantScale
		{
			get { return this.invariantScale; }
			set { this.invariantScale = value; }
		}

		public override void Draw(Canvas target, Vector3 basePos, float baseRotation, float baseScale)
		{
			float borderRadius = DefaultOutlineWidth;
			float polyScale = 1.0f;

			// Scale anti-proportional to perspective scale in order to keep a constant size 
			// in screen space even when actually drawing in world space.
			{
				float scale = 1.0f;
				Vector3 posTemp = this.pos + basePos;
				target.DrawDevice.PreprocessCoords(ref posTemp, ref scale);
				borderRadius /= scale;
				if (this.invariantScale) polyScale /= scale;
			}

			// Determine base position
			Vector3 circlePos = this.pos;
			MathF.TransformCoord(ref circlePos.X, ref circlePos.Y, baseRotation, baseScale);
			circlePos += basePos;

			// Draw polygon and outline
			target.State.ColorTint *= this.Color;
			target.State.TransformAngle = baseRotation;
			target.State.TransformScale = new Vector2(baseScale, baseScale) * polyScale;
			target.FillPolygon(
				this.vertices,
				circlePos.X, 
				circlePos.Y, 
				circlePos.Z);
			if (target.DrawDevice.DepthWrite) target.State.ZOffset -= 0.1f;
			target.State.ColorTint *= ColorRgba.Black;
			target.FillPolygonOutline(
				this.vertices,
				borderRadius / polyScale,
				circlePos.X, 
				circlePos.Y, 
				circlePos.Z);
		}
	}
}
