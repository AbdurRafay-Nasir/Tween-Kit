using UnityEngine;
using DG.Tweening;

namespace DOTweenModular
{
    public sealed class DOJump : DOBase
    {
        #region Properties

        public float power;
        [Min(1)] public int jumpCount;
        public bool snapping;
        public bool useLocal;
        public bool relative;
        public Vector3 targetPosition = new Vector3(10f, 10f, 10f);

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