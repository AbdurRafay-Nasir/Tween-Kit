using DG.Tweening;

namespace DOTweenModular
{
    public sealed class DOPunchPosition : DOPunchBase
    {
        public bool snapping;

        protected override void InitializeTween()
        {
            Tween = transform.DOPunchPosition(punch, duration, vibrato, elasticity, snapping);
        }
    }
}