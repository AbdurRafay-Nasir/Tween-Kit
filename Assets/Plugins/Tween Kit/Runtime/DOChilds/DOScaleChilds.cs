using UnityEngine;
using DG.Tweening;

namespace TweenKit
{
    [AddComponentMenu("Tween Kit/DO Scale Childs")]
    public sealed class DOScaleChilds : DOChildsBase
    {
        #region Properties

        [Tooltip("If TRUE the childs will Scale relative to their current Local Scale" + "\n" +
                 "Target Scale for each child will be calculated as: " + "\n" +
                 "Target Scale = Target Scale + child Local Scale")]
        public bool relative;

        [Tooltip("The scale to reach")]
        public Vector3 targetScale = new(3f, 3f, 3f);

        #endregion

        protected override Tween InitializeChildTween(Transform currentChild)
        {
            return currentChild.DOScale(targetScale, duration)
                               .SetRelative(relative);            
        }
    }
}