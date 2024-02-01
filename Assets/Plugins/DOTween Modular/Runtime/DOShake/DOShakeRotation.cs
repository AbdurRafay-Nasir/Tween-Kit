using DG.Tweening;

namespace DOTweenModular
{
    public sealed class DOShakeRotation : DOShakeBase
    {
        protected override void InitializeTween()
        {
            Tween = transform.DOShakeRotation(duration, strength, vibrato, randomness, fadeout, randomnessMode);
        }
    }
}