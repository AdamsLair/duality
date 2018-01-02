using System;

namespace FarseerPhysics.Common.ConvexHull
{
    public static class GiftWrap
    {
        // From Eric Jordan's convex decomposition library (box2D rev 32)

        /// <summary>
        /// Find the convex hull of a point cloud using "Gift-wrap" algorithm - start
        /// with an extremal point, and walk around the outside edge by testing
        /// angles.
        /// 
        /// Runs in O(N*S) time where S is number of sides of resulting polygon.
        /// Worst case: point cloud is all vertices of convex polygon: O(N^2).
        /// There may be faster algorithms to do this, should you need one -
        /// this is just the simplest. You can get O(N log N) expected time if you
        /// try, I think, and O(N) if you restrict inputs to simple polygons.
        /// Returns null if number of vertices passed is less than 3.
        /// Results should be passed through convex decomposition afterwards
        /// to ensure that each shape has few enough points to be used in Box2d.
        /// 
        /// Warning: May be buggy with colinear points on hull.
        /// </summary>
        /// <param name="vertices">The vertices.</param>
        /// <returns></returns>
        public static Vertices GetConvexHull(Vertices vertices)
        {
            if (vertices.Count < 3)
                return vertices;

            int[] edgeList = new int[vertices.Count];
            int numEdges = 0;

            float minY = float.MaxValue;
            int minYIndex = vertices.Count;
            for (int i = 0; i < vertices.Count; ++i)
            {
                if (vertices[i].Y < minY)
                {
                    minY = vertices[i].Y;
                    minYIndex = i;
                }
            }

            int startIndex = minYIndex;
            int winIndex = -1;
            float dx = -1.0f;
            float dy = 0.0f;
            while (winIndex != minYIndex)
            {
                float maxDot = -2.0f;
                float nrm;

                for (int i = 0; i < vertices.Count; ++i)
                {
                    if (i == startIndex)
                        continue;
                    float newdx = vertices[i].X - vertices[startIndex].X;
                    float newdy = vertices[i].Y - vertices[startIndex].Y;
                    nrm = (float)Math.Sqrt(newdx * newdx + newdy * newdy);
                    nrm = (nrm == 0.0f) ? 1.0f : nrm;
                    newdx /= nrm;
                    newdy /= nrm;

                    //Dot products act as proxy for angle
                    //without requiring inverse trig.
                    float newDot = newdx * dx + newdy * dy;
                    if (newDot > maxDot)
                    {
                        maxDot = newDot;
                        winIndex = i;
                    }
                }
                edgeList[numEdges++] = winIndex;
                dx = vertices[winIndex].X - vertices[startIndex].X;
                dy = vertices[winIndex].Y - vertices[startIndex].Y;
                nrm = (float)Math.Sqrt(dx * dx + dy * dy);
                nrm = (nrm == 0.0f) ? 1.0f : nrm;
                dx /= nrm;
                dy /= nrm;
                startIndex = winIndex;
            }

            Vertices returnVal = new Vertices(numEdges);

            for (int i = 0; i < numEdges; i++)
            {
                returnVal.Add(vertices[edgeList[i]]);
                //Debug.WriteLine(string.Format("{0}, {1}", vertices[edgeList[i]].X, vertices[edgeList[i]].Y));
            }

            //Not sure if we need this
            //returnVal.MergeParallelEdges(Settings.b2_angularSlop);

            return returnVal;
        }
    }
}