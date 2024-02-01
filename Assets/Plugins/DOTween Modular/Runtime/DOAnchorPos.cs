using UnityEngine;
using DG.Tweening;

namespace DOTweenModular
{
    public sealed class DOAnchorPos : DOBase
    {
        #region Properties

        #endregion

        public override Tween CreateTween()
        {
            

            if (easeType == Ease.INTERNAL_Custom)
                Tween.SetEase(curve);
            else
                Tween.SetEase(easeType);

            if (tweenType == Enums.TweenType.Looped)
                Tween.SetLoops(loops, loopType);

            Tween.SetDelay(delay);

            TweenCreated();

            return Tween;
        }
    }
}