using UnityEngine;
using DG.Tweening;
using DOTweenModular.Miscellaneous;

namespace DOTweenModular
{
    [AddComponentMenu("Tween Kit/DO Move Circle")]
    public sealed class DOMoveCircle : DOBase
    {
        #region Properties

        [Tooltip("If TRUE the gameObject will move in Local Space, regardless of 'relative")]
        public bool useLocal;

        [Tooltip("If TRUE the gameObject will move relative to current Position in world space, regardless of 'Use Local'" + "\n" +
                 "center will be calculated as: " + "\n" +
                 "center = center + transform.position")]
        public bool relative;

        [Tooltip("If TRUE, the tween will smoothly snap all values to integers")]
        public bool snapping;

        [Tooltip("Center of Circle")]
        public Vector2 center;

        [Tooltip("The degree at which object will stop, 360 means mid of 1st & 2nd Quadrant")]
        public float endDegree;

        #endregion

        public override Tween CreateTween()
        {
            if (useLocal)
                Tween = transform.DOLocalMoveCircle(center, endDegree, duration, snapping);
            else
                Tween = transform.DOMoveCircle(center, endDegree, duration, relative, snapping);

            if (easeType == Ease.INTERNAL_Custom)
                Tween.SetEase(curve);
            else
                Tween.SetEase(easeType);

            if (tweenType == Enums.TweenType.Looped)
                Tween.SetLoops(loops, loopType);

            Tween.SetDelay(delay);

            TweenCreated();

            return Tween;
        }
    }
}