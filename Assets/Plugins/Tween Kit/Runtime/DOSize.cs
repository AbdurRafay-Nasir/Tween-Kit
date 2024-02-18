using UnityEngine;
using DG.Tweening;
using TweenKit.Miscellaneous;

namespace TweenKit
{
    [AddComponentMenu("Tween Kit/DO Size")]
    [RequireComponent(typeof(SpriteRenderer))]
    public sealed class DOSize : DOBase
    {
        #region Properties

        [Tooltip("If TRUE, the tween will Move 'duration' amount in each second")]
        public bool speedBased;

        [Tooltip("If TRUE, the targetSize will be calculated as: " + "\n" +
                  "targetSize = targetSize + spriteRenderer.size")]
        public bool relative;

        [Tooltip("The size to reach")]
        public Vector2 targetSize = new(3f, 3f);

        #endregion

        public override Tween CreateTween()
        {
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            Tween = sr.DOSize(targetSize, duration);

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