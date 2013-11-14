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

using System.Diagnostics;
using FarseerPhysics.Common;
using OpenTK;

namespace FarseerPhysics.Collision.Shapes
{
    /// <summary>
    /// A loop Shape is a free form sequence of line segments that form a circular list.
    /// The loop may cross upon itself, but this is not recommended for smooth collision.
    /// The loop has double sided collision, so you can use inside and outside collision.
    /// Therefore, you may use any winding order.
    /// </summary>
    public class LoopShape : Shape
    {
        private static EdgeShape _edgeShape = new EdgeShape();

        /// <summary>
        /// The vertices. These are not owned/freed by the loop Shape.
        /// </summary>
        public Vertices Vertices;

        private LoopShape()
            : base(0)
        {
            ShapeType = ShapeType.Loop;
            _radius = Settings.PolygonRadius;
        }

        public LoopShape(Vertices vertices)
            : base(0)
        {
            ShapeType = ShapeType.Loop;
            _radius = Settings.PolygonRadius;

            if (Settings.ConserveMemory)
                Vertices = vertices;
            else
                // Copy vertices.
                Vertices = new Vertices(vertices);
        }

        public override int ChildCount
        {
            get { return Vertices.Count; }
        }

        public override Shape Clone()
        {
            LoopShape loop = new LoopShape();
            loop._density = _density;
            loop._radius = _radius;
            loop.Vertices = Vertices;
            loop.MassData = MassData;
            return loop;
        }

        /// <summary>
        /// Get a child edge.
        /// </summary>
        /// <param name="edge">The edge.</param>
        /// <param name="index">The index.</param>
        public void GetChildEdge(ref EdgeShape edge, int index)
        {
            Debug.Assert(2 <= Vertices.Count);
            Debug.Assert(0 <= index && index < Vertices.Count);
            edge.ShapeType = ShapeType.Edge;
            edge._radius = _radius;

            int i0 = index - 1 >= 0 ? index - 1 : Vertices.Count - 1;
            int i1 = index;
            int i2 = index + 1 < Vertices.Count ? index + 1 : 0;
            int i3 = index + 2;
            while (i3 >= Vertices.Count)
            {
                i3 -= Vertices.Count;
            }

            edge.Vertex0 = Vertices[i0];
            edge.Vertex1 = Vertices[i1];
            edge.Vertex2 = Vertices[i2];
            edge.Vertex3 = Vertices[i3];

			edge.HasVertex0 = Vector2.CalculateAngle(edge.Vertex0, edge.Vertex1) > MathHelper.PiOver2;
			edge.HasVertex3 = Vector2.CalculateAngle(edge.Vertex2, edge.Vertex3) > MathHelper.PiOver2;
        }

        /// <summary>
        /// Test a point for containment in this shape. This only works for convex shapes.
        /// </summary>
        /// <param name="transform">The shape world transform.</param>
        /// <param name="point">a point in world coordinates.</param>
        /// <returns>True if the point is inside the shape</returns>
        public override bool TestPoint(ref Transform transform, ref Vector2 point)
        {
            return false;
        }

        /// <summary>
        /// Cast a ray against a child shape.
        /// </summary>
        /// <param name="output">The ray-cast results.</param>
        /// <param name="input">The ray-cast input parameters.</param>
        /// <param name="transform">The transform to be applied to the shape.</param>
        /// <param name="childIndex">The child shape index.</param>
        /// <returns>True if the ray-cast hits the shape</returns>
        public override bool RayCast(out RayCastOutput output, ref RayCastInput input,
                                     ref Transform transform, int childIndex)
        {
            Debug.Assert(childIndex < Vertices.Count);

            int i1 = childIndex;
            int i2 = childIndex + 1;
            if (i2 == Vertices.Count)
            {
                i2 = 0;
            }

            _edgeShape.Vertex1 = Vertices[i1];
            _edgeShape.Vertex2 = Vertices[i2];

            return _edgeShape.RayCast(out output, ref input, ref transform, 0);
        }

        /// <summary>
        /// Given a transform, compute the associated axis aligned bounding box for a child shape.
        /// </summary>
        /// <param name="aabb">The aabb results.</param>
        /// <param name="transform">The world transform of the shape.</param>
        /// <param name="childIndex">The child shape index.</param>
        public override void ComputeAABB(out AABB aabb, ref Transform transform, int childIndex)
        {
            Debug.Assert(childIndex < Vertices.Count);

            int i1 = childIndex;
            int i2 = childIndex + 1;
            if (i2 == Vertices.Count)
            {
                i2 = 0;
            }

            Vector2 v1 = MathUtils.Multiply(ref transform, Vertices[i1]);
            Vector2 v2 = MathUtils.Multiply(ref transform, Vertices[i2]);

            aabb.LowerBound = Vector2.ComponentMin(v1, v2);
            aabb.UpperBound = Vector2.ComponentMax(v1, v2);
        }

        /// <summary>
        /// Chains have zero mass.
        /// </summary>
        public override void ComputeProperties()
        {
            //Does nothing. Loop shapes don't have properties.
        }

        public override float ComputeSubmergedArea(Vector2 normal, float offset, Transform xf, out Vector2 sc)
        {
            sc = Vector2.Zero;
            return 0;
        }
    }
}