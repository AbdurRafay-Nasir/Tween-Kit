using UnityEngine;
using DG.Tweening;

namespace DOTweenModular
{
    [AddComponentMenu("Tween Kit/DO Punch Anchor Pos")]
    public sealed class DOPunchAnchorPos : DOPunchBase
    {
        [Tooltip("If TRUE the tween will smoothly snap all values to integers")]
        public bool snapping;

        protected override void InitializeTween()
        {
            RectTransform rectTransform = (RectTransform)transform;

            Tween = rectTransform.DOPunchAnchorPos(punch, duration, vibrato, elasticity, snapping);
        }
    }
}