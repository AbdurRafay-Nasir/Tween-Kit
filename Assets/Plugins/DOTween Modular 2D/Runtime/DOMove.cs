using DG.Tweening;
using UnityEngine;

namespace DOTweenModular
{   
    [AddComponentMenu("DOTween Modular 2D/Transform/DO Move", 50)]
    public class DOMove : DOLookAtBase
    {
        #region Properties

        [Tooltip("If TRUE, the tween will Move duration amount in each second")]
        public bool speedBased;

        [Tooltip("If TRUE, the targetPosition will be calculated as: " + "\n" +
                  "targetPosition = targetPosition + transform.position")]
        public bool relative;

        [Tooltip("If TRUE, the tween will smoothly snap all values to integers")]
        public bool snapping;

        [Tooltip("The position to reach, if relative is true game object will move as: " + "\n" + 
                 "targetPosition = targetPosition + transform.position")]
        public Vector3 targetPosition;

        [Tooltip("If TRUE, game object will move in local space")]
        public bool useLocal;

        #endregion

        public override Tween CreateTween()
        {
            if (useLocal)
                Tween = transform.DOLocalMove(targetPosition, duration, snapping);

            else
                Tween = transform.DOMove(targetPosition, duration, snapping);

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