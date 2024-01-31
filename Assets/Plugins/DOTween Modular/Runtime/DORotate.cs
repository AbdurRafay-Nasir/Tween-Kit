using UnityEngine;
using DG.Tweening;

namespace DOTweenModular
{
    public sealed class DORotate : DOBase
    {
        [Tooltip("Fast - Fastest way that never rotates beyond 360° " + "\n" + "\n" +
         "FastBeyond360 - Fastest way that rotates beyond 360°, use this for full 360° rotation " + "\n" + "\n" +
         "WorldAxisAdd - Adds the given rotation to the transform using world axis and an advanced precision mode (like when using transform.Rotate(Space.World))" + "\n" +
         "In this mode the end value is is always considered relative" + "\n" + "\n" +
         "LocalAxisAdd - Adds the given rotation to the transform's local axis (like when rotating an" + "\n" +
         "object with the local switch enabled in Unity's editor or using transform.Rotate(Space.Self))" + "\n" +
         "In this mode the end value is is always considered relative")]
        public RotateMode rotateMode;

        [Tooltip("If TRUE, game object will rotate in local space")]
        public bool useLocal;

        [Tooltip("If TRUE, the Tween will Move duration amount in each second")]
        public bool speedBased;

        [Tooltip("The rotation to reach, if relative is true game object will rotate as: " + "\n" +
                 "targetZRotation = targetZRotation + transform.rotation")]
        public Vector3 targetRotation = new Vector3(90f, 90f, 90f);

        public override Tween CreateTween()
        {
            if (useLocal)
            {
                Tween = transform.DOLocalRotate(targetRotation, duration, rotateMode);
            }
            else
            {
                Tween = transform.DORotate(targetRotation, duration, rotateMode);
            }

            if (easeType == Ease.INTERNAL_Custom)
                Tween.SetEase(curve);
            else
                Tween.SetEase(easeType);

            if (tweenType == Enums.TweenType.Looped)
                Tween.SetLoops(loops, loopType);

            Tween.SetSpeedBased(speedBased);
            Tween.SetDelay(delay);

            TweenCreated();

            return Tween;
        }
    }
}
