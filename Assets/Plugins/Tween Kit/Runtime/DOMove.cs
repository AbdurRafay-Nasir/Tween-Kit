using DG.Tweening;
using UnityEngine;

namespace TweenKit
{   
    [AddComponentMenu("Tween Kit/DO Move")]
    public sealed class DOMove : DOLookAtBase
    {
        #region Properties

        [Tooltip("If TRUE, the tween will Move 'duration' amount in each second")]
        public bool speedBased;

        [Tooltip("If TRUE, the tween will smoothly snap all values to integers")]
        public bool snapping;

        [Tooltip("If TRUE the gameObject will move in Local Space, regardless of 'relative'")]
        public bool useLocal;

        [Tooltip("If TRUE the gameObject will move relative to current Position in world space, regardless of 'Use Local'" + "\n" + 
                 "targetPosition will be calculated as: " + "\n" +
                 "targetPosition = targetPosition + transform.position")]
        public bool relative;

        [Tooltip("The position to reach")]
        public Vector3 targetPosition;

        #endregion

        public override Tween CreateTween()
        {
            if (useLocal)
                Tween = transform.DOLocalMove(targetPosition, duration, snapping);

            else
                Tween = transform.DOMove(targetPosition, duration, snapping);

            if (easeType == Ease.INTERNAL_Custom)
                Tween.SetEase(curve);
            else
                Tween.SetEase(easeType);

            if (tweenType == Enums.TweenType.Looped)
                Tween.SetLoops(loops, loopType);

            Tween.SetSpeedBased(speedBased);
            Tween.SetRelative(relative);
            Tween.SetDelay(delay);

            TweenCreated();

            return Tween;
        }
    }
}