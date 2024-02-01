using UnityEngine;
using DG.Tweening;

namespace DOTweenModular
{
    public abstract class DOShakeBase : DOBase
    {
        #region Properties

        [Tooltip("If TRUE, tween will smoothly reach back to start Position/Rotation/Scale")]
        public bool fadeout = true;

        [Tooltip("Indicates how much will the shake vibrate")]
        public int vibrato = 10;

        [Tooltip("Indicates how much the shake will be random (values higher than 90 kind of suck, so beware) " +
                 "Setting it to 0 will shake along a single direction.")]
        [Range(0f, 180f)]
        public float randomness = 90f;

        [Tooltip("Full - Full randomness " + "\n" +
                 "Harmonic - Creates a more balanced randomness that looks more harmonic")]
        public ShakeRandomnessMode randomnessMode;

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