using UnityEngine;
using DG.Tweening;

namespace DOTweenModular
{
    [AddComponentMenu("DOTween Modular/DO Scale Childs")]
    public sealed class DOScaleChilds : DOBase
    {
        #region Properties

        [Tooltip("If TRUE, All childs will move simultaneously")]
        public bool join = true;

        [Tooltip("If TRUE the childs will Scale relative to their current Local Scale" + "\n" +
                 "Target Scale for each child will be calculated as: " + "\n" +
                 "Target Scale = Target Scale + child Local Scale")]
        public bool relative;

        [Tooltip("The scale to reach")]
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