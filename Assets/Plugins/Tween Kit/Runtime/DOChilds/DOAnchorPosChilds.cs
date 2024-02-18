using UnityEngine;
using DG.Tweening;

namespace DOTweenModular
{
    [AddComponentMenu("DOTween Modular/DO Anchor Pos Childs")]
    public sealed class DOAnchorPosChilds : DOChildsBase
    {
        #region Properties

        [Tooltip("If TRUE the childs will move relative to their current Position in world space" + "\n" +
                 "Target Position for each child will be calculated as: " + "\n" +
                 "Target Position = Target Position + child (world space) current position")]
        public bool relative;

        [Tooltip("If TRUE, the tween will smoothly snap all values to integers")]
        public bool snapping;

        [Tooltip("The position to reach")]
        public Vector2 targetPosition;

        #endregion

        protected override Tween InitializeChildTween(Transform currentChild)
        {
            RectTransform childRectTransform = (RectTransform)currentChild;
            
            return childRectTransform.DOAnchorPos(targetPosition, duration, snapping)
                                     .SetRelative(relative);
        }
    }
}