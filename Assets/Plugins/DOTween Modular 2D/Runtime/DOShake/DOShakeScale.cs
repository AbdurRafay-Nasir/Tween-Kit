using DG.Tweening;
using UnityEngine;

namespace DOTweenModular2D
{
    [AddComponentMenu("DOTween Modular 2D/Transform/DO Shake/DO Shake Scale")]
    public class DOShakeScale : DOShakeBase
    {
        public Vector2 strength;

        protected override void InitializeTween()
        {
            tween = transform.DOShakeScale(duration, strength, vibrato, randomness, fadeOut, randomnessMode);
        }
    }
}
