using UnityEngine;
using DG.Tweening;

namespace DOTweenModular
{
    public sealed class DOScale : DOLookAtBase
    {
        #region Properties

        [Tooltip("If TRUE, the tween will be scaled 'duration' amount in each second")]
        public bool speedBased;

        [Tooltip("If TRUE, the targetScale will be calculated as: " + "\n" +
                "targetScale = targetScale + transform.localScale")]
        public bool relative;

        [Tooltip("The position to reach, if relative is true game object will move as: " + "\n" +
                 "targetScale = targetScale + transform.localScale")]
        public Vector3 targetScale = new Vector3(3f, 3f, 3f);

        #endregion

        public override Tween CreateTween()
        {
            Tween = transform.DOScale(targetScale, duration);

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