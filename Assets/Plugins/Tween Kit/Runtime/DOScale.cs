using UnityEngine;
using DG.Tweening;

namespace DOTweenModular
{
    [AddComponentMenu("DOTween Modular/DO Scale")]
    public sealed class DOScale : DOLookAtBase
    {
        #region Properties

        [Tooltip("If TRUE, the gameObject will be scaled 'duration' amount in each second")]
        public bool speedBased;

        [Tooltip("If TRUE, the targetScale will be calculated as: " + "\n" +
                 "targetScale = targetScale + transform.localScale")]
        public bool relative;

        [Tooltip("The scale to reach")]
        public Vector3 targetScale = new(3f, 3f, 3f);

        #endregion

        public override Tween CreateTween()
        {
            // DOScale works on local scale
            Tween = transform.DOScale(targetScale, duration);

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