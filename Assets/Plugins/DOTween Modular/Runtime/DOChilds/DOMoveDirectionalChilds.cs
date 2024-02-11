using UnityEngine;
using DG.Tweening;
using DOTweenModular.Enums;

namespace DOTweenModular
{
    [AddComponentMenu("DOTween Modular/DO Move Directional Childs")]
    public sealed class DOMoveDirectionalChilds : DOBase
    {
        #region Properties

        [Tooltip("If TRUE, All childs will move simultaneously")]
        public bool join = true;

        [Tooltip("The direction to move in")]
        public Direction direction;

        [Tooltip("If TRUE, childs will move in local space")]
        public bool moveLocally;

        [Tooltip("If TRUE, the tween will smoothly snap all values to integers")]
        public bool snapping;

        [Tooltip("The amount to move in direction defined by 'Direction' property")]
        public float moveAmount = 5f;

        #endregion

        public override Tween CreateTween()
        {
            Sequence childsMoveSequence = DOTween.Sequence();

            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);

                Vector3 childMoveDirection = Vector3.zero;

                switch (direction)
                {
                    case Direction.LocalUp:
                        childMoveDirection = child.TransformDirection(Vector3.up);
                        break;
                    case Direction.LocalRight:
                        childMoveDirection = child.TransformDirection(Vector3.right);
                        break;
                    case Direction.LocalForward:
                        childMoveDirection = child.TransformDirection(Vector3.forward);
                        break;
                }

                Tween childMoveTween;

                if (moveLocally)
                    childMoveTween = child.DOLocalMove(child.position + childMoveDirection * moveAmount, 
                                                       duration, snapping);
                else
                    childMoveTween = child.DOMove(child.position + childMoveDirection * moveAmount,
                                                  duration, snapping);

                if (easeType == Ease.INTERNAL_Custom)
                    childMoveTween.SetEase(curve);
                else
                    childMoveTween.SetEase(easeType);

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