using DG.Tweening;

namespace DOTweenModular
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