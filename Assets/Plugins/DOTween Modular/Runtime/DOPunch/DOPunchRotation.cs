using DG.Tweening;

namespace DOTweenModular
{
    public sealed class DOPunchRotation : DOPunchBase
    {
        protected override void InitializeTween()
        {
            Tween = transform.DOPunchRotation(punch, duration, vibrato, elasticity);
        }
    }
}