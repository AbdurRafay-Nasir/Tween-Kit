using UnityEngine;
using DG.Tweening;

namespace DOTweenModular
{
    [AddComponentMenu("DOTween Modular/DO Move Childs")]
    public sealed class DOMoveChilds : DOBase
    {
        #region Properties

        [Tooltip("If TRUE, All childs will move simultaneously")]
        public bool join;

        [Tooltip("If TRUE the childs will move relative to their current Position in world space" + "\n" +
                 "Target Position for each child will be calculated as: " + "\n" +
                 "Target Position = Target Position + child current position")]
        public bool relative;

        [Tooltip("If TRUE, the tween will smoothly snap all values to integers")]
        public bool snapping;

        [Tooltip("The position to reach")]
        public Vector3 targetPosition;

        #endregion

        public override Tween CreateTween()
        {
            Sequence childsMoveSequence = DOTween.Sequence();

            for (int i = 0; i < transform.childCount; i++)
            {
                Tween childMoveTween;

                childMoveTween = transform.GetChild(i).DOMove(targetPosition, duration, snapping);

                if (easeType == Ease.INTERNAL_Custom)
                    childMoveTween.SetEase(curve);
                else
                    childMoveTween.SetEase(easeType);
                
                childMoveTween.SetRelative(relative);
                childMoveTween.SetDelay(delay);

                if (join)
                    childsMoveSequence.Join(childMoveTween);
                else
                    childsMoveSequence.Append(childMoveTween);
            }

            if (tweenType == Enums.TweenType.Looped)
                childsMoveSequence.SetLoops(loops, loopType);

            Tween = childsMoveSequence;

            TweenCreated();

            return Tween;
        }
    }
}