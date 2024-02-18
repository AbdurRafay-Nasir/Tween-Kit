using UnityEngine;
using DG.Tweening;

namespace DOTweenModular
{
    [AddComponentMenu("DOTween Modular/DO Anchor Pos")]
    public sealed class DOAnchorPos : DOBase
    {
        #region Properties

        [Tooltip("If TRUE, the tween will Move 'duration' amount in each second")]
        public bool speedBased;

        [Tooltip("If TRUE, the targetPosition will be calculated as: " + "\n" +
                  "targetPosition = targetPosition + transform.position")]
        public bool relative;

        [Tooltip("If TRUE, the tween will smoothly snap all values to integers")]
        public bool snapping;

        [Tooltip("The position to reach, if relative is true game object will move as: " + "\n" +
                 "targetPosition = targetPosition + transform.position")]
        public Vector2 targetPosition;

        #endregion

        public override Tween CreateTween()
        {
            RectTransform rectTransform = (RectTransform)transform;
            Tween = rectTransform.DOAnchorPos(targetPosition, duration, snapping);

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