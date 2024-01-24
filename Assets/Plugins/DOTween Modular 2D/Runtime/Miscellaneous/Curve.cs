using System;
using System.Collections.Generic;
using UnityEngine;

namespace DOTweenModular2D.Miscellaneous
{
    /// <summary>
    /// Utility methods for generating various types of curves and <br/> 
    /// for converting points between relative and absolute modes.
    /// </summary>
    public static class Curve
    {

    #region Cubic Bezier

        /// <summary>
        /// Converts the given points into a series of points forming a cubic Bezier curve.
        /// </summary>
        /// <param name="startPosition">The starting position of the path, often `transform.position`.</param>
        /// <param name="points">The points used to create the cubic Bezier curve.</param>
        /// <param name="resolution">The smoothness of the curve; higher values generate smoother paths but are more computationally expensive.</param>
        /// <returns>An array of points representing a cubic Bezier curve.</returns>
        /// <remarks>Returns NULL if points are NULL or the number of points is not a multiple of 3.</remarks>
        public static Vector2[] GetCubicBezierPoints(Vector3 startPosition, Vector2[] points, int resolution)
        {
            if (points == null || points.Length % 3 != 0)
                return null;

            List<Vector2> cubicBezierPoints = new();

            for (int i = 0; i < points.Length; i += 3)
            {
                Vector2 startPoint = (i == 0) ? startPosition : points[i - 3];
                Vector2 endPoint = points[i];
                Vector2 controlPoint1 = points[i + 1];
                Vector2 controlPoint2 = points[i + 2];

                for (int j = 0; j <= resolution; j++)
                {
                    float t = j / (float) resolution;

                    Vector2 point = GetCubicBezierPoint(startPoint, controlPoint1, 
                                                        controlPoint2, endPoint, t);

                    cubicBezierPoints.Add(point);
                }
            }

            return cubicBezierPoints.ToArray();
        }

        /// <summary>
        /// Converts the given points into a series of points forming a cubic Bezier curve, optionally relative to startPosition.
        /// </summary>
        /// <param name="startPosition">The starting position of the path, often `transform.position`.</param>
        /// <param name="points">The points used to create the cubic Bezier curve.</param>
        /// <param name="resolution">The smoothness of the curve; higher values generate smoother paths but are more computationally expensive.</param>
        /// <param name="relative">If true, the points are first converted to world position using startPosition as a reference before generating the curve.</param>
        /// <returns>An array of points representing a cubic Bezier curve.</returns>
        /// <remarks>Returns NULL if points are NULL or the number of points is not a multiple of 3.</remarks>
        public static Vector2[] GetCubicBezierPoints(Vector3 startPosition, Vector2[] points, 
                                                     int resolution, bool relative)
        {
            if (points == null || points.Length % 3 != 0)
                return null;

            if (!relative)
                return GetCubicBezierPoints(startPosition, points, resolution);

            Vector2[] absolutePoints = GetAbsolutePoints(startPosition, points); 

            return GetCubicBezierPoints(startPosition, absolutePoints, resolution);
        }

    #endregion

    #region Catmull-Rom

        /// <summary>
        /// Generates an open Catmull-Rom spline curve.
        /// </summary>
        /// <param name="startPosition">The point from which the curve will start, usually transform.position.</param>
        /// <param name="points">The points used to create the open Catmull-Rom curve.</param>
        /// <param name="resolution">The smoothness of the curve; higher values generate smoother paths but are more computationally expensive.</param>
        /// <returns>An array of points representing an open Catmull-Rom spline curve.</returns>
        /// <remarks>Returns NULL if points are NULL or the number of points is less than 2.</remarks>
        public static Vector2[] GetOpenCatmullRomPoints(Vector3 startPosition, Vector2[] points, int resolution)
        {
            if (points == null || points.Length < 2)
                return null;

            Vector2[] pointsWithStartPosition = new Vector2[points.Length + 1];
            pointsWithStartPosition[0] = startPosition;
            Array.Copy(points, 0, pointsWithStartPosition, 1, points.Length);

            List<Vector2> catmullRomPoints = new();

            // Loop through the points, computing the Catmull-Rom spline segments
            for (int i = 0; i < pointsWithStartPosition.Length - 1; i++)
            {
                // Calculate the control points for this segment
                Vector2 previousPoint = (i == 0) ? pointsWithStartPosition[i] : pointsWithStartPosition[i - 1];
                Vector2 currentPoint = pointsWithStartPosition[i];
                Vector2 nextPoint = pointsWithStartPosition[i + 1];
                Vector2 nextNextPoint = (i + 2 == pointsWithStartPosition.Length) ? 
                                        pointsWithStartPosition[pointsWithStartPosition.Length - 1] : 
                                        pointsWithStartPosition[i + 2];

                for (int j = 0; j <= resolution; j++)
                {
                    float t = (float) j / resolution;
                    Vector2 point = GetCatmullRomPoint(previousPoint, currentPoint, nextPoint, nextNextPoint, t);

                    catmullRomPoints.Add(point);
                }
            }

            return catmullRomPoints.ToArray();
        }

