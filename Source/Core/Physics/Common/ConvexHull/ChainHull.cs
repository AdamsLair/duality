using System.Collections.Generic;
using Duality;

namespace FarseerPhysics.Common.ConvexHull
{
    public static class ChainHull
    {
        //Andrew's monotone chain 2D convex hull algorithm.
        //Copyright 2001, softSurfer (www.softsurfer.com)

        /// <summary>
        /// Gets the convex hull.
        /// </summary>
        /// <remarks>
        /// http://www.softsurfer.com/Archive/algorithm_0109/algorithm_0109.htm
        /// </remarks>
        /// <returns></returns>
        public static Vertices GetConvexHull(Vertices P)
        {
            P.Sort(new PointComparer());

            Vector2[] H = new Vector2[P.Count];
            Vertices res = new Vertices();

            int n = P.Count;

            int bot, top = -1; // indices for bottom and top of the stack
            int i; // array scan index

            // Get the indices of points with min x-coord and min|max y-coord
            int minmin = 0, minmax;
            float xmin = P[0].X;
            for (i = 1; i < n; i++)
                if (P[i].X != xmin) break;
            minmax = i - 1;
            if (minmax == n - 1)
            {
                // degenerate case: all x-coords == xmin
                H[++top] = P[minmin];
                if (P[minmax].Y != P[minmin].Y) // a nontrivial segment
                    H[++top] = P[minmax];
                H[++top] = P[minmin]; // add polygon endpoint

                for (int j = 0; j < top + 1; j++)
                {
                    res.Add(H[j]);
                }

                return res;
            }

            top = res.Count - 1;

            // Get the indices of points with max x-coord and min|max y-coord
            int maxmin, maxmax = n - 1;
            float xmax = P[n - 1].X;
            for (i = n - 2; i >= 0; i--)
                if (P[i].X != xmax) break;
            maxmin = i + 1;

            // Compute the lower hull on the stack H
            H[++top] = P[minmin]; // push minmin point onto stack
            i = minmax;
            while (++i <= maxmin)
            {
                // the lower line joins P[minmin] with P[maxmin]
                if (MathUtils.Area(P[minmin], P[maxmin], P[i]) >= 0 && i < maxmin)
                    continue; // ignore P[i] above or on the lower line

                while (top > 0) // there are at least 2 points on the stack
                {
                    // test if P[i] is left of the line at the stack top
                    if (MathUtils.Area(H[top - 1], H[top], P[i]) > 0)
                        break; // P[i] is a new hull vertex
                    else
                        top--; // pop top point off stack
                }
                H[++top] = P[i]; // push P[i] onto stack
            }

            // Next, compute the upper hull on the stack H above the bottom hull
            if (maxmax != maxmin) // if distinct xmax points
                H[++top] = P[maxmax]; // push maxmax point onto stack
            bot = top; // the bottom point of the upper hull stack
            i = maxmin;
            while (--i >= minmax)
            {
                // the upper line joins P[maxmax] with P[minmax]
                if (MathUtils.Area(P[maxmax], P[minmax], P[i]) >= 0 && i > minmax)
                    continue; // ignore P[i] below or on the upper line

                while (top > bot) // at least 2 points on the upper stack
                {
                    // test if P[i] is left of the line at the stack top
                    if (MathUtils.Area(H[top - 1], H[top], P[i]) > 0)
                        break; // P[i] is a new hull vertex
                    else
                        top--; // pop top point off stack
                }
                H[++top] = P[i]; // push P[i] onto stack
            }
            if (minmax != minmin)
                H[++top] = P[minmin]; // push joining endpoint onto stack

            for (int j = 0; j < top + 1; j++)
            {
                res.Add(H[j]);
            }

            return res;
        }

        #region Nested type: PointComparer

        public class PointComparer : Comparer<Vector2>
        {
            public override int Compare(Vector2 a, Vector2 b)
            {
                int f = a.X.CompareTo(b.X);
                return f != 0 ? f : a.Y.CompareTo(b.Y);
            }
        }

        #endregion
    }
}