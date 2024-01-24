using DG.Tweening;
using DOTweenModular2D.Miscellaneous;
using UnityEngine;

namespace DOTweenModular2D
{
    [AddComponentMenu("DOTween Modular 2D/Sprite Renderer/DO Size")]
    public class DOSize : DOBase
    {
        [Tooltip("If TRUE, the tween will Move duration amount in each second")]
        public bool speedBased;

        [Tooltip("If TRUE, the targetSize will be calculated as: " + "\n" +
                  "targetSize = targetSize + spriteRenderer.size")]
        public bool relative;

        [Tooltip("If TRUE, the tween will smoothly snap all values to integers")]
        public bool snapping;

        [Tooltip("The size to reach, if relative is true Sprite Rendere size will tween as: " + "\n" +
                 "targetSize = targetSize + spriteRenderer.size")]
        public Vector2 targetSize = Vector2.one;

        public override void CreateTween()
        {
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            tween = sr.DOSize(targetSize, duration);

            if (easeType == Ease.INTERNAL_Custom)
                tween.SetEase(curve);
            else
                tween.SetEase(easeType);

            if (tweenType == Enums.TweenType.Looped)
                tween.SetLoops(loops, loopType);

            tween.SetSpeedBased(speedBased);
            tween.SetRelative(relative);
            tween.SetDelay(delay);

            InvokeTweenCreated();
        }
    }

}