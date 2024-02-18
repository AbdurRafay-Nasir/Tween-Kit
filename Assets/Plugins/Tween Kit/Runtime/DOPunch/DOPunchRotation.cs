using DG.Tweening;

namespace TweenKit
{
    [UnityEngine.AddComponentMenu("Tween Kit/DO Punch Rotation")]
    public sealed class DOPunchRotation : DOPunchBase
    {
        protected override void InitializeTween()
        {
            Tween = transform.DOPunchRotation(punch, duration, vibrato, elasticity);
        }
    }
}