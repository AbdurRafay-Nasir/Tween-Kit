using UnityEngine;
using DG.Tweening;
using DOTweenModular.Enums;
using DOTweenModular.Miscellaneous;

namespace DOTweenModular
{
    [AddComponentMenu("DOTween Modular/DO Path")]
    public sealed class DOPath : DOBase
    {
        // The 'resolution' had no effect on creating the Catmull-Rom and Cubic Bezier
        // Paths, the path always had same resolution regardless of 'resolution' variable
        // Therefore, Paths are created with help of Curve.cs

        #region Path Properties

        [Tooltip("Type of Path")]
        public PathType pathType;

        [Tooltip("Used to determine correct LookAt orientation")]
        public PathMode pathMode;

        [Tooltip("Smoothness of the Path, minimum value is 1")]
        [Min(1f)] public int resolution = 1;

        [Tooltip("If TRUE, the last point and transform.position will be connected")]
        public bool closePath;

        [Tooltip("If TRUE, the tween will move 'duration' amount in each second")]
        public bool speedBased;

        [Tooltip("If TRUE, the targetPosition will be calculated as: " + "\n" +
                 "pathPoints[0] = pathPoints[0] + transform.position" + "\n" +
                 "pathPoints[1] = pathPoints[1] + transform.position and so on")]
        public bool relative;

        [Tooltip("The Points through which gameObject will move")]
        public Vector3[] wayPoints;

        #endregion

        #region LookAt Properties

        [Tooltip("The type of Look At: " + "\n" + "\n" +
                 "None - Nothing to Look At, what did you expect?" + "\n" +
                 "Position - The Position to Look At, useful when lookAt target won't move" + "\n" +
                 "Transform - The gameObject to Look At, useful when lookAt target can/will move" + "\n" + 
                 "Percentage - Rotate along the path")]
        public LookAtAdvanced lookAt;

        [Tooltip("The Position to Look At")]
        public Vector3 lookAtPosition;

        [Tooltip("The gameObject to Look At")]
        public Transform lookAtTarget;

        [Tooltip("The percentage of lookAhead to use (0 to 1)")]
        [Range(0f, 1f)] public float lookAhead;

        [Tooltip("If TRUE doesn't rotate the gameObject along the Z-axis")]
        public bool stableZRotation;

        #endregion

        public override Tween CreateTween()
        {
            switch (pathType)
            {
                case PathType.Linear:
                    SetupLinearTweenWithLookAt();
                    break;

                case PathType.CatmullRom:
                    SetupCatmullRomTweenWithLookAt();
                    break;

                case PathType.CubicBezier:
                    SetupCubicBezierTweenWithLookAt();
                    break;
            }

            if (easeType == Ease.INTERNAL_Custom)
                Tween.SetEase(curve);
            else
                Tween.SetEase(easeType);

            if (tweenType == Enums.TweenType.Looped)
                Tween.SetLoops(loops, loopType);

            Tween.SetSpeedBased(speedBased);
            Tween.SetDelay(delay);

            TweenCreated();

            return Tween;
        }

        private void SetupLinearTweenWithLookAt()
        {
            Tween = lookAt switch
            {
                LookAtAdvanced.Position =>   transform.DOPath(wayPoints, duration, PathType.Linear, pathMode, 1)
                                                      .SetOptions(closePath)
                                                      .SetLookAt(lookAtPosition, stableZRotation),

                LookAtAdvanced.Transform =>  transform.DOPath(wayPoints, duration, PathType.Linear, pathMode, 1)
                                                      .SetOptions(closePath)
                                                      .SetLookAt(lookAtTarget, stableZRotation),

                LookAtAdvanced.Percentage => transform.DOPath(wayPoints, duration, PathType.Linear, pathMode, 1)
                                                      .SetOptions(closePath)
                                                      .SetLookAt(lookAhead, stableZRotation),

                _ => transform.DOPath(wayPoints, duration, PathType.Linear, pathMode, 1)
                              .SetOptions(closePath),
            };

            Tween.SetRelative(relative);
        }

        private void SetupCatmullRomTweenWithLookAt()
        {
            Vector3[] absolutePoints = (Vector3[])wayPoints.Clone();

            if (relative)
            {
                for (int i = 0; i < absolutePoints.Length; i++)
                {
                    absolutePoints[i] += transform.position;
                }
            }

            Vector3[] catmullRomPoints = Curve.CatmullRom.GetSpline(transform.position, absolutePoints,
                                                                    resolution, closePath);

            Tween = lookAt switch
            {
                LookAtAdvanced.Position =>   transform.DOPath(catmullRomPoints, duration, PathType.Linear, pathMode, resolution)                                                      
                                                      .SetLookAt(lookAtPosition, stableZRotation),

                LookAtAdvanced.Transform =>  transform.DOPath(catmullRomPoints, duration, PathType.Linear, pathMode, resolution)
                                                      .SetLookAt(lookAtTarget, stableZRotation),

                LookAtAdvanced.Percentage => transform.DOPath(catmullRomPoints, duration, PathType.Linear, pathMode, resolution)
                                                      .SetLookAt(lookAhead, stableZRotation),

                _ => transform.DOPath(catmullRomPoints, duration, PathType.Linear, pathMode, resolution)                              
            };
        }

        private void SetupCubicBezierTweenWithLookAt()
        {
            Vector3[] absolutePoints = (Vector3[])wayPoints.Clone();

            if (relative)
            {
                for (int i = 0; i < absolutePoints.Length; i++)
                {
                    absolutePoints[i] += transform.position;
                }
            }

            Vector3[] cubicBezierPoints = Curve.CubicBezier.GetSpline(transform.position, absolutePoints,
                                                                      resolution);

            Tween = lookAt switch
            {
                LookAtAdvanced.Position =>   transform.DOPath(cubicBezierPoints, duration, PathType.Linear, pathMode, resolution)
                                                      .SetLookAt(lookAtPosition, stableZRotation),

                LookAtAdvanced.Transform =>  transform.DOPath(cubicBezierPoints, duration, PathType.Linear, pathMode, resolution)
                                                      .SetLookAt(lookAtTarget, stableZRotation),

                LookAtAdvanced.Percentage => transform.DOPath(cubicBezierPoints, duration, PathType.Linear, pathMode, resolution)
                                                      .SetLookAt(lookAhead, stableZRotation),

                _ => transform.DOPath(cubicBezierPoints, duration, PathType.Linear, pathMode, resolution)
            };
        }
    }
}