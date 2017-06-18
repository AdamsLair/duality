using System;
using System.Collections.Generic;
using System.Linq;

using FarseerPhysics.Dynamics;
using FarseerPhysics.Common;
using FarseerPhysics.Collision.Shapes;

using Duality.Editor;

namespace Duality.Components.Physics
{
	public abstract class VertexBasedShapeInfo : ShapeInfo
	{
		protected Vector2[] vertices;


		/// <summary>
		/// [GET / SET] The vertices that describe this shape. 
		/// While assinging the array will cause an automatic update, simply 
		/// modifying it will require you to call <see cref="ShapeInfo.UpdateShape"/> manually.
		/// </summary>
		[EditorHintFlags(MemberFlags.ForceWriteback)]
		[EditorHintIncrement(1)]
		[EditorHintDecimalPlaces(1)]
		public Vector2[] Vertices
		{
			get { return this.vertices; }
			set
			{
				this.vertices = value ?? new Vector2[] { Vector2.Zero, Vector2.UnitX, Vector2.UnitY };
				this.UpdateInternalShape(true);
			}
		}
		/// <summary>
		/// [GET] A flagged enum describing traits of the geometry that is formed by this shapes <see cref="Vertices"/>.
		/// </summary>
		public abstract VertexShapeTrait ShapeTraits { get; }
		[EditorHintFlags(MemberFlags.Invisible)]
		public override Rect AABB
		{
			get 
			{
				float minX = float.MaxValue;
				float minY = float.MaxValue;
				float maxX = float.MinValue;
				float maxY = float.MinValue;
				for (int i = 0; i < this.vertices.Length; i++)
				{
					minX = MathF.Min(minX, this.vertices[i].X);
					minY = MathF.Min(minY, this.vertices[i].Y);
					maxX = MathF.Max(maxX, this.vertices[i].X);
					maxY = MathF.Max(maxY, this.vertices[i].Y);
				}
				return new Rect(minX, minY, maxX - minX, maxY - minY);
			}
		}


		protected VertexBasedShapeInfo() { }
		protected VertexBasedShapeInfo(Vector2[] vertices)
		{
			this.vertices = vertices;
		}
	}
}
