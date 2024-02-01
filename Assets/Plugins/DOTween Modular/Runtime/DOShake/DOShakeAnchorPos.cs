using UnityEngine;
using DG.Tweening;

namespace DOTweenModular
{
    public sealed class DOShakeAnchorPos : DOShakeBase
    {
        public bool snapping;

        protected override void InitializeTween()
        {
            RectTransform rectTransform = (RectTransform)transform;

            Tween = rectTransform.DOShakeAnchorPos(duration, strength, vibrato, randomness, 
                                                   snapping, fadeout, randomnessMode);
        }
    }
}