        /// <summary>
        /// Generates an open Catmull-Rom spline curve, optionally relative to a starting position.
        /// </summary>
        /// <param name="startPosition">The point from which the curve will start, usually transform.position.</param>
        /// <param name="points">The points used to create the open Catmull-Rom curve.</param>
        /// <param name="resolution">The smoothness of the curve; higher values generate smoother paths but are more computationally expensive.</param>
        /// <param name="relative">If true, the points are first converted to world position using startPosition as a reference before generating the curve.</param>
        /// <returns>An array of points representing an open Catmull-Rom spline curve.</returns>
        /// <remarks>Returns NULL if points are NULL or the number of points is less than 2.</remarks>
        public static Vector2[] GetOpenCatmullRomPoints(Vector3 startPosition, Vector2[] points, int resolution, bool relative)
        {
            if (points == null || points.Length < 2)
                return null;

            if (!relative) 
                return GetOpenCatmullRomPoints(startPosition, points, resolution);

            Vector2[] absolutePoints = GetAbsolutePoints(startPosition, points);

            return GetOpenCatmullRomPoints(startPosition, absolutePoints, resolution);
        }

        /// <summary>
        /// Generates a closed Catmull-Rom spline curve.
        /// </summary>
        /// <param name="startPosition">The point from which the curve will start, typically transform.position.</param>
        /// <param name="points">The points used to create the open Catmull-Rom curve.</param>
        /// <param name="resolution">The smoothness of the curve; higher values generate smoother paths but are more computationally expensive.</param>
        /// <returns>An array of points representing a closed Catmull-Rom spline curve.</returns>
        /// <remarks>Returns NULL if points are NULL or the number of points is less than 2.</remarks>
        public static Vector2[] GetClosedCatmullRomPoints(Vector3 startPosition, Vector2[] points, int resolution)
        {
            if (points == null || points.Length < 2)
                return null;

            Vector2[] pointsWithStartPosition = new Vector2[points.Length + 1];
            pointsWithStartPosition[0] = startPosition;
            Array.Copy(points, 0, pointsWithStartPosition, 1, points.Length);

            List<Vector2> catmullRomPoints = new();

            // Loop through the points, computing the Catmull-Rom spline segments
            for (int i = 0; i < pointsWithStartPosition.Length; i++)
            {
                int prevIndex = (i - 1 + pointsWithStartPosition.Length) % pointsWithStartPosition.Length;
                int nextIndex = (i + 1) % pointsWithStartPosition.Length;

                Vector2 previousPoint = pointsWithStartPosition[prevIndex];
                Vector2 currentPoint = pointsWithStartPosition[i];
                Vector2 nextPoint = pointsWithStartPosition[nextIndex];
                Vector2 nextNextPoint = pointsWithStartPosition[(nextIndex + 1) % pointsWithStartPosition.Length];

                for (int j = 0; j <= resolution; j++)
                {
                    float t = (float) j / resolution;
                    Vector3 point = GetCatmullRomPoint(previousPoint, currentPoint, 
                                                            nextPoint, nextNextPoint, t);

                    catmullRomPoints.Add(point);
                }
            }

            return catmullRomPoints.ToArray();
        }

