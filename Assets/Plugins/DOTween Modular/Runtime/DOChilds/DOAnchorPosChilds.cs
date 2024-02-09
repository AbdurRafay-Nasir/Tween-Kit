using UnityEngine;
using DG.Tweening;

namespace DOTweenModular
{
    [AddComponentMenu("DOTween Modular/DO Anchor Pos Childs")]
    public sealed class DOAnchorPosChilds : DOBase
    {
        #region Properties

        [Tooltip("If TRUE, All childs will move simultaneously")]
        public bool join = true;

        [Tooltip("If TRUE the childs will move relative to their current Position in world space" + "\n" +
                 "Target Position for each child will be calculated as: " + "\n" +
                 "Target Position = Target Position + child (world space) current position")]
        public bool relative;

        [Tooltip("If TRUE, the tween will smoothly snap all values to integers")]
        public bool snapping;

        [Tooltip("The position to reach")]
        public Vector2 targetPosition;

        #endregion

        public override Tween CreateTween()
        {
            Sequence childsAnchorPosSequence = DOTween.Sequence();

            for (int i = 0; i < transform.childCount; i++)
            {
                RectTransform childRectTransform = (RectTransform)transform.GetChild(i);

                Tween childMoveTween = childRectTransform.DOAnchorPos(targetPosition, duration, snapping);

                if (easeType == Ease.INTERNAL_Custom)
                    childMoveTween.SetEase(curve);
                else
                    childMoveTween.SetEase(easeType);

                childMoveTween.SetRelative(relative);
                childMoveTween.SetDelay(delay);

                if (join)
                    childsAnchorPosSequence.Join(childMoveTween);
                else
                    childsAnchorPosSequence.Append(childMoveTween);
            }

            if (tweenType == Enums.TweenType.Looped)
                childsAnchorPosSequence.SetLoops(loops, loopType);

            Tween = childsAnchorPosSequence;

            TweenCreated();

            return Tween;
        }
    }
}