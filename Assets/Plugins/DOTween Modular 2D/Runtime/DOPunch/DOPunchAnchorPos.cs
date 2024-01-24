using DG.Tweening;
using UnityEngine;

namespace DOTweenModular2D
{
    [AddComponentMenu("DOTween Modular 2D/UI/DO Punch Anchor Pos")]
    public class DOPunchAnchorPos : DOPunchPosition
    {
        protected override void InitializeTween()
        {
            RectTransform rectTransform = (RectTransform)transform;
            tween = rectTransform.DOPunchAnchorPos(punchAmount, duration, vibrato, elasticity, snapping);
        }
    }
}