        /// <summary>
        /// Generates a closed Catmull-Rom spline curve, optionally relative to a starting position.
        /// </summary>
        /// <param name="startPosition">The point from which the curve will start, usually transform.position.</param>
        /// <param name="points">The points used to create the open Catmull-Rom curve.</param>
        /// <param name="resolution">The smoothness of the curve; higher values generate smoother paths but are more computationally expensive.</param>
        /// <param name="relative">If true, the points are first converted to world position using startPosition as a reference before generating the curve.</param>
        /// <returns>An array of points representing a closed Catmull-Rom spline curve.</returns>
        /// <remarks>Returns null if points are null or the number of points is less than 2.</remarks>
        public static Vector2[] GetClosedCatmullRomPoints(Vector3 startPosition, Vector2[] points, int resolution, bool relative)
        {
            if (points == null || points.Length < 2)
                return null;

            if (!relative) 
                return GetClosedCatmullRomPoints(startPosition, points, resolution);

            Vector2[] absolutePoints = GetAbsolutePoints(startPosition, points);

            // Call GetCatmullRomPoints to generate the closed Catmull-Rom spline
            return GetClosedCatmullRomPoints(startPosition, absolutePoints, resolution);
        }

    #endregion

    #region Get Point on Curve Segment Functions

        /// <summary>
        /// Get Catmull Rom Point for given segment
        /// </summary>
        public static Vector3 GetCatmullRomPoint(Vector3 previousPoint, Vector3 currentPoint, Vector3 nextPoint, Vector3 nextNextPoint, float t)
        {
            float t2 = t * t;
            float t3 = t2 * t;

            float b1 = 0.5f * (-t3 + 2.0f * t2 - t);
            float b2 = 0.5f * (3.0f * t3 - 5.0f * t2 + 2.0f);
            float b3 = 0.5f * (-3.0f * t3 + 4.0f * t2 + t);
            float b4 = 0.5f * (t3 - t2);

            return b1 * previousPoint + b2 * currentPoint + b3 * nextPoint + b4 * nextNextPoint;
        }

        /// <summary>
        /// Get Cubic Bezier Point for given segment
        /// </summary>
        public static Vector3 GetCubicBezierPoint(Vector3 startPoint, Vector3 controlPoint1, 
                                            Vector3 controlPoint2, Vector3 endPoint, float t)
        {
            float u = 1 - t;
            float tt = t * t;
            float uu = u * u;
            float uuu = uu * u;
            float ttt = tt * t;

            Vector3 p = uuu * startPoint;
            p += 3 * uu * t * controlPoint1;
            p += 3 * u * tt * controlPoint2;
            p += ttt * endPoint;

            return p;
        }

    #endregion

    #region Get Absolute/Relative Points Functions

        /// <summary>
        /// Converts an array of points from world position to be relative to a specified reference point.
        /// </summary>
        /// <param name="relativeTo">The reference point to which the points will be made relative.</param>
        /// <param name="points">An array of points to be transformed.</param>
        /// <returns>An array of points relative to the specified reference point.</returns>
        public static Vector2[] GetRelativePoints(Vector2 relativeTo, Vector2[] points)
        {
            Vector2[] pointsCopy = (Vector2[])points.Clone();
        
            for (int i = 0; i < points.Length; i++)
                pointsCopy[i] -= relativeTo;

            return pointsCopy;
        }

        /// <summary>
        /// Converts an array of points from relative to a specified reference point to absolute positions.
        /// </summary>
        /// <param name="absoluteTo">The reference point to which the points will be made absolute.</param>
        /// <param name="points">An array of points in relative coordinates.</param>
        /// <returns>An array of points in absolute coordinates (i.e,. World space).</returns>
        public static Vector2[] GetAbsolutePoints(Vector2 absoluteTo, Vector2[] points)
        {
            Vector2[] pointsCopy = (Vector2[])points.Clone();

            for (int i = 0; i < points.Length; i++)
                pointsCopy[i] += absoluteTo;            

            return pointsCopy;
        }

    #endregion

    }

}