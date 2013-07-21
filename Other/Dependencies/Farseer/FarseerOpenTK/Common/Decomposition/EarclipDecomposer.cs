/*
 * C# Version Ported by Matt Bettcher and Ian Qvist 2009-2010
 * 
 * Original C++ Version Copyright (c) 2007 Eric Jordan
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
using System.Collections.Generic;
using FarseerPhysics.Common.PolygonManipulation;
using OpenTK;

namespace FarseerPhysics.Common.Decomposition
{
    /// <summary>
    /// Ported from jBox2D. Original author: ewjordan 
    /// Triangulates a polygon using simple ear-clipping algorithm.
    /// 
    /// Only works on simple polygons.
    /// 
    /// Triangles may be degenerate, especially if you have identical points
    /// in the input to the algorithm.  Check this before you use them.
    /// </summary>
    public static class EarclipDecomposer
    {
        //box2D rev 32 - for details, see http://www.box2d.org/forum/viewtopic.php?f=4&t=83&start=50

        private const float Tol = .001f;

        /// <summary>
        /// Decomposes a non-convex polygon into a number of convex polygons, up
        /// to maxPolys (remaining pieces are thrown out).
        ///
        /// Each resulting polygon will have no more than Settings.MaxPolygonVertices
        /// vertices.
        /// 
        /// Warning: Only works on simple polygons
        /// </summary>
        /// <param name="vertices">The vertices.</param>
        /// <returns></returns>
        public static List<Vertices> ConvexPartition(Vertices vertices)
        {
            return ConvexPartition(vertices, int.MaxValue, 0);
        }

        /// <summary>
        /// Decomposes a non-convex polygon into a number of convex polygons, up
        /// to maxPolys (remaining pieces are thrown out).
        /// Each resulting polygon will have no more than Settings.MaxPolygonVertices
        /// vertices.
        /// Warning: Only works on simple polygons
        /// </summary>
        /// <param name="vertices">The vertices.</param>
        /// <param name="maxPolys">The maximum number of polygons.</param>
        /// <param name="tolerance">The tolerance.</param>
        /// <returns></returns>
        public static List<Vertices> ConvexPartition(Vertices vertices, int maxPolys, float tolerance)
        {
            if (vertices.Count < 3)
                return new List<Vertices> { vertices };
            /*
            if (vertices.IsConvex() && vertices.Count <= Settings.MaxPolygonVertices)
            {
                if (vertices.IsCounterClockWise())
                {
                    Vertices tempP = new Vertices(vertices);
                    tempP.Reverse();
                    tempP = SimplifyTools.CollinearSimplify(tempP);
                    tempP.ForceCounterClockWise();
                    return new List<Vertices> { tempP };
                }
                vertices = SimplifyTools.CollinearSimplify(vertices);
                vertices.ForceCounterClockWise();
                return new List<Vertices> { vertices };
            }
            */
            List<Triangle> triangulated;

            if (vertices.IsCounterClockWise())
            {
                Vertices tempP = new Vertices(vertices);
                tempP.Reverse();
                triangulated = TriangulatePolygon(tempP);
            }
            else
            {
                triangulated = TriangulatePolygon(vertices);
            }
            if (triangulated.Count < 1)
            {
                //Still no luck?  Oh well...
                throw new Exception("Can't triangulate your polygon.");
            }

            List<Vertices> polygonizedTriangles = PolygonizeTriangles(triangulated, maxPolys, tolerance);

            //The polygonized triangles are not guaranteed to be without collinear points. We remove
            //them to be sure.
            for (int i = 0; i < polygonizedTriangles.Count; i++)
            {
                polygonizedTriangles[i] = SimplifyTools.CollinearSimplify(polygonizedTriangles[i], 0);
            }

            //Remove empty vertice collections
            for (int i = polygonizedTriangles.Count - 1; i >= 0; i--)
            {
                if (polygonizedTriangles[i].Count == 0)
                    polygonizedTriangles.RemoveAt(i);
            }

            return polygonizedTriangles;
        }

        /// <summary>
        /// Turns a list of triangles into a list of convex polygons. Very simple
        /// method - start with a seed triangle, keep adding triangles to it until
        /// you can't add any more without making the polygon non-convex.
        ///
        /// Returns an integer telling how many polygons were created.  Will fill
        /// polys array up to polysLength entries, which may be smaller or larger
        /// than the return value.
        /// 
        /// Takes O(N///P) where P is the number of resultant polygons, N is triangle
        /// count.
        /// 
        /// The final polygon list will not necessarily be minimal, though in
        /// practice it works fairly well.
        /// </summary>
        /// <param name="triangulated">The triangulated.</param>
        ///<param name="maxPolys">The maximun number of polygons</param>
        ///<param name="tolerance">The tolerance</param>
        ///<returns></returns>
        public static List<Vertices> PolygonizeTriangles(List<Triangle> triangulated, int maxPolys, float tolerance)
        {
            List<Vertices> polys = new List<Vertices>(50);

            int polyIndex = 0;

            if (triangulated.Count <= 0)
            {
                //return empty polygon list
                return polys;
            }

            bool[] covered = new bool[triangulated.Count];
            for (int i = 0; i < triangulated.Count; ++i)
            {
                covered[i] = false;

                //Check here for degenerate triangles
                if (((triangulated[i].X[0] == triangulated[i].X[1]) && (triangulated[i].Y[0] == triangulated[i].Y[1]))
                    ||
                    ((triangulated[i].X[1] == triangulated[i].X[2]) && (triangulated[i].Y[1] == triangulated[i].Y[2]))
                    ||
                    ((triangulated[i].X[0] == triangulated[i].X[2]) && (triangulated[i].Y[0] == triangulated[i].Y[2])))
                {
                    covered[i] = true;
                }
            }

            bool notDone = true;
            while (notDone)
            {
                int currTri = -1;
                for (int i = 0; i < triangulated.Count; ++i)
                {
                    if (covered[i])
                        continue;
                    currTri = i;
                    break;
                }
                if (currTri == -1)
                {
                    notDone = false;
                }
                else
                {
                    Vertices poly = new Vertices(3);

                    for (int i = 0; i < 3; i++)
                    {
                        poly.Add(new Vector2(triangulated[currTri].X[i], triangulated[currTri].Y[i]));
                    }

                    covered[currTri] = true;
                    int index = 0;
                    for (int i = 0; i < 2 * triangulated.Count; ++i, ++index)
                    {
                        while (index >= triangulated.Count) index -= triangulated.Count;
                        if (covered[index])
                        {
                            continue;
                        }
                        Vertices newP = AddTriangle(triangulated[index], poly);
                        if (newP == null)
                            continue; // is this right

                        if (newP.Count > Settings.MaxPolygonVertices)
                            continue;

                        if (newP.IsConvex())
                        {
                            //Or should it be IsUsable?  Maybe re-write IsConvex to apply the angle threshold from Box2d
                            poly = new Vertices(newP);
                            covered[index] = true;
                        }
                    }

                    //We have a maximum of polygons that we need to keep under.
                    if (polyIndex < maxPolys)
                    {
                        //SimplifyTools.MergeParallelEdges(poly, tolerance);

                        //If identical points are present, a triangle gets
                        //borked by the MergeParallelEdges function, hence
                        //the vertex number check
                        if (poly.Count >= 3)
                            polys.Add(new Vertices(poly));
                        //else
                        //    printf("Skipping corrupt poly\n");
                    }
                    if (poly.Count >= 3)
                        polyIndex++; //Must be outside (polyIndex < polysLength) test
                }
            }

            return polys;
        }

        /// <summary>
        /// Triangulates a polygon using simple ear-clipping algorithm. Returns
        /// size of Triangle array unless the polygon can't be triangulated.
        /// This should only happen if the polygon self-intersects,
        /// though it will not _always_ return null for a bad polygon - it is the
        /// caller's responsibility to check for self-intersection, and if it
        /// doesn't, it should at least check that the return value is non-null
        /// before using. You're warned!
        ///
        /// Triangles may be degenerate, especially if you have identical points
        /// in the input to the algorithm.  Check this before you use them.
        ///
        /// This is totally unoptimized, so for large polygons it should not be part
        /// of the simulation loop.
        ///
        /// Warning: Only works on simple polygons.
        /// </summary>
        /// <returns></returns>
        public static List<Triangle> TriangulatePolygon(Vertices vertices)
        {
            List<Triangle> results = new List<Triangle>();
            if (vertices.Count < 3)
                return new List<Triangle>();

            //Recurse and split on pinch points
            Vertices pA, pB;
            Vertices pin = new Vertices(vertices);
            if (ResolvePinchPoint(pin, out pA, out pB))
            {
                List<Triangle> mergeA = TriangulatePolygon(pA);
                List<Triangle> mergeB = TriangulatePolygon(pB);

                if (mergeA.Count == -1 || mergeB.Count == -1)
                    throw new Exception("Can't triangulate your polygon.");

                for (int i = 0; i < mergeA.Count; ++i)
                {
                    results.Add(new Triangle(mergeA[i]));
                }
                for (int i = 0; i < mergeB.Count; ++i)
                {
                    results.Add(new Triangle(mergeB[i]));
                }

                return results;
            }

            Triangle[] buffer = new Triangle[vertices.Count - 2];
            int bufferSize = 0;
            float[] xrem = new float[vertices.Count];
            float[] yrem = new float[vertices.Count];
            for (int i = 0; i < vertices.Count; ++i)
            {
                xrem[i] = vertices[i].X;
                yrem[i] = vertices[i].Y;
            }

            int vNum = vertices.Count;

            while (vNum > 3)
            {
                // Find an ear
                int earIndex = -1;
                float earMaxMinCross = -10.0f;
                for (int i = 0; i < vNum; ++i)
                {
                    if (IsEar(i, xrem, yrem, vNum))
                    {
                        int lower = Remainder(i - 1, vNum);
                        int upper = Remainder(i + 1, vNum);
                        Vector2 d1 = new Vector2(xrem[upper] - xrem[i], yrem[upper] - yrem[i]);
                        Vector2 d2 = new Vector2(xrem[i] - xrem[lower], yrem[i] - yrem[lower]);
                        Vector2 d3 = new Vector2(xrem[lower] - xrem[upper], yrem[lower] - yrem[upper]);

                        d1.Normalize();
                        d2.Normalize();
                        d3.Normalize();
                        float cross12;
                        MathUtils.Cross(ref d1, ref d2, out cross12);
                        cross12 = Math.Abs(cross12);

                        float cross23;
                        MathUtils.Cross(ref d2, ref d3, out cross23);
                        cross23 = Math.Abs(cross23);

                        float cross31;
                        MathUtils.Cross(ref d3, ref d1, out cross31);
                        cross31 = Math.Abs(cross31);

                        //Find the maximum minimum angle
                        float minCross = Math.Min(cross12, Math.Min(cross23, cross31));
                        if (minCross > earMaxMinCross)
                        {
                            earIndex = i;
                            earMaxMinCross = minCross;
                        }
                    }
                }

                // If we still haven't found an ear, we're screwed.
                // Note: sometimes this is happening because the
                // remaining points are collinear.  Really these
                // should just be thrown out without halting triangulation.
                if (earIndex == -1)
                {
                    for (int i = 0; i < bufferSize; i++)
                    {
                        results.Add(new Triangle(buffer[i]));
                    }

                    return results;
                }

                // Clip off the ear:
                // - remove the ear tip from the list

                --vNum;
                float[] newx = new float[vNum];
                float[] newy = new float[vNum];
                int currDest = 0;
                for (int i = 0; i < vNum; ++i)
                {
                    if (currDest == earIndex) ++currDest;
                    newx[i] = xrem[currDest];
                    newy[i] = yrem[currDest];
                    ++currDest;
                }

                // - add the clipped triangle to the triangle list
                int under = (earIndex == 0) ? (vNum) : (earIndex - 1);
                int over = (earIndex == vNum) ? 0 : (earIndex + 1);
                Triangle toAdd = new Triangle(xrem[earIndex], yrem[earIndex], xrem[over], yrem[over], xrem[under],
                                              yrem[under]);
                buffer[bufferSize] = toAdd;
                ++bufferSize;

                // - replace the old list with the new one
                xrem = newx;
                yrem = newy;
            }

            Triangle tooAdd = new Triangle(xrem[1], yrem[1], xrem[2], yrem[2], xrem[0], yrem[0]);
            buffer[bufferSize] = tooAdd;
            ++bufferSize;

            for (int i = 0; i < bufferSize; i++)
            {
                results.Add(new Triangle(buffer[i]));
            }

            return results;
        }

        /// <summary>
        /// Finds and fixes "pinch points," points where two polygon
        /// vertices are at the same point.
        /// 
        /// If a pinch point is found, pin is broken up into poutA and poutB
        /// and true is returned; otherwise, returns false.
        /// 
        /// Mostly for internal use.
        /// 
        /// O(N^2) time, which sucks...
        /// </summary>
        /// <param name="pin">The pin.</param>
        /// <param name="poutA">The pout A.</param>
        /// <param name="poutB">The pout B.</param>
        /// <returns></returns>
        private static bool ResolvePinchPoint(Vertices pin, out Vertices poutA, out Vertices poutB)
        {
            poutA = new Vertices();
            poutB = new Vertices();

            if (pin.Count < 3)
                return false;

            bool hasPinchPoint = false;
            int pinchIndexA = -1;
            int pinchIndexB = -1;
            for (int i = 0; i < pin.Count; ++i)
            {
                for (int j = i + 1; j < pin.Count; ++j)
                {
                    //Don't worry about pinch points where the points
                    //are actually just dupe neighbors
                    if (Math.Abs(pin[i].X - pin[j].X) < Tol && Math.Abs(pin[i].Y - pin[j].Y) < Tol && j != i + 1)
                    {
                        pinchIndexA = i;
                        pinchIndexB = j;
                        hasPinchPoint = true;
                        break;
                    }
                }
                if (hasPinchPoint) break;
            }
            if (hasPinchPoint)
            {
                int sizeA = pinchIndexB - pinchIndexA;
                if (sizeA == pin.Count) return false; //has dupe points at wraparound, not a problem here
                for (int i = 0; i < sizeA; ++i)
                {
                    int ind = Remainder(pinchIndexA + i, pin.Count); // is this right
                    poutA.Add(pin[ind]);
                }

                int sizeB = pin.Count - sizeA;
                for (int i = 0; i < sizeB; ++i)
                {
                    int ind = Remainder(pinchIndexB + i, pin.Count); // is this right    
                    poutB.Add(pin[ind]);
                }
            }
            return hasPinchPoint;
        }

        /// <summary>
        /// Fix for obnoxious behavior for the % operator for negative numbers...
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="modulus">The modulus.</param>
        /// <returns></returns>
        private static int Remainder(int x, int modulus)
        {
            int rem = x % modulus;
            while (rem < 0)
            {
                rem += modulus;
            }
            return rem;
        }

        private static Vertices AddTriangle(Triangle t, Vertices vertices)
        {
            // First, find vertices that connect
            int firstP = -1;
            int firstT = -1;
            int secondP = -1;
            int secondT = -1;
            for (int i = 0; i < vertices.Count; i++)
            {
                if (t.X[0] == vertices[i].X && t.Y[0] == vertices[i].Y)
                {
                    if (firstP == -1)
                    {
                        firstP = i;
                        firstT = 0;
                    }
                    else
                    {
                        secondP = i;
                        secondT = 0;
                    }
                }
                else if (t.X[1] == vertices[i].X && t.Y[1] == vertices[i].Y)
                {
                    if (firstP == -1)
                    {
                        firstP = i;
                        firstT = 1;
                    }
                    else
                    {
                        secondP = i;
                        secondT = 1;
                    }
                }
                else if (t.X[2] == vertices[i].X && t.Y[2] == vertices[i].Y)
                {
                    if (firstP == -1)
                    {
                        firstP = i;
                        firstT = 2;
                    }
                    else
                    {
                        secondP = i;
                        secondT = 2;
                    }
                }
            }
            // Fix ordering if first should be last vertex of poly
            if (firstP == 0 && secondP == vertices.Count - 1)
            {
                firstP = vertices.Count - 1;
                secondP = 0;
            }

            // Didn't find it
            if (secondP == -1)
            {
                return null;
            }

            // Find tip index on triangle
            int tipT = 0;
            if (tipT == firstT || tipT == secondT)
                tipT = 1;
            if (tipT == firstT || tipT == secondT)
                tipT = 2;

            Vertices result = new Vertices(vertices.Count + 1);
            for (int i = 0; i < vertices.Count; i++)
            {
                result.Add(vertices[i]);

                if (i == firstP)
                    result.Add(new Vector2(t.X[tipT], t.Y[tipT]));
            }

            return result;
        }

        /// <summary>
        /// Checks if vertex i is the tip of an ear in polygon defined by xv[] and
        /// yv[].
        ///
        /// Assumes clockwise orientation of polygon...ick
        /// </summary>
        /// <param name="i">The i.</param>
        /// <param name="xv">The xv.</param>
        /// <param name="yv">The yv.</param>
        /// <param name="xvLength">Length of the xv.</param>
        /// <returns>
        /// 	<c>true</c> if the specified i is ear; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsEar(int i, float[] xv, float[] yv, int xvLength)
        {
            float dx0, dy0, dx1, dy1;
            if (i >= xvLength || i < 0 || xvLength < 3)
            {
                return false;
            }
            int upper = i + 1;
            int lower = i - 1;
            if (i == 0)
            {
                dx0 = xv[0] - xv[xvLength - 1];
                dy0 = yv[0] - yv[xvLength - 1];
                dx1 = xv[1] - xv[0];
                dy1 = yv[1] - yv[0];
                lower = xvLength - 1;
            }
            else if (i == xvLength - 1)
            {
                dx0 = xv[i] - xv[i - 1];
                dy0 = yv[i] - yv[i - 1];
                dx1 = xv[0] - xv[i];
                dy1 = yv[0] - yv[i];
                upper = 0;
            }
            else
            {
                dx0 = xv[i] - xv[i - 1];
                dy0 = yv[i] - yv[i - 1];
                dx1 = xv[i + 1] - xv[i];
                dy1 = yv[i + 1] - yv[i];
            }
            float cross = dx0 * dy1 - dx1 * dy0;
            if (cross > 0)
                return false;
            Triangle myTri = new Triangle(xv[i], yv[i], xv[upper], yv[upper],
                                          xv[lower], yv[lower]);
            for (int j = 0; j < xvLength; ++j)
            {
                if (j == i || j == lower || j == upper)
                    continue;
                if (myTri.IsInside(xv[j], yv[j]))
                    return false;
            }
            return true;
        }
    }

    public class Triangle
    {
        public float[] X;
        public float[] Y;

        //Constructor automatically fixes orientation to ccw
        public Triangle(float x1, float y1, float x2, float y2, float x3, float y3)
        {
            X = new float[3];
            Y = new float[3];
            float dx1 = x2 - x1;
            float dx2 = x3 - x1;
            float dy1 = y2 - y1;
            float dy2 = y3 - y1;
            float cross = dx1 * dy2 - dx2 * dy1;
            bool ccw = (cross > 0);
            if (ccw)
            {
                X[0] = x1;
                X[1] = x2;
                X[2] = x3;
                Y[0] = y1;
                Y[1] = y2;
                Y[2] = y3;
            }
            else
            {
                X[0] = x1;
                X[1] = x3;
                X[2] = x2;
                Y[0] = y1;
                Y[1] = y3;
                Y[2] = y2;
            }
        }

        public Triangle(Triangle t)
        {
            X = new float[3];
            Y = new float[3];

            X[0] = t.X[0];
            X[1] = t.X[1];
            X[2] = t.X[2];
            Y[0] = t.Y[0];
            Y[1] = t.Y[1];
            Y[2] = t.Y[2];
        }

        public bool IsInside(float x, float y)
        {
            if (x < X[0] && x < X[1] && x < X[2]) return false;
            if (x > X[0] && x > X[1] && x > X[2]) return false;
            if (y < Y[0] && y < Y[1] && y < Y[2]) return false;
            if (y > Y[0] && y > Y[1] && y > Y[2]) return false;

            float vx2 = x - X[0];
            float vy2 = y - Y[0];
            float vx1 = X[1] - X[0];
            float vy1 = Y[1] - Y[0];
            float vx0 = X[2] - X[0];
            float vy0 = Y[2] - Y[0];

            float dot00 = vx0 * vx0 + vy0 * vy0;
            float dot01 = vx0 * vx1 + vy0 * vy1;
            float dot02 = vx0 * vx2 + vy0 * vy2;
            float dot11 = vx1 * vx1 + vy1 * vy1;
            float dot12 = vx1 * vx2 + vy1 * vy2;
            float invDenom = 1.0f / (dot00 * dot11 - dot01 * dot01);
            float u = (dot11 * dot02 - dot01 * dot12) * invDenom;
            float v = (dot00 * dot12 - dot01 * dot02) * invDenom;

            return ((u > 0) && (v > 0) && (u + v < 1));
        }
    }
}