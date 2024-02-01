using UnityEngine;
using DG.Tweening;
using DOTweenModular.Miscellaneous;

namespace DOTweenModular
{
    public sealed class DOShapeCircle : DOBase
    {
        #region Properties

        [Tooltip("If TRUE, game object will move in local space")]
        public bool useLocal;

        [Tooltip("If TRUE, the center will be calculated as: " + "\n" +
          "center = center + transform.position")]
        public bool relative;

        [Tooltip("If TRUE, the tween will smoothly snap all values to integers")]
        public bool snapping;

        [Tooltip("Center of Circle")]
        public Vector3 center;

        [Tooltip("The degree at which object will stop, 360 means mid of 1st & 2nd Quadrant")]
        public float endDegree;

        #endregion

        public override Tween CreateTween()
        {
            if (useLocal)
                Tween = transform.DOLocalShapeCircle(center, endDegree, duration, snapping);
            else
                Tween = transform.DOShapeCircle(center, endDegree, duration, relative, snapping);

            if (easeType == Ease.INTERNAL_Custom)
                Tween.SetEase(curve);
            else
                Tween.SetEase(easeType);

            if (tweenType == Enums.TweenType.Looped)
                Tween.SetLoops(loops, loopType);

            Tween.SetRelative(relative);
            Tween.SetDelay(delay);

            TweenCreated();

            return Tween;
        }
    }
}