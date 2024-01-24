using DG.Tweening;
using UnityEngine;

namespace DOTweenModular2D
{
    [AddComponentMenu("DOTween Modular 2D/UI/DO Anchor Pos")]
    public class DOAnchorPos : DOBase
    {
        [Tooltip("If TRUE, the tween will Move duration amount in each second")]
        public bool speedBased;

        [Tooltip("If TRUE, the targetPosition will be calculated as: " + "\n" +
                  "targetPosition = targetPosition + transform.position")]
        public bool relative;

        [Tooltip("If TRUE, the tween will smoothly snap all values to integers")]
        public bool snapping;

        [Tooltip("The position to reach, if relative is true game object will move as: " + "\n" +
                 "targetPosition = targetPosition + transform.position")]
        public Vector2 targetPosition;

        public override void CreateTween()
        {
            RectTransform rectTransform = (RectTransform)transform;
            tween = rectTransform.DOAnchorPos(targetPosition, duration, snapping);

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
