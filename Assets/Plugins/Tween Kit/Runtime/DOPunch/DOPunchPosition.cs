using DG.Tweening;

namespace DOTweenModular
{
    [UnityEngine.AddComponentMenu("DOTween Modular/DO Punch Position")]
    public sealed class DOPunchPosition : DOPunchBase
    {
        [UnityEngine.Tooltip("If TRUE the tween will smoothly snap all values to integers")]
        public bool snapping;

        protected override void InitializeTween()
        {
            Tween = transform.DOPunchPosition(punch, duration, vibrato, elasticity, snapping);
        }
    }
}