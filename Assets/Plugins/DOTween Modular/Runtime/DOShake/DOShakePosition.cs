using UnityEngine;
using DG.Tweening;

namespace DOTweenModular
{
    public sealed class DOShakePosition : DOShakeBase
    {
        #region Properties

        public Vector3 strength = new Vector3(10f, 10f, 10f);
        public bool snapping;

        #endregion

        protected override void InitializeTween()
        {
            Tween = transform.DOShakePosition(duration, strength, vibrato, randomness, 
                                              snapping, fadeout, randomnessMode);
        }
    }
}