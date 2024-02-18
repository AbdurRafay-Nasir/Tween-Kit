using DG.Tweening;

namespace TweenKit
{
    [UnityEngine.AddComponentMenu("Tween Kit/DO Punch Scale")]
    public sealed class DOPunchScale : DOPunchBase
    {
        protected override void InitializeTween()
        {
            Tween = transform.DOPunchScale(punch, duration, vibrato, elasticity);
        }
    }
}