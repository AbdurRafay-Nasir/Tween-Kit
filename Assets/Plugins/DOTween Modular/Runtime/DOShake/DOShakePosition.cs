using UnityEngine;
using DG.Tweening;

namespace DOTweenModular
{
    public sealed class DOShakePosition : DOShakeBase
    {
        public bool snapping;

        protected override void InitializeTween()
        {
            Tween = transform.DOShakePosition(duration, strength, vibrato, randomness, 
                                              snapping, fadeout, randomnessMode);
        }
    }
}