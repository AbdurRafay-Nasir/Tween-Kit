using UnityEngine;
using System.Collections.Generic;

namespace DOTweenModular.Miscellaneous
{
    /// <summary>
    /// Utility methods for generating Curves
    /// </summary>
    public static class Curve
    {

        public static class CatmullRom
        {

            /// <summary>
            /// Generates Catmull-Rom spline.
            /// </summary>
            /// <param name="startPosition">The point from which the curve will start, usually transform.position.</param>
            /// <param name="points">The points used to create Catmull-Rom Spline.</param>
            /// <param name="resolution">The smoothness of the curve; higher values generate smoother paths but are computationally expensive</param>
            /// <param name="closed">If true connects startPosition with last point</param>
            /// <returns>A List of points representing a Open or Closed Catmull-Rom spline.</returns>
            public static Vector3[] GetSpline(Vector3 startPosition, Vector3[] points, int resolution = 5, bool closed = false)
            {
                if (closed)
                    return GetClosedSpline(startPosition, points, resolution);

                else
                    return GetOpenSpline(startPosition, points, resolution);
            }

            /// <summary>
            /// Generates an Open Catmull-Rom spline.
            /// </summary>
            /// <param name="startPosition">The point from which the curve will start, usually transform.position.</param>
            /// <param name="points">The points used to create the Open Catmull-Rom Spline.</param>
            /// <param name="resolution">The smoothness of the curve; higher values generate smoother paths but are computationally more expensive.</param>
            /// <returns>A List of points representing an Open Catmull-Rom spline.</returns>
            public static Vector3[] GetOpenSpline(Vector3 startPosition, Vector3[] points, int resolution)
            {
                List<Vector3> modifiedPoints = new();
                List<Vector3> catmullRomPoints = new();

                modifiedPoints.Add(startPosition);
                modifiedPoints.Add(startPosition);

                for (int i = 0; i < points.Length; i++)
                {
                    modifiedPoints.Add(points[i]);
                }

                modifiedPoints.Add(points[^1]);

                for (int i = 0; i < modifiedPoints.Count - 3; i++)
                {
                    Vector3 p0 = modifiedPoints[i];
                    Vector3 p1 = modifiedPoints[i + 1];
                    Vector3 p2 = modifiedPoints[i + 2];
                    Vector3 p3 = modifiedPoints[i + 3];

                    for (int j = 0; j <= resolution; j++)
                    {
                        float t = j / (float)resolution;

                        Vector3 newPos = GetPoint(p0, p1, p2, p3, t);
                        catmullRomPoints.Add(newPos);
                    }
                }

                return catmullRomPoints.ToArray();
            }

            /// <summary>
            /// Generates a Closed Catmull-Rom spline.
            /// </summary>
            /// <param name="startPosition">The point from which the curve will start, typically transform.position.</param>
            /// <param name="points">The points used to create the Closed Catmull-Rom Spline.</param>
            /// <param name="resolution">The smoothness of the curve; higher values generate smoother paths but are computationally more expensive.</param>
            /// <returns>An array of points representing a Closed Catmull-Rom spline.</returns>
            public static Vector3[] GetClosedSpline(Vector3 startPosition, Vector3[] points, int resolution)
            {
                List<Vector3> catmullRomPoints = new();
                List<Vector3> modifiedPoints = new(points);

                modifiedPoints.Insert(0, startPosition);
                modifiedPoints[0] = startPosition;

                for (int i = 0; i < modifiedPoints.Count; i++)
                {
                    int prevIndex = (i - 1 + modifiedPoints.Count) % modifiedPoints.Count;
                    int nextIndex = (i + 1) % modifiedPoints.Count;

                    Vector3 previousPoint = modifiedPoints[prevIndex];
                    Vector3 currentPoint = modifiedPoints[i];
                    Vector3 nextPoint = modifiedPoints[nextIndex];
                    Vector3 nextNextPoint = modifiedPoints[(nextIndex + 1) % modifiedPoints.Count];

                    for (int j = 0; j <= resolution; j++)
                    {
                        float t = j / (float)resolution;
                        Vector3 point = GetPoint(previousPoint, currentPoint,
                                                 nextPoint, nextNextPoint, t);

                        catmullRomPoints.Add(point);
                    }
                }

                return catmullRomPoints.ToArray();
            }

            /// <summary>
            /// Get Catmull-Rom Point for given segment
            /// </summary>
            public static Vector3 GetPoint(Vector3 previousPoint, Vector3 currentPoint, 
                                           Vector3 nextPoint, Vector3 nextNextPoint, float t)
            {
                float t2 = t * t;
                float t3 = t2 * t;

                float b1 = 0.5f * (-t3 + 2.0f * t2 - t);
                float b2 = 0.5f * (3.0f * t3 - 5.0f * t2 + 2.0f);
                float b3 = 0.5f * (-3.0f * t3 + 4.0f * t2 + t);
                float b4 = 0.5f * (t3 - t2);

                return b1 * previousPoint + b2 * currentPoint + b3 * nextPoint + b4 * nextNextPoint;
            }

        }

        public static class CubicBezier
        {

            /// <summary>
            /// Converts the given points into a series of points forming a cubic Bezier curve.
            /// </summary>
            /// <param name="startPosition">The starting position of the path, often `transform.position`.</param>
            /// <param name="points">The points used to create the cubic Bezier curve.</param>
            /// <param name="resolution">The smoothness of the curve; higher values generate smoother paths but are more computationally expensive.</param>
            /// <returns>An array of points representing a cubic Bezier curve.</returns>
            /// <remarks>Returns NULL if points are NULL or the number of points is not a multiple of 3.</remarks>
            public static Vector3[] GetSpline(Vector3 startPosition, Vector3[] points, int resolution)
            {
                if (points == null || points.Length % 3 != 0)
                    return null;

                List<Vector3> cubicBezierPoints = new();

                for (int i = 0; i < points.Length; i += 3)
                {
                    Vector3 controlPoint1 = (i == 0) ? startPosition : points[i - 1];
                    Vector3 controlPoint2 = points[i + 2];
                    Vector3 tangent1 = points[i];
                    Vector3 tangent2 = points[i + 1];

                    for (int j = 0; j <= resolution; j++)
                    {
                        float t = j / (float)resolution;

                        Vector3 point = GetPoint(controlPoint1, tangent1,
                                                 controlPoint2, tangent2, t);

                        cubicBezierPoints.Add(point);
                    }
                }

                return cubicBezierPoints.ToArray();
            }

            /// <summary>
            /// Get Cubic Bezier Point for given segment
            /// </summary>
            public static Vector3 GetPoint(Vector3 controlPoint1, Vector3 tangent1,
                                           Vector3 controlPoint2, Vector3 tangent2, float t)
            {
                float u = 1 - t;
                float tt = t * t;
                float uu = u * u;
                float uuu = uu * u;
                float ttt = tt * t;

                Vector3 p = uuu * controlPoint1;
                p += 3 * uu * t * tangent1;
                p += 3 * u * tt * tangent2;
                p += ttt * controlPoint2;

                return p;
            }

        }

    }

}