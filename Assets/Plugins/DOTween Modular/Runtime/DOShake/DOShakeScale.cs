using DG.Tweening;

namespace DOTweenModular
{
    public sealed class DOShakeScale : DOShakeBase
    {
        protected override void InitializeTween()
        {
            Tween = transform.DOShakeScale(duration, strength, vibrato, randomness, fadeout, randomnessMode);
        }
    }
}