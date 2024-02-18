using UnityEngine;
using DG.Tweening;

namespace TweenKit
{
    public abstract class DOChildsBase : DOBase
    {
        [Tooltip("If TRUE, All childs will be Tweened simultaneously")]
        public bool join = true;

        protected abstract Tween InitializeChildTween(Transform currentChild);

        public override Tween CreateTween()
        {
            Sequence childsSequence = DOTween.Sequence();

            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);

                Tween childTween = InitializeChildTween(child);

                if (easeType == Ease.INTERNAL_Custom)
                    childTween.SetEase(curve);
                else
                    childTween.SetEase(easeType);

                childTween.SetDelay(delay);

                if (join)
                    childsSequence.Join(childTween);
                else
                    childsSequence.Append(childTween);
            }

            if (tweenType == Enums.TweenType.Looped)
                childsSequence.SetLoops(loops, loopType);

            Tween = childsSequence;

            TweenCreated();

            return Tween;
        }
    }
}