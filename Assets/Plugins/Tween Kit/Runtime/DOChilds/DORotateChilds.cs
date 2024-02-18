using UnityEngine;
using DG.Tweening;

namespace DOTweenModular
{
    [AddComponentMenu("Tween Kit/DO Rotate Childs")]
    public sealed class DORotateChilds : DOChildsBase
    {
        #region Properties

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

        protected override Tween InitializeChildTween(Transform currentChild)
        {
            Tween childRotateTween;

            if (useLocal)
                childRotateTween = currentChild.DORotate(targetRotation, duration, rotateMode);
            else
                childRotateTween = currentChild.DOLocalRotate(targetRotation, duration, rotateMode);

            return childRotateTween;
        }
    }
}