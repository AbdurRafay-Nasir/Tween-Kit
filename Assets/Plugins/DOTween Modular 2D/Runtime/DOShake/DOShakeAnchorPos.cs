using DG.Tweening;
using UnityEngine;

namespace DOTweenModular2D
{
    [AddComponentMenu("DOTween Modular 2D/UI/DO Shake Anchor Pos")]
    public class DOShakeAnchorPos : DOShakePosition
    {
        protected override void InitializeTween()
        {
            RectTransform rectTransform = (RectTransform)transform;

            tween = rectTransform.DOShakeAnchorPos(duration, strength, vibrato, randomness, 
                                                   snapping, fadeOut, randomnessMode);
        }
    }
}
