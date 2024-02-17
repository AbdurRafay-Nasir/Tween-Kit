using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace DOTweenModular.Miscellaneous
{
    /// <summary>
    /// Utility Class for generating Curves
    /// </summary>
    public static class Curve
    {
        /// <summary>
        /// Utility Class for generating Catmull-Rom Splines
        /// </summary>
        public static class CatmullRom
        {

            /// <summary>
            /// Generates Catmull-Rom spline.
            /// </summary>
            /// <param name="startPosition">The point from which the curve will start, usually transform.position.</param>
            /// <param name="points">The points used to create Catmull-Rom Spline.</param>
            /// <param name="resolution">The smoothness of the curve; higher values generate smoother paths but are computationally expensive</param>
            /// <param name="closed">If true connects startPosition with last point</param>
            /// <returns>Array of points representing a Open or Closed Catmull-Rom spline.</returns>
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
            /// <returns>Array of points representing an Open Catmull-Rom spline.</returns>
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

                catmullRomPoints = catmullRomPoints.Distinct().ToList();

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

                // Save last point as it is needed to form closed loop
                Vector3 lastPoint = catmullRomPoints[^1];

                // Remove Duplicates
                catmullRomPoints.RemoveAt(catmullRomPoints.Count - 1);
                catmullRomPoints = catmullRomPoints.Distinct().ToList();

                // Add last point back
                catmullRomPoints.Add(lastPoint);

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

        /// <summary>
        /// Utility Class for generating Cubic Bezier Splines
        /// </summary>
        public static class CubicBezier
        {

            /// <summary>
            /// Converts the given points into a series of points forming a cubic Bezier curve.
            /// </summary>
            /// <param name="startPosition">The starting position of the path, often `transform.position`.</param>
            /// <param name="points">The points used to create the cubic Bezier curve.</param>
            /// <param name="resolution">The smoothness of the curve; higher values generate smoother paths but are more computationally expensive.</param>
            /// <returns>Array of points representing a cubic Bezier curve.</returns>
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

        /// <summary>
        /// Utility Class for Arcs
        /// </summary>
        public static class Arc
        {
            /// <summary>
            /// Generates Rounded Rectangle
            /// </summary>
            /// <param name="center">Center of rectangle</param>
            /// <param name="width">Width of rectnagle</param>
            /// <param name="height">Height of rectnagle</param>
            /// <param name="cornerRadius">Corner radius of Rectangle</param>
            /// <param name="resolution">Smoothness of Corners</param>
            /// <returns>Array of Vector2 representing Rounded Rectangle</returns>
            public static Vector2[] GetRect(Vector2 center, Vector2 size, float cornerRadius, float resolution = 3f)
            {
                List<Vector2> points = new();

                float width = size.x;
                float height = size.y;

                float minRadius = Mathf.Min(width, height);
                float clampedRadius = Mathf.Clamp(cornerRadius, 0f, minRadius);

                Vector2 topLeft1 = new(center.x - width, center.y + height - clampedRadius);
                Vector2 topLeft2 = new(center.x - width + clampedRadius, center.y + height);

                Vector2 topRight1 = new(center.x + width - clampedRadius, center.y + height);
                Vector2 topRight2 = new(center.x + width, center.y + height - clampedRadius);

                Vector2 bottomRight1 = new(center.x + width, center.y - height + clampedRadius);
                Vector2 bottomRight2 = new(center.x + width - clampedRadius, center.y - height);

                Vector2 bottomLeft1 = new(center.x - width + clampedRadius, center.y - height);
                Vector2 bottomLeft2 = new(center.x - width, center.y - height + clampedRadius);

                Vector2 topLeftArcCenter = new(topLeft2.x, topLeft1.y);
                Vector2 topRightArcCenter = new(topRight1.x, topRight2.y);
                Vector2 bottomLeftArcCenter = new(bottomLeft1.x, bottomLeft2.y);
                Vector2 bottomRightArcCenter = new(bottomRight2.x, bottomRight1.y);

                Vector2[] topLeftArcPoints = GetArcPoints(topLeftArcCenter, clampedRadius, 90f, 180f, resolution);
                Vector2[] topRightArcPoints = GetArcPoints(topRightArcCenter, clampedRadius, 0f, 90f, resolution);
                Vector2[] bottomrightArcPoints = GetArcPoints(bottomRightArcCenter, clampedRadius, 270f, 360f, resolution);
                Vector2[] bottomLeftArcPoints = GetArcPoints(bottomLeftArcCenter, clampedRadius, 180f, 270f, resolution);

                points.AddRange(topLeftArcPoints);
                points.AddRange(bottomLeftArcPoints);
                points.AddRange(bottomrightArcPoints);
                points.AddRange(topRightArcPoints);

                return points.ToArray();
            }

            /// <summary>
            /// Generate arc points
            /// </summary>
            /// <param name="center">Center of Circle</param>
            /// <param name="radius">Radius of Circle</param>
            /// <param name="startAngle">Angle from where arc will begin</param>
            /// <param name="endAngle">Angle at which arc will end</param>
            /// <param name="resolution">How smooth arc is</param>
            /// <returns>Array of Vector2 representing an arc</returns>
            /// <remarks>
            /// 0 angle is at mid of I and IV Quadrant <br/>
            /// 90 angle is at mid of I and II Quadrant
            /// </remarks>
            public static Vector2[] GetArcPoints(Vector2 center, float radius, float startAngle, 
                                                 float endAngle, float resolution)
            {
                List<Vector2> points = new();

                float angleStep = (endAngle - startAngle) / resolution;

                for (int i = 0; i <= resolution; i++)
                {
                    float angle = Mathf.Deg2Rad * (startAngle + i * angleStep);

                    float x = center.x + radius * Mathf.Cos(angle);
                    float y = center.y + radius * Mathf.Sin(angle);

                    points.Add(new Vector2(x, y));
                }

                return points.ToArray();
            }
        }
    }

}