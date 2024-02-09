using UnityEngine;
using DG.Tweening;

namespace DOTweenModular
{
    [AddComponentMenu("DOTween Modular/DO Rotate Childs")]
    public sealed class DORotateChilds : DOBase
    {
        #region Properties

        public bool join;
        public RotateMode rotateMode;
        public bool useLocal;
        public Vector3 targetRotation = new Vector3(90f, 90f, 90f);

        #endregion

        public override Tween CreateTween()
        {
            Sequence childsRotationSequence = DOTween.Sequence();

            for (int i = 0; i < transform.childCount; i++)
            {
                Tween childRotateTween;

                if (useLocal)
                    childRotateTween = transform.GetChild(i).DORotate(targetRotation, duration, rotateMode);
                else
                    childRotateTween = transform.GetChild(i).DOLocalRotate(targetRotation, duration, rotateMode);

                if (easeType == Ease.INTERNAL_Custom)
                    childRotateTween.SetEase(curve);
                else
                    childRotateTween.SetEase(easeType);

                childRotateTween.SetDelay(delay);

                if (join)
                    childsRotationSequence.Join(childRotateTween);
                else
                    childsRotationSequence.Append(childRotateTween);
            }

            if (tweenType == Enums.TweenType.Looped)
                childsRotationSequence.SetLoops(loops, loopType);

            Tween = childsRotationSequence;

            TweenCreated();

            return Tween;
        }
    }
}