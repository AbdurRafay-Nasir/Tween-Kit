using UnityEngine;
using DG.Tweening;

namespace DOTweenModular
{
    [AddComponentMenu("DOTween Modular/DO Shake Anchor Pos")]
    public sealed class DOShakeAnchorPos : DOShakeBase
    {
        [Tooltip("If TRUE the tween will smoothly snap all values to integers")]
        public bool snapping;

        protected override void InitializeTween()
        {
            RectTransform rectTransform = (RectTransform)transform;

            Tween = rectTransform.DOShakeAnchorPos(duration, strength, vibrato, randomness, 
                                                   snapping, fadeout, randomnessMode);
        }
    }
}