using UnityEngine;
using DG.Tweening;
using DOTweenModular.Enums;

namespace DOTweenModular
{
    [AddComponentMenu("Tween Kit/DO Move Directional Childs")]
    public sealed class DOMoveDirectionalChilds : DOChildsBase
    {
        #region Properties

        [Tooltip("The direction to move in")]
        public Direction direction;

        [Tooltip("If TRUE, childs will move in local space")]
        public bool moveLocally;

        [Tooltip("If TRUE, the tween will smoothly snap all values to integers")]
        public bool snapping;

        [Tooltip("The amount to move in direction defined by 'Direction' property")]
        public float moveAmount = 5f;

        #endregion

        protected override Tween InitializeChildTween(Transform currentChild)
        {
            Tween childMoveTween;

            if (moveLocally)
                childMoveTween = currentChild.DOLocalMove(GetLocalTargetPosition(currentChild),
                                                          duration, snapping);
            else
                childMoveTween = currentChild.DOMove(GetGlobalTargetPosition(currentChild),
                                                     duration, snapping);

            return childMoveTween;
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