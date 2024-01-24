using DOTweenModular2D.Enums;
using DOTweenModular2D.Miscellaneous;
using DG.Tweening;
using UnityEngine;

namespace DOTweenModular2D
{
    [AddComponentMenu("DOTween Modular 2D/Transform/DO Shape Circle", 40)]
    public class DOShapeCircle : DOLookAt
    {
        public LookAtPath look;

        [Tooltip("If TRUE, game object will move in local space")]
        public bool useLocal;

        [Tooltip("If TRUE, the center will be calculated as: " + "\n" +
          "center = center + transform.position")]
        public bool relative;

        [Tooltip("If TRUE, the tween will smoothly snap all values to integers")]
        public bool snapping;

        [Tooltip("Center of Circle")]
        public Vector2 center;

        [Tooltip("The degree at which object will stop, 360 means mid of 1st & 2nd Quadrant")]
        public float endDegree;

        public override void CreateTween()
        {
            if (useLocal)
                tween = transform.DOLocalShapeCircle(center, endDegree, duration, snapping);
            else
                tween = transform.DOShapeCircle(center, endDegree, duration, relative, snapping);

            if (easeType == Ease.INTERNAL_Custom)
                tween.SetEase(curve);
            else
                tween.SetEase(easeType);

            if (tweenType == Enums.TweenType.Looped)
                tween.SetLoops(loops, loopType);

            tween.SetDelay(delay);

            InvokeTweenCreated();

            SetupLookAt();
        }

        private new void SetupLookAt()
        {
            switch (look)
            {
                case LookAtPath.Position:
                    tween.onUpdate += LookAtPositionUpdate;
                    break;

                case LookAtPath.Transform:
                    tween.onUpdate += LookAtTransformUpdate;
                    break;

                case LookAtPath.Percentage:
                    tween.onUpdate += LookAtPercentageUpdate;
                    break;

                default:
                    break;
            }
        }

        private void LookAtPositionUpdate()
        {
            transform.LookAt2DSmooth(lookAtPosition, offset, smoothFactor, min, max);
        }

        private void LookAtTransformUpdate()
        {
            transform.LookAt2DSmooth(lookAtTarget, offset, smoothFactor, min, max);
        }

        private void LookAtPercentageUpdate()
        {
            Vector2 dir = center - (Vector2)transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.LookAt2DSmooth(lookAtTarget, offset, smoothFactor, min, max);
        }
    }
}
