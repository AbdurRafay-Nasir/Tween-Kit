using UnityEngine;
using DG.Tweening;

namespace DOTweenModular
{
    [AddComponentMenu("Tween Kit/DO Jump")]
    public sealed class DOJump : DOBase
    {
        #region Properties

        [Tooltip("Power of the jump (the max height of the jump is represented by this plus the final Y offset)")]
        public float power = 5f;

        [Tooltip("Total number of jumps, atleast 1")]
        [Min(1)] public int jumpCount = 1;

        [Tooltip("If TRUE the tween will smoothly snap all values to integers")]
        public bool snapping;

        [Tooltip("If TRUE the gameObject will move in Local Space, regardless of 'relative'")]
        public bool useLocal;

        [Tooltip("If TRUE the gameObject will move relative to current Position in world space, regardless of 'Use Local'" + "\n" +
                 "targetPosition will be calculated as:" + "\n" +
                 "targetPosition = targetPosition + tranform.position")]
        public bool relative;

        [Tooltip("The Position to reach")]
        public Vector3 targetPosition = new Vector3(1f, 1f, 1f);

        #endregion

        public override Tween CreateTween()
        {
            if (useLocal)
                Tween = transform.DOLocalJump(targetPosition, power, jumpCount, duration, snapping);
            else
                Tween = transform.DOJump(targetPosition, power, jumpCount, duration, snapping);

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