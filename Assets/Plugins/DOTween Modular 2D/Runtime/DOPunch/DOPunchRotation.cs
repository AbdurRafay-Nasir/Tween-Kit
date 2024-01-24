using DG.Tweening;
using UnityEngine;

namespace DOTweenModular2D
{
    [AddComponentMenu("DOTween Modular 2D/Transform/DO Punch/DO Punch Rotation")]
    public class DOPunchRotation : DOPunchBase
    {
        public float punchAmount;

        protected override void InitializeTween()
        {
            tween = transform.DOPunchRotation(Vector3.forward * punchAmount, duration, vibrato, elasticity);
        }

    }

}