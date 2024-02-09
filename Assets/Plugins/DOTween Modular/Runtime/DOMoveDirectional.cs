using UnityEngine;
using DG.Tweening;
using DOTweenModular.Enums;

namespace DOTweenModular
{
    [AddComponentMenu("DOTween Modular/DO Move Directional")]
    public sealed class DOMoveDirectional : DOBase
    {
        #region Properties

        [Tooltip("The direction to move in")]
        public Direction direction;

        [Tooltip("If TRUE, the tween will Move 'duration' amount in each second")]
        public bool speedBased;

        [Tooltip("If TRUE, the tween will smoothly snap all values to integers")]
        public bool snapping;

        [Tooltip("The amount to move in direction defined by 'Direction' property")]
        public float moveAmount = 5f;

        #endregion

        public override Tween CreateTween()
        {
            Vector3 moveDirection = Vector3.zero;

            // Calculate the movement direction based on the specified direction
            switch (direction)
            {
                case Direction.LocalUp:
                    moveDirection = transform.TransformDirection(Vector3.up);
                    break;
                case Direction.LocalRight:
                    moveDirection = transform.TransformDirection(Vector3.right);
                    break;
                case Direction.LocalForward:
                    moveDirection = transform.TransformDirection(Vector3.forward);
                    break;
            }

            Tween = transform.DOMove(transform.position + moveDirection * moveAmount, duration, snapping);

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
    }
}