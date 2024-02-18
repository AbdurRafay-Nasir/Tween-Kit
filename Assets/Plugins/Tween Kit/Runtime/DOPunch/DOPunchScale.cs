using DG.Tweening;

namespace DOTweenModular
{
    [UnityEngine.AddComponentMenu("DOTween Modular/DO Punch Scale")]
    public sealed class DOPunchScale : DOPunchBase
    {
        protected override void InitializeTween()
        {
            Tween = transform.DOPunchScale(punch, duration, vibrato, elasticity);
        }
    }
}