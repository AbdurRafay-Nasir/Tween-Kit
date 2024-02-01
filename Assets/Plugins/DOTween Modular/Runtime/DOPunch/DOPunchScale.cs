using DG.Tweening;

namespace DOTweenModular
{
    public sealed class DOPunchScale : DOPunchBase
    {
        protected override void InitializeTween()
        {
            Tween = transform.DOPunchScale(punch, duration, vibrato, elasticity);
        }
    }
}