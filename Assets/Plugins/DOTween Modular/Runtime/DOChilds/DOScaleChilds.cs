using UnityEngine;
using DG.Tweening;

namespace DOTweenModular
{
    [AddComponentMenu("DOTween Modular/DO Scale Childs")]
    public sealed class DOScaleChilds : DOBase
    {
        #region Properties

        public bool join;
        public bool relative;
        public Vector3 targetScale = new Vector3(3f, 3f, 3f);

        #endregion

        public override Tween CreateTween()
        {
            Sequence childsScaleSequence = DOTween.Sequence();

            for (int i = 0; i < transform.childCount; i++)
            {
                Tween childMoveTween = transform.GetChild(i).DOScale(targetScale, duration);

                if (easeType == Ease.INTERNAL_Custom)
                    childMoveTween.SetEase(curve);
                else
                    childMoveTween.SetEase(easeType);

                childMoveTween.SetRelative(relative);
                childMoveTween.SetDelay(delay);

                if (join)
                    childsScaleSequence.Join(childMoveTween);
                else
                    childsScaleSequence.Append(childMoveTween);
            }

            if (tweenType == Enums.TweenType.Looped)
                childsScaleSequence.SetLoops(loops, loopType);

            Tween = childsScaleSequence;

            TweenCreated();

            return Tween;
        }
    }
}