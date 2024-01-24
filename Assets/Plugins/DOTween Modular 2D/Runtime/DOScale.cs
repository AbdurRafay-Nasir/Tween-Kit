using DG.Tweening;
using UnityEngine;

namespace DOTweenModular2D
{
    [AddComponentMenu("DOTween Modular 2D/Transform/DO Scale", 70)]
    public class DOScale : DOLookAt
    {
        [Tooltip("If TRUE, the tween will Move duration amount in each second")]
        public bool speedBased;

        [Tooltip("If TRUE, the targetScale will be calculated as: " + "\n" +
                "targetScale = targetScale + transform.localScale")]
        public bool relative;

        [Tooltip("The position to reach, if relative is true game object will move as: " + "\n" +
                 "targetScale = targetScale + transform.localScale")]
        public Vector2 targetScale = Vector2.one;

        public override void CreateTween()
        {
            tween = transform.DOScale(targetScale, duration);
        
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

            SetupLookAt();
        }
    }

}