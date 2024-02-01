using UnityEngine;
using DG.Tweening;

namespace DOTweenModular
{
    public sealed class DOPunchAnchorPos : DOPunchBase
    {
        public bool snapping;

        protected override void InitializeTween()
        {
            RectTransform rectTransform = (RectTransform)transform;

            Tween = rectTransform.DOPunchAnchorPos(punch, duration, vibrato, elasticity, snapping);
        }
    }
}