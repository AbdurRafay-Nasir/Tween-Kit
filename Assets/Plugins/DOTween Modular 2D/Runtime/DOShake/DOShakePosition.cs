using DG.Tweening;
using UnityEngine;

namespace DOTweenModular2D
{
    [AddComponentMenu("DOTween Modular 2D/Transform/DO Shake/DO Shake Position")]
    public class DOShakePosition : DOShakeBase
    {
        public Vector2 strength;
        public bool snapping;

        protected override void InitializeTween()
        {
            tween = transform.DOShakePosition(duration, strength, vibrato, randomness, 
                                              snapping, fadeOut, randomnessMode);
        }
    }
}

