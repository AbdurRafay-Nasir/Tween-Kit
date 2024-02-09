using UnityEngine;
using DG.Tweening;

namespace DOTweenModular
{
    [AddComponentMenu("DOTween Modular/DO Rotate Childs")]
    public sealed class DORotateChilds : DOBase
    {
        #region Properties

        [Tooltip("If TRUE, All childs will move simultaneously")]
        public bool join = true;

        [Tooltip("Fast - Fastest way that never rotates beyond 360° " + "\n" + "\n" +
                 "FastBeyond360 - Fastest way that rotates beyond 360°, use this for full 360° rotation " + "\n" + "\n" +
                 "WorldAxisAdd - Adds the given rotation to the transform using world axis and an advanced precision mode (like when using transform.Rotate(Space.World))" + "\n" +
                 "In this mode the end value is is always considered relative" + "\n" + "\n" +
                 "LocalAxisAdd - Adds the given rotation to the transform's local axis (like when rotating an" + "\n" +
                 "object with the local switch enabled in Unity's editor or using transform.Rotate(Space.Self))" + "\n" +
                 "In this mode the end value is is always considered relative")]
        public RotateMode rotateMode;

        [Tooltip("If TRUE, Childs will rotate in local space")]
        public bool useLocal;

        [Tooltip("The rotation to reach")]
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