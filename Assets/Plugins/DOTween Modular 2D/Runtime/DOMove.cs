using DG.Tweening;
using UnityEngine;

namespace DOTweenModular
{   
    [AddComponentMenu("DOTween Modular 2D/Transform/DO Move", 50)]
    public class DOMove : DOLookAt
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

        [Tooltip("If TRUE, game object will move in local space")]
        public bool useLocal;

        public override void CreateTween()
        {
            if (useLocal)
                tween = transform.DOLocalMove(targetPosition, duration, snapping);
          
            else
                tween = transform.DOMove(targetPosition, duration, snapping);
        
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

            SetupLookAt();
        }
    }
}