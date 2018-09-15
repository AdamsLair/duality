/*
* Farseer Physics Engine based on Box2D.XNA port:
* Copyright (c) 2010 Ian Qvist
* 
* Box2D.XNA port of Box2D:
* Copyright (c) 2009 Brandon Furtwangler, Nathan Furtwangler
*
* Original source Box2D:
* Copyright (c) 2006-2009 Erin Catto http://www.gphysics.com 
* 
* This software is provided 'as-is', without any express or implied 
* warranty.  In no event will the authors be held liable for any damages 
* arising from the use of this software. 
* Permission is granted to anyone to use this software for any purpose, 
* including commercial applications, and to alter it and redistribute it 
* freely, subject to the following restrictions: 
* 1. The origin of this software must not be misrepresented; you must not 
* claim that you wrote the original software. If you use this software 
* in a product, an acknowledgment in the product documentation would be 
* appreciated but is not required. 
* 2. Altered source versions must be plainly marked as such, and must not be 
* misrepresented as being the original software. 
* 3. This notice may not be removed or altered from any source distribution. 
*/

using System;
using FarseerPhysics.Common;
using Duality;

namespace FarseerPhysics.Collision.Shapes
{
	public class CircleShape : Shape
	{
		internal Vector2 _position;

		public CircleShape(float radius, float density)
			: base(density)
		{
			this.ShapeType = ShapeType.Circle;
			this._radius = radius;
			this._position = Vector2.Zero;
			ComputeProperties();
		}

		internal CircleShape()
			: base(0)
		{
			this.ShapeType = ShapeType.Circle;
			this._radius = 0.0f;
			this._position = Vector2.Zero;
		}

		public override int ChildCount
		{
			get { return 1; }
		}

		public Vector2 Position
		{
			get { return this._position; }
			set
			{
				this._position = value;
				ComputeProperties();
			}
		}

		public override Shape Clone()
		{
			CircleShape shape = new CircleShape();
			shape._radius = this.Radius;
			shape._density = this._density;
			shape._position = this._position;
			shape.ShapeType = this.ShapeType;
			shape.MassData = this.MassData;
			return shape;
		}

		/// <summary>
		/// Test a point for containment in this shape. This only works for convex shapes.
		/// </summary>
		/// <param name="transform">The shape world transform.</param>
		/// <param name="point">a point in world coordinates.</param>
		/// <returns>True if the point is inside the shape</returns>
		public override bool TestPoint(ref Transform transform, ref Vector2 point)
		{
			Vector2 center = transform.Position + MathUtils.Multiply(ref transform.R, this.Position);
			Vector2 d = point - center;
			return Vector2.Dot(d, d) <= this.Radius * this.Radius;
		}

		/// <summary>
		/// Cast a ray against a child shape.
		/// </summary>
		/// <param name="output">The ray-cast results.</param>
		/// <param name="input">The ray-cast input parameters.</param>
		/// <param name="transform">The transform to be applied to the shape.</param>
		/// <param name="childIndex">The child shape index.</param>
		/// <returns>True if the ray-cast hits the shape</returns>
		public override bool RayCast(out RayCastOutput output, ref RayCastInput input, ref Transform transform,
									 int childIndex)
		{
			// Collision Detection in Interactive 3D Environments by Gino van den Bergen
			// From Section 3.1.2
			// x = s + a * r
			// norm(x) = radius

			output = new RayCastOutput();

			Vector2 position = transform.Position + MathUtils.Multiply(ref transform.R, this.Position);
			Vector2 s = input.Point1 - position;
			float b = Vector2.Dot(s, s) - this.Radius * this.Radius;

			// Solve quadratic equation.
			Vector2 r = input.Point2 - input.Point1;
			float c = Vector2.Dot(s, r);
			float rr = Vector2.Dot(r, r);
			float sigma = c * c - rr * b;

			// Check for negative discriminant and short segment.
			if (sigma < 0.0f || rr < Settings.Epsilon)
			{
				return false;
			}

			// Find the point of intersection of the line with the circle.
			float a = -(c + (float)Math.Sqrt(sigma));

			// Is the intersection point on the segment?
			if (0.0f <= a && a <= input.MaxFraction * rr)
			{
				a /= rr;
				output.Fraction = a;
				Vector2 norm = (s + a * r);
				norm.Normalize();
				output.Normal = norm;
				return true;
			}

			return false;
		}

		/// <summary>
		/// Given a transform, compute the associated axis aligned bounding box for a child shape.
		/// </summary>
		/// <param name="aabb">The aabb results.</param>
		/// <param name="transform">The world transform of the shape.</param>
		/// <param name="childIndex">The child shape index.</param>
		public override void ComputeAABB(out AABB aabb, ref Transform transform, int childIndex)
		{
			Vector2 p = transform.Position + MathUtils.Multiply(ref transform.R, this.Position);
			aabb.LowerBound = new Vector2(p.X - this.Radius, p.Y - this.Radius);
			aabb.UpperBound = new Vector2(p.X + this.Radius, p.Y + this.Radius);
		}

		/// <summary>
		/// Compute the mass properties of this shape using its dimensions and density.
		/// The inertia tensor is computed about the local origin, not the centroid.
		/// </summary>
		public override sealed void ComputeProperties()
		{
			float area = Settings.Pi * this.Radius * this.Radius;
			this.MassData.Area = area;
			this.MassData.Mass = this.Density * area;
			this.MassData.Centroid = this.Position;

			// inertia about the local origin
			this.MassData.Inertia = this.MassData.Mass * (0.5f * this.Radius * this.Radius + Vector2.Dot(this.Position, this.Position));
		}

		public bool CompareTo(CircleShape shape)
		{
			return (this.Radius == shape.Radius &&
					this.Position == shape.Position);
		}

		public override float ComputeSubmergedArea(Vector2 normal, float offset, Transform xf, out Vector2 sc)
		{
			sc = Vector2.Zero;

			Vector2 p = MathUtils.Multiply(ref xf, this.Position);
			float l = -(Vector2.Dot(normal, p) - offset);
			if (l < -this.Radius + Settings.Epsilon)
			{
				//Completely dry
				return 0;
			}
			if (l > this.Radius)
			{
				//Completely wet
				sc = p;
				return Settings.Pi * this.Radius * this.Radius;
			}

			//Magic
			float r2 = this.Radius * this.Radius;
			float l2 = l * l;
			float area = r2 * (float)((Math.Asin(l / this.Radius) + Settings.Pi / 2) + l * Math.Sqrt(r2 - l2));
			float com = -2.0f / 3.0f * (float)Math.Pow(r2 - l2, 1.5f) / area;

			sc.X = p.X + normal.X * com;
			sc.Y = p.Y + normal.Y * com;

			return area;
		}
	}
}