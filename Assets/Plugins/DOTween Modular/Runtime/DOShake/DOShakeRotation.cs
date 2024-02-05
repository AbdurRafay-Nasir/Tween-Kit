using DG.Tweening;

namespace DOTweenModular
{
    [UnityEngine.AddComponentMenu("DOTween Modular/DO Shake Rotation")]
    public sealed class DOShakeRotation : DOShakeBase
    {
        protected override void InitializeTween()
        {
            Tween = transform.DOShakeRotation(duration, strength, vibrato, randomness, fadeout, randomnessMode);
        }
    }
}