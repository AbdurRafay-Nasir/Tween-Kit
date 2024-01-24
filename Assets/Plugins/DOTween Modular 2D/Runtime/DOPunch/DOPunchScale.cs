using DG.Tweening;
using UnityEngine;

namespace DOTweenModular2D
{
    [AddComponentMenu("DOTween Modular 2D/Transform/DO Punch/DO Punch Scale")]
    public class DOPunchScale : DOPunchBase
    {
        public Vector2 punchAmount;

        protected override void InitializeTween()
        {
            tween = transform.DOPunchScale(punchAmount, duration, vibrato, elasticity);
        }
    }
}