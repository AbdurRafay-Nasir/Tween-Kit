using System;
using System.Collections.Generic;
using UnityEngine;

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
            /// Generates an open Catmull-Rom spline curve.
            /// </summary>
            /// <param name="startPosition">The point from which the curve will start, usually transform.position.</param>
            /// <param name="points">The points used to create the open Catmull-Rom curve.</param>
            /// <param name="resolution">The smoothness of the curve; higher values generate smoother paths but are more computationally expensive.</param>
            /// <returns>An array of points representing an open Catmull-Rom spline curve.</returns>
            /// <remarks>Returns NULL if points are NULL or the number of points is less than 2.</remarks>
            public static Vector3[] GetOpenSpline(Vector3 startPosition, Vector3[] points, int resolution)
            {
                if (points == null || points.Length < 2)
                    return null;

                Vector3[] pointsWithStartPosition = new Vector3[points.Length + 1];
                pointsWithStartPosition[0] = startPosition;
                Array.Copy(points, 0, pointsWithStartPosition, 1, points.Length);

                List<Vector3> catmullRomPoints = new List<Vector3>();

                // Loop through the points, computing the Catmull-Rom spline segments
                for (int i = 0; i < pointsWithStartPosition.Length - 1; i++)
                {
                    // Calculate the control points for this segment
                    Vector3 previousPoint = (i == 0) ? pointsWithStartPosition[i] : pointsWithStartPosition[i - 1];
                    Vector3 currentPoint = pointsWithStartPosition[i];
                    Vector3 nextPoint = pointsWithStartPosition[i + 1];
                    Vector3 nextNextPoint = (i + 2 == pointsWithStartPosition.Length) ?
                                            pointsWithStartPosition[pointsWithStartPosition.Length - 1] :
                                            pointsWithStartPosition[i + 2];

                    for (int j = 0; j <= resolution; j++)
                    {
                        float t = (float)j / resolution;
                        Vector3 point = GetPoint(previousPoint, currentPoint, nextPoint, nextNextPoint, t);

                        catmullRomPoints.Add(point);
                    }
                }

                return catmullRomPoints.ToArray();
            }


            /// <summary>
            /// Generates a closed Catmull-Rom spline curve.
            /// </summary>
            /// <param name="startPosition">The point from which the curve will start, typically transform.position.</param>
            /// <param name="points">The points used to create the open Catmull-Rom curve.</param>
            /// <param name="resolution">The smoothness of the curve; higher values generate smoother paths but are more computationally expensive.</param>
            /// <returns>An array of points representing a closed Catmull-Rom spline curve.</returns>
            /// <remarks>Returns NULL if points are NULL or the number of points is less than 2.</remarks>
            public static Vector3[] GetClosedCurve(Vector3 startPosition, Vector3[] points, int resolution)
            {
                if (points == null || points.Length < 2)
                    return null;

                Vector3[] pointsWithStartPosition = new Vector3[points.Length + 1];
                pointsWithStartPosition[0] = startPosition;
                Array.Copy(points, 0, pointsWithStartPosition, 1, points.Length);

                List<Vector3> catmullRomPoints = new();

                // Loop through the points, computing the Catmull-Rom spline segments
                for (int i = 0; i < pointsWithStartPosition.Length; i++)
                {
                    int prevIndex = (i - 1 + pointsWithStartPosition.Length) % pointsWithStartPosition.Length;
                    int nextIndex = (i + 1) % pointsWithStartPosition.Length;

                    Vector3 previousPoint = pointsWithStartPosition[prevIndex];
                    Vector3 currentPoint = pointsWithStartPosition[i];
                    Vector3 nextPoint = pointsWithStartPosition[nextIndex];
                    Vector3 nextNextPoint = pointsWithStartPosition[(nextIndex + 1) % pointsWithStartPosition.Length];

                    for (int j = 0; j <= resolution; j++)
                    {
                        float t = (float)j / resolution;
                        Vector3 point = GetPoint(previousPoint, currentPoint,
                                                                nextPoint, nextNextPoint, t);

                        catmullRomPoints.Add(point);
                    }
                }

                return catmullRomPoints.ToArray();
            }

            /// <summary>
            /// Get Catmull Rom Point for given segment
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