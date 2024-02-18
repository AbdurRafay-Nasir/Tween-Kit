using DG.Tweening;

namespace TweenKit
{
    [UnityEngine.AddComponentMenu("Tween Kit/DO Shake Rotation")]
    public sealed class DOShakeRotation : DOShakeBase
    {
        protected override void InitializeTween()
        {
            Tween = transform.DOShakeRotation(duration, strength, vibrato, randomness, fadeout, randomnessMode);
        }
    }
}