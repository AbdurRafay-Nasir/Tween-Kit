using DG.Tweening;
using UnityEngine;

namespace DOTweenModular2D
{
    public abstract class DOPunchBase : DOBase
    {
        [Tooltip("Indicates how much will the punch vibrate")]
        public int vibrato = 10;

        [Tooltip("Represents how much the vector will go beyond the starting position when bouncing" + "\n" +
                 "backwards. 1 creates a full oscillation between the punch direction " + "\n" +
                 "and the opposite direction, while 0 oscillates only between the punch and the start position")]
        [Range(0f, 1f)]
        public float elasticity = 1;

        public override void CreateTween()
        {
            InitializeTween();

            ApplyTweenSettings();

            InvokeTweenCreated();
        }

        protected abstract void InitializeTween();

        private void ApplyTweenSettings()
        {
            if (easeType == Ease.INTERNAL_Custom)
            {
                tween.SetEase(curve);
            }
            else
            {
                tween.SetEase(easeType);
            }

            if (tweenType == DOTweenModular2D.Enums.TweenType.Looped)
                tween.SetLoops(loops, loopType);

            tween.SetDelay(delay);
        }
    }
}

