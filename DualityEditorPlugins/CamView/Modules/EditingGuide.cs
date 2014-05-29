using System;
using System.Collections.Generic;
using System.Linq;

using Duality;

using OpenTK;

namespace Duality.Editor.Plugins.CamView
{
	public class EditingGuide
	{
		private	Vector3		gridSize		= Vector3.Zero;
		private	Vector3		snapPosOrigin	= Vector3.Zero;
		private	Vector3		snapScaleOrigin	= Vector3.One;


		public Vector3 GridSize
		{
			get { return this.gridSize; }
			set { this.gridSize = value; }
		}
		public Vector3 SnapPosOrigin
		{
			get { return this.snapPosOrigin; }
			set { this.snapPosOrigin = value; }
		}
		public Vector3 SnapScaleOrigin
		{
			get { return this.snapScaleOrigin; }
			set { this.snapScaleOrigin = value; }
		}
		

		/// <summary>
		/// Snaps the specified world position to this editing guide.
		/// </summary>
		/// <param name="pos"></param>
		/// <returns></returns>
		public Vector3 SnapPosition(Vector3 pos)
		{
			pos -= this.snapPosOrigin;
			pos /= this.snapScaleOrigin;
			return this.snapPosOrigin + this.snapScaleOrigin * new Vector3(
				this.gridSize.X > 0.001f ? this.gridSize.X * (int)(pos.X / this.gridSize.X) : pos.X,
				this.gridSize.Y > 0.001f ? this.gridSize.Y * (int)(pos.Y / this.gridSize.Y) : pos.Y,
				this.gridSize.Z > 0.001f ? this.gridSize.Z * (int)(pos.Z / this.gridSize.Z) : pos.Z);
		}
		/// <summary>
		/// Snaps the specified world position to this editing guide.
		/// </summary>
		/// <param name="pos"></param>
		/// <returns></returns>
		public Vector2 SnapPosition(Vector2 pos)
		{
			return this.SnapPosition(new Vector3(pos)).Xy;
		}

		/// <summary>
		/// Snaps the specified size value according to match this editing guide.
		/// </summary>
		/// <param name="size"></param>
		/// <returns></returns>
		public Vector3 SnapSize(Vector3 size)
		{
			size /= this.snapScaleOrigin;
			return this.snapScaleOrigin * new Vector3(
				this.gridSize.X > 0.001f ? this.gridSize.X * Math.Max(1, (int)(size.X / this.gridSize.X)) : size.X,
				this.gridSize.Y > 0.001f ? this.gridSize.Y * Math.Max(1, (int)(size.Y / this.gridSize.Y)) : size.Y,
				this.gridSize.Z > 0.001f ? this.gridSize.Z * Math.Max(1, (int)(size.Z / this.gridSize.Z)) : size.Z);
		}
		/// <summary>
		/// Snaps the specified size value according to match this editing guide.
		/// </summary>
		/// <param name="size"></param>
		/// <returns></returns>
		public Vector2 SnapSize(Vector2 size)
		{
			return this.SnapSize(new Vector3(size)).Xy;
		}
		/// <summary>
		/// Snaps the specified size value according to match this editing guide.
		/// </summary>
		/// <param name="size"></param>
		/// <returns></returns>
		public float SnapSize(float size)
		{
			float snapOrigin = 1.0f;
			float snapStep = 0.001f;
			if (this.gridSize.X > snapStep)
			{
				snapStep = this.gridSize.X;
				snapOrigin = this.snapScaleOrigin.X;
			}
			if (this.gridSize.Y > snapStep)
			{
				snapStep = this.gridSize.Y;
				snapOrigin = this.snapScaleOrigin.Y;
			}
			if (this.gridSize.Z > snapStep)
			{
				snapStep = this.gridSize.Z;
				snapOrigin = this.snapScaleOrigin.Z;
			}
			return snapStep > 0.001f ? snapOrigin * snapStep * Math.Max(1, (int)((size / snapOrigin) / snapStep)) : size;
		}
	}
}
