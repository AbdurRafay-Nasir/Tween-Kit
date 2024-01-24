using DG.Tweening;
using UnityEngine;

namespace DOTweenModular2D
{
    [AddComponentMenu("DOTween Modular 2D/Transform/DO Rotate", 60)]
    public class DORotate : DOBase
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

        [Tooltip("If TRUE, the tween will Move duration amount in each second")]
        public bool speedBased;

        [Tooltip("If TRUE, the targetZRotation will be calculated as: " + "\n" +
                 "targetZRotation = targetZRotation + transform.rotation")]
        public bool relative;

        [Tooltip("The rotation to reach, if relative is true game object will rotate as: " + "\n" +
                 "targetZRotation = targetZRotation + transform.rotation")]
        [Range(-360f, 360f)]
        public float targetZRotation = 90;
    
        public override void CreateTween()
        {
            if (useLocal)
            {
                tween = transform.DOLocalRotate(Vector3.forward * targetZRotation, duration, rotateMode);
            }            
            else
            {
                tween = transform.DORotate(Vector3.forward * targetZRotation, duration, rotateMode);
            }
        
            if (easeType == Ease.INTERNAL_Custom)
                tween.SetEase(curve);
            else
                tween.SetEase(easeType);

            if (tweenType == Enums.TweenType.Looped)
                tween.SetLoops(loops, loopType);

            tween.SetSpeedBased(speedBased);
            tween.SetRelative(relative);
            tween.SetDelay(delay);

            InvokeTweenCreated();
        }
    }

}