﻿using System;
using System.Collections.Generic;
using System.Text;
using Duality;

namespace FarseerPhysics.Common
{
	//Contributed by Matthew Bettcher

	/// <summary>
	/// Path:
	/// Very similar to Vertices, but this
	/// class contains vectors describing
	/// control points on a Catmull-Rom
	/// curve.
	/// </summary>
	public class Path
	{
		/// <summary>
		/// All the points that makes up the curve
		/// </summary>
		public List<Vector2> ControlPoints;

		private float _deltaT;

		/// <summary>
		/// Initializes a new instance of the <see cref="Path"/> class.
		/// </summary>
		public Path()
		{
			this.ControlPoints = new List<Vector2>();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Path"/> class.
		/// </summary>
		/// <param name="vertices">The vertices to created the path from.</param>
		public Path(Vector2[] vertices)
		{
			this.ControlPoints = new List<Vector2>(vertices.Length);

			for (int i = 0; i < vertices.Length; i++)
			{
				Add(vertices[i]);
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Path"/> class.
		/// </summary>
		/// <param name="vertices">The vertices to created the path from.</param>
		public Path(IList<Vector2> vertices)
		{
			this.ControlPoints = new List<Vector2>(vertices.Count);
			for (int i = 0; i < vertices.Count; i++)
			{
				Add(vertices[i]);
			}
		}

		/// <summary>
		/// True if the curve is closed.
		/// </summary>
		/// <value><c>true</c> if closed; otherwise, <c>false</c>.</value>
		public bool Closed { get; set; }

		/// <summary>
		/// Gets the next index of a controlpoint
		/// </summary>
		/// <param name="index">The index.</param>
		public int NextIndex(int index)
		{
			if (index == this.ControlPoints.Count - 1)
			{
				return 0;
			}
			return index + 1;
		}

		/// <summary>
		/// Gets the previous index of a controlpoint
		/// </summary>
		/// <param name="index">The index.</param>
		public int PreviousIndex(int index)
		{
			if (index == 0)
			{
				return this.ControlPoints.Count - 1;
			}
			return index - 1;
		}

		/// <summary>
		/// Translates the control points by the specified vector.
		/// </summary>
		/// <param name="vector">The vector.</param>
		public void Translate(ref Vector2 vector)
		{
			for (int i = 0; i < this.ControlPoints.Count; i++)
				this.ControlPoints[i] = this.ControlPoints[i] + vector;
		}

		/// <summary>
		/// Scales the control points by the specified vector.
		/// </summary>
		/// <param name="value">The Value.</param>
		public void Scale(ref Vector2 value)
		{
			for (int i = 0; i < this.ControlPoints.Count; i++)
				this.ControlPoints[i] = this.ControlPoints[i] * value;
		}

		/// <summary>
		/// Rotate the control points by the defined value in radians.
		/// </summary>
		/// <param name="value">The amount to rotate by in radians.</param>
		public void Rotate(float value)
		{
			Matrix4 rotationMatrix;
			Matrix4.CreateRotationZ(value, out rotationMatrix);

			for (int i = 0; i < this.ControlPoints.Count; i++)
				this.ControlPoints[i] = Vector2.Transform(this.ControlPoints[i], rotationMatrix);
		}

		public override string ToString()
		{
			StringBuilder builder = new StringBuilder();
			for (int i = 0; i < this.ControlPoints.Count; i++)
			{
				builder.Append(this.ControlPoints[i].ToString());
				if (i < this.ControlPoints.Count - 1)
				{
					builder.Append(" ");
				}
			}
			return builder.ToString();
		}

		/// <summary>
		/// Returns a set of points defining the
		/// curve with the specifed number of divisions
		/// between each control point.
		/// </summary>
		/// <param name="divisions">Number of divisions between each control point.</param>
		public Vertices GetVertices(int divisions)
		{
			Vertices verts = new Vertices();

			float timeStep = 1f / divisions;

			for (float i = 0; i < 1f; i += timeStep)
			{
				verts.Add(GetPosition(i));
			}

			return verts;
		}

		public Vector2 GetPosition(float time)
		{
			Vector2 temp;

			if (this.ControlPoints.Count < 2)
				throw new Exception("You need at least 2 control points to calculate a position.");

			if (this.Closed)
			{
				Add(this.ControlPoints[0]);

				this._deltaT = 1f / (this.ControlPoints.Count - 1);

				int p = (int)(time / this._deltaT);

				// use a circular indexing system
				int p0 = p - 1;
				if (p0 < 0) p0 = p0 + (this.ControlPoints.Count - 1);
				else if (p0 >= this.ControlPoints.Count - 1) p0 = p0 - (this.ControlPoints.Count - 1);
				int p1 = p;
				if (p1 < 0) p1 = p1 + (this.ControlPoints.Count - 1);
				else if (p1 >= this.ControlPoints.Count - 1) p1 = p1 - (this.ControlPoints.Count - 1);
				int p2 = p + 1;
				if (p2 < 0) p2 = p2 + (this.ControlPoints.Count - 1);
				else if (p2 >= this.ControlPoints.Count - 1) p2 = p2 - (this.ControlPoints.Count - 1);
				int p3 = p + 2;
				if (p3 < 0) p3 = p3 + (this.ControlPoints.Count - 1);
				else if (p3 >= this.ControlPoints.Count - 1) p3 = p3 - (this.ControlPoints.Count - 1);

				// relative time
				float lt = (time - this._deltaT * p) / this._deltaT;

				temp = CatmullRom(this.ControlPoints[p0], this.ControlPoints[p1], this.ControlPoints[p2], this.ControlPoints[p3], lt);

				RemoveAt(this.ControlPoints.Count - 1);
			}
			else
			{
				int p = (int)(time / this._deltaT);

				// 
				int p0 = p - 1;
				if (p0 < 0) p0 = 0;
				else if (p0 >= this.ControlPoints.Count - 1) p0 = this.ControlPoints.Count - 1;
				int p1 = p;
				if (p1 < 0) p1 = 0;
				else if (p1 >= this.ControlPoints.Count - 1) p1 = this.ControlPoints.Count - 1;
				int p2 = p + 1;
				if (p2 < 0) p2 = 0;
				else if (p2 >= this.ControlPoints.Count - 1) p2 = this.ControlPoints.Count - 1;
				int p3 = p + 2;
				if (p3 < 0) p3 = 0;
				else if (p3 >= this.ControlPoints.Count - 1) p3 = this.ControlPoints.Count - 1;

				// relative time
				float lt = (time - this._deltaT * p) / this._deltaT;

				temp = CatmullRom(this.ControlPoints[p0], this.ControlPoints[p1], this.ControlPoints[p2], this.ControlPoints[p3], lt);
			}

			return temp;
		}

		/// <summary>
		/// Gets the normal for the given time.
		/// </summary>
		/// <param name="time">The time</param>
		/// <returns>The normal.</returns>
		public Vector2 GetPositionNormal(float time)
		{
			float offsetTime = time + 0.0001f;

			Vector2 a = GetPosition(time);
			Vector2 b = GetPosition(offsetTime);

			Vector2 output, temp;

			Vector2.Subtract(ref a, ref b, out temp);

			output.X = -temp.Y;
			output.Y = temp.X;

			output.Normalize();

			return output;
		}

		public void Add(Vector2 point)
		{
			this.ControlPoints.Add(point);
			this._deltaT = 1f / (this.ControlPoints.Count - 1);
		}

		public void Remove(Vector2 point)
		{
			this.ControlPoints.Remove(point);
			this._deltaT = 1f / (this.ControlPoints.Count - 1);
		}

		public void RemoveAt(int index)
		{
			this.ControlPoints.RemoveAt(index);
			this._deltaT = 1f / (this.ControlPoints.Count - 1);
		}

		public float GetLength()
		{
			List<Vector2> verts = GetVertices(this.ControlPoints.Count * 25);
			float length = 0;

			for (int i = 1; i < verts.Count; i++)
			{
				length += (verts[i - 1] - verts[i]).Length;
			}

			if (this.Closed)
				length += (verts[this.ControlPoints.Count - 1] - verts[0]).Length;

			return length;
		}

		public List<Vector3> SubdivideEvenly(int divisions)
		{
			List<Vector3> verts = new List<Vector3>();

			float length = GetLength();

			float deltaLength = length / divisions + 0.001f;
			float t = 0.000f;

			// we always start at the first control point
			Vector2 start = this.ControlPoints[0];
			Vector2 end = GetPosition(t);

			// increment t until we are at half the distance
			while (deltaLength * 0.5f >= (start - end).Length)
			{
				end = GetPosition(t);
				t += 0.0001f;

				if (t >= 1f)
					break;
			}

			start = end;

			// for each box
			for (int i = 1; i < divisions; i++)
			{
				Vector2 normal = GetPositionNormal(t);
				float angle = (float)Math.Atan2(normal.Y, normal.X);

				verts.Add(new Vector3(end, angle));

				// until we reach the correct distance down the curve
				while (deltaLength >= (start - end).Length)
				{
					end = GetPosition(t);
					t += 0.00001f;

					if (t >= 1f)
						break;
				}
				if (t >= 1f)
					break;

				start = end;
			}
			return verts;
		}

		internal static Vector2 CatmullRom(Vector2 value1, Vector2 value2, Vector2 value3, Vector2 value4, float amount)
		{
			Vector2 vector;
			float num = amount * amount;
			float num2 = amount * num;
			vector.X = 0.5f * ((((2f * value2.X) + ((-value1.X + value3.X) * amount)) + (((((2f * value1.X) - (5f * value2.X)) + (4f * value3.X)) - value4.X) * num)) + ((((-value1.X + (3f * value2.X)) - (3f * value3.X)) + value4.X) * num2));
			vector.Y = 0.5f * ((((2f * value2.Y) + ((-value1.Y + value3.Y) * amount)) + (((((2f * value1.Y) - (5f * value2.Y)) + (4f * value3.Y)) - value4.Y) * num)) + ((((-value1.Y + (3f * value2.Y)) - (3f * value3.Y)) + value4.Y) * num2));
			return vector;
		}
	}
}