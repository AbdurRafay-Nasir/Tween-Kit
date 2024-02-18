using UnityEngine;
using DG.Tweening;

namespace TweenKit
{
    [AddComponentMenu("Tween Kit/DO Move Childs")]
    public sealed class DOMoveChilds : DOChildsBase
    {
        #region Properties

        [Tooltip("If TRUE the childs will move relative to their current Position in world space" + "\n" + "\n" +
                 "Target Position will be calculated as: " + "\n" +
                 "Target Position = Target Position + parent current position (in world space)")]
        public bool relative;

        [Tooltip("If TRUE, the tween will smoothly snap all values to integers")]
        public bool snapping;

        [Tooltip("The position to reach")]
        public Vector3 targetPosition;

        #endregion

        protected override Tween InitializeChildTween(Transform currentChild)
        {
            return currentChild.DOMove(targetPosition, duration, snapping)
                               .SetRelative(relative);
        }
    }
}