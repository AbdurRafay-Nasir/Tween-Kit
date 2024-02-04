using UnityEngine;
using DG.Tweening;
using DOTweenModular.Enums;
using DOTweenModular.Miscellaneous;

namespace DOTweenModular
{
    public sealed class DOPath : DOBase
    {
        #region Path Properties

        [Tooltip("Type of Path")]
        public PathType pathType;

        public PathMode pathMode;

        [Tooltip("Smoothness of the Path")]
        [Min(1f)] public int resolution = 1;

        public bool closePath;

        [Tooltip("If TRUE, the tween will Move duration amount in each second")]
        public bool speedBased;

        [Tooltip("If TRUE, the targetPosition will be calculated as: " + "\n" +
                 "pathPoints[0] = pathPoints[0] + transform.position" + "\n" +
                 "pathPoints[1] = pathPoints[1] + transform.position and so on")]
        public bool relative;

        [Tooltip("The Points at which object will move")]
        public Vector3[] pathPoints;

        #endregion

        #region LookAt Properties

        public LookAtAdvanced lookAt;
        public Vector3 lookAtPosition;
        public Transform lookAtTarget;
        [Range(0f, 1f)] public float lookAhead;
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

            Tween.SetRelative(relative);
            Tween.SetSpeedBased(speedBased);
            Tween.SetDelay(delay);

            TweenCreated();

            return Tween;
        }

        private void SetupLinearTweenWithLookAt()
        {
            Tween = lookAt switch
            {
                LookAtAdvanced.Position =>   transform.DOPath(pathPoints, duration, PathType.Linear, pathMode, 1)
                                                      .SetOptions(closePath)
                                                      .SetLookAt(lookAtPosition, stableZRotation),

                LookAtAdvanced.Transform =>  transform.DOPath(pathPoints, duration, PathType.Linear, pathMode, 1)
                                                      .SetOptions(closePath)
                                                      .SetLookAt(lookAtTarget, stableZRotation),

                LookAtAdvanced.Percentage => transform.DOPath(pathPoints, duration, PathType.Linear, pathMode, 1)
                                                      .SetOptions(closePath)
                                                      .SetLookAt(lookAhead, stableZRotation),

                _ => transform.DOPath(pathPoints, duration, PathType.Linear, pathMode, 1)
                              .SetOptions(closePath),
            };
        }

        private void SetupCatmullRomTweenWithLookAt()
        {
            Vector3[] catmullRomPoints = Curve.CatmullRom.GetSpline(transform.position, pathPoints,
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
            Vector3[] cubicBezierPoints = Curve.CubicBezier.GetSpline(transform.position, pathPoints,
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