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

                Tween childMoveTween;

                if (moveLocally)
                    childMoveTween = child.DOLocalMove(GetLocalTargetPosition(child), 
                                                       duration, snapping);
                else
                    childMoveTween = child.DOMove(GetGlobalTargetPosition(child),
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

        /// <summary>
        /// Returns Target Position in world space for give child defined by Direction and moveLocally
        /// </summary>
        private Vector3 GetGlobalTargetPosition(Transform child)
        {
            Vector3 moveDirection = Vector3.zero;

            switch (direction)
            {
                case Direction.LocalUp:
                    moveDirection = child.up;
                    break;
                case Direction.LocalRight:
                    moveDirection = child.right;
                    break;
                case Direction.LocalForward:
                    moveDirection = child.forward;
                    break;
            }

            return child.position + moveDirection * moveAmount;
        }

        /// <summary>
        /// Returns Target Position in local space for give child defined by Direction and moveLocally
        /// </summary>
        private Vector3 GetLocalTargetPosition(Transform child)
        {
            Vector3 moveDirection = Vector3.zero;

            // Calculate the movement direction based on the specified direction
            switch (direction)
            {
                case Direction.LocalUp:
                    moveDirection = Vector3.up;
                    break;
                case Direction.LocalRight:
                    moveDirection = Vector3.right;
                    break;
                case Direction.LocalForward:
                    moveDirection = Vector3.forward;
                    break;
            }

            // Transform the movement direction from local space to world space using the child's rotation
            moveDirection = child.localRotation * moveDirection;

            // Normalize the movement direction
            moveDirection.Normalize();

            // Calculate the target Position based on moveDirection and moveAmount
            return child.localPosition + moveDirection * moveAmount;
        }
    }
}