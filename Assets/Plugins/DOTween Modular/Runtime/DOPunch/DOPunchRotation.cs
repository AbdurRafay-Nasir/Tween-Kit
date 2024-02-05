using DG.Tweening;

namespace DOTweenModular
{
    [UnityEngine.AddComponentMenu("DOTween Modular/DO Punch Rotation")]
    public sealed class DOPunchRotation : DOPunchBase
    {
        protected override void InitializeTween()
        {
            Tween = transform.DOPunchRotation(punch, duration, vibrato, elasticity);
        }
    }
}