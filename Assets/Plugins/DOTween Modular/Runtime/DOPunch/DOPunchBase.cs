using UnityEngine;
using DG.Tweening;

namespace DOTweenModular
{
    public abstract class DOPunchBase : DOBase
    {
        #region Properties

        [Tooltip("Indicates how much will the punch vibrate")]
        public int vibrato = 10;

        [Tooltip("Represents how much the vector will go beyond the starting position when bouncing" + "\n" +
                 "backwards. 1 creates a full oscillation between the punch direction " + "\n" +
                 "and the opposite direction, while 0 oscillates only between the punch and the start position")]
        [Range(0f, 1f)]
        public float elasticity = 1;

        public Vector3 punch = new Vector3(3f, 3f, 3f);

        #endregion

        public override Tween CreateTween()
        {
            InitializeTween();

            if (easeType == Ease.INTERNAL_Custom)
                Tween.SetEase(curve);
            else
                Tween.SetEase(easeType);

            if (tweenType == Enums.TweenType.Looped)
                Tween.SetLoops(loops, loopType);

            Tween.SetDelay(delay);

            TweenCreated();

            return Tween;
        }

        protected abstract void InitializeTween();
    }
}