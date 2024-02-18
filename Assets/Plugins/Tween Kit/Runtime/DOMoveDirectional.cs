using UnityEngine;
using DG.Tweening;
using TweenKit.Enums;

namespace TweenKit
{
    [AddComponentMenu("Tween Kit/DO Move Directional")]
    public sealed class DOMoveDirectional : DOBase
    {
        #region Properties

        [Tooltip("The direction to move in")]
        public Direction direction;

        [Tooltip("If TRUE, childs will move in local space")]
        public bool moveLocally;

        [Tooltip("If TRUE, the tween will Move 'duration' amount in each second")]
        public bool speedBased;

        [Tooltip("If TRUE, the tween will smoothly snap all values to integers")]
        public bool snapping;

        [Tooltip("The amount to move in direction defined by 'Direction' property")]
        public float moveAmount = 5f;

        #endregion

        public override Tween CreateTween()
        {
            if (moveLocally)
                Tween = transform.DOLocalMove(GetLocalTargetPosition(), duration, snapping);
            else
                Tween = transform.DOMove(GetGlobalTargetPosition(), duration, snapping);

            if (easeType == Ease.INTERNAL_Custom)
                Tween.SetEase(curve);
            else
                Tween.SetEase(easeType);

            if (tweenType == Enums.TweenType.Looped)
                Tween.SetLoops(loops, loopType);

            Tween.SetSpeedBased(speedBased);
            Tween.SetDelay(delay);

            TweenCreated();

            return Tween;
        }

        /// <summary>
        /// Returns Target Position in world space defined by Direction and moveLocally
        /// </summary>
        private Vector3 GetGlobalTargetPosition()
        {
            Vector3 moveDirection = Vector3.zero;

            switch (direction)
            {
                case Direction.LocalUp:
                    moveDirection = transform.up;
                    break;
                case Direction.LocalRight:
                    moveDirection = transform.right;
                    break;
                case Direction.LocalForward:
                    moveDirection = transform.forward;
                    break;
            }

            return transform.position + moveDirection * moveAmount;
        }

        /// <summary>
        /// Returns Target Position in local space defined by Direction and moveLocally
        /// </summary>
        private Vector3 GetLocalTargetPosition()
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
            moveDirection = transform.localRotation * moveDirection;

            // Normalize the movement direction
            moveDirection.Normalize();

            // Calculate the target Position based on moveDirection and moveAmount
            return transform.localPosition + moveDirection * moveAmount;
        }
    }
}