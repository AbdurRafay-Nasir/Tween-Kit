using DG.Tweening;
using UnityEngine;

namespace DOTweenModular2D
{
    [AddComponentMenu("DOTween Modular 2D/Transform/DO Jump")]
    public class DOJump : DOLookAt
    {
        [Tooltip("If TRUE, the targetPosition will be calculated as: " + "\n" +
                  "targetPosition = targetPosition + transform.position")]
        public bool relative;

        [Tooltip("If TRUE, game object will move in local space")]
        public bool useLocal;

        [Tooltip("If TRUE, the tween will smoothly snap all values to integers")]
        public bool snapping;

        [Tooltip("The position to reach, if relative is true game object will move as: " + "\n" +
                 "targetPosition = targetPosition + transform.position")]
        public Vector2 targetPosition;

        [Tooltip("Maximum height object should reach")]
        public float jumpPower;

        [Tooltip("Number of Jumps")]
        [Min(1)] public int jumps = 1;

        public override void CreateTween()
        {
            if (useLocal)
                tween = transform.DOLocalJump(targetPosition, jumpPower, jumps, duration, snapping);
            else
                tween = transform.DOJump(targetPosition, jumpPower, jumps, duration, snapping);

            if (easeType == Ease.INTERNAL_Custom)
                tween.SetEase(curve);
            else
                tween.SetEase(easeType);

            if (tweenType == Enums.TweenType.Looped)
                tween.SetLoops(loops, loopType);

            if (!useLocal) 
                tween.SetRelative(relative);

            tween.SetDelay(delay);

            InvokeTweenCreated();

            SetupLookAt();
        }
    }
}
