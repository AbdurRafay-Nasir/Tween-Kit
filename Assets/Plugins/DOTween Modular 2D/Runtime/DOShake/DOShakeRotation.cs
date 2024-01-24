using DG.Tweening;
using UnityEngine;

namespace DOTweenModular2D
{
    [AddComponentMenu("DOTween Modular 2D/Transform/DO Shake/DO Shake Rotation")]
    public class DOShakeRotation : DOShakeBase
    {
        public float strength;

        protected override void InitializeTween()
        {
            tween = transform.DOShakeRotation(duration, Vector3.forward * strength, vibrato, 
                                              randomness, fadeOut, randomnessMode);
        }
    }
}
