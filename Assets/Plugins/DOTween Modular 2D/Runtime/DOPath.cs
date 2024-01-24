using DOTweenModular2D.Enums;
using DOTweenModular2D.Miscellaneous;
using System;
using DG.Tweening;
using UnityEngine;

namespace DOTweenModular2D
{

    [AddComponentMenu("DOTween Modular 2D/Transform/DO Path", 80)]
    public class DOPath : DOBase
    {
        // Path Settings
        [Tooltip("Type of Path")]
        public PathType pathType;

        [Tooltip("Smoothness of the Path")]
        [Min(1f)] public int resolution = 1;

        // Look At Settings
        [Tooltip("Type of Look At")]
        public LookAtPath lookAt;

        [Tooltip("The game Object to Look At")]
        public Transform lookAtTarget;

        [Tooltip("The position to Look At")]
        public Vector2 lookAtPosition;

        [Tooltip("The offet to add to rotation, value of -90 means the game object will look directly towards lookAtPosition/lookAtTarget")]
        public float offset = -90f;

        [Tooltip("Smoothness of rotation, 1 means there will be no smoothness")]
        [Range(0f, 1f)] public float smoothFactor = 1f;

        [Tooltip("Look At percentage, smaller values tend to work better")]
        [Range(0f, 1f)] public float percentage = 0.01f;

        // Values
        [Tooltip("If TRUE, the tween will Move duration amount in each second")]
        public bool speedBased;

        [Tooltip("If TRUE, the targetPosition will be calculated as: " + "\n" + 
                 "pathPoints[0] = pathPoints[0] + transform.position" + "\n" +
                 "pathPoints[1] = pathPoints[1] + transform.position and so on")]
        public bool relative;

        [Tooltip("If TRUE, connects transform.position and end Path Point")]
        public bool connectStartAndEnd;

        [Tooltip("The Points at which object will move")]
        public Vector2[] pathPoints;

        public override void CreateTween()
        {
            Vector2[] points = new Vector2[0];

            switch (pathType)
            {
                case PathType.Linear:
                    points = pathPoints;
                break;

                case PathType.CatmullRom:
                    if (connectStartAndEnd)
                    {
                        points = Curve.GetClosedCatmullRomPoints(transform.position, pathPoints,
                                                                    resolution, relative);
                    }
                    else
                    {
                        points = Curve.GetOpenCatmullRomPoints(transform.position, pathPoints,
                                                                  resolution, relative);
                    }
                break;

                case PathType.CubicBezier:
                    points = Curve.GetCubicBezierPoints(transform.position, pathPoints, resolution, relative);
                break;
            }

            if (pathType == PathType.Linear)
            {
                if (lookAt == LookAtPath.None)
                {
                    tween = transform.DOPath(points.ToVector3Array(), duration, PathType.Linear, PathMode.Sidescroller2D, 0)
                                     .SetOptions(connectStartAndEnd);
                }
                else if (lookAt == LookAtPath.Percentage)
                {
                    tween = transform.DOPath(points.ToVector3Array(), duration, PathType.Linear, PathMode.Sidescroller2D, 0)                                 
                                     .SetOptions(connectStartAndEnd)
                                     .SetLookAt(percentage);
                }
                else
                {
                    tween = transform.DOPath(points.ToVector3Array(), duration, PathType.Linear, PathMode.Sidescroller2D, 0)                                 
                                     .SetOptions(connectStartAndEnd);
                }
            }
            else
            {
                if (lookAt == LookAtPath.Percentage)
                {
                    tween = transform.DOPath(points.ToVector3Array(), duration, PathType.Linear, PathMode.Sidescroller2D, 0)
                                     .SetLookAt(percentage);
                }
                else
                {
                    tween = transform.DOPath(points.ToVector3Array(), duration, PathType.Linear, PathMode.Sidescroller2D, 0);
                }
            }

            if (easeType == Ease.INTERNAL_Custom)
                tween.SetEase(curve);
            else
                tween.SetEase(easeType);

            if (tweenType == Enums.TweenType.Looped)
                tween.SetLoops(loops, loopType);

            if (pathType == PathType.Linear)
                tween.SetRelative(relative);
             
            tween.SetSpeedBased(speedBased);
            tween.SetDelay(delay);

            InvokeTweenCreated();

            if (lookAt != LookAtPath.None && lookAt != LookAtPath.Percentage)
            {
                tween.onUpdate += OnTweenUpdated;
            }
        }

        private void OnTweenUpdated()
        {
            if (lookAt == LookAtPath.Position)
            {
                transform.LookAt2DSmooth(lookAtPosition, offset, smoothFactor);
            }
            else
            {
                transform.LookAt2DSmooth(lookAtTarget, offset, smoothFactor);
            }
        }
    }

}