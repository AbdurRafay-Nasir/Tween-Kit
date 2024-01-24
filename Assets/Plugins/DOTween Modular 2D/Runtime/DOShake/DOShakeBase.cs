using DG.Tweening;
using UnityEngine;

namespace DOTweenModular2D
{
    public abstract class DOShakeBase : DOBase
    {
        [Tooltip("If TRUE, tween will smoothly reach back to start Position/Rotation/Scale")]
        public bool fadeOut = true;

        [Tooltip("Indicates how much will the shake vibrate")]
        public int vibrato = 10;

        [Tooltip("Indicates how much the shake will be random (values higher than 90 kind of suck, so beware) " +
                 "Setting it to 0 will shake along a single direction.")]
        [Range(0f, 180f)]
        public float randomness = 90f;

        [Tooltip("Full - Full randomness " + "\n" +
                 "Harmonic - Creates a more balanced randomness that looks more harmonic")]
        public ShakeRandomnessMode randomnessMode;

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

            if (tweenType == Enums.TweenType.Looped)
                tween.SetLoops(loops, loopType);

            tween.SetDelay(delay);
        }

    }
